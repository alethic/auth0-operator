using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Client;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using k8s.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Queue;
using KubeOps.Abstractions.Rbac;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ClientController :
        V1TenantEntityController<V1Client, V1Client.SpecDef, V1Client.StatusDef, V1ClientConf, Hashtable>,
        IEntityController<V1Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ClientController(IKubernetesClient kube, EntityRequeue<V1Client> requeue, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ClientController> logger) :
            base(kube, requeue, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Client";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return TransformToSystemTextJson<Hashtable>(await api.Clients.GetAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Client entity, V1Client.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.ClientId is string clientId)
                {
                    try
                    {
                        var client = await api.Clients.GetAsync(clientId, "client_id,name", cancellationToken: cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing client: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), client.Name);
                        return client.ClientId;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find client with id {ClientId}.", EntityTypeName, entity.Namespace(), entity.Name(), clientId);
                        return null;
                    }
                }

                return null;
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is null)
                    return null;

                var list = await api.Clients.GetAllAsync(new GetClientsRequest() { Fields = "client_id,name" }, cancellationToken: cancellationToken);
                var self = list.FirstOrDefault(i => i.Name == conf.Name);
                return self?.ClientId;
            }
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1ClientConf conf)
        {
            if (conf.ApplicationType == null)
                return "missing a value for application type";

            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating client in Auth0 with name: {ClientName}", EntityTypeName, conf.Name);
            var self = await api.Clients.CreateAsync(TransformToNewtonsoftJson<V1ClientConf, ClientCreateRequest>(conf), cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created client in Auth0 with ID: {ClientId} and name: {ClientName}", EntityTypeName, self.ClientId, conf.Name);
            return self.ClientId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, V1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);

            // transform initial request
            var req = TransformToNewtonsoftJson<V1ClientConf, ClientUpdateRequest>(conf);

            // explicitely null out missing metadata if previously present
            if (last is not null && last.ContainsKey("client_metadata") && conf.ClientMetaData != null)
                foreach (string key in ((Hashtable)last["client_metadata"]!).Keys)
                    if (conf.ClientMetaData.ContainsKey(key) == false)
                        req.ClientMetaData[key] = null;

            await api.Clients.UpdateAsync(id, req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1Client entity, Hashtable lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // Always attempt to apply secret if secretRef is specified, regardless of whether we have the clientSecret value
            // This ensures secret resources are created for existing clients even when Auth0 API doesn't return the secret
            if (entity.Spec.SecretRef is not null)
            {
                var clientId = (string?)lastConf["client_id"];
                var clientSecret = (string?)lastConf["client_secret"];
                await ApplySecret(entity, clientId, clientSecret, defaultNamespace, cancellationToken);
            }

            lastConf.Remove("client_id");
            lastConf.Remove("client_secret");
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <summary>
        /// Applies the client secret.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task ApplySecret(V1Client entity, string? clientId, string? clientSecret, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (entity.Spec.SecretRef is null)
                return;

            // find existing secret or create
            var secret = await ResolveSecretRef(entity.Spec.SecretRef, entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, cancellationToken);
            if (secret is null)
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName} which does not exist: creating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                secret = await Kube.CreateAsync(
                    new V1Secret(
                        metadata: new V1ObjectMeta(namespaceProperty: entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, name: entity.Spec.SecretRef.Name))
                        .WithOwnerReference(entity),
                    cancellationToken);
            }

            // only apply actual values if we are the owner
            if (secret.IsOwnedBy(entity))
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName}: updating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                secret.StringData ??= new Dictionary<string, string>();

                // Always set clientId if available
                if (clientId is not null)
                {
                    secret.StringData["clientId"] = clientId;
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} updated secret {SecretName} with clientId", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }
                else if (!secret.StringData.ContainsKey("clientId"))
                {
                    // Initialize empty clientId field if not present and no value available
                    secret.StringData["clientId"] = "";
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} initialized empty clientId in secret {SecretName}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }

                // Handle clientSecret - for existing clients, Auth0 API doesn't return the secret
                if (clientSecret is not null)
                {
                    secret.StringData["clientSecret"] = clientSecret;
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} updated secret {SecretName} with clientSecret", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }
                else if (!secret.StringData.ContainsKey("clientSecret"))
                {
                    // Initialize empty clientSecret field if not present and no value available
                    // Note: For existing clients, Auth0 API doesn't return the secret value for security reasons
                    secret.StringData["clientSecret"] = "";
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} initialized empty clientSecret in secret {SecretName} (Auth0 API does not return secrets for existing clients)", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }

                secret = await Kube.UpdateAsync(secret, cancellationToken);
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} successfully updated secret {SecretName}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
            }
            else
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} secret {SecretName} exists but is not owned by this client, skipping update", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
            }
        }

        /// <inheritdoc />
        protected override async Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting client from Auth0 with ID: {ClientId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Clients.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted client from Auth0 with ID: {ClientId}", EntityTypeName, id);
        }

    }

}
