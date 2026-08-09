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

        static HttpResponseMessage Response(long? remaining = null, long? resetEpochSeconds = null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (remaining is { } r)
                response.Headers.Add("x-ratelimit-remaining", r.ToString());
            if (resetEpochSeconds is { } s)
                response.Headers.Add("x-ratelimit-reset", s.ToString());
            return response;
        }

        [TestMethod]
        public void NoRecordedBudget_NoDelay()
        {
            var pacer = new Auth0RatePacer(new PacingOptions());
            Assert.AreEqual(TimeSpan.Zero, pacer.GetDelay("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void BudgetAboveThreshold_NoDelay()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com", Response(remaining: 50, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.Zero, pacer.GetDelay("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void BudgetAtThreshold_SpreadsRemainingUntilReset()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromMinutes(1) }, time);

            // 10 requests left, 60 seconds until refill: pace at one request per 6 seconds
            pacer.Record("tenant.us.auth0.com", Response(remaining: 10, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.FromSeconds(6), pacer.GetDelay("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void ExhaustedBudget_DelayCappedAtMaxRequestDelay()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10, MaxRequestDelay = TimeSpan.FromSeconds(10) }, time);

            // 0 remaining with a 60s reset would want a 60s delay; the cap bounds it
            pacer.Record("tenant.us.auth0.com", Response(remaining: 0, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.FromSeconds(10), pacer.GetDelay("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void PastReset_NoDelay()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com", Response(remaining: 1, resetEpochSeconds: 60));
            time.Advance(TimeSpan.FromSeconds(61));

            Assert.AreEqual(TimeSpan.Zero, pacer.GetDelay("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void BudgetIsPerHost()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("a.us.auth0.com", Response(remaining: 1, resetEpochSeconds: 60));

            Assert.AreNotEqual(TimeSpan.Zero, pacer.GetDelay("a.us.auth0.com"));
            Assert.AreEqual(TimeSpan.Zero, pacer.GetDelay("b.us.auth0.com"));
        }

        [TestMethod]
        public void ResponseWithoutHeaders_Ignored()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com", Response(remaining: 1, resetEpochSeconds: 60));
            pacer.Record("tenant.us.auth0.com", Response());

            // the headerless response must not clobber the recorded budget
            Assert.AreNotEqual(TimeSpan.Zero, pacer.GetDelay("tenant.us.auth0.com"));
        }

        [TestMethod]
        public void NewerBudget_ReplacesOlder()
        {
            var time = new FakeTimeProvider(DateTimeOffset.UnixEpoch);
            var pacer = new Auth0RatePacer(new PacingOptions { RemainingThreshold = 10 }, time);

            pacer.Record("tenant.us.auth0.com", Response(remaining: 1, resetEpochSeconds: 60));
            pacer.Record("tenant.us.auth0.com", Response(remaining: 50, resetEpochSeconds: 60));

            Assert.AreEqual(TimeSpan.Zero, pacer.GetDelay("tenant.us.auth0.com"));
        }

    }

}
