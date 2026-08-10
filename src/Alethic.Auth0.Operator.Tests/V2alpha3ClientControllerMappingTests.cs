using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Client.V1;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    using V2alpha3ClientEntity = Alethic.Auth0.Operator.Models.V2alpha3Client;

    [TestClass]
    [System.Runtime.Versioning.RequiresPreviewFeatures]
    public class V2alpha3ClientControllerMappingTests
    {

        [TestMethod]
        public void FromApi_Client_Null_ReturnsNull()
        {
            Assert.IsNull(V2alpha3ClientController.FromApi((GetClientResponseContent?)null));
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
                SkipNonVerifiableCallbackUriConfirmationPrompt = true,
                AppType = new ClientAppTypeEnum(ClientAppTypeEnum.Values.RegularWeb),
                TokenEndpointAuthMethod = new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost),
                ComplianceLevel = Optional<ClientComplianceLevelEnum?>.Of(new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar)),
                OrganizationUsage = new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require),
                OrganizationRequireBehavior = new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt),
            };

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("My App", result.Name);
            Assert.AreEqual("Test app", result.Description);
            Assert.AreEqual("https://example.com/logo.png", result.LogoUri);
            Assert.AreEqual(true, result.OidcConformant);
            Assert.AreEqual(false, result.Sso);
            Assert.AreEqual(true, result.CrossOriginAuthentication);
            Assert.AreEqual(true, result.IsFirstParty);
            Assert.AreEqual(true, result.SkipNonVerifiableCallbackUriConfirmationPrompt);
            Assert.AreEqual(V2alpha3ClientAppTypeEnum.RegularWeb, result.ApplicationType);
            Assert.AreEqual(V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost, result.TokenEndpointAuthMethod);
            Assert.AreEqual(V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar, result.ComplianceLevel);
            Assert.AreEqual(V2alpha3ClientOrganizationUsageEnum.Require, result.OrganizationUsage);
            Assert.AreEqual(V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt, result.OrganizationRequireBehavior);
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

            var result = V2alpha3ClientController.FromApi(source);

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

            var result = V2alpha3ClientController.FromApi(source);

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

            var result = V2alpha3ClientController.FromApi(source);

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
                Scopes = new Dictionary<string, object?>
                {
                    ["read:data"] = "allow",
                },
            };

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.SecretEncoded);
            Assert.AreEqual(3600, result.LifetimeInSeconds);
            Assert.AreEqual(V2alpha3ClientSigningAlgorithmEnum.Rs256, result.Alg);
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

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating, result.RotationType);
            Assert.AreEqual(V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring, result.ExpirationType);
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

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.BackchannelLogoutUrls);
            Assert.IsNotNull(result.BackchannelLogoutInitiators);
            Assert.AreEqual(V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom, result.BackchannelLogoutInitiators.Mode);
            CollectionAssert.AreEqual(new[] { V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout }, result.BackchannelLogoutInitiators.SelectedInitiators);
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

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.All, result.Mode);
            CollectionAssert.AreEqual(
                new[]
                {
                    V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout,
                    V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.SessionExpired,
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

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("org_123", result.OrganizationId);
            CollectionAssert.AreEqual(new[] { V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials }, result.Flows);
        }

        [TestMethod]
        public void FromApi_SharePointAddon_MapsExternalUrlAsArray()
        {
            var source = new ClientAddonSharePoint
            {
                Url = "https://sharepoint.example.com",
                ExternalUrl = ClientAddonSharePointExternalUrl.FromListOfString(["https://external.example.com", "https://external2.example.com"]),
            };

            var result = V2alpha3ClientController.FromApi(source);

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

            var result = V2alpha3ClientController.FromApi(source);

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
                    Sha256CertFingerprints = ["AA:BB:CC", "DD:EE:FF"],
                },
            };

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Ios);
            Assert.AreEqual("com.example.ios", result.Ios.AppBundleIdentifier);
            Assert.AreEqual("TEAM123", result.Ios.TeamId);
            Assert.IsNotNull(result.Android);
            Assert.AreEqual("com.example.android", result.Android.AppPackageName);
            CollectionAssert.AreEqual(new[] { "AA:BB:CC", "DD:EE:FF" }, result.Android.Sha256CertFingerprints);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyIos_ReturnsNullIos()
        {
            var source = new ClientMobile
            {
                Ios = new ClientMobileiOs(),
            };

            var result = V2alpha3ClientController.FromApi(source);

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

            var result = V2alpha3ClientController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Android);
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsOidcLogoutSessionMetadata()
        {
            var conf = new V2alpha3ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha3ClientAppTypeEnum.RegularWeb,
                OidcLogout = new V2alpha3ClientOidcBackchannelLogoutSettings
                {
                    BackchannelLogoutUrls = ["https://example.com/logout"],
                    BackchannelLogoutInitiators = new V2alpha3ClientOidcBackchannelLogoutInitiators
                    {
                        Mode = V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
                        SelectedInitiators = [V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout],
                    },
                    BackchannelLogoutSessionMetadata = new V2alpha3ClientOidcBackchannelLogoutSessionMetadata
                    {
                        Include = true,
                    },
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha3ClientController.ApplyToApi(conf, request);

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
        public void ApplyToApi_Create_MapsAndroidMobile()
        {
            var conf = new V2alpha3ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha3ClientAppTypeEnum.Native,
                Mobile = new V2alpha3ClientMobile
                {
                    Android = new V2alpha3ClientMobileAndroid
                    {
                        AppPackageName = "com.example.android",
                        Sha256CertFingerprints = ["AA:BB:CC", "DD:EE:FF"],
                    },
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha3ClientController.ApplyToApi(conf, request);

            Assert.IsNotNull(request.Mobile);
            Assert.IsNotNull(request.Mobile.Android);
            Assert.AreEqual("com.example.android", request.Mobile.Android.AppPackageName);
            CollectionAssert.AreEqual(
                new[] { "AA:BB:CC", "DD:EE:FF" },
                request.Mobile.Android.Sha256CertFingerprints?.ToArray());
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsDefaultOrganization()
        {
            var conf = new V2alpha3ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha3ClientAppTypeEnum.RegularWeb,
                DefaultOrganization = new V2alpha3ClientDefaultOrganization
                {
                    OrganizationId = "org_123",
                    Flows = [V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials],
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha3ClientController.ApplyToApi(conf, request);

            Assert.IsTrue(request.DefaultOrganization.IsDefined);
            Assert.AreEqual("org_123", request.DefaultOrganization.Value?.OrganizationId);
            CollectionAssert.AreEqual(
                new[] { ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials },
                request.DefaultOrganization.Value?.Flows?.Select(static i => i.Value).ToArray());
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsSkipNonVerifiableCallbackUriConfirmationPrompt()
        {
            var conf = new V2alpha3ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha3ClientAppTypeEnum.RegularWeb,
                SkipNonVerifiableCallbackUriConfirmationPrompt = true,
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha3ClientController.ApplyToApi(conf, request);

            Assert.AreEqual(true, request.SkipNonVerifiableCallbackUriConfirmationPrompt);
        }

        [TestMethod]
        public void ApplyToApi_Update_MapsSkipNonVerifiableCallbackUriConfirmationPrompt()
        {
            var conf = new V2alpha3ClientConf
            {
                SkipNonVerifiableCallbackUriConfirmationPrompt = false,
            };

            var request = new UpdateClientRequestContent();
            V2alpha3ClientController.ApplyToApi(conf, request);

            Assert.AreEqual(false, request.SkipNonVerifiableCallbackUriConfirmationPrompt);
        }

        [TestMethod]
        public void ApplyToApi_Create_MapsSharePointAddonExternalUrlAsList()
        {
            var conf = new V2alpha3ClientConf
            {
                Name = "my-app",
                ApplicationType = V2alpha3ClientAppTypeEnum.RegularWeb,
                AddOns = new V2alpha3ClientAddons
                {
                    Sharepoint = new V2alpha3ClientAddonSharePoint
                    {
                        Url = "https://sharepoint.example.com",
                        ExternalUrl = ["https://external.example.com", "https://external2.example.com"],
                    },
                },
            };

            var request = new CreateClientRequestContent { Name = conf.Name! };
            V2alpha3ClientController.ApplyToApi(conf, request);

            Assert.IsNotNull(request.Addons?.Sharepoint);
            Assert.AreEqual("https://sharepoint.example.com", request.Addons.Sharepoint.Url);
            Assert.IsNotNull(request.Addons.Sharepoint.ExternalUrl);
            Assert.IsTrue(request.Addons.Sharepoint.ExternalUrl.TryGetListOfString(out var values));
            CollectionAssert.AreEqual(new[] { "https://external.example.com", "https://external2.example.com" }, values?.ToArray());
        }

        [TestMethod]
        public void FromApi_EnumNulls_ReturnNull()
        {
            Assert.IsNull(V2alpha3ClientController.FromApi((ClientAppTypeEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((ClientTokenEndpointAuthMethodEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((RefreshTokenRotationTypeEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((RefreshTokenExpirationTypeEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((ClientOrganizationUsageEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((ClientOrganizationRequireBehaviorEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((ClientComplianceLevelEnum?)null));
            Assert.IsNull(V2alpha3ClientController.FromApi((ClientOidcBackchannelLogoutInitiatorsModeEnum?)null));
        }

        [TestMethod]
        public void ClientApplicationTypes_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientAppTypeEnum Model)[]
            {
                (ClientAppTypeEnum.Values.Native, V2alpha3ClientAppTypeEnum.Native),
                (ClientAppTypeEnum.Values.NonInteractive, V2alpha3ClientAppTypeEnum.NonInteractive),
                (ClientAppTypeEnum.Values.Spa, V2alpha3ClientAppTypeEnum.Spa),
                (ClientAppTypeEnum.Values.RegularWeb, V2alpha3ClientAppTypeEnum.RegularWeb),
                (ClientAppTypeEnum.Values.Box, V2alpha3ClientAppTypeEnum.Box),
                (ClientAppTypeEnum.Values.Cloudbees, V2alpha3ClientAppTypeEnum.Cloudbees),
                (ClientAppTypeEnum.Values.Concur, V2alpha3ClientAppTypeEnum.Concur),
                (ClientAppTypeEnum.Values.Dropbox, V2alpha3ClientAppTypeEnum.Dropbox),
                (ClientAppTypeEnum.Values.Echosign, V2alpha3ClientAppTypeEnum.Echosign),
                (ClientAppTypeEnum.Values.Egnyte, V2alpha3ClientAppTypeEnum.Egnyte),
                (ClientAppTypeEnum.Values.Mscrm, V2alpha3ClientAppTypeEnum.Mscrm),
                (ClientAppTypeEnum.Values.Newrelic, V2alpha3ClientAppTypeEnum.Newrelic),
                (ClientAppTypeEnum.Values.Office365, V2alpha3ClientAppTypeEnum.Office365),
                (ClientAppTypeEnum.Values.Rms, V2alpha3ClientAppTypeEnum.Rms),
                (ClientAppTypeEnum.Values.Salesforce, V2alpha3ClientAppTypeEnum.Salesforce),
                (ClientAppTypeEnum.Values.Sentry, V2alpha3ClientAppTypeEnum.Sentry),
                (ClientAppTypeEnum.Values.Sharepoint, V2alpha3ClientAppTypeEnum.Sharepoint),
                (ClientAppTypeEnum.Values.Slack, V2alpha3ClientAppTypeEnum.Slack),
                (ClientAppTypeEnum.Values.Springcm, V2alpha3ClientAppTypeEnum.Springcm),
                (ClientAppTypeEnum.Values.Zendesk, V2alpha3ClientAppTypeEnum.Zendesk),
                (ClientAppTypeEnum.Values.Zoom, V2alpha3ClientAppTypeEnum.Zoom),
                (ClientAppTypeEnum.Values.ResourceServer, V2alpha3ClientAppTypeEnum.ResourceServer),
                (ClientAppTypeEnum.Values.ExpressConfiguration, V2alpha3ClientAppTypeEnum.ExpressConfiguration),
                (ClientAppTypeEnum.Values.SsoIntegration, V2alpha3ClientAppTypeEnum.SsoIntegration),
                (ClientAppTypeEnum.Values.Oag, V2alpha3ClientAppTypeEnum.Oag),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new ClientAppTypeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void TokenEndpointAuthMethod_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientTokenEndpointAuthMethodEnum Model)[]
            {
                (ClientTokenEndpointAuthMethodEnum.Values.None, V2alpha3ClientTokenEndpointAuthMethodEnum.None),
                (ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost, V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost),
                (ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic, V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretBasic),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new ClientTokenEndpointAuthMethodEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void RefreshTokenRotationType_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientRefreshTokenRotationTypeEnum Model)[]
            {
                (RefreshTokenRotationTypeEnum.Values.Rotating, V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating),
                (RefreshTokenRotationTypeEnum.Values.NonRotating, V2alpha3ClientRefreshTokenRotationTypeEnum.NonRotating),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new RefreshTokenRotationTypeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void RefreshTokenExpirationType_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientRefreshTokenExpirationTypeEnum Model)[]
            {
                (RefreshTokenExpirationTypeEnum.Values.Expiring, V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring),
                (RefreshTokenExpirationTypeEnum.Values.NonExpiring, V2alpha3ClientRefreshTokenExpirationTypeEnum.NonExpiring),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new RefreshTokenExpirationTypeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void OrganizationUsage_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientOrganizationUsageEnum Model)[]
            {
                (ClientOrganizationUsageEnum.Values.Deny, V2alpha3ClientOrganizationUsageEnum.Deny),
                (ClientOrganizationUsageEnum.Values.Allow, V2alpha3ClientOrganizationUsageEnum.Allow),
                (ClientOrganizationUsageEnum.Values.Require, V2alpha3ClientOrganizationUsageEnum.Require),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new ClientOrganizationUsageEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void OrganizationRequireBehavior_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientOrganizationRequireBehaviorEnum Model)[]
            {
                (ClientOrganizationRequireBehaviorEnum.Values.NoPrompt, V2alpha3ClientOrganizationRequireBehaviorEnum.NoPrompt),
                (ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt, V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt),
                (ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt, V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new ClientOrganizationRequireBehaviorEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
            }
        }

        [TestMethod]
        public void ComplianceLevel_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientComplianceLevelEnum Model)[]
            {
                (ClientComplianceLevelEnum.Values.None, V2alpha3ClientComplianceLevelEnum.None),
                (ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar, V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar),
                (ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar, V2alpha3ClientComplianceLevelEnum.Fapi1AdvMtlsPar),
                (ClientComplianceLevelEnum.Values.Fapi2SpPkjMtls, V2alpha3ClientComplianceLevelEnum.Fapi2SpPkjMtls),
                (ClientComplianceLevelEnum.Values.Fapi2SpMtlsMtls, V2alpha3ClientComplianceLevelEnum.Fapi2SpMtlsMtls),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new ClientComplianceLevelEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
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
                var result = V2alpha3ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsEnum(testCase));
                Assert.AreEqual(testCase, V2alpha3ClientController.ToApi(result).Value);
            }
        }

        [TestMethod]
        public void LogoutInitiatorModes_Roundtrip_AllSupportedValues()
        {
            var cases = new (string Api, V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum Model)[]
            {
                (ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All, V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.All),
                (ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom, V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom),
            };

            foreach (var testCase in cases)
            {
                Assert.AreEqual(testCase.Model, V2alpha3ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsModeEnum(testCase.Api)));
                Assert.AreEqual(testCase.Api, V2alpha3ClientController.ToApi(testCase.Model).Value);
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
                        Mobile = new V1ClientMobile
                        {
                            Android = new V1ClientMobile.MobileAndroid
                            {
                                AppPackageName = "com.example.android",
                                KeystoreHash = "AA:BB:CC",
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
            Assert.AreEqual(V2alpha3ClientAppTypeEnum.RegularWeb, result.Spec.Conf?.ApplicationType);
            Assert.AreEqual(V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost, result.Spec.Conf?.TokenEndpointAuthMethod);
            Assert.AreEqual(V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar, result.Spec.Conf?.ComplianceLevel);
            Assert.AreEqual(V2alpha3ClientOrganizationUsageEnum.Require, result.Spec.Conf?.OrganizationUsage);
            Assert.AreEqual(V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt, result.Spec.Conf?.OrganizationRequireBehavior);
            Assert.IsNotNull(result.Spec.Conf?.RefreshToken);
            Assert.AreEqual(V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating, result.Spec.Conf.RefreshToken.RotationType);
            Assert.AreEqual(V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring, result.Spec.Conf.RefreshToken.ExpirationType);
            Assert.AreEqual(10, result.Spec.Conf.RefreshToken.Leeway);
            Assert.IsNotNull(result.Spec.Conf.DefaultOrganization);
            Assert.AreEqual("org_123", result.Spec.Conf.DefaultOrganization.OrganizationId);
            CollectionAssert.AreEqual(new[] { V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials }, result.Spec.Conf.DefaultOrganization.Flows);
            Assert.IsNotNull(result.Spec.Conf.OidcLogout);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.Spec.Conf.OidcLogout.BackchannelLogoutUrls);
            Assert.AreEqual(V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom, result.Spec.Conf.OidcLogout.BackchannelLogoutInitiators?.Mode);
            CollectionAssert.AreEqual(new[] { V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout }, result.Spec.Conf.OidcLogout.BackchannelLogoutInitiators?.SelectedInitiators);
            Assert.IsNotNull(result.Spec.Conf.Mobile?.Android);
            Assert.AreEqual("com.example.android", result.Spec.Conf.Mobile!.Android!.AppPackageName);
            CollectionAssert.AreEqual(new[] { "AA:BB:CC" }, result.Spec.Conf.Mobile.Android.Sha256CertFingerprints);
        }

        [TestMethod]
        public void Converter_RevertConf_MapsClientSpecificFields_WithoutDirectJsonV2ToV1()
        {
            var source = new V2alpha3ClientEntity
            {
                Spec =
                {
                    Find = new V2alpha3ClientFind { ClientId = "abc123", Name = "modern-app" },
                    Conf = new V2alpha3ClientConf
                    {
                        Name = "modern-app",
                        ApplicationType = V2alpha3ClientAppTypeEnum.RegularWeb,
                        TokenEndpointAuthMethod = V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
                        ComplianceLevel = V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar,
                        OrganizationUsage = V2alpha3ClientOrganizationUsageEnum.Require,
                        OrganizationRequireBehavior = V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt,
                        RefreshToken = new V2alpha3ClientRefreshTokenConfiguration
                        {
                            RotationType = V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating,
                            ExpirationType = V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring,
                            Leeway = 10,
                            TokenLifetime = 7200,
                            InfiniteTokenLifetime = false,
                            IdleTokenLifetime = 1800,
                            InfiniteIdleTokenLifetime = false,
                        },
                        DefaultOrganization = new V2alpha3ClientDefaultOrganization
                        {
                            OrganizationId = "org_123",
                            Flows = [V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials],
                        },
                        OidcLogout = new V2alpha3ClientOidcBackchannelLogoutSettings
                        {
                            BackchannelLogoutUrls = ["https://example.com/logout"],
                            BackchannelLogoutInitiators = new V2alpha3ClientOidcBackchannelLogoutInitiators
                            {
                                Mode = V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
                                SelectedInitiators = [V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout],
                            },
                        },
                        EncryptionKey = new V2alpha3ClientEncryptionKey
                        {
                            Cert = "cert",
                            Pub = "pub",
                            Subject = "subject",
                        },
                        Mobile = new V2alpha3ClientMobile
                        {
                            Android = new V2alpha3ClientMobileAndroid
                            {
                                AppPackageName = "com.example.android",
                                Sha256CertFingerprints = ["AA:BB:CC", "DD:EE:FF"],
                            },
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
            Assert.IsNotNull(result.Spec.Conf.Mobile?.Android);
            Assert.AreEqual("com.example.android", result.Spec.Conf.Mobile!.Android!.AppPackageName);
            Assert.AreEqual("AA:BB:CC", result.Spec.Conf.Mobile.Android.KeystoreHash);
        }

        static V2alpha3ClientEntity InvokeConvert(V1Client source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V2alpha3ClientEntity)method!.Invoke(converter, [source])!;
        }

        static V1Client InvokeRevert(V2alpha3ClientEntity source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Revert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V1Client)method!.Invoke(converter, [source])!;
        }

        static object CreateConverter()
        {
            var converterType = Type.GetType("Alethic.Auth0.Operator.Converters.ClientConverter+V1ToV2alpha3, Alethic.Auth0.Operator");
            Assert.IsNotNull(converterType);
            return Activator.CreateInstance(converterType!, nonPublic: true)!;
        }

    }

}
