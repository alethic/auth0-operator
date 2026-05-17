using System.Collections.Generic;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Client.V1;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1ClientControllerMappingTests
    {

        // ──────────────────────── FromApi null-guard tests ────────────────────────

        [TestMethod]
        public void FromApi_Client_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((GetClientResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_SigningKey_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientSigningKey?)null));
        }

        [TestMethod]
        public void FromApi_RefreshToken_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientRefreshTokenConfiguration?)null));
        }

        [TestMethod]
        public void FromApi_OidcLogoutConfig_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientOidcBackchannelLogoutSettings?)null));
        }

        [TestMethod]
        public void FromApi_BackchannelLogoutInitiators_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientOidcBackchannelLogoutInitiators?)null));
        }

        [TestMethod]
        public void FromApi_JwtConfiguration_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientJwtConfiguration?)null));
        }

        [TestMethod]
        public void FromApi_EncryptionKey_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientEncryptionKey?)null));
        }

        [TestMethod]
        public void FromApi_DefaultOrganization_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientDefaultOrganization?)null));
        }

        [TestMethod]
        public void FromApi_Mobile_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientMobile?)null));
        }

        [TestMethod]
        public void FromApi_Addons_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientAddons?)null));
        }

        // ──────────────────────── FromApi TokenEndpointAuthMethod ─────────────────

        [TestMethod]
        public void FromApi_TokenEndpointAuthMethod_None() => Assert.AreEqual(V1ClientTokenEndpointAuthMethod.None, V1ClientController.FromApi(new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.None)));

        [TestMethod]
        public void FromApi_TokenEndpointAuthMethod_ClientSecretPost() => Assert.AreEqual(V1ClientTokenEndpointAuthMethod.ClientSecretPost, V1ClientController.FromApi(new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost)));

        [TestMethod]
        public void FromApi_TokenEndpointAuthMethod_ClientSecretBasic() => Assert.AreEqual(V1ClientTokenEndpointAuthMethod.ClientSecretBasic, V1ClientController.FromApi(new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic)));

        [TestMethod]
        public void FromApi_TokenEndpointAuthMethod_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientTokenEndpointAuthMethodEnum?)null));
        }

        // ──────────────────────── FromApi RefreshTokenRotationType ────────────────

        [TestMethod]
        public void FromApi_RefreshTokenRotationType_Rotating() => Assert.AreEqual(V1ClientRefreshTokenRotationType.Rotating, V1ClientController.FromApi(new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating)));

        [TestMethod]
        public void FromApi_RefreshTokenRotationType_NonRotating() => Assert.AreEqual(V1ClientRefreshTokenRotationType.NonRotating, V1ClientController.FromApi(new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating)));

        [TestMethod]
        public void FromApi_RefreshTokenRotationType_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((RefreshTokenRotationTypeEnum?)null));
        }

        // ──────────────────────── FromApi RefreshTokenExpirationType ──────────────

        [TestMethod]
        public void FromApi_RefreshTokenExpirationType_Expiring() => Assert.AreEqual(V1ClientRefreshTokenExpirationType.Expiring, V1ClientController.FromApi(new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring)));

        [TestMethod]
        public void FromApi_RefreshTokenExpirationType_NonExpiring() => Assert.AreEqual(V1ClientRefreshTokenExpirationType.NonExpiring, V1ClientController.FromApi(new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring)));

        [TestMethod]
        public void FromApi_RefreshTokenExpirationType_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((RefreshTokenExpirationTypeEnum?)null));
        }

        // ──────────────────────── FromApi OrganizationUsage ───────────────────────

        [TestMethod]
        public void FromApi_OrganizationUsage_Deny() => Assert.AreEqual(V1ClientOrganizationUsage.Deny, V1ClientController.FromApi(new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Deny)));

        [TestMethod]
        public void FromApi_OrganizationUsage_Allow() => Assert.AreEqual(V1ClientOrganizationUsage.Allow, V1ClientController.FromApi(new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Allow)));

        [TestMethod]
        public void FromApi_OrganizationUsage_Require() => Assert.AreEqual(V1ClientOrganizationUsage.Require, V1ClientController.FromApi(new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require)));

        [TestMethod]
        public void FromApi_OrganizationUsage_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientOrganizationUsageEnum?)null));
        }

        // ──────────────────────── FromApi OrganizationRequireBehavior ─────────────

        [TestMethod]
        public void FromApi_OrganizationRequireBehavior_NoPrompt() => Assert.AreEqual(V1ClientOrganizationRequireBehavior.NoPrompt, V1ClientController.FromApi(new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.NoPrompt)));

        [TestMethod]
        public void FromApi_OrganizationRequireBehavior_PreLoginPrompt() => Assert.AreEqual(V1ClientOrganizationRequireBehavior.PreLoginPrompt, V1ClientController.FromApi(new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt)));

        [TestMethod]
        public void FromApi_OrganizationRequireBehavior_PostLoginPrompt() => Assert.AreEqual(V1ClientOrganizationRequireBehavior.PostLoginPrompt, V1ClientController.FromApi(new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt)));

        [TestMethod]
        public void FromApi_OrganizationRequireBehavior_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientOrganizationRequireBehaviorEnum?)null));
        }

        // ──────────────────────── FromApi LogoutInitiators ────────────────────────

        [TestMethod]
        public void FromApi_LogoutInitiators_RpLogout() => Assert.AreEqual(V1ClientLogoutInitiators.RpLogout, V1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout)));

        [TestMethod]
        public void FromApi_LogoutInitiators_IdpLogout() => Assert.AreEqual(V1ClientLogoutInitiators.IdpLogout, V1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout)));

        [TestMethod]
        public void FromApi_LogoutInitiators_PasswordChanged() => Assert.AreEqual(V1ClientLogoutInitiators.PasswordChanged, V1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged)));

        [TestMethod]
        public void FromApi_LogoutInitiators_SessionExpired() => Assert.AreEqual(V1ClientLogoutInitiators.SessionExpired, V1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired)));

        // ──────────────────────── FromApi LogoutInitiatorModes ────────────────────

        [TestMethod]
        public void FromApi_LogoutInitiatorModes_All() => Assert.AreEqual(V1ClientLogoutInitiatorModes.All, V1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All)));

        [TestMethod]
        public void FromApi_LogoutInitiatorModes_Custom() => Assert.AreEqual(V1ClientLogoutInitiatorModes.Custom, V1ClientController.FromApi(new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom)));

        [TestMethod]
        public void FromApi_LogoutInitiatorModes_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientOidcBackchannelLogoutInitiatorsModeEnum?)null));
        }

        // ──────────────────────── FromApi ComplianceLevel ────────────────────────

        [TestMethod]
        public void FromApi_ComplianceLevel_None() => Assert.AreEqual(V1ClientComplianceLevel.NONE, V1ClientController.FromApi(new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.None)));

        [TestMethod]
        public void FromApi_ComplianceLevel_Fapi1AdvPkjPar() => Assert.AreEqual(V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR, V1ClientController.FromApi(new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar)));

        [TestMethod]
        public void FromApi_ComplianceLevel_Fapi1AdvMtlsPar() => Assert.AreEqual(V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR, V1ClientController.FromApi(new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar)));

        [TestMethod]
        public void FromApi_ComplianceLevel_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientComplianceLevelEnum?)null));
        }

        // ──────────────────────── FromApi Flows ───────────────────────────────────

        [TestMethod]
        public void FromApi_Flows_ClientCredentials_MapsCorrectly()
        {
            Assert.AreEqual(V1ClientFlows.ClientCredentials, V1ClientController.FromApi(new ClientDefaultOrganizationFlowsEnum(ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials)));
        }

        // ──────────────────────── FromApi ClientApplicationType ──────────────────

        [TestMethod]
        public void FromApi_ClientApplicationType_Native() => Assert.AreEqual(V1ClientApplicationType.Native, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Native)));

        [TestMethod]
        public void FromApi_ClientApplicationType_NonInteractive() => Assert.AreEqual(V1ClientApplicationType.NonInteractive, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.NonInteractive)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Spa() => Assert.AreEqual(V1ClientApplicationType.Spa, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Spa)));

        [TestMethod]
        public void FromApi_ClientApplicationType_RegularWeb() => Assert.AreEqual(V1ClientApplicationType.RegularWeb, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.RegularWeb)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Box() => Assert.AreEqual(V1ClientApplicationType.Box, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Box)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Cloudbees() => Assert.AreEqual(V1ClientApplicationType.Cloudbees, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Cloudbees)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Concur() => Assert.AreEqual(V1ClientApplicationType.Concur, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Concur)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Dropbox() => Assert.AreEqual(V1ClientApplicationType.Dropbox, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Dropbox)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Echosign() => Assert.AreEqual(V1ClientApplicationType.Echosign, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Echosign)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Egnyte() => Assert.AreEqual(V1ClientApplicationType.Egnyte, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Egnyte)));

        [TestMethod]
        public void FromApi_ClientApplicationType_MsCrm() => Assert.AreEqual(V1ClientApplicationType.MsCrm, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Mscrm)));

        [TestMethod]
        public void FromApi_ClientApplicationType_NewRelic() => Assert.AreEqual(V1ClientApplicationType.NewRelic, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Newrelic)));

        [TestMethod]
        public void FromApi_ClientApplicationType_Office365()
        {
            Assert.AreEqual(V1ClientApplicationType.Office365, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Office365)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Rms()
        {
            Assert.AreEqual(V1ClientApplicationType.Rms, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Rms)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Salesforce()
        {
            Assert.AreEqual(V1ClientApplicationType.Salesforce, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Salesforce)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Sentry()
        {
            Assert.AreEqual(V1ClientApplicationType.Sentry, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sentry)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_SharePoint()
        {
            Assert.AreEqual(V1ClientApplicationType.SharePoint, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sharepoint)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Slack()
        {
            Assert.AreEqual(V1ClientApplicationType.Slack, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Slack)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_SpringCm()
        {
            Assert.AreEqual(V1ClientApplicationType.SpringCm, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Springcm)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Zendesk()
        {
            Assert.AreEqual(V1ClientApplicationType.Zendesk, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zendesk)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Zoom()
        {
            Assert.AreEqual(V1ClientApplicationType.Zoom, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zoom)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_ResourceServer()
        {
            Assert.AreEqual(V1ClientApplicationType.ResourceServer, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.ResourceServer)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_ExpressConfiguration()
        {
            Assert.AreEqual(V1ClientApplicationType.ExpressConfiguration, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.ExpressConfiguration)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_SsoIntegration()
        {
            Assert.AreEqual(V1ClientApplicationType.SsoIntegration, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.SsoIntegration)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Oag()
        {
            Assert.AreEqual(V1ClientApplicationType.Oag, V1ClientController.FromApi(new ClientAppTypeEnum(ClientAppTypeEnum.Values.Oag)));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientAppTypeEnum?)null));
        }

        // ──────────────────────── FromApi value objects ───────────────────────────

        [TestMethod]
        public void FromApi_SigningKey_MapsProperties()
        {
            var source = new ClientSigningKey { Cert = "cert", Pkcs7 = "pkcs7" };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("cert", result.Cert);
            Assert.AreEqual("pkcs7", result.Pkcs7);
        }

        [TestMethod]
        public void FromApi_EncryptionKey_MapsProperties()
        {
            var source = new ClientEncryptionKey { Cert = "cert", Pub = "pub", Subject = "sub" };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("cert", result.Certificate);
            Assert.AreEqual("pub", result.PublicKey);
            Assert.AreEqual("sub", result.Subject);
        }

        [TestMethod]
        public void FromApi_JwtConfiguration_MapsProperties()
        {
            var source = new ClientJwtConfiguration { SecretEncoded = true, LifetimeInSeconds = 3600, Alg = new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256) };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsSecretEncoded);
            Assert.AreEqual(3600, result.LifetimeInSeconds);
            Assert.AreEqual("RS256", result.SigningAlgorithm);
        }

        [TestMethod]
        public void FromApi_RefreshToken_MapsProperties()
        {
            var source = new ClientRefreshTokenConfiguration
            {
                RotationType = new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating),
                ExpirationType = new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring),
                Leeway = 10,
                TokenLifetime = 86400,
                InfiniteTokenLifetime = false,
                InfiniteIdleTokenLifetime = false,
            };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(V1ClientRefreshTokenRotationType.Rotating, result.RotationType);
            Assert.AreEqual(V1ClientRefreshTokenExpirationType.Expiring, result.ExpirationType);
            Assert.AreEqual(10, result.Leeway);
            Assert.AreEqual(86400, result.TokenLifetime);
            Assert.AreEqual(false, result.InfiniteTokenLifetime);
            Assert.AreEqual(false, result.InfiniteIdleTokenLifetime);
        }

        [TestMethod]
        public void FromApi_OidcLogoutConfig_MapsBackchannelLogoutUrls()
        {
            var source = new ClientOidcBackchannelLogoutSettings { BackchannelLogoutUrls = ["https://example.com/logout"] };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.BackchannelLogoutUrls);
        }

        [TestMethod]
        public void FromApi_BackchannelLogoutInitiators_MapsProperties()
        {
            var source = new ClientOidcBackchannelLogoutInitiators
            {
                Mode = new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom),
                SelectedInitiators = [new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout), new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout)],
            };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(V1ClientLogoutInitiatorModes.Custom, result.Mode);
            Assert.IsNotNull(result.SelectedInitiators);
            Assert.AreEqual(2, result.SelectedInitiators.Length);
            Assert.AreEqual(V1ClientLogoutInitiators.RpLogout, result.SelectedInitiators[0]);
            Assert.AreEqual(V1ClientLogoutInitiators.IdpLogout, result.SelectedInitiators[1]);
        }

        [TestMethod]
        public void FromApi_DefaultOrganization_MapsProperties()
        {
            var source = new ClientDefaultOrganization { OrganizationId = "org_123", Flows = [new ClientDefaultOrganizationFlowsEnum(ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials)] };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("org_123", result.OrganizationId);
            Assert.IsNotNull(result.Flows);
            Assert.AreEqual(1, result.Flows.Length);
            Assert.AreEqual(V1ClientFlows.ClientCredentials, result.Flows[0]);
        }

        [TestMethod]
        public void FromApi_Mobile_WithIosAndAndroid_MapsProperties()
        {
            var source = new ClientMobile
            {
                Ios = new ClientMobileiOs { AppBundleIdentifier = "com.example.app", TeamId = "TEAM123" },
                Android = new ClientMobileAndroid { AppPackageName = "com.example.app" },
            };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Ios);
            Assert.AreEqual("com.example.app", result.Ios.AppBundleIdentifier);
            Assert.AreEqual("TEAM123", result.Ios.TeamId);
            Assert.IsNotNull(result.Android);
            Assert.AreEqual("com.example.app", result.Android.AppPackageName);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyIos_Returns_NullIos()
        {
            var source = new ClientMobile { Ios = new ClientMobileiOs { AppBundleIdentifier = null, TeamId = null } };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Ios);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyAndroid_Returns_NullAndroid()
        {
            var source = new ClientMobile { Android = new ClientMobileAndroid { AppPackageName = null } };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Android);
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
            };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("My App", result.Name);
            Assert.AreEqual("Test app", result.Description);
            Assert.AreEqual("https://example.com/logo.png", result.LogoUri);
            Assert.AreEqual(true, result.OidcConformant);
            Assert.AreEqual(false, result.Sso);
            Assert.AreEqual(true, result.CrossOriginAuthentication);
            Assert.AreEqual(true, result.IsFirstParty);
            Assert.AreEqual(V1ClientApplicationType.RegularWeb, result.ApplicationType);
            Assert.AreEqual(V1ClientTokenEndpointAuthMethod.ClientSecretPost, result.TokenEndpointAuthMethod);
        }

        [TestMethod]
        public void FromApi_Client_MapsArrayProperties()
        {
            var source = new GetClientResponseContent
            {
                Name = "My App",
                AllowedClients = ["client1", "client2"],
                AllowedLogoutUrls = ["https://example.com/logout"],
                AllowedOrigins = ["https://example.com"],
                WebOrigins = ["https://example.com"],
                Callbacks = ["https://example.com/callback"],
                ClientAliases = ["alias1"],
                GrantTypes = ["authorization_code", "refresh_token"],
            };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "client1", "client2" }, result.AllowedClients);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.AllowedLogoutUrls);
            CollectionAssert.AreEqual(new[] { "https://example.com" }, result.AllowedOrigins);
            CollectionAssert.AreEqual(new[] { "https://example.com" }, result.WebOrigins);
            CollectionAssert.AreEqual(new[] { "https://example.com/callback" }, result.Callbacks);
            CollectionAssert.AreEqual(new[] { "alias1" }, result.ClientAliases);
            CollectionAssert.AreEqual(new[] { "authorization_code", "refresh_token" }, result.GrantTypes);
        }

        // ──────────────────────── ToApi ───────────────────────────────────────────

        [TestMethod]
        public void ToApi_ComplianceLevel_None() => Assert.AreEqual(ClientComplianceLevelEnum.Values.None, V1ClientController.ToApi(V1ClientComplianceLevel.NONE).Value);

        [TestMethod]
        public void ToApi_ComplianceLevel_Fapi1AdvPkjPar() => Assert.AreEqual(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar, V1ClientController.ToApi(V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR).Value);

        [TestMethod]
        public void ToApi_ComplianceLevel_Fapi1AdvMtlsPar() => Assert.AreEqual(ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar, V1ClientController.ToApi(V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR).Value);

        [TestMethod]
        public void ToApi_OrganizationRequireBehavior_NoPrompt() => Assert.AreEqual(ClientOrganizationRequireBehaviorEnum.Values.NoPrompt, V1ClientController.ToApi(V1ClientOrganizationRequireBehavior.NoPrompt).Value);
        [TestMethod]
        public void ToApi_OrganizationRequireBehavior_PreLoginPrompt() => Assert.AreEqual(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt, V1ClientController.ToApi(V1ClientOrganizationRequireBehavior.PreLoginPrompt).Value);
        [TestMethod]
        public void ToApi_OrganizationRequireBehavior_PostLoginPrompt() => Assert.AreEqual(ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt, V1ClientController.ToApi(V1ClientOrganizationRequireBehavior.PostLoginPrompt).Value);

        [TestMethod]
        public void ToApi_OrganizationUsage_Deny() => Assert.AreEqual(ClientOrganizationUsageEnum.Values.Deny, V1ClientController.ToApi(V1ClientOrganizationUsage.Deny).Value);
        [TestMethod]
        public void ToApi_OrganizationUsage_Allow() => Assert.AreEqual(ClientOrganizationUsageEnum.Values.Allow, V1ClientController.ToApi(V1ClientOrganizationUsage.Allow).Value);
        [TestMethod]
        public void ToApi_OrganizationUsage_Require() => Assert.AreEqual(ClientOrganizationUsageEnum.Values.Require, V1ClientController.ToApi(V1ClientOrganizationUsage.Require).Value);

        [TestMethod]
        public void ToApi_RefreshTokenRotationType_Rotating() => Assert.AreEqual(RefreshTokenRotationTypeEnum.Values.Rotating, V1ClientController.ToApi(V1ClientRefreshTokenRotationType.Rotating).Value);
        [TestMethod]
        public void ToApi_RefreshTokenRotationType_NonRotating() => Assert.AreEqual(RefreshTokenRotationTypeEnum.Values.NonRotating, V1ClientController.ToApi(V1ClientRefreshTokenRotationType.NonRotating).Value);

        [TestMethod]
        public void ToApi_RefreshTokenExpirationType_Expiring() => Assert.AreEqual(RefreshTokenExpirationTypeEnum.Values.Expiring, V1ClientController.ToApi(V1ClientRefreshTokenExpirationType.Expiring).Value);
        [TestMethod]
        public void ToApi_RefreshTokenExpirationType_NonExpiring() => Assert.AreEqual(RefreshTokenExpirationTypeEnum.Values.NonExpiring, V1ClientController.ToApi(V1ClientRefreshTokenExpirationType.NonExpiring).Value);

        [TestMethod]
        public void ToApi_LogoutInitiatorModes_All() => Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All, V1ClientController.ToApi(V1ClientLogoutInitiatorModes.All).Value);
        [TestMethod]
        public void ToApi_LogoutInitiatorModes_Custom() => Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom, V1ClientController.ToApi(V1ClientLogoutInitiatorModes.Custom).Value);

        [TestMethod]
        public void ToApi_LogoutInitiators_RpLogout() => Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout, V1ClientController.ToApi(V1ClientLogoutInitiators.RpLogout).Value);
        [TestMethod]
        public void ToApi_LogoutInitiators_IdpLogout() => Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout, V1ClientController.ToApi(V1ClientLogoutInitiators.IdpLogout).Value);
        [TestMethod]
        public void ToApi_LogoutInitiators_PasswordChanged() => Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged, V1ClientController.ToApi(V1ClientLogoutInitiators.PasswordChanged).Value);
        [TestMethod]
        public void ToApi_LogoutInitiators_SessionExpired() => Assert.AreEqual(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired, V1ClientController.ToApi(V1ClientLogoutInitiators.SessionExpired).Value);

        [TestMethod]
        public void ToApi_ClientApplicationType_Box() => Assert.AreEqual(ClientAppTypeEnum.Values.Box, V1ClientController.ToApi(V1ClientApplicationType.Box).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Cloudbees() => Assert.AreEqual(ClientAppTypeEnum.Values.Cloudbees, V1ClientController.ToApi(V1ClientApplicationType.Cloudbees).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Concur() => Assert.AreEqual(ClientAppTypeEnum.Values.Concur, V1ClientController.ToApi(V1ClientApplicationType.Concur).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Dropbox() => Assert.AreEqual(ClientAppTypeEnum.Values.Dropbox, V1ClientController.ToApi(V1ClientApplicationType.Dropbox).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Echosign() => Assert.AreEqual(ClientAppTypeEnum.Values.Echosign, V1ClientController.ToApi(V1ClientApplicationType.Echosign).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Egnyte() => Assert.AreEqual(ClientAppTypeEnum.Values.Egnyte, V1ClientController.ToApi(V1ClientApplicationType.Egnyte).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_MsCrm() => Assert.AreEqual(ClientAppTypeEnum.Values.Mscrm, V1ClientController.ToApi(V1ClientApplicationType.MsCrm).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Native() => Assert.AreEqual(ClientAppTypeEnum.Values.Native, V1ClientController.ToApi(V1ClientApplicationType.Native).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_NewRelic() => Assert.AreEqual(ClientAppTypeEnum.Values.Newrelic, V1ClientController.ToApi(V1ClientApplicationType.NewRelic).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_NonInteractive() => Assert.AreEqual(ClientAppTypeEnum.Values.NonInteractive, V1ClientController.ToApi(V1ClientApplicationType.NonInteractive).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Office365() => Assert.AreEqual(ClientAppTypeEnum.Values.Office365, V1ClientController.ToApi(V1ClientApplicationType.Office365).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_RegularWeb() => Assert.AreEqual(ClientAppTypeEnum.Values.RegularWeb, V1ClientController.ToApi(V1ClientApplicationType.RegularWeb).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Rms() => Assert.AreEqual(ClientAppTypeEnum.Values.Rms, V1ClientController.ToApi(V1ClientApplicationType.Rms).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Salesforce() => Assert.AreEqual(ClientAppTypeEnum.Values.Salesforce, V1ClientController.ToApi(V1ClientApplicationType.Salesforce).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Sentry() => Assert.AreEqual(ClientAppTypeEnum.Values.Sentry, V1ClientController.ToApi(V1ClientApplicationType.Sentry).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_SharePoint() => Assert.AreEqual(ClientAppTypeEnum.Values.Sharepoint, V1ClientController.ToApi(V1ClientApplicationType.SharePoint).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Slack() => Assert.AreEqual(ClientAppTypeEnum.Values.Slack, V1ClientController.ToApi(V1ClientApplicationType.Slack).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_SpringCm() => Assert.AreEqual(ClientAppTypeEnum.Values.Springcm, V1ClientController.ToApi(V1ClientApplicationType.SpringCm).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Spa() => Assert.AreEqual(ClientAppTypeEnum.Values.Spa, V1ClientController.ToApi(V1ClientApplicationType.Spa).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Zendesk() => Assert.AreEqual(ClientAppTypeEnum.Values.Zendesk, V1ClientController.ToApi(V1ClientApplicationType.Zendesk).Value);
        [TestMethod]
        public void ToApi_ClientApplicationType_Zoom() => Assert.AreEqual(ClientAppTypeEnum.Values.Zoom, V1ClientController.ToApi(V1ClientApplicationType.Zoom).Value);

        [TestMethod]
        public void ToApi_TokenEndpointAuthMethod_None() => Assert.AreEqual(ClientTokenEndpointAuthMethodEnum.Values.None, V1ClientController.ToApi(V1ClientTokenEndpointAuthMethod.None).Value);

        [TestMethod]
        public void ToApi_TokenEndpointAuthMethod_ClientSecretPost() => Assert.AreEqual(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost, V1ClientController.ToApi(V1ClientTokenEndpointAuthMethod.ClientSecretPost).Value);

        [TestMethod]
        public void ToApi_TokenEndpointAuthMethod_ClientSecretBasic() => Assert.AreEqual(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic, V1ClientController.ToApi(V1ClientTokenEndpointAuthMethod.ClientSecretBasic).Value);

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        public void TokenEndpointAuthMethod_Roundtrip_None()
        {
            var input = new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.None);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void TokenEndpointAuthMethod_Roundtrip_ClientSecretPost()
        {
            var input = new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void TokenEndpointAuthMethod_Roundtrip_ClientSecretBasic()
        {
            var input = new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void RefreshTokenRotationType_Roundtrip_Rotating()
        {
            var input = new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void RefreshTokenRotationType_Roundtrip_NonRotating()
        {
            var input = new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void RefreshTokenExpirationType_Roundtrip_Expiring()
        {
            var input = new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void RefreshTokenExpirationType_Roundtrip_NonExpiring()
        {
            var input = new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void OrganizationUsage_Roundtrip_Deny()
        {
            var input = new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Deny);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void OrganizationUsage_Roundtrip_Allow()
        {
            var input = new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Allow);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void OrganizationUsage_Roundtrip_Require()
        {
            var input = new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void OrganizationRequireBehavior_Roundtrip_NoPrompt()
        {
            var input = new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.NoPrompt);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void OrganizationRequireBehavior_Roundtrip_PreLoginPrompt()
        {
            var input = new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void OrganizationRequireBehavior_Roundtrip_PostLoginPrompt()
        {
            var input = new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void ComplianceLevel_Roundtrip_None()
        {
            var input = new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.None);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void ComplianceLevel_Roundtrip_Fapi1AdvPkjPar()
        {
            var input = new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void ComplianceLevel_Roundtrip_Fapi1AdvMtlsPar()
        {
            var input = new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void LogoutInitiators_Roundtrip_RpLogout()
        {
            var input = new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)).Value);
        }

        [TestMethod]
        public void LogoutInitiators_Roundtrip_IdpLogout()
        {
            var input = new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)).Value);
        }

        [TestMethod]
        public void LogoutInitiators_Roundtrip_PasswordChanged()
        {
            var input = new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)).Value);
        }

        [TestMethod]
        public void LogoutInitiators_Roundtrip_SessionExpired()
        {
            var input = new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)).Value);
        }

        [TestMethod]
        public void LogoutInitiatorModes_Roundtrip_All()
        {
            var input = new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

        [TestMethod]
        public void LogoutInitiatorModes_Roundtrip_Custom()
        {
            var input = new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom);
            Assert.AreEqual(input.Value, V1ClientController.ToApi(V1ClientController.FromApi(input)!.Value).Value);
        }

    }

}
