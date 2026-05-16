using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1;

using Auth0.ManagementApi.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1alpha1BrandingThemeControllerMappingTests
    {

        // ──────────────────────── ToApi CaptchaWidgetTheme ────────────────────────

        [TestMethod]
        [DataRow(V1alpha1BrandingThemeCaptchaWidgetTheme.Light, CaptchaWidgetTheme.Light)]
        [DataRow(V1alpha1BrandingThemeCaptchaWidgetTheme.Dark, CaptchaWidgetTheme.Dark)]
        [DataRow(V1alpha1BrandingThemeCaptchaWidgetTheme.Auto, CaptchaWidgetTheme.Auto)]
        public void ToApi_CaptchaWidgetTheme_MapsCorrectly(V1alpha1BrandingThemeCaptchaWidgetTheme input, CaptchaWidgetTheme expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.ToApi(input));
        }

        // ──────────────────────── ToApi SocialButtonsLayout ───────────────────────

        [TestMethod]
        [DataRow(V1alpha1BrandingThemeSocialButtonsLayout.Top, SocialButtonsLayout.Top)]
        [DataRow(V1alpha1BrandingThemeSocialButtonsLayout.Bottom, SocialButtonsLayout.Bottom)]
        public void ToApi_SocialButtonsLayout_MapsCorrectly(V1alpha1BrandingThemeSocialButtonsLayout input, SocialButtonsLayout expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.ToApi(input));
        }

        // ──────────────────────── ToApi LogoPosition ──────────────────────────────

        [TestMethod]
        [DataRow(V1alpha1BrandingThemeLogoPosition.Center, LogoPosition.Center)]
        [DataRow(V1alpha1BrandingThemeLogoPosition.Left, LogoPosition.Left)]
        [DataRow(V1alpha1BrandingThemeLogoPosition.Right, LogoPosition.Right)]
        [DataRow(V1alpha1BrandingThemeLogoPosition.None, LogoPosition.None)]
        public void ToApi_LogoPosition_MapsCorrectly(V1alpha1BrandingThemeLogoPosition input, LogoPosition expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.ToApi(input));
        }

        // ──────────────────────── ToApi HeaderTextAlignment ───────────────────────

        [TestMethod]
        [DataRow(V1alpha1BrandingThemeHeaderTextAlignment.Center, HeaderTextAlignment.Center)]
        [DataRow(V1alpha1BrandingThemeHeaderTextAlignment.Left, HeaderTextAlignment.Left)]
        [DataRow(V1alpha1BrandingThemeHeaderTextAlignment.Right, HeaderTextAlignment.Right)]
        public void ToApi_HeaderTextAlignment_MapsCorrectly(V1alpha1BrandingThemeHeaderTextAlignment input, HeaderTextAlignment expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.ToApi(input));
        }

        // ──────────────────────── ToApi PageLayout ────────────────────────────────

        [TestMethod]
        [DataRow(V1alpha1BrandingThemePageLayout.Right, PageLayout.Right)]
        [DataRow(V1alpha1BrandingThemePageLayout.Center, PageLayout.Center)]
        [DataRow(V1alpha1BrandingThemePageLayout.Left, PageLayout.Left)]
        public void ToApi_PageLayout_MapsCorrectly(V1alpha1BrandingThemePageLayout input, PageLayout expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.ToApi(input));
        }

        // ──────────────────────── ToApi ButtonsStyle ──────────────────────────────

        [TestMethod]
        [DataRow(V1alpha1BrandingThemeButtonsStyle.Pill, ButtonsStyle.Pill)]
        [DataRow(V1alpha1BrandingThemeButtonsStyle.Rounded, ButtonsStyle.Rounded)]
        [DataRow(V1alpha1BrandingThemeButtonsStyle.Sharp, ButtonsStyle.Sharp)]
        public void ToApi_ButtonsStyle_MapsCorrectly(V1alpha1BrandingThemeButtonsStyle input, ButtonsStyle expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.ToApi(input));
        }

        // ──────────────────────── FromApi CaptchaWidgetTheme ─────────────────────

        [TestMethod]
        [DataRow(CaptchaWidgetTheme.Light, V1alpha1BrandingThemeCaptchaWidgetTheme.Light)]
        [DataRow(CaptchaWidgetTheme.Dark, V1alpha1BrandingThemeCaptchaWidgetTheme.Dark)]
        [DataRow(CaptchaWidgetTheme.Auto, V1alpha1BrandingThemeCaptchaWidgetTheme.Auto)]
        public void FromApi_CaptchaWidgetTheme_MapsCorrectly(CaptchaWidgetTheme input, V1alpha1BrandingThemeCaptchaWidgetTheme expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── FromApi SocialButtonsLayout ────────────────────

        [TestMethod]
        [DataRow(SocialButtonsLayout.Top, V1alpha1BrandingThemeSocialButtonsLayout.Top)]
        [DataRow(SocialButtonsLayout.Bottom, V1alpha1BrandingThemeSocialButtonsLayout.Bottom)]
        public void FromApi_SocialButtonsLayout_MapsCorrectly(SocialButtonsLayout input, V1alpha1BrandingThemeSocialButtonsLayout expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── FromApi LogoPosition ───────────────────────────

        [TestMethod]
        [DataRow(LogoPosition.Center, V1alpha1BrandingThemeLogoPosition.Center)]
        [DataRow(LogoPosition.Left, V1alpha1BrandingThemeLogoPosition.Left)]
        [DataRow(LogoPosition.Right, V1alpha1BrandingThemeLogoPosition.Right)]
        [DataRow(LogoPosition.None, V1alpha1BrandingThemeLogoPosition.None)]
        public void FromApi_LogoPosition_MapsCorrectly(LogoPosition input, V1alpha1BrandingThemeLogoPosition expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── FromApi HeaderTextAlignment ────────────────────

        [TestMethod]
        [DataRow(HeaderTextAlignment.Center, V1alpha1BrandingThemeHeaderTextAlignment.Center)]
        [DataRow(HeaderTextAlignment.Left, V1alpha1BrandingThemeHeaderTextAlignment.Left)]
        [DataRow(HeaderTextAlignment.Right, V1alpha1BrandingThemeHeaderTextAlignment.Right)]
        public void FromApi_HeaderTextAlignment_MapsCorrectly(HeaderTextAlignment input, V1alpha1BrandingThemeHeaderTextAlignment expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── FromApi PageLayout ─────────────────────────────

        [TestMethod]
        [DataRow(PageLayout.Center, V1alpha1BrandingThemePageLayout.Center)]
        [DataRow(PageLayout.Left, V1alpha1BrandingThemePageLayout.Left)]
        [DataRow(PageLayout.Right, V1alpha1BrandingThemePageLayout.Right)]
        public void FromApi_PageLayout_MapsCorrectly(PageLayout input, V1alpha1BrandingThemePageLayout expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── FromApi LinksStyle ─────────────────────────────

        [TestMethod]
        [DataRow(LinksStyle.Normal, V1alpha1BrandingThemeLinksStyle.Normal)]
        [DataRow(LinksStyle.Underlined, V1alpha1BrandingThemeLinksStyle.Underlined)]
        public void FromApi_LinksStyle_MapsCorrectly(LinksStyle input, V1alpha1BrandingThemeLinksStyle expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── FromApi ButtonsStyle ───────────────────────────

        [TestMethod]
        [DataRow(ButtonsStyle.Pill, V1alpha1BrandingThemeButtonsStyle.Pill)]
        [DataRow(ButtonsStyle.Rounded, V1alpha1BrandingThemeButtonsStyle.Rounded)]
        [DataRow(ButtonsStyle.Sharp, V1alpha1BrandingThemeButtonsStyle.Sharp)]
        public void FromApi_ButtonsStyle_MapsCorrectly(ButtonsStyle input, V1alpha1BrandingThemeButtonsStyle expected)
        {
            Assert.AreEqual(expected, V1alpha1BrandingThemeController.FromApi(input));
        }

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        [DataRow(CaptchaWidgetTheme.Light)]
        [DataRow(CaptchaWidgetTheme.Dark)]
        [DataRow(CaptchaWidgetTheme.Auto)]
        public void CaptchaWidgetTheme_Roundtrip(CaptchaWidgetTheme input)
        {
            var op = V1alpha1BrandingThemeController.FromApi(input)!.Value;
            Assert.AreEqual(input, V1alpha1BrandingThemeController.ToApi(op));
        }

        [TestMethod]
        [DataRow(SocialButtonsLayout.Top)]
        [DataRow(SocialButtonsLayout.Bottom)]
        public void SocialButtonsLayout_Roundtrip(SocialButtonsLayout input)
        {
            var op = V1alpha1BrandingThemeController.FromApi(input)!.Value;
            Assert.AreEqual(input, V1alpha1BrandingThemeController.ToApi(op));
        }

        [TestMethod]
        [DataRow(LogoPosition.Center)]
        [DataRow(LogoPosition.Left)]
        [DataRow(LogoPosition.Right)]
        [DataRow(LogoPosition.None)]
        public void LogoPosition_Roundtrip(LogoPosition input)
        {
            var op = V1alpha1BrandingThemeController.FromApi(input)!.Value;
            Assert.AreEqual(input, V1alpha1BrandingThemeController.ToApi(op));
        }

        [TestMethod]
        [DataRow(HeaderTextAlignment.Center)]
        [DataRow(HeaderTextAlignment.Left)]
        [DataRow(HeaderTextAlignment.Right)]
        public void HeaderTextAlignment_Roundtrip(HeaderTextAlignment input)
        {
            var op = V1alpha1BrandingThemeController.FromApi(input)!.Value;
            Assert.AreEqual(input, V1alpha1BrandingThemeController.ToApi(op));
        }

        [TestMethod]
        [DataRow(PageLayout.Center)]
        [DataRow(PageLayout.Left)]
        [DataRow(PageLayout.Right)]
        public void PageLayout_Roundtrip(PageLayout input)
        {
            var op = V1alpha1BrandingThemeController.FromApi(input)!.Value;
            Assert.AreEqual(input, V1alpha1BrandingThemeController.ToApi(op));
        }

        [TestMethod]
        [DataRow(ButtonsStyle.Pill)]
        [DataRow(ButtonsStyle.Rounded)]
        [DataRow(ButtonsStyle.Sharp)]
        public void ButtonsStyle_Roundtrip(ButtonsStyle input)
        {
            var op = V1alpha1BrandingThemeController.FromApi(input);
            Assert.AreEqual(input, V1alpha1BrandingThemeController.ToApi(op));
        }

    }

}
