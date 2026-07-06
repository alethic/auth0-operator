using System.Linq;

using Alethic.Auth0.Operator.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V2alpha3ConnectionClientControllerMappingTests
    {

        // ──────────────────────── synthetic ID round-trip ────────────────────────

        [TestMethod]
        public void BuildId_ThenParse_RoundTrips()
        {
            var id = V2alpha3ConnectionClientController.BuildId("con_123", "client_abc");

            Assert.IsTrue(V2alpha3ConnectionClientController.TryParseId(id, out var connectionId, out var clientId));
            Assert.AreEqual("con_123", connectionId);
            Assert.AreEqual("client_abc", clientId);
        }

        [TestMethod]
        public void TryParseId_Null_ReturnsFalse()
        {
            Assert.IsFalse(V2alpha3ConnectionClientController.TryParseId(null, out _, out _));
        }

        [TestMethod]
        public void TryParseId_Empty_ReturnsFalse()
        {
            Assert.IsFalse(V2alpha3ConnectionClientController.TryParseId("", out _, out _));
        }

        [TestMethod]
        public void TryParseId_NoSeparator_ReturnsFalse()
        {
            Assert.IsFalse(V2alpha3ConnectionClientController.TryParseId("con_123", out _, out _));
        }

        [TestMethod]
        public void TryParseId_MissingClient_ReturnsFalse()
        {
            Assert.IsFalse(V2alpha3ConnectionClientController.TryParseId("con_123|", out _, out _));
        }

        [TestMethod]
        public void TryParseId_MissingConnection_ReturnsFalse()
        {
            Assert.IsFalse(V2alpha3ConnectionClientController.TryParseId("|client_abc", out _, out _));
        }

    }

}
