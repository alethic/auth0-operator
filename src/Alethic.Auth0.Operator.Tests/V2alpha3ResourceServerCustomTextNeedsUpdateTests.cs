using System.Collections.Generic;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.CustomText.V2alpha3;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Tests for the hand-written drift detection of the ResourceServer and CustomText controllers.
    /// </summary>
    [TestClass]
    public class V2alpha3ResourceServerCustomTextNeedsUpdateTests
    {

        // ──────────────────────── ResourceServer ──────────────────────────────────

        [TestMethod]
        public void ResourceServer_ChangedScopeElement_RequiresUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { Name = "api", Scopes = [new V2alpha3ResourceServerScope { Value = "read", Description = "Read access" }] };
            var last = new V2alpha3ResourceServerConf { Name = "api", Scopes = [new V2alpha3ResourceServerScope { Value = "read", Description = "Old description" }] };

            Assert.IsTrue(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_IdenticalScopes_NoUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { Name = "api", Scopes = [new V2alpha3ResourceServerScope { Value = "read", Description = "Read access" }, new V2alpha3ResourceServerScope { Value = "write", Description = "Write access" }] };
            var last = new V2alpha3ResourceServerConf { Name = "api", Scopes = [new V2alpha3ResourceServerScope { Value = "read", Description = "Read access" }, new V2alpha3ResourceServerScope { Value = "write", Description = "Write access" }] };

            Assert.IsFalse(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_ChangedTokenEncryptionLeaf_RequiresUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { Name = "api", TokenEncryption = new V2alpha3ResourceServerTokenEncryption { Format = V2alpha3ResourceServerTokenFormat.CompactNestedJwe, EncryptionKey = new V2alpha3ResourceServerTokenEncryptionKey { Name = "key", Pem = "-----BEGIN PUBLIC KEY-----new" } } };
            var last = new V2alpha3ResourceServerConf { Name = "api", TokenEncryption = new V2alpha3ResourceServerTokenEncryption { Format = V2alpha3ResourceServerTokenFormat.CompactNestedJwe, EncryptionKey = new V2alpha3ResourceServerTokenEncryptionKey { Name = "key", Pem = "-----BEGIN PUBLIC KEY-----old" } } };

            Assert.IsTrue(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_SubsetWithAuth0Extras_NoUpdate()
        {
            // Auth0 reports values the spec does not manage; unmanaged properties must not force an update
            var conf = new V2alpha3ResourceServerConf { Name = "api", TokenLifetime = 3600 };
            var last = new V2alpha3ResourceServerConf
            {
                Id = "rs_123",
                Identifier = "https://api",
                Name = "api",
                TokenLifetime = 3600,
                SigningSecret = "auth0-generated",
                SigningAlgorithm = V2alpha3ResourceServerSigningAlgorithm.RS256,
                AllowOfflineAccess = false,
                SkipConsentForVerifiableFirstPartyClients = true,
                ProofOfPossession = new V2alpha3ResourceServerProofOfPossession { Required = false },
            };

            Assert.IsFalse(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        // ──────────────────────── CustomText ──────────────────────────────────────

        static V2alpha3CustomTextScreen Screen(params (string Key, string Value)[] entries)
        {
            var screen = new V2alpha3CustomTextScreen();

            foreach (var entry in entries)
                screen[entry.Key] = entry.Value;

            return screen;
        }

        [TestMethod]
        public void CustomText_IdenticalScreens_NoUpdate()
        {
            var conf = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome"), ("description", "Sign in")) };
            var last = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome"), ("description", "Sign in")) };

            Assert.IsFalse(V2alpha3CustomTextController.ScreensRequireUpdate(conf, last));
        }

        [TestMethod]
        public void CustomText_ChangedLeafValue_RequiresUpdate()
        {
            var conf = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome back")) };
            var last = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome")) };

            Assert.IsTrue(V2alpha3CustomTextController.ScreensRequireUpdate(conf, last));
        }

        [TestMethod]
        public void CustomText_ScreenOnlyInSpec_RequiresUpdate()
        {
            var conf = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome")), ["signup"] = Screen(("title", "Join")) };
            var last = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome")) };

            Assert.IsTrue(V2alpha3CustomTextController.ScreensRequireUpdate(conf, last));
        }

        [TestMethod]
        public void CustomText_ScreenOnlyInAuth0_RequiresUpdate()
        {
            // SetAsync is a full replace; a screen removed from the spec must still trigger the write
            var conf = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome")) };
            var last = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome")), ["signup"] = Screen(("title", "Join")) };

            Assert.IsTrue(V2alpha3CustomTextController.ScreensRequireUpdate(conf, last));
        }

        [TestMethod]
        public void CustomText_KeyRemovedFromScreen_RequiresUpdate()
        {
            // SetAsync is a full replace; a key removed from a screen must still trigger the write
            var conf = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome")) };
            var last = new Dictionary<string, V2alpha3CustomTextScreen> { ["login"] = Screen(("title", "Welcome"), ("description", "Sign in")) };

            Assert.IsTrue(V2alpha3CustomTextController.ScreensRequireUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_ScopesReordered_NoUpdate()
        {
            // scopes are a set keyed by value; Auth0 returning them in a different order is not drift
            var conf = new V2alpha3ResourceServerConf { Scopes = [new V2alpha3ResourceServerScope { Value = "read" }, new V2alpha3ResourceServerScope { Value = "write" }] };
            var last = new V2alpha3ResourceServerConf { Scopes = [new V2alpha3ResourceServerScope { Value = "write" }, new V2alpha3ResourceServerScope { Value = "read" }] };

            Assert.IsFalse(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_AuthorizationDetailsChange_RequiresUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { AuthorizationDetails = [new V2alpha3ResourceServerAuthorizationDetail { Type = "payment" }] };
            var last = new V2alpha3ResourceServerConf { AuthorizationDetails = [new V2alpha3ResourceServerAuthorizationDetail { Type = "account" }] };

            Assert.IsTrue(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ResourceServer_AuthorizationDetailsIdentical_NoUpdate()
        {
            var conf = new V2alpha3ResourceServerConf { AuthorizationDetails = [new V2alpha3ResourceServerAuthorizationDetail { Type = "payment" }] };
            var last = new V2alpha3ResourceServerConf { AuthorizationDetails = [new V2alpha3ResourceServerAuthorizationDetail { Type = "payment" }] };

            Assert.IsFalse(V2alpha3ResourceServerController.ConfRequiresUpdate(conf, last));
        }

    }

}
