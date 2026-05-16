using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Tenants;

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
        internal static V2alpha1TenantPrompts? FromApi(Prompt? source) => source is null ? null : new()
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
        internal static V2alpha1TenantUniversalLoginExperience? FromApi(string? source) => source switch
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
        internal static V2alpha1TenantBranding? FromApi(GetBrandingResponseContent? source) => source is null ? null : new()
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
        internal static V2alpha1TenantBrandingColors? FromApi(BrandingColors? source) => source is null ? null : new()
        {
            Primary = source.Primary,
            PageBackground = FromApi(source.PageBackground),
        };

        /// <summary>
        /// Creates a new instance of the internal TenantSettings model from an Auth0 API TenantSettings object,
        /// delegating nested objects to their respective <c>FromApi</c> overloads.
        /// </summary>
        /// <param name="source">The Auth0 API tenant settings to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantSettings"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantSettings? FromApi(GetTenantSettingsResponseContent? source) => source is null ? null : new()
        {
            FriendlyName = source.FriendlyName,
            Flags = FromApi(source.Flags),
            AcrValuesSupported = source.AcrValuesSupported.IsDefined ? source.AcrValuesSupported.Value?.ToArray() : null,
            AllowedLogoutUrls = source.AllowedLogoutUrls?.ToArray(),
            ChangePassword = source.ChangePassword.IsDefined ? FromApi(source.ChangePassword.Value) : null,
            CustomizeMfaInPostLoginAction = source.CustomizeMfaInPostloginAction,
            DefaultAudience = source.DefaultAudience,
            DefaultDirectory = source.DefaultDirectory,
            DeviceFlow = source.DeviceFlow.IsDefined ? FromApi(source.DeviceFlow.Value) : null,
            EnabledLocales = source.EnabledLocales?.Select(i => i.Value).ToList(),
            ErrorPage = source.ErrorPage.IsDefined ? FromApi(source.ErrorPage) : null,
            GuardianMfaPage = source.GuardianMfaPage.IsDefined ? FromApi(source.GuardianMfaPage) : null,
            IdleSessionLifetime = source.IdleSessionLifetime,
            PictureUrl = source.PictureUrl,
            SessionLifetime = source.SessionLifetime,
            SessionCookie = source.SessionCookie.IsDefined ? FromApi(source.SessionCookie.Value) : null,
            SupportEmail = source.SupportEmail,
            SupportUrl = source.SupportUrl,
            SandboxVersion = source.SandboxVersion,
            SandboxVersionsAvailable = source.SandboxVersionsAvailable?.ToArray(),
            PushedAuthorizationRequestsSupported = source.PushedAuthorizationRequestsSupported,
            Mtls = FromApi(source.Mtls),
        };

        /// <summary>
        /// Creates a new instance of the internal TenantMtls model from an Auth0 Management API TenantMtls object.
        /// </summary>
        /// <param name="source">The Auth0 Management API TenantMtls object to convert.</param>
        /// <returns>A new TenantMtls instance populated with values from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantMtls? FromApi(TenantSettingsMtls? source) => source is null ? null : new()
        {
            EnableEndpointAliases = source.EnableEndpointAliases,
        };

        /// <summary>
        /// Creates a new SessionCookie instance from an Auth0 Management API session cookie model.
        /// </summary>
        /// <param name="source">The Auth0 Management API session cookie to convert.</param>
        /// <returns>A SessionCookie instance populated with values from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantSessionCookie? FromApi(SessionCookieSchema? source) => source is null ? null : new()
        {
            Mode = source.Mode.Value,
        };

        /// <summary>
        /// Converts an Auth0 API TenantGuardianMfaPage to an internal <see cref="Core.Models.Tenant.V1.V1TenantGuardianMfaPage"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API Guardian MFA page configuration to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantGuardianMfaPage"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantGuardianMfaPage? FromApi(TenantSettingsGuardianPage? source) => source is null ? null : new()
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
        internal static V2alpha1TenantErrorPage? FromApi(TenantSettingsErrorPage? source) => source is null ? null : new()
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
        internal static V2alpha1TenantDeviceFlow? FromApi(TenantSettingsDeviceFlow? source) => source is null ? null : new()
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
        internal static V2alpha1TenantCharset? FromApi(TenantSettingsDeviceFlowCharset? source)
        {
            return source?.Value switch
            {
                TenantSettingsDeviceFlowCharset.Values.Base20 => V2alpha1TenantCharset.Base20,
                TenantSettingsDeviceFlowCharset.Values.Digits => V2alpha1TenantCharset.Digits,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Converts an Auth0 API TenantChangePassword to an internal <see cref="V2alpha1TenantChangePassword"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API change password page configuration to convert.</param>
        /// <returns>A new <see cref="V2alpha1vTenantChangePassword"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantChangePassword? FromApi(TenantSettingsPasswordPage? source) => source is null ? null : new()
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
        internal static V2alpha1TenantFlags? FromApi(TenantSettingsFlags? source) => source is null ? null : new()
        {
            AllowLegacyDelegationGrantTypes = source.AllowLegacyDelegationGrantTypes,
            AllowLegacyRoGrantTypes = source.AllowLegacyRoGrantTypes,
            AllowLegacyTokeninfoEndpoint = source.AllowLegacyTokeninfoEndpoint,
            ChangePwdFlowV1 = source.ChangePwdFlowV1,
            DisableClickjackProtectionHeaders = source.DisableClickjackProtectionHeaders,
            DisableManagementApiSmsObfuscation = source.DisableManagementApiSmsObfuscation,
            EnableAdfsWaadEmailVerification = source.EnableAdfsWaadEmailVerification,
            EnableAPIsSection = source.EnableApisSection,
            EnableClientConnections = source.EnableClientConnections,
            EnableCustomDomainInEmails = source.EnableCustomDomainInEmails,
            EnableDynamicClientRegistration = source.EnableDynamicClientRegistration,
            EnableIdTokenApi2 = source.EnableIdtokenApi2,
            EnableLegacyProfile = source.EnableLegacyProfile,
            EnablePipeline2 = source.EnablePipeline2,
            EnablePublicSignupUserExistsError = source.EnablePublicSignupUserExistsError,
            EnableSSO = source.EnableSso,
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

        internal static void ApplyToApi(V2alpha1TenantSettings source, UpdateTenantSettingsRequestContent target)
        {
            if (source.AcrValuesSupported is { } acr_values_supported)
                target.AcrValuesSupported = acr_values_supported;

            if (source.AllowedLogoutUrls is { } allowed_logout_urls)
                target.AllowedLogoutUrls = allowed_logout_urls;

            if (source.ChangePassword is { } change_password)
            {
                var v = new TenantSettingsPasswordPage();
                ApplyToApi(change_password, v);
                target.ChangePassword = v;
            }

            if (source.CustomizeMfaInPostLoginAction is { } customize_mfa_in_postlogin_action)
                target.CustomizeMfaInPostloginAction = customize_mfa_in_postlogin_action;

            if (source.DefaultAudience is { } default_audience)
                target.DefaultAudience = default_audience;

            if (source.DefaultDirectory is { } default_directory)
                target.DefaultDirectory = default_directory;

            if (source.DeviceFlow is { } device_flow)
            {
                var v = new TenantSettingsDeviceFlow();
                ApplyToApi(device_flow, v);
                target.DeviceFlow = v;
            }

            if (source.EnabledLocales is { } enabled_locales)
                target.EnabledLocales = enabled_locales.Select(i => TenantSettingsSupportedLocalesEnum.FromCustom(i)).ToArray();

            if (source.ErrorPage is { } error_page)
            {
                var v = new TenantSettingsErrorPage();
                ApplyToApi(error_page, v);
                target.ErrorPage = v;
            }

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
            {
                if (session_cookie.Mode is { } mode)
                {
                    target.SessionCookie = new SessionCookieSchema() { Mode = SessionCookieModeEnum.FromCustom(mode) };
                }
            }

            if (source.SessionLifetime is { } session_lifetime)
                target.SessionLifetime = session_lifetime;

            if (source.SupportEmail is { } support_email)
                target.SupportEmail = support_email;

            if (source.SupportUrl is { } support_url)
                target.SupportUrl = support_url;
        }

        internal static void ApplyToApi(V2alpha1TenantChangePassword source, TenantSettingsPasswordPage target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Html is { } html)
                target.Html = html;
        }

        internal static void ApplyToApi(V2alpha1TenantDeviceFlow source, TenantSettingsDeviceFlow target)
        {
            if (source.Charset is { } charset)
                target.Charset = ToApi(charset);

            if (source.Mask is { } mask)
                target.Mask = mask;
        }

        internal static TenantSettingsDeviceFlowCharset ToApi(V2alpha1TenantCharset charset) => charset switch
        {
            V2alpha1TenantCharset.Base20 => TenantSettingsDeviceFlowCharset.Base20,
            V2alpha1TenantCharset.Digits => TenantSettingsDeviceFlowCharset.Digits,
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha1TenantErrorPage source, TenantSettingsErrorPage target)
        {
            if (source.ShowLogLink is { } show_log_link)
                target.ShowLogLink = show_log_link;

            if (source.Url is { } url)
                target.Url = url;

            if (source.Html is { } html)
                target.Html = html;
        }

        internal static void ApplyToApi(V2alpha1TenantFlags source, TenantSettingsFlags target)
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
                target.EnableApisSection = enable_apis_section;

            if (source.EnableClientConnections is { } enable_client_connections)
                target.EnableClientConnections = enable_client_connections;

            if (source.EnableCustomDomainInEmails is { } enable_custom_domain_in_emails)
                target.EnableCustomDomainInEmails = enable_custom_domain_in_emails;

            if (source.EnableDynamicClientRegistration is { } enable_dynamic_client_registration)
                target.EnableDynamicClientRegistration = enable_dynamic_client_registration;

            if (source.EnableIdTokenApi2 is { } enable_id_token_api2)
                target.EnableIdtokenApi2 = enable_id_token_api2;

            if (source.EnableLegacyProfile is { } enable_legacy_profile)
                target.EnableLegacyProfile = enable_legacy_profile;

            if (source.EnablePipeline2 is { } enable_pipeline2)
                target.EnablePipeline2 = enable_pipeline2;

            if (source.EnablePublicSignupUserExistsError is { } enable_public_signup_user_exists_error)
                target.EnablePublicSignupUserExistsError = enable_public_signup_user_exists_error;

            if (source.EnableSSO is { } enable_sso)
                target.EnableSso = enable_sso;

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

        internal static void ApplyToApi(V2alpha1TenantGuardianMfaPage source, TenantGuardianMfaPage target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Html is { } html)
                target.Html = html;
        }

        internal static void ApplyToApi(V2alpha1TenantMtls source, TenantSettingsMtls target)
        {
            if (source.EnableEndpointAliases is { } enable_endpoint_aliases)
                target.EnableEndpointAliases = enable_endpoint_aliases;
        }

        internal static void ApplyToApi(V2alpha1TenantSessionCookie source, SessionCookieSchema target)
        {
            if (source.Mode is { } mode)
                target.Mode = SessionCookieModeEnum.FromCustom(mode);
        }

        internal static void ApplyToApi(V2alpha1TenantPrompts source, PromptUpdateRequest target)
        {
            if (source.IdentifierFirst is { } identifier_first)
                target.IdentifierFirst = identifier_first;

            if (source.UniversalLoginExperience is { } universal_login_experience)
                target.UniversalLoginExperience = ToApi(universal_login_experience);
        }

        internal static string ToApi(V2alpha1TenantUniversalLoginExperience source) => source switch
        {
            V2alpha1TenantUniversalLoginExperience.New => "new",
            V2alpha1TenantUniversalLoginExperience.Classic => "classic",
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha1TenantBranding source, UpdateBrandingRequestContent target)
        {
            if (source.LogoUrl is { } logo_url)
                target.LogoUrl = logo_url;

            if (source.FaviconUrl is { } favicon_url)
                target.FaviconUrl = favicon_url;

            if (source.Colors is { } colors)
            {
                var v = new UpdateBrandingColors();
                ApplyToApi(colors, v);
                target.Colors = v;
            }
        }

        internal static void ApplyToApi(V2alpha1TenantBrandingColors source, UpdateBrandingColors target)
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

            var settings = await api.Tenants.Settings.GetAsync(new GetTenantSettingsRequestParameters() { }, cancellationToken: cancellationToken);
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
                    var req = new UpdateTenantSettingsRequestContent();
                    ApplyToApi(newSettings, req);
                    req.Flags.EnableSSO = null; // this can never be passed
                    var res = await api.TenantSettings.UpdateAsync(req, cancellationToken);
                    var settings = await api.Tenants.Settings.GetAsync(new GetTenantSettingsRequestParameters() { }, cancellationToken: cancellationToken);
                }

                // branding may not be specified
                if (conf.Branding is { } newBranding)
                {
                    // push update to Auth0
                    var req = new UpdateBrandingRequestContent();
                    ApplyToApi(newBranding, req);
                    var res = await api.Branding.UpdateAsync(req, cancellationToken: cancellationToken);
                    branding = await api.Branding.GetAsync(cancellationToken: cancellationToken);
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
