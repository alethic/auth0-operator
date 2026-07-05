using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V1alpha1 and V2alpha3 BrandingTheme models.</summary>
    internal static class BrandingThemeCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3BrandingThemeBorders? Convert(V1alpha1BrandingThemeBorders? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeBorders
            {
                ButtonBorderRadius = s.ButtonBorderRadius,
                ButtonBorderWeight = s.ButtonBorderWeight,
                ButtonsStyle = MapEnumN<V1alpha1BrandingThemeButtonsStyle, V2alpha3BrandingThemeButtonsStyle>(s.ButtonsStyle),
                InputBorderRadius = s.InputBorderRadius,
                InputBorderWeight = s.InputBorderWeight,
                InputsStyle = MapEnumN<V1alpha1BrandingThemeButtonsStyle, V2alpha3BrandingThemeButtonsStyle>(s.InputsStyle),
                ShowWidgetShadow = s.ShowWidgetShadow,
                WidgetBorderWeight = s.WidgetBorderWeight,
                WidgetCornerRadius = s.WidgetCornerRadius,
            };
        }

        public static V1alpha1BrandingThemeBorders? Revert(V2alpha3BrandingThemeBorders? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeBorders
            {
                ButtonBorderRadius = s.ButtonBorderRadius,
                ButtonBorderWeight = s.ButtonBorderWeight,
                ButtonsStyle = MapEnumN<V2alpha3BrandingThemeButtonsStyle, V1alpha1BrandingThemeButtonsStyle>(s.ButtonsStyle),
                InputBorderRadius = s.InputBorderRadius,
                InputBorderWeight = s.InputBorderWeight,
                InputsStyle = MapEnumN<V2alpha3BrandingThemeButtonsStyle, V1alpha1BrandingThemeButtonsStyle>(s.InputsStyle),
                ShowWidgetShadow = s.ShowWidgetShadow,
                WidgetBorderWeight = s.WidgetBorderWeight,
                WidgetCornerRadius = s.WidgetCornerRadius,
            };
        }

        public static V2alpha3BrandingThemeColors? Convert(V1alpha1BrandingThemeColors? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeColors
            {
                BaseFocusColor = s.BaseFocusColor,
                BaseHoverColor = s.BaseHoverColor,
                BodyText = s.BodyText,
                CaptchaWidgetTheme = MapEnumN<V1alpha1BrandingThemeCaptchaWidgetTheme, V2alpha3BrandingThemeCaptchaWidgetTheme>(s.CaptchaWidgetTheme),
                Error = s.Error,
                Header = s.Header,
                Icons = s.Icons,
                InputBackground = s.InputBackground,
                InputBorder = s.InputBorder,
                InputFilledText = s.InputFilledText,
                InputLabelsPlaceholders = s.InputLabelsPlaceholders,
                LinksFocusedComponents = s.LinksFocusedComponents,
                PrimaryButton = s.PrimaryButton,
                PrimaryButtonLabel = s.PrimaryButtonLabel,
                SecondaryButtonBorder = s.SecondaryButtonBorder,
                SecondaryButtonLabel = s.SecondaryButtonLabel,
                Success = s.Success,
                WidgetBackground = s.WidgetBackground,
                WidgetBorder = s.WidgetBorder,
            };
        }

        public static V1alpha1BrandingThemeColors? Revert(V2alpha3BrandingThemeColors? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeColors
            {
                BaseFocusColor = s.BaseFocusColor,
                BaseHoverColor = s.BaseHoverColor,
                BodyText = s.BodyText,
                CaptchaWidgetTheme = MapEnumN<V2alpha3BrandingThemeCaptchaWidgetTheme, V1alpha1BrandingThemeCaptchaWidgetTheme>(s.CaptchaWidgetTheme),
                Error = s.Error,
                Header = s.Header,
                Icons = s.Icons,
                InputBackground = s.InputBackground,
                InputBorder = s.InputBorder,
                InputFilledText = s.InputFilledText,
                InputLabelsPlaceholders = s.InputLabelsPlaceholders,
                LinksFocusedComponents = s.LinksFocusedComponents,
                PrimaryButton = s.PrimaryButton,
                PrimaryButtonLabel = s.PrimaryButtonLabel,
                SecondaryButtonBorder = s.SecondaryButtonBorder,
                SecondaryButtonLabel = s.SecondaryButtonLabel,
                Success = s.Success,
                WidgetBackground = s.WidgetBackground,
                WidgetBorder = s.WidgetBorder,
            };
        }

        public static V2alpha3BrandingThemeConf? Convert(V1alpha1BrandingThemeConf? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeConf
            {
                DisplayName = s.DisplayName,
                Borders = Convert(s.Borders),
                Colors = Convert(s.Colors),
                Fonts = Convert(s.Fonts),
                PageBackground = Convert(s.PageBackground),
                Widget = Convert(s.Widget),
            };
        }

        public static V1alpha1BrandingThemeConf? Revert(V2alpha3BrandingThemeConf? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeConf
            {
                DisplayName = s.DisplayName,
                Borders = Revert(s.Borders),
                Colors = Revert(s.Colors),
                Fonts = Revert(s.Fonts),
                PageBackground = Revert(s.PageBackground),
                Widget = Revert(s.Widget),
            };
        }

        public static V2alpha3BrandingThemeFind? Convert(V1alpha1BrandingThemeFind? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeFind
            {
                Id = s.Id,
            };
        }

        public static V1alpha1BrandingThemeFind? Revert(V2alpha3BrandingThemeFind? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeFind
            {
                Id = s.Id,
            };
        }

        public static V2alpha3BrandingThemeFont? Convert(V1alpha1BrandingThemeFont? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeFont
            {
                Bold = s.Bold,
                Size = s.Size,
            };
        }

        public static V1alpha1BrandingThemeFont? Revert(V2alpha3BrandingThemeFont? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeFont
            {
                Bold = s.Bold,
                Size = s.Size,
            };
        }

        public static V2alpha3BrandingThemeFonts? Convert(V1alpha1BrandingThemeFonts? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeFonts
            {
                BodyText = Convert(s.BodyText),
                ButtonsText = Convert(s.ButtonsText),
                FontUrl = s.FontUrl,
                InputLabels = Convert(s.InputLabels),
                Links = Convert(s.Links),
                LinksStyle = MapEnumN<V1alpha1BrandingThemeLinksStyle, V2alpha3BrandingThemeLinksStyle>(s.LinksStyle),
                ReferenceTextSize = s.ReferenceTextSize,
                Subtitle = Convert(s.Subtitle),
                Title = Convert(s.Title),
            };
        }

        public static V1alpha1BrandingThemeFonts? Revert(V2alpha3BrandingThemeFonts? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeFonts
            {
                BodyText = Revert(s.BodyText),
                ButtonsText = Revert(s.ButtonsText),
                FontUrl = s.FontUrl,
                InputLabels = Revert(s.InputLabels),
                Links = Revert(s.Links),
                LinksStyle = MapEnumN<V2alpha3BrandingThemeLinksStyle, V1alpha1BrandingThemeLinksStyle>(s.LinksStyle),
                ReferenceTextSize = s.ReferenceTextSize,
                Subtitle = Revert(s.Subtitle),
                Title = Revert(s.Title),
            };
        }

        public static V2alpha3BrandingThemePageBackground? Convert(V1alpha1BrandingThemePageBackground? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemePageBackground
            {
                BackgroundColor = s.BackgroundColor,
                BackgroundImageUrl = s.BackgroundImageUrl,
                PageLayout = MapEnumN<V1alpha1BrandingThemePageLayout, V2alpha3BrandingThemePageLayout>(s.PageLayout),
            };
        }

        public static V1alpha1BrandingThemePageBackground? Revert(V2alpha3BrandingThemePageBackground? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemePageBackground
            {
                BackgroundColor = s.BackgroundColor,
                BackgroundImageUrl = s.BackgroundImageUrl,
                PageLayout = MapEnumN<V2alpha3BrandingThemePageLayout, V1alpha1BrandingThemePageLayout>(s.PageLayout),
            };
        }

        public static V2alpha3BrandingThemeWidget? Convert(V1alpha1BrandingThemeWidget? s)
        {
            if (s is null) return null;
            return new V2alpha3BrandingThemeWidget
            {
                HeaderTextAlignment = MapEnumN<V1alpha1BrandingThemeHeaderTextAlignment, V2alpha3BrandingThemeHeaderTextAlignment>(s.HeaderTextAlignment),
                LogoHeight = s.LogoHeight,
                LogoPosition = MapEnumN<V1alpha1BrandingThemeLogoPosition, V2alpha3BrandingThemeLogoPosition>(s.LogoPosition),
                LogoUrl = s.LogoUrl,
                SocialButtonsLayout = MapEnumN<V1alpha1BrandingThemeSocialButtonsLayout, V2alpha3BrandingThemeSocialButtonsLayout>(s.SocialButtonsLayout),
            };
        }

        public static V1alpha1BrandingThemeWidget? Revert(V2alpha3BrandingThemeWidget? s)
        {
            if (s is null) return null;
            return new V1alpha1BrandingThemeWidget
            {
                HeaderTextAlignment = MapEnumN<V2alpha3BrandingThemeHeaderTextAlignment, V1alpha1BrandingThemeHeaderTextAlignment>(s.HeaderTextAlignment),
                LogoHeight = s.LogoHeight,
                LogoPosition = MapEnumN<V2alpha3BrandingThemeLogoPosition, V1alpha1BrandingThemeLogoPosition>(s.LogoPosition),
                LogoUrl = s.LogoUrl,
                SocialButtonsLayout = MapEnumN<V2alpha3BrandingThemeSocialButtonsLayout, V1alpha1BrandingThemeSocialButtonsLayout>(s.SocialButtonsLayout),
            };
        }

    }

}
