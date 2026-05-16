using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1alpha1BrandingTheme), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1alpha1BrandingThemeController :
        V1TenantEntityInstanceController<V1alpha1BrandingTheme, V1alpha1BrandingTheme.SpecDef, V1alpha1BrandingTheme.StatusDef, V1alpha1BrandingThemeConf, V1alpha1BrandingThemeConf>,
        IEntityController<V1alpha1BrandingTheme>
    {

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static CaptchaWidgetTheme ToApi(V1alpha1BrandingThemeCaptchaWidgetTheme source) => source switch
        {
            V1alpha1BrandingThemeCaptchaWidgetTheme.Light => CaptchaWidgetTheme.Light,
            V1alpha1BrandingThemeCaptchaWidgetTheme.Dark => CaptchaWidgetTheme.Dark,
            V1alpha1BrandingThemeCaptchaWidgetTheme.Auto => CaptchaWidgetTheme.Auto,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static LinksStyle ToApi(V1alpha1BrandingThemeFonts source) => source.LinksStyle switch
        {
            V1alpha1BrandingThemeLinksStyle.Normal => LinksStyle.Normal,
            V1alpha1BrandingThemeLinksStyle.Underlined => LinksStyle.Underlined,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static SocialButtonsLayout ToApi(V1alpha1BrandingThemeSocialButtonsLayout source) => source switch
        {
            V1alpha1BrandingThemeSocialButtonsLayout.Top => SocialButtonsLayout.Top,
            V1alpha1BrandingThemeSocialButtonsLayout.Bottom => SocialButtonsLayout.Bottom,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static LogoPosition ToApi(V1alpha1BrandingThemeLogoPosition source) => source switch
        {
            V1alpha1BrandingThemeLogoPosition.Center => LogoPosition.Center,
            V1alpha1BrandingThemeLogoPosition.Left => LogoPosition.Left,
            V1alpha1BrandingThemeLogoPosition.Right => LogoPosition.Right,
            V1alpha1BrandingThemeLogoPosition.None => LogoPosition.None,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static HeaderTextAlignment ToApi(V1alpha1BrandingThemeHeaderTextAlignment source) => source switch
        {
            V1alpha1BrandingThemeHeaderTextAlignment.Center => HeaderTextAlignment.Center,
            V1alpha1BrandingThemeHeaderTextAlignment.Left => HeaderTextAlignment.Left,
            V1alpha1BrandingThemeHeaderTextAlignment.Right => HeaderTextAlignment.Right,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static PageLayout ToApi(V1alpha1BrandingThemePageLayout source) => source switch
        {
            V1alpha1BrandingThemePageLayout.Right => PageLayout.Right,
            V1alpha1BrandingThemePageLayout.Center => PageLayout.Center,
            V1alpha1BrandingThemePageLayout.Left => PageLayout.Left,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source to the API type.
        /// </summary>
        /// <param name="buttonsStyle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static ButtonsStyle ToApi(V1alpha1BrandingThemeButtonsStyle buttonsStyle) => buttonsStyle switch
        {
            V1alpha1BrandingThemeButtonsStyle.Pill => ButtonsStyle.Pill,
            V1alpha1BrandingThemeButtonsStyle.Rounded => ButtonsStyle.Rounded,
            V1alpha1BrandingThemeButtonsStyle.Sharp => ButtonsStyle.Sharp,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1alpha1BrandingThemeConf? source, BrandingThemeBase target)
        {
            if (source is null)
                return;

            if (source.DisplayName is not null)
                target.DisplayName = source.DisplayName;

            if (source.Borders is not null)
                ApplyToApi(source.Borders, target.Borders = new BrandingThemeBorder());

            if (source.Colors is not null)
                ApplyToApi(source.Colors, target.Colors = new BrandingThemeColors());

            if (source.Fonts is not null)
                ApplyToApi(source.Fonts, target.Fonts = new BrandingThemeFonts());

            if (source.Widget is not null)
                ApplyToApi(source.Widget, target.Widget = new BrandingThemeWidget());

            if (source.PageBackground is not null)
                ApplyToApi(source.PageBackground, target.PageBackground = new BrandingThemePageBackground());
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal static void ApplyToApi(V1alpha1BrandingThemeBorders? source, BrandingThemeBorder target)
        {
            if (source is null)
                return;

            if (source.ButtonBorderRadius is float buttonBorderRadius)
                target.ButtonBorderRadius = buttonBorderRadius;

            if (source.ButtonBorderWeight is float buttonBorderWeight)
                target.ButtonBorderWeight = buttonBorderWeight;

            if (source.ButtonsStyle is V1alpha1BrandingThemeButtonsStyle buttonsStyle)
                target.ButtonsStyle = ToApi(buttonsStyle);

            if (source.InputBorderRadius is float inputBorderRadius)
                target.InputBorderRadius = inputBorderRadius;

            if (source.InputBorderWeight is float inputBorderWeight)
                target.InputBorderWeight = inputBorderWeight;

            if (source.InputsStyle is V1alpha1BrandingThemeButtonsStyle inputsStyle)
                target.InputsStyle = ToApi(inputsStyle);

            if (source.ShowWidgetShadow is bool showWidgetShadow)
                target.ShowWidgetShadow = showWidgetShadow;

            if (source.WidgetBorderWeight is float widgetBorderWeight)
                target.WidgetBorderWeight = widgetBorderWeight;

            if (source.WidgetCornerRadius is float widgetCornerRadius)
                target.WidgetCornerRadius = widgetCornerRadius;
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1alpha1BrandingThemeColors? source, BrandingThemeColors target)
        {
            if (source is null)
                return;

            if (source.BaseFocusColor is string baseFocusColor)
                target.BaseFocusColor = baseFocusColor;

            if (source.BaseHoverColor is string baseHoverColor)
                target.BaseHoverColor = baseHoverColor;

            if (source.BodyText is string bodyText)
                target.BodyText = bodyText;

            if (source.CaptchaWidgetTheme is V1alpha1BrandingThemeCaptchaWidgetTheme captchaWidgetTheme)
                target.CaptchaWidgetTheme = ToApi(captchaWidgetTheme);

            if (source.Error is string error)
                target.Error = error;

            if (source.Header is string header)
                target.Header = header;

            if (source.Icons is string icons)
                target.Icons = icons;

            if (source.InputBackground is string inputBackground)
                target.InputBackground = inputBackground;

            if (source.InputBorder is string inputBorder)
                target.InputBorder = inputBorder;

            if (source.InputFilledText is string inputFilledText)
                target.InputFilledText = inputFilledText;

            if (source.InputLabelsPlaceholders is string inputLabelsPlaceholders)
                target.InputLabelsPlaceholders = inputLabelsPlaceholders;

            if (source.LinksFocusedComponents is string linksFocusedComponents)
                target.LinksFocusedComponents = linksFocusedComponents;

            if (source.PrimaryButton is string primaryButton)
                target.PrimaryButton = primaryButton;

            if (source.PrimaryButtonLabel is string primaryButtonLabel)
                target.PrimaryButtonLabel = primaryButtonLabel;

            if (source.SecondaryButtonBorder is string secondaryButtonBorder)
                target.SecondaryButtonBorder = secondaryButtonBorder;

            if (source.SecondaryButtonLabel is string secondaryButtonLabel)
                target.SecondaryButtonLabel = secondaryButtonLabel;

            if (source.Success is string success)
                target.Success = success;

            if (source.WidgetBackground is string widgetBackground)
                target.WidgetBackground = widgetBackground;

            if (source.WidgetBorder is string widgetBorder)
                target.WidgetBorder = widgetBorder;
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1alpha1BrandingThemeFonts? source, BrandingThemeFonts target)
        {
            if (source is null)
                return;

            if (source.BodyText is not null)
                ApplyToApi(source.BodyText, target.BodyText = new BodyText());

            if (source.ButtonsText is not null)
                ApplyToApi(source.ButtonsText, target.ButtonsText = new ButtonsText());

            if (source.FontUrl is not null)
                target.FontUrl = source.FontUrl;

            if (source.InputLabels is not null)
                ApplyToApi(source.InputLabels, target.InputLabels = new InputLabels());

            if (source.Links is not null)
                ApplyToApi(source.Links, target.Links = new Links());

            if (source.LinksStyle is not null)
                target.LinksStyle = ToApi(source);

            if (source.ReferenceTextSize is float referenceTextSize)
                target.ReferenceTextSize = referenceTextSize;

            if (source.Subtitle is not null)
                ApplyToApi(source.Subtitle, target.Subtitle = new Subtitle());

            if (source.Title is not null)
                ApplyToApi(source.Title, target.Title = new Title());
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1alpha1BrandingThemeFont? source, BrandingThemeFontsBase target)
        {
            if (source is null)
                return;

            if (source.Bold is bool bold)
                target.Bold = bold;

            if (source.Size is float size)
                target.Size = size;
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1alpha1BrandingThemeWidget? source, BrandingThemeWidget target)
        {
            if (source is null)
                return;

            if (source.HeaderTextAlignment is V1alpha1BrandingThemeHeaderTextAlignment headerTextAlignment)
                target.HeaderTextAlignment = ToApi(headerTextAlignment);

            if (source.LogoHeight is float logoHeight)
                target.LogoHeight = logoHeight;

            if (source.LogoPosition is V1alpha1BrandingThemeLogoPosition logoPosition)
                target.LogoPosition = ToApi(logoPosition);

            if (source.LogoUrl is string logoUrl)
                target.LogoUrl = logoUrl;

            if (source.SocialButtonsLayout is V1alpha1BrandingThemeSocialButtonsLayout socialButtonsLayout)
                target.SocialButtonsLayout = ToApi(socialButtonsLayout);
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1alpha1BrandingThemePageBackground? source, BrandingThemePageBackground target)
        {
            if (source is null)
                return;

            if (source.BackgroundColor is string backgroundColor)
                target.BackgroundColor = backgroundColor;

            if (source.BackgroundImageUrl is string backgroundImageUrl)
                target.BackgroundImageUrl = backgroundImageUrl;

            if (source.PageLayout is V1alpha1BrandingThemePageLayout pageLayout)
                target.PageLayout = ToApi(pageLayout);
        }

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeConf FromApi(BrandingTheme source) => new()
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
        internal static V1alpha1BrandingThemeWidget FromApi(BrandingThemeWidget source) => new()
        {
            HeaderTextAlignment = FromApi(source.HeaderTextAlignment),
            LogoHeight = source.LogoHeight,
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
        internal static V1alpha1BrandingThemeSocialButtonsLayout? FromApi(SocialButtonsLayout source) => source switch
        {
            SocialButtonsLayout.Top => V1alpha1BrandingThemeSocialButtonsLayout.Top,
            SocialButtonsLayout.Bottom => V1alpha1BrandingThemeSocialButtonsLayout.Bottom,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V1alpha1BrandingThemeLogoPosition? FromApi(LogoPosition source) => source switch
        {
            LogoPosition.Center => V1alpha1BrandingThemeLogoPosition.Center,
            LogoPosition.Left => V1alpha1BrandingThemeLogoPosition.Left,
            LogoPosition.Right => V1alpha1BrandingThemeLogoPosition.Right,
            LogoPosition.None => V1alpha1BrandingThemeLogoPosition.None,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V1alpha1BrandingThemeHeaderTextAlignment? FromApi(HeaderTextAlignment source) => source switch
        {
            HeaderTextAlignment.Center => V1alpha1BrandingThemeHeaderTextAlignment.Center,
            HeaderTextAlignment.Left => V1alpha1BrandingThemeHeaderTextAlignment.Left,
            HeaderTextAlignment.Right => V1alpha1BrandingThemeHeaderTextAlignment.Right,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemePageBackground FromApi(BrandingThemePageBackground source) => new()
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
        internal static V1alpha1BrandingThemePageLayout? FromApi(PageLayout source) => source switch
        {
            PageLayout.Center => V1alpha1BrandingThemePageLayout.Center,
            PageLayout.Left => V1alpha1BrandingThemePageLayout.Left,
            PageLayout.Right => V1alpha1BrandingThemePageLayout.Right,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFonts FromApi(BrandingThemeFonts source) => new()
        {
            BodyText = FromApi(source.BodyText),
            ButtonsText = FromApi(source.ButtonsText),
            FontUrl = source.FontUrl,
            InputLabels = FromApi(source.InputLabels),
            Links = FromApi(source.Links),
            LinksStyle = FromApi(source.LinksStyle),
            ReferenceTextSize = source.ReferenceTextSize,
            Subtitle = FromApi(source.Subtitle),
            Title = FromApi(source.Title)
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFont FromApi(Title source) => new()
        {
            Bold = source.Bold,
            Size = source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFont FromApi(Links source) => new()
        {
            Bold = source.Bold,
            Size = source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFont FromApi(Subtitle source) => new()
        {
            Bold = source.Bold,
            Size = source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static V1alpha1BrandingThemeLinksStyle? FromApi(LinksStyle source) => source switch
        {
            LinksStyle.Normal => V1alpha1BrandingThemeLinksStyle.Normal,
            LinksStyle.Underlined => V1alpha1BrandingThemeLinksStyle.Underlined,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFont FromApi(InputLabels source) => new()
        {
            Bold = source.Bold,
            Size = source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFont FromApi(ButtonsText source) => new()
        {
            Bold = source.Bold,
            Size = source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeFont FromApi(BodyText source) => new()
        {
            Bold = source.Bold,
            Size = source.Size
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeColors FromApi(BrandingThemeColors source) => new()
        {
            BaseFocusColor = source.BaseFocusColor,
            BaseHoverColor = source.BaseHoverColor,
            BodyText = source.BodyText,
            CaptchaWidgetTheme = FromApi(source.CaptchaWidgetTheme),
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
        internal static V1alpha1BrandingThemeCaptchaWidgetTheme? FromApi(CaptchaWidgetTheme source) => source switch
        {
            CaptchaWidgetTheme.Light => V1alpha1BrandingThemeCaptchaWidgetTheme.Light,
            CaptchaWidgetTheme.Dark => V1alpha1BrandingThemeCaptchaWidgetTheme.Dark,
            CaptchaWidgetTheme.Auto => V1alpha1BrandingThemeCaptchaWidgetTheme.Auto,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1alpha1BrandingThemeBorders? FromApi(BrandingThemeBorder source) => new()
        {
            ButtonBorderRadius = source.ButtonBorderRadius,
            ButtonBorderWeight = source.ButtonBorderWeight,
            InputBorderRadius = source.InputBorderRadius,
            InputBorderWeight = source.InputBorderWeight,
            ShowWidgetShadow = source.ShowWidgetShadow,
            WidgetBorderWeight = source.WidgetBorderWeight,
            WidgetCornerRadius = source.WidgetCornerRadius,
            ButtonsStyle = FromApi(source.ButtonsStyle),
            InputsStyle = FromApi(source.InputsStyle),
        };

        /// <summary>
        /// Transforms the specified source from the API type.
        /// </summary>
        /// <param name="inputsStyle"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static V1alpha1BrandingThemeButtonsStyle FromApi(ButtonsStyle inputsStyle) => inputsStyle switch
        {
            ButtonsStyle.Pill => V1alpha1BrandingThemeButtonsStyle.Pill,
            ButtonsStyle.Rounded => V1alpha1BrandingThemeButtonsStyle.Rounded,
            ButtonsStyle.Sharp => V1alpha1BrandingThemeButtonsStyle.Sharp,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1alpha1BrandingThemeController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1alpha1BrandingThemeController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "BrandingTheme";

        /// <inheritdoc />
        protected override async Task<V1alpha1BrandingThemeConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.Branding.GetBrandingThemeAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1alpha1BrandingTheme entity, V1alpha1BrandingTheme.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.Id is string id)
                {
                    try
                    {
                        var theme = await api.Branding.GetBrandingThemeAsync(id, cancellationToken);
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
        protected override string? ValidateCreate(V1alpha1BrandingThemeConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1alpha1BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating theme in Auth0 with name: {ThemeName}", EntityTypeName, conf.DisplayName);

            var req = new BrandingThemeCreateRequest();
            ApplyToApi(conf, req);

            var self = await api.Branding.CreateBrandingThemeAsync(req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created theme in Auth0 with ID: {ThemeId} and name: {ThemeName}", EntityTypeName, self.ThemeId, conf.DisplayName);
            return self.ThemeId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1alpha1BrandingThemeConf? last, V1alpha1BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);

            // apply last conf to request to ensure we don't overwrite any properties not managed by us
            var req = new BrandingThemeUpdateRequest();
            ApplyToApi(last, req);
            ApplyToApi(conf, req);

            await api.Branding.UpdateBrandingThemeAsync(id, req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1alpha1BrandingTheme entity, V1alpha1BrandingThemeConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting theme from Auth0 with ID: {ThemeId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Branding.DeleteBrandingThemeAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted theme from Auth0 with ID: {ThemeId}", EntityTypeName, id);
        }

    }

}
