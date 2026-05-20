using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Tenants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V2alpha1TenantControllerMappingTests
    {

        // ──────────────────────── FromApi null-guard tests ────────────────────────

        [TestMethod]
        public void FromApi_Prompt_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((GetSettingsResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_Branding_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((GetBrandingResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_BrandingColors_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((BrandingColors?)null));
        }

        [TestMethod]
        public void FromApi_TenantSettings_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((GetTenantSettingsResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_TenantMtls_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsMtls?)null));
        }

        [TestMethod]
        public void FromApi_SessionCookie_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((SessionCookieSchema?)null));
        }

        [TestMethod]
        public void FromApi_TenantGuardianMfaPage_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsGuardianPage?)null));
        }

        [TestMethod]
        public void FromApi_TenantErrorPage_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsErrorPage?)null));
        }

        [TestMethod]
        public void FromApi_TenantDeviceFlow_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsDeviceFlow?)null));
        }

        [TestMethod]
        public void FromApi_TenantChangePassword_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsPasswordPage?)null));
        }

        [TestMethod]
        public void FromApi_TenantFlags_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsFlags?)null));
        }

        // ──────────────────────── FromApi string UniversalLoginExperience ─────────

        [TestMethod]
        [DataRow("new", V2alpha1TenantUniversalLoginExperience.New)]
        [DataRow("classic", V2alpha1TenantUniversalLoginExperience.Classic)]
        public void FromApi_UniversalLoginExperience_MapsCorrectly(string inputValue, V2alpha1TenantUniversalLoginExperience expected)
        {
            Assert.AreEqual(expected, V2alpha1TenantController.FromApi(new UniversalLoginExperienceEnum(inputValue)));
        }

        [TestMethod]
        public void FromApi_UniversalLoginExperience_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((UniversalLoginExperienceEnum?)null));
        }

        // ──────────────────────── FromApi TenantDeviceFlowCharset ─────────────────

        [TestMethod]
        public void FromApi_TenantDeviceFlowCharset_Base20() => Assert.AreEqual(V2alpha1TenantCharset.Base20, V2alpha1TenantController.FromApi((TenantSettingsDeviceFlowCharset?)new TenantSettingsDeviceFlowCharset(TenantSettingsDeviceFlowCharset.Values.Base20)));
        [TestMethod]
        public void FromApi_TenantDeviceFlowCharset_Digits() => Assert.AreEqual(V2alpha1TenantCharset.Digits, V2alpha1TenantController.FromApi((TenantSettingsDeviceFlowCharset?)new TenantSettingsDeviceFlowCharset(TenantSettingsDeviceFlowCharset.Values.Digits)));

        [TestMethod]
        public void FromApi_TenantDeviceFlowCharset_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettingsDeviceFlowCharset?)null));
        }

        // ──────────────────────── FromApi value objects ───────────────────────────

        [TestMethod]
        public void FromApi_Prompt_MapsProperties()
        {
            var source = new GetSettingsResponseContent { IdentifierFirst = true, WebauthnPlatformFirstFactor = false };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IdentifierFirst);
            Assert.AreEqual(false, result.WebauthnPlatformFirstFactor);
        }

        [TestMethod]
        public void FromApi_Branding_MapsProperties()
        {
            var source = new GetBrandingResponseContent { LogoUrl = "https://example.com/logo.png", FaviconUrl = "https://example.com/favicon.ico" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("https://example.com/logo.png", result.LogoUrl);
            Assert.AreEqual("https://example.com/favicon.ico", result.FaviconUrl);
        }

        [TestMethod]
        public void FromApi_BrandingColors_MapsProperties()
        {
            var source = new BrandingColors { Primary = "#ff0000", PageBackground = "#ffffff" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("#ff0000", result.Primary);
            Assert.AreEqual("#ffffff", result.PageBackground);
        }

        [TestMethod]
        public void FromApi_TenantMtls_MapsProperties()
        {
            var source = new TenantSettingsMtls { EnableEndpointAliases = true };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.EnableEndpointAliases);
        }

        [TestMethod]
        public void FromApi_SessionCookie_MapsMode()
        {
            var source = new SessionCookieSchema { Mode = new SessionCookieModeEnum("persistent") };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha1TenantSessionCookieModeEnum.Persistent, result.Mode);
        }

        [TestMethod]
        public void FromApi_TenantGuardianMfaPage_MapsProperties()
        {
            var source = new TenantSettingsGuardianPage { Enabled = true, Html = "<html/>" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Enabled);
            Assert.AreEqual("<html/>", result.Html);
        }

        [TestMethod]
        public void FromApi_TenantErrorPage_MapsProperties()
        {
            var source = new TenantSettingsErrorPage { ShowLogLink = true, Url = "https://example.com/error", Html = "<html/>" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ShowLogLink);
            Assert.AreEqual("https://example.com/error", result.Url);
            Assert.AreEqual("<html/>", result.Html);
        }

        [TestMethod]
        public void FromApi_TenantDeviceFlow_MapsMask()
        {
            var source = new TenantSettingsDeviceFlow { Mask = "***-***", Charset = new TenantSettingsDeviceFlowCharset(TenantSettingsDeviceFlowCharset.Values.Digits) };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("***-***", result.Mask);
            Assert.AreEqual(V2alpha1TenantCharset.Digits, result.Charset);
        }

        [TestMethod]
        public void FromApi_TenantChangePassword_MapsProperties()
        {
            var source = new TenantSettingsPasswordPage { Enabled = true, Html = "<html/>" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Enabled);
            Assert.AreEqual("<html/>", result.Html);
        }

        [TestMethod]
        public void FromApi_TenantFlags_MapsBoolProperties()
        {
            var source = new TenantSettingsFlags { EnableSso = true, EnablePipeline2 = false, RemoveAlgFromJwks = true };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.EnableSso);
            Assert.AreEqual(false, result.EnablePipeline2);
            Assert.AreEqual(true, result.RemoveAlgFromJwks);
        }

        [TestMethod]
        public void FromApi_TenantSettings_Maps_NewProperties()
        {
            var source = new GetTenantSettingsResponseContent
            {
                DefaultRedirectionUri = "https://example.com/callback",
                LegacySandboxVersion = "8",
                AllowOrganizationNameInAuthenticationApi = true,
                CustomizeMfaInPostloginAction = false,
                PushedAuthorizationRequestsSupported = true,
                ClientIdMetadataDocumentSupported = true,
                EnableAiGuide = false,
                PhoneConsolidatedExperience = true,
                OidcLogout = new TenantOidcLogoutSettings { RpLogoutEndSessionEndpointDiscovery = true },
                Sessions = new TenantSettingsSessions { OidcLogoutPromptEnabled = true },
                DefaultTokenQuota = new DefaultTokenQuota
                {
                    Clients = new TokenQuotaConfiguration
                    {
                        ClientCredentials = new TokenQuotaClientCredentials { Enforce = true, PerDay = 10, PerHour = 2 }
                    }
                },
                ResourceParameterProfile = new TenantSettingsResourceParameterProfile(TenantSettingsResourceParameterProfile.Values.Audience),
                DynamicClientRegistrationSecurityMode = new TenantSettingsDynamicClientRegistrationSecurityMode(TenantSettingsDynamicClientRegistrationSecurityMode.Values.Strict),
                AuthorizationResponseIssParameterSupported = true,
                SkipNonVerifiableCallbackUriConfirmationPrompt = false,
                SessionCookie = new SessionCookieSchema { Mode = new SessionCookieModeEnum("persistent") }
            };

            var result = V2alpha1TenantController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("https://example.com/callback", result.DefaultRedirectionUri);
            Assert.AreEqual("8", result.LegacySandboxVersion);
            Assert.AreEqual(true, result.AllowOrganizationNameInAuthenticationApi);
            Assert.AreEqual(false, result.CustomizeMfaInPostloginAction);
            Assert.AreEqual(true, result.PushedAuthorizationRequestsSupported);
            Assert.AreEqual(true, result.ClientIdMetadataDocumentSupported);
            Assert.AreEqual(false, result.EnableAiGuide);
            Assert.AreEqual(true, result.PhoneConsolidatedExperience);
            Assert.AreEqual(true, result.OidcLogout?.RpLogoutEndSessionEndpointDiscovery);
            Assert.AreEqual(true, result.Sessions?.OidcLogoutPromptEnabled);
            Assert.AreEqual(true, result.DefaultTokenQuota?.Clients?.ClientCredentials?.Enforce);
            Assert.AreEqual(10, result.DefaultTokenQuota?.Clients?.ClientCredentials?.PerDay);
            Assert.AreEqual(2, result.DefaultTokenQuota?.Clients?.ClientCredentials?.PerHour);
            Assert.AreEqual(V2alpha1TenantResourceParameterProfile.Audience, result.ResourceParameterProfile);
            Assert.AreEqual(V2alpha1TenantDynamicClientRegistrationSecurityMode.Strict, result.DynamicClientRegistrationSecurityMode);
            Assert.AreEqual(true, result.AuthorizationResponseIssParameterSupported);
            Assert.AreEqual(false, result.SkipNonVerifiableCallbackUriConfirmationPrompt);
            Assert.AreEqual(V2alpha1TenantSessionCookieModeEnum.Persistent, result.SessionCookie?.Mode);
        }

        [TestMethod]
        public void ToApi_TenantSettings_Maps_NewProperties()
        {
            var source = new V2alpha1TenantSettings
            {
                DefaultRedirectionUri = "https://example.com/callback",
                LegacySandboxVersion = "8",
                AllowOrganizationNameInAuthenticationApi = true,
                CustomizeMfaInPostloginAction = false,
                PushedAuthorizationRequestsSupported = true,
                ClientIdMetadataDocumentSupported = true,
                EnableAiGuide = false,
                PhoneConsolidatedExperience = true,
                OidcLogout = new V2alpha1TenantOidcLogout { RpLogoutEndSessionEndpointDiscovery = true },
                Sessions = new V2alpha1TenantSessions { OidcLogoutPromptEnabled = true },
                DefaultTokenQuota = new V2alpha1TenantDefaultTokenQuota
                {
                    Clients = new V2alpha1TenantTokenQuotaConfiguration
                    {
                        ClientCredentials = new V2alpha1TenantTokenQuotaClientCredentials { Enforce = true, PerDay = 10, PerHour = 2 }
                    }
                },
                ResourceParameterProfile = V2alpha1TenantResourceParameterProfile.Audience,
                DynamicClientRegistrationSecurityMode = V2alpha1TenantDynamicClientRegistrationSecurityMode.Strict,
                AuthorizationResponseIssParameterSupported = true,
                SkipNonVerifiableCallbackUriConfirmationPrompt = false,
                SessionCookie = new V2alpha1TenantSessionCookie { Mode = V2alpha1TenantSessionCookieModeEnum.Persistent }
            };

            var target = new UpdateTenantSettingsRequestContent();
            V2alpha1TenantController.ApplyToApi(source, target);

            Assert.AreEqual("https://example.com/callback", target.DefaultRedirectionUri);
            Assert.AreEqual("8", target.LegacySandboxVersion);
            Assert.AreEqual(true, target.AllowOrganizationNameInAuthenticationApi.Value);
            Assert.AreEqual(false, target.CustomizeMfaInPostloginAction.Value);
            Assert.AreEqual(true, target.PushedAuthorizationRequestsSupported.Value);
            Assert.AreEqual(true, target.ClientIdMetadataDocumentSupported);
            Assert.AreEqual(false, target.EnableAiGuide);
            Assert.AreEqual(true, target.PhoneConsolidatedExperience);
            Assert.AreEqual(true, target.OidcLogout?.RpLogoutEndSessionEndpointDiscovery);
            Assert.AreEqual(true, target.Sessions.Value?.OidcLogoutPromptEnabled);
            Assert.AreEqual(true, target.DefaultTokenQuota.Value?.Clients?.ClientCredentials?.Enforce);
            Assert.AreEqual(10, target.DefaultTokenQuota.Value?.Clients?.ClientCredentials?.PerDay);
            Assert.AreEqual(2, target.DefaultTokenQuota.Value?.Clients?.ClientCredentials?.PerHour);
            Assert.AreEqual(TenantSettingsResourceParameterProfile.Values.Audience, target.ResourceParameterProfile?.Value);
            Assert.AreEqual(TenantSettingsDynamicClientRegistrationSecurityMode.Values.Strict, target.DynamicClientRegistrationSecurityMode?.Value);
            Assert.AreEqual(true, target.AuthorizationResponseIssParameterSupported.Value);
            Assert.AreEqual(false, target.SkipNonVerifiableCallbackUriConfirmationPrompt.Value);
            Assert.AreEqual("persistent", target.SessionCookie.Value?.Mode.Value);
        }

        // ──────────────────────── ToApi ───────────────────────────────────────────

        [TestMethod]
        [DataRow(V2alpha1TenantUniversalLoginExperience.New, "new")]
        [DataRow(V2alpha1TenantUniversalLoginExperience.Classic, "classic")]
        public void ToApi_UniversalLoginExperience_MapsCorrectly(V2alpha1TenantUniversalLoginExperience input, string expectedValue)
        {
            Assert.AreEqual(expectedValue, V2alpha1TenantController.ToApi(input).Value);
        }

        [TestMethod]
        public void ToApi_TenantCharset_Base20() => Assert.AreEqual(TenantSettingsDeviceFlowCharset.Values.Base20, V2alpha1TenantController.ToApi(V2alpha1TenantCharset.Base20).Value);
        [TestMethod]
        public void ToApi_TenantCharset_Digits() => Assert.AreEqual(TenantSettingsDeviceFlowCharset.Values.Digits, V2alpha1TenantController.ToApi(V2alpha1TenantCharset.Digits).Value);

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        public void TenantDeviceFlowCharset_Roundtrip_Base20() { var v = new TenantSettingsDeviceFlowCharset(TenantSettingsDeviceFlowCharset.Values.Base20); var op = V2alpha1TenantController.FromApi((TenantSettingsDeviceFlowCharset?)v)!.Value; Assert.AreEqual(v.Value, V2alpha1TenantController.ToApi(op).Value); }
        [TestMethod]
        public void TenantDeviceFlowCharset_Roundtrip_Digits() { var v = new TenantSettingsDeviceFlowCharset(TenantSettingsDeviceFlowCharset.Values.Digits); var op = V2alpha1TenantController.FromApi((TenantSettingsDeviceFlowCharset?)v)!.Value; Assert.AreEqual(v.Value, V2alpha1TenantController.ToApi(op).Value); }

        [TestMethod]
        [DataRow("new")]
        [DataRow("classic")]
        public void UniversalLoginExperience_Roundtrip(string inputValue)
        {
            var op = V2alpha1TenantController.FromApi(new UniversalLoginExperienceEnum(inputValue))!.Value;
            Assert.AreEqual(inputValue, V2alpha1TenantController.ToApi(op).Value);
        }

    }

}
