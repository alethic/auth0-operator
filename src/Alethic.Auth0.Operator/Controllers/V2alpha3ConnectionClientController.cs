using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ConnectionClient.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Connections;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3ConnectionClient), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Connection), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3ConnectionClientController :
        V1TenantEntityInstanceController<V2alpha3ConnectionClient, V2alpha3ConnectionClient.SpecDef, V2alpha3ConnectionClient.StatusDef, V2alpha3ConnectionClientConf, V2alpha3ConnectionClientConf>,
        IEntityController<V2alpha3ConnectionClient>
    {

        /// <summary>
        /// Separator used to encode the (connectionId, clientId) pair into the synthetic status ID.
        /// An enabled-client link has no dedicated Auth0 identifier, so we synthesize one from the pair.
        /// </summary>
        const char IdSeparator = '|';

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3ConnectionClientController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3ConnectionClientController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "ConnectionClient";

        /// <summary>
        /// Builds the synthetic status ID for a connection/client enablement.
        /// </summary>
        internal static string BuildId(string connectionId, string clientId) => $"{connectionId}{IdSeparator}{clientId}";

        /// <summary>
        /// Parses a synthetic status ID into its connection and client components.
        /// </summary>
        internal static bool TryParseId(string? id, out string connectionId, out string clientId)
        {
            connectionId = string.Empty;
            clientId = string.Empty;

            if (string.IsNullOrWhiteSpace(id))
                return false;

            var i = id.IndexOf(IdSeparator);
            if (i <= 0 || i >= id.Length - 1)
                return false;

            connectionId = id[..i];
            clientId = id[(i + 1)..];
            return true;
        }

        /// <summary>
        /// Determines whether the given client is currently enabled on the given connection.
        /// </summary>
        async Task<bool> IsClientEnabledAsync(IManagementApiClient api, string connectionId, string clientId, CancellationToken cancellationToken)
        {
            var pager = await api.Connections.Clients.GetAsync(connectionId, new GetConnectionEnabledClientsRequestParameters(), null, cancellationToken);
            if (pager?.CurrentPage?.Items is { } items)
                foreach (var item in items)
                    if (item.ClientId == clientId)
                        return true;

            return false;
        }

        /// <summary>
        /// Applies the given enablement status for a single client on a connection.
        /// </summary>
        Task SetClientEnabledAsync(IManagementApiClient api, string connectionId, string clientId, bool status, CancellationToken cancellationToken)
        {
            var req = new List<UpdateEnabledClientConnectionsRequestContentItem>
            {
                new UpdateEnabledClientConnectionsRequestContentItem { ClientId = clientId, Status = status },
            };

            return api.Connections.Clients.UpdateAsync(connectionId, req, null, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task<V2alpha3ConnectionClientConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (TryParseId(id, out var connectionId, out var clientId) == false)
                return null;

            try
            {
                if (await IsClientEnabledAsync(api, connectionId, clientId, cancellationToken) == false)
                    return null;
            }
            catch (NotFoundError)
            {
                // connection no longer exists
                return null;
            }

            return new V2alpha3ConnectionClientConf
            {
                ConnectionRef = new V1ConnectionReference { Id = connectionId },
                ClientRef = new V1ClientReference { Id = clientId },
            };
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3ConnectionClient entity, V2alpha3ConnectionClient.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            var connectionId = await ResolveConnectionRefToId(api, conf.ConnectionRef, defaultNamespace, cancellationToken);
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new InvalidOperationException("ConnectionRef is required.");

            var clientId = await ResolveClientRefToId(api, conf.ClientRef, defaultNamespace, cancellationToken);
            if (string.IsNullOrWhiteSpace(clientId))
                throw new InvalidOperationException("ClientRef is required.");

            try
            {
                if (await IsClientEnabledAsync(api, connectionId, clientId, cancellationToken))
                    return BuildId(connectionId, clientId);
            }
            catch (NotFoundError)
            {
                return null;
            }

            return null;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V2alpha3ConnectionClientConf conf)
        {
            if (conf.ConnectionRef is null)
                return "missing a value for ConnectionRef";
            if (conf.ClientRef is null)
                return "missing a value for ClientRef";

            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3ConnectionClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var connectionId = await ResolveConnectionRefToId(api, conf.ConnectionRef, defaultNamespace, cancellationToken) ?? throw new InvalidOperationException("ConnectionRef could not be resolved.");
            var clientId = await ResolveClientRefToId(api, conf.ClientRef, defaultNamespace, cancellationToken) ?? throw new InvalidOperationException("ClientRef could not be resolved.");

            Logger.LogInformation("{EntityTypeName} enabling client {ClientId} on connection {ConnectionId}.", EntityTypeName, clientId, connectionId);
            await SetClientEnabledAsync(api, connectionId, clientId, true, cancellationToken);

            return BuildId(connectionId, clientId);
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3ConnectionClientConf? last, V2alpha3ConnectionClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // an enablement has no mutable fields; re-assert the enabled state idempotently
            if (TryParseId(id, out var connectionId, out var clientId) == false)
                throw new InvalidOperationException($"{EntityTypeName} has an invalid ID: {id}.");

            await SetClientEnabledAsync(api, connectionId, clientId, true, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            if (TryParseId(id, out var connectionId, out var clientId) == false)
                return;

            Logger.LogInformation("{EntityTypeName} disabling client {ClientId} on connection {ConnectionId} (reason: Kubernetes entity deleted).", EntityTypeName, clientId, connectionId);

            try
            {
                await SetClientEnabledAsync(api, connectionId, clientId, false, cancellationToken);
            }
            catch (NotFoundError)
            {
                // connection already gone; nothing to disable
            }
        }

    }

}
