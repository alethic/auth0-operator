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

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="target"></param>
        void Apply(BrandingThemeConf conf, BrandingThemeBase target)
        {
            if (conf.DisplayName is not null)
                target.DisplayName = conf.DisplayName;

            if (conf.Borders is not null)
                Apply(conf.Borders, target.Borders = new global::Auth0.ManagementApi.Models.BrandingThemeBorder());

            if (conf.Colors is not null)
                Apply(conf.Colors, target.Colors = new global::Auth0.ManagementApi.Models.BrandingThemeColors());

            if (conf.Fonts is not null)
                Apply(conf.Fonts, target.Fonts = new global::Auth0.ManagementApi.Models.BrandingThemeFonts());

            if (conf.Widget is not null)
                Apply(conf.Widget, target.Widget = new global::Auth0.ManagementApi.Models.BrandingThemeWidget());

            if (conf.PageBackground is not null)
                Apply(conf.PageBackground, target.PageBackground = new global::Auth0.ManagementApi.Models.BrandingThemePageBackground());
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="InvalidOperationException"></exception>
        void Apply(Core.Models.BrandingTheme.BrandingThemeBorders source, global::Auth0.ManagementApi.Models.BrandingThemeBorder target)
        {
            if (source.ButtonBorderRadius is float buttonBorderRadius)
                target.ButtonBorderRadius = buttonBorderRadius;

            if (source.ButtonBorderWeight is float buttonBorderWeight)
                target.ButtonBorderWeight = buttonBorderWeight;

            if (source.ButtonsStyle is Core.Models.BrandingTheme.ButtonsStyle buttonsStyle)
                target.ButtonsStyle = buttonsStyle switch
                {
                    Core.Models.BrandingTheme.ButtonsStyle.Pill => global::Auth0.ManagementApi.Models.ButtonsStyle.Pill,
                    Core.Models.BrandingTheme.ButtonsStyle.Rounded => global::Auth0.ManagementApi.Models.ButtonsStyle.Rounded,
                    Core.Models.BrandingTheme.ButtonsStyle.Sharp => global::Auth0.ManagementApi.Models.ButtonsStyle.Sharp,
                    _ => throw new InvalidOperationException()
                };

            if (source.InputBorderRadius is float inputBorderRadius)
                target.InputBorderRadius = inputBorderRadius;

            if (source.InputBorderWeight is float inputBorderWeight)
                target.InputBorderWeight = inputBorderWeight;

            if (source.InputsStyle is Core.Models.BrandingTheme.ButtonsStyle inputsStyle)
                target.InputsStyle = inputsStyle switch
                {
                    Core.Models.BrandingTheme.ButtonsStyle.Pill => global::Auth0.ManagementApi.Models.ButtonsStyle.Pill,
                    Core.Models.BrandingTheme.ButtonsStyle.Rounded => global::Auth0.ManagementApi.Models.ButtonsStyle.Rounded,
                    Core.Models.BrandingTheme.ButtonsStyle.Sharp => global::Auth0.ManagementApi.Models.ButtonsStyle.Sharp,
                    _ => throw new InvalidOperationException()
                };

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
        /// <param name="colors"></param>
        /// <param name="target"></param>
        void Apply(Core.Models.BrandingTheme.BrandingThemeColors colors, global::Auth0.ManagementApi.Models.BrandingThemeColors target)
        {
            if (colors.BaseFocusColor is string baseFocusColor)
                target.BaseFocusColor = baseFocusColor;

            if (colors.BaseHoverColor is string baseHoverColor)
                target.BaseHoverColor = baseHoverColor;

            if (colors.BodyText is string bodyText)
                target.BodyText = bodyText;

            if (colors.CaptchaWidgetTheme is Core.Models.BrandingTheme.CaptchaWidgetTheme captchaWidgetTheme)
                target.CaptchaWidgetTheme = ToApi(captchaWidgetTheme);

            if (colors.Error is string error)
                target.Error = error;

            if (colors.Header is string header)
                target.Header = header;

            if (colors.Icons is string icons)
                target.Icons = icons;

            if (colors.InputBackground is string inputBackground)
                target.InputBackground = inputBackground;

            if (colors.InputBorder is string inputBorder)
                target.InputBorder = inputBorder;

            if (colors.InputFilledText is string inputFilledText)
                target.InputFilledText = inputFilledText;

            if (colors.InputLabelsPlaceholders is string inputLabelsPlaceholders)
                target.InputLabelsPlaceholders = inputLabelsPlaceholders;

            if (colors.LinksFocusedComponents is string linksFocusedComponents)
                target.LinksFocusedComponents = linksFocusedComponents;

            if (colors.PrimaryButton is string primaryButton)
                target.PrimaryButton = primaryButton;

            if (colors.PrimaryButtonLabel is string primaryButtonLabel)
                target.PrimaryButtonLabel = primaryButtonLabel;

            if (colors.SecondaryButtonBorder is string secondaryButtonBorder)
                target.SecondaryButtonBorder = secondaryButtonBorder;

            if (colors.SecondaryButtonLabel is string secondaryButtonLabel)
                target.SecondaryButtonLabel = secondaryButtonLabel;

            if (colors.Success is string success)
                target.Success = success;

            if (colors.WidgetBackground is string widgetBackground)
                target.WidgetBackground = widgetBackground;

            if (colors.WidgetBorder is string widgetBorder)
                target.WidgetBorder = widgetBorder;
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="fonts"></param>
        /// <param name="target"></param>
        void Apply(Core.Models.BrandingTheme.BrandingThemeFonts fonts, global::Auth0.ManagementApi.Models.BrandingThemeFonts target)
        {
            if (fonts.BodyText is not null)
                Apply(fonts.BodyText, target.BodyText = new BodyText());

            if (fonts.ButtonsText is not null)
                Apply(fonts.ButtonsText, target.ButtonsText = new ButtonsText());

            if (fonts.FontUrl is not null)
                target.FontUrl = fonts.FontUrl;

            if (fonts.InputLabels is not null)
                Apply(fonts.InputLabels, target.InputLabels = new InputLabels());

            if (fonts.Links is not null)
                Apply(fonts.Links, target.Links = new Links());

            if (fonts.LinksStyle is not null)
                target.LinksStyle = ToApi(fonts);

            if (fonts.ReferenceTextSize is float referenceTextSize)
                target.ReferenceTextSize = referenceTextSize;

            if (fonts.Subtitle is not null)
                Apply(fonts.Subtitle, target.Subtitle = new Subtitle());

            if (fonts.Title is not null)
                Apply(fonts.Title, target.Title = new Title());
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="font"></param>
        /// <param name="target"></param>
        void Apply(BrandingThemeFont font, BrandingThemeFontsBase target)
        {
            if (font.Bold is bool bold)
                target.Bold = bold;

            if (font.Size is float size)
                target.Size = size;
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void Apply(Core.Models.BrandingTheme.BrandingThemeWidgets source, global::Auth0.ManagementApi.Models.BrandingThemeWidget target)
        {
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
        void Apply(Core.Models.BrandingTheme.BrandingThemePageBackground source, global::Auth0.ManagementApi.Models.BrandingThemePageBackground target)
        {
            if (source.BackgroundColor is string backgroundColor)
                target.BackgroundColor = backgroundColor;
            if (source.PageLayout is Core.Models.BrandingTheme.PageLayout pageLayout)
                target.PageLayout = ToApi(pageLayout);
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating theme in Auth0 with name: {ThemeName}", EntityTypeName, conf.DisplayName);
            var req = new BrandingThemeCreateRequest();
            Apply(conf, req);
            var self = await api.Branding.CreateBrandingThemeAsync(req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created theme in Auth0 with ID: {ThemeId} and name: {ThemeName}", EntityTypeName, self.ThemeId, conf.DisplayName);
            return self.ThemeId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, BrandingThemeConf? last, BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);
            var req = new BrandingThemeUpdateRequest();

            // these settings need to be filled in
            if (last is not null)
                Apply(last, req);

            Apply(conf, req);
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
