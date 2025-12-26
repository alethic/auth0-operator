using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi;

using k8s;
using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public abstract class ControllerBase<TEntity, TSpec, TStatus, TConf, TLastConf> : IEntityController<TEntity>
        where TEntity : IKubernetesObject<V1ObjectMeta>, ApiEntity<TSpec, TStatus, TConf, TLastConf>
        where TSpec : ApiEntitySpec<TConf>
        where TStatus : ApiEntityStatus<TLastConf>
        where TConf : class
        where TLastConf : class
    {

        static readonly Newtonsoft.Json.JsonSerializer _newtonsoftJsonSerializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
        static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { Converters = { new SimplePrimitiveHashtableConverter() } };

        readonly IKubernetesClient _kube;
        readonly IMemoryCache _cache;
        readonly IOptions<OperatorOptions> _options;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public ControllerBase(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _kube = kube ?? throw new ArgumentNullException(nameof(kube));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the type name of the entity used in messages.
        /// </summary>
        protected abstract string EntityTypeName { get; }

        /// <summary>
        /// Gets the Kubernetes API client.
        /// </summary>
        protected IKubernetesClient Kube => _kube;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger => _logger;

        /// <summary>
        /// Gets operator options.
        /// </summary>
        protected OperatorOptions Options => _options.Value;

        /// <summary>
        /// Attempts to resolve the secret document referenced by the secret reference.
        /// </summary>
        /// <param name="secretRef"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<V1Secret?> ResolveSecretRef(V1SecretReference? secretRef, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (secretRef is null)
                return null;

            if (string.IsNullOrWhiteSpace(secretRef.Name))
                throw new InvalidOperationException($"Secret reference {secretRef} has no name.");

            var ns = secretRef.NamespaceProperty ?? defaultNamespace;
            if (string.IsNullOrWhiteSpace(ns))
                throw new InvalidOperationException($"Secret reference {secretRef} has no discovered namesace.");

            var secret = await _kube.GetAsync<V1Secret>(secretRef.Name, ns, cancellationToken);
            if (secret is null)
                return null;

            return secret;
        }

        /// <summary>
        /// Attempts to resolve the tenant document referenced by the tenant reference.
        /// </summary>
        /// <param name="tenantRef"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<V1Tenant?> ResolveV1TenantRef(V1TenantReference? tenantRef, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (tenantRef is null)
                return null;

            if (string.IsNullOrWhiteSpace(tenantRef.Name))
                throw new InvalidOperationException($"Tenant reference {tenantRef} has no name.");

            var ns = tenantRef.Namespace ?? defaultNamespace;
            if (string.IsNullOrWhiteSpace(ns))
                throw new InvalidOperationException($"Tenant reference {tenantRef} has no discovered namesace.");

            var tenant = await _kube.GetAsync<V1Tenant>(tenantRef.Name, ns, cancellationToken);
            if (tenant is null)
                throw new RetryException($"Tenant reference {tenantRef} cannot be resolved.");

            return tenant;
        }

        /// <summary>
        /// Attempts to resolve the V2 tenant document referenced by the tenant reference.
        /// </summary>
        /// <param name="tenantRef"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<V2alpha1Tenant?> ResolveV2alpha1TenantRef(V1TenantReference? tenantRef, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (tenantRef is null)
                return null;

            if (string.IsNullOrWhiteSpace(tenantRef.Name))
                throw new InvalidOperationException($"Tenant reference {tenantRef} has no name.");

            var ns = tenantRef.Namespace ?? defaultNamespace;
            if (string.IsNullOrWhiteSpace(ns))
                throw new InvalidOperationException($"Tenant reference {tenantRef} has no discovered namesace.");

            var tenant = await _kube.GetAsync<V2alpha1Tenant>(tenantRef.Name, ns, cancellationToken);
            if (tenant is null)
                throw new RetryException($"Tenant reference {tenantRef} cannot be resolved.");

            return tenant;
        }

        /// <summary>
        /// Attempts to resolve the client document referenced by the client reference.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="clientRef"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<V1Client?> ResolveClientRef(IManagementApiClient api, V1ClientReference? clientRef, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (clientRef is null)
                return null;

            if (string.IsNullOrWhiteSpace(clientRef.Name))
                throw new InvalidOperationException($"Client reference has no name.");

            var ns = clientRef.Namespace ?? defaultNamespace;
            if (string.IsNullOrWhiteSpace(ns))
                throw new InvalidOperationException($"Client reference has no discovered namesace.");

            var client = await _kube.GetAsync<V1Client>(clientRef.Name, ns, cancellationToken);
            if (client is null)
                throw new RetryException($"Client reference cannot be resolved.");

            return client;
        }

        /// <summary>
        /// Attempts to resolve the client reference to client ID.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="clientRef"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<string?> ResolveClientRefToId(IManagementApiClient api, V1ClientReference? clientRef, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (clientRef is null)
                return null;

            if (clientRef.Id is { } id && string.IsNullOrWhiteSpace(id) == false)
                return id;

            Logger.LogDebug("Attempting to resolve ClientRef {Namespace}/{Name}.", clientRef.Namespace, clientRef.Name);

            var client = await ResolveClientRef(api, clientRef, defaultNamespace, cancellationToken);
            if (client is null)
                throw new RetryException($"Could not resolve ClientRef {clientRef}.");
            if (string.IsNullOrWhiteSpace(client.Status.Id))
                throw new RetryException($"Referenced Client {client.Namespace()}/{client.Name()} has not been reconciled.");

            Logger.LogDebug("Resolved ClientRef {Namespace}/{Name} to {Id}.", clientRef.Namespace, clientRef.Name, client.Status.Id);
            return client.Status.Id;
        }

        /// <summary>
        /// Attempts to resolve the resource server document referenced by the resource server reference.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="resourceServerRef"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<V1ResourceServer?> ResolveResourceServerRef(IManagementApiClient api, V1ResourceServerReference? resourceServerRef, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (resourceServerRef is null)
                return null;

            var ns = resourceServerRef.Namespace ?? defaultNamespace;
            if (string.IsNullOrWhiteSpace(ns))
                throw new InvalidOperationException($"ResourceServer reference has no namespace.");

            if (string.IsNullOrWhiteSpace(resourceServerRef.Name))
                throw new InvalidOperationException($"ResourceServer reference has no name.");

            var resourceServer = await _kube.GetAsync<V1ResourceServer>(resourceServerRef.Name, ns, cancellationToken);
            if (resourceServer is null)
                throw new RetryException($"ResourceServer reference cannot be resolved.");

            return resourceServer;
        }

        /// <summary>
        /// Attempts to resolve the list of client references to client IDs.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="reference"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<string?> ResolveResourceServerRefToIdentifier(IManagementApiClient api, V1ResourceServerReference? reference, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (reference is null)
                return null;

            // identifier is specified directly by reference
            if (reference.Identifier is { } identifier && string.IsNullOrWhiteSpace(identifier) == false)
                return identifier;

            // id is specified by reference, lookup identifier
            if (reference.Id is { } id && string.IsNullOrWhiteSpace(id) == false)
            {
                var self = await api.ResourceServers.GetAsync(id, cancellationToken);
                if (self is null)
                    throw new RetryException($"Failed to resolve ResourceServer reference {id}.");

                return self.Identifier;
            }

            Logger.LogDebug("Attempting to resolve ResourceServer reference {Namespace}/{Name}.", reference.Namespace, reference.Name);

            var resourceServer = await ResolveResourceServerRef(api, reference, defaultNamespace, cancellationToken);
            if (resourceServer is null)
                throw new RetryException($"Could not resolve ResourceServerRef {reference}.");

            if (resourceServer.Status.Identifier is null)
                throw new RetryException($"Referenced ResourceServer {resourceServer.Namespace()}/{resourceServer.Name()} has not been reconcilled.");

            Logger.LogDebug("Resolved ResourceServer reference {Namespace}/{Name} to {Identifier}.", reference.Namespace, reference.Name, resourceServer.Status.Identifier);
            return resourceServer.Status.Identifier;
        }

        /// <summary>
        /// Gets an active <see cref="ManagementApiClient"/> for the specified tenant.
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<IManagementApiClient> GetTenantApiClientAsync(V1Tenant tenant, CancellationToken cancellationToken)
        {
            var api = await _cache.GetOrCreateAsync((tenant.Namespace(), tenant.Name()), async entry =>
            {
                var domain = tenant.Spec.Auth?.Domain;
                if (string.IsNullOrWhiteSpace(domain))
                    throw new InvalidOperationException($"Tenant {tenant.Namespace()}/{tenant.Name()} has no authentication domain.");

                var secretRef = tenant.Spec.Auth?.SecretRef;
                if (secretRef == null)
                    throw new InvalidOperationException($"Tenant {tenant.Namespace()}/{tenant.Name()} has no authentication secret.");

                if (string.IsNullOrWhiteSpace(secretRef.Name))
                    throw new InvalidOperationException($"Tenant {tenant.Namespace()}/{tenant.Name()} has no secret name.");

                var secret = _kube.Get<V1Secret>(secretRef.Name, secretRef.NamespaceProperty ?? tenant.Namespace());
                if (secret == null)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} has missing secret.");

                if (secret.Data.TryGetValue("clientId", out var clientIdBuf) == false)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} has missing clientId value on secret.");

                if (secret.Data.TryGetValue("clientSecret", out var clientSecretBuf) == false)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} has missing clientSecret value on secret.");

                // decode secret values
                var clientId = Encoding.UTF8.GetString(clientIdBuf);
                var clientSecret = Encoding.UTF8.GetString(clientSecretBuf);

                // retrieve authentication token
                var auth = new AuthenticationApiClient(new Uri($"https://{domain}"));
                var authToken = await auth.GetTokenAsync(new ClientCredentialsTokenRequest() { Audience = $"https://{domain}/api/v2/", ClientId = clientId, ClientSecret = clientSecret }, cancellationToken);
                if (authToken.AccessToken == null || authToken.AccessToken.Length == 0)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} failed to retrieve management API token.");

                // contact API using token and domain
                var api = new ManagementApiClient(authToken.AccessToken, new Uri($"https://{domain}/api/v2/"));

                // cache API client for 1 minute
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return (IManagementApiClient)api;
            });

            if (api is null)
                throw new RetryException("Cannot retrieve tenant API client.");

            return api;
        }

        /// <summary>
        /// Gets an active <see cref="ManagementApiClient"/> for the specified tenant.
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<IManagementApiClient> GetTenantApiClientAsync(V2alpha1Tenant tenant, CancellationToken cancellationToken)
        {
            var api = await _cache.GetOrCreateAsync((tenant.Namespace(), tenant.Name()), async entry =>
            {
                var domain = tenant.Spec.Auth?.Domain;
                if (string.IsNullOrWhiteSpace(domain))
                    throw new InvalidOperationException($"Tenant {tenant.Namespace()}/{tenant.Name()} has no authentication domain.");

                var secretRef = tenant.Spec.Auth?.SecretRef;
                if (secretRef == null)
                    throw new InvalidOperationException($"Tenant {tenant.Namespace()}/{tenant.Name()} has no authentication secret.");

                if (string.IsNullOrWhiteSpace(secretRef.Name))
                    throw new InvalidOperationException($"Tenant {tenant.Namespace()}/{tenant.Name()} has no secret name.");

                var secret = _kube.Get<V1Secret>(secretRef.Name, secretRef.NamespaceProperty ?? tenant.Namespace());
                if (secret == null)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} has missing secret.");

                if (secret.Data.TryGetValue("clientId", out var clientIdBuf) == false)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} has missing clientId value on secret.");

                if (secret.Data.TryGetValue("clientSecret", out var clientSecretBuf) == false)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} has missing clientSecret value on secret.");

                // decode secret values
                var clientId = Encoding.UTF8.GetString(clientIdBuf);
                var clientSecret = Encoding.UTF8.GetString(clientSecretBuf);

                // retrieve authentication token
                var auth = new AuthenticationApiClient(new Uri($"https://{domain}"));
                var authToken = await auth.GetTokenAsync(new ClientCredentialsTokenRequest() { Audience = $"https://{domain}/api/v2/", ClientId = clientId, ClientSecret = clientSecret }, cancellationToken);
                if (authToken.AccessToken == null || authToken.AccessToken.Length == 0)
                    throw new RetryException($"Tenant {tenant.Namespace()}/{tenant.Name()} failed to retrieve management API token.");

                // contact API using token and domain
                var api = new ManagementApiClient(authToken.AccessToken, new Uri($"https://{domain}/api/v2/"));

                // cache API client for 1 minute
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return (IManagementApiClient)api;
            });

            if (api is null)
                throw new RetryException("Cannot retrieve tenant API client.");

            return api;
        }

        /// <summary>
        /// Gets an active <see cref="ManagementApiClient"/> for the specified tenant reference.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tenantRef"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected async Task<IManagementApiClient> GetTenantApiClientAsync(TEntity entity, V1TenantReference tenantRef, CancellationToken cancellationToken)
        {
            var v1Tenant = await ResolveV1TenantRef(tenantRef, entity.Namespace(), cancellationToken);
            if (v1Tenant is not null)
            {
                var api = await GetTenantApiClientAsync(v1Tenant, cancellationToken);
                if (api is null)
                    throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} failed to retrieve API client.");

                return api;
            }

            var v2alpha1Tenant = await ResolveV2alpha1TenantRef(tenantRef, entity.Namespace(), cancellationToken);
            if (v2alpha1Tenant is not null)
            {
                var api = await GetTenantApiClientAsync(v2alpha1Tenant, cancellationToken);
                if (api is null)
                    throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} failed to retrieve API client.");

                return api;
            }

            throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing a tenant.");
        }

        /// <summary>
        /// Updates the Reconcile event to a warning.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task ReconcileSuccessAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _kube.CreateAsync(new Eventsv1Event()
            {
                EventTime = DateTime.Now,
                Metadata = new V1ObjectMeta() { NamespaceProperty = entity.Namespace(), GenerateName = "auth0" },
                ReportingController = "kubernetes.auth0.com/operator",
                ReportingInstance = Dns.GetHostName(),
                Regarding = entity.MakeObjectReference(),
                Action = "Reconcile",
                Type = "Normal",
                Reason = "Success"
            }, cancellationToken);
        }

        /// <summary>
        /// Updates the Reconcile event to a warning.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reason"></param>
        /// <param name="note"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task ReconcileWarningAsync(TEntity entity, string reason, string note, CancellationToken cancellationToken)
        {
            await _kube.CreateAsync(new Eventsv1Event()
            {
                EventTime = DateTime.Now,
                Metadata = new V1ObjectMeta() { NamespaceProperty = entity.Namespace(), GenerateName = "auth0" },
                ReportingController = "kubernetes.auth0.com/operator",
                ReportingInstance = Dns.GetHostName(),
                Regarding = entity.MakeObjectReference(),
                Action = "Reconcile",
                Type = "Warning",
                Reason = reason,
                Note = note
            }, cancellationToken);
        }

        /// <summary>
        /// Updates the Deleting event to a warning.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reason"></param>
        /// <param name="note"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task DeletingSuccessAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _kube.CreateAsync(new Eventsv1Event()
            {
                EventTime = DateTime.Now,
                Metadata = new V1ObjectMeta() { NamespaceProperty = entity.Namespace(), GenerateName = "auth0" },
                ReportingController = "kubernetes.auth0.com/operator",
                ReportingInstance = Dns.GetHostName(),
                Regarding = entity.MakeObjectReference(),
                Action = "Deleting",
                Type = "Normal",
                Reason = "Success"
            }, cancellationToken);
        }

        /// <summary>
        /// Updates the Deleting event to a warning.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reason"></param>
        /// <param name="note"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task DeletingWarningAsync(TEntity entity, string reason, string note, CancellationToken cancellationToken)
        {
            await _kube.CreateAsync(new Eventsv1Event()
            {
                EventTime = DateTime.Now,
                Metadata = new V1ObjectMeta() { NamespaceProperty = entity.Namespace(), GenerateName = "auth0" },
                ReportingController = "kubernetes.auth0.com/operator",
                ReportingInstance = Dns.GetHostName(),
                Regarding = entity.MakeObjectReference(),
                Action = "Deleting",
                Type = "Warning",
                Reason = reason,
                Note = note
            }, cancellationToken);
        }

        /// <summary>
        /// Transforms the given Newtonsoft JSON serializable object to a System.Text.Json serializable object.
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(from))]
        protected static TTo? TransformToNewtonsoftJson<TFrom, TTo>(TFrom? from)
            where TFrom : class
            where TTo : class
        {
            if (from == null)
                return null;

            var to = _newtonsoftJsonSerializer.Deserialize<TTo>(new JsonTextReader(new StringReader(System.Text.Json.JsonSerializer.Serialize(from, _jsonSerializerOptions))));
            if (to is null)
                throw new InvalidOperationException();

            return to;
        }

        /// <summary>
        /// Transforms the given Newtonsoft JSON serializable object to a System.Text.Json serializable object.
        /// </summary>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(from))]
        protected static TTo? TransformToSystemTextJson<TTo>(object? from)
            where TTo : class
        {
            if (from == null)
                return null;

            using var w = new StringWriter();
            _newtonsoftJsonSerializer.Serialize(w, from);

            var to = System.Text.Json.JsonSerializer.Deserialize<TTo>(w.ToString(), _jsonSerializerOptions);
            if (to is null)
                throw new InvalidOperationException();

            return to;
        }

        /// <summary>
        /// Implement this method to attempt the reconcillation.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        protected abstract Task Reconcile(TEntity entity, CancellationToken cancellationToken);

        /// <inheritdoc />
        async Task<ReconciliationResult<TEntity>> IEntityController<TEntity>.ReconcileAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                // does work of reconciling, and log success
                await Reconcile(entity, cancellationToken);
                await ReconcileSuccessAsync(entity, cancellationToken);
            }
            catch (ErrorApiException e)
            {
                try
                {
                    Logger.LogError(e, "API error reconciling {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.ApiError?.Message ?? "");
                    await ReconcileWarningAsync(entity, "ApiError", e.ApiError?.Message ?? "", cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // retry after the retry interval
                var interval = Options.Reconciliation.RetryInterval;
                Logger.LogDebug("{EntityTypeName} {Namespace}/{Name} scheduling next reconciliation in {IntervalSeconds}s", EntityTypeName, entity.Namespace(), entity.Name(), interval.TotalSeconds);
                return ReconciliationResult<TEntity>.Failure(entity, e.Message, e, interval);
            }
            catch (RateLimitApiException e)
            {
                try
                {
                    Logger.LogError("Rate limit hit reconciling {EntityTypeName} {EntityNamespace}/{EntityName}", EntityTypeName, entity.Namespace(), entity.Name());
                    await ReconcileWarningAsync(entity, "RateLimit", e.ApiError?.Message ?? "", cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // calculate next attempt time, floored to one minute
                var interval = e.RateLimit?.Reset is DateTimeOffset r ? r - DateTimeOffset.Now : TimeSpan.FromMinutes(1);
                if (interval < TimeSpan.FromMinutes(1))
                    interval = TimeSpan.FromMinutes(1);

                Logger.LogInformation("Rescheduling reconcilation after {TimeSpan}.", interval);
                return ReconciliationResult<TEntity>.Failure(entity, e.Message, e, interval);
            }
            catch (RetryException e)
            {
                try
                {
                    Logger.LogError("Retry hit reconciling {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.Message);
                    await ReconcileWarningAsync(entity, "Retry", e.Message, cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // retry after the error interval
                var interval = Options.Reconciliation.RetryInterval;
                Logger.LogDebug("{EntityTypeName} {Namespace}/{Name} scheduling next reconciliation in {IntervalSeconds}s", EntityTypeName, entity.Namespace(), entity.Name(), interval.TotalSeconds);
                return ReconciliationResult<TEntity>.Failure(entity, e.Message, e, interval);
            }
            catch (Exception e)
            {
                try
                {
                    await ReconcileWarningAsync(entity, "Unknown", e.Message, cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                throw;
            }

            return ReconciliationResult<TEntity>.Success(entity, Options.Reconciliation.Interval);
        }

        /// <summary>
        /// Implement this method to attempt the deletion.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task DeletedAsync(TEntity entity, CancellationToken cancellationToken);

        /// <inheritdoc />
        async Task<ReconciliationResult<TEntity>> IEntityController<TEntity>.DeletedAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                // does work of deleting, and log success
                await DeletedAsync(entity, cancellationToken);
                await DeletingSuccessAsync(entity, cancellationToken);
            }
            catch (ErrorApiException e)
            {
                try
                {
                    Logger.LogError(e, "API error deleting {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.ApiError?.Message ?? "");
                    await DeletingWarningAsync(entity, "ApiError", e.ApiError?.Message ?? "", cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // retry after the retry interval
                var interval = Options.Reconciliation.RetryInterval;
                Logger.LogDebug("{EntityTypeName} {Namespace}/{Name} scheduling next deletion in {IntervalSeconds}s", EntityTypeName, entity.Namespace(), entity.Name(), interval.TotalSeconds);
                return ReconciliationResult<TEntity>.Failure(entity, e.Message, e, interval);
            }
            catch (RateLimitApiException e)
            {
                try
                {
                    Logger.LogError("Rate limit hit deletion {EntityTypeName} {EntityNamespace}/{EntityName}", EntityTypeName, entity.Namespace(), entity.Name());
                    await DeletingWarningAsync(entity, "RateLimit", e.ApiError?.Message ?? "", cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // calculate next attempt time, floored to one minute
                var interval = e.RateLimit?.Reset is DateTimeOffset r ? r - DateTimeOffset.Now : TimeSpan.FromMinutes(1);
                if (interval < TimeSpan.FromMinutes(1))
                    interval = TimeSpan.FromMinutes(1);

                Logger.LogInformation("Rescheduling deletion after {TimeSpan}.", interval);
                return ReconciliationResult<TEntity>.Failure(entity, e.Message, e, interval);
            }
            catch (RetryException e)
            {
                try
                {
                    Logger.LogError("Retry hit deletion {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.Message);
                    await DeletingWarningAsync(entity, "Retry", e.Message, cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // retry after the error interval
                var interval = Options.Reconciliation.Interval;
                Logger.LogDebug("{EntityTypeName} {Namespace}/{Name} scheduling next deletion in {IntervalSeconds}s", EntityTypeName, entity.Namespace(), entity.Name(), interval.TotalSeconds);
                return ReconciliationResult<TEntity>.Failure(entity, e.Message, e, interval);
            }
            catch (Exception e)
            {
                try
                {
                    await DeletingWarningAsync(entity, "Unknown", e.Message, cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                throw;
            }

            return ReconciliationResult<TEntity>.Success(entity);
        }

    }

}
