using System;
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
        V1TenantEntityController<V1Client, V1Client.SpecDef, V1Client.StatusDef, ClientConf>,
        IEntityController<V1Client>
    {
        readonly IMemoryCache _clientCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public V1ClientController(IKubernetesClient kube, EntityRequeue<V1Client> requeue, IMemoryCache cache,
            ILogger<V1ClientController> logger, IOptions<OperatorOptions> options) :
            base(kube, requeue, cache, logger, options)
        {
            _clientCache = cache;
        }

        /// <inheritdoc />
        protected override string EntityTypeName => "A0Client";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace,
            CancellationToken cancellationToken)
        {
            try
            {
                return TransformToSystemTextJson<Hashtable>(await api.Clients.GetAsync(id,
                    cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error retrieving {EntityTypeName} with ID {Id}: {Message}", EntityTypeName, id,
                    e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Client entity, V1Client.SpecDef spec,
            string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                // Log the beginning of find operation with all criteria
                Logger.LogInformation(
                    "{EntityTypeName} {EntityNamespace}/{EntityName} starting client lookup with find criteria: ClientId={ClientId}, CallbackUrls=[{CallbackUrls}], MatchMode={MatchMode}",
                    EntityTypeName, entity.Namespace(), entity.Name(),
                    spec.Find.ClientId ?? "null",
                    spec.Find.CallbackUrls != null ? string.Join(", ", spec.Find.CallbackUrls) : "null",
                    spec.Find.CallbackUrlMatchMode ?? "strict");

                if (spec.Find.ClientId is string clientId)
                {
                    Logger.LogDebug(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} initiating client_id lookup: {ClientId}",
                        EntityTypeName, entity.Namespace(), entity.Name(), clientId);

                    try
                    {
                        var client = await api.Clients.GetAsync(clientId, "client_id,name",
                            cancellationToken: cancellationToken);
                        Logger.LogInformation(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} client_id lookup SUCCESSFUL - found existing client: {Name} (ClientId: {ClientId})",
                            EntityTypeName, entity.Namespace(), entity.Name(), client.Name, client.ClientId);
                        return client.ClientId;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogWarning(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} client_id lookup FAILED - could not find client with id {ClientId}",
                            EntityTypeName, entity.Namespace(), entity.Name(), clientId);
                        return null;
                    }
                }

                if (spec.Find.CallbackUrls is { Length: > 0 } callbackUrls)
                {
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} initiating callback URL lookup: URLs=[{CallbackUrls}], Mode={MatchMode}",
                        EntityTypeName, entity.Namespace(), entity.Name(),
                        string.Join(", ", callbackUrls),
                        spec.Find.CallbackUrlMatchMode ?? "strict");

                    var result = await FindByCallbackUrls(api, entity, callbackUrls, spec.Find.CallbackUrlMatchMode,
                        cancellationToken);

                    if (result != null)
                    {
                        Logger.LogInformation(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} callback URL lookup SUCCESSFUL - found client with id: {ClientId}",
                            EntityTypeName, entity.Namespace(), entity.Name(), result);
                    }
                    else
                    {
                        Logger.LogInformation(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} callback URL lookup FAILED - no matching client found",
                            EntityTypeName, entity.Namespace(), entity.Name());
                    }

                    return result;
                }

                Logger.LogInformation(
                    "{EntityTypeName} {EntityNamespace}/{EntityName} find operation completed - no valid lookup criteria provided",
                    EntityTypeName, entity.Namespace(), entity.Name());
                return null;
            }
            else
            {
                Logger.LogDebug(
                    "{EntityTypeName} {EntityNamespace}/{EntityName} no find criteria specified - falling back to name-based lookup",
                    EntityTypeName, entity.Namespace(), entity.Name());

                var conf = spec.Init ?? spec.Conf;
                if (conf is null)
                    return null;

                Logger.LogDebug(
                    "{EntityTypeName} {EntityNamespace}/{EntityName} initiating name-based lookup for client: {ClientName}",
                    EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);

                var list = await api.Clients.GetAllAsync(new GetClientsRequest() { Fields = "client_id,name" },
                    cancellationToken: cancellationToken);
                var self = list.FirstOrDefault(i => i.Name == conf.Name);

                if (self != null)
                {
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} name-based lookup SUCCESSFUL - found client: {ClientName} (ClientId: {ClientId})",
                        EntityTypeName, entity.Namespace(), entity.Name(), conf.Name, self.ClientId);
                }
                else
                {
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} name-based lookup FAILED - no client found with name: {ClientName}",
                        EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);
                }

                return self?.ClientId;
            }
        }

        /// <summary>
        /// Finds an Auth0 client by callback URLs using loose or strict matching mode.
        /// </summary>
        /// <param name="api">Auth0 Management API client</param>
        /// <param name="entity">Kubernetes entity for logging</param>
        /// <param name="targetCallbackUrls">Array of callback URLs to match</param>
        /// <param name="matchMode">Matching mode: "loose" (partial match) or "strict" (all URLs must match)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Client ID if found, null otherwise</returns>
        private async Task<string?> FindByCallbackUrls(IManagementApiClient api, V1Client entity,
            string[] targetCallbackUrls, string? matchMode, CancellationToken cancellationToken)
        {
            // Validate callback URLs
            foreach (var url in targetCallbackUrls)
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                {
                    Logger.LogWarning(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} invalid callback URL format: {CallbackUrl}",
                        EntityTypeName, entity.Namespace(), entity.Name(), url);
                    return null;
                }
            }

            var isStrictMode = !string.Equals(matchMode, "loose", StringComparison.OrdinalIgnoreCase);
            var modeName = isStrictMode ? "strict" : "loose";

            Logger.LogDebug(
                "{EntityTypeName} {EntityNamespace}/{EntityName} executing callback URL search with {Mode} mode matching against Auth0 clients",
                EntityTypeName, entity.Namespace(), entity.Name(), modeName);

            var clients = await GetClientsForCallbackSearch(api, cancellationToken);

            var matchingClients = clients
                .Where(client => HasMatchingCallbackUrls(client, targetCallbackUrls, isStrictMode)).ToList();

            if (matchingClients.Count == 0)
            {
                Logger.LogDebug(
                    "{EntityTypeName} {EntityNamespace}/{EntityName} no clients matched callback URL criteria ({Mode} mode)",
                    EntityTypeName, entity.Namespace(), entity.Name(), modeName);
                return null;
            }

            if (matchingClients.Count > 1)
            {
                Logger.LogWarning(
                    "{EntityTypeName} {EntityNamespace}/{EntityName} found multiple clients ({Count}) matching callback URL criteria ({Mode} mode). Using first match: {ClientName} ({ClientId})",
                    EntityTypeName, entity.Namespace(), entity.Name(), matchingClients.Count, modeName,
                    matchingClients[0].Name, matchingClients[0].ClientId);
            }

            var selectedClient = matchingClients[0];
            Logger.LogDebug(
                "{EntityTypeName} {EntityNamespace}/{EntityName} selected client for callback URL match ({Mode} mode): {Name} ({ClientId})",
                EntityTypeName, entity.Namespace(), entity.Name(), modeName, selectedClient.Name,
                selectedClient.ClientId);

            return selectedClient.ClientId;
        }

        /// <summary>
        /// Gets the list of Auth0 clients with caching for callback URL searches.
        /// </summary>
        /// <param name="api">Auth0 Management API client</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of clients with callback-related fields</returns>
        private async Task<IList<Client>> GetClientsForCallbackSearch(IManagementApiClient api,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"auth0_clients_callback_search_{api.GetHashCode()}";

            if (_clientCache.TryGetValue(cacheKey, out IList<Client>? cachedClients) && cachedClients != null)
            {
                Logger.LogDebug("Using cached client list for callback URL search");
                return cachedClients;
            }

            Logger.LogDebug("Fetching client list for callback URL search");
            var clients = await api.Clients.GetAllAsync(new GetClientsRequest()
            {
                Fields = "client_id,name,callbacks",
                IncludeFields = true
            }, cancellationToken: cancellationToken);

            // Cache for 30 seconds to avoid repeated API calls during reconciliation
            _clientCache.Set(cacheKey, clients, TimeSpan.FromSeconds(30));

            return clients;
        }

        /// <summary>
        /// Checks if a client has matching callback URLs based on the specified matching mode.
        /// </summary>
        /// <param name="client">Auth0 client to check</param>
        /// <param name="targetUrls">Target callback URLs to match</param>
        /// <param name="isStrictMode">True for strict mode (all URLs must match), false for loose mode (any URL match)</param>
        /// <returns>True if the client matches, false otherwise</returns>
        private static bool HasMatchingCallbackUrls(Client client, string[] targetUrls, bool isStrictMode)
        {
            if (client.Callbacks == null || client.Callbacks.Length == 0)
                return false;

            if (isStrictMode)
            {
                // Strict mode: ALL target URLs must be found in client's callbacks
                return targetUrls.All(targetUrl => client.Callbacks.Contains(targetUrl));
            }
            else
            {
                // Loose mode: AT LEAST ONE target URL must be found in client's callbacks
                return targetUrls.Any(targetUrl => client.Callbacks.Contains(targetUrl));
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
        protected override async Task<string> Create(IManagementApiClient api, ClientConf conf, string defaultNamespace,
            CancellationToken cancellationToken)
        {
            var startTime = DateTimeOffset.UtcNow;
            Logger.LogInformation("{EntityTypeName} creating client in Auth0 with name: {ClientName}", EntityTypeName,
                conf.Name);
            var self = await api.Clients.CreateAsync(TransformToNewtonsoftJson<ClientConf, ClientCreateRequest>(conf),
                cancellationToken);
            var duration = DateTimeOffset.UtcNow - startTime;
            Logger.LogInformation(
                "{EntityTypeName} successfully created client in Auth0 with ID: {ClientId} and name: {ClientName} in {Duration}ms",
                EntityTypeName, self.ClientId, conf.Name, duration.TotalMilliseconds);
            return self.ClientId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, ClientConf conf,
            string defaultNamespace, CancellationToken cancellationToken)
        {
            var startTime = DateTimeOffset.UtcNow;
            Logger.LogInformation(
                "{EntityTypeName} updating client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName,
                id, conf.Name);

            // transform initial request
            var req = TransformToNewtonsoftJson<ClientConf, ClientUpdateRequest>(conf);

            // explicitely null out missing metadata if previously present
            if (last is not null && last.ContainsKey("client_metadata") && conf.ClientMetaData != null)
                foreach (string key in ((Hashtable)last["client_metadata"]!).Keys)
                    if (conf.ClientMetaData.ContainsKey(key) == false)
                        req.ClientMetaData[key] = null;

            await api.Clients.UpdateAsync(id, req, cancellationToken);
            var duration = DateTimeOffset.UtcNow - startTime;
            Logger.LogInformation(
                "{EntityTypeName} successfully updated client in Auth0 with id: {ClientId} and name: {ClientName} in {Duration}ms",
                EntityTypeName, id, conf.Name, duration.TotalMilliseconds);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1Client entity, Hashtable lastConf,
            string defaultNamespace, CancellationToken cancellationToken)
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
        async Task ApplySecret(V1Client entity, string? clientId, string? clientSecret, string defaultNamespace,
            CancellationToken cancellationToken)
        {
            try
            {
                if (entity.Spec.SecretRef is null)
                    return;

                // find existing secret or create
                var secret = await ResolveSecretRef(entity.Spec.SecretRef,
                    entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, cancellationToken);
                if (secret is null)
                {
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName} which does not exist: creating.",
                        EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                    secret = await Kube.CreateAsync(
                        new V1Secret(
                                metadata: new V1ObjectMeta(
                                    namespaceProperty: entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace,
                                    name: entity.Spec.SecretRef.Name))
                            .WithOwnerReference(entity),
                        cancellationToken);
                }

                // only apply actual values if we are the owner
                if (secret.IsOwnedBy(entity))
                {
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName}: updating.",
                        EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                    secret.StringData ??= new Dictionary<string, string>();

                    // Always set clientId if available
                    if (clientId is not null)
                    {
                        secret.StringData["clientId"] = clientId;
                        Logger.LogDebug(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} updated secret {SecretName} with clientId",
                            EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                    }
                    else if (!secret.StringData.ContainsKey("clientId"))
                    {
                        // Initialize empty clientId field if not present and no value available
                        secret.StringData["clientId"] = "";
                        Logger.LogDebug(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} initialized empty clientId in secret {SecretName}",
                            EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                    }

                    // Handle clientSecret - for existing clients, Auth0 API doesn't return the secret
                    if (clientSecret is not null)
                    {
                        secret.StringData["clientSecret"] = clientSecret;
                        Logger.LogDebug(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} updated secret {SecretName} with clientSecret",
                            EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                    }
                    else if (!secret.StringData.ContainsKey("clientSecret"))
                    {
                        // Initialize empty clientSecret field if not present and no value available
                        // Note: For existing clients, Auth0 API doesn't return the secret value for security reasons
                        secret.StringData["clientSecret"] = "";
                        Logger.LogDebug(
                            "{EntityTypeName} {EntityNamespace}/{EntityName} initialized empty clientSecret in secret {SecretName} (Auth0 API does not return secrets for existing clients)",
                            EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                    }

                    secret = await Kube.UpdateAsync(secret, cancellationToken);
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} successfully updated secret {SecretName}",
                        EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }
                else
                {
                    Logger.LogInformation(
                        "{EntityTypeName} {EntityNamespace}/{EntityName} secret {SecretName} exists but is not owned by this client, skipping update",
                        EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e,
                    "Error applying secret for {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}",
                    EntityTypeName, entity.Namespace(), entity.Name(), e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        protected override async Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation(
                "{EntityTypeName} deleting client from Auth0 with ID: {ClientId} (reason: Kubernetes entity deleted)",
                EntityTypeName, id);
            await api.Clients.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted client from Auth0 with ID: {ClientId}",
                EntityTypeName, id);
        }

        /// <inheritdoc />

        protected override DriftDetectionMode GetDriftDetectionMode() => DriftDetectionMode.IncludeSpecificFields;
        
        protected override string[] GetIncludedFields()
        {
            // Only compare these specific fields for drift detection
            return new[]
            {
                "name",
                "app_type", 
                "grant_types",
                "callbacks",
                "allowed_logout_urls",
                "client_metadata"
            };
        }
    }
}