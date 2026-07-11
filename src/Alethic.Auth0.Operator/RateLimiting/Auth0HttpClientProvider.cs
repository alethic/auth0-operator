using System.Net.Http;
using System.Threading.RateLimiting;

using Alethic.Auth0.Operator.Options;

using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// Provides the process-wide, rate-limited <see cref="HttpClient"/> used for all Auth0 Management API traffic.
    /// Registered as a singleton so a single client — and its per-domain token bucket — is shared across every
    /// controller. The Auth0 SDK attaches the per-tenant bearer token on each request, so the transport carries no
    /// tenant-specific state and is safe to share.
    /// </summary>
    public sealed class Auth0HttpClientProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public Auth0HttpClientProvider(IOptions<OperatorOptions> options)
        {
            Client = Create(options.Value.RateLimit);
        }

        /// <summary>
        /// Gets the shared, rate-limited <see cref="HttpClient"/> for Auth0 Management API traffic.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Builds the <see cref="HttpClient"/>, applying the rate limiter when enabled.
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
