using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Client;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using k8s;
using k8s.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Queue;
using KubeOps.Abstractions.Rbac;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ClientController :
        V1TenantEntityController<V1Client, V1Client.SpecDef, V1Client.StatusDef, ClientConf>,
        IEntityController<V1Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public V1ClientController(IKubernetesClient kube, EntityRequeue<V1Client> requeue, IMemoryCache cache, ILogger<V1ClientController> logger) :
            base(kube, requeue, cache, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Client";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            return TransformToSystemTextJson<Hashtable>(await api.Clients.GetAsync(id, cancellationToken: cancellationToken));
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Client.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.ClientId is string clientId)
                {
                    var list = await api.Clients.GetAllAsync(new GetClientsRequest() { Fields = "client_id" }, cancellationToken: cancellationToken);
                    var self = list.FirstOrDefault(i => i.ClientId == clientId);
                    return self?.ClientId;
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
        protected override string? ValidateCreate(ClientConf conf)
        {
            if (conf.ApplicationType == null)
                return "missing a value for application type";

            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var self = await api.Clients.CreateAsync(TransformToNewtonsoftJson<ClientConf, ClientCreateRequest>(conf), cancellationToken);
            return self.ClientId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await api.Clients.UpdateAsync(id, TransformToNewtonsoftJson<ClientConf, ClientUpdateRequest>(conf), cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1Client entity, Hashtable lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var clientId = (string?)lastConf["client_id"];
            var clientSecret = (string?)lastConf["client_secret"];
            if (string.IsNullOrWhiteSpace(clientSecret) == false)
                await ApplySecret(entity, clientId, clientSecret, defaultNamespace, cancellationToken);

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
                secret.StringData["clientId"] = null;
                secret.StringData["clientSecret"] = null;
                if (clientId is not null)
                    secret.StringData["clientId"] = clientId;
                if (clientSecret is not null)
                    secret.StringData["clientSecret"] = clientSecret;

                secret = await Kube.UpdateAsync(secret, cancellationToken);
            }
        }

        /// <inheritdoc />
        protected override Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.Clients.DeleteAsync(id, cancellationToken);
        }

    }

}
