using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Connections;

using Microsoft.Extensions.Caching.Memory;

namespace Alethic.Auth0.Operator.Controllers
{

    /// <summary>
    /// Caches the set of client ids enabled on a connection. Many ConnectionClient resources typically share one
    /// connection, and Auth0 places <c>GET /api/v2/connections/{id}/clients</c> in a tightly limited rate limit
    /// bucket (connection reads allow as little as 50 requests/minute on self-service plans), so each reconcile
    /// fetching the list independently drains the bucket during sweeps — one fetch per connection per TTL serves
    /// them all instead. Entries are keyed by the management API client instance (itself cached per tenant for
    /// the token lifetime) so tenants never share entries, and every write through
    /// <see cref="SetClientEnabledAsync"/> invalidates the connection's entry so reconciles observe their own
    /// writes immediately.
    /// </summary>
    public sealed class ConnectionEnabledClientsCache
    {

        sealed record CacheKey(IManagementApiClient Api, string ConnectionId);

        /// <summary>
        /// How long a fetched enabled-client set is served before it is refetched. Long enough to cover the
        /// burst of ConnectionClient reconciles in a sweep, short enough that external drift is still observed
        /// on the following sweep.
        /// </summary>
        static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);

        readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cache"></param>
        public ConnectionEnabledClientsCache(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Determines whether the given client is currently enabled on the given connection, serving the
        /// connection's enabled-client set from cache when a fetch within the TTL already retrieved it.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="connectionId"></param>
        /// <param name="clientId"></param>
        /// <param name="cancellationToken"></param>
        public async Task<bool> IsClientEnabledAsync(IManagementApiClient api, string connectionId, string clientId, CancellationToken cancellationToken)
        {
            var ids = await _cache.GetOrCreateAsync(new CacheKey(api, connectionId), async entry =>
            {
                entry.SetAbsoluteExpiration(CacheDuration);
                return await GetEnabledClientIdsAsync(api, connectionId, cancellationToken);
            });

            return ids is not null && ids.Contains(clientId);
        }

        /// <summary>
        /// Applies the given enablement status for a single client on a connection, and invalidates the cached
        /// enabled-client set so subsequent reads observe the write.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="connectionId"></param>
        /// <param name="clientId"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        public async Task SetClientEnabledAsync(IManagementApiClient api, string connectionId, string clientId, bool status, CancellationToken cancellationToken)
        {
            var req = new List<UpdateEnabledClientConnectionsRequestContentItem>
            {
                new UpdateEnabledClientConnectionsRequestContentItem { ClientId = clientId, Status = status },
            };

            try
            {
                await api.Connections.Clients.UpdateAsync(connectionId, req, null, cancellationToken);
            }
            finally
            {
                _cache.Remove(new CacheKey(api, connectionId));
            }
        }

        /// <summary>
        /// Fetches the set of client ids enabled on the connection.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="connectionId"></param>
        /// <param name="cancellationToken"></param>
        static async Task<HashSet<string>> GetEnabledClientIdsAsync(IManagementApiClient api, string connectionId, CancellationToken cancellationToken)
        {
            var ids = new HashSet<string>();

            var pager = await api.Connections.Clients.GetAsync(connectionId, new GetConnectionEnabledClientsRequestParameters(), null, cancellationToken);
            if (pager?.CurrentPage?.Items is { } items)
                foreach (var item in items)
                    if (item.ClientId is { } id)
                        ids.Add(id);

            return ids;
        }

    }

}
