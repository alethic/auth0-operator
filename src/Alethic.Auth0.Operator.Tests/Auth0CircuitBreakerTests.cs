using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class Auth0CircuitBreakerTests
    {

        sealed class FakeTimeProvider : TimeProvider
        {

            DateTimeOffset _now;

            public FakeTimeProvider(DateTimeOffset now)
            {
                _now = now;
            }

            public override DateTimeOffset GetUtcNow() => _now;

            public void Advance(TimeSpan by) => _now += by;

        }

        static HttpResponseMessage TooManyRequests(Action<HttpResponseMessage>? configure = null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            configure?.Invoke(response);
            return response;
        }

        [TestMethod]
        public void ClosedCircuit_DoesNotThrow()
        {
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions());
            breaker.ThrowIfOpen("tenant.us.auth0.com");
        }

        [TestMethod]
        public void OpenCircuit_ThrowsUntilExpiry()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { DefaultOpenDuration = TimeSpan.FromMinutes(1) }, time);

            breaker.Open("tenant.us.auth0.com", TooManyRequests());
            Assert.ThrowsExactly<Auth0CircuitOpenException>(() => breaker.ThrowIfOpen("tenant.us.auth0.com"));

            time.Advance(TimeSpan.FromMinutes(2));
            breaker.ThrowIfOpen("tenant.us.auth0.com");
        }

        [TestMethod]
        public void OpenCircuit_IsPerHost()
        {
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions());
            breaker.Open("a.us.auth0.com", TooManyRequests());

            Assert.ThrowsExactly<Auth0CircuitOpenException>(() => breaker.ThrowIfOpen("a.us.auth0.com"));
            breaker.ThrowIfOpen("b.us.auth0.com");
        }

        [TestMethod]
        public void Open_UsesRetryAfterDelta()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { DefaultOpenDuration = TimeSpan.FromMinutes(1) }, time);

            var until = breaker.Open("tenant.us.auth0.com", TooManyRequests(r => r.Headers.Add("Retry-After", "120")));

            Assert.AreEqual(DateTimeOffset.UnixEpoch + TimeSpan.FromSeconds(120), until);
        }

        [TestMethod]
        public void Open_UsesRateLimitResetEpoch()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions(), time);

            var until = breaker.Open("tenant.us.auth0.com", TooManyRequests(r => r.Headers.Add("x-ratelimit-reset", "90")));

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(90), until);
        }

        [TestMethod]
        public void Open_WithoutHeaders_UsesDefaultDuration()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { DefaultOpenDuration = TimeSpan.FromSeconds(30) }, time);

            var until = breaker.Open("tenant.us.auth0.com", TooManyRequests());

            Assert.AreEqual(DateTimeOffset.UnixEpoch + TimeSpan.FromSeconds(30), until);
        }

        [TestMethod]
        public void Open_ResetInThePast_StillOpensForDefaultDuration()
        {
            var time = new FakeTimeProvider(DateTimeOffset.FromUnixTimeSeconds(1000));
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { DefaultOpenDuration = TimeSpan.FromSeconds(30) }, time);

            var until = breaker.Open("tenant.us.auth0.com", TooManyRequests(r => r.Headers.Add("x-ratelimit-reset", "500")));

            Assert.AreEqual(time.GetUtcNow() + TimeSpan.FromSeconds(30), until);
        }

        [TestMethod]
        public void Open_ResetBeyondMax_IsClamped()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { MaxOpenDuration = TimeSpan.FromMinutes(10) }, time);

            var until = breaker.Open("tenant.us.auth0.com", TooManyRequests(r => r.Headers.Add("Retry-After", "86400")));

            Assert.AreEqual(DateTimeOffset.UnixEpoch + TimeSpan.FromMinutes(10), until);
        }

        [TestMethod]
        public void Open_NeverShortensAnOpenCircuit()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { DefaultOpenDuration = TimeSpan.FromMinutes(5) }, time);

            var longer = breaker.Open("tenant.us.auth0.com", TooManyRequests());
            var shorter = breaker.Open("tenant.us.auth0.com", TooManyRequests(r => r.Headers.Add("Retry-After", "10")));

            Assert.AreEqual(longer, shorter);
        }

        [TestMethod]
        public void OpenIfExhausted_RemainingZeroWithReset_OpensUntilReset()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions(), time);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("x-ratelimit-remaining", "0");
            response.Headers.Add("x-ratelimit-reset", "60");

            var until = breaker.OpenIfExhausted("tenant.us.auth0.com", response);

            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(60), until);
            Assert.ThrowsExactly<Auth0CircuitOpenException>(() => breaker.ThrowIfOpen("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void OpenIfExhausted_RemainingZeroWithoutReset_DoesNotOpen()
        {
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions());
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("x-ratelimit-remaining", "0");

            Assert.IsNull(breaker.OpenIfExhausted("tenant.us.auth0.com", response));
            breaker.ThrowIfOpen("tenant.us.auth0.com");
        }

        [TestMethod]
        public void OpenIfExhausted_RemainingZeroWithPastReset_DoesNotOpen()
        {
            var time = new FakeTimeProvider(DateTimeOffset.FromUnixTimeSeconds(1000));
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions(), time);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("x-ratelimit-remaining", "0");
            response.Headers.Add("x-ratelimit-reset", "500");

            Assert.IsNull(breaker.OpenIfExhausted("tenant.us.auth0.com", response));
            breaker.ThrowIfOpen("tenant.us.auth0.com");
        }

        [TestMethod]
        public void OpenIfExhausted_RemainingNonZero_DoesNotOpen()
        {
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions());
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("x-ratelimit-remaining", "5");
            response.Headers.Add("x-ratelimit-reset", "9999999999");

            Assert.IsNull(breaker.OpenIfExhausted("tenant.us.auth0.com", response));
            breaker.ThrowIfOpen("tenant.us.auth0.com");
        }

        [TestMethod]
        public async Task Handler_PreemptivelyOpensWhenBucketExhausted()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions(), time);
            var stub = new StubHandler
            {
                Respond = _ =>
                {
                    var ok = new HttpResponseMessage(HttpStatusCode.OK);
                    ok.Headers.Add("x-ratelimit-remaining", "0");
                    ok.Headers.Add("x-ratelimit-reset", "60");
                    return ok;
                },
            };
            using var client = new HttpClient(new Auth0RateLimitingHandler(null, breaker, stub));

            // the exhausting request itself succeeds
            var response = await client.GetAsync("https://tenant.us.auth0.com/api/v2/clients");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // the request that would have received the 429 fails fast instead
            await Assert.ThrowsExactlyAsync<Auth0CircuitOpenException>(() => client.GetAsync("https://tenant.us.auth0.com/api/v2/clients"));
            Assert.AreEqual(1, stub.Sent);

            time.Advance(TimeSpan.FromSeconds(61));
            stub.Respond = _ => new HttpResponseMessage(HttpStatusCode.OK);
            var recovered = await client.GetAsync("https://tenant.us.auth0.com/api/v2/clients");
            Assert.AreEqual(HttpStatusCode.OK, recovered.StatusCode);
        }

        [TestMethod]
        public void FindException_UnwrapsInnerChain()
        {
            var circuitOpen = new Auth0CircuitOpenException("tenant.us.auth0.com", DateTimeOffset.UnixEpoch);
            var wrapped = new InvalidOperationException("outer", new HttpRequestException("mid", circuitOpen));

            Assert.AreSame(circuitOpen, Auth0CircuitOpenException.Find(wrapped));
            Assert.IsNull(Auth0CircuitOpenException.Find(new InvalidOperationException("no match")));
        }

        // ──────────────────────── handler integration ─────────────────────────────

        sealed class StubHandler : HttpMessageHandler
        {

            public Func<HttpRequestMessage, HttpResponseMessage> Respond { get; set; } = _ => new HttpResponseMessage(HttpStatusCode.OK);

            public int Sent { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Sent++;
                return Task.FromResult(Respond(request));
            }

        }

        [TestMethod]
        public async Task Handler_TripsBreakerOn429_AndFailsFastAfterwards()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var breaker = new Auth0CircuitBreaker(new CircuitBreakerOptions { DefaultOpenDuration = TimeSpan.FromMinutes(1) }, time);
            var stub = new StubHandler { Respond = _ => TooManyRequests() };
            using var client = new HttpClient(new Auth0RateLimitingHandler(null, breaker, stub));

            // first request reaches Auth0 and receives the 429 back
            var response = await client.GetAsync("https://tenant.us.auth0.com/api/v2/clients");
            Assert.AreEqual(HttpStatusCode.TooManyRequests, response.StatusCode);
            Assert.AreEqual(1, stub.Sent);

            // every request while the circuit is open fails fast without reaching Auth0
            await Assert.ThrowsExactlyAsync<Auth0CircuitOpenException>(() => client.GetAsync("https://tenant.us.auth0.com/api/v2/clients"));
            Assert.AreEqual(1, stub.Sent);

            // requests to other domains are unaffected
            stub.Respond = _ => new HttpResponseMessage(HttpStatusCode.OK);
            var other = await client.GetAsync("https://other.us.auth0.com/api/v2/clients");
            Assert.AreEqual(HttpStatusCode.OK, other.StatusCode);

            // once the reset passes, requests flow again
            time.Advance(TimeSpan.FromMinutes(2));
            var recovered = await client.GetAsync("https://tenant.us.auth0.com/api/v2/clients");
            Assert.AreEqual(HttpStatusCode.OK, recovered.StatusCode);
        }

    }

}
