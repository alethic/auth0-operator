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
    /// before it is sent, and, as a last resort, honors any <c>Retry-After</c> header returned on a 429 response.
    /// </summary>
    sealed class Auth0RateLimitingHandler : DelegatingHandler
    {

        readonly PartitionedRateLimiter<HttpRequestMessage> _limiter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="limiter"></param>
        /// <param name="innerHandler"></param>
        public Auth0RateLimitingHandler(PartitionedRateLimiter<HttpRequestMessage> limiter, HttpMessageHandler innerHandler)
        {
            _limiter = limiter ?? throw new ArgumentNullException(nameof(limiter));
            InnerHandler = innerHandler ?? throw new ArgumentNullException(nameof(innerHandler));
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var lease = await _limiter.AcquireAsync(request, 1, cancellationToken);
            if (lease.IsAcquired == false)
                throw new HttpRequestException("Auth0 Management API client-side rate limit queue is full; backing off.");

            var response = await base.SendAsync(request, cancellationToken);

            // last resort: if a 429 still slips through, respect the server's Retry-After before returning
            if (response.StatusCode == HttpStatusCode.TooManyRequests && response.Headers.RetryAfter?.Delta is { } delta)
                await Task.Delay(delta, cancellationToken);

            return response;
        }

    }

}
