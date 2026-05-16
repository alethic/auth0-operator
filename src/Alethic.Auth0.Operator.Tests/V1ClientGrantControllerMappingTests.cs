using System.Collections.Generic;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V1;

using Auth0.ManagementApi.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1ClientGrantControllerMappingTests
    {

        // ──────────────────────── FromApi OrganizationUsage ──────────────────────

        [TestMethod]
        [DataRow(OrganizationUsage.Deny, V1ClientGrantOrganizationUsage.Deny)]
        [DataRow(OrganizationUsage.Allow, V1ClientGrantOrganizationUsage.Allow)]
        [DataRow(OrganizationUsage.Require, V1ClientGrantOrganizationUsage.Require)]
        public void FromApi_OrganizationUsage_MapsCorrectly(OrganizationUsage input, V1ClientGrantOrganizationUsage expected)
        {
            Assert.AreEqual(expected, V1ClientGrantController.FromApi((OrganizationUsage?)input));
        }

        [TestMethod]
        public void FromApi_OrganizationUsage_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientGrantController.FromApi((OrganizationUsage?)null));
        }

        // ──────────────────────── FromApi ClientGrant ────────────────────────────

        [TestMethod]
        public void FromApi_ClientGrant_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientGrantController.FromApi((ClientGrant?)null));
        }

        [TestMethod]
        public void FromApi_ClientGrant_MapsScalarProperties()
        {
            var source = new ClientGrant
            {
                AllowAnyOrganization = true,
                OrganizationUsage = OrganizationUsage.Allow,
                Scope = new List<string> { "read:users", "write:users" },
            };

            var result = V1ClientGrantController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.AllowAnyOrganization);
            Assert.AreEqual(V1ClientGrantOrganizationUsage.Allow, result.OrganizationUsage);
            CollectionAssert.AreEqual(new[] { "read:users", "write:users" }, result.Scope);
        }

        [TestMethod]
        public void FromApi_ClientGrant_NullScope_ProducesNullScope()
        {
            var source = new ClientGrant { Scope = null };
            var result = V1ClientGrantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Scope);
        }

        [TestMethod]
        public void FromApi_ClientGrant_ClientRef_IsNull()
        {
            var source = new ClientGrant { ClientId = "some-client-id" };
            var result = V1ClientGrantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.ClientRef);
        }

        [TestMethod]
        public void FromApi_ClientGrant_Audience_IsNull()
        {
            var source = new ClientGrant { Audience = "https://api.example.com/" };
            var result = V1ClientGrantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Audience);
        }

        // ──────────────────────── ToApi OrganizationUsage ────────────────────────

        [TestMethod]
        [DataRow(V1ClientGrantOrganizationUsage.Deny, OrganizationUsage.Deny)]
        [DataRow(V1ClientGrantOrganizationUsage.Allow, OrganizationUsage.Allow)]
        [DataRow(V1ClientGrantOrganizationUsage.Require, OrganizationUsage.Require)]
        public void ToApi_OrganizationUsage_MapsCorrectly(V1ClientGrantOrganizationUsage input, OrganizationUsage expected)
        {
            Assert.AreEqual(expected, V1ClientGrantController.ToApi((V1ClientGrantOrganizationUsage?)input));
        }

        [TestMethod]
        public void ToApi_OrganizationUsage_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientGrantController.ToApi((V1ClientGrantOrganizationUsage?)null));
        }

        // ──────────────────────── Roundtrip OrganizationUsage ────────────────────

        [TestMethod]
        [DataRow(V1ClientGrantOrganizationUsage.Deny)]
        [DataRow(V1ClientGrantOrganizationUsage.Allow)]
        [DataRow(V1ClientGrantOrganizationUsage.Require)]
        public void Roundtrip_OrganizationUsage(V1ClientGrantOrganizationUsage value)
        {
            var api = V1ClientGrantController.ToApi((V1ClientGrantOrganizationUsage?)value);
            var back = V1ClientGrantController.FromApi(api);
            Assert.AreEqual(value, back);
        }

        // ──────────────────────── ApplyToApi ─────────────────────────────────────

        [TestMethod]
        public void ApplyToApi_CreateRequest_MapsAllFields()
        {
            var conf = new V1ClientGrantConf
            {
                Scope = ["read:users", "write:users"],
                AllowAnyOrganization = false,
                OrganizationUsage = V1ClientGrantOrganizationUsage.Deny,
            };

            var req = new ClientGrantCreateRequest();
            V1ClientGrantController.ApplyToApi(conf, req);

            CollectionAssert.AreEqual(new[] { "read:users", "write:users" }, req.Scope);
            Assert.AreEqual(false, req.AllowAnyOrganization);
            Assert.AreEqual(OrganizationUsage.Deny, req.OrganizationUsage);
        }

        [TestMethod]
        public void ApplyToApi_UpdateRequest_MapsAllFields()
        {
            var conf = new V1ClientGrantConf
            {
                Scope = ["openid"],
                AllowAnyOrganization = true,
                OrganizationUsage = V1ClientGrantOrganizationUsage.Require,
            };

            var req = new ClientGrantUpdateRequest();
            V1ClientGrantController.ApplyToApi(conf, req);

            CollectionAssert.AreEqual(new[] { "openid" }, req.Scope);
            Assert.AreEqual(true, req.AllowAnyOrganization);
            Assert.AreEqual(OrganizationUsage.Require, req.OrganizationUsage);
        }

        [TestMethod]
        public void ApplyToApi_NullOrganizationUsage_LeavesNull()
        {
            var conf = new V1ClientGrantConf { OrganizationUsage = null };
            var req = new ClientGrantUpdateRequest();
            V1ClientGrantController.ApplyToApi(conf, req);
            Assert.IsNull(req.OrganizationUsage);
        }

    }

}
