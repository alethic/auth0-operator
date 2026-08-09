using System.Collections.Generic;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3;
using Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Tests for the drift detection that decides whether a reconcile cycle issues an update to Auth0.
    /// </summary>
    [TestClass]
    public class NeedsUpdateTests
    {

        // ──────────────────────── ConfDiff ────────────────────────────────────────

        class Sample
        {
            public string? Name { get; set; }
            public string[]? Items { get; set; }
            public Sample? Nested { get; set; }
        }

        [TestMethod]
        public void ConfDiff_NullDesired_IsSubset()
        {
            Assert.IsTrue(ConfDiff.IsSubsetOf(null, new Sample()));
        }

        [TestMethod]
        public void ConfDiff_NullActual_IsNotSubset()
        {
            Assert.IsFalse(ConfDiff.IsSubsetOf(new Sample(), null));
        }

        [TestMethod]
        public void ConfDiff_UnsetDesiredProperties_AreIgnored()
        {
            Assert.IsTrue(ConfDiff.IsSubsetOf(new Sample { Name = "a" }, new Sample { Name = "a", Items = ["x"] }));
        }

        [TestMethod]
        public void ConfDiff_MissingActualProperty_IsNotSubset()
        {
            Assert.IsFalse(ConfDiff.IsSubsetOf(new Sample { Items = ["x"] }, new Sample { Name = "a" }));
        }

        [TestMethod]
        public void ConfDiff_DifferentScalar_IsNotSubset()
        {
            Assert.IsFalse(ConfDiff.IsSubsetOf(new Sample { Name = "a" }, new Sample { Name = "b" }));
        }

        [TestMethod]
        public void ConfDiff_NestedSubset_IsSubset()
        {
            var desired = new Sample { Nested = new Sample { Name = "a" } };
            var actual = new Sample { Name = "outer", Nested = new Sample { Name = "a", Items = ["extra"] } };
            Assert.IsTrue(ConfDiff.IsSubsetOf(desired, actual));
        }

        [TestMethod]
        public void ConfDiff_NestedMismatch_IsNotSubset()
        {
            var desired = new Sample { Nested = new Sample { Name = "a" } };
            var actual = new Sample { Nested = new Sample { Name = "b" } };
            Assert.IsFalse(ConfDiff.IsSubsetOf(desired, actual));
        }

        [TestMethod]
        public void ConfDiff_ArrayLengthMismatch_IsNotSubset()
        {
            Assert.IsFalse(ConfDiff.IsSubsetOf(new Sample { Items = ["x"] }, new Sample { Items = ["x", "y"] }));
        }

        [TestMethod]
        public void ConfDiff_ArrayElementsSubset_IsSubset()
        {
            var desired = new Sample { Items = ["x", "y"] };
            var actual = new Sample { Items = ["x", "y"] };
            Assert.IsTrue(ConfDiff.IsSubsetOf(desired, actual));
        }

        [TestMethod]
        public void ConfDiff_ExcludedProperty_IsIgnored()
        {
            Assert.IsTrue(ConfDiff.IsSubsetOf(new Sample { Name = "a" }, new Sample { Name = "b" }, "Name"));
        }

        [TestMethod]
        public void ConfDiff_DeepEquals_DetectsRemovedProperty()
        {
            // unlike IsSubsetOf, DeepEquals reports a difference when the actual has extra content
            Assert.IsFalse(ConfDiff.DeepEquals(new Sample { Name = "a" }, new Sample { Name = "a", Items = ["x"] }));
            Assert.IsTrue(ConfDiff.DeepEquals(new Sample { Name = "a" }, new Sample { Name = "a" }));
        }

        // ──────────────────────── Client ──────────────────────────────────────────

        [TestMethod]
        public void Client_IdenticalConf_NoUpdate()
        {
            var conf = new V2alpha3ClientConf { Name = "app", GrantTypes = ["authorization_code"] };
            var last = new V2alpha3ClientConf { Name = "app", GrantTypes = ["authorization_code"], Description = "auth0 side" };

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void Client_ChangedName_RequiresUpdate()
        {
            var conf = new V2alpha3ClientConf { Name = "new" };
            var last = new V2alpha3ClientConf { Name = "old" };

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void Client_RemovedMetadataKey_RequiresUpdate()
        {
            // a key present in Auth0 but absent from the spec must be tombstoned on the wire
            var conf = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["keep"] = "1" } };
            var last = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["keep"] = "1", ["stale"] = "2" } };

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void Client_SameMetadata_NoUpdate()
        {
            var conf = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["keep"] = "1" } };
            var last = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["keep"] = "1" } };

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        // ──────────────────────── ClientGrant ─────────────────────────────────────

        [TestMethod]
        public void ClientGrant_IdenticalScope_NoUpdate()
        {
            // refs come back from Auth0 in resolved form and must not force an update
            var conf = new V2alpha3ClientGrantConf { Scope = ["read", "write"], ClientRef = new V1ClientReference { Name = "my-client" } };
            var last = new V2alpha3ClientGrantConf { Scope = ["write", "read"], ClientRef = new V1ClientReference { Id = "abc" }, Audience = new V1ResourceServerReference { Identifier = "https://api" } };

            Assert.IsFalse(V2alpha3ClientGrantController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ClientGrant_ChangedScope_RequiresUpdate()
        {
            var conf = new V2alpha3ClientGrantConf { Scope = ["read", "admin"] };
            var last = new V2alpha3ClientGrantConf { Scope = ["read"] };

            Assert.IsTrue(V2alpha3ClientGrantController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ClientGrant_ChangedOrganizationUsage_RequiresUpdate()
        {
            var conf = new V2alpha3ClientGrantConf { Scope = ["read"], OrganizationUsage = V2alpha3ClientGrantOrganizationUsage.Require };
            var last = new V2alpha3ClientGrantConf { Scope = ["read"], OrganizationUsage = V2alpha3ClientGrantOrganizationUsage.Allow };

            Assert.IsTrue(V2alpha3ClientGrantController.ConfRequiresUpdate(conf, last));
        }

        // ──────────────────────── Role ────────────────────────────────────────────

        [TestMethod]
        public void Role_IdenticalNameAndDescription_NoUpdate()
        {
            // permissions are reconciled separately and must not force the PATCH
            var conf = new V2alpha3RoleConf { Name = "admin", Description = "Admin", Permissions = [new V2alpha3RolePermission { Name = "read", ResourceServerRef = new V1ResourceServerReference { Name = "api" } }] };
            var last = new V2alpha3RoleConf { Name = "admin", Description = "Admin", Permissions = [new V2alpha3RolePermission { Name = "read", ResourceServerRef = new V1ResourceServerReference { Identifier = "https://api" } }] };

            Assert.IsFalse(V2alpha3RoleController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void Role_ChangedDescription_RequiresUpdate()
        {
            var conf = new V2alpha3RoleConf { Name = "admin", Description = "New" };
            var last = new V2alpha3RoleConf { Name = "admin", Description = "Old" };

            Assert.IsTrue(V2alpha3RoleController.ConfRequiresUpdate(conf, last));
        }

        // ──────────────────────── CustomDomain ────────────────────────────────────

        [TestMethod]
        public void CustomDomain_OnlyMutableFieldsConsidered_NoUpdate()
        {
            // domain/type/verificationMethod are create-only and primary is read-only
            var conf = new V2alpha3CustomDomainConf { Domain = "login.example.com", TlsPolicy = "recommended" };
            var last = new V2alpha3CustomDomainConf { Domain = "other.example.com", TlsPolicy = "recommended", Primary = true };

            Assert.IsFalse(V2alpha3CustomDomainController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void CustomDomain_ChangedTlsPolicy_RequiresUpdate()
        {
            var conf = new V2alpha3CustomDomainConf { TlsPolicy = "compatible" };
            var last = new V2alpha3CustomDomainConf { TlsPolicy = "recommended" };

            Assert.IsTrue(V2alpha3CustomDomainController.ConfRequiresUpdate(conf, last));
        }

        // ──────────────────────── ResourceServer ──────────────────────────────────

        [TestMethod]
        public void ResourceServer_IdenticalConf_NoUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { Identifier = "https://api", Name = "api", TokenLifetime = 3600 };
            var last = new V2alpha3ResourceServerConf { Id = "rs_123", Identifier = "https://api", Name = "api", TokenLifetime = 3600, SigningSecret = "auth0-generated" };

            Assert.IsFalse(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_UntransmittableFields_DoNotRequireUpdate()
        {
            // tokenLifetimeForWeb is in the model but never sent by the update request; a skew there
            // can never be corrected by updating, so it must not cause one
            var conf = new V2alpha3ResourceServerConf { Name = "api", TokenLifetimeForWeb = 1200 };
            var last = new V2alpha3ResourceServerConf { Name = "api", TokenLifetimeForWeb = 900 };

            Assert.IsFalse(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_ChangedTokenLifetime_RequiresUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { Name = "api", TokenLifetime = 7200 };
            var last = new V2alpha3ResourceServerConf { Name = "api", TokenLifetime = 3600 };

            Assert.IsTrue(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

    }

}
