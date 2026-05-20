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
        internal static V2alpha1TenantPrompts? FromApi(GetSettingsResponseContent? source) => source is null ? null : new()
        {
            IdentifierFirst = source.IdentifierFirst,
            UniversalLoginExperience = FromApi(source.UniversalLoginExperience),
            WebauthnPlatformFirstFactor = source.WebauthnPlatformFirstFactor,
        };

        /// <summary>
        /// Converts a universal login experience enum to the corresponding <see cref="V2alpha1TenantUniversalLoginExperience"/> enum value.
        /// </summary>
        /// <param name="source">The Auth0 API universal login experience enum value.</param>
        /// <returns>The matching <see cref="V2alpha1TenantUniversalLoginExperience"/> value.</returns>
        /// <exception cref="NotImplementedException">Thrown when the value is not a recognized experience.</exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantUniversalLoginExperience? FromApi(UniversalLoginExperienceEnum? source)
        {
            return source?.Value switch
            {
                UniversalLoginExperienceEnum.Values.New => V2alpha1TenantUniversalLoginExperience.New,
                UniversalLoginExperienceEnum.Values.Classic => V2alpha1TenantUniversalLoginExperience.Classic,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

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
            PageBackground = source.PageBackground?.Value?.ToString(),
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
            AllowOrganizationNameInAuthenticationApi = source.AllowOrganizationNameInAuthenticationApi,
            AuthorizationResponseIssParameterSupported = source.AuthorizationResponseIssParameterSupported.IsDefined ? source.AuthorizationResponseIssParameterSupported.Value : null,
            ChangePassword = source.ChangePassword.IsDefined ? FromApi(source.ChangePassword.Value) : null,
            ClientIdMetadataDocumentSupported = source.ClientIdMetadataDocumentSupported,
            CustomizeMfaInPostloginAction = source.CustomizeMfaInPostloginAction,
            DefaultRedirectionUri = source.DefaultRedirectionUri,
            DefaultTokenQuota = source.DefaultTokenQuota.IsDefined ? FromApi(source.DefaultTokenQuota.Value) : null,
            DefaultAudience = source.DefaultAudience,
            DefaultDirectory = source.DefaultDirectory,
            DynamicClientRegistrationSecurityMode = FromApi(source.DynamicClientRegistrationSecurityMode),
            DeviceFlow = source.DeviceFlow.IsDefined ? FromApi(source.DeviceFlow.Value) : null,
            EnabledLocales = source.EnabledLocales?.Select(i => i.Value).ToArray(),
            EnableAiGuide = source.EnableAiGuide,
            EphemeralSessionLifetime = source.EphemeralSessionLifetime is { } ephemeral_session_lifetime ? (int?)ephemeral_session_lifetime : null,
            ErrorPage = source.ErrorPage.IsDefined ? FromApi(source.ErrorPage.Value) : null,
            GuardianMfaPage = source.GuardianMfaPage.IsDefined ? FromApi(source.GuardianMfaPage.Value) : null,
            IdleEphemeralSessionLifetime = source.IdleEphemeralSessionLifetime is { } idle_ephemeral_session_lifetime ? (int?)idle_ephemeral_session_lifetime : null,
            IdleSessionLifetime = source.IdleSessionLifetime is { } idle_session_lifetime ? (int?)idle_session_lifetime : null,
            LegacySandboxVersion = source.LegacySandboxVersion,
            OidcLogout = FromApi(source.OidcLogout),
            PictureUrl = source.PictureUrl,
            PhoneConsolidatedExperience = source.PhoneConsolidatedExperience,
            ResourceParameterProfile = FromApi(source.ResourceParameterProfile),
            SessionLifetime = source.SessionLifetime is { } session_lifetime ? (int?)session_lifetime : null,
            SessionCookie = source.SessionCookie.IsDefined ? FromApi(source.SessionCookie.Value) : null,
            Sessions = source.Sessions.IsDefined ? FromApi(source.Sessions.Value) : null,
            SkipNonVerifiableCallbackUriConfirmationPrompt = source.SkipNonVerifiableCallbackUriConfirmationPrompt.IsDefined ? source.SkipNonVerifiableCallbackUriConfirmationPrompt.Value : null,
            SupportEmail = source.SupportEmail,
            SupportUrl = source.SupportUrl,
            SandboxVersion = source.SandboxVersion,
            PushedAuthorizationRequestsSupported = source.PushedAuthorizationRequestsSupported,
            Mtls = source.Mtls.IsDefined ? FromApi(source.Mtls.Value) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantDefaultTokenQuota? FromApi(DefaultTokenQuota? source) => source is null ? null : new()
        {
            Clients = FromApi(source.Clients),
            Organizations = FromApi(source.Organizations),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantTokenQuotaConfiguration? FromApi(TokenQuotaConfiguration? source) => source is null ? null : new()
        {
            ClientCredentials = FromApi(source.ClientCredentials),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantTokenQuotaClientCredentials? FromApi(TokenQuotaClientCredentials? source) => source is null ? null : new()
        {
            Enforce = source.Enforce,
            PerDay = source.PerDay,
            PerHour = source.PerHour,
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
            Mode = FromApi(source.Mode),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantSessionCookieModeEnum? FromApi(SessionCookieModeEnum? source)
        {
            return source?.Value switch
            {
                "persistent" => V2alpha1TenantSessionCookieModeEnum.Persistent,
                "non_persistent" => V2alpha1TenantSessionCookieModeEnum.NonPersistent,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantSessions? FromApi(TenantSettingsSessions? source) => source is null ? null : new()
        {
            OidcLogoutPromptEnabled = source.OidcLogoutPromptEnabled,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantOidcLogout? FromApi(TenantOidcLogoutSettings? source) => source is null ? null : new()
        {
            RpLogoutEndSessionEndpointDiscovery = source.RpLogoutEndSessionEndpointDiscovery,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantResourceParameterProfile? FromApi(TenantSettingsResourceParameterProfile? source)
        {
            return source?.Value switch
            {
                "audience" => V2alpha1TenantResourceParameterProfile.Audience,
                "compatibility" => V2alpha1TenantResourceParameterProfile.Compatibility,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1TenantDynamicClientRegistrationSecurityMode? FromApi(TenantSettingsDynamicClientRegistrationSecurityMode? source)
        {
            return source?.Value switch
            {
                "strict" => V2alpha1TenantDynamicClientRegistrationSecurityMode.Strict,
                "permissive" => V2alpha1TenantDynamicClientRegistrationSecurityMode.Permissive,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

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
            EnableApisSection = source.EnableApisSection,
            EnableClientConnections = source.EnableClientConnections,
            EnableDynamicClientRegistration = source.EnableDynamicClientRegistration,
            EnableIdtokenApi2 = source.EnableIdtokenApi2,
            EnableLegacyProfile = source.EnableLegacyProfile,
            EnablePipeline2 = source.EnablePipeline2,
            EnablePublicSignupUserExistsError = source.EnablePublicSignupUserExistsError,
            EnableSso = source.EnableSso,
            EnforceClientAuthenticationOnPasswordlessStart = source.EnforceClientAuthenticationOnPasswordlessStart,
            NoDiscloseEnterpriseConnections = source.NoDiscloseEnterpriseConnections,
            RemoveAlgFromJwks = source.RemoveAlgFromJwks,
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

            if (source.AllowOrganizationNameInAuthenticationApi is { } allow_organization_name_in_authentication_api)
                target.AllowOrganizationNameInAuthenticationApi = allow_organization_name_in_authentication_api;

            if (source.AuthorizationResponseIssParameterSupported is { } authorization_response_iss_parameter_supported)
                target.AuthorizationResponseIssParameterSupported = authorization_response_iss_parameter_supported;

            if (source.ChangePassword is { } change_password)
            {
                var v = new TenantSettingsPasswordPage();
                ApplyToApi(change_password, v);
                target.ChangePassword = v;
            }

            if (source.ClientIdMetadataDocumentSupported is { } client_id_metadata_document_supported)
                target.ClientIdMetadataDocumentSupported = client_id_metadata_document_supported;

            if (source.CustomizeMfaInPostloginAction is { } customize_mfa_in_postlogin_action)
                target.CustomizeMfaInPostloginAction = customize_mfa_in_postlogin_action;

            if (source.DefaultRedirectionUri is { } default_redirection_uri)
                target.DefaultRedirectionUri = default_redirection_uri;

            if (source.DefaultTokenQuota is { } default_token_quota)
            {
                var v = new DefaultTokenQuota();
                ApplyToApi(default_token_quota, v);
                target.DefaultTokenQuota = v;
            }

            if (source.DefaultAudience is { } default_audience)
                target.DefaultAudience = default_audience;

            if (source.DefaultDirectory is { } default_directory)
                target.DefaultDirectory = default_directory;

            if (source.DynamicClientRegistrationSecurityMode is { } dynamic_client_registration_security_mode)
                target.DynamicClientRegistrationSecurityMode = ToApi(dynamic_client_registration_security_mode);

            if (source.DeviceFlow is { } device_flow)
            {
                var v = new TenantSettingsDeviceFlow();
                ApplyToApi(device_flow, v);
                target.DeviceFlow = v;
            }

            if (source.EnabledLocales is { } enabled_locales)
                target.EnabledLocales = enabled_locales.Select(i => TenantSettingsSupportedLocalesEnum.FromCustom(i)).ToArray();

            if (source.EnableAiGuide is { } enable_ai_guide)
                target.EnableAiGuide = enable_ai_guide;

            if (source.EphemeralSessionLifetime is { } ephemeral_session_lifetime)
                target.EphemeralSessionLifetime = ephemeral_session_lifetime;

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
            {
                var v = new TenantSettingsGuardianPage();
                ApplyToApi(guardian_mfa_page, v);
                target.GuardianMfaPage = v;
            }

            if (source.IdleSessionLifetime is { } idle_session_lifetime)
                target.IdleSessionLifetime = (int?)idle_session_lifetime;

            if (source.IdleEphemeralSessionLifetime is { } idle_ephemeral_session_lifetime)
                target.IdleEphemeralSessionLifetime = idle_ephemeral_session_lifetime;

            if (source.LegacySandboxVersion is { } legacy_sandbox_version)
                target.LegacySandboxVersion = legacy_sandbox_version;

            if (source.Mtls is { } mtls)
            {
                var v = new TenantSettingsMtls();
                ApplyToApi(mtls, v);
                target.Mtls = v;
            }

            if (source.OidcLogout is { } oidc_logout)
            {
                var v = new TenantOidcLogoutSettings();
                ApplyToApi(oidc_logout, v);
                target.OidcLogout = v;
            }

            if (source.PictureUrl is { } picture_url)
                target.PictureUrl = picture_url;

            if (source.PhoneConsolidatedExperience is { } phone_consolidated_experience)
                target.PhoneConsolidatedExperience = phone_consolidated_experience;

            if (source.PushedAuthorizationRequestsSupported is { } pushed_authorization_requests_supported)
                target.PushedAuthorizationRequestsSupported = pushed_authorization_requests_supported;

            if (source.ResourceParameterProfile is { } resource_parameter_profile)
                target.ResourceParameterProfile = ToApi(resource_parameter_profile);

            if (source.SandboxVersion is { } sandbox_version)
                target.SandboxVersion = sandbox_version;

            if (source.SessionCookie is { } session_cookie)
            {
                var v = new SessionCookieSchema { Mode = SessionCookieModeEnum.FromCustom("persistent") };
                ApplyToApi(session_cookie, v);
                target.SessionCookie = v;
            }

            if (source.SessionLifetime is { } session_lifetime)
                target.SessionLifetime = (int?)session_lifetime;

            if (source.Sessions is { } sessions)
            {
                var v = new TenantSettingsSessions();
                ApplyToApi(sessions, v);
                target.Sessions = v;
            }

            if (source.SkipNonVerifiableCallbackUriConfirmationPrompt is { } skip_non_verifiable_callback_uri_confirmation_prompt)
                target.SkipNonVerifiableCallbackUriConfirmationPrompt = skip_non_verifiable_callback_uri_confirmation_prompt;

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

            if (source.EnableApisSection is { } enable_apis_section)
                target.EnableApisSection = enable_apis_section;

            if (source.EnableClientConnections is { } enable_client_connections)
                target.EnableClientConnections = enable_client_connections;

            if (source.EnableDynamicClientRegistration is { } enable_dynamic_client_registration)
                target.EnableDynamicClientRegistration = enable_dynamic_client_registration;

            if (source.EnableIdtokenApi2 is { } enable_id_token_api2)
                target.EnableIdtokenApi2 = enable_id_token_api2;

            if (source.EnableLegacyProfile is { } enable_legacy_profile)
                target.EnableLegacyProfile = enable_legacy_profile;

            if (source.EnablePipeline2 is { } enable_pipeline2)
                target.EnablePipeline2 = enable_pipeline2;

            if (source.EnablePublicSignupUserExistsError is { } enable_public_signup_user_exists_error)
                target.EnablePublicSignupUserExistsError = enable_public_signup_user_exists_error;

            if (source.EnableSso is { } enable_sso)
                target.EnableSso = enable_sso;

            if (source.EnforceClientAuthenticationOnPasswordlessStart is { } enforce_client_authentication_on_passwordless_start)
                target.EnforceClientAuthenticationOnPasswordlessStart = enforce_client_authentication_on_passwordless_start;

            if (source.NoDiscloseEnterpriseConnections is { } no_disclose_enterprise_connections)
                target.NoDiscloseEnterpriseConnections = no_disclose_enterprise_connections;

            if (source.RemoveAlgFromJwks is { } remove_alg_from_jwks)
                target.RemoveAlgFromJwks = remove_alg_from_jwks;

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

        internal static void ApplyToApi(V2alpha1TenantGuardianMfaPage source, TenantSettingsGuardianPage target)
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

        internal static void ApplyToApi(V2alpha1TenantDefaultTokenQuota source, DefaultTokenQuota target)
        {
            if (source.Clients?.ClientCredentials is not null)
            {
                var v = new TokenQuotaConfiguration { ClientCredentials = new TokenQuotaClientCredentials() };
                ApplyToApi(source.Clients, v);
                target.Clients = v;
            }

            if (source.Organizations?.ClientCredentials is not null)
            {
                var v = new TokenQuotaConfiguration { ClientCredentials = new TokenQuotaClientCredentials() };
                ApplyToApi(source.Organizations, v);
                target.Organizations = v;
            }
        }

        internal static void ApplyToApi(V2alpha1TenantTokenQuotaConfiguration source, TokenQuotaConfiguration target)
        {
            if (source.ClientCredentials is { } client_credentials)
            {
                var v = new TokenQuotaClientCredentials();
                ApplyToApi(client_credentials, v);
                target.ClientCredentials = v;
            }
        }

        internal static void ApplyToApi(V2alpha1TenantTokenQuotaClientCredentials source, TokenQuotaClientCredentials target)
        {
            if (source.Enforce is { } enforce)
                target.Enforce = enforce;

            if (source.PerDay is { } per_day)
                target.PerDay = per_day;

            if (source.PerHour is { } per_hour)
                target.PerHour = per_hour;
        }

        internal static void ApplyToApi(V2alpha1TenantOidcLogout source, TenantOidcLogoutSettings target)
        {
            if (source.RpLogoutEndSessionEndpointDiscovery is { } rp_logout_end_session_endpoint_discovery)
                target.RpLogoutEndSessionEndpointDiscovery = rp_logout_end_session_endpoint_discovery;
        }

        internal static void ApplyToApi(V2alpha1TenantSessionCookie source, SessionCookieSchema target)
        {
            if (source.Mode is { } mode)
                target.Mode = ToApi(mode);
        }

        internal static SessionCookieModeEnum ToApi(V2alpha1TenantSessionCookieModeEnum source) => source switch
        {
            V2alpha1TenantSessionCookieModeEnum.Persistent => SessionCookieModeEnum.FromCustom("persistent"),
            V2alpha1TenantSessionCookieModeEnum.NonPersistent => SessionCookieModeEnum.FromCustom("non_persistent"),
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha1TenantSessions source, TenantSettingsSessions target)
        {
            if (source.OidcLogoutPromptEnabled is { } oidc_logout_prompt_enabled)
                target.OidcLogoutPromptEnabled = oidc_logout_prompt_enabled;
        }

        internal static void ApplyToApi(V2alpha1TenantPrompts source, UpdateSettingsRequestContent target)
        {
            if (source.IdentifierFirst is { } identifier_first)
                target.IdentifierFirst = identifier_first;

            if (source.UniversalLoginExperience is { } universal_login_experience)
                target.UniversalLoginExperience = ToApi(universal_login_experience);
        }

        internal static UniversalLoginExperienceEnum ToApi(V2alpha1TenantUniversalLoginExperience source) => source switch
        {
            V2alpha1TenantUniversalLoginExperience.New => UniversalLoginExperienceEnum.FromCustom(UniversalLoginExperienceEnum.Values.New),
            V2alpha1TenantUniversalLoginExperience.Classic => UniversalLoginExperienceEnum.FromCustom(UniversalLoginExperienceEnum.Values.Classic),
            _ => throw new NotImplementedException(),
        };

        internal static TenantSettingsResourceParameterProfile ToApi(V2alpha1TenantResourceParameterProfile source) => source switch
        {
            V2alpha1TenantResourceParameterProfile.Audience => TenantSettingsResourceParameterProfile.FromCustom("audience"),
            V2alpha1TenantResourceParameterProfile.Compatibility => TenantSettingsResourceParameterProfile.FromCustom("compatibility"),
            _ => throw new NotImplementedException(),
        };

        internal static TenantSettingsDynamicClientRegistrationSecurityMode ToApi(V2alpha1TenantDynamicClientRegistrationSecurityMode source) => source switch
        {
            V2alpha1TenantDynamicClientRegistrationSecurityMode.Strict => TenantSettingsDynamicClientRegistrationSecurityMode.FromCustom("strict"),
            V2alpha1TenantDynamicClientRegistrationSecurityMode.Permissive => TenantSettingsDynamicClientRegistrationSecurityMode.FromCustom("permissive"),
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
                target.PageBackground = UpdateBrandingPageBackground.FromString(page_background);
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

            var prompts = await api.Prompts.GetSettingsAsync(cancellationToken: cancellationToken);
            if (prompts is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} prompts cannot be loaded from API.");

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                // settings may not be specified
                if (conf.Settings is { } newSettings)
                {
                    // verify that no changes to enable_sso are being made
                    if (newSettings.Flags != null && newSettings.Flags.EnableSso != null && settings.Flags.EnableSso != null && newSettings.Flags.EnableSso != settings.Flags.EnableSso)
                        throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()}: updating the enable_sso flag is not allowed.");

                    // push update to Auth0
                    var req = new UpdateTenantSettingsRequestContent();
                    ApplyToApi(newSettings, req);
                    req.Flags.EnableSso = null; // this can never be passed
                    var res = await api.Tenants.Settings.UpdateAsync(req, cancellationToken: cancellationToken);
                    settings = await api.Tenants.Settings.GetAsync(new GetTenantSettingsRequestParameters() { }, cancellationToken: cancellationToken);
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
                    var req = new UpdateSettingsRequestContent();
                    ApplyToApi(newPrompts, req);
                    await api.Prompts.UpdateSettingsAsync(req, cancellationToken: cancellationToken);
                    prompts = await api.Prompts.GetSettingsAsync(cancellationToken: cancellationToken);
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
