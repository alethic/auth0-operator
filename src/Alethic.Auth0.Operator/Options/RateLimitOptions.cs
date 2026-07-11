using System;

namespace Alethic.Auth0.Operator.Options
{

    /// <summary>
    /// Configuration for client-side rate limiting of Auth0 Management API requests. Requests are throttled with a
    /// token bucket partitioned by Auth0 domain, so the operator stays within Auth0's per-tenant rate limits even when
    /// many resources reconcile at once (e.g. on startup). Because every reconcile awaits its Auth0 calls, throttling
    /// the HTTP layer also naturally backpressures the reconcile loop.
    /// </summary>
    public class RateLimitOptions
    {

        /// <summary>
        /// Whether client-side rate limiting is applied. When false, requests are sent without throttling.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Maximum burst capacity: the number of requests that may be issued immediately before throttling applies.
        /// </summary>
        public int TokenLimit { get; set; } = 10;

        /// <summary>
        /// Number of request tokens replenished each <see cref="ReplenishmentPeriod"/> (i.e. the sustained rate).
        /// </summary>
        public int TokensPerPeriod { get; set; } = 5;

        /// <summary>
        /// How often <see cref="TokensPerPeriod"/> tokens are added back to the bucket.
        /// </summary>
        public TimeSpan ReplenishmentPeriod { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Maximum number of requests permitted to wait for a token. Requests beyond this fail fast and are retried on
        /// a later reconcile rather than queueing unbounded.
        /// </summary>
        public int QueueLimit { get; set; } = 10000;

    }

}
