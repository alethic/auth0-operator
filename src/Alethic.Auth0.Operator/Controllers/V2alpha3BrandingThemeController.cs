using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Branding;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3BrandingTheme), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3BrandingThemeController :
        V1TenantEntityInstanceController<V2alpha3BrandingTheme, V2alpha3BrandingTheme.SpecDef, V2alpha3BrandingTheme.StatusDef, V2alpha3BrandingThemeConf, V2alpha3BrandingThemeConf>,
        IEntityController<V2alpha3BrandingTheme>
    {

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeColorsCaptchaWidgetThemeEnum ToApi(V2alpha3BrandingThemeCaptchaWidgetTheme source) => source switch
        {
            V2alpha3BrandingThemeCaptchaWidgetTheme.Light => new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light),
            V2alpha3BrandingThemeCaptchaWidgetTheme.Dark => new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark),
            V2alpha3BrandingThemeCaptchaWidgetTheme.Auto => new BrandingThemeColorsCaptchaWidgetThemeEnum(BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeFontLinksStyleEnum ToApiLinksStyle(V2alpha3BrandingThemeLinksStyle? source) => source switch
        {
            V2alpha3BrandingThemeLinksStyle.Normal => new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Normal),
            V2alpha3BrandingThemeLinksStyle.Underlined => new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Underlined),
            null => new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Normal),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeWidgetSocialButtonsLayoutEnum ToApi(V2alpha3BrandingThemeSocialButtonsLayout source) => source switch
        {
            V2alpha3BrandingThemeSocialButtonsLayout.Top => new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top),
            V2alpha3BrandingThemeSocialButtonsLayout.Bottom => new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeWidgetLogoPositionEnum ToApi(V2alpha3BrandingThemeLogoPosition source) => source switch
        {
            V2alpha3BrandingThemeLogoPosition.Center => new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Center),
            V2alpha3BrandingThemeLogoPosition.Left => new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Left),
            V2alpha3BrandingThemeLogoPosition.Right => new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Right),
            V2alpha3BrandingThemeLogoPosition.None => new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.None),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeWidgetHeaderTextAlignmentEnum ToApi(V2alpha3BrandingThemeHeaderTextAlignment source) => source switch
        {
            V2alpha3BrandingThemeHeaderTextAlignment.Center => new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center),
            V2alpha3BrandingThemeHeaderTextAlignment.Left => new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left),
            V2alpha3BrandingThemeHeaderTextAlignment.Right => new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemePageBackgroundPageLayoutEnum ToApi(V2alpha3BrandingThemePageLayout source) => source switch
        {
            V2alpha3BrandingThemePageLayout.Right => new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Right),
            V2alpha3BrandingThemePageLayout.Center => new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Center),
            V2alpha3BrandingThemePageLayout.Left => new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Left),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="buttonsStyle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeBordersButtonsStyleEnum ToApiButtonsStyle(V2alpha3BrandingThemeButtonsStyle buttonsStyle) => buttonsStyle switch
        {
            V2alpha3BrandingThemeButtonsStyle.Pill => new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Pill),
            V2alpha3BrandingThemeButtonsStyle.Rounded => new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Rounded),
            V2alpha3BrandingThemeButtonsStyle.Sharp => new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Sharp),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="inputsStyle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static BrandingThemeBordersInputsStyleEnum ToApiInputsStyle(V2alpha3BrandingThemeButtonsStyle inputsStyle) => inputsStyle switch
        {
            V2alpha3BrandingThemeButtonsStyle.Pill => new BrandingThemeBordersInputsStyleEnum(BrandingThemeBordersInputsStyleEnum.Values.Pill),
            V2alpha3BrandingThemeButtonsStyle.Rounded => new BrandingThemeBordersInputsStyleEnum(BrandingThemeBordersInputsStyleEnum.Values.Rounded),
            V2alpha3BrandingThemeButtonsStyle.Sharp => new BrandingThemeBordersInputsStyleEnum(BrandingThemeBordersInputsStyleEnum.Values.Sharp),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFontBodyText"/>.
        /// </summary>
        internal static BrandingThemeFontBodyText ToApi(V2alpha3BrandingThemeFont? source, BrandingThemeFontBodyText? existing = null) => new()
        {
            Bold = source?.Bold ?? existing?.Bold ?? false,
            Size = source?.Size ?? (existing?.Size is double es ? (float)es : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFontButtonsText"/>.
        /// </summary>
        internal static BrandingThemeFontButtonsText ToApi(V2alpha3BrandingThemeFont? source, BrandingThemeFontButtonsText? existing = null) => new()
        {
            Bold = source?.Bold ?? existing?.Bold ?? false,
            Size = source?.Size ?? (existing?.Size is double es ? (float)es : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFontInputLabels"/>.
        /// </summary>
        internal static BrandingThemeFontInputLabels ToApi(V2alpha3BrandingThemeFont? source, BrandingThemeFontInputLabels? existing = null) => new()
        {
            Bold = source?.Bold ?? existing?.Bold ?? false,
            Size = source?.Size ?? (existing?.Size is double es ? (float)es : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFontLinks"/>.
        /// </summary>
        internal static BrandingThemeFontLinks ToApi(V2alpha3BrandingThemeFont? source, BrandingThemeFontLinks? existing = null) => new()
        {
            Bold = source?.Bold ?? existing?.Bold ?? false,
            Size = source?.Size ?? (existing?.Size is double es ? (float)es : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFontSubtitle"/>.
        /// </summary>
        internal static BrandingThemeFontSubtitle ToApi(V2alpha3BrandingThemeFont? source, BrandingThemeFontSubtitle? existing = null) => new()
        {
            Bold = source?.Bold ?? existing?.Bold ?? false,
            Size = source?.Size ?? (existing?.Size is double es ? (float)es : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFontTitle"/>.
        /// </summary>
        internal static BrandingThemeFontTitle ToApi(V2alpha3BrandingThemeFont? source, BrandingThemeFontTitle? existing = null) => new()
        {
            Bold = source?.Bold ?? existing?.Bold ?? false,
            Size = source?.Size ?? (existing?.Size is double es ? (float)es : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeBorders"/>.
        /// </summary>
        internal static BrandingThemeBorders ToApi(V2alpha3BrandingThemeBorders? source, BrandingThemeBorders? existing = null) => new()
        {
            ButtonBorderRadius = source?.ButtonBorderRadius ?? (existing?.ButtonBorderRadius is double ebr ? (float)ebr : 0f),
            ButtonBorderWeight = source?.ButtonBorderWeight ?? (existing?.ButtonBorderWeight is double ebw ? (float)ebw : 0f),
            ButtonsStyle = source?.ButtonsStyle is { } bs ? ToApiButtonsStyle(bs) : existing?.ButtonsStyle ?? new BrandingThemeBordersButtonsStyleEnum(BrandingThemeBordersButtonsStyleEnum.Values.Rounded),
            InputBorderRadius = source?.InputBorderRadius ?? (existing?.InputBorderRadius is double eibr ? (float)eibr : 0f),
            InputBorderWeight = source?.InputBorderWeight ?? (existing?.InputBorderWeight is double eibw ? (float)eibw : 0f),
            InputsStyle = source?.InputsStyle is { } iss ? ToApiInputsStyle(iss) : existing?.InputsStyle ?? new BrandingThemeBordersInputsStyleEnum(BrandingThemeBordersInputsStyleEnum.Values.Rounded),
            ShowWidgetShadow = source?.ShowWidgetShadow ?? existing?.ShowWidgetShadow ?? false,
            WidgetBorderWeight = source?.WidgetBorderWeight ?? (existing?.WidgetBorderWeight is double ewbw ? (float)ewbw : 0f),
            WidgetCornerRadius = source?.WidgetCornerRadius ?? (existing?.WidgetCornerRadius is double ewcr ? (float)ewcr : 0f),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeColors"/>.
        /// </summary>
        internal static BrandingThemeColors ToApi(V2alpha3BrandingThemeColors? source, BrandingThemeColors? existing = null) => new()
        {
            BaseFocusColor = source?.BaseFocusColor ?? existing?.BaseFocusColor ?? string.Empty,
            BaseHoverColor = source?.BaseHoverColor ?? existing?.BaseHoverColor ?? string.Empty,
            BodyText = source?.BodyText ?? existing?.BodyText ?? string.Empty,
            CaptchaWidgetTheme = source?.CaptchaWidgetTheme is { } cwt ? ToApi(cwt) : existing?.CaptchaWidgetTheme,
            Error = source?.Error ?? existing?.Error ?? string.Empty,
            Header = source?.Header ?? existing?.Header ?? string.Empty,
            Icons = source?.Icons ?? existing?.Icons ?? string.Empty,
            InputBackground = source?.InputBackground ?? existing?.InputBackground ?? string.Empty,
            InputBorder = source?.InputBorder ?? existing?.InputBorder ?? string.Empty,
            InputFilledText = source?.InputFilledText ?? existing?.InputFilledText ?? string.Empty,
            InputLabelsPlaceholders = source?.InputLabelsPlaceholders ?? existing?.InputLabelsPlaceholders ?? string.Empty,
            LinksFocusedComponents = source?.LinksFocusedComponents ?? existing?.LinksFocusedComponents ?? string.Empty,
            PrimaryButton = source?.PrimaryButton ?? existing?.PrimaryButton ?? string.Empty,
            PrimaryButtonLabel = source?.PrimaryButtonLabel ?? existing?.PrimaryButtonLabel ?? string.Empty,
            SecondaryButtonBorder = source?.SecondaryButtonBorder ?? existing?.SecondaryButtonBorder ?? string.Empty,
            SecondaryButtonLabel = source?.SecondaryButtonLabel ?? existing?.SecondaryButtonLabel ?? string.Empty,
            Success = source?.Success ?? existing?.Success ?? string.Empty,
            WidgetBackground = source?.WidgetBackground ?? existing?.WidgetBackground ?? string.Empty,
            WidgetBorder = source?.WidgetBorder ?? existing?.WidgetBorder ?? string.Empty,
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeFonts"/>.
        /// </summary>
        internal static BrandingThemeFonts ToApi(V2alpha3BrandingThemeFonts? source, BrandingThemeFonts? existing = null) => new()
        {
            BodyText = ToApi(source?.BodyText, existing?.BodyText),
            ButtonsText = ToApi(source?.ButtonsText, existing?.ButtonsText),
            FontUrl = source?.FontUrl ?? existing?.FontUrl ?? string.Empty,
            InputLabels = ToApi(source?.InputLabels, existing?.InputLabels),
            Links = ToApi(source?.Links, existing?.Links),
            LinksStyle = source?.LinksStyle is { } ls ? ToApiLinksStyle(ls) : existing?.LinksStyle ?? new BrandingThemeFontLinksStyleEnum(BrandingThemeFontLinksStyleEnum.Values.Normal),
            ReferenceTextSize = source?.ReferenceTextSize ?? (existing?.ReferenceTextSize is double ers ? (float)ers : 0f),
            Subtitle = ToApi(source?.Subtitle, existing?.Subtitle),
            Title = ToApi(source?.Title, existing?.Title),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemeWidget"/>.
        /// </summary>
        internal static BrandingThemeWidget ToApi(V2alpha3BrandingThemeWidget? source, BrandingThemeWidget? existing = null) => new()
        {
            HeaderTextAlignment = source?.HeaderTextAlignment is { } hta ? ToApi(hta) : existing?.HeaderTextAlignment ?? new BrandingThemeWidgetHeaderTextAlignmentEnum(BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center),
            LogoHeight = source?.LogoHeight ?? (existing?.LogoHeight is double elh ? (float)elh : 0f),
            LogoPosition = source?.LogoPosition is { } lp ? ToApi(lp) : existing?.LogoPosition ?? new BrandingThemeWidgetLogoPositionEnum(BrandingThemeWidgetLogoPositionEnum.Values.Center),
            LogoUrl = source?.LogoUrl ?? existing?.LogoUrl ?? string.Empty,
            SocialButtonsLayout = source?.SocialButtonsLayout is { } sbl ? ToApi(sbl) : existing?.SocialButtonsLayout ?? new BrandingThemeWidgetSocialButtonsLayoutEnum(BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="BrandingThemePageBackground"/>.
        /// </summary>
        internal static BrandingThemePageBackground ToApi(V2alpha3BrandingThemePageBackground? source, BrandingThemePageBackground? existing = null) => new()
        {
            BackgroundColor = source?.BackgroundColor ?? existing?.BackgroundColor ?? string.Empty,
            BackgroundImageUrl = source?.BackgroundImageUrl ?? existing?.BackgroundImageUrl ?? string.Empty,
            PageLayout = source?.PageLayout is { } pl ? ToApi(pl) : existing?.PageLayout ?? new BrandingThemePageBackgroundPageLayoutEnum(BrandingThemePageBackgroundPageLayoutEnum.Values.Center),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="CreateBrandingThemeRequestContent"/>.
        /// </summary>
        internal static CreateBrandingThemeRequestContent ToCreateRequest(V2alpha3BrandingThemeConf conf) => new()
        {
            DisplayName = conf.DisplayName,
            Borders = ToApi(conf.Borders),
            Colors = ToApi(conf.Colors),
            Fonts = ToApi(conf.Fonts),
            Widget = ToApi(conf.Widget),
            PageBackground = ToApi(conf.PageBackground),
        };

        /// <summary>
        /// Converts the specified configuration to a new <see cref="UpdateBrandingThemeRequestContent"/>,
        /// layering <paramref name="conf"/> over <paramref name="existing"/> so unmanaged fields are preserved.
        /// </summary>
        internal static UpdateBrandingThemeRequestContent ToUpdateRequest(V2alpha3BrandingThemeConf conf, GetBrandingThemeResponseContent existing) => new()
        {
            DisplayName = conf.DisplayName ?? existing?.DisplayName,
            Borders = ToApi(conf.Borders, existing?.Borders),
            Colors = ToApi(conf.Colors, existing?.Colors),
            Fonts = ToApi(conf.Fonts, existing?.Fonts),
            Widget = ToApi(conf.Widget, existing?.Widget),
            PageBackground = ToApi(conf.PageBackground, existing?.PageBackground),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeConf FromApi(GetBrandingThemeResponseContent source) => new()
        {
            DisplayName = source.DisplayName,
            Borders = FromApi(source.Borders),
            Colors = FromApi(source.Colors),
            Fonts = FromApi(source.Fonts),
            PageBackground = FromApi(source.PageBackground),
            Widget = FromApi(source.Widget),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeWidget? FromApi(BrandingThemeWidget? source) => source is null ? null : new()
        {
            HeaderTextAlignment = FromApi(source.HeaderTextAlignment),
            LogoHeight = (float)source.LogoHeight,
            LogoPosition = FromApi(source.LogoPosition),
            LogoUrl = source.LogoUrl,
            SocialButtonsLayout = FromApi(source.SocialButtonsLayout)
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V2alpha3BrandingThemeSocialButtonsLayout? FromApi(BrandingThemeWidgetSocialButtonsLayoutEnum source) => source.Value switch
        {
            BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Top => V2alpha3BrandingThemeSocialButtonsLayout.Top,
            BrandingThemeWidgetSocialButtonsLayoutEnum.Values.Bottom => V2alpha3BrandingThemeSocialButtonsLayout.Bottom,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V2alpha3BrandingThemeLogoPosition? FromApi(BrandingThemeWidgetLogoPositionEnum source) => source.Value switch
        {
            BrandingThemeWidgetLogoPositionEnum.Values.Center => V2alpha3BrandingThemeLogoPosition.Center,
            BrandingThemeWidgetLogoPositionEnum.Values.Left => V2alpha3BrandingThemeLogoPosition.Left,
            BrandingThemeWidgetLogoPositionEnum.Values.Right => V2alpha3BrandingThemeLogoPosition.Right,
            BrandingThemeWidgetLogoPositionEnum.Values.None => V2alpha3BrandingThemeLogoPosition.None,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V2alpha3BrandingThemeHeaderTextAlignment? FromApi(BrandingThemeWidgetHeaderTextAlignmentEnum source) => source.Value switch
        {
            BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Center => V2alpha3BrandingThemeHeaderTextAlignment.Center,
            BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Left => V2alpha3BrandingThemeHeaderTextAlignment.Left,
            BrandingThemeWidgetHeaderTextAlignmentEnum.Values.Right => V2alpha3BrandingThemeHeaderTextAlignment.Right,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemePageBackground? FromApi(BrandingThemePageBackground? source) => source is null ? null : new()
        {
            BackgroundColor = source.BackgroundColor,
            BackgroundImageUrl = source.BackgroundImageUrl,
            PageLayout = FromApi(source.PageLayout)
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V2alpha3BrandingThemePageLayout? FromApi(BrandingThemePageBackgroundPageLayoutEnum source) => source.Value switch
        {
            BrandingThemePageBackgroundPageLayoutEnum.Values.Center => V2alpha3BrandingThemePageLayout.Center,
            BrandingThemePageBackgroundPageLayoutEnum.Values.Left => V2alpha3BrandingThemePageLayout.Left,
            BrandingThemePageBackgroundPageLayoutEnum.Values.Right => V2alpha3BrandingThemePageLayout.Right,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFonts? FromApi(BrandingThemeFonts? source) => source is null ? null : new()
        {
            BodyText = FromApi(source.BodyText),
            ButtonsText = FromApi(source.ButtonsText),
            FontUrl = source.FontUrl,
            InputLabels = FromApi(source.InputLabels),
            Links = FromApi(source.Links),
            LinksStyle = FromApi(source.LinksStyle),
            ReferenceTextSize = (float)source.ReferenceTextSize,
            Subtitle = FromApi(source.Subtitle),
            Title = FromApi(source.Title)
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFont? FromApi(BrandingThemeFontTitle? source) => source is null ? null : new()
        {
            Bold = source.Bold,
            Size = (float)source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFont? FromApi(BrandingThemeFontLinks? source) => source is null ? null : new()
        {
            Bold = source.Bold,
            Size = (float)source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFont? FromApi(BrandingThemeFontSubtitle? source) => source is null ? null : new()
        {
            Bold = source.Bold,
            Size = (float)source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V2alpha3BrandingThemeLinksStyle? FromApi(BrandingThemeFontLinksStyleEnum source) => source.Value switch
        {
            BrandingThemeFontLinksStyleEnum.Values.Normal => V2alpha3BrandingThemeLinksStyle.Normal,
            BrandingThemeFontLinksStyleEnum.Values.Underlined => V2alpha3BrandingThemeLinksStyle.Underlined,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFont? FromApi(BrandingThemeFontInputLabels? source) => source is null ? null : new()
        {
            Bold = source.Bold,
            Size = (float)source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFont? FromApi(BrandingThemeFontButtonsText? source) => source is null ? null : new()
        {
            Bold = source.Bold,
            Size = (float)source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeFont? FromApi(BrandingThemeFontBodyText? source) => source is null ? null : new()
        {
            Bold = source.Bold,
            Size = (float)source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeColors? FromApi(BrandingThemeColors? source) => source is null ? null : new()
        {
            BaseFocusColor = source.BaseFocusColor,
            BaseHoverColor = source.BaseHoverColor,
            BodyText = source.BodyText,
            CaptchaWidgetTheme = source.CaptchaWidgetTheme is { } cwt ? FromApi(cwt) : null,
            Error = source.Error,
            Header = source.Header,
            Icons = source.Icons,
            InputBackground = source.InputBackground,
            InputBorder = source.InputBorder,
            InputFilledText = source.InputFilledText,
            InputLabelsPlaceholders = source.InputLabelsPlaceholders,
            LinksFocusedComponents = source.LinksFocusedComponents,
            PrimaryButton = source.PrimaryButton,
            PrimaryButtonLabel = source.PrimaryButtonLabel,
            SecondaryButtonBorder = source.SecondaryButtonBorder,
            SecondaryButtonLabel = source.SecondaryButtonLabel,
            Success = source.Success,
            WidgetBackground = source.WidgetBackground,
            WidgetBorder = source.WidgetBorder
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V2alpha3BrandingThemeCaptchaWidgetTheme? FromApi(BrandingThemeColorsCaptchaWidgetThemeEnum source) => source.Value switch
        {
            BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Light => V2alpha3BrandingThemeCaptchaWidgetTheme.Light,
            BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Dark => V2alpha3BrandingThemeCaptchaWidgetTheme.Dark,
            BrandingThemeColorsCaptchaWidgetThemeEnum.Values.Auto => V2alpha3BrandingThemeCaptchaWidgetTheme.Auto,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V2alpha3BrandingThemeBorders? FromApi(BrandingThemeBorders? source) => source is null ? null : new()
        {
            ButtonBorderRadius = (float)source.ButtonBorderRadius,
            ButtonBorderWeight = (float)source.ButtonBorderWeight,
            InputBorderRadius = (float)source.InputBorderRadius,
            InputBorderWeight = (float)source.InputBorderWeight,
            ShowWidgetShadow = source.ShowWidgetShadow,
            WidgetBorderWeight = (float)source.WidgetBorderWeight,
            WidgetCornerRadius = (float)source.WidgetCornerRadius,
            ButtonsStyle = FromApi(source.ButtonsStyle),
            InputsStyle = FromApi(source.InputsStyle),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static V2alpha3BrandingThemeButtonsStyle FromApi(BrandingThemeBordersButtonsStyleEnum source) => source.Value switch
        {
            BrandingThemeBordersButtonsStyleEnum.Values.Pill => V2alpha3BrandingThemeButtonsStyle.Pill,
            BrandingThemeBordersButtonsStyleEnum.Values.Rounded => V2alpha3BrandingThemeButtonsStyle.Rounded,
            BrandingThemeBordersButtonsStyleEnum.Values.Sharp => V2alpha3BrandingThemeButtonsStyle.Sharp,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static V2alpha3BrandingThemeButtonsStyle FromApi(BrandingThemeBordersInputsStyleEnum source) => source.Value switch
        {
            BrandingThemeBordersInputsStyleEnum.Values.Pill => V2alpha3BrandingThemeButtonsStyle.Pill,
            BrandingThemeBordersInputsStyleEnum.Values.Rounded => V2alpha3BrandingThemeButtonsStyle.Rounded,
            BrandingThemeBordersInputsStyleEnum.Values.Sharp => V2alpha3BrandingThemeButtonsStyle.Sharp,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3BrandingThemeController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3BrandingThemeController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "BrandingTheme";

        /// <inheritdoc />
        protected override async Task<V2alpha3BrandingThemeConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.Branding.Themes.GetAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3BrandingTheme entity, V2alpha3BrandingTheme.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.Id is string id)
                {
                    try
                    {
                        var theme = await api.Branding.Themes.GetAsync(id, cancellationToken: cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing theme: {DisplayName}", EntityTypeName, entity.Namespace(), entity.Name(), theme.DisplayName);
                        return theme.ThemeId;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find theme with id {ThemeId}.", EntityTypeName, entity.Namespace(), entity.Name(), id);
                        return null;
                    }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V2alpha3BrandingThemeConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating theme in Auth0 with name: {ThemeName}", EntityTypeName, conf.DisplayName);

            var self = await api.Branding.Themes.CreateAsync(ToCreateRequest(conf), cancellationToken: cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created theme in Auth0 with ID: {ThemeId} and name: {ThemeName}", EntityTypeName, self.ThemeId, conf.DisplayName);
            return self.ThemeId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3BrandingThemeConf? last, V2alpha3BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);

            // fetch the current state so unmanaged fields are preserved
            var self = await api.Branding.Themes.GetAsync(id, cancellationToken: cancellationToken);
            await api.Branding.Themes.UpdateAsync(id, ToUpdateRequest(conf, self), cancellationToken: cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V2alpha3BrandingTheme entity, V2alpha3BrandingThemeConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting theme from Auth0 with ID: {ThemeId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Branding.Themes.DeleteAsync(id, cancellationToken: cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted theme from Auth0 with ID: {ThemeId}", EntityTypeName, id);
        }

    }

}
