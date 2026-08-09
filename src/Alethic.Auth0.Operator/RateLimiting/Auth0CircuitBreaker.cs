using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net.Http;

using Alethic.Auth0.Operator.Options;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// Per-domain circuit breaker for Auth0 rate limits. When any request to a domain receives a 429 the circuit
    /// opens until the server-reported reset time, and every request issued while it is open fails fast with an
    /// <see cref="Auth0CircuitOpenException"/> instead of reaching Auth0. The circuit closes by the passage of
    /// time alone; a further 429 after it closes simply reopens it.
    /// </summary>
    public sealed class Auth0CircuitBreaker
    {

        readonly CircuitBreakerOptions _options;
        readonly TimeProvider _time;
        readonly ConcurrentDictionary<string, DateTimeOffset> _openUntil = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="time"></param>
        public Auth0CircuitBreaker(CircuitBreakerOptions options, TimeProvider? time = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _time = time ?? TimeProvider.System;
        }

        /// <summary>
        /// Throws <see cref="Auth0CircuitOpenException"/> when the circuit for <paramref name="host"/> is open.
        /// </summary>
        /// <param name="host"></param>
        /// <exception cref="Auth0CircuitOpenException"></exception>
        public void ThrowIfOpen(string host)
        {
            if (_openUntil.TryGetValue(host, out var until) == false)
                return;

            if (_time.GetUtcNow() < until)
                throw new Auth0CircuitOpenException(host, until);

            // expired; remove only if unchanged so a concurrent reopen is not lost
            _openUntil.TryRemove(new System.Collections.Generic.KeyValuePair<string, DateTimeOffset>(host, until));
        }

        /// <summary>
        /// Opens the circuit for <paramref name="host"/> in response to a 429, using the response's rate limit
        /// reset headers when available and the configured default otherwise. Returns the time until which the
        /// circuit is open.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="response"></param>
        public DateTimeOffset Open(string host, HttpResponseMessage response)
        {
            var now = _time.GetUtcNow();

            var until = ResolveResetTime(response, now) ?? now + _options.DefaultOpenDuration;

            // clamp into (now, now + max]; a reset in the past still opens for the default duration so
            // repeated 429s cannot bypass the breaker entirely
            if (until <= now)
                until = now + _options.DefaultOpenDuration;
            if (until > now + _options.MaxOpenDuration)
                until = now + _options.MaxOpenDuration;

            // an already-open circuit is never shortened; return whichever deadline is now in effect
            return _openUntil.AddOrUpdate(host, until, (_, existing) => until > existing ? until : existing);
        }

        /// <summary>
        /// Resolves the server-reported moment the rate limit resets: Retry-After (delta or date) first, then the
        /// Auth0 <c>x-ratelimit-reset</c> header (unix epoch seconds).
        /// </summary>
        /// <param name="response"></param>
        /// <param name="now"></param>
        static DateTimeOffset? ResolveResetTime(HttpResponseMessage response, DateTimeOffset now)
        {
            if (response.Headers.RetryAfter?.Delta is { } delta)
                return now + delta;

            if (response.Headers.RetryAfter?.Date is { } date)
                return date;

            if (response.Headers.TryGetValues("x-ratelimit-reset", out var values))
                foreach (var value in values)
                    if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var epochSeconds))
                        return DateTimeOffset.FromUnixTimeSeconds(epochSeconds);

            return null;
        }

    }

}
