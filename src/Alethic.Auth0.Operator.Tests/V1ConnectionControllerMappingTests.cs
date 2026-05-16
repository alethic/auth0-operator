using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;

using Auth0.ManagementApi.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1ConnectionControllerMappingTests
    {

        [TestMethod]
        public void FromApi_Null_ReturnsNull()
        {
            Assert.IsNull(V1ConnectionController.FromApi((Connection?)null));
        }

        [TestMethod]
        public void FromApi_Connection_MapsScalarProperties()
        {
            var source = new Connection
            {
                Name = "test-conn",
                DisplayName = "Test Connection",
                Strategy = "auth0",
                Realms = ["realm1", "realm2"],
                IsDomainConnection = true,
                ShowAsButton = false,
                ProvisioningTicketUrl = "https://example.com/ticket",
            };

            var result = V1ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("test-conn", result.Name);
            Assert.AreEqual("Test Connection", result.DisplayName);
            Assert.AreEqual("auth0", result.Strategy);
            CollectionAssert.AreEqual(new[] { "realm1", "realm2" }, result.Realms);
            Assert.AreEqual(true, result.IsDomainConnection);
            Assert.AreEqual(false, result.ShowAsButton);
            Assert.AreEqual("https://example.com/ticket", result.ProvisioningTicketUrl);
        }

        [TestMethod]
        public void FromApi_Connection_EnabledClientsIsNull()
        {
            var result = V1ConnectionController.FromApi(new Connection { Name = "x", Strategy = "auth0" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.EnabledClients);
        }

        [TestMethod]
        public void FromApi_Connection_NullOptionsAndMetadata_MapsNull()
        {
            var result = V1ConnectionController.FromApi(new Connection { Name = "x", Strategy = "auth0" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Options);
            Assert.IsNull(result.Metadata);
        }

        [TestMethod]
        public void FromApiOptions_Null_ReturnsNull()
        {
            Assert.IsNull(V1ConnectionController.FromApiOptions(null, "auth0"));
            Assert.IsNull(V1ConnectionController.FromApiOptions(null, "oidc"));
            Assert.IsNull(V1ConnectionController.FromApiOptions(null, null));
        }

        [TestMethod]
        public void FromApiOptions_Auth0Strategy_MapsTypedProperties()
        {
            var result = V1ConnectionController.FromApiOptions(new { passwordPolicy = "good" }, "auth0");
            Assert.IsNotNull(result);
            Assert.AreEqual(V1ConnectionOptionsPasswordPolicy.Good, result.PasswordPolicy);
            Assert.IsNull(result.AdditionalProperties);
        }

        [TestMethod]
        public void FromApiOptions_OtherStrategy_CapturesAsAdditionalProperties()
        {
            var result = V1ConnectionController.FromApiOptions(new { clientId = "abc", tenant = "mytenant" }, "oidc");
            Assert.IsNotNull(result);
            Assert.IsNull(result.PasswordPolicy);
            Assert.IsNotNull(result.AdditionalProperties);
            Assert.AreEqual("abc", result.AdditionalProperties["clientId"]?.ToString());
            Assert.AreEqual("mytenant", result.AdditionalProperties["tenant"]?.ToString());
        }

        [TestMethod]
        public void FromApiOptions_NullStrategy_CapturesAsAdditionalProperties()
        {
            var result = V1ConnectionController.FromApiOptions(new { foo = "bar" }, null);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.AdditionalProperties);
            Assert.AreEqual("bar", result.AdditionalProperties["foo"]?.ToString());
        }

        [TestMethod]
        public void FromApi_Connection_Auth0Strategy_MapsOptionsTyped()
        {
            var source = new Connection
            {
                Name = "x",
                Strategy = "auth0",
                Options = new { passwordPolicy = "good" },
            };

            var result = V1ConnectionController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Options);
            Assert.AreEqual(V1ConnectionOptionsPasswordPolicy.Good, result.Options.PasswordPolicy);
            Assert.IsNull(result.Options.AdditionalProperties);
        }

        [TestMethod]
        public void FromApi_Connection_OtherStrategy_MapsOptionsAsAdditionalProperties()
        {
            var source = new Connection
            {
                Name = "x",
                Strategy = "oidc",
                Options = new { clientId = "abc" },
            };

            var result = V1ConnectionController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Options);
            Assert.IsNull(result.Options.PasswordPolicy);
            Assert.IsNotNull(result.Options.AdditionalProperties);
            Assert.AreEqual("abc", result.Options.AdditionalProperties["clientId"]?.ToString());
        }

        [TestMethod]
        public void FromApi_Connection_MapsMetadata()
        {
            var source = new Connection
            {
                Name = "x",
                Strategy = "auth0",
                Metadata = new { env = "prod" },
            };

            var result = V1ConnectionController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Metadata);
            Assert.AreEqual("prod", result.Metadata["env"]?.ToString());
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategy_MapsNull()
        {
            var result = V1ConnectionController.FromApi(new Connection { Name = "no-strat" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Strategy);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsName()
        {
            var conf = new V1ConnectionConf { Name = "my-conn" };
            var req = new ConnectionCreateRequest { Strategy = "auth0" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("my-conn", req.Name);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsDisplayName()
        {
            var conf = new V1ConnectionConf { DisplayName = "My Conn" };
            var req = new ConnectionCreateRequest { Strategy = "auth0" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("My Conn", req.DisplayName);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsRealms()
        {
            var conf = new V1ConnectionConf { Realms = ["r1"] };
            var req = new ConnectionCreateRequest { Strategy = "auth0" };
            V1ConnectionController.ApplyToApi(conf, req);
            CollectionAssert.AreEqual(new[] { "r1" }, req.Realms);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsIsDomainConnection()
        {
            var conf = new V1ConnectionConf { IsDomainConnection = true };
            var req = new ConnectionCreateRequest { Strategy = "auth0" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.IsDomainConnection);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsShowAsButton()
        {
            var conf = new V1ConnectionConf { ShowAsButton = true };
            var req = new ConnectionCreateRequest { Strategy = "auth0" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.ShowAsButton);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_NullFieldsLeaveTargetUnchanged()
        {
            var conf = new V1ConnectionConf();
            var req = new ConnectionCreateRequest { Strategy = "auth0", Name = "original" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("original", req.Name);
        }

        [TestMethod]
        public void Roundtrip_ScalarProperties()
        {
            var source = new Connection
            {
                Name = "roundtrip",
                DisplayName = "Roundtrip",
                Strategy = "auth0",
                IsDomainConnection = false,
                ShowAsButton = true,
                ProvisioningTicketUrl = "https://ticket",
            };

            var conf = V1ConnectionController.FromApi(source)!;
            var req = new ConnectionCreateRequest { Strategy = conf.Strategy! };
            V1ConnectionController.ApplyToApi(conf, req);

            Assert.AreEqual(source.Name, req.Name);
            Assert.AreEqual(source.DisplayName, req.DisplayName);
            Assert.AreEqual(source.IsDomainConnection, req.IsDomainConnection);
            Assert.AreEqual(source.ShowAsButton, req.ShowAsButton);
        }

    }

}
