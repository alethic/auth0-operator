using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace Alethic.Auth0.Operator.RateLimiting
{

    /// <summary>
    /// <see cref="DelegatingHandler"/> that gates every outgoing Auth0 Management API request through a rate limiter
    /// before it is sent, paces requests down to Auth0's refill rate as the reported budget of the endpoint's rate
    /// limit bucket depletes, and trips the per-domain circuit breaker on any 429 response so that no further
    /// requests reach that domain until the server-reported rate limit reset.
    /// </summary>
    sealed class Auth0RateLimitingHandler : DelegatingHandler
    {

        readonly PartitionedRateLimiter<HttpRequestMessage>? _limiter;
        readonly Auth0CircuitBreaker? _breaker;
        readonly Auth0RatePacer? _pacer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="limiter"></param>
        /// <param name="breaker"></param>
        /// <param name="pacer"></param>
        /// <param name="innerHandler"></param>
        public Auth0RateLimitingHandler(PartitionedRateLimiter<HttpRequestMessage>? limiter, Auth0CircuitBreaker? breaker, Auth0RatePacer? pacer, HttpMessageHandler innerHandler)
        {
            _limiter = limiter;
            _breaker = breaker;
            _pacer = pacer;
            InnerHandler = innerHandler ?? throw new ArgumentNullException(nameof(innerHandler));
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var host = request.RequestUri?.Host ?? string.Empty;

            // fail fast before consuming a limiter token when the domain's circuit is open
            _breaker?.ThrowIfOpen(host);

            if (_limiter is not null)
            {
                using var lease = await _limiter.AcquireAsync(request, 1, cancellationToken);
                if (lease.IsAcquired == false)
                    throw new HttpRequestException("Auth0 Management API client-side rate limit queue is full; backing off.");
            }

            // when the reported budget of the endpoint's rate limit bucket is low, reserve a send slot so
            // consumption slows to Auth0's refill rate instead of draining the bucket; holding the request
            // (and thereby its reconcile slot) is the intended backpressure
            var endpoint = _pacer is null ? null : Auth0RatePacer.EndpointKeyFor(request);
            if (endpoint is not null)
            {
                var delay = _pacer!.Reserve(endpoint);
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay, cancellationToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (endpoint is not null)
                _pacer!.Record(endpoint, response);

            // a 429 opens the circuit for the whole domain; the response still flows back so the SDK surfaces
            // its rate limit error to the requesting reconcile, which reschedules on its own. a successful
            // response reporting the bucket as exhausted opens the circuit preemptively, so the 429 the next
            // request would receive never happens
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                _breaker?.Open(host, response);
            else
                _breaker?.OpenIfExhausted(host, response);

            return response;
        }

    }

}
