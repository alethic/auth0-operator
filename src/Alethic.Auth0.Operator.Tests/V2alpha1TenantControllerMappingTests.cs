using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Models.Prompts;

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
            Assert.IsNull(V2alpha1TenantController.FromApi((Prompt?)null));
        }

        [TestMethod]
        public void FromApi_Branding_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((Branding?)null));
        }

        [TestMethod]
        public void FromApi_BrandingColors_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((BrandingColors?)null));
        }

        [TestMethod]
        public void FromApi_TenantSettings_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantSettings?)null));
        }

        [TestMethod]
        public void FromApi_TenantMtls_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantMtls?)null));
        }

        [TestMethod]
        public void FromApi_SessionCookie_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((SessionCookie?)null));
        }

        [TestMethod]
        public void FromApi_TenantGuardianMfaPage_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantGuardianMfaPage?)null));
        }

        [TestMethod]
        public void FromApi_TenantErrorPage_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantErrorPage?)null));
        }

        [TestMethod]
        public void FromApi_TenantDeviceFlow_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantDeviceFlow?)null));
        }

        [TestMethod]
        public void FromApi_TenantChangePassword_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantChangePassword?)null));
        }

        [TestMethod]
        public void FromApi_TenantFlags_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantFlags?)null));
        }

        // ──────────────────────── FromApi string UniversalLoginExperience ─────────

        [TestMethod]
        [DataRow("new", V2alpha1TenantUniversalLoginExperience.New)]
        [DataRow("classic", V2alpha1TenantUniversalLoginExperience.Classic)]
        public void FromApi_UniversalLoginExperience_MapsCorrectly(string input, V2alpha1TenantUniversalLoginExperience expected)
        {
            Assert.AreEqual(expected, V2alpha1TenantController.FromApi(input));
        }

        [TestMethod]
        public void FromApi_UniversalLoginExperience_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((string?)null));
        }

        // ──────────────────────── FromApi TenantDeviceFlowCharset ─────────────────

        [TestMethod]
        [DataRow(TenantDeviceFlowCharset.Base20, V2alpha1TenantCharset.Base20)]
        [DataRow(TenantDeviceFlowCharset.Digits, V2alpha1TenantCharset.Digits)]
        public void FromApi_TenantDeviceFlowCharset_MapsCorrectly(TenantDeviceFlowCharset input, V2alpha1TenantCharset expected)
        {
            Assert.AreEqual(expected, V2alpha1TenantController.FromApi((TenantDeviceFlowCharset?)input));
        }

        [TestMethod]
        public void FromApi_TenantDeviceFlowCharset_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha1TenantController.FromApi((TenantDeviceFlowCharset?)null));
        }

        // ──────────────────────── FromApi value objects ───────────────────────────

        [TestMethod]
        public void FromApi_Prompt_MapsProperties()
        {
            var source = new Prompt { IdentifierFirst = true, WebAuthnPlatformFirstFactor = false };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IdentifierFirst);
            Assert.AreEqual(false, result.WebauthnPlatformFirstFactor);
        }

        [TestMethod]
        public void FromApi_Branding_MapsProperties()
        {
            var source = new Branding { LogoUrl = "https://example.com/logo.png", FaviconUrl = "https://example.com/favicon.ico" };
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
            var source = new TenantMtls { EnableEndpointAliases = true };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.EnableEndpointAliases);
        }

        [TestMethod]
        public void FromApi_SessionCookie_MapsMode()
        {
            var source = new SessionCookie { Mode = "persistent" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("persistent", result.Mode);
        }

        [TestMethod]
        public void FromApi_TenantGuardianMfaPage_MapsProperties()
        {
            var source = new TenantGuardianMfaPage { Enabled = true, Html = "<html/>" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Enabled);
            Assert.AreEqual("<html/>", result.Html);
        }

        [TestMethod]
        public void FromApi_TenantErrorPage_MapsProperties()
        {
            var source = new TenantErrorPage { ShowLogLink = true, Url = "https://example.com/error", Html = "<html/>" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ShowLogLink);
            Assert.AreEqual("https://example.com/error", result.Url);
            Assert.AreEqual("<html/>", result.Html);
        }

        [TestMethod]
        public void FromApi_TenantDeviceFlow_MapsMask()
        {
            var source = new TenantDeviceFlow { Mask = "***-***", Charset = TenantDeviceFlowCharset.Digits };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual("***-***", result.Mask);
            Assert.AreEqual(V2alpha1TenantCharset.Digits, result.Charset);
        }

        [TestMethod]
        public void FromApi_TenantChangePassword_MapsProperties()
        {
            var source = new TenantChangePassword { Enabled = true, Html = "<html/>" };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Enabled);
            Assert.AreEqual("<html/>", result.Html);
        }

        [TestMethod]
        public void FromApi_TenantFlags_MapsBoolProperties()
        {
            var source = new TenantFlags { EnableSSO = true, EnablePipeline2 = false, RemoveAlgFromJwks = true };
            var result = V2alpha1TenantController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.EnableSSO);
            Assert.AreEqual(false, result.EnablePipeline2);
            Assert.AreEqual(true, result.RemoveAlgFromJwks);
        }

        // ──────────────────────── ToApi ───────────────────────────────────────────

        [TestMethod]
        [DataRow(V2alpha1TenantUniversalLoginExperience.New, "new")]
        [DataRow(V2alpha1TenantUniversalLoginExperience.Classic, "classic")]
        public void ToApi_UniversalLoginExperience_MapsCorrectly(V2alpha1TenantUniversalLoginExperience input, string expected)
        {
            Assert.AreEqual(expected, V2alpha1TenantController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V2alpha1TenantCharset.Base20, TenantDeviceFlowCharset.Base20)]
        [DataRow(V2alpha1TenantCharset.Digits, TenantDeviceFlowCharset.Digits)]
        public void ToApi_TenantCharset_MapsCorrectly(V2alpha1TenantCharset input, TenantDeviceFlowCharset expected)
        {
            Assert.AreEqual(expected, V2alpha1TenantController.ToApi(input));
        }

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        [DataRow(TenantDeviceFlowCharset.Base20)]
        [DataRow(TenantDeviceFlowCharset.Digits)]
        public void TenantDeviceFlowCharset_Roundtrip(TenantDeviceFlowCharset input)
        {
            var op = V2alpha1TenantController.FromApi((TenantDeviceFlowCharset?)input)!.Value;
            Assert.AreEqual(input, V2alpha1TenantController.ToApi(op));
        }

        [TestMethod]
        [DataRow("new")]
        [DataRow("classic")]
        public void UniversalLoginExperience_Roundtrip(string input)
        {
            var op = V2alpha1TenantController.FromApi(input)!.Value;
            Assert.AreEqual(input, V2alpha1TenantController.ToApi(op));
        }

    }

}
