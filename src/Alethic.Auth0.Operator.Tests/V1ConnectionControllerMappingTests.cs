using System.Linq;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1ConnectionControllerMappingTests
    {

        [TestMethod]
        public void FromApi_Null_ReturnsNull()
        {
            Assert.IsNull(V1ConnectionController.FromApi((GetConnectionResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_Connection_MapsScalarProperties()
        {
            var source = new GetConnectionResponseContent
            {
                Name = "test-conn",
                DisplayName = "Test Connection",
                Strategy = "auth0",
                Realms = ["realm1", "realm2"],
                IsDomainConnection = true,
                ShowAsButton = false,
            };

            var result = V1ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("test-conn", result.Name);
            Assert.AreEqual("Test Connection", result.DisplayName);
            Assert.AreEqual("auth0", result.Strategy);
            CollectionAssert.AreEqual(new[] { "realm1", "realm2" }, result.Realms);
            Assert.AreEqual(true, result.IsDomainConnection);
            Assert.AreEqual(false, result.ShowAsButton);
        }

        [TestMethod]
        public void FromApi_Connection_EnabledClientsIsNull()
        {
            var result = V1ConnectionController.FromApi(new GetConnectionResponseContent { Name = "x", Strategy = "auth0" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.EnabledClients);
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategyOptions_AllStrategySpecificPropertiesNull()
        {
            var result = V1ConnectionController.FromApi(new GetConnectionResponseContent { Name = "x", Strategy = "auth0" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Auth0Options);
            Assert.IsNull(result.OidcOptions);
            Assert.IsNull(result.Metadata);
        }

        [TestMethod]
        public void FromApi_Connection_MapsMetadata()
        {
            var source = new GetConnectionResponseContent
            {
                Name = "x",
                Strategy = "auth0",
                Metadata = new System.Collections.Generic.Dictionary<string, string> { ["env"] = "prod" },
            };

            var result = V1ConnectionController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Metadata);
            Assert.AreEqual("prod", result.Metadata["env"]?.ToString());
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategy_MapsNull()
        {
            var result = V1ConnectionController.FromApi(new GetConnectionResponseContent { Name = "no-strat" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Strategy);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsName()
        {
            var conf = new V1ConnectionConf { Name = "my-conn" };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "placeholder" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("my-conn", req.Name);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsDisplayName()
        {
            var conf = new V1ConnectionConf { DisplayName = "My Conn" };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("My Conn", req.DisplayName);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsRealms()
        {
            var conf = new V1ConnectionConf { Realms = ["r1"] };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V1ConnectionController.ApplyToApi(conf, req);
            CollectionAssert.AreEqual(new[] { "r1" }, req.Realms?.ToList());
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsIsDomainConnection()
        {
            var conf = new V1ConnectionConf { IsDomainConnection = true };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.IsDomainConnection);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsShowAsButton()
        {
            var conf = new V1ConnectionConf { ShowAsButton = true };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.ShowAsButton);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_NullFieldsLeaveTargetUnchanged()
        {
            var conf = new V1ConnectionConf();
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "original" };
            V1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("original", req.Name);
        }

        [TestMethod]
        public void Roundtrip_ScalarProperties()
        {
            var source = new GetConnectionResponseContent
            {
                Name = "roundtrip",
                DisplayName = "Roundtrip",
                Strategy = "auth0",
                IsDomainConnection = false,
                ShowAsButton = true,
            };

            var conf = V1ConnectionController.FromApi(source)!;
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(conf.Strategy!), Name = conf.Name! };
            V1ConnectionController.ApplyToApi(conf, req);

            Assert.AreEqual(source.Name, req.Name);
            Assert.AreEqual(source.DisplayName, req.DisplayName);
            Assert.AreEqual(source.IsDomainConnection, req.IsDomainConnection);
            Assert.AreEqual(source.ShowAsButton, req.ShowAsButton);
        }

    }

}
