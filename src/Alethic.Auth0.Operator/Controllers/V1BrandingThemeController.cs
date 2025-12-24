using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.BrandingTheme;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using k8s.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Queue;
using KubeOps.Abstractions.Rbac;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1BrandingTheme), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1BrandingThemeController :
        V1TenantEntityController<V1BrandingTheme, V1BrandingTheme.SpecDef, V1BrandingTheme.StatusDef, BrandingThemeConf, BrandingThemeConf>,
        IEntityController<V1BrandingTheme>
    {

        static global::Auth0.ManagementApi.Models.CaptchaWidgetTheme ToApi(Core.Models.BrandingTheme.CaptchaWidgetTheme source) => source switch
        {
            Core.Models.BrandingTheme.CaptchaWidgetTheme.Light => global::Auth0.ManagementApi.Models.CaptchaWidgetTheme.Light,
            Core.Models.BrandingTheme.CaptchaWidgetTheme.Dark => global::Auth0.ManagementApi.Models.CaptchaWidgetTheme.Dark,
            _ => throw new InvalidOperationException()
        };

        static global::Auth0.ManagementApi.Models.LinksStyle ToApi(Core.Models.BrandingTheme.BrandingThemeFonts source) => source.LinksStyle switch
        {
            Core.Models.BrandingTheme.LinksStyle.Normal => global::Auth0.ManagementApi.Models.LinksStyle.Normal,
            Core.Models.BrandingTheme.LinksStyle.Underlined => global::Auth0.ManagementApi.Models.LinksStyle.Underlined,
            _ => throw new InvalidOperationException()
        };

        static global::Auth0.ManagementApi.Models.SocialButtonsLayout ToApi(Core.Models.BrandingTheme.SocialButtonsLayout source) => source switch
        {
            Core.Models.BrandingTheme.SocialButtonsLayout.Top => global::Auth0.ManagementApi.Models.SocialButtonsLayout.Top,
            Core.Models.BrandingTheme.SocialButtonsLayout.Bottom => global::Auth0.ManagementApi.Models.SocialButtonsLayout.Bottom,
            _ => throw new InvalidOperationException()
        };

        static global::Auth0.ManagementApi.Models.LogoPosition ToApi(Core.Models.BrandingTheme.LogoPosition source) => source switch
        {
            Core.Models.BrandingTheme.LogoPosition.Center => global::Auth0.ManagementApi.Models.LogoPosition.Center,
            Core.Models.BrandingTheme.LogoPosition.Left => global::Auth0.ManagementApi.Models.LogoPosition.Left,
            Core.Models.BrandingTheme.LogoPosition.Right => global::Auth0.ManagementApi.Models.LogoPosition.Right,
            Core.Models.BrandingTheme.LogoPosition.None => global::Auth0.ManagementApi.Models.LogoPosition.None,
            _ => throw new InvalidOperationException()
        };

        static global::Auth0.ManagementApi.Models.HeaderTextAlignment ToApi(Core.Models.BrandingTheme.HeaderTextAlignment source) => source switch
        {
            Core.Models.BrandingTheme.HeaderTextAlignment.Center => global::Auth0.ManagementApi.Models.HeaderTextAlignment.Center,
            Core.Models.BrandingTheme.HeaderTextAlignment.Left => global::Auth0.ManagementApi.Models.HeaderTextAlignment.Left,
            Core.Models.BrandingTheme.HeaderTextAlignment.Right => global::Auth0.ManagementApi.Models.HeaderTextAlignment.Right,
            _ => throw new InvalidOperationException()
        };

        static global::Auth0.ManagementApi.Models.PageLayout ToApi(Core.Models.BrandingTheme.PageLayout source) => source switch
        {
            Core.Models.BrandingTheme.PageLayout.Right => global::Auth0.ManagementApi.Models.PageLayout.Right,
            Core.Models.BrandingTheme.PageLayout.Center => global::Auth0.ManagementApi.Models.PageLayout.Center,
            Core.Models.BrandingTheme.PageLayout.Left => global::Auth0.ManagementApi.Models.PageLayout.Left,
            _ => throw new InvalidOperationException()
        };

        static global::Auth0.ManagementApi.Models.ButtonsStyle ToApi(Core.Models.BrandingTheme.ButtonsStyle buttonsStyle) => buttonsStyle switch
        {
            Core.Models.BrandingTheme.ButtonsStyle.Pill => global::Auth0.ManagementApi.Models.ButtonsStyle.Pill,
            Core.Models.BrandingTheme.ButtonsStyle.Rounded => global::Auth0.ManagementApi.Models.ButtonsStyle.Rounded,
            Core.Models.BrandingTheme.ButtonsStyle.Sharp => global::Auth0.ManagementApi.Models.ButtonsStyle.Sharp,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        static void ApplyToApi(BrandingThemeConf? source, BrandingThemeBase target)
        {
            if (source is null)
                return;

            if (source.DisplayName is not null)
                target.DisplayName = source.DisplayName;

            if (source.Borders is not null)
                ApplyToApi(source.Borders, target.Borders = new global::Auth0.ManagementApi.Models.BrandingThemeBorder());

            if (source.Colors is not null)
                ApplyToApi(source.Colors, target.Colors = new global::Auth0.ManagementApi.Models.BrandingThemeColors());

            if (source.Fonts is not null)
                ApplyToApi(source.Fonts, target.Fonts = new global::Auth0.ManagementApi.Models.BrandingThemeFonts());

            if (source.Widget is not null)
                ApplyToApi(source.Widget, target.Widget = new global::Auth0.ManagementApi.Models.BrandingThemeWidget());

            if (source.PageBackground is not null)
                ApplyToApi(source.PageBackground, target.PageBackground = new global::Auth0.ManagementApi.Models.BrandingThemePageBackground());
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="InvalidOperationException"></exception>
        static void ApplyToApi(Core.Models.BrandingTheme.BrandingThemeBorders? source, global::Auth0.ManagementApi.Models.BrandingThemeBorder target)
        {
            if (source is null)
                return;

            if (source.ButtonBorderRadius is float buttonBorderRadius)
                target.ButtonBorderRadius = buttonBorderRadius;

            if (source.ButtonBorderWeight is float buttonBorderWeight)
                target.ButtonBorderWeight = buttonBorderWeight;

            if (source.ButtonsStyle is Core.Models.BrandingTheme.ButtonsStyle buttonsStyle)
                target.ButtonsStyle = ToApi(buttonsStyle);

            if (source.InputBorderRadius is float inputBorderRadius)
                target.InputBorderRadius = inputBorderRadius;

            if (source.InputBorderWeight is float inputBorderWeight)
                target.InputBorderWeight = inputBorderWeight;

            if (source.InputsStyle is Core.Models.BrandingTheme.ButtonsStyle inputsStyle)
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
        static void ApplyToApi(Core.Models.BrandingTheme.BrandingThemeColors? source, global::Auth0.ManagementApi.Models.BrandingThemeColors target)
        {
            if (source is null)
                return;

            if (source.BaseFocusColor is string baseFocusColor)
                target.BaseFocusColor = baseFocusColor;

            if (source.BaseHoverColor is string baseHoverColor)
                target.BaseHoverColor = baseHoverColor;

            if (source.BodyText is string bodyText)
                target.BodyText = bodyText;

            if (source.CaptchaWidgetTheme is Core.Models.BrandingTheme.CaptchaWidgetTheme captchaWidgetTheme)
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
        static void ApplyToApi(Core.Models.BrandingTheme.BrandingThemeFonts? source, global::Auth0.ManagementApi.Models.BrandingThemeFonts target)
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
        static void ApplyToApi(BrandingThemeFont? source, BrandingThemeFontsBase target)
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
        static void ApplyToApi(Core.Models.BrandingTheme.BrandingThemeWidget? source, global::Auth0.ManagementApi.Models.BrandingThemeWidget target)
        {
            if (source is null)
                return;

            if (source.HeaderTextAlignment is Core.Models.BrandingTheme.HeaderTextAlignment headerTextAlignment)
                target.HeaderTextAlignment = ToApi(headerTextAlignment);

            if (source.LogoHeight is float logoHeight)
                target.LogoHeight = logoHeight;

            if (source.LogoPosition is Core.Models.BrandingTheme.LogoPosition logoPosition)
                target.LogoPosition = ToApi(logoPosition);

            if (source.LogoUrl is string logoUrl)
                target.LogoUrl = logoUrl;

            if (source.SocialButtonsLayout is Core.Models.BrandingTheme.SocialButtonsLayout socialButtonsLayout)
                target.SocialButtonsLayout = ToApi(socialButtonsLayout);
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        static void ApplyToApi(Core.Models.BrandingTheme.BrandingThemePageBackground? source, global::Auth0.ManagementApi.Models.BrandingThemePageBackground target)
        {
            if (source is null)
                return;

            if (source.BackgroundColor is string backgroundColor)
                target.BackgroundColor = backgroundColor;

            if (source.BackgroundImageUrl is string backgroundImageUrl)
                target.BackgroundImageUrl = backgroundImageUrl;

            if (source.PageLayout is Core.Models.BrandingTheme.PageLayout pageLayout)
                target.PageLayout = ToApi(pageLayout);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1BrandingThemeController(IKubernetesClient kube, EntityRequeue<V1BrandingTheme> requeue, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1BrandingThemeController> logger) :
            base(kube, requeue, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "BrandingTheme";

        /// <inheritdoc />
        protected override async Task<BrandingThemeConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return TransformToSystemTextJson<BrandingThemeConf>(await api.Branding.GetBrandingThemeAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1BrandingTheme entity, V1BrandingTheme.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
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
        protected override string? ValidateCreate(BrandingThemeConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating theme in Auth0 with name: {ThemeName}", EntityTypeName, conf.DisplayName);

            var req = new BrandingThemeCreateRequest();
            ApplyToApi(conf, req);

            var self = await api.Branding.CreateBrandingThemeAsync(req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created theme in Auth0 with ID: {ThemeId} and name: {ThemeName}", EntityTypeName, self.ThemeId, conf.DisplayName);
            return self.ThemeId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, BrandingThemeConf? last, BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
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
        protected override async Task ApplyStatus(IManagementApiClient api, V1BrandingTheme entity, BrandingThemeConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting theme from Auth0 with ID: {ThemeId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Branding.DeleteBrandingThemeAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted theme from Auth0 with ID: {ThemeId}", EntityTypeName, id);
        }

    }

}
