using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Client.V1;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    using V2alpha1ClientEntity = Alethic.Auth0.Operator.Models.V2alpha1Client;

    [TestClass]
    [System.Runtime.Versioning.RequiresPreviewFeatures]
    public class V2alpha1ClientControllerMappingTests
    {

        [TestMethod]
        public void FromApi_Client_Null_ReturnsNull()
        {
            Assert.IsNull(V2alpha1ClientController.FromApi((GetClientResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_Client_MapsScalarProperties()
        {
            var source = new GetClientResponseContent
            {
                Name = "My App",
                Description = "Test app",
                LogoUri = "https://example.com/logo.png",
                OidcConformant = true,
                Sso = false,
                CrossOriginAuthentication = true,
                IsFirstParty = true,
                AppType = new ClientAppTypeEnum(ClientAppTypeEnum.Values.RegularWeb),
                TokenEndpointAuthMethod = new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost),
                ComplianceLevel = Optional<ClientComplianceLevelEnum?>.Of(new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar)),
                OrganizationUsage = new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require),
                OrganizationRequireBehavior = new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt),
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("My App", result.Name);
            Assert.AreEqual("Test app", result.Description);
            Assert.AreEqual("https://example.com/logo.png", result.LogoUri);
            Assert.AreEqual(true, result.OidcConformant);
            Assert.AreEqual(false, result.Sso);
            Assert.AreEqual(true, result.CrossOriginAuthentication);
            Assert.AreEqual(true, result.IsFirstParty);
            Assert.AreEqual(V2alpha1ClientAppTypeEnum.RegularWeb, result.ApplicationType);
            Assert.AreEqual(V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost, result.TokenEndpointAuthMethod);
            Assert.AreEqual(V2alpha1ClientComplianceLevelEnum.Fapi1AdvPkjPar, result.ComplianceLevel);
            Assert.AreEqual(V2alpha1ClientOrganizationUsageEnum.Require, result.OrganizationUsage);
            Assert.AreEqual(V2alpha1ClientOrganizationRequireBehaviorEnum.PreLoginPrompt, result.OrganizationRequireBehavior);
        }

        [TestMethod]
        public void FromApi_Client_MapsArrayProperties()
        {
            var source = new GetClientResponseContent
            {
                AllowedClients = ["client-1", "client-2"],
                AllowedLogoutUrls = ["https://example.com/logout"],
                AllowedOrigins = ["https://origin.example.com"],
                WebOrigins = ["https://web.example.com"],
                Callbacks = ["https://example.com/callback"],
                ClientAliases = ["alias-1", "alias-2"],
                GrantTypes = ["authorization_code", "refresh_token"],
                ClientMetadata = new Dictionary<string, object?>
                {
                    ["environment"] = "test",
                    ["enabled"] = true,
                },
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "client-1", "client-2" }, result.AllowedClients);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.AllowedLogoutUrls);
            CollectionAssert.AreEqual(new[] { "https://origin.example.com" }, result.AllowedOrigins);
            CollectionAssert.AreEqual(new[] { "https://web.example.com" }, result.WebOrigins);
            CollectionAssert.AreEqual(new[] { "https://example.com/callback" }, result.Callbacks);
            CollectionAssert.AreEqual(new[] { "alias-1", "alias-2" }, result.ClientAliases);
            CollectionAssert.AreEqual(new[] { "authorization_code", "refresh_token" }, result.GrantTypes);
            Assert.IsNotNull(result.ClientMetaData);
            Assert.AreEqual("test", result.ClientMetaData["environment"]);
            Assert.AreEqual(true, result.ClientMetaData["enabled"]);
        }

        [TestMethod]
        public void FromApi_SigningKey_MapsProperties()
        {
            var source = new ClientSigningKey
            {
                Cert = "cert",
                Pkcs7 = "pkcs7",
                Subject = "subject",
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("cert", result.Cert);
            Assert.AreEqual("pkcs7", result.Pkcs7);
            Assert.AreEqual("subject", result.Subject);
        }

        [TestMethod]
        public void FromApi_EncryptionKey_MapsProperties()
        {
            var source = new ClientEncryptionKey
            {
                Cert = "cert",
                Pub = "pub",
                Subject = "subject",
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("cert", result.Cert);
            Assert.AreEqual("pub", result.Pub);
            Assert.AreEqual("subject", result.Subject);
        }

        [TestMethod]
        public void FromApi_JwtConfiguration_MapsProperties()
        {
            var source = new ClientJwtConfiguration
            {
                SecretEncoded = true,
                LifetimeInSeconds = 3600,
                Alg = new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256),
                Scopes = new Dictionary<string, object>
                {
                    ["read:data"] = "allow",
                },
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.SecretEncoded);
            Assert.AreEqual(3600, result.LifetimeInSeconds);
            Assert.AreEqual(V2alpha1ClientSigningAlgorithmEnum.Rs256, result.Alg);
            Assert.IsNotNull(result.Scopes);
            Assert.AreEqual("allow", result.Scopes["read:data"]);
        }

        [TestMethod]
        public void FromApi_RefreshToken_MapsProperties()
        {
            var source = new ClientRefreshTokenConfiguration
            {
                RotationType = new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating),
                ExpirationType = new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring),
                Leeway = 10,
                TokenLifetime = 7200,
                InfiniteTokenLifetime = false,
                IdleTokenLifetime = 1800,
                InfiniteIdleTokenLifetime = false,
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha1ClientRefreshTokenRotationTypeEnum.Rotating, result.RotationType);
            Assert.AreEqual(V2alpha1ClientRefreshTokenExpirationTypeEnum.Expiring, result.ExpirationType);
            Assert.AreEqual(10, result.Leeway);
            Assert.AreEqual(7200, result.TokenLifetime);
            Assert.AreEqual(false, result.InfiniteTokenLifetime);
            Assert.AreEqual(1800, result.IdleTokenLifetime);
            Assert.AreEqual(false, result.InfiniteIdleTokenLifetime);
        }

        [TestMethod]
        public void FromApi_Client_MapsOidcLogoutSessionMetadata()
        {
            var source = new ClientOidcBackchannelLogoutSettings
            {
                BackchannelLogoutUrls = ["https://example.com/logout"],
                BackchannelLogoutInitiators = new ClientOidcBackchannelLogoutInitiators
                {
                    Mode = new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom),
                    SelectedInitiators = [new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout)],
                },
                BackchannelLogoutSessionMetadata = Optional<ClientOidcBackchannelLogoutSessionMetadata?>.Of(
                    new ClientOidcBackchannelLogoutSessionMetadata { Include = true }),
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.BackchannelLogoutUrls);
            Assert.IsNotNull(result.BackchannelLogoutInitiators);
            Assert.AreEqual(V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom, result.BackchannelLogoutInitiators.Mode);
            CollectionAssert.AreEqual(new[] { V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout }, result.BackchannelLogoutInitiators.SelectedInitiators);
            Assert.IsNotNull(result.BackchannelLogoutSessionMetadata);
            Assert.AreEqual(true, result.BackchannelLogoutSessionMetadata.Include);
        }

        [TestMethod]
        public void FromApi_BackchannelLogoutInitiators_MapsProperties()
        {
            var source = new ClientOidcBackchannelLogoutInitiators
            {
                Mode = new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All),
                SelectedInitiators =
                [
                    new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout),
                    new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired),
                ],
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.All, result.Mode);
            CollectionAssert.AreEqual(
                new[]
                {
                    V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout,
                    V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.SessionExpired,
                },
                result.SelectedInitiators);
        }

        [TestMethod]
        public void FromApi_DefaultOrganization_MapsProperties()
        {
            var source = new ClientDefaultOrganization
            {
                OrganizationId = "org_123",
                Flows = [new ClientDefaultOrganizationFlowsEnum(ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials)],
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("org_123", result.OrganizationId);
            CollectionAssert.AreEqual(new[] { V2alpha1ClientDefaultOrganizationFlowsEnum.ClientCredentials }, result.Flows);
        }

        [TestMethod]
        public void FromApi_SharePointAddon_MapsExternalUrlAsArray()
        {
            var source = new ClientAddonSharePoint
            {
                Url = "https://sharepoint.example.com",
                ExternalUrl = ClientAddonSharePointExternalUrl.FromListOfString(["https://external.example.com", "https://external2.example.com"]),
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("https://sharepoint.example.com", result.Url);
            CollectionAssert.AreEqual(new[] { "https://external.example.com", "https://external2.example.com" }, result.ExternalUrl);
        }

        [TestMethod]
        public void FromApi_SharePointAddon_StringExternalUrl_MapsToSingleItemArray()
        {
            var source = new ClientAddonSharePoint
            {
                ExternalUrl = ClientAddonSharePointExternalUrl.FromString("https://external.example.com"),
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "https://external.example.com" }, result.ExternalUrl);
        }

        [TestMethod]
        public void FromApi_Mobile_WithIosAndAndroid_MapsProperties()
        {
            var source = new ClientMobile
            {
                Ios = new ClientMobileiOs
                {
                    AppBundleIdentifier = "com.example.ios",
                    TeamId = "TEAM123",
                },
                Android = new ClientMobileAndroid
                {
                    AppPackageName = "com.example.android",
                },
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Ios);
            Assert.AreEqual("com.example.ios", result.Ios.AppBundleIdentifier);
            Assert.AreEqual("TEAM123", result.Ios.TeamId);
            Assert.IsNotNull(result.Android);
            Assert.AreEqual("com.example.android", result.Android.AppPackageName);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyIos_ReturnsNullIos()
        {
            var source = new ClientMobile
            {
                Ios = new ClientMobileiOs(),
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Ios);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyAndroid_ReturnsNullAndroid()
        {
            var source = new ClientMobile
            {
                Android = new ClientMobileAndroid(),
            };

            var result = V2alpha1ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Android);
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsOidcLogoutSessionMetadata()
        {
            var conf = new V2alpha1ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha1ClientAppTypeEnum.RegularWeb,
                OidcLogout = new V2alpha1ClientOidcBackchannelLogoutSettings
                {
                    BackchannelLogoutUrls = ["https://example.com/logout"],
                    BackchannelLogoutInitiators = new V2alpha1ClientOidcBackchannelLogoutInitiators
                    {
                        Mode = V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
                        SelectedInitiators = [V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout],
                    },
                    BackchannelLogoutSessionMetadata = new V2alpha1ClientOidcBackchannelLogoutSessionMetadata
                    {
                        Include = true,
                    },
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha1ClientController.ApplyToApi(conf, request);

            Assert.IsNotNull(request.OidcLogout);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, request.OidcLogout.BackchannelLogoutUrls?.ToArray());
            Assert.IsNotNull(request.OidcLogout.BackchannelLogoutInitiators);
            Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom, request.OidcLogout.BackchannelLogoutInitiators.Mode?.Value);
            CollectionAssert.AreEqual(
                new[] { ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout },
                request.OidcLogout.BackchannelLogoutInitiators.SelectedInitiators?.Select(static i => i.Value).ToArray());
            Assert.IsTrue(request.OidcLogout.BackchannelLogoutSessionMetadata.IsDefined);
            Assert.AreEqual(true, request.OidcLogout.BackchannelLogoutSessionMetadata.Value?.Include);
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsDefaultOrganization()
        {
            var conf = new V2alpha1ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha1ClientAppTypeEnum.RegularWeb,
                DefaultOrganization = new V2alpha1ClientDefaultOrganization
                {
                    OrganizationId = "org_123",
                    Flows = [V2alpha1ClientDefaultOrganizationFlowsEnum.ClientCredentials],
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha1ClientController.ApplyToApi(conf, request);

            Assert.IsTrue(request.DefaultOrganization.IsDefined);
            Assert.AreEqual("org_123", request.DefaultOrganization.Value?.OrganizationId);
            CollectionAssert.AreEqual(
                new[] { ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials },
                request.DefaultOrganization.Value?.Flows?.Select(static i => i.Value).ToArray());
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsSharePointAddonExternalUrlAsList()
        {
            var conf = new V2alpha1ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha1ClientAppTypeEnum.RegularWeb,
                AddOns = new V2alpha1ClientAddons
                {
                    Sharepoint = new V2alpha1ClientAddonSharePoint
                    {
                        Url = "https://sharepoint.example.com",
                        ExternalUrl = ["https://external.example.com", "https://external2.example.com"],
                    },
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha1ClientController.ApplyToApi(conf, request);

            Assert.IsNotNull(request.Addons?.Sharepoint);
            Assert.AreEqual("https://sharepoint.example.com", request.Addons.Sharepoint.Url);
            Assert.IsNotNull(request.Addons.Sharepoint.ExternalUrl);
            Assert.IsTrue(request.Addons.Sharepoint.ExternalUrl.TryGetListOfString(out var values));
            CollectionAssert.AreEqual(new[] { "https://external.example.com", "https://external2.example.com" }, values?.ToArray());
        }

        [TestMethod]
        public void FromApi_EnumNulls_ReturnNull()
        {
            Assert.IsNull(V2alpha1ClientController.FromApi((ClientAppTypeEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((ClientTokenEndpointAuthMethodEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((RefreshTokenRotationTypeEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((RefreshTokenExpirationTypeEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((ClientOrganizationUsageEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((ClientOrganizationRequireBehaviorEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((ClientComplianceLevelEnum?)null));
            Assert.IsNull(V2alpha1ClientController.FromApi((ClientOidcBackchannelLogoutInitiatorsModeEnum?)null));
        }

        [TestMethod]
        public void ClientApplicationTypes_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientAppTypeEnum Model)[]
            {
                (ClientAppTypeEnum.Values.Native, V2alpha1ClientAppTypeEnum.Native),
                (ClientAppTypeEnum.Values.NonInteractive, V2alpha1ClientAppTypeEnum.NonInteractive),
                (ClientAppTypeEnum.Values.Spa, V2alpha1ClientAppTypeEnum.Spa),
                (ClientAppTypeEnum.Values.RegularWeb, V2alpha1ClientAppTypeEnum.RegularWeb),
                (ClientAppTypeEnum.Values.Box, V2alpha1ClientAppTypeEnum.Box),
                (ClientAppTypeEnum.Values.Cloudbees, V2alpha1ClientAppTypeEnum.Cloudbees),
                (ClientAppTypeEnum.Values.Concur, V2alpha1ClientAppTypeEnum.Concur),
                (ClientAppTypeEnum.Values.Dropbox, V2alpha1ClientAppTypeEnum.Dropbox),
                (ClientAppTypeEnum.Values.Echosign, V2alpha1ClientAppTypeEnum.Echosign),
                (ClientAppTypeEnum.Values.Egnyte, V2alpha1ClientAppTypeEnum.Egnyte),
                (ClientAppTypeEnum.Values.Mscrm, V2alpha1ClientAppTypeEnum.Mscrm),
                (ClientAppTypeEnum.Values.Newrelic, V2alpha1ClientAppTypeEnum.Newrelic),
                (ClientAppTypeEnum.Values.Office365, V2alpha1ClientAppTypeEnum.Office365),
                (ClientAppTypeEnum.Values.Rms, V2alpha1ClientAppTypeEnum.Rms),
                (ClientAppTypeEnum.Values.Salesforce, V2alpha1ClientAppTypeEnum.Salesforce),
                (ClientAppTypeEnum.Values.Sentry, V2alpha1ClientAppTypeEnum.Sentry),
                (ClientAppTypeEnum.Values.Sharepoint, V2alpha1ClientAppTypeEnum.Sharepoint),
                (ClientAppTypeEnum.Values.Slack, V2alpha1ClientAppTypeEnum.Slack),
                (ClientAppTypeEnum.Values.Springcm, V2alpha1ClientAppTypeEnum.Springcm),
                (ClientAppTypeEnum.Values.Zendesk, V2alpha1ClientAppTypeEnum.Zendesk),
                (ClientAppTypeEnum.Values.Zoom, V2alpha1ClientAppTypeEnum.Zoom),
                (ClientAppTypeEnum.Values.ResourceServer, V2alpha1ClientAppTypeEnum.ResourceServer),
                (ClientAppTypeEnum.Values.ExpressConfiguration, V2alpha1ClientAppTypeEnum.ExpressConfiguration),
                (ClientAppTypeEnum.Values.SsoIntegration, V2alpha1ClientAppTypeEnum.SsoIntegration),
                (ClientAppTypeEnum.Values.Oag, V2alpha1ClientAppTypeEnum.Oag),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new ClientAppTypeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void TokenEndpointAuthMethod_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientTokenEndpointAuthMethodEnum Model)[]
            {
                (ClientTokenEndpointAuthMethodEnum.Values.None, V2alpha1ClientTokenEndpointAuthMethodEnum.None),
                (ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost, V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost),
                (ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic, V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretBasic),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new ClientTokenEndpointAuthMethodEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void RefreshTokenRotationType_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientRefreshTokenRotationTypeEnum Model)[]
            {
                (RefreshTokenRotationTypeEnum.Values.Rotating, V2alpha1ClientRefreshTokenRotationTypeEnum.Rotating),
                (RefreshTokenRotationTypeEnum.Values.NonRotating, V2alpha1ClientRefreshTokenRotationTypeEnum.NonRotating),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new RefreshTokenRotationTypeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void RefreshTokenExpirationType_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientRefreshTokenExpirationTypeEnum Model)[]
            {
                (RefreshTokenExpirationTypeEnum.Values.Expiring, V2alpha1ClientRefreshTokenExpirationTypeEnum.Expiring),
                (RefreshTokenExpirationTypeEnum.Values.NonExpiring, V2alpha1ClientRefreshTokenExpirationTypeEnum.NonExpiring),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new RefreshTokenExpirationTypeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void OrganizationUsage_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientOrganizationUsageEnum Model)[]
            {
                (ClientOrganizationUsageEnum.Values.Deny, V2alpha1ClientOrganizationUsageEnum.Deny),
                (ClientOrganizationUsageEnum.Values.Allow, V2alpha1ClientOrganizationUsageEnum.Allow),
                (ClientOrganizationUsageEnum.Values.Require, V2alpha1ClientOrganizationUsageEnum.Require),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new ClientOrganizationUsageEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void OrganizationRequireBehavior_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientOrganizationRequireBehaviorEnum Model)[]
            {
                (ClientOrganizationRequireBehaviorEnum.Values.NoPrompt, V2alpha1ClientOrganizationRequireBehaviorEnum.NoPrompt),
                (ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt, V2alpha1ClientOrganizationRequireBehaviorEnum.PreLoginPrompt),
                (ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt, V2alpha1ClientOrganizationRequireBehaviorEnum.PostLoginPrompt),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new ClientOrganizationRequireBehaviorEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void ComplianceLevel_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientComplianceLevelEnum Model)[]
            {
                (ClientComplianceLevelEnum.Values.None, V2alpha1ClientComplianceLevelEnum.None),
                (ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar, V2alpha1ClientComplianceLevelEnum.Fapi1AdvPkjPar),
                (ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar, V2alpha1ClientComplianceLevelEnum.Fapi1AdvMtlsPar),
                (ClientComplianceLevelEnum.Values.Fapi2SpPkjMtls, V2alpha1ClientComplianceLevelEnum.Fapi2SpPkjMtls),
                (ClientComplianceLevelEnum.Values.Fapi2SpMtlsMtls, V2alpha1ClientComplianceLevelEnum.Fapi2SpMtlsMtls),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new ClientComplianceLevelEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void LogoutInitiators_Roundtrip_AllSupportedValues()
        {
            var cases = new string[]
            {
                ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout,
                ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout,
                ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged,
                ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired,
            };

            foreach (var testCase in cases)
            {
                var result = V2alpha1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsEnum(testCase));
                Assert.AreEqual(testCase, V2alpha1ClientController.ToApi(result).Value);
            }
        }

        [TestMethod]
        public void LogoutInitiatorModes_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum Model)[]
            {
                (ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All, V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.All),
                (ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom, V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsModeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha1ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void Converter_ConvertConf_MapsClientSpecificFields()
        {
            var source = new V1Client
            {
                Spec =
                {
                    Find = new V1ClientFind { ClientId = "abc123", Name = "legacy-app" },
                    Conf = new V1ClientConf
                    {
                        Name = "legacy-app",
                        ApplicationType = V1ClientApplicationType.RegularWeb,
                        TokenEndpointAuthMethod = V1ClientTokenEndpointAuthMethod.ClientSecretPost,
                        ComplianceLevel = V1ClientComplianceLevel.Fapi1AdvPkjPar,
                        OrganizationUsage = V1ClientOrganizationUsage.Require,
                        OrganizationRequireBehavior = V1ClientOrganizationRequireBehavior.PostLoginPrompt,
                        RefreshToken = new V1ClientRefreshToken
                        {
                            RotationType = V1ClientRefreshTokenRotationType.Rotating,
                            ExpirationType = V1ClientRefreshTokenExpirationType.Expiring,
                            Leeway = 10,
                            TokenLifetime = 7200,
                            InfiniteTokenLifetime = false,
                            IdleTokenLifetime = 1800,
                            InfiniteIdleTokenLifetime = false,
                        },
                        DefaultOrganization = new V1ClientDefaultOrganization
                        {
                            OrganizationId = "org_123",
                            Flows = [V1ClientFlows.ClientCredentials],
                        },
                        OidcLogout = new V1ClientOidcLogoutConfig
                        {
                            BackchannelLogoutUrls = ["https://example.com/logout"],
                            BackchannelLogoutInitiators = new V1ClientBackchannelLogoutInitiators
                            {
                                Mode = V1ClientLogoutInitiatorModes.Custom,
                                SelectedInitiators = [V1ClientLogoutInitiators.RpLogout],
                            },
                        },
                    },
                },
                Status =
                {
                    Id = "cli_123",
                },
            };

            var result = InvokeConvert(source);

            Assert.AreEqual("abc123", result.Spec.Find?.ClientId);
            Assert.AreEqual("legacy-app", result.Spec.Find?.Name);
            Assert.AreEqual("cli_123", result.Status.Id);
            Assert.AreEqual(V2alpha1ClientAppTypeEnum.RegularWeb, result.Spec.Conf?.ApplicationType);
            Assert.AreEqual(V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost, result.Spec.Conf?.TokenEndpointAuthMethod);
            Assert.AreEqual(V2alpha1ClientComplianceLevelEnum.Fapi1AdvPkjPar, result.Spec.Conf?.ComplianceLevel);
            Assert.AreEqual(V2alpha1ClientOrganizationUsageEnum.Require, result.Spec.Conf?.OrganizationUsage);
            Assert.AreEqual(V2alpha1ClientOrganizationRequireBehaviorEnum.PostLoginPrompt, result.Spec.Conf?.OrganizationRequireBehavior);
            Assert.IsNotNull(result.Spec.Conf?.RefreshToken);
            Assert.AreEqual(V2alpha1ClientRefreshTokenRotationTypeEnum.Rotating, result.Spec.Conf.RefreshToken.RotationType);
            Assert.AreEqual(V2alpha1ClientRefreshTokenExpirationTypeEnum.Expiring, result.Spec.Conf.RefreshToken.ExpirationType);
            Assert.AreEqual(10, result.Spec.Conf.RefreshToken.Leeway);
            Assert.IsNotNull(result.Spec.Conf.DefaultOrganization);
            Assert.AreEqual("org_123", result.Spec.Conf.DefaultOrganization.OrganizationId);
            CollectionAssert.AreEqual(new[] { V2alpha1ClientDefaultOrganizationFlowsEnum.ClientCredentials }, result.Spec.Conf.DefaultOrganization.Flows);
            Assert.IsNotNull(result.Spec.Conf.OidcLogout);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.Spec.Conf.OidcLogout.BackchannelLogoutUrls);
            Assert.AreEqual(V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom, result.Spec.Conf.OidcLogout.BackchannelLogoutInitiators?.Mode);
            CollectionAssert.AreEqual(new[] { V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout }, result.Spec.Conf.OidcLogout.BackchannelLogoutInitiators?.SelectedInitiators);
        }

        [TestMethod]
        public void Converter_RevertConf_MapsClientSpecificFields_WithoutDirectJsonV2ToV1()
        {
            var source = new V2alpha1ClientEntity
            {
                Spec =
                {
                    Find = new V2alpha1ClientFind { ClientId = "abc123", Name = "modern-app" },
                    Conf = new V2alpha1ClientConf
                    {
                        Name = "modern-app",
                        ApplicationType = V2alpha1ClientAppTypeEnum.RegularWeb,
                        TokenEndpointAuthMethod = V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
                        ComplianceLevel = V2alpha1ClientComplianceLevelEnum.Fapi1AdvPkjPar,
                        OrganizationUsage = V2alpha1ClientOrganizationUsageEnum.Require,
                        OrganizationRequireBehavior = V2alpha1ClientOrganizationRequireBehaviorEnum.PostLoginPrompt,
                        RefreshToken = new V2alpha1ClientRefreshTokenConfiguration
                        {
                            RotationType = V2alpha1ClientRefreshTokenRotationTypeEnum.Rotating,
                            ExpirationType = V2alpha1ClientRefreshTokenExpirationTypeEnum.Expiring,
                            Leeway = 10,
                            TokenLifetime = 7200,
                            InfiniteTokenLifetime = false,
                            IdleTokenLifetime = 1800,
                            InfiniteIdleTokenLifetime = false,
                        },
                        DefaultOrganization = new V2alpha1ClientDefaultOrganization
                        {
                            OrganizationId = "org_123",
                            Flows = [V2alpha1ClientDefaultOrganizationFlowsEnum.ClientCredentials],
                        },
                        OidcLogout = new V2alpha1ClientOidcBackchannelLogoutSettings
                        {
                            BackchannelLogoutUrls = ["https://example.com/logout"],
                            BackchannelLogoutInitiators = new V2alpha1ClientOidcBackchannelLogoutInitiators
                            {
                                Mode = V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
                                SelectedInitiators = [V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout],
                            },
                        },
                        EncryptionKey = new V2alpha1ClientEncryptionKey
                        {
                            Cert = "cert",
                            Pub = "pub",
                            Subject = "subject",
                        },
                    },
                },
                Status =
                {
                    Id = "cli_123",
                },
            };

            var result = InvokeRevert(source);

            Assert.AreEqual("abc123", result.Spec.Find?.ClientId);
            Assert.AreEqual("modern-app", result.Spec.Find?.Name);
            Assert.AreEqual("cli_123", result.Status.Id);
            Assert.AreEqual(V1ClientApplicationType.RegularWeb, result.Spec.Conf?.ApplicationType);
            Assert.AreEqual(V1ClientTokenEndpointAuthMethod.ClientSecretPost, result.Spec.Conf?.TokenEndpointAuthMethod);
            Assert.AreEqual(V1ClientComplianceLevel.Fapi1AdvPkjPar, result.Spec.Conf?.ComplianceLevel);
            Assert.AreEqual(V1ClientOrganizationUsage.Require, result.Spec.Conf?.OrganizationUsage);
            Assert.AreEqual(V1ClientOrganizationRequireBehavior.PostLoginPrompt, result.Spec.Conf?.OrganizationRequireBehavior);
            Assert.IsNotNull(result.Spec.Conf?.RefreshToken);
            Assert.AreEqual(V1ClientRefreshTokenRotationType.Rotating, result.Spec.Conf.RefreshToken.RotationType);
            Assert.AreEqual(V1ClientRefreshTokenExpirationType.Expiring, result.Spec.Conf.RefreshToken.ExpirationType);
            Assert.AreEqual(10, result.Spec.Conf.RefreshToken.Leeway);
            Assert.IsNotNull(result.Spec.Conf.DefaultOrganization);
            Assert.AreEqual("org_123", result.Spec.Conf.DefaultOrganization.OrganizationId);
            CollectionAssert.AreEqual(new[] { V1ClientFlows.ClientCredentials }, result.Spec.Conf.DefaultOrganization.Flows);
            Assert.IsNotNull(result.Spec.Conf.OidcLogout);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.Spec.Conf.OidcLogout.BackchannelLogoutUrls);
            Assert.AreEqual(V1ClientLogoutInitiatorModes.Custom, result.Spec.Conf.OidcLogout.BackchannelLogoutInitiators?.Mode);
            CollectionAssert.AreEqual(new[] { V1ClientLogoutInitiators.RpLogout }, result.Spec.Conf.OidcLogout.BackchannelLogoutInitiators?.SelectedInitiators);
            Assert.IsNotNull(result.Spec.Conf.EncryptionKey);
            Assert.AreEqual("cert", result.Spec.Conf.EncryptionKey.Certificate);
            Assert.AreEqual("pub", result.Spec.Conf.EncryptionKey.PublicKey);
            Assert.AreEqual("subject", result.Spec.Conf.EncryptionKey.Subject);
        }

        static V2alpha1ClientEntity InvokeConvert(V1Client source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V2alpha1ClientEntity)method!.Invoke(converter, [source])!;
        }

        static V1Client InvokeRevert(V2alpha1ClientEntity source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Revert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V1Client)method!.Invoke(converter, [source])!;
        }

        static object CreateConverter()
        {
            var converterType = Type.GetType("Alethic.Auth0.Operator.Converters.ClientConverter+V1ToV2alpha1, Alethic.Auth0.Operator");
            Assert.IsNotNull(converterType);
            return Activator.CreateInstance(converterType!, nonPublic: true)!;
        }

    }

}
