using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Regression coverage for the "not found" exception contract that every controller's Get/Find self-heal
    /// logic depends on. The Auth0 8.x Management SDK throws <see cref="NotFoundError"/> on a 404 — NOT the
    /// legacy <c>Auth0.Core.Exceptions.ErrorApiException</c> the controllers used to catch. Because a
    /// <c>catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)</c> never matched
    /// <see cref="NotFoundError"/>, a stale/missing remote id (e.g. a BrandingTheme whose Auth0 theme was
    /// deleted) escaped as an unhandled NotFoundError and the reconcile hard-failed, instead of returning null
    /// so the reconciler could clear status and recreate. These tests pin the SDK behaviour that makes
    /// <c>catch (NotFoundError)</c> correct.
    /// </summary>
    [TestClass]
    public class V2alpha3BrandingThemeControllerNotFoundTests
    {

        /// <summary>
        /// An <see cref="HttpMessageHandler"/> that always responds with a fixed status code and body, so the
        /// real generated Management SDK client runs its status-code-to-exception mapping.
        /// </summary>
        sealed class StaticResponseHandler : HttpMessageHandler
        {

            readonly HttpStatusCode _status;
            readonly string _body;

            public StaticResponseHandler(HttpStatusCode status, string body)
            {
                _status = status;
                _body = body;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
                Task.FromResult(new HttpResponseMessage(_status) { Content = new StringContent(_body, Encoding.UTF8, "application/json") });

        }

        static ManagementApiClient CreateClient(HttpStatusCode status, string body) =>
            new ManagementApiClient("test-token", new ClientOptions
            {
                BaseUrl = "https://tenant.example.com/api/v2/",
                HttpClient = new HttpClient(new StaticResponseHandler(status, body)),
                MaxRetries = 0,
            });

        [TestMethod]
        public async Task GetAsync_on_404_throws_NotFoundError()
        {
            var api = CreateClient(HttpStatusCode.NotFound, "{\"statusCode\":404,\"error\":\"Not Found\",\"message\":\"Theme not found\",\"errorCode\":\"inexistent_theme\"}");

            var threwNotFound = false;
            try
            {
                // this is exactly what the controller's Get(...) awaits; on a 404 it must throw NotFoundError so
                // the controller's catch (NotFoundError) returns null and the reconciler self-heals
                await api.Branding.Themes.GetAsync("nonexistent-theme-id");
            }
            catch (NotFoundError)
            {
                threwNotFound = true;
            }

            Assert.IsTrue(threwNotFound, "a 404 from the 8.x Management SDK must surface as NotFoundError (the type the controllers catch).");
        }

        [TestMethod]
        public void NotFoundError_is_a_ManagementApiException_but_not_the_legacy_ErrorApiException()
        {
            // ControllerBase's generic handler catches ManagementApiException, so every 8.x API error (including
            // NotFoundError) is turned into an ApiError event + retry rather than an unhandled crash
            Assert.IsTrue(
                typeof(ManagementApiException).IsAssignableFrom(typeof(NotFoundError)),
                "NotFoundError must be a ManagementApiException so ControllerBase's generic handler catches it.");

            // the original bug: the controllers caught the legacy Auth0.Core ErrorApiException, which NotFoundError
            // does not derive from, so the NotFound catch never fired
            var legacyErrorApiException = Type.GetType("Auth0.Core.Exceptions.ErrorApiException, Auth0.Core");
            if (legacyErrorApiException is not null)
                Assert.IsFalse(
                    legacyErrorApiException.IsAssignableFrom(typeof(NotFoundError)),
                    "NotFoundError must NOT derive from the legacy ErrorApiException; catching ErrorApiException silently misses 404s.");
        }

    }

}
