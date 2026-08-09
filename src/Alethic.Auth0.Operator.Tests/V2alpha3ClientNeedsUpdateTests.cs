using System.Collections.Generic;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Tests for the explicit per-property drift detection of <see cref="V2alpha3ClientController.ConfRequiresUpdate"/>.
    /// </summary>
    [TestClass]
    public class V2alpha3ClientNeedsUpdateTests
    {

        static V2alpha3ClientConf BuildConf() => new()
        {
            Name = "app",
            Description = "my app",
            ApplicationType = V2alpha3ClientAppTypeEnum.RegularWeb,
            TokenEndpointAuthMethod = V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
            Callbacks = ["https://example.com/callback"],
            AllowedLogoutUrls = ["https://example.com/logout"],
            GrantTypes = ["authorization_code", "refresh_token"],
            OidcConformant = true,
            Sso = false,
            ClientMetaData = new Dictionary<string, object?> { ["env"] = "prod" },
            JwtConfiguration = new V2alpha3ClientJwtConfiguration
            {
                LifetimeInSeconds = 3600,
                Alg = V2alpha3ClientSigningAlgorithmEnum.Rs256,
            },
            RefreshToken = new V2alpha3ClientRefreshTokenConfiguration
            {
                RotationType = V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating,
                ExpirationType = V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring,
                TokenLifetime = 2592000,
                Leeway = 0,
            },
            OidcLogout = new V2alpha3ClientOidcBackchannelLogoutSettings
            {
                BackchannelLogoutUrls = ["https://example.com/backchannel"],
                BackchannelLogoutInitiators = new V2alpha3ClientOidcBackchannelLogoutInitiators
                {
                    Mode = V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
                    SelectedInitiators = [V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout],
                },
            },
            AddOns = new V2alpha3ClientAddons
            {
                Samlp = new V2alpha3ClientAddonSaml
                {
                    Audience = "urn:example:api",
                    SignResponse = true,
                },
                Box = new Dictionary<string, object?> { ["key"] = "value" },
            },
            DefaultOrganization = new V2alpha3ClientDefaultOrganization
            {
                OrganizationId = "org_123",
                Flows = [V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials],
            },
        };

        [TestMethod]
        public void IdenticalConf_NoUpdate()
        {
            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(BuildConf(), BuildConf()));
        }

        [TestMethod]
        public void ConfSubsetOfLast_NoUpdate()
        {
            // properties unset on the spec are unmanaged; Auth0-side extras must not force an update
            var conf = new V2alpha3ClientConf { Name = "app", Callbacks = ["https://example.com/callback"] };
            var last = BuildConf();
            last.LogoUri = "https://example.com/logo.png";
            last.IsFirstParty = true;
            last.CrossOriginAuthentication = false;

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedName_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.Name = "other";

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedApplicationType_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.ApplicationType = V2alpha3ClientAppTypeEnum.Spa;

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedCallbacksArray_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.Callbacks = ["https://example.com/callback", "https://example.com/other"];

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedJwtConfigurationLifetime_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.JwtConfiguration!.LifetimeInSeconds = 7200;

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedRefreshTokenLeeway_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.RefreshToken!.Leeway = 30;

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedOidcLogoutInitiatorsMode_RequiresUpdate()
        {
            // a deep doubly-nested leaf must be observed
            var conf = BuildConf();
            var last = BuildConf();
            last.OidcLogout!.BackchannelLogoutInitiators!.Mode = V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.All;

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedSamlAddonAudience_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.AddOns!.Samlp!.Audience = "urn:example:other";

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedDictionaryAddonValue_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.AddOns!.Box = new Dictionary<string, object?> { ["key"] = "changed" };

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void NestedSetOnConfButMissingOnLast_RequiresUpdate()
        {
            var conf = BuildConf();
            var last = BuildConf();
            last.JwtConfiguration = null;

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void MismatchedSigningKeys_NoUpdate()
        {
            // signingKeys is read-only and generated by Auth0
            var conf = BuildConf();
            conf.SigningKeys = [new V2alpha3ClientSigningKey { Cert = "spec-cert" }];
            var last = BuildConf();
            last.SigningKeys = [new V2alpha3ClientSigningKey { Cert = "auth0-cert" }];

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void MismatchedResourceServers_NoUpdate()
        {
            // resourceServers is never transmitted to Auth0
            var conf = BuildConf();
            conf.ResourceServers = [new V1ClientReference { Name = "api-a" }];
            var last = BuildConf();

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void RemovedMetadataKey_RequiresUpdate()
        {
            // a key present in Auth0 but absent from the spec is tombstoned on the wire
            var conf = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["keep"] = "1" } };
            var last = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["keep"] = "1", ["stale"] = "2" } };

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ChangedMetadataValue_RequiresUpdate()
        {
            var conf = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["env"] = "prod" } };
            var last = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["env"] = "dev" } };

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void SameMetadata_NoUpdate()
        {
            var conf = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["env"] = "prod" } };
            var last = new V2alpha3ClientConf { ClientMetaData = new Dictionary<string, object?> { ["env"] = "prod" } };

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void RefreshTokenPolicies_MissingFromAuth0_RequiresUpdate()
        {
            var conf = BuildConf();
            conf.RefreshToken!.Policies = [new V2alpha3ClientRefreshTokenPolicy { Audience = "https://api", Scope = ["read"] }];
            var last = BuildConf();

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void RefreshTokenPolicies_ChangedScope_RequiresUpdate()
        {
            var conf = BuildConf();
            conf.RefreshToken!.Policies = [new V2alpha3ClientRefreshTokenPolicy { Audience = "https://api", Scope = ["read", "write"] }];
            var last = BuildConf();
            last.RefreshToken!.Policies = [new V2alpha3ClientRefreshTokenPolicy { Audience = "https://api", Scope = ["read"] }];

            Assert.IsTrue(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void RefreshTokenPolicies_ReorderedPoliciesAndScopes_NoUpdate()
        {
            // policies are a set keyed by audience and scopes are unordered; reordering is not drift
            var conf = BuildConf();
            conf.RefreshToken!.Policies =
            [
                new V2alpha3ClientRefreshTokenPolicy { Audience = "https://a", Scope = ["read", "write"] },
                new V2alpha3ClientRefreshTokenPolicy { Audience = "https://b", Scope = ["admin"] },
            ];
            var last = BuildConf();
            last.RefreshToken!.Policies =
            [
                new V2alpha3ClientRefreshTokenPolicy { Audience = "https://b", Scope = ["admin"] },
                new V2alpha3ClientRefreshTokenPolicy { Audience = "https://a", Scope = ["write", "read"] },
            ];

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void Callbacks_Reordered_NoUpdate()
        {
            // callbacks are an allowlist; Auth0 returning them in a different order is not drift
            var conf = new V2alpha3ClientConf { Callbacks = ["https://a/cb", "https://b/cb"] };
            var last = new V2alpha3ClientConf { Callbacks = ["https://b/cb", "https://a/cb"] };

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void GrantTypes_Reordered_NoUpdate()
        {
            var conf = new V2alpha3ClientConf { GrantTypes = ["authorization_code", "refresh_token"] };
            var last = new V2alpha3ClientConf { GrantTypes = ["refresh_token", "authorization_code"] };

            Assert.IsFalse(V2alpha3ClientController.ConfRequiresUpdate(conf, last));
        }

    }

}
