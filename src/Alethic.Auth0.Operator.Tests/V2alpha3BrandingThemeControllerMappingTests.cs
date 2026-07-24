using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V2alpha3BrandingThemeControllerMappingTests
    {

        // ──────────────────────── ToApi CaptchaWidgetTheme ────────────────────────

        [TestMethod]
        public void ToApi_CaptchaWidgetTheme_Light() => Assert.AreEqual(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeCaptchaWidgetTheme.Light).Value);

        [TestMethod]
        public void ToApi_CaptchaWidgetTheme_Dark() => Assert.AreEqual(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeCaptchaWidgetTheme.Dark).Value);

        [TestMethod]
        public void ToApi_CaptchaWidgetTheme_Auto() => Assert.AreEqual(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeCaptchaWidgetTheme.Auto).Value);

        // ──────────────────────── ToApi SocialButtonsLayout ───────────────────────

        [TestMethod]
        public void ToApi_SocialButtonsLayout_Top() => Assert.AreEqual(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeSocialButtonsLayout.Top).Value);

        [TestMethod]
        public void ToApi_SocialButtonsLayout_Bottom() => Assert.AreEqual(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeSocialButtonsLayout.Bottom).Value);

        // ──────────────────────── ToApi LogoPosition ──────────────────────────────

        [TestMethod]
        public void ToApi_LogoPosition_Center() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.Center, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeLogoPosition.Center).Value);

        [TestMethod]
        public void ToApi_LogoPosition_Left() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.Left, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeLogoPosition.Left).Value);

        [TestMethod]
        public void ToApi_LogoPosition_Right() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.Right, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeLogoPosition.Right).Value);

        [TestMethod]
        public void ToApi_LogoPosition_None() => Assert.AreEqual(BrandingThemeWidgetLogoPositionEnum.Values.None, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeLogoPosition.None).Value);

        // ──────────────────────── ToApi HeaderTextAlignment ───────────────────────

        [TestMethod]
        public void ToApi_HeaderTextAlignment_Center() => Assert.AreEqual(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeHeaderTextAlignment.Center).Value);

        [TestMethod]
        public void ToApi_HeaderTextAlignment_Left() => Assert.AreEqual(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeHeaderTextAlignment.Left).Value);

        [TestMethod]
        public void ToApi_HeaderTextAlignment_Right() => Assert.AreEqual(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeHeaderTextAlignment.Right).Value);

        // ──────────────────────── ToApi PageLayout ────────────────────────────────

        [TestMethod]
        public void ToApi_PageLayout_Center() => Assert.AreEqual(BrandingThemePageBackgroundPageLayoutEnum.Values.Center, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemePageLayout.Center).Value);

        [TestMethod]
        public void ToApi_PageLayout_Left() => Assert.AreEqual(BrandingThemePageBackgroundPageLayoutEnum.Values.Left, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemePageLayout.Left).Value);

        [TestMethod]
        public void ToApi_PageLayout_Right() => Assert.AreEqual(BrandingThemePageBackgroundPageLayoutEnum.Values.Right, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemePageLayout.Right).Value);

        // ──────────────────────── ToApi ButtonsStyle ──────────────────────────────

        [TestMethod]
        public void ToApiButtonsStyle_Pill() => Assert.AreEqual(BrandingThemeBordersButtonsStyleEnum.Values.Pill, V2alpha3BrandingThemeController.ToApiButtonsStyle(V2alpha3BrandingThemeButtonsStyle.Pill).Value);

        [TestMethod]
        public void ToApiButtonsStyle_Rounded() => Assert.AreEqual(BrandingThemeBordersButtonsStyleEnum.Values.Rounded, V2alpha3BrandingThemeController.ToApiButtonsStyle(V2alpha3BrandingThemeButtonsStyle.Rounded).Value);

        [TestMethod]
        public void ToApiButtonsStyle_Sharp() => Assert.AreEqual(BrandingThemeBordersButtonsStyleEnum.Values.Sharp, V2alpha3BrandingThemeController.ToApiButtonsStyle(V2alpha3BrandingThemeButtonsStyle.Sharp).Value);

        // ──────────────────────── FromApi CaptchaWidgetTheme ─────────────────────

        [TestMethod]
        public void FromApi_CaptchaWidgetTheme_Light() => Assert.AreEqual(V2alpha3BrandingThemeCaptchaWidgetTheme.Light, V2alpha3BrandingThemeController.FromApi(new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light)));

        [TestMethod]
        public void FromApi_CaptchaWidgetTheme_Dark() => Assert.AreEqual(V2alpha3BrandingThemeCaptchaWidgetTheme.Dark, V2alpha3BrandingThemeController.FromApi(new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark)));

        [TestMethod]
        public void FromApi_CaptchaWidgetTheme_Auto() => Assert.AreEqual(V2alpha3BrandingThemeCaptchaWidgetTheme.Auto, V2alpha3BrandingThemeController.FromApi(new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto)));

        // ──────────────────────── FromApi SocialButtonsLayout ────────────────────

        [TestMethod]
        public void FromApi_SocialButtonsLayout_Top() => Assert.AreEqual(V2alpha3BrandingThemeSocialButtonsLayout.Top, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top)));

        [TestMethod]
        public void FromApi_SocialButtonsLayout_Bottom() => Assert.AreEqual(V2alpha3BrandingThemeSocialButtonsLayout.Bottom, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom)));

        // ──────────────────────── FromApi LogoPosition ───────────────────────────

        [TestMethod]
        public void FromApi_LogoPosition_Center() => Assert.AreEqual(V2alpha3BrandingThemeLogoPosition.Center, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Center)));

        [TestMethod]
        public void FromApi_LogoPosition_Left() => Assert.AreEqual(V2alpha3BrandingThemeLogoPosition.Left, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Left)));

        [TestMethod]
        public void FromApi_LogoPosition_Right() => Assert.AreEqual(V2alpha3BrandingThemeLogoPosition.Right, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Right)));

        [TestMethod]
        public void FromApi_LogoPosition_None() => Assert.AreEqual(V2alpha3BrandingThemeLogoPosition.None, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.None)));

        // ──────────────────────── FromApi HeaderTextAlignment ────────────────────

        [TestMethod]
        public void FromApi_HeaderTextAlignment_Center() => Assert.AreEqual(V2alpha3BrandingThemeHeaderTextAlignment.Center, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center)));

        [TestMethod]
        public void FromApi_HeaderTextAlignment_Left() => Assert.AreEqual(V2alpha3BrandingThemeHeaderTextAlignment.Left, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left)));

        [TestMethod]
        public void FromApi_HeaderTextAlignment_Right() => Assert.AreEqual(V2alpha3BrandingThemeHeaderTextAlignment.Right, V2alpha3BrandingThemeController.FromApi(new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right)));

        // ──────────────────────── FromApi PageLayout ─────────────────────────────

        [TestMethod]
        public void FromApi_PageLayout_Center() => Assert.AreEqual(V2alpha3BrandingThemePageLayout.Center, V2alpha3BrandingThemeController.FromApi(new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Center)));

        [TestMethod]
        public void FromApi_PageLayout_Left() => Assert.AreEqual(V2alpha3BrandingThemePageLayout.Left, V2alpha3BrandingThemeController.FromApi(new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Left)));

        [TestMethod]
        public void FromApi_PageLayout_Right() => Assert.AreEqual(V2alpha3BrandingThemePageLayout.Right, V2alpha3BrandingThemeController.FromApi(new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Right)));

        // ──────────────────────── FromApi LinksStyle ─────────────────────────────

        [TestMethod]
        public void FromApi_LinksStyle_Normal() => Assert.AreEqual(V2alpha3BrandingThemeLinksStyle.Normal, V2alpha3BrandingThemeController.FromApi(new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Normal)));

        [TestMethod]
        public void FromApi_LinksStyle_Underlined() => Assert.AreEqual(V2alpha3BrandingThemeLinksStyle.Underlined, V2alpha3BrandingThemeController.FromApi(new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Underlined)));

        // ──────────────────────── FromApi ButtonsStyle ───────────────────────────

        [TestMethod]
        public void FromApi_ButtonsStyle_Pill() => Assert.AreEqual(V2alpha3BrandingThemeButtonsStyle.Pill, V2alpha3BrandingThemeController.FromApi(new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Pill)));

        [TestMethod]
        public void FromApi_ButtonsStyle_Rounded() => Assert.AreEqual(V2alpha3BrandingThemeButtonsStyle.Rounded, V2alpha3BrandingThemeController.FromApi(new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Rounded)));

        [TestMethod]
        public void FromApi_ButtonsStyle_Sharp() => Assert.AreEqual(V2alpha3BrandingThemeButtonsStyle.Sharp, V2alpha3BrandingThemeController.FromApi(new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Sharp)));

        // ──────────────────────── Roundtrip tests ─────────────────────────────────

        [TestMethod]
        public void CaptchaWidgetTheme_Roundtrip_Light() { var input = new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void CaptchaWidgetTheme_Roundtrip_Dark() { var input = new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void CaptchaWidgetTheme_Roundtrip_Auto() { var input = new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void SocialButtonsLayout_Roundtrip_Top() { var input = new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void SocialButtonsLayout_Roundtrip_Bottom() { var input = new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_Center() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Center); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_Left() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Left); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_Right() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Right); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void LogoPosition_Roundtrip_None() { var input = new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.None); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void HeaderTextAlignment_Roundtrip_Center() { var input = new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void HeaderTextAlignment_Roundtrip_Left() { var input = new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void HeaderTextAlignment_Roundtrip_Right() { var input = new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void PageLayout_Roundtrip_Center() { var input = new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Center); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void PageLayout_Roundtrip_Left() { var input = new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Left); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void PageLayout_Roundtrip_Right() { var input = new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Right); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApi(V2alpha3BrandingThemeController.FromApi(input)!.Value).Value); }

        [TestMethod]
        public void ButtonsStyle_Roundtrip_Pill() { var input = new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Pill); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApiButtonsStyle(V2alpha3BrandingThemeController.FromApi(input)).Value); }

        [TestMethod]
        public void ButtonsStyle_Roundtrip_Rounded() { var input = new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Rounded); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApiButtonsStyle(V2alpha3BrandingThemeController.FromApi(input)).Value); }

        [TestMethod]
        public void ButtonsStyle_Roundtrip_Sharp() { var input = new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Sharp); Assert.AreEqual(input.Value, V2alpha3BrandingThemeController.ToApiButtonsStyle(V2alpha3BrandingThemeController.FromApi(input)).Value); }

        // DisplayName defaulting: Auth0 omits displayName on read-back when a theme has none, and the SDK
        // response models mark it required, so the create/update requests must always send a non-null value.

        [TestMethod]
        public void ToCreateRequest_DisplayName_DefaultsWhenNull() => Assert.AreEqual(V2alpha3BrandingThemeController.DefaultDisplayName, V2alpha3BrandingThemeController.ToCreateRequest(new V2alpha3BrandingThemeConf { DisplayName = null }).DisplayName);

        [TestMethod]
        public void ToCreateRequest_DisplayName_PreservedWhenSet() => Assert.AreEqual("Custom Theme", V2alpha3BrandingThemeController.ToCreateRequest(new V2alpha3BrandingThemeConf { DisplayName = "Custom Theme" }).DisplayName);

        [TestMethod]
        public void ToUpdateRequest_DisplayName_UsesExistingWhenConfNull() => Assert.AreEqual("Existing Theme", V2alpha3BrandingThemeController.ToUpdateRequest(new V2alpha3BrandingThemeConf { DisplayName = null }, new GetBrandingThemeResponseContent { Borders = null!, Colors = null!, DisplayName = "Existing Theme", Fonts = null!, PageBackground = null!, ThemeId = null!, Widget = null! }).DisplayName);

        [TestMethod]
        public void ToUpdateRequest_DisplayName_DefaultsWhenConfAndExistingNull() => Assert.AreEqual(V2alpha3BrandingThemeController.DefaultDisplayName, V2alpha3BrandingThemeController.ToUpdateRequest(new V2alpha3BrandingThemeConf { DisplayName = null }, new GetBrandingThemeResponseContent { Borders = null!, Colors = null!, DisplayName = null!, Fonts = null!, PageBackground = null!, ThemeId = null!, Widget = null! }).DisplayName);

    }

}
