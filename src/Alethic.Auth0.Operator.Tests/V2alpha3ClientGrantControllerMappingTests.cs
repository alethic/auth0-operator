using System.Collections.Generic;

using System.Linq;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V2alpha3ClientGrantControllerMappingTests
    {

        // ──────────────────────── FromApi OrganizationUsage ──────────────────────

        [TestMethod]
        public void FromApi_OrganizationUsage_Deny()
        {
            Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Deny, V2alpha3ClientGrantController.FromApi(new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Deny)));
        }

        [TestMethod]
        public void FromApi_OrganizationUsage_Allow()
        {
            Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Allow, V2alpha3ClientGrantController.FromApi(new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Allow)));
        }

        [TestMethod]
        public void FromApi_OrganizationUsage_Require() => Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Require, V2alpha3ClientGrantController.FromApi(new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Require)));

        [TestMethod]
        public void FromApi_OrganizationUsage_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ClientGrantController.FromApi((ClientGrantOrganizationUsageEnum?)null));
        }

        // ──────────────────────── FromApi ClientGrant ────────────────────────────

        [TestMethod]
        public void FromApi_ClientGrant_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ClientGrantController.FromApi((GetClientGrantResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_ClientGrant_MapsScalarProperties()
        {
            var source = new GetClientGrantResponseContent
            {
                AllowAnyOrganization = true,
                OrganizationUsage = new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Allow),
                Scope = new List<string> { "read:users", "write:users" },
            };

            var result = V2alpha3ClientGrantController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.AllowAnyOrganization);
            Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Allow, result.OrganizationUsage);
            CollectionAssert.AreEqual(new[] { "read:users", "write:users" }, result.Scope);
        }

        [TestMethod]
        public void FromApi_ClientGrant_NullScope_ProducesNullScope()
        {
            var source = new GetClientGrantResponseContent { Scope = null };
            var result = V2alpha3ClientGrantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Scope);
        }

        [TestMethod]
        public void FromApi_ClientGrant_ClientRef_IsMapped()
        {
            var source = new GetClientGrantResponseContent { ClientId = "some-client-id" };
            var result = V2alpha3ClientGrantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ClientRef);
            Assert.AreEqual("some-client-id", result.ClientRef.Id);
        }

        [TestMethod]
        public void FromApi_ClientGrant_Audience_IsMapped()
        {
            var source = new GetClientGrantResponseContent { Audience = "https://api.example.com/" };
            var result = V2alpha3ClientGrantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Audience);
            Assert.AreEqual("https://api.example.com/", result.Audience.Identifier);
        }

        // ──────────────────────── ToApi OrganizationUsage ────────────────────────

        [TestMethod]
        public void ToApi_OrganizationUsage_Deny() => Assert.AreEqual(ClientGrantOrganizationUsageEnum.Values.Deny, V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)V2alpha3ClientGrantOrganizationUsage.Deny)?.Value);
        [TestMethod]
        public void ToApi_OrganizationUsage_Allow() => Assert.AreEqual(ClientGrantOrganizationUsageEnum.Values.Allow, V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)V2alpha3ClientGrantOrganizationUsage.Allow)?.Value);
        [TestMethod]
        public void ToApi_OrganizationUsage_Require() => Assert.AreEqual(ClientGrantOrganizationUsageEnum.Values.Require, V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)V2alpha3ClientGrantOrganizationUsage.Require)?.Value);

        [TestMethod]
        public void ToApi_OrganizationUsage_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)null));
        }

        // ──────────────────────── Roundtrip OrganizationUsage ────────────────────

        [TestMethod]
        public void Roundtrip_OrganizationUsage_Deny()
        {
            var api = V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)V2alpha3ClientGrantOrganizationUsage.Deny); var back = V2alpha3ClientGrantController.FromApi(api); Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Deny, back);
        }
        [TestMethod]
        public void Roundtrip_OrganizationUsage_Allow()
        {
            var api = V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)V2alpha3ClientGrantOrganizationUsage.Allow); var back = V2alpha3ClientGrantController.FromApi(api); Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Allow, back);
        }
        [TestMethod]
        public void Roundtrip_OrganizationUsage_Require()
        {
            var api = V2alpha3ClientGrantController.ToApi((V2alpha3ClientGrantOrganizationUsage?)V2alpha3ClientGrantOrganizationUsage.Require); var back = V2alpha3ClientGrantController.FromApi(api); Assert.AreEqual(V2alpha3ClientGrantOrganizationUsage.Require, back);
        }

        // ──────────────────────── ApplyToApi ─────────────────────────────────────

        [TestMethod]
        public void ApplyToApi_CreateRequest_MapsAllFields()
        {
            var conf = new V2alpha3ClientGrantConf
            {
                Scope = ["read:users", "write:users"],
                AllowAnyOrganization = false,
                OrganizationUsage = V2alpha3ClientGrantOrganizationUsage.Deny,
            };

            var req = new CreateClientGrantRequestContent { ClientId = "client-id", Audience = "https://api.example.com" };
            V2alpha3ClientGrantController.ApplyToApi(conf, req);

            CollectionAssert.AreEqual(new[] { "read:users", "write:users" }, req.Scope?.ToArray());
            Assert.AreEqual(false, req.AllowAnyOrganization);
            Assert.AreEqual(ClientGrantOrganizationUsageEnum.Values.Deny, req.OrganizationUsage?.Value);
        }

        [TestMethod]
        public void ApplyToApi_UpdateRequest_MapsAllFields()
        {
            var conf = new V2alpha3ClientGrantConf
            {
                Scope = ["openid"],
                AllowAnyOrganization = true,
                OrganizationUsage = V2alpha3ClientGrantOrganizationUsage.Require,
            };

            var req = new UpdateClientGrantRequestContent();
            V2alpha3ClientGrantController.ApplyToApi(conf, req);

            CollectionAssert.AreEqual(new[] { "openid" }, req.Scope.Value?.ToArray());
            Assert.AreEqual(true, req.AllowAnyOrganization.Value);
            Assert.AreEqual(ClientGrantOrganizationNullableUsageEnum.Values.Require, req.OrganizationUsage.Value?.Value);
        }

        [TestMethod]
        public void ApplyToApi_NullOrganizationUsage_LeavesNull()
        {
            var conf = new V2alpha3ClientGrantConf { OrganizationUsage = null };
            var req = new UpdateClientGrantRequestContent();
            V2alpha3ClientGrantController.ApplyToApi(conf, req);
            Assert.IsFalse(req.OrganizationUsage.IsDefined);
        }

    }

}
