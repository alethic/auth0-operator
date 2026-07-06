using System.Net.Http;
using System.Threading.RateLimiting;

using Alethic.Auth0.Operator.Options;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// Builds the process-wide <see cref="HttpClient"/> used for all Auth0 Management API traffic. A single client is
    /// shared across tenants: the Auth0 SDK attaches the per-tenant bearer token on each request, so the transport
    /// carries no tenant-specific state. Requests are throttled with a token bucket partitioned by Auth0 domain, so
    /// each tenant is limited independently (Auth0 rate limits are per-tenant).
    /// </summary>
    static class Auth0HttpClientFactory
    {

        static HttpClient? _shared;
        static readonly object _lock = new();

        /// <summary>
        /// Returns the process-wide shared <see cref="HttpClient"/>, building it once from <paramref name="options"/>
        /// on first use. This memoization lives in this non-generic type deliberately: a static field on the generic
        /// <c>ControllerBase&lt;...&gt;</c> would be duplicated per closed generic type (one per entity kind), giving
        /// each kind its own limiter rather than a single shared budget.
        /// </summary>
        /// <param name="options"></param>
        public static HttpClient GetOrCreate(RateLimitOptions options)
        {
            if (_shared is { } c)
                return c;

            lock (_lock)
                return _shared ??= Create(options);
        }

        /// <summary>
        /// Creates the shared <see cref="HttpClient"/>, applying the rate limiter when enabled.
        /// </summary>
        /// <param name="options"></param>
        static HttpClient Create(RateLimitOptions options)
        {
            var transport = new SocketsHttpHandler();
            if (options.Enabled == false)
                return new HttpClient(transport);

            var limiter = PartitionedRateLimiter.Create<HttpRequestMessage, string>(request =>
                RateLimitPartition.GetTokenBucketLimiter(
                    request.RequestUri?.Host ?? string.Empty,
                    _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = options.TokenLimit,
                        TokensPerPeriod = options.TokensPerPeriod,
                        ReplenishmentPeriod = options.ReplenishmentPeriod,
                        QueueLimit = options.QueueLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        AutoReplenishment = true,
                    }));

            return new HttpClient(new Auth0RateLimitingHandler(limiter, transport));
        }

    }

}
