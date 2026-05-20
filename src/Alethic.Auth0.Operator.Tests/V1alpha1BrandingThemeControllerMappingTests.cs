using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1alpha1BrandingThemeControllerMappingTests
    {

        // ──────────────────────── ToApi CaptchaWidgetTheme ────────────────────────

        [TestMethod]
        public void ToApi_CaptchaWidgetTheme_Light() => Assert.AreEqual(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeCaptchaWidgetTheme.Light).Value);

        [TestMethod]
        public void ToApi_CaptchaWidgetTheme_Dark() => Assert.AreEqual(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeCaptchaWidgetTheme.Dark).Value);

        [TestMethod]
        public void ToApi_CaptchaWidgetTheme_Auto() => Assert.AreEqual(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeCaptchaWidgetTheme.Auto).Value);

        // ──────────────────────── ToApi SocialButtonsLayout ───────────────────────

        [TestMethod]
        public void ToApi_SocialButtonsLayout_Top() => Assert.AreEqual(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeSocialButtonsLayout.Top).Value);

        [TestMethod]
        public void ToApi_SocialButtonsLayout_Bottom() => Assert.AreEqual(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeSocialButtonsLayout.Bottom).Value);

        // ──────────────────────── ToApi LogoPosition ──────────────────────────────

        [TestMethod]
        public void ToApi_LogoPosition_Center() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.Center, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeLogoPosition.Center).Value);

        [TestMethod]
        public void ToApi_LogoPosition_Left() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.Left, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeLogoPosition.Left).Value);

        [TestMethod]
        public void ToApi_LogoPosition_Right() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.Right, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeLogoPosition.Right).Value);

        [TestMethod]
        public void ToApi_LogoPosition_None() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.None, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeLogoPosition.None).Value);

        // ──────────────────────── ToApi HeaderTextAlignment ───────────────────────

        [TestMethod]
        public void ToApi_HeaderTextAlignment_Center() => Assert.AreEqual(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeHeaderTextAlignment.Center).Value);

        [TestMethod]
        public void ToApi_HeaderTextAlignment_Left() => Assert.AreEqual(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeHeaderTextAlignment.Left).Value);

        [TestMethod]
        public void ToApi_HeaderTextAlignment_Right() => Assert.AreEqual(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeHeaderTextAlignment.Right).Value);

        // ──────────────────────── ToApi PageLayout ────────────────────────────────

        [TestMethod]
        public void ToApi_PageLayout_Center() => Assert.AreEqual(BrandingThemePageBackgroundPageLayoutEnum.Values.Center, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemePageLayout.Center).Value);

        [TestMethod]
        public void ToApi_PageLayout_Left() => Assert.AreEqual(BrandingThemePageBackgroundPageLayoutEnum.Values.Left, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemePageLayout.Left).Value);

        [TestMethod]
        public void ToApi_PageLayout_Right() => Assert.AreEqual(BrandingThemePageBackgroundPageLayoutEnum.Values.Right, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemePageLayout.Right).Value);

        // ──────────────────────── ToApi ButtonsStyle ──────────────────────────────

        [TestMethod]
        public void ToApiButtonsStyle_Pill() => Assert.AreEqual(BrandingThemeBordersButtonsStyleEnum.Values.Pill, V1alpha1BrandingThemeController.ToApiButtonsStyle(V1alpha1BrandingThemeButtonsStyle.Pill).Value);

        [TestMethod]
        public void ToApiButtonsStyle_Rounded() => Assert.AreEqual(BrandingThemeBordersButtonsStyleEnum.Values.Rounded, V1alpha1BrandingThemeController.ToApiButtonsStyle(V1alpha1BrandingThemeButtonsStyle.Rounded).Value);

        [TestMethod]
        public void ToApiButtonsStyle_Sharp() => Assert.AreEqual(BrandingThemeBordersButtonsStyleEnum.Values.Sharp, V1alpha1BrandingThemeController.ToApiButtonsStyle(V1alpha1BrandingThemeButtonsStyle.Sharp).Value);

        // ──────────────────────── FromApi CaptchaWidgetTheme ─────────────────────

        [TestMethod]
        public void FromApi_CaptchaWidgetTheme_Light() => Assert.AreEqual(V1alpha1BrandingThemeCaptchaWidgetTheme.Light, V1alpha1BrandingThemeController.FromApi(new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light)));

        [TestMethod]
        public void FromApi_CaptchaWidgetTheme_Dark() => Assert.AreEqual(V1alpha1BrandingThemeCaptchaWidgetTheme.Dark, V1alpha1BrandingThemeController.FromApi(new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark)));

        [TestMethod]
        public void FromApi_CaptchaWidgetTheme_Auto() => Assert.AreEqual(V1alpha1BrandingThemeCaptchaWidgetTheme.Auto, V1alpha1BrandingThemeController.FromApi(new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto)));

        // ──────────────────────── FromApi SocialButtonsLayout ────────────────────

        [TestMethod]
        public void FromApi_SocialButtonsLayout_Top() => Assert.AreEqual(V1alpha1BrandingThemeSocialButtonsLayout.Top, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top)));

        [TestMethod]
        public void FromApi_SocialButtonsLayout_Bottom() => Assert.AreEqual(V1alpha1BrandingThemeSocialButtonsLayout.Bottom, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom)));

        // ──────────────────────── FromApi LogoPosition ───────────────────────────

        [TestMethod]
        public void FromApi_LogoPosition_Center() => Assert.AreEqual(V1alpha1BrandingThemeLogoPosition.Center, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Center)));

        [TestMethod]
        public void FromApi_LogoPosition_Left() => Assert.AreEqual(V1alpha1BrandingThemeLogoPosition.Left, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Left)));

        [TestMethod]
        public void FromApi_LogoPosition_Right() => Assert.AreEqual(V1alpha1BrandingThemeLogoPosition.Right, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Right)));

        [TestMethod]
        public void FromApi_LogoPosition_None() => Assert.AreEqual(V1alpha1BrandingThemeLogoPosition.None, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.None)));

        // ──────────────────────── FromApi HeaderTextAlignment ────────────────────

        [TestMethod]
        public void FromApi_HeaderTextAlignment_Center() => Assert.AreEqual(V1alpha1BrandingThemeHeaderTextAlignment.Center, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center)));

        [TestMethod]
        public void FromApi_HeaderTextAlignment_Left() => Assert.AreEqual(V1alpha1BrandingThemeHeaderTextAlignment.Left, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left)));

        [TestMethod]
        public void FromApi_HeaderTextAlignment_Right() => Assert.AreEqual(V1alpha1BrandingThemeHeaderTextAlignment.Right, V1alpha1BrandingThemeController.FromApi(new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right)));

        // ──────────────────────── FromApi PageLayout ─────────────────────────────

        [TestMethod]
        public void FromApi_PageLayout_Center() => Assert.AreEqual(V1alpha1BrandingThemePageLayout.Center, V1alpha1BrandingThemeController.FromApi(new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Center)));

        [TestMethod]
        public void FromApi_PageLayout_Left() => Assert.AreEqual(V1alpha1BrandingThemePageLayout.Left, V1alpha1BrandingThemeController.FromApi(new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Left)));

        [TestMethod]
        public void FromApi_PageLayout_Right() => Assert.AreEqual(V1alpha1BrandingThemePageLayout.Right, V1alpha1BrandingThemeController.FromApi(new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Right)));

        // ──────────────────────── FromApi LinksStyle ─────────────────────────────

        [TestMethod]
        public void FromApi_LinksStyle_Normal() => Assert.AreEqual(V1alpha1BrandingThemeLinksStyle.Normal, V1alpha1BrandingThemeController.FromApi(new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Normal)));

        [TestMethod]
        public void FromApi_LinksStyle_Underlined() => Assert.AreEqual(V1alpha1BrandingThemeLinksStyle.Underlined, V1alpha1BrandingThemeController.FromApi(new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Underlined)));

        // ──────────────────────── FromApi ButtonsStyle ───────────────────────────

        [TestMethod]
        public void FromApi_ButtonsStyle_Pill() => Assert.AreEqual(V1alpha1BrandingThemeButtonsStyle.Pill, V1alpha1BrandingThemeController.FromApi(new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Pill)));

        [TestMethod]
        public void FromApi_ButtonsStyle_Rounded() => Assert.AreEqual(V1alpha1BrandingThemeButtonsStyle.Rounded, V1alpha1BrandingThemeController.FromApi(new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Rounded)));

        [TestMethod]
        public void FromApi_ButtonsStyle_Sharp() => Assert.AreEqual(V1alpha1BrandingThemeButtonsStyle.Sharp, V1alpha1BrandingThemeController.FromApi(new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Sharp)));

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        public void CaptchaWidgetTheme_Roundtrip_Light() { var input = new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void CaptchaWidgetTheme_Roundtrip_Dark() { var input = new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void CaptchaWidgetTheme_Roundtrip_Auto() { var input = new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void SocialButtonsLayout_Roundtrip_Top() { var input = new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void SocialButtonsLayout_Roundtrip_Bottom() { var input = new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_Center() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Center); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_Left() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Left); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_Right() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Right); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_None() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.None); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void HeaderTextAlignment_Roundtrip_Center() { var input = new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void HeaderTextAlignment_Roundtrip_Left() { var input = new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void HeaderTextAlignment_Roundtrip_Right() { var input = new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void PageLayout_Roundtrip_Center() { var input = new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Center); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void PageLayout_Roundtrip_Left() { var input = new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Left); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void PageLayout_Roundtrip_Right() { var input = new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Right); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApi(V1alpha1BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void ButtonsStyle_Roundtrip_Pill() { var input = new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Pill); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApiButtonsStyle(V1alpha1BrandingThemeController.FromApi(input)).Value); }

        [TestMethod]
        public void ButtonsStyle_Roundtrip_Rounded() { var input = new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Rounded); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApiButtonsStyle(V1alpha1BrandingThemeController.FromApi(input)).Value); }

        [TestMethod]
        public void ButtonsStyle_Roundtrip_Sharp() { var input = new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Sharp); Assert.AreEqual(input.Value, V1alpha1BrandingThemeController.ToApiButtonsStyle(V1alpha1BrandingThemeController.FromApi(input)).Value); }

    }

}
