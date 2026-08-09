using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Tests for the explicit drift comparer on <see cref="V2alpha3BrandingThemeController"/> that decides whether
    /// a reconcile cycle pushes a theme update to Auth0. The theme update is a whole-object replace that back-fills
    /// unspecified leaves from the live theme, so a sparse conf that matches the live theme must not update.
    /// </summary>
    [TestClass]
    public class V2alpha3BrandingThemeNeedsUpdateTests
    {

        /// <summary>
        /// Builds a fully populated conf resembling what <c>FromApi</c> returns for a live theme with defaults.
        /// </summary>
        static V2alpha3BrandingThemeConf MakeFullConf() => new()
        {
            DisplayName = "Default Theme",
            Borders = new V2alpha3BrandingThemeBorders
            {
                ButtonBorderRadius = 3f,
                ButtonBorderWeight = 1f,
                ButtonsStyle = V2alpha3BrandingThemeButtonsStyle.Rounded,
                InputBorderRadius = 3f,
                InputBorderWeight = 1f,
                InputsStyle = V2alpha3BrandingThemeButtonsStyle.Rounded,
                ShowWidgetShadow = true,
                WidgetBorderWeight = 0f,
                WidgetCornerRadius = 5f,
            },
            Colors = new V2alpha3BrandingThemeColors
            {
                BaseFocusColor = "#635dff",
                BaseHoverColor = "#000000",
                BodyText = "#1e212a",
                CaptchaWidgetTheme = V2alpha3BrandingThemeCaptchaWidgetTheme.Auto,
                Error = "#d03c38",
                Header = "#1e212a",
                Icons = "#65676e",
                InputBackground = "#ffffff",
                InputBorder = "#c9cace",
                InputFilledText = "#000000",
                InputLabelsPlaceholders = "#65676e",
                LinksFocusedComponents = "#635dff",
                PrimaryButton = "#635dff",
                PrimaryButtonLabel = "#ffffff",
                SecondaryButtonBorder = "#c9cace",
                SecondaryButtonLabel = "#1e212a",
                Success = "#13a688",
                WidgetBackground = "#ffffff",
                WidgetBorder = "#c9cace",
            },
            Fonts = new V2alpha3BrandingThemeFonts
            {
                BodyText = new V2alpha3BrandingThemeFont { Bold = false, Size = 87.5f },
                ButtonsText = new V2alpha3BrandingThemeFont { Bold = false, Size = 100f },
                FontUrl = "https://example.com/font.woff2",
                InputLabels = new V2alpha3BrandingThemeFont { Bold = false, Size = 100f },
                Links = new V2alpha3BrandingThemeFont { Bold = true, Size = 87.5f },
                LinksStyle = V2alpha3BrandingThemeLinksStyle.Normal,
                ReferenceTextSize = 16f,
                Subtitle = new V2alpha3BrandingThemeFont { Bold = false, Size = 87.5f },
                Title = new V2alpha3BrandingThemeFont { Bold = false, Size = 150f },
            },
            PageBackground = new V2alpha3BrandingThemePageBackground
            {
                BackgroundColor = "#000000",
                BackgroundImageUrl = "",
                PageLayout = V2alpha3BrandingThemePageLayout.Center,
            },
            Widget = new V2alpha3BrandingThemeWidget
            {
                HeaderTextAlignment = V2alpha3BrandingThemeHeaderTextAlignment.Center,
                LogoHeight = 52f,
                LogoPosition = V2alpha3BrandingThemeLogoPosition.Center,
                LogoUrl = "",
                SocialButtonsLayout = V2alpha3BrandingThemeSocialButtonsLayout.Bottom,
            },
        };

        [TestMethod]
        public void Identical_DoesNotRequireUpdate()
        {
            Assert.IsFalse(V2alpha3BrandingThemeController.ConfRequiresUpdate(MakeFullConf(), MakeFullConf()));
        }

        [TestMethod]
        public void SparseConfMatchingPopulatedLast_DoesNotRequireUpdate()
        {
            var conf = new V2alpha3BrandingThemeConf
            {
                Colors = new V2alpha3BrandingThemeColors
                {
                    PrimaryButton = "#635dff",
                },
                Widget = new V2alpha3BrandingThemeWidget
                {
                    LogoPosition = V2alpha3BrandingThemeLogoPosition.Center,
                },
            };

            Assert.IsFalse(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void ColorLeafChange_RequiresUpdate()
        {
            var conf = MakeFullConf();
            conf.Colors!.PrimaryButton = "#ff0000";

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void BorderLeafChange_RequiresUpdate()
        {
            var conf = MakeFullConf();
            conf.Borders!.ButtonBorderRadius = 8f;

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void BorderStyleChange_RequiresUpdate()
        {
            var conf = MakeFullConf();
            conf.Borders!.ButtonsStyle = V2alpha3BrandingThemeButtonsStyle.Pill;

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void FontLeafChange_RequiresUpdate()
        {
            var conf = MakeFullConf();
            conf.Fonts!.Title!.Size = 175f;

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void PageBackgroundLeafChange_RequiresUpdate()
        {
            var conf = MakeFullConf();
            conf.PageBackground!.PageLayout = V2alpha3BrandingThemePageLayout.Left;

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void DisplayNameChange_RequiresUpdate()
        {
            var conf = MakeFullConf();
            conf.DisplayName = "Renamed Theme";

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, MakeFullConf()));
        }

        [TestMethod]
        public void SparseConfWithMissingLastGroup_RequiresUpdate()
        {
            var conf = MakeFullConf();
            var last = MakeFullConf();
            last.Fonts = null;

            Assert.IsTrue(V2alpha3BrandingThemeController.ConfRequiresUpdate(conf, last));
        }

    }

}
