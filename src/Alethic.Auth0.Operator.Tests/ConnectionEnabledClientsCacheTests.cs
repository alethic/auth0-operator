using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Controllers;

using Auth0.ManagementApi;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Coverage for the enabled-clients cache that keeps ConnectionClient reconciles from independently fetching
    /// <c>GET /api/v2/connections/{id}/clients</c> — Auth0 places connection reads in a tightly limited rate limit
    /// bucket, and the sweep-time fan-out of that call is what drained it. The tests run the real Management SDK
    /// client over a counting handler so the response parsing contract is pinned too.
    /// </summary>
    [TestClass]
    public class ConnectionEnabledClientsCacheTests
    {

        /// <summary>
        /// Responds to enabled-clients GETs with a fixed client list and to enablement PATCHes with 204, counting
        /// requests per method.
        /// </summary>
        sealed class CountingHandler : HttpMessageHandler
        {

            readonly string _body;

            public CountingHandler(params string[] clientIds)
            {
                var items = new List<string>();
                foreach (var clientId in clientIds)
                    items.Add($"{{\"client_id\":\"{clientId}\"}}");

                _body = $"{{\"clients\":[{string.Join(',', items)}],\"next\":null}}";
            }

            public int GetCount { get; private set; }

            public int UpdateCount { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request.Method == HttpMethod.Get)
                {
                    GetCount++;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_body, Encoding.UTF8, "application/json") });
                }

                UpdateCount++;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

        }

        static ManagementApiClient CreateClient(CountingHandler handler) =>
            new ManagementApiClient("test-token", new ClientOptions
            {
                BaseUrl = "https://tenant.example.com/api/v2/",
                HttpClient = new HttpClient(handler),
                MaxRetries = 0,
            });

        [TestMethod]
        public async Task RepeatedLookups_ShareOneFetchPerConnection()
        {
            var handler = new CountingHandler("client-a", "client-b");
            var api = CreateClient(handler);
            var cache = new ConnectionEnabledClientsCache(new MemoryCache(new MemoryCacheOptions()));

            Assert.IsTrue(await cache.IsClientEnabledAsync(api, "con_1", "client-a", CancellationToken.None));
            Assert.IsTrue(await cache.IsClientEnabledAsync(api, "con_1", "client-b", CancellationToken.None));
            Assert.IsFalse(await cache.IsClientEnabledAsync(api, "con_1", "client-c", CancellationToken.None));

            Assert.AreEqual(1, handler.GetCount, "every lookup for the same connection must be served by one fetch.");
        }

        [TestMethod]
        public async Task SetClientEnabled_InvalidatesTheConnectionsEntry()
        {
            var handler = new CountingHandler("client-a");
            var api = CreateClient(handler);
            var cache = new ConnectionEnabledClientsCache(new MemoryCache(new MemoryCacheOptions()));

            Assert.IsTrue(await cache.IsClientEnabledAsync(api, "con_1", "client-a", CancellationToken.None));
            await cache.SetClientEnabledAsync(api, "con_1", "client-b", true, CancellationToken.None);
            await cache.IsClientEnabledAsync(api, "con_1", "client-a", CancellationToken.None);

            Assert.AreEqual(1, handler.UpdateCount);
            Assert.AreEqual(2, handler.GetCount, "a write through the cache must invalidate the connection's entry so the next lookup refetches.");
        }

        [TestMethod]
        public async Task DifferentConnections_FetchIndependently()
        {
            var handler = new CountingHandler("client-a");
            var api = CreateClient(handler);
            var cache = new ConnectionEnabledClientsCache(new MemoryCache(new MemoryCacheOptions()));

            await cache.IsClientEnabledAsync(api, "con_1", "client-a", CancellationToken.None);
            await cache.IsClientEnabledAsync(api, "con_2", "client-a", CancellationToken.None);

            Assert.AreEqual(2, handler.GetCount);
        }

        [TestMethod]
        public async Task DifferentApiClients_FetchIndependently()
        {
            // the api client instance is per tenant, so two tenants using the same connection id must not share
            // an entry
            var handlerA = new CountingHandler("client-a");
            var handlerB = new CountingHandler("client-a");
            var cache = new ConnectionEnabledClientsCache(new MemoryCache(new MemoryCacheOptions()));

            await cache.IsClientEnabledAsync(CreateClient(handlerA), "con_1", "client-a", CancellationToken.None);
            await cache.IsClientEnabledAsync(CreateClient(handlerB), "con_1", "client-a", CancellationToken.None);

            Assert.AreEqual(1, handlerA.GetCount);
            Assert.AreEqual(1, handlerB.GetCount);
        }

    }

}
