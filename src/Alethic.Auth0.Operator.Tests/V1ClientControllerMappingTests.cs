using System.Collections.Generic;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Client.V1;

using Auth0.ManagementApi.Models;

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
            Assert.IsNull(V1ClientController.FromApi((Client?)null));
        }

        [TestMethod]
        public void FromApi_SigningKey_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((SigningKey?)null));
        }

        [TestMethod]
        public void FromApi_ClientResourceServerAssociation_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientResourceServerAssociation?)null));
        }

        [TestMethod]
        public void FromApi_RefreshToken_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((RefreshToken?)null));
        }

        [TestMethod]
        public void FromApi_OidcLogoutConfig_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((OidcLogoutConfig?)null));
        }

        [TestMethod]
        public void FromApi_BackchannelLogoutInitiators_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((BackchannelLogoutInitiators?)null));
        }

        [TestMethod]
        public void FromApi_JwtConfiguration_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((JwtConfiguration?)null));
        }

        [TestMethod]
        public void FromApi_Scopes_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((Scopes?)null));
        }

        [TestMethod]
        public void FromApi_ScopeEntry_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ScopeEntry?)null));
        }

        [TestMethod]
        public void FromApi_EncryptionKey_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((EncryptionKey?)null));
        }

        [TestMethod]
        public void FromApi_DefaultOrganization_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((DefaultOrganization?)null));
        }

        [TestMethod]
        public void FromApi_Mobile_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((Mobile?)null));
        }

        [TestMethod]
        public void FromApi_Addons_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((Addons?)null));
        }

        // ──────────────────────── FromApi TokenEndpointAuthMethod ─────────────────

        [TestMethod]
        [DataRow(TokenEndpointAuthMethod.None, V1ClientTokenEndpointAuthMethod.None)]
        [DataRow(TokenEndpointAuthMethod.ClientSecretPost, V1ClientTokenEndpointAuthMethod.ClientSecretPost)]
        [DataRow(TokenEndpointAuthMethod.ClientSecretBasic, V1ClientTokenEndpointAuthMethod.ClientSecretBasic)]
        public void FromApi_TokenEndpointAuthMethod_MapsCorrectly(TokenEndpointAuthMethod input, V1ClientTokenEndpointAuthMethod expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((TokenEndpointAuthMethod?)input));
        }

        [TestMethod]
        public void FromApi_TokenEndpointAuthMethod_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((TokenEndpointAuthMethod?)null));
        }

        // ──────────────────────── FromApi RefreshTokenRotationType ────────────────

        [TestMethod]
        [DataRow(RefreshTokenRotationType.Rotating, V1ClientRefreshTokenRotationType.Rotating)]
        [DataRow(RefreshTokenRotationType.NonRotating, V1ClientRefreshTokenRotationType.NonRotating)]
        public void FromApi_RefreshTokenRotationType_MapsCorrectly(RefreshTokenRotationType input, V1ClientRefreshTokenRotationType expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((RefreshTokenRotationType?)input));
        }

        [TestMethod]
        public void FromApi_RefreshTokenRotationType_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((RefreshTokenRotationType?)null));
        }

        // ──────────────────────── FromApi RefreshTokenExpirationType ──────────────

        [TestMethod]
        [DataRow(RefreshTokenExpirationType.Expiring, V1ClientRefreshTokenExpirationType.Expiring)]
        [DataRow(RefreshTokenExpirationType.NonExpiring, V1ClientRefreshTokenExpirationType.NonExpiring)]
        public void FromApi_RefreshTokenExpirationType_MapsCorrectly(RefreshTokenExpirationType input, V1ClientRefreshTokenExpirationType expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((RefreshTokenExpirationType?)input));
        }

        [TestMethod]
        public void FromApi_RefreshTokenExpirationType_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((RefreshTokenExpirationType?)null));
        }

        // ──────────────────────── FromApi OrganizationUsage ───────────────────────

        [TestMethod]
        [DataRow(OrganizationUsage.Deny, V1ClientOrganizationUsage.Deny)]
        [DataRow(OrganizationUsage.Allow, V1ClientOrganizationUsage.Allow)]
        [DataRow(OrganizationUsage.Require, V1ClientOrganizationUsage.Require)]
        public void FromApi_OrganizationUsage_MapsCorrectly(OrganizationUsage input, V1ClientOrganizationUsage expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((OrganizationUsage?)input));
        }

        [TestMethod]
        public void FromApi_OrganizationUsage_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((OrganizationUsage?)null));
        }

        // ──────────────────────── FromApi OrganizationRequireBehavior ─────────────

        [TestMethod]
        [DataRow(OrganizationRequireBehavior.NoPrompt, V1ClientOrganizationRequireBehavior.NoPrompt)]
        [DataRow(OrganizationRequireBehavior.PreLoginPrompt, V1ClientOrganizationRequireBehavior.PreLoginPrompt)]
        [DataRow(OrganizationRequireBehavior.PostLoginPrompt, V1ClientOrganizationRequireBehavior.PostLoginPrompt)]
        public void FromApi_OrganizationRequireBehavior_MapsCorrectly(OrganizationRequireBehavior input, V1ClientOrganizationRequireBehavior expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((OrganizationRequireBehavior?)input));
        }

        [TestMethod]
        public void FromApi_OrganizationRequireBehavior_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((OrganizationRequireBehavior?)null));
        }

        // ──────────────────────── FromApi LogoutInitiators ────────────────────────

        [TestMethod]
        [DataRow(LogoutInitiators.RpLogout, V1ClientLogoutInitiators.RpLogout)]
        [DataRow(LogoutInitiators.IdpLogout, V1ClientLogoutInitiators.IdpLogout)]
        [DataRow(LogoutInitiators.PasswordChanged, V1ClientLogoutInitiators.PasswordChanged)]
        [DataRow(LogoutInitiators.SessionExpired, V1ClientLogoutInitiators.SessionExpired)]
        public void FromApi_LogoutInitiators_MapsCorrectly(LogoutInitiators input, V1ClientLogoutInitiators expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi(input));
        }

        // ──────────────────────── FromApi LogoutInitiatorModes ────────────────────

        [TestMethod]
        [DataRow(LogoutInitiatorModes.All, V1ClientLogoutInitiatorModes.All)]
        [DataRow(LogoutInitiatorModes.Custom, V1ClientLogoutInitiatorModes.Custom)]
        public void FromApi_LogoutInitiatorModes_MapsCorrectly(LogoutInitiatorModes input, V1ClientLogoutInitiatorModes expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((LogoutInitiatorModes?)input));
        }

        [TestMethod]
        public void FromApi_LogoutInitiatorModes_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((LogoutInitiatorModes?)null));
        }

        // ──────────────────────── FromApi ComplianceLevel ────────────────────────

        [TestMethod]
        [DataRow(ComplianceLevel.NONE, V1ClientComplianceLevel.NONE)]
        [DataRow(ComplianceLevel.FAPI1_ADV_PKJ_PAR, V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR)]
        [DataRow(ComplianceLevel.FAPI1_ADV_MTLS_PAR, V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR)]
        public void FromApi_ComplianceLevel_MapsCorrectly(ComplianceLevel input, V1ClientComplianceLevel expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((ComplianceLevel?)input));
        }

        [TestMethod]
        public void FromApi_ComplianceLevel_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ComplianceLevel?)null));
        }

        // ──────────────────────── FromApi Flows ───────────────────────────────────

        [TestMethod]
        public void FromApi_Flows_ClientCredentials_MapsCorrectly()
        {
            Assert.AreEqual(V1ClientFlows.ClientCredentials, V1ClientController.FromApi(Flows.ClientCredentials));
        }

        // ──────────────────────── FromApi ClientApplicationType ──────────────────

        [TestMethod]
        [DataRow(ClientApplicationType.Native, V1ClientApplicationType.Native)]
        [DataRow(ClientApplicationType.NonInteractive, V1ClientApplicationType.NonInteractive)]
        [DataRow(ClientApplicationType.Spa, V1ClientApplicationType.Spa)]
        [DataRow(ClientApplicationType.RegularWeb, V1ClientApplicationType.RegularWeb)]
        [DataRow(ClientApplicationType.Box, V1ClientApplicationType.Box)]
        [DataRow(ClientApplicationType.Cloudbees, V1ClientApplicationType.Cloudbees)]
        [DataRow(ClientApplicationType.Concur, V1ClientApplicationType.Concur)]
        [DataRow(ClientApplicationType.Dropbox, V1ClientApplicationType.Dropbox)]
        [DataRow(ClientApplicationType.Echosign, V1ClientApplicationType.Echosign)]
        [DataRow(ClientApplicationType.Egnyte, V1ClientApplicationType.Egnyte)]
        [DataRow(ClientApplicationType.MsCrm, V1ClientApplicationType.MsCrm)]
        [DataRow(ClientApplicationType.NewRelic, V1ClientApplicationType.NewRelic)]
        [DataRow(ClientApplicationType.Office365, V1ClientApplicationType.Office365)]
        [DataRow(ClientApplicationType.Rms, V1ClientApplicationType.Rms)]
        [DataRow(ClientApplicationType.Salesforce, V1ClientApplicationType.Salesforce)]
        [DataRow(ClientApplicationType.Sentry, V1ClientApplicationType.Sentry)]
        [DataRow(ClientApplicationType.SharePoint, V1ClientApplicationType.SharePoint)]
        [DataRow(ClientApplicationType.Slack, V1ClientApplicationType.Slack)]
        [DataRow(ClientApplicationType.SpringCm, V1ClientApplicationType.SpringCm)]
        [DataRow(ClientApplicationType.Zendesk, V1ClientApplicationType.Zendesk)]
        [DataRow(ClientApplicationType.Zoom, V1ClientApplicationType.Zoom)]
        [DataRow(ClientApplicationType.ResourceServer, V1ClientApplicationType.ResourceServer)]
        [DataRow(ClientApplicationType.ExpressConfiguration, V1ClientApplicationType.ExpressConfiguration)]
        [DataRow(ClientApplicationType.SsoIntegration, V1ClientApplicationType.SsoIntegration)]
        [DataRow(ClientApplicationType.Oag, V1ClientApplicationType.Oag)]
        public void FromApi_ClientApplicationType_MapsCorrectly(ClientApplicationType input, V1ClientApplicationType expected)
        {
            Assert.AreEqual(expected, V1ClientController.FromApi((ClientApplicationType?)input));
        }

        [TestMethod]
        public void FromApi_ClientApplicationType_Null_Returns_Null()
        {
            Assert.IsNull(V1ClientController.FromApi((ClientApplicationType?)null));
        }

        // ──────────────────────── FromApi value objects ───────────────────────────

        [TestMethod]
        public void FromApi_SigningKey_MapsProperties()
        {
            var source = new SigningKey { Cert = "cert", Key = "key", Pkcs7 = "pkcs7" };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("cert", result.Cert);
            Assert.AreEqual("key", result.Key);
            Assert.AreEqual("pkcs7", result.Pkcs7);
        }

        [TestMethod]
        public void FromApi_ClientResourceServerAssociation_MapsProperties()
        {
            var source = new ClientResourceServerAssociation { Identifier = "https://api.example.com", Scopes = ["read:data"] };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("https://api.example.com", result.Identifier);
            CollectionAssert.AreEqual(new[] { "read:data" }, result.Scopes);
        }

        [TestMethod]
        public void FromApi_EncryptionKey_MapsProperties()
        {
            var source = new EncryptionKey { Certificate = "cert", PublicKey = "pub", Subject = "sub" };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("cert", result.Certificate);
            Assert.AreEqual("pub", result.PublicKey);
            Assert.AreEqual("sub", result.Subject);
        }

        [TestMethod]
        public void FromApi_ScopeEntry_MapsActions()
        {
            var source = new ScopeEntry { Actions = ["read", "write"] };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "read", "write" }, result.Actions);
        }

        [TestMethod]
        public void FromApi_JwtConfiguration_MapsProperties()
        {
            var source = new JwtConfiguration { IsSecretEncoded = true, LifetimeInSeconds = 3600, SigningAlgorithm = "RS256" };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsSecretEncoded);
            Assert.AreEqual(3600, result.LifetimeInSeconds);
            Assert.AreEqual("RS256", result.SigningAlgorithm);
        }

        [TestMethod]
        public void FromApi_RefreshToken_MapsProperties()
        {
            var source = new RefreshToken
            {
                RotationType = RefreshTokenRotationType.Rotating,
                ExpirationType = RefreshTokenExpirationType.Expiring,
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
            var source = new OidcLogoutConfig { BackchannelLogoutUrls = ["https://example.com/logout"] };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new[] { "https://example.com/logout" }, result.BackchannelLogoutUrls);
        }

        [TestMethod]
        public void FromApi_BackchannelLogoutInitiators_MapsProperties()
        {
            var source = new BackchannelLogoutInitiators
            {
                Mode = LogoutInitiatorModes.Custom,
                SelectedInitiators = [LogoutInitiators.RpLogout, LogoutInitiators.IdpLogout],
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
            var source = new DefaultOrganization { OrganizationId = "org_123", Flows = [Flows.ClientCredentials] };
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
            var source = new Mobile
            {
                Ios = new Mobile.MobileIos { AppBundleIdentifier = "com.example.app", TeamId = "TEAM123" },
                Android = new Mobile.MobileAndroid { AppPackageName = "com.example.app", KeystoreHash = "hash" },
            };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Ios);
            Assert.AreEqual("com.example.app", result.Ios.AppBundleIdentifier);
            Assert.AreEqual("TEAM123", result.Ios.TeamId);
            Assert.IsNotNull(result.Android);
            Assert.AreEqual("com.example.app", result.Android.AppPackageName);
            Assert.AreEqual("hash", result.Android.KeystoreHash);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyIos_Returns_NullIos()
        {
            var source = new Mobile { Ios = new Mobile.MobileIos { AppBundleIdentifier = null, TeamId = null } };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Ios);
        }

        [TestMethod]
        public void FromApi_Mobile_WithEmptyAndroid_Returns_NullAndroid()
        {
            var source = new Mobile { Android = new Mobile.MobileAndroid { AppPackageName = null, KeystoreHash = null } };
            var result = V1ClientController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Android);
        }

        [TestMethod]
        public void FromApi_Client_MapsScalarProperties()
        {
            var source = new Client
            {
                Name = "My App",
                Description = "Test app",
                LogoUri = "https://example.com/logo.png",
                OidcConformant = true,
                Sso = false,
                CrossOriginAuthentication = true,
                IsFirstParty = true,
                ApplicationType = ClientApplicationType.RegularWeb,
                TokenEndpointAuthMethod = TokenEndpointAuthMethod.ClientSecretPost,
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
            var source = new Client
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
        [DataRow(V1ClientComplianceLevel.NONE, ComplianceLevel.NONE)]
        [DataRow(V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR, ComplianceLevel.FAPI1_ADV_PKJ_PAR)]
        [DataRow(V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR, ComplianceLevel.FAPI1_ADV_MTLS_PAR)]
        public void ToApi_ComplianceLevel_MapsCorrectly(V1ClientComplianceLevel input, ComplianceLevel expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientOrganizationRequireBehavior.NoPrompt, OrganizationRequireBehavior.NoPrompt)]
        [DataRow(V1ClientOrganizationRequireBehavior.PreLoginPrompt, OrganizationRequireBehavior.PreLoginPrompt)]
        [DataRow(V1ClientOrganizationRequireBehavior.PostLoginPrompt, OrganizationRequireBehavior.PostLoginPrompt)]
        public void ToApi_OrganizationRequireBehavior_MapsCorrectly(V1ClientOrganizationRequireBehavior input, OrganizationRequireBehavior expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientOrganizationUsage.Deny, OrganizationUsage.Deny)]
        [DataRow(V1ClientOrganizationUsage.Allow, OrganizationUsage.Allow)]
        [DataRow(V1ClientOrganizationUsage.Require, OrganizationUsage.Require)]
        public void ToApi_OrganizationUsage_MapsCorrectly(V1ClientOrganizationUsage input, OrganizationUsage expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientRefreshTokenRotationType.Rotating, RefreshTokenRotationType.Rotating)]
        [DataRow(V1ClientRefreshTokenRotationType.NonRotating, RefreshTokenRotationType.NonRotating)]
        public void ToApi_RefreshTokenRotationType_MapsCorrectly(V1ClientRefreshTokenRotationType input, RefreshTokenRotationType expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientRefreshTokenExpirationType.Expiring, RefreshTokenExpirationType.Expiring)]
        [DataRow(V1ClientRefreshTokenExpirationType.NonExpiring, RefreshTokenExpirationType.NonExpiring)]
        public void ToApi_RefreshTokenExpirationType_MapsCorrectly(V1ClientRefreshTokenExpirationType input, RefreshTokenExpirationType expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientLogoutInitiatorModes.All, LogoutInitiatorModes.All)]
        [DataRow(V1ClientLogoutInitiatorModes.Custom, LogoutInitiatorModes.Custom)]
        public void ToApi_LogoutInitiatorModes_MapsCorrectly(V1ClientLogoutInitiatorModes input, LogoutInitiatorModes expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientLogoutInitiators.RpLogout, LogoutInitiators.RpLogout)]
        [DataRow(V1ClientLogoutInitiators.IdpLogout, LogoutInitiators.IdpLogout)]
        [DataRow(V1ClientLogoutInitiators.PasswordChanged, LogoutInitiators.PasswordChanged)]
        [DataRow(V1ClientLogoutInitiators.SessionExpired, LogoutInitiators.SessionExpired)]
        public void ToApi_LogoutInitiators_MapsCorrectly(V1ClientLogoutInitiators input, LogoutInitiators expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientApplicationType.Box, ClientApplicationType.Box)]
        [DataRow(V1ClientApplicationType.Cloudbees, ClientApplicationType.Cloudbees)]
        [DataRow(V1ClientApplicationType.Concur, ClientApplicationType.Concur)]
        [DataRow(V1ClientApplicationType.Dropbox, ClientApplicationType.Dropbox)]
        [DataRow(V1ClientApplicationType.Echosign, ClientApplicationType.Echosign)]
        [DataRow(V1ClientApplicationType.Egnyte, ClientApplicationType.Egnyte)]
        [DataRow(V1ClientApplicationType.MsCrm, ClientApplicationType.MsCrm)]
        [DataRow(V1ClientApplicationType.Native, ClientApplicationType.Native)]
        [DataRow(V1ClientApplicationType.NewRelic, ClientApplicationType.NewRelic)]
        [DataRow(V1ClientApplicationType.NonInteractive, ClientApplicationType.NonInteractive)]
        [DataRow(V1ClientApplicationType.Office365, ClientApplicationType.Office365)]
        [DataRow(V1ClientApplicationType.RegularWeb, ClientApplicationType.RegularWeb)]
        [DataRow(V1ClientApplicationType.Rms, ClientApplicationType.Rms)]
        [DataRow(V1ClientApplicationType.Salesforce, ClientApplicationType.Salesforce)]
        [DataRow(V1ClientApplicationType.Sentry, ClientApplicationType.Sentry)]
        [DataRow(V1ClientApplicationType.SharePoint, ClientApplicationType.SharePoint)]
        [DataRow(V1ClientApplicationType.Slack, ClientApplicationType.Slack)]
        [DataRow(V1ClientApplicationType.SpringCm, ClientApplicationType.SpringCm)]
        [DataRow(V1ClientApplicationType.Spa, ClientApplicationType.Spa)]
        [DataRow(V1ClientApplicationType.Zendesk, ClientApplicationType.Zendesk)]
        [DataRow(V1ClientApplicationType.Zoom, ClientApplicationType.Zoom)]
        public void ToApi_ClientApplicationType_MapsCorrectly(V1ClientApplicationType input, ClientApplicationType expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ClientTokenEndpointAuthMethod.None, TokenEndpointAuthMethod.None)]
        [DataRow(V1ClientTokenEndpointAuthMethod.ClientSecretPost, TokenEndpointAuthMethod.ClientSecretPost)]
        [DataRow(V1ClientTokenEndpointAuthMethod.ClientSecretBasic, TokenEndpointAuthMethod.ClientSecretBasic)]
        public void ToApi_TokenEndpointAuthMethod_MapsCorrectly(V1ClientTokenEndpointAuthMethod input, TokenEndpointAuthMethod expected)
        {
            Assert.AreEqual(expected, V1ClientController.ToApi(input));
        }

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        [DataRow(TokenEndpointAuthMethod.None)]
        [DataRow(TokenEndpointAuthMethod.ClientSecretPost)]
        [DataRow(TokenEndpointAuthMethod.ClientSecretBasic)]
        public void TokenEndpointAuthMethod_Roundtrip(TokenEndpointAuthMethod input)
        {
            var op = V1ClientController.FromApi((TokenEndpointAuthMethod?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(RefreshTokenRotationType.Rotating)]
        [DataRow(RefreshTokenRotationType.NonRotating)]
        public void RefreshTokenRotationType_Roundtrip(RefreshTokenRotationType input)
        {
            var op = V1ClientController.FromApi((RefreshTokenRotationType?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(RefreshTokenExpirationType.Expiring)]
        [DataRow(RefreshTokenExpirationType.NonExpiring)]
        public void RefreshTokenExpirationType_Roundtrip(RefreshTokenExpirationType input)
        {
            var op = V1ClientController.FromApi((RefreshTokenExpirationType?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(OrganizationUsage.Deny)]
        [DataRow(OrganizationUsage.Allow)]
        [DataRow(OrganizationUsage.Require)]
        public void OrganizationUsage_Roundtrip(OrganizationUsage input)
        {
            var op = V1ClientController.FromApi((OrganizationUsage?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(OrganizationRequireBehavior.NoPrompt)]
        [DataRow(OrganizationRequireBehavior.PreLoginPrompt)]
        [DataRow(OrganizationRequireBehavior.PostLoginPrompt)]
        public void OrganizationRequireBehavior_Roundtrip(OrganizationRequireBehavior input)
        {
            var op = V1ClientController.FromApi((OrganizationRequireBehavior?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(ComplianceLevel.NONE)]
        [DataRow(ComplianceLevel.FAPI1_ADV_PKJ_PAR)]
        [DataRow(ComplianceLevel.FAPI1_ADV_MTLS_PAR)]
        public void ComplianceLevel_Roundtrip(ComplianceLevel input)
        {
            var op = V1ClientController.FromApi((ComplianceLevel?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(LogoutInitiators.RpLogout)]
        [DataRow(LogoutInitiators.IdpLogout)]
        [DataRow(LogoutInitiators.PasswordChanged)]
        [DataRow(LogoutInitiators.SessionExpired)]
        public void LogoutInitiators_Roundtrip(LogoutInitiators input)
        {
            var op = V1ClientController.FromApi(input);
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

        [TestMethod]
        [DataRow(LogoutInitiatorModes.All)]
        [DataRow(LogoutInitiatorModes.Custom)]
        public void LogoutInitiatorModes_Roundtrip(LogoutInitiatorModes input)
        {
            var op = V1ClientController.FromApi((LogoutInitiatorModes?)input)!.Value;
            Assert.AreEqual(input, V1ClientController.ToApi(op));
        }

    }

}
