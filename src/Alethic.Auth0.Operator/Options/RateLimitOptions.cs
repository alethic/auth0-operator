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

        /// <summary>
        /// Configuration of the circuit breaker that opens when Auth0 returns a 429.
        /// </summary>
        public CircuitBreakerOptions CircuitBreaker { get; set; } = new();

    }

    /// <summary>
    /// Configuration for the per-domain circuit breaker. When Auth0 answers any request with a 429, the breaker
    /// opens for that domain until the server-reported rate limit reset, and every request in the meantime fails
    /// fast without reaching Auth0 — so a single 429 backpressures the whole reconcile loop instead of every
    /// entity discovering the rate limit on its own.
    /// </summary>
    public class CircuitBreakerOptions
    {

        /// <summary>
        /// Whether the circuit breaker is applied. When false, 429 responses only affect the requesting reconcile.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// How long the breaker stays open after a 429 that carries no usable Retry-After or rate limit reset header.
        /// </summary>
        public TimeSpan DefaultOpenDuration { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Upper bound on how long the breaker stays open, guarding against clock skew in server-reported reset times.
        /// </summary>
        public TimeSpan MaxOpenDuration { get; set; } = TimeSpan.FromMinutes(10);

    }

}
