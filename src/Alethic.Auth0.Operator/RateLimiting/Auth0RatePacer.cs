using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Net.Http;

using Alethic.Auth0.Operator.Options;

using Microsoft.Extensions.Logging;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// Adaptive pacing of Auth0 Management API requests, tracked per rate limit bucket. Auth0 enforces separate
    /// token buckets for different endpoint groups (e.g. connection reads have a far smaller budget than the
    /// general Management API on most plans), but no response header names the bucket — so the pacer learns the
    /// mapping: each request is classified into an endpoint key (host + method + id-normalized path), and every
    /// response binds that endpoint to a bucket fingerprinted by host and the reported <c>x-ratelimit-limit</c>.
    /// Endpoints that report the same limit share one budget; endpoints with a dedicated bucket get their own,
    /// so a generous bucket's headers can no longer mask an exhausted one. Once a bucket's remaining budget falls
    /// to the configured threshold, sends are reserved serially — each caller takes the next send slot, spaced by
    /// the time until reset divided by the remaining budget — so concurrent requests spread out to the refill
    /// rate instead of sleeping identically and firing together.
    /// </summary>
    public sealed class Auth0RatePacer
    {

        readonly record struct Budget(long Remaining, DateTimeOffset Reset);

        readonly PacingOptions _options;
        readonly TimeProvider _time;
        readonly ILogger? _logger;
        readonly ConcurrentDictionary<string, string> _buckets = new();
        readonly ConcurrentDictionary<string, Budget> _budgets = new();
        readonly ConcurrentDictionary<string, DateTimeOffset> _nextSend = new();
        readonly object _sync = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="time"></param>
        /// <param name="logger"></param>
        public Auth0RatePacer(PacingOptions options, TimeProvider? time = null, ILogger? logger = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _time = time ?? TimeProvider.System;
            _logger = logger;
        }

        /// <summary>
        /// Classifies a request into its endpoint key: host, method and path, with identifier segments (those
        /// containing characters outside lowercase letters, digits and hyphens — Auth0 resource ids always do)
        /// replaced by <c>*</c> so all requests against the same route template share one key.
        /// </summary>
        /// <param name="request"></param>
        public static string EndpointKeyFor(HttpRequestMessage request)
        {
            var host = request.RequestUri?.Host ?? string.Empty;
            var path = request.RequestUri?.AbsolutePath ?? string.Empty;
            return $"{host} {request.Method.Method} {NormalizePath(path)}";
        }

        /// <summary>
        /// Replaces identifier path segments with <c>*</c>, keeping route vocabulary segments intact.
        /// </summary>
        /// <param name="path"></param>
        internal static string NormalizePath(string path)
        {
            var segments = path.Split('/');
            for (var i = 0; i < segments.Length; i++)
                if (IsIdSegment(segments[i]))
                    segments[i] = "*";

            return string.Join('/', segments);
        }

        /// <summary>
        /// An identifier segment contains at least one character outside lowercase letters, digits and hyphens —
        /// Auth0 route vocabulary ("connections", "email-templates", "v2") is all lowercase, while most resource
        /// ids carry uppercase letters or underscores ("con_AbC123", client ids) — or is a long lowercase-hex
        /// string, which slips past the character test but is always an id (e.g. resource server ids); route
        /// vocabulary is short and always contains letters outside a–f.
        /// </summary>
        /// <param name="segment"></param>
        static bool IsIdSegment(string segment)
        {
            foreach (var c in segment)
                if (char.IsAsciiLetterLower(c) == false && char.IsAsciiDigit(c) == false && c != '-')
                    return true;

            return segment.Length >= 16 && IsLowercaseHex(segment);
        }

        /// <summary>
        /// Whether the segment consists solely of lowercase hexadecimal characters.
        /// </summary>
        /// <param name="segment"></param>
        static bool IsLowercaseHex(string segment)
        {
            foreach (var c in segment)
                if (char.IsAsciiDigit(c) == false && (c < 'a' || c > 'f'))
                    return false;

            return true;
        }

        /// <summary>
        /// Records the rate limit budget reported by a response to the given endpoint, and binds the endpoint to
        /// the bucket the response's <c>x-ratelimit-limit</c> fingerprints. Responses without both a parseable
        /// remaining count and reset time are ignored; without a limit header the endpoint is its own bucket.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="response"></param>
        public void Record(string endpoint, HttpResponseMessage response)
        {
            if (TryGetHeaderValue(response, "x-ratelimit-remaining", out var remaining) == false)
                return;

            if (TryGetHeaderValue(response, "x-ratelimit-reset", out var resetEpochSeconds) == false)
                return;

            var hasLimit = TryGetHeaderValue(response, "x-ratelimit-limit", out var limit);
            var bucket = hasLimit ? BucketIdFor(endpoint, limit) : endpoint;

            if (_buckets.TryGetValue(endpoint, out var previous) == false || previous != bucket)
                _logger?.LogDebug("Auth0 endpoint {Endpoint} bound to rate limit bucket {Bucket} (limit {Limit}).", endpoint, bucket, hasLimit ? limit : (long?)null);

            var reset = DateTimeOffset.FromUnixTimeSeconds(resetEpochSeconds);

            _buckets[endpoint] = bucket;
            _budgets[bucket] = new Budget(remaining, reset);

            _logger?.LogDebug("Auth0 rate limit bucket {Bucket} has {Remaining} of {Limit} remaining, reset at {Reset:O} (reported by {Endpoint}).", bucket, remaining, hasLimit ? limit : (long?)null, reset, endpoint);
        }

        /// <summary>
        /// Fingerprints a bucket by the endpoint's host and the reported limit, so endpoints of one tenant that
        /// report the same limit share a budget while other tenants' buckets stay separate.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="limit"></param>
        static string BucketIdFor(string endpoint, long limit)
        {
            var i = endpoint.IndexOf(' ');
            var host = i > 0 ? endpoint[..i] : endpoint;
            return $"{host}#{limit}";
        }

        /// <summary>
        /// Reserves a send slot for the next request to <paramref name="endpoint"/> and returns how long the
        /// caller must wait before sending: zero while the endpoint's bucket is unknown, above the threshold or
        /// already reset, otherwise the time until the reserved slot — slots are spaced by the time until reset
        /// divided by the remaining budget, and each reservation pushes the bucket's next slot out, so concurrent
        /// senders serialize instead of firing together. The returned delay is bounded by the configured maximum
        /// per-request delay; beyond it, the circuit breaker is the backstop.
        /// </summary>
        /// <param name="endpoint"></param>
        public TimeSpan Reserve(string endpoint)
        {
            if (_buckets.TryGetValue(endpoint, out var bucket) == false)
                return TimeSpan.Zero;

            if (_budgets.TryGetValue(bucket, out var budget) == false)
                return TimeSpan.Zero;

            var now = _time.GetUtcNow();
            if (budget.Reset <= now)
            {
                // the bucket has refilled; forget the stale budget unless a newer one raced in
                _budgets.TryRemove(new System.Collections.Generic.KeyValuePair<string, Budget>(bucket, budget));
                return TimeSpan.Zero;
            }

            if (budget.Remaining > _options.RemainingThreshold)
                return TimeSpan.Zero;

            var interval = (budget.Reset - now) / Math.Max(budget.Remaining, 1);
            if (interval > _options.MaxRequestDelay)
                interval = _options.MaxRequestDelay;

            DateTimeOffset slot;
            lock (_sync)
            {
                slot = _nextSend.TryGetValue(bucket, out var next) && next > now ? next + interval : now + interval;
                _nextSend[bucket] = slot;
            }

            var delay = slot - now;
            if (delay > _options.MaxRequestDelay)
                delay = _options.MaxRequestDelay;

            if (delay > TimeSpan.Zero)
                _logger?.LogDebug("Pacing Auth0 request to {Endpoint}: waiting {DelayMs}ms for a send slot on bucket {Bucket} ({Remaining} remaining, reset at {Reset:O}).", endpoint, (long)delay.TotalMilliseconds, bucket, budget.Remaining, budget.Reset);

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
