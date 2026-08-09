using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Net.Http;

using Alethic.Auth0.Operator.Options;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// Adaptive per-domain pacing of Auth0 Management API requests. Every response's rate limit headers are
    /// recorded, and once the reported remaining budget falls to the configured threshold, outbound requests are
    /// delayed just enough to spread the remaining budget across the time until the server-reported reset — so
    /// consumption slows to Auth0's refill rate instead of draining the bucket and tripping the circuit breaker.
    /// </summary>
    public sealed class Auth0RatePacer
    {

        readonly record struct Budget(long Remaining, DateTimeOffset Reset);

        readonly PacingOptions _options;
        readonly TimeProvider _time;
        readonly ConcurrentDictionary<string, Budget> _budgets = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="time"></param>
        public Auth0RatePacer(PacingOptions options, TimeProvider? time = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _time = time ?? TimeProvider.System;
        }

        /// <summary>
        /// Records the rate limit budget reported by a response. Responses without both a parseable remaining
        /// count and reset time are ignored.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="response"></param>
        public void Record(string host, HttpResponseMessage response)
        {
            if (TryGetHeaderValue(response, "x-ratelimit-remaining", out var remaining) == false)
                return;

            if (TryGetHeaderValue(response, "x-ratelimit-reset", out var resetEpochSeconds) == false)
                return;

            _budgets[host] = new Budget(remaining, DateTimeOffset.FromUnixTimeSeconds(resetEpochSeconds));
        }

        /// <summary>
        /// Computes the delay to apply before the next request to <paramref name="host"/>: zero while the
        /// reported budget is above the threshold or already reset, otherwise the time until reset divided by
        /// the remaining budget, bounded by the configured maximum per-request delay.
        /// </summary>
        /// <param name="host"></param>
        public TimeSpan GetDelay(string host)
        {
            if (_budgets.TryGetValue(host, out var budget) == false)
                return TimeSpan.Zero;

            var now = _time.GetUtcNow();
            if (budget.Reset <= now)
            {
                // the bucket has refilled; forget the stale budget unless a newer one raced in
                _budgets.TryRemove(new System.Collections.Generic.KeyValuePair<string, Budget>(host, budget));
                return TimeSpan.Zero;
            }

            if (budget.Remaining > _options.RemainingThreshold)
                return TimeSpan.Zero;

            var delay = (budget.Reset - now) / Math.Max(budget.Remaining, 1);
            if (delay > _options.MaxRequestDelay)
                delay = _options.MaxRequestDelay;

            return delay;
        }

        static bool TryGetHeaderValue(HttpResponseMessage response, string name, out long value)
        {
            value = 0;

            if (response.Headers.TryGetValues(name, out var values) == false)
                return false;

            return long.TryParse(values.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

    }

}
