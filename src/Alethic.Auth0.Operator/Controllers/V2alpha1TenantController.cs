using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Models.Prompts;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha1TenantController :
        ControllerBase<V2alpha1Tenant, V2alpha1Tenant.SpecDef, V2alpha1Tenant.StatusDef, V2alpha1TenantConf, V2alpha1TenantConf>,
        IEntityController<V2alpha1Tenant>
    {

        /// <summary>
        /// Converts an Auth0 API <see cref="Prompt"/> to an internal <see cref="V1TenantPrompts"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API prompt configuration to convert.</param>
        /// <returns>A new <see cref="V1TenantPrompts"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantPrompts? FromApi(Prompt? source) => source is null ? null : new()
        {
            IdentifierFirst = source.IdentifierFirst,
            UniversalLoginExperience = FromApi(source.UniversalLoginExperience),
            WebauthnPlatformFirstFactor = source.WebAuthnPlatformFirstFactor,
        };

        /// <summary>
        /// Converts a universal login experience string to the corresponding <see cref="V1TenantUniversalLoginExperience"/> enum value.
        /// </summary>
        /// <param name="source">The Auth0 API universal login experience string (e.g. "new" or "classic").</param>
        /// <returns>The matching <see cref="V1TenantUniversalLoginExperience"/> value.</returns>
        /// <exception cref="NotImplementedException">Thrown when the value is not a recognized experience string.</exception>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantUniversalLoginExperience? FromApi(string? source) => source switch
        {
            "new" => V2alpha1TenantUniversalLoginExperience.New,
            "classic" => V2alpha1TenantUniversalLoginExperience.Classic,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Converts an Auth0 API <see cref="Branding"/> object to an internal <see cref="V1TenantBranding"/> model, including nested colors.
        /// </summary>
        /// <param name="source">The Auth0 API branding configuration to convert.</param>
        /// <returns>A new <see cref="V1TenantBranding"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantBranding? FromApi(Branding? source) => source is null ? null : new()
        {
            LogoUrl = source.LogoUrl, 
            FaviconUrl = source.FaviconUrl,
            Colors = FromApi(source.Colors),
        };

        /// <summary>
        /// Creates a new instance of the internal BrandingColors model from an Auth0 API BrandingColors object.
        /// </summary>
        /// <param name="source">The Auth0 API branding colors to convert.</param>
        /// <returns>A new BrandingColors instance with properties mapped from the specified Auth0 branding colors.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantBrandingColors? FromApi(BrandingColors? source) => source is null ? null : new()
        {
            Primary = source.Primary,
            PageBackground = source.PageBackground,
        };

        /// <summary>
        /// Creates a new instance of the internal TenantSettings model from an Auth0 API TenantSettings object,
        /// delegating nested objects to their respective <c>FromApi</c> overloads.
        /// </summary>
        /// <param name="source">The Auth0 API tenant settings to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantSettings"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantSettings? FromApi(TenantSettings? source) => source is null ? null : new()
        {
            FriendlyName = source.FriendlyName,
            Flags = FromApi(source.Flags),
            AcrValuesSupported = source.AcrValuesSupported,
            AllowedLogoutUrls = source.AllowedLogoutUrls,
            ChangePassword = FromApi(source.ChangePassword),
            CustomizeMfaInPostLoginAction = source.CustomizeMfaInPostLoginAction,
            DefaultAudience = source.DefaultAudience,
            DefaultDirectory = source.DefaultDirectory,
            DeviceFlow = FromApi(source.DeviceFlow),
            EnabledLocales = source.EnabledLocales.ToList(),
            ErrorPage = FromApi(source.ErrorPage),
            GuardianMfaPage = FromApi(source.GuardianMfaPage),
            IdleSessionLifetime = source.IdleSessionLifetime,
            PictureUrl = source.PictureUrl,
            SessionLifetime = source.SessionLifetime,
            SessionCookie = FromApi(source.SessionCookie),
            SupportEmail = source.SupportEmail,
            SupportUrl = source.SupportUrl,
            SandboxVersion = source.SandboxVersion,
            SandboxVersionsAvailable = source.SandboxVersionsAvailable,
            PushedAuthorizationRequestsSupported = source.PushedAuthorizationRequestsSupported,
            Mtls = FromApi(source.Mtls),
        };

        /// <summary>
        /// Creates a new instance of the internal TenantMtls model from an Auth0 Management API TenantMtls object.
        /// </summary>
        /// <param name="source">The Auth0 Management API TenantMtls object to convert.</param>
        /// <returns>A new TenantMtls instance populated with values from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantMtls? FromApi(TenantMtls? source) => source is null ? null : new()
        {
            EnableEndpointAliases = source.EnableEndpointAliases,
        };

        /// <summary>
        /// Creates a new SessionCookie instance from an Auth0 Management API session cookie model.
        /// </summary>
        /// <param name="source">The Auth0 Management API session cookie to convert.</param>
        /// <returns>A SessionCookie instance populated with values from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantSessionCookie? FromApi(SessionCookie? source) => source is null ? null: new()
        {
            Mode = source.Mode,
        };

        /// <summary>
        /// Converts an Auth0 API TenantGuardianMfaPage to an internal <see cref="Core.Models.Tenant.V1.V1TenantGuardianMfaPage"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API Guardian MFA page configuration to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantGuardianMfaPage"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantGuardianMfaPage? FromApi(TenantGuardianMfaPage? source) => source is null ? null : new()
        {
            Html = source.Html,
            Enabled = source.Enabled,
        };

        /// <summary>
        /// Converts an Auth0 API TenantErrorPage to an internal <see cref="Core.Models.Tenant.V1.V1TenantErrorPage"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API error page configuration to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantErrorPage"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantErrorPage? FromApi(TenantErrorPage? source) => source is null ? null : new()
        {
            ShowLogLink = source.ShowLogLink,
            Url = source.Url,
            Html = source.Html,
        };

        /// <summary>
        /// Converts an Auth0 API TenantDeviceFlow to an internal <see cref="Core.Models.Tenant.V2alpha1.V2alpha1TenantDeviceFlow"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API device flow configuration to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V2alpha1.V2alpha1TenantDeviceFlow"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantDeviceFlow? FromApi(TenantDeviceFlow? source) => source is null ? null : new()
        {
            Charset = FromApi(source.Charset),
            Mask = source.Mask,
        };

        /// <summary>
        /// Converts an Auth0 API <see cref="TenantDeviceFlowCharset"/> enum to the internal <see cref="V1TenantCharset"/> enum.
        /// </summary>
        /// <param name="source">The Auth0 API device flow charset value to convert.</param>
        /// <returns>The matching <see cref="V1TenantCharset"/> value.</returns>
        /// <exception cref="NotImplementedException">Thrown when the charset value is not recognized.</exception>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantCharset? FromApi(TenantDeviceFlowCharset? source) => source switch
        {
            TenantDeviceFlowCharset.Base20 => V2alpha1TenantCharset.Base20,
            TenantDeviceFlowCharset.Digits => V2alpha1TenantCharset.Digits,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Converts an Auth0 API TenantChangePassword to an internal <see cref="V2alpha1TenantChangePassword"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API change password page configuration to convert.</param>
        /// <returns>A new <see cref="V2alpha1vTenantChangePassword"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantChangePassword? FromApi(TenantChangePassword? source) => source is null ? null : new()
        {
            Enabled = source.Enabled,
            Html = source.Html,
        };

        /// <summary>
        /// Converts all Auth0 API tenant feature flags to an internal <see cref="V2alpha1TenantFlags"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API tenant feature flags to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantFlags"/> instance with all flag values mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        static V2alpha1TenantFlags? FromApi(TenantFlags? source) => source is null ? null : new()
        {
            AllowLegacyDelegationGrantTypes = source.AllowLegacyDelegationGrantTypes,
            AllowLegacyRoGrantTypes = source.AllowLegacyRoGrantTypes,
            AllowLegacyTokeninfoEndpoint = source.AllowLegacyTokeninfoEndpoint,
            ChangePwdFlowV1 = source.ChangePwdFlowV1,
            DisableClickjackProtectionHeaders = source.DisableClickjackProtectionHeaders,
            DisableManagementApiSmsObfuscation = source.DisableManagementApiSmsObfuscation,
            EnableAdfsWaadEmailVerification = source.EnableAdfsWaadEmailVerification,
            EnableAPIsSection = source.EnableAPIsSection,
            EnableClientConnections = source.EnableClientConnections,
            EnableCustomDomainInEmails = source.EnableCustomDomainInEmails,
            EnableDynamicClientRegistration = source.EnableDynamicClientRegistration,
            EnableIdTokenApi2 = source.EnableIdTokenApi2,
            EnableLegacyProfile = source.EnableLegacyProfile,
            EnablePipeline2 = source.EnablePipeline2,
            EnablePublicSignupUserExistsError = source.EnablePublicSignupUserExistsError,
            EnableSSO = source.EnableSSO,
            EnforceClientAuthenticationOnPasswordlessStart = source.EnforceClientAuthenticationOnPasswordlessStart,
            NoDiscloseEnterpriseConnections = source.NoDiscloseEnterpriseConnections,
            RemoveAlgFromJwks = source.RemoveAlgFromJwks,
            RequirePushedAuthorizationRequests = source.RequirePushedAuthorizationRequests,
            RevokeRefreshTokenGrant = source.RevokeRefreshTokenGrant,
            TrustAzureAdfsEmailVerifiedConnectionProperty = source.TrustAzureAdfsEmailVerifiedConnectionProperty,
            DashboardInsightsView = source.DashboardInsightsView,
            DashboardLogStreamsNext = source.DashboardLogStreamsNext,
            DisableFieldsMapFix = source.DisableFieldsMapFix,
            MfaShowFactorListOnEnrollment = source.MfaShowFactorListOnEnrollment,
            ImprovedSignupBotDetectionInClassic = source.ImprovedSignupBotDetectionInClassic,
            GenaiTrial = source.GenaiTrial,
            CustomDomainsProvisioning = source.CustomDomainsProvisioning,
        };

        static void ApplyToApi(V2alpha1TenantSettings source, TenantSettingsUpdateRequest target)
        {
            if (source.AcrValuesSupported is { } acr_values_supported)
                target.AcrValuesSupported = acr_values_supported;

            if (source.AllowedLogoutUrls is { } allowed_logout_urls)
                target.AllowedLogoutUrls = allowed_logout_urls;

            if (source.ChangePassword is { } change_password)
                ApplyToApi(change_password, target.ChangePassword = new());

            if (source.CustomizeMfaInPostLoginAction is { } customize_mfa_in_postlogin_action)
                target.CustomizeMfaInPostLoginAction = customize_mfa_in_postlogin_action;

            if (source.DefaultAudience is { } default_audience)
                target.DefaultAudience = default_audience;

            if (source.DefaultDirectory is { } default_directory)
                target.DefaultDirectory = default_directory;

            if (source.DeviceFlow is { } device_flow)
                ApplyToApi(device_flow, target.DeviceFlow = new());

            if (source.EnabledLocales is { } enabled_locales)
                target.EnabledLocales = enabled_locales.ToArray();

            if (source.ErrorPage is { } error_page)
                ApplyToApi(error_page, target.ErrorPage = new());

            if (source.Flags is { } flags)
                ApplyToApi(flags, target.Flags = new());

            if (source.FriendlyName is { } friendly_name)
                target.FriendlyName = friendly_name;

            if (source.GuardianMfaPage is { } guardian_mfa_page)
                ApplyToApi(guardian_mfa_page, target.GuardianMfaPage = new());

            if (source.IdleSessionLifetime is { } idle_session_lifetime)
                target.IdleSessionLifetime = idle_session_lifetime;

            if (source.Mtls is { } mtls)
                ApplyToApi(mtls, target.Mtls = new());

            if (source.PictureUrl is { } picture_url)
                target.PictureUrl = picture_url;

            if (source.PushedAuthorizationRequestsSupported is { } pushed_authorization_requests_supported)
                target.PushedAuthorizationRequestsSupported = pushed_authorization_requests_supported;

            if (source.SandboxVersion is { } sandbox_version)
                target.SandboxVersion = sandbox_version;

            if (source.SandboxVersionsAvailable is { } sandbox_versions_available)
                target.SandboxVersionsAvailable = sandbox_versions_available;

            if (source.SessionCookie is { } session_cookie)
                ApplyToApi(session_cookie, target.SessionCookie = new());

            if (source.SessionLifetime is { } session_lifetime)
                target.SessionLifetime = session_lifetime;

            if (source.SupportEmail is { } support_email)
                target.SupportEmail = support_email;

            if (source.SupportUrl is { } support_url)
                target.SupportUrl = support_url;
        }

        static void ApplyToApi(V2alpha1TenantChangePassword source, TenantChangePassword target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Html is { } html)
                target.Html = html;
        }

        static void ApplyToApi(V2alpha1TenantDeviceFlow source, TenantDeviceFlow target)
        {
            if (source.Charset is { } charset)
                target.Charset = ToApi(charset);

            if (source.Mask is { } mask)
                target.Mask = mask;
        }

        static TenantDeviceFlowCharset ToApi(V2alpha1TenantCharset charset) => charset switch
        {
            V2alpha1TenantCharset.Base20 => TenantDeviceFlowCharset.Base20,
            V2alpha1TenantCharset.Digits => TenantDeviceFlowCharset.Digits,
            _ => throw new NotImplementedException(),
        };

        static void ApplyToApi(V2alpha1TenantErrorPage source, TenantErrorPage target)
        {
            if (source.ShowLogLink is { } show_log_link)
                target.ShowLogLink = show_log_link;

            if (source.Url is { } url)
                target.Url = url;

            if (source.Html is { } html)
                target.Html = html;
        }

        static void ApplyToApi(V2alpha1TenantFlags source, TenantFlags target)
        {
            if (source.AllowLegacyDelegationGrantTypes is { } allow_legacy_delegation_grant_types)
                target.AllowLegacyDelegationGrantTypes = allow_legacy_delegation_grant_types;

            if (source.AllowLegacyRoGrantTypes is { } allow_legacy_ro_grant_types)
                target.AllowLegacyRoGrantTypes = allow_legacy_ro_grant_types;

            if (source.AllowLegacyTokeninfoEndpoint is { } allow_legacy_tokeninfo_endpoint)
                target.AllowLegacyTokeninfoEndpoint = allow_legacy_tokeninfo_endpoint;

            if (source.ChangePwdFlowV1 is { } change_pwd_flow_v1)
                target.ChangePwdFlowV1 = change_pwd_flow_v1;

            if (source.DisableClickjackProtectionHeaders is { } disable_clickjack_protection_headers)
                target.DisableClickjackProtectionHeaders = disable_clickjack_protection_headers;

            if (source.DisableManagementApiSmsObfuscation is { } disable_management_api_sms_obfuscation)
                target.DisableManagementApiSmsObfuscation = disable_management_api_sms_obfuscation;

            if (source.EnableAdfsWaadEmailVerification is { } enable_adfs_waad_email_verification)
                target.EnableAdfsWaadEmailVerification = enable_adfs_waad_email_verification;

            if (source.EnableAPIsSection is { } enable_apis_section)
                target.EnableAPIsSection = enable_apis_section;

            if (source.EnableClientConnections is { } enable_client_connections)
                target.EnableClientConnections = enable_client_connections;

            if (source.EnableCustomDomainInEmails is { } enable_custom_domain_in_emails)
                target.EnableCustomDomainInEmails = enable_custom_domain_in_emails;

            if (source.EnableDynamicClientRegistration is { } enable_dynamic_client_registration)
                target.EnableDynamicClientRegistration = enable_dynamic_client_registration;

            if (source.EnableIdTokenApi2 is { } enable_id_token_api2)
                target.EnableIdTokenApi2 = enable_id_token_api2;

            if (source.EnableLegacyProfile is { } enable_legacy_profile)
                target.EnableLegacyProfile = enable_legacy_profile;

            if (source.EnablePipeline2 is { } enable_pipeline2)
                target.EnablePipeline2 = enable_pipeline2;

            if (source.EnablePublicSignupUserExistsError is { } enable_public_signup_user_exists_error)
                target.EnablePublicSignupUserExistsError = enable_public_signup_user_exists_error;

            if (source.EnableSSO is { } enable_sso)
                target.EnableSSO = enable_sso;

            if (source.EnforceClientAuthenticationOnPasswordlessStart is { } enforce_client_authentication_on_passwordless_start)
                target.EnforceClientAuthenticationOnPasswordlessStart = enforce_client_authentication_on_passwordless_start;

            if (source.NoDiscloseEnterpriseConnections is { } no_disclose_enterprise_connections)
                target.NoDiscloseEnterpriseConnections = no_disclose_enterprise_connections;

            if (source.RemoveAlgFromJwks is { } remove_alg_from_jwks)
                target.RemoveAlgFromJwks = remove_alg_from_jwks;

            if (source.RequirePushedAuthorizationRequests is { } require_pushed_authorization_requests)
                target.RequirePushedAuthorizationRequests = require_pushed_authorization_requests;

            if (source.RevokeRefreshTokenGrant is { } revoke_refresh_token_grant)
                target.RevokeRefreshTokenGrant = revoke_refresh_token_grant;

            if (source.TrustAzureAdfsEmailVerifiedConnectionProperty is { } trust_azure_adfs_email_verified_connection_property)
                target.TrustAzureAdfsEmailVerifiedConnectionProperty = trust_azure_adfs_email_verified_connection_property;

            if (source.DashboardInsightsView is { } dashboard_insights_view)
                target.DashboardInsightsView = dashboard_insights_view;

            if (source.DashboardLogStreamsNext is { } dashboard_log_streams_next)
                target.DashboardLogStreamsNext = dashboard_log_streams_next;

            if (source.DisableFieldsMapFix is { } disable_fields_map_fix)
                target.DisableFieldsMapFix = disable_fields_map_fix;

            if (source.MfaShowFactorListOnEnrollment is { } mfa_show_factor_list_on_enrollment)
                target.MfaShowFactorListOnEnrollment = mfa_show_factor_list_on_enrollment;

            if (source.ImprovedSignupBotDetectionInClassic is { } improved_signup_bot_detection_in_classic)
                target.ImprovedSignupBotDetectionInClassic = improved_signup_bot_detection_in_classic;

            if (source.GenaiTrial is { } genai_trial)
                target.GenaiTrial = genai_trial;

            if (source.CustomDomainsProvisioning is { } custom_domains_provisioning)
                target.CustomDomainsProvisioning = custom_domains_provisioning;
        }

        static void ApplyToApi(V2alpha1TenantGuardianMfaPage source, TenantGuardianMfaPage target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Html is { } html)
                target.Html = html;
        }

        static void ApplyToApi(V2alpha1TenantMtls source, TenantMtls target)
        {
            if (source.EnableEndpointAliases is { } enable_endpoint_aliases)
                target.EnableEndpointAliases = enable_endpoint_aliases;
        }

        static void ApplyToApi(V2alpha1TenantSessionCookie source, SessionCookie target)
        {
            if (source.Mode is { } mode)
                target.Mode = mode;
        }

        static void ApplyToApi(V2alpha1TenantPrompts source, PromptUpdateRequest target)
        {
            if (source.IdentifierFirst is { } identifier_first)
                target.IdentifierFirst = identifier_first;

            if (source.UniversalLoginExperience is { } universal_login_experience)
                target.UniversalLoginExperience = ToApi(universal_login_experience);
        }

        static string ToApi(V2alpha1TenantUniversalLoginExperience source) => source switch
        {
            V2alpha1TenantUniversalLoginExperience.New => "new",
            V2alpha1TenantUniversalLoginExperience.Classic => "classic",
            _ => throw new NotImplementedException(),
        };

        static void ApplyToApi(V2alpha1TenantBranding source, BrandingUpdateRequest target)
        {
            if (source.LogoUrl is { } logo_url)
                target.LogoUrl = logo_url;

            if (source.FaviconUrl is { } favicon_url)
                target.FaviconUrl = favicon_url;

            if (source.Colors is { } colors)
                ApplyToApi(colors, target.Colors = new());
        }

        static void ApplyToApi(V2alpha1TenantBrandingColors source, BrandingColors target)
        {
            if (source.Primary is { } primary)
                target.Primary = primary;

            if (source.PageBackground is { } page_background)
                target.PageBackground = page_background;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha1TenantController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V2alpha1TenantController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Tenant";

        /// <inheritdoc />
        protected override async Task Reconcile(V2alpha1Tenant entity, CancellationToken cancellationToken)
        {
            var api = await GetTenantApiClientAsync(entity, cancellationToken);
            if (api == null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}:{entity.Name()} failed to retrieve API client.");

            var settings = await api.TenantSettings.GetAsync(cancellationToken: cancellationToken);
            if (settings is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} settings cannot be loaded from API.");

            var branding = await api.Branding.GetAsync(cancellationToken: cancellationToken);
            if (branding is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} branding cannot be loaded from API.");

            var prompts = await api.Prompts.GetAsync(cancellationToken: cancellationToken);
            if (prompts is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} prompts cannot be loaded from API.");

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                // settings may not be specified
                if (conf.Settings is { } newSettings)
                {
                    // verify that no changes to enable_sso are being made
                    if (newSettings.Flags != null && newSettings.Flags.EnableSSO != null && settings.Flags.EnableSSO != null && newSettings.Flags.EnableSSO != settings.Flags.EnableSSO)
                        throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()}: updating the enable_sso flag is not allowed.");

                    // push update to Auth0
                    var req = new TenantSettingsUpdateRequest();
                    ApplyToApi(newSettings, req);
                    req.Flags.EnableSSO = null; // this can never be passed
                    settings = await api.TenantSettings.UpdateAsync(req, cancellationToken);
                }

                // branding may not be specified
                if (conf.Branding is { } newBranding)
                {
                    // push update to Auth0
                    var req = new BrandingUpdateRequest();
                    ApplyToApi(newBranding, req);
                    branding = await api.Branding.UpdateAsync(req, cancellationToken);
                }

                // prompts may not be specified
                if (conf.Prompts is { } newPrompts)
                {
                    // push update to Auth0
                    var req = new PromptUpdateRequest();
                    ApplyToApi(newPrompts, req);
                    prompts = await api.Prompts.UpdateAsync(req, cancellationToken);
                }
            }

            // retrieve and copy new properties to status
            entity.Status.LastConf ??= new V2alpha1TenantConf();
            entity.Status.LastConf.Settings = FromApi(settings);
            entity.Status.LastConf.Branding = FromApi(branding);
            entity.Status.LastConf.Prompts = FromApi(prompts);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
            await ReconcileSuccessAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(V2alpha1Tenant entity, CancellationToken cancellationToken)
        {
            Logger.LogWarning("Unsupported operation deleting entity {Entity}.", entity);
            return Task.CompletedTask;
        }

    }

}
