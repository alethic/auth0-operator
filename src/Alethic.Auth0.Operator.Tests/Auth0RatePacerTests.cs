using System;
using System.Net;
using System.Net.Http;

using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class Auth0RatePacerTests
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

        static HttpResponseMessage Response(long? remaining = null, long? resetEpochSeconds = null, long? limit = null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (remaining is { } r)
                response.Headers.Add("x-ratelimit-remaining", r.ToString());
            if (resetEpochSeconds is { } s)
                response.Headers.Add("x-ratelimit-reset", s.ToString());
            if (limit is { } l)
                response.Headers.Add("x-ratelimit-limit", l.ToString());
            return response;
        }

        [TestMethod]
        public void NoRecordedBudget_NoDelay()
        {
            var pacer = new Auth0RatePacer(new PacingOptions());
            Assert.AreEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void BudgetAboveThreshold_NoDelay()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 50, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void BudgetAtThreshold_SpreadsRemainingUntilReset()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromMinutes(1) }, time);

            // 10 requests left, 60 seconds until refill: pace at one request per 6 seconds
            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 10, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.FromSeconds(6), pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void ConcurrentReservations_SerializeSends()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromMinutes(1) }, time);

            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 10, resetEpochSeconds: 60));

            // each reservation takes the next send slot instead of every caller sleeping the same 6 seconds
            // and firing together
            Assert.AreEqual(TimeSpan.FromSeconds(6), pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
            Assert.AreEqual(TimeSpan.FromSeconds(12), pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
            Assert.AreEqual(TimeSpan.FromSeconds(18), pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void ExhaustedBudget_DelayCappedAtMaxRequestDelay()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromSeconds(10) }, time);

            // 0 remaining with a 60s reset would want a 60s delay; the cap bounds it
            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 0, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.FromSeconds(10), pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void PastReset_NoDelay()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 1, resetEpochSeconds: 60));
            time.Advance(TimeSpan.FromSeconds(61));

            Assert.AreEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void WithoutLimitHeader_BudgetIsPerEndpoint()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("a.us.auth0.com GET /api/v2/clients", Response(remaining: 1, resetEpochSeconds: 60));

            Assert.AreNotEqual(TimeSpan.Zero, pacer.Reserve("a.us.auth0.com GET /api/v2/clients"));
            Assert.AreEqual(TimeSpan.Zero, pacer.Reserve("b.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void ResponseWithoutHeaders_Ignored()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 1, resetEpochSeconds: 60));
            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response());

            // the headerless response must not clobber the recorded budget
            Assert.AreNotEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void NewerBudget_ReplacesOlder()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 1, resetEpochSeconds: 60));
            pacer.Record("tenant.us.auth0.com GET /api/v2/clients", Response(remaining: 50, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void GenerousBucket_DoesNotMaskTightBucket()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromMinutes(1) }, time);

            // the connection-reads bucket is nearly exhausted; a later response from the far more generous
            // general bucket must not overwrite its budget and let connection reads flow unpaced
            pacer.Record("tenant.us.auth0.com GET /api/v2/connections/*/clients", Response(remaining: 1, resetEpochSeconds: 60, limit: 50));
            pacer.Record("tenant.us.auth0.com GET /api/v2/clients/*", Response(remaining: 900, resetEpochSeconds: 60, limit: 1000));

            Assert.AreNotEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/connections/*/clients"));
            Assert.AreEqual(TimeSpan.Zero, pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients/*"));
        }

        [TestMethod]
        public void EndpointsReportingSameLimit_ShareOneBucket()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromMinutes(2) }, time);

            // two endpoint families reporting the same limit are one server-side bucket (e.g. the free tier's
            // single Management API bucket); their sends must serialize against one shared budget, not each
            // pace to the full refill rate independently
            pacer.Record("tenant.us.auth0.com GET /api/v2/clients/*", Response(remaining: 2, resetEpochSeconds: 60, limit: 2));
            pacer.Record("tenant.us.auth0.com GET /api/v2/client-grants", Response(remaining: 2, resetEpochSeconds: 60, limit: 2));

            Assert.AreEqual(TimeSpan.FromSeconds(30), pacer.Reserve("tenant.us.auth0.com GET /api/v2/clients/*"));
            Assert.AreEqual(TimeSpan.FromSeconds(60), pacer.Reserve("tenant.us.auth0.com GET /api/v2/client-grants"));
        }

        [TestMethod]
        public void SameLimitOnDifferentHosts_SeparateBuckets()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromMinutes(2) }, time);

            pacer.Record("a.us.auth0.com GET /api/v2/clients", Response(remaining: 2, resetEpochSeconds: 60, limit: 2));
            pacer.Record("b.us.auth0.com GET /api/v2/clients", Response(remaining: 2, resetEpochSeconds: 60, limit: 2));

            // tenants never share budgets, even when their buckets report the same limit
            Assert.AreEqual(TimeSpan.FromSeconds(30), pacer.Reserve("a.us.auth0.com GET /api/v2/clients"));
            Assert.AreEqual(TimeSpan.FromSeconds(30), pacer.Reserve("b.us.auth0.com GET /api/v2/clients"));
        }

        [TestMethod]
        public void EndpointKeyFor_ReplacesIdSegments()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://tenant.us.auth0.com/api/v2/connections/con_AbC123/clients");
            Assert.AreEqual("tenant.us.auth0.com GET /api/v2/connections/*/clients", Auth0RatePacer.EndpointKeyFor(request));
        }

        [TestMethod]
        public void EndpointKeyFor_ReplacesUnprefixedIdSegments()
        {
            // client ids carry no prefix but always contain uppercase characters
            var request = new HttpRequestMessage(HttpMethod.Get, "https://tenant.us.auth0.com/api/v2/clients/HqT2mXWuRfg1KVXwLutM");
            Assert.AreEqual("tenant.us.auth0.com GET /api/v2/clients/*", Auth0RatePacer.EndpointKeyFor(request));
        }

        [TestMethod]
        public void EndpointKeyFor_KeepsRouteVocabulary()
        {
            // lowercase segments, hyphens and the version segment are route vocabulary, not ids
            var request = new HttpRequestMessage(HttpMethod.Patch, "https://tenant.us.auth0.com/api/v2/email-templates/verify");
            Assert.AreEqual("tenant.us.auth0.com PATCH /api/v2/email-templates/verify", Auth0RatePacer.EndpointKeyFor(request));
        }

        [TestMethod]
        public void EndpointKeyFor_DistinguishesMethods()
        {
            var get = new HttpRequestMessage(HttpMethod.Get, "https://tenant.us.auth0.com/api/v2/clients");
            var post = new HttpRequestMessage(HttpMethod.Post, "https://tenant.us.auth0.com/api/v2/clients");
            Assert.AreNotEqual(Auth0RatePacer.EndpointKeyFor(get), Auth0RatePacer.EndpointKeyFor(post));
        }

    }

}
