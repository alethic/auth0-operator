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
    /// before it is sent, and trips the per-domain circuit breaker on any 429 response so that no further requests
    /// reach that domain until the server-reported rate limit reset.
    /// </summary>
    sealed class Auth0RateLimitingHandler : DelegatingHandler
    {

        readonly PartitionedRateLimiter<HttpRequestMessage>? _limiter;
        readonly Auth0CircuitBreaker? _breaker;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="limiter"></param>
        /// <param name="breaker"></param>
        /// <param name="innerHandler"></param>
        public Auth0RateLimitingHandler(PartitionedRateLimiter<HttpRequestMessage>? limiter, Auth0CircuitBreaker? breaker, HttpMessageHandler innerHandler)
        {
            _limiter = limiter;
            _breaker = breaker;
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

            var response = await base.SendAsync(request, cancellationToken);

            // a 429 opens the circuit for the whole domain; the response still flows back so the SDK surfaces
            // its rate limit error to the requesting reconcile, which reschedules on its own
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                _breaker?.Open(host, response);

            return response;
        }

    }

}
