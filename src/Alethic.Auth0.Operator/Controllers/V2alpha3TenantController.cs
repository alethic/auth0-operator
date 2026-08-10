using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

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

    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3TenantController :
        ControllerBase<V2alpha3Tenant, V2alpha3Tenant.SpecDef, V2alpha3Tenant.StatusDef, V2alpha3TenantConf, V2alpha3TenantConf>,
        IEntityController<V2alpha3Tenant>
    {

        /// <summary>
        /// Converts an Auth0 API <see cref="Prompt"/> to an internal <see cref="V1TenantPrompts"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API prompt configuration to convert.</param>
        /// <returns>A new <see cref="V1TenantPrompts"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantPrompts? FromApi(GetSettingsResponseContent? source) => source is null ? null : new()
        {
            IdentifierFirst = source.IdentifierFirst,
            UniversalLoginExperience = FromApi(source.UniversalLoginExperience),
            WebauthnPlatformFirstFactor = source.WebauthnPlatformFirstFactor,
        };

        /// <summary>
        /// Converts a universal login experience enum to the corresponding <see cref="V2alpha3TenantUniversalLoginExperience"/> enum value.
        /// </summary>
        /// <param name="source">The Auth0 API universal login experience enum value.</param>
        /// <returns>The matching <see cref="V2alpha3TenantUniversalLoginExperience"/> value.</returns>
        /// <exception cref="NotImplementedException">Thrown when the value is not a recognized experience.</exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantUniversalLoginExperience? FromApi(UniversalLoginExperienceEnum? source)
        {
            return source?.Value switch
            {
                UniversalLoginExperienceEnum.Values.New => V2alpha3TenantUniversalLoginExperience.New,
                UniversalLoginExperienceEnum.Values.Classic => V2alpha3TenantUniversalLoginExperience.Classic,
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
        internal static V2alpha3TenantBranding? FromApi(GetBrandingResponseContent? source) => source is null ? null : new()
        {
            LogoUrl = source.LogoUrl,
            FaviconUrl = source.FaviconUrl,
            Colors = FromApi(source.Colors),
            Font = FromApi(source.Font),
        };

        /// <summary>
        /// Converts an Auth0 API <see cref="BrandingFont"/> to an internal <see cref="V2alpha3TenantBrandingFont"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API branding font to convert.</param>
        /// <returns>A new <see cref="V2alpha3TenantBrandingFont"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantBrandingFont? FromApi(BrandingFont? source) => source is null ? null : new()
        {
            Url = source.Url,
        };

        /// <summary>
        /// Creates a new instance of the internal BrandingColors model from an Auth0 API BrandingColors object.
        /// </summary>
        /// <param name="source">The Auth0 API branding colors to convert.</param>
        /// <returns>A new BrandingColors instance with properties mapped from the specified Auth0 branding colors.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantBrandingColors? FromApi(BrandingColors? source) => source is null ? null : new()
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
        internal static V2alpha3TenantSettings? FromApi(GetTenantSettingsResponseContent? source) => source is null ? null : new()
        {
            FriendlyName = source.FriendlyName,
            Flags = FromApi(source.Flags),
            AcrValuesSupported = source.AcrValuesSupported.IsDefined ? source.AcrValuesSupported.Value?.ToArray() : null,
            AllowedLogoutUrls = source.AllowedLogoutUrls?.ToArray(),
            AllowOrganizationNameInAuthenticationApi = source.AllowOrganizationNameInAuthenticationApi,
            AuthorizationResponseIssParameterSupported = source.AuthorizationResponseIssParameterSupported.IsDefined ? source.AuthorizationResponseIssParameterSupported.Value : null,
            ChangePassword = source.ChangePassword.IsDefined ? FromApi(source.ChangePassword.Value) : null,
            ClientIdMetadataDocumentSupported = source.ClientIdMetadataDocumentSupported,
            CountryCodes = FromApi(source.CountryCodes),
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
            IncludeSessionMetadataInTenantLogs = source.IncludeSessionMetadataInTenantLogs,
            LegacySandboxVersion = source.LegacySandboxVersion,
            OidcLogout = FromApi(source.OidcLogout),
            PictureUrl = source.PictureUrl,
            PhoneConsolidatedExperience = source.PhoneConsolidatedExperience,
            ResourceParameterProfile = FromApi(source.ResourceParameterProfile),
            SecurityHeaders = source.SecurityHeaders.IsDefined ? FromApi(source.SecurityHeaders.Value) : null,
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
        internal static V2alpha3TenantDefaultTokenQuota? FromApi(DefaultTokenQuota? source) => source is null ? null : new()
        {
            Clients = FromApi(source.Clients),
            Organizations = FromApi(source.Organizations),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantTokenQuotaConfiguration? FromApi(TokenQuotaConfiguration? source) => source is null ? null : new()
        {
            ClientCredentials = FromApi(source.ClientCredentials),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantTokenQuotaClientCredentials? FromApi(TokenQuotaClientCredentials? source) => source is null ? null : new()
        {
            Enforce = source.Enforce,
            PerDay = source.PerDay,
            PerHour = source.PerHour,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantCountryCodes? FromApi(TenantSettingsCountryCodesResponse? source) => source is null ? null : new()
        {
            List = source.List?.ToArray(),
            Mode = source.Mode switch
            {
                { Value: TenantSettingsCountryCodesModeResponse.Values.Allow } => V2alpha3TenantCountryCodesModeEnum.Allow,
                { Value: TenantSettingsCountryCodesModeResponse.Values.Deny } => V2alpha3TenantCountryCodesModeEnum.Deny,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source.Mode, null),
            },
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantSecurityHeaders? FromApi(TenantSettingsNullableSecurityHeaders? source) => source is null ? null : new()
        {
            ContentSecurityPolicy = source.ContentSecurityPolicy.IsDefined ? FromApi(source.ContentSecurityPolicy.Value) : null,
            XXssProtection = source.XXssProtection.IsDefined ? FromApi(source.XXssProtection.Value) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantContentSecurityPolicy? FromApi(ContentSecurityPolicyConfig? source) => source is null ? null : new()
        {
            Enabled = source.Enabled,
            Policies = source.Policies?.Select(FromApi).ToArray(),
            ReportingInfrastructure = source.ReportingInfrastructure.IsDefined ? FromApi(source.ReportingInfrastructure.Value) : null,
        };

        internal static V2alpha3TenantCspPolicy FromApi(CspPolicy source) => new()
        {
            Directives = source.Directives?.ToDictionary(i => i.Key, i => i.Value.ToArray()),
            Flags = source.Flags?.Select(FromApi).ToArray(),
            Mode = source.Mode switch
            {
                { Value: CspPolicyMode.Values.Enforcing } => V2alpha3TenantCspPolicyModeEnum.Enforcing,
                { Value: CspPolicyMode.Values.Reporting } => V2alpha3TenantCspPolicyModeEnum.Reporting,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source.Mode, null),
            },
            Reporting = source.Reporting.IsDefined ? FromApi(source.Reporting.Value) : null,
        };

        internal static V2alpha3TenantCspFlagEnum FromApi(CspFlag source) => source switch
        {
            { Value: CspFlag.Values.BlockAllMixedContent } => V2alpha3TenantCspFlagEnum.BlockAllMixedContent,
            { Value: CspFlag.Values.UpgradeInsecureRequests } => V2alpha3TenantCspFlagEnum.UpgradeInsecureRequests,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantCspPolicyReporting? FromApi(CspPolicyReporting? source) => source is null ? null : new()
        {
            ReportToGroup = source.ReportToGroup,
            ReportUri = source.ReportUri,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantCspReportingInfrastructure? FromApi(CspReportingInfrastructure? source) => source is null ? null : new()
        {
            ReportingEndpoints = source.ReportingEndpoints?.ToDictionary(i => i.Key, i => i.Value),
            ReportTo = source.ReportTo.IsDefined ? FromApi(source.ReportTo.Value) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantCspReportTo? FromApi(CspReportTo? source) => source is null ? null : new()
        {
            Endpoints = source.Endpoints?.Select(FromApi).ToArray(),
            Group = source.Group,
            MaxAge = source.MaxAge,
        };

        internal static V2alpha3TenantCspReportToEndpoint FromApi(CspReportToEndpoint source) => new()
        {
            Url = source.Url,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantXssProtection? FromApi(XssProtectionConfig? source) => source is null ? null : new()
        {
            Enabled = source.Enabled,
            Mode = source.Mode switch
            {
                { Value: XssProtectionMode.Values.Block } => V2alpha3TenantXssProtectionModeEnum.Block,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source.Mode, null),
            },
            ReportUri = source.ReportUri,
        };

        /// <summary>
        /// Creates a new instance of the internal TenantMtls model from an Auth0 Management API TenantMtls object.
        /// </summary>
        /// <param name="source">The Auth0 Management API TenantMtls object to convert.</param>
        /// <returns>A new TenantMtls instance populated with values from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantMtls? FromApi(TenantSettingsMtls? source) => source is null ? null : new()
        {
            EnableEndpointAliases = source.EnableEndpointAliases,
        };

        /// <summary>
        /// Creates a new SessionCookie instance from an Auth0 Management API session cookie model.
        /// </summary>
        /// <param name="source">The Auth0 Management API session cookie to convert.</param>
        /// <returns>A SessionCookie instance populated with values from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantSessionCookie? FromApi(SessionCookieSchema? source) => source is null ? null : new()
        {
            Mode = FromApi(source.Mode),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantSessionCookieModeEnum? FromApi(SessionCookieModeEnum? source)
        {
            return source?.Value switch
            {
                "persistent" => V2alpha3TenantSessionCookieModeEnum.Persistent,
                "non_persistent" => V2alpha3TenantSessionCookieModeEnum.NonPersistent,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantSessions? FromApi(TenantSettingsSessions? source) => source is null ? null : new()
        {
            OidcLogoutPromptEnabled = source.OidcLogoutPromptEnabled,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantOidcLogout? FromApi(TenantOidcLogoutSettings? source) => source is null ? null : new()
        {
            RpLogoutEndSessionEndpointDiscovery = source.RpLogoutEndSessionEndpointDiscovery,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantResourceParameterProfile? FromApi(TenantSettingsResourceParameterProfile? source)
        {
            return source?.Value switch
            {
                "audience" => V2alpha3TenantResourceParameterProfile.Audience,
                "compatibility" => V2alpha3TenantResourceParameterProfile.Compatibility,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantDynamicClientRegistrationSecurityMode? FromApi(TenantSettingsDynamicClientRegistrationSecurityMode? source)
        {
            return source?.Value switch
            {
                "strict" => V2alpha3TenantDynamicClientRegistrationSecurityMode.Strict,
                "permissive" => V2alpha3TenantDynamicClientRegistrationSecurityMode.Permissive,
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
        internal static V2alpha3TenantGuardianMfaPage? FromApi(TenantSettingsGuardianPage? source) => source is null ? null : new()
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
        internal static V2alpha3TenantErrorPage? FromApi(TenantSettingsErrorPage? source) => source is null ? null : new()
        {
            ShowLogLink = source.ShowLogLink,
            Url = source.Url,
            Html = source.Html,
        };

        /// <summary>
        /// Converts an Auth0 API TenantDeviceFlow to an internal <see cref="Core.Models.Tenant.V2alpha3.V2alpha3TenantDeviceFlow"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API device flow configuration to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V2alpha3.V2alpha3TenantDeviceFlow"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantDeviceFlow? FromApi(TenantSettingsDeviceFlow? source) => source is null ? null : new()
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
        internal static V2alpha3TenantCharset? FromApi(TenantSettingsDeviceFlowCharset? source)
        {
            return source?.Value switch
            {
                TenantSettingsDeviceFlowCharset.Values.Base20 => V2alpha3TenantCharset.Base20,
                TenantSettingsDeviceFlowCharset.Values.Digits => V2alpha3TenantCharset.Digits,
                null => null,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Converts an Auth0 API TenantChangePassword to an internal <see cref="V2alpha3TenantChangePassword"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API change password page configuration to convert.</param>
        /// <returns>A new <see cref="V2alpha3TenantChangePassword"/> instance mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantChangePassword? FromApi(TenantSettingsPasswordPage? source) => source is null ? null : new()
        {
            Enabled = source.Enabled,
            Html = source.Html,
        };

        /// <summary>
        /// Converts all Auth0 API tenant feature flags to an internal <see cref="V2alpha3TenantFlags"/> model.
        /// </summary>
        /// <param name="source">The Auth0 API tenant feature flags to convert.</param>
        /// <returns>A new <see cref="Core.Models.Tenant.V1.V1TenantFlags"/> instance with all flag values mapped from the specified API model.</returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3TenantFlags? FromApi(TenantSettingsFlags? source) => source is null ? null : new()
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
            DisableImpersonation = source.DisableImpersonation,
            AllowChangingEnableSso = source.AllowChangingEnableSso,
        };

        internal static void ApplyToApi(V2alpha3TenantSettings source, UpdateTenantSettingsRequestContent target)
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

            if (source.CountryCodes is { } country_codes)
            {
                var v = new TenantSettingsCountryCodes();
                ApplyToApi(country_codes, v);
                target.CountryCodes = v;
            }

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

            if (source.IncludeSessionMetadataInTenantLogs is { } include_session_metadata_in_tenant_logs)
                target.IncludeSessionMetadataInTenantLogs = include_session_metadata_in_tenant_logs;

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

            if (source.SecurityHeaders is { } security_headers)
            {
                var v = new TenantSettingsNullableSecurityHeaders();
                ApplyToApi(security_headers, v);
                target.SecurityHeaders = v;
            }

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

        internal static void ApplyToApi(V2alpha3TenantChangePassword source, TenantSettingsPasswordPage target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Html is { } html)
                target.Html = html;
        }

        internal static void ApplyToApi(V2alpha3TenantCountryCodes source, TenantSettingsCountryCodes target)
        {
            if (source.List is { } list)
                target.List = list;

            if (source.Mode is { } mode)
                target.Mode = mode switch
                {
                    V2alpha3TenantCountryCodesModeEnum.Allow => new TenantSettingsCountryCodesMode(TenantSettingsCountryCodesMode.Values.Allow),
                    V2alpha3TenantCountryCodesModeEnum.Deny => new TenantSettingsCountryCodesMode(TenantSettingsCountryCodesMode.Values.Deny),
                    _ => throw new ArgumentOutOfRangeException(nameof(source), mode, null),
                };
        }

        internal static void ApplyToApi(V2alpha3TenantSecurityHeaders source, TenantSettingsNullableSecurityHeaders target)
        {
            if (source.ContentSecurityPolicy is { } content_security_policy)
            {
                var v = new ContentSecurityPolicyConfig();
                ApplyToApi(content_security_policy, v);
                target.ContentSecurityPolicy = v;
            }

            if (source.XXssProtection is { } x_xss_protection)
            {
                var v = new XssProtectionConfig();
                ApplyToApi(x_xss_protection, v);
                target.XXssProtection = v;
            }
        }

        internal static void ApplyToApi(V2alpha3TenantContentSecurityPolicy source, ContentSecurityPolicyConfig target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Policies is { } policies)
                target.Policies = policies.Select(ToApi).ToArray();

            if (source.ReportingInfrastructure is { } reporting_infrastructure)
            {
                var v = new CspReportingInfrastructure();
                ApplyToApi(reporting_infrastructure, v);
                target.ReportingInfrastructure = v;
            }
        }

        internal static CspPolicy ToApi(V2alpha3TenantCspPolicy source)
        {
            var target = new CspPolicy();

            if (source.Directives is { } directives)
                target.Directives = directives.ToDictionary(i => i.Key, i => (IEnumerable<string>)i.Value);

            if (source.Flags is { } flags)
                target.Flags = flags.Select(ToApi).ToArray();

            if (source.Mode is { } mode)
                target.Mode = mode switch
                {
                    V2alpha3TenantCspPolicyModeEnum.Enforcing => new CspPolicyMode(CspPolicyMode.Values.Enforcing),
                    V2alpha3TenantCspPolicyModeEnum.Reporting => new CspPolicyMode(CspPolicyMode.Values.Reporting),
                    _ => throw new ArgumentOutOfRangeException(nameof(source), mode, null),
                };

            if (source.Reporting is { } reporting)
                target.Reporting = new CspPolicyReporting
                {
                    ReportToGroup = reporting.ReportToGroup,
                    ReportUri = reporting.ReportUri,
                };

            return target;
        }

        internal static CspFlag ToApi(V2alpha3TenantCspFlagEnum source) => source switch
        {
            V2alpha3TenantCspFlagEnum.BlockAllMixedContent => new CspFlag(CspFlag.Values.BlockAllMixedContent),
            V2alpha3TenantCspFlagEnum.UpgradeInsecureRequests => new CspFlag(CspFlag.Values.UpgradeInsecureRequests),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static void ApplyToApi(V2alpha3TenantCspReportingInfrastructure source, CspReportingInfrastructure target)
        {
            if (source.ReportingEndpoints is { } reporting_endpoints)
                target.ReportingEndpoints = reporting_endpoints.ToDictionary(i => i.Key, i => i.Value);

            if (source.ReportTo is { } report_to)
                target.ReportTo = new CspReportTo
                {
                    Endpoints = report_to.Endpoints?.Select(i => new CspReportToEndpoint { Url = i.Url }).ToArray(),
                    Group = report_to.Group,
                    MaxAge = report_to.MaxAge,
                };
        }

        internal static void ApplyToApi(V2alpha3TenantXssProtection source, XssProtectionConfig target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Mode is { } mode)
                target.Mode = mode switch
                {
                    V2alpha3TenantXssProtectionModeEnum.Block => new XssProtectionMode(XssProtectionMode.Values.Block),
                    _ => throw new ArgumentOutOfRangeException(nameof(source), mode, null),
                };

            if (source.ReportUri is { } report_uri)
                target.ReportUri = report_uri;
        }

        internal static void ApplyToApi(V2alpha3TenantDeviceFlow source, TenantSettingsDeviceFlow target)
        {
            if (source.Charset is { } charset)
                target.Charset = ToApi(charset);

            if (source.Mask is { } mask)
                target.Mask = mask;
        }

        internal static TenantSettingsDeviceFlowCharset ToApi(V2alpha3TenantCharset charset) => charset switch
        {
            V2alpha3TenantCharset.Base20 => TenantSettingsDeviceFlowCharset.Base20,
            V2alpha3TenantCharset.Digits => TenantSettingsDeviceFlowCharset.Digits,
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha3TenantErrorPage source, TenantSettingsErrorPage target)
        {
            if (source.ShowLogLink is { } show_log_link)
                target.ShowLogLink = show_log_link;

            if (source.Url is { } url)
                target.Url = url;

            if (source.Html is { } html)
                target.Html = html;
        }

        internal static void ApplyToApi(V2alpha3TenantFlags source, TenantSettingsFlags target)
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

            if (source.DisableImpersonation is { } disable_impersonation)
                target.DisableImpersonation = disable_impersonation;

            // AllowChangingEnableSso is a read-only indicator reported by Auth0 and is never transmitted
        }

        internal static void ApplyToApi(V2alpha3TenantGuardianMfaPage source, TenantSettingsGuardianPage target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;

            if (source.Html is { } html)
                target.Html = html;
        }

        internal static void ApplyToApi(V2alpha3TenantMtls source, TenantSettingsMtls target)
        {
            if (source.EnableEndpointAliases is { } enable_endpoint_aliases)
                target.EnableEndpointAliases = enable_endpoint_aliases;
        }

        internal static void ApplyToApi(V2alpha3TenantDefaultTokenQuota source, DefaultTokenQuota target)
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

        internal static void ApplyToApi(V2alpha3TenantTokenQuotaConfiguration source, TokenQuotaConfiguration target)
        {
            if (source.ClientCredentials is { } client_credentials)
            {
                var v = new TokenQuotaClientCredentials();
                ApplyToApi(client_credentials, v);
                target.ClientCredentials = v;
            }
        }

        internal static void ApplyToApi(V2alpha3TenantTokenQuotaClientCredentials source, TokenQuotaClientCredentials target)
        {
            if (source.Enforce is { } enforce)
                target.Enforce = enforce;

            if (source.PerDay is { } per_day)
                target.PerDay = per_day;

            if (source.PerHour is { } per_hour)
                target.PerHour = per_hour;
        }

        internal static void ApplyToApi(V2alpha3TenantOidcLogout source, TenantOidcLogoutSettings target)
        {
            if (source.RpLogoutEndSessionEndpointDiscovery is { } rp_logout_end_session_endpoint_discovery)
                target.RpLogoutEndSessionEndpointDiscovery = rp_logout_end_session_endpoint_discovery;
        }

        internal static void ApplyToApi(V2alpha3TenantSessionCookie source, SessionCookieSchema target)
        {
            if (source.Mode is { } mode)
                target.Mode = ToApi(mode);
        }

        internal static SessionCookieModeEnum ToApi(V2alpha3TenantSessionCookieModeEnum source) => source switch
        {
            V2alpha3TenantSessionCookieModeEnum.Persistent => SessionCookieModeEnum.FromCustom("persistent"),
            V2alpha3TenantSessionCookieModeEnum.NonPersistent => SessionCookieModeEnum.FromCustom("non_persistent"),
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha3TenantSessions source, TenantSettingsSessions target)
        {
            if (source.OidcLogoutPromptEnabled is { } oidc_logout_prompt_enabled)
                target.OidcLogoutPromptEnabled = oidc_logout_prompt_enabled;
        }

        internal static void ApplyToApi(V2alpha3TenantPrompts source, UpdateSettingsRequestContent target)
        {
            if (source.IdentifierFirst is { } identifier_first)
                target.IdentifierFirst = identifier_first;

            if (source.UniversalLoginExperience is { } universal_login_experience)
                target.UniversalLoginExperience = ToApi(universal_login_experience);

            if (source.WebauthnPlatformFirstFactor is { } webauthn_platform_first_factor)
                target.WebauthnPlatformFirstFactor = webauthn_platform_first_factor;
        }

        internal static UniversalLoginExperienceEnum ToApi(V2alpha3TenantUniversalLoginExperience source) => source switch
        {
            V2alpha3TenantUniversalLoginExperience.New => UniversalLoginExperienceEnum.FromCustom(UniversalLoginExperienceEnum.Values.New),
            V2alpha3TenantUniversalLoginExperience.Classic => UniversalLoginExperienceEnum.FromCustom(UniversalLoginExperienceEnum.Values.Classic),
            _ => throw new NotImplementedException(),
        };

        internal static TenantSettingsResourceParameterProfile ToApi(V2alpha3TenantResourceParameterProfile source) => source switch
        {
            V2alpha3TenantResourceParameterProfile.Audience => TenantSettingsResourceParameterProfile.FromCustom("audience"),
            V2alpha3TenantResourceParameterProfile.Compatibility => TenantSettingsResourceParameterProfile.FromCustom("compatibility"),
            _ => throw new NotImplementedException(),
        };

        internal static TenantSettingsDynamicClientRegistrationSecurityMode ToApi(V2alpha3TenantDynamicClientRegistrationSecurityMode source) => source switch
        {
            V2alpha3TenantDynamicClientRegistrationSecurityMode.Strict => TenantSettingsDynamicClientRegistrationSecurityMode.FromCustom("strict"),
            V2alpha3TenantDynamicClientRegistrationSecurityMode.Permissive => TenantSettingsDynamicClientRegistrationSecurityMode.FromCustom("permissive"),
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha3TenantBranding source, UpdateBrandingRequestContent target)
        {
            if (source.LogoUrl is { } logo_url)
                target.LogoUrl = logo_url;

            if (source.FaviconUrl is { } favicon_url)
                target.FaviconUrl = favicon_url;

            if (source.Font is { } font)
                target.Font = new UpdateBrandingFont() { Url = font.Url };

            if (source.Colors is { } colors)
            {
                var v = new UpdateBrandingColors();
                ApplyToApi(colors, v);
                target.Colors = v;
            }
        }

        internal static void ApplyToApi(V2alpha3TenantBrandingColors source, UpdateBrandingColors target)
        {
            if (source.Primary is { } primary)
                target.Primary = primary;

            if (source.PageBackground is { } page_background)
                target.PageBackground = UpdateBrandingPageBackground.FromString(page_background);
        }

        /// <summary>
        /// Determines whether the configured tenant settings differ from the settings last observed in Auth0.
        /// Only properties set on <paramref name="conf"/> are considered; a null property is unmanaged and ignored.
        /// Covers every property pushed by <see cref="ApplyToApi(V2alpha3TenantSettings, UpdateTenantSettingsRequestContent)"/>.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool SettingsRequireUpdate(V2alpha3TenantSettings conf, V2alpha3TenantSettings last)
        {
            if (conf.AcrValuesSupported is not null && (last.AcrValuesSupported is null || conf.AcrValuesSupported.ToHashSet().SetEquals(last.AcrValuesSupported) == false))
                return true;

            if (conf.AllowedLogoutUrls is not null && (last.AllowedLogoutUrls is null || conf.AllowedLogoutUrls.ToHashSet().SetEquals(last.AllowedLogoutUrls) == false))
                return true;

            if (conf.AllowOrganizationNameInAuthenticationApi is not null && conf.AllowOrganizationNameInAuthenticationApi != last.AllowOrganizationNameInAuthenticationApi)
                return true;

            if (conf.AuthorizationResponseIssParameterSupported is not null && conf.AuthorizationResponseIssParameterSupported != last.AuthorizationResponseIssParameterSupported)
                return true;

            if (RequiresUpdate(conf.ChangePassword, last.ChangePassword))
                return true;

            if (conf.ClientIdMetadataDocumentSupported is not null && conf.ClientIdMetadataDocumentSupported != last.ClientIdMetadataDocumentSupported)
                return true;

            if (RequiresUpdate(conf.CountryCodes, last.CountryCodes))
                return true;

            if (conf.CustomizeMfaInPostloginAction is not null && conf.CustomizeMfaInPostloginAction != last.CustomizeMfaInPostloginAction)
                return true;

            if (conf.DefaultRedirectionUri is not null && conf.DefaultRedirectionUri != last.DefaultRedirectionUri)
                return true;

            if (RequiresUpdate(conf.DefaultTokenQuota, last.DefaultTokenQuota))
                return true;

            if (conf.DefaultAudience is not null && conf.DefaultAudience != last.DefaultAudience)
                return true;

            if (conf.DefaultDirectory is not null && conf.DefaultDirectory != last.DefaultDirectory)
                return true;

            if (conf.DynamicClientRegistrationSecurityMode is not null && conf.DynamicClientRegistrationSecurityMode != last.DynamicClientRegistrationSecurityMode)
                return true;

            if (RequiresUpdate(conf.DeviceFlow, last.DeviceFlow))
                return true;

            // the first enabled locale acts as the default, so ordering is semantic and compared as-is
            if (conf.EnabledLocales is not null && (last.EnabledLocales is null || conf.EnabledLocales.SequenceEqual(last.EnabledLocales) == false))
                return true;

            if (conf.EnableAiGuide is not null && conf.EnableAiGuide != last.EnableAiGuide)
                return true;

            if (conf.EphemeralSessionLifetime is not null && conf.EphemeralSessionLifetime != last.EphemeralSessionLifetime)
                return true;

            if (RequiresUpdate(conf.ErrorPage, last.ErrorPage))
                return true;

            if (RequiresUpdate(conf.Flags, last.Flags))
                return true;

            if (conf.FriendlyName is not null && conf.FriendlyName != last.FriendlyName)
                return true;

            if (RequiresUpdate(conf.GuardianMfaPage, last.GuardianMfaPage))
                return true;

            if (conf.IdleEphemeralSessionLifetime is not null && conf.IdleEphemeralSessionLifetime != last.IdleEphemeralSessionLifetime)
                return true;

            if (conf.IdleSessionLifetime is not null && conf.IdleSessionLifetime != last.IdleSessionLifetime)
                return true;

            if (conf.IncludeSessionMetadataInTenantLogs is not null && conf.IncludeSessionMetadataInTenantLogs != last.IncludeSessionMetadataInTenantLogs)
                return true;

            if (conf.LegacySandboxVersion is not null && conf.LegacySandboxVersion != last.LegacySandboxVersion)
                return true;

            if (RequiresUpdate(conf.Mtls, last.Mtls))
                return true;

            if (RequiresUpdate(conf.OidcLogout, last.OidcLogout))
                return true;

            if (conf.PictureUrl is not null && conf.PictureUrl != last.PictureUrl)
                return true;

            if (conf.PhoneConsolidatedExperience is not null && conf.PhoneConsolidatedExperience != last.PhoneConsolidatedExperience)
                return true;

            if (conf.PushedAuthorizationRequestsSupported is not null && conf.PushedAuthorizationRequestsSupported != last.PushedAuthorizationRequestsSupported)
                return true;

            if (conf.ResourceParameterProfile is not null && conf.ResourceParameterProfile != last.ResourceParameterProfile)
                return true;

            if (conf.SandboxVersion is not null && conf.SandboxVersion != last.SandboxVersion)
                return true;

            if (RequiresUpdate(conf.SecurityHeaders, last.SecurityHeaders))
                return true;

            if (RequiresUpdate(conf.SessionCookie, last.SessionCookie))
                return true;

            if (conf.SessionLifetime is not null && conf.SessionLifetime != last.SessionLifetime)
                return true;

            if (RequiresUpdate(conf.Sessions, last.Sessions))
                return true;

            if (conf.SkipNonVerifiableCallbackUriConfirmationPrompt is not null && conf.SkipNonVerifiableCallbackUriConfirmationPrompt != last.SkipNonVerifiableCallbackUriConfirmationPrompt)
                return true;

            if (conf.SupportEmail is not null && conf.SupportEmail != last.SupportEmail)
                return true;

            if (conf.SupportUrl is not null && conf.SupportUrl != last.SupportUrl)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured country codes differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantCountryCodes? conf, V2alpha3TenantCountryCodes? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.List is not null && (last.List is null || conf.List.SequenceEqual(last.List) == false))
                return true;

            if (conf.Mode is not null && conf.Mode != last.Mode)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured security headers differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantSecurityHeaders? conf, V2alpha3TenantSecurityHeaders? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (RequiresUpdate(conf.ContentSecurityPolicy, last.ContentSecurityPolicy))
                return true;

            if (RequiresUpdate(conf.XXssProtection, last.XXssProtection))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured Content-Security-Policy differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantContentSecurityPolicy? conf, V2alpha3TenantContentSecurityPolicy? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            // policy ordering is semantic; policies are pushed as a whole list
            if (conf.Policies is { } policies)
            {
                if (last.Policies is null || policies.Length != last.Policies.Length)
                    return true;

                for (var i = 0; i < policies.Length; i++)
                    if (RequiresUpdate(policies[i], last.Policies[i]))
                        return true;
            }

            if (RequiresUpdate(conf.ReportingInfrastructure, last.ReportingInfrastructure))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured Content-Security-Policy policy differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantCspPolicy? conf, V2alpha3TenantCspPolicy? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Directives is { } directives)
            {
                if (last.Directives is null || directives.Count != last.Directives.Count)
                    return true;

                foreach (var (key, value) in directives)
                    if (last.Directives.TryGetValue(key, out var lastValue) == false || value.SequenceEqual(lastValue) == false)
                        return true;
            }

            if (conf.Flags is not null && (last.Flags is null || conf.Flags.ToHashSet().SetEquals(last.Flags) == false))
                return true;

            if (conf.Mode is not null && conf.Mode != last.Mode)
                return true;

            if (conf.Reporting is { } reporting)
            {
                if (last.Reporting is null)
                    return true;

                if (reporting.ReportToGroup is not null && reporting.ReportToGroup != last.Reporting.ReportToGroup)
                    return true;

                if (reporting.ReportUri is not null && reporting.ReportUri != last.Reporting.ReportUri)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the configured Content-Security-Policy reporting infrastructure differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantCspReportingInfrastructure? conf, V2alpha3TenantCspReportingInfrastructure? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.ReportingEndpoints is { } endpoints)
            {
                if (last.ReportingEndpoints is null || endpoints.Count != last.ReportingEndpoints.Count)
                    return true;

                foreach (var (key, value) in endpoints)
                    if (last.ReportingEndpoints.TryGetValue(key, out var lastValue) == false || value != lastValue)
                        return true;
            }

            if (conf.ReportTo is { } reportTo)
            {
                if (last.ReportTo is null)
                    return true;

                if (reportTo.Group is not null && reportTo.Group != last.ReportTo.Group)
                    return true;

                if (reportTo.MaxAge is not null && reportTo.MaxAge != last.ReportTo.MaxAge)
                    return true;

                if (reportTo.Endpoints is { } reportToEndpoints)
                {
                    if (last.ReportTo.Endpoints is null || reportToEndpoints.Length != last.ReportTo.Endpoints.Length)
                        return true;

                    for (var i = 0; i < reportToEndpoints.Length; i++)
                        if (reportToEndpoints[i].Url != last.ReportTo.Endpoints[i].Url)
                            return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the configured X-XSS-Protection header differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantXssProtection? conf, V2alpha3TenantXssProtection? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            if (conf.Mode is not null && conf.Mode != last.Mode)
                return true;

            if (conf.ReportUri is not null && conf.ReportUri != last.ReportUri)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured change password page differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantChangePassword? conf, V2alpha3TenantChangePassword? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            if (conf.Html is not null && conf.Html != last.Html)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured default token quota differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantDefaultTokenQuota? conf, V2alpha3TenantDefaultTokenQuota? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (RequiresUpdate(conf.Clients, last.Clients))
                return true;

            if (RequiresUpdate(conf.Organizations, last.Organizations))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured token quota configuration differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantTokenQuotaConfiguration? conf, V2alpha3TenantTokenQuotaConfiguration? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (RequiresUpdate(conf.ClientCredentials, last.ClientCredentials))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured token quota client credentials differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantTokenQuotaClientCredentials? conf, V2alpha3TenantTokenQuotaClientCredentials? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Enforce is not null && conf.Enforce != last.Enforce)
                return true;

            if (conf.PerDay is not null && conf.PerDay != last.PerDay)
                return true;

            if (conf.PerHour is not null && conf.PerHour != last.PerHour)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured device flow differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantDeviceFlow? conf, V2alpha3TenantDeviceFlow? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Charset is not null && conf.Charset != last.Charset)
                return true;

            if (conf.Mask is not null && conf.Mask != last.Mask)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured error page differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantErrorPage? conf, V2alpha3TenantErrorPage? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Html is not null && conf.Html != last.Html)
                return true;

            if (conf.ShowLogLink is not null && conf.ShowLogLink != last.ShowLogLink)
                return true;

            if (conf.Url is not null && conf.Url != last.Url)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured feature flags differ from the last observed values. Covers every flag
        /// pushed by <see cref="ApplyToApi(V2alpha3TenantFlags, TenantSettingsFlags)"/>, including <c>EnableSso</c>:
        /// although that flag is force-nulled on the wire, a changed value must enter the update block so the
        /// guard that rejects <c>enable_sso</c> changes fires.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantFlags? conf, V2alpha3TenantFlags? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.AllowLegacyDelegationGrantTypes is not null && conf.AllowLegacyDelegationGrantTypes != last.AllowLegacyDelegationGrantTypes)
                return true;

            if (conf.AllowLegacyRoGrantTypes is not null && conf.AllowLegacyRoGrantTypes != last.AllowLegacyRoGrantTypes)
                return true;

            if (conf.AllowLegacyTokeninfoEndpoint is not null && conf.AllowLegacyTokeninfoEndpoint != last.AllowLegacyTokeninfoEndpoint)
                return true;

            if (conf.ChangePwdFlowV1 is not null && conf.ChangePwdFlowV1 != last.ChangePwdFlowV1)
                return true;

            if (conf.DisableClickjackProtectionHeaders is not null && conf.DisableClickjackProtectionHeaders != last.DisableClickjackProtectionHeaders)
                return true;

            if (conf.DisableManagementApiSmsObfuscation is not null && conf.DisableManagementApiSmsObfuscation != last.DisableManagementApiSmsObfuscation)
                return true;

            if (conf.EnableAdfsWaadEmailVerification is not null && conf.EnableAdfsWaadEmailVerification != last.EnableAdfsWaadEmailVerification)
                return true;

            if (conf.EnableApisSection is not null && conf.EnableApisSection != last.EnableApisSection)
                return true;

            if (conf.EnableClientConnections is not null && conf.EnableClientConnections != last.EnableClientConnections)
                return true;

            if (conf.EnableDynamicClientRegistration is not null && conf.EnableDynamicClientRegistration != last.EnableDynamicClientRegistration)
                return true;

            if (conf.EnableIdtokenApi2 is not null && conf.EnableIdtokenApi2 != last.EnableIdtokenApi2)
                return true;

            if (conf.EnableLegacyProfile is not null && conf.EnableLegacyProfile != last.EnableLegacyProfile)
                return true;

            if (conf.EnablePipeline2 is not null && conf.EnablePipeline2 != last.EnablePipeline2)
                return true;

            if (conf.EnablePublicSignupUserExistsError is not null && conf.EnablePublicSignupUserExistsError != last.EnablePublicSignupUserExistsError)
                return true;

            if (conf.EnableSso is not null && conf.EnableSso != last.EnableSso)
                return true;

            if (conf.EnforceClientAuthenticationOnPasswordlessStart is not null && conf.EnforceClientAuthenticationOnPasswordlessStart != last.EnforceClientAuthenticationOnPasswordlessStart)
                return true;

            if (conf.NoDiscloseEnterpriseConnections is not null && conf.NoDiscloseEnterpriseConnections != last.NoDiscloseEnterpriseConnections)
                return true;

            if (conf.RemoveAlgFromJwks is not null && conf.RemoveAlgFromJwks != last.RemoveAlgFromJwks)
                return true;

            if (conf.RevokeRefreshTokenGrant is not null && conf.RevokeRefreshTokenGrant != last.RevokeRefreshTokenGrant)
                return true;

            if (conf.TrustAzureAdfsEmailVerifiedConnectionProperty is not null && conf.TrustAzureAdfsEmailVerifiedConnectionProperty != last.TrustAzureAdfsEmailVerifiedConnectionProperty)
                return true;

            if (conf.DashboardInsightsView is not null && conf.DashboardInsightsView != last.DashboardInsightsView)
                return true;

            if (conf.DashboardLogStreamsNext is not null && conf.DashboardLogStreamsNext != last.DashboardLogStreamsNext)
                return true;

            if (conf.DisableFieldsMapFix is not null && conf.DisableFieldsMapFix != last.DisableFieldsMapFix)
                return true;

            if (conf.MfaShowFactorListOnEnrollment is not null && conf.MfaShowFactorListOnEnrollment != last.MfaShowFactorListOnEnrollment)
                return true;

            if (conf.ImprovedSignupBotDetectionInClassic is not null && conf.ImprovedSignupBotDetectionInClassic != last.ImprovedSignupBotDetectionInClassic)
                return true;

            if (conf.GenaiTrial is not null && conf.GenaiTrial != last.GenaiTrial)
                return true;

            if (conf.CustomDomainsProvisioning is not null && conf.CustomDomainsProvisioning != last.CustomDomainsProvisioning)
                return true;

            if (conf.DisableImpersonation is not null && conf.DisableImpersonation != last.DisableImpersonation)
                return true;

            // AllowChangingEnableSso is a read-only indicator reported by Auth0; it is never transmitted, so a
            // skew there could never be corrected by updating and must not trigger one

            return false;
        }

        /// <summary>
        /// Determines whether the configured Guardian MFA page differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantGuardianMfaPage? conf, V2alpha3TenantGuardianMfaPage? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            if (conf.Html is not null && conf.Html != last.Html)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured mTLS settings differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantMtls? conf, V2alpha3TenantMtls? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.EnableEndpointAliases is not null && conf.EnableEndpointAliases != last.EnableEndpointAliases)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured OIDC logout settings differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantOidcLogout? conf, V2alpha3TenantOidcLogout? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.RpLogoutEndSessionEndpointDiscovery is not null && conf.RpLogoutEndSessionEndpointDiscovery != last.RpLogoutEndSessionEndpointDiscovery)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured session cookie differs from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantSessionCookie? conf, V2alpha3TenantSessionCookie? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Mode is not null && conf.Mode != last.Mode)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured sessions settings differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantSessions? conf, V2alpha3TenantSessions? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.OidcLogoutPromptEnabled is not null && conf.OidcLogoutPromptEnabled != last.OidcLogoutPromptEnabled)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured tenant branding differs from the branding last observed in Auth0.
        /// Only properties set on <paramref name="conf"/> are considered; a null property is unmanaged and ignored.
        /// Covers every property pushed by <see cref="ApplyToApi(V2alpha3TenantBranding, UpdateBrandingRequestContent)"/>.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool BrandingRequiresUpdate(V2alpha3TenantBranding conf, V2alpha3TenantBranding last)
        {
            if (conf.LogoUrl is not null && conf.LogoUrl != last.LogoUrl)
                return true;

            if (conf.FaviconUrl is not null && conf.FaviconUrl != last.FaviconUrl)
                return true;

            if (RequiresUpdate(conf.Colors, last.Colors))
                return true;

            if (conf.Font is not null && conf.Font.Url is not null && (last.Font is null || conf.Font.Url != last.Font.Url))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured branding colors differ from the last observed value.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool RequiresUpdate(V2alpha3TenantBrandingColors? conf, V2alpha3TenantBrandingColors? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;

            if (conf.Primary is not null && conf.Primary != last.Primary)
                return true;

            if (conf.PageBackground is not null && conf.PageBackground != last.PageBackground)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the configured prompts differ from the prompts last observed in Auth0.
        /// Only properties set on <paramref name="conf"/> are considered; a null property is unmanaged and ignored.
        /// Covers every property pushed by <see cref="ApplyToApi(V2alpha3TenantPrompts, UpdateSettingsRequestContent)"/>.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="last"></param>
        internal static bool PromptsRequireUpdate(V2alpha3TenantPrompts conf, V2alpha3TenantPrompts last)
        {
            if (conf.IdentifierFirst is not null && conf.IdentifierFirst != last.IdentifierFirst)
                return true;

            if (conf.UniversalLoginExperience is not null && conf.UniversalLoginExperience != last.UniversalLoginExperience)
                return true;

            if (conf.WebauthnPlatformFirstFactor is not null && conf.WebauthnPlatformFirstFactor != last.WebauthnPlatformFirstFactor)
                return true;

            return false;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3TenantController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3TenantController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Tenant";

        /// <inheritdoc />
        protected override async Task Reconcile(V2alpha3Tenant entity, CancellationToken cancellationToken)
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
                // settings may not be specified, and are only pushed when they differ from the live settings
                if (conf.Settings is { } newSettings && SettingsRequireUpdate(newSettings, FromApi(settings)))
                {
                    // verify that no changes to enable_sso are being made
                    if (newSettings.Flags != null && newSettings.Flags.EnableSso != null && settings.Flags != null && settings.Flags.EnableSso != null && newSettings.Flags.EnableSso != settings.Flags.EnableSso)
                        throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()}: updating the enable_sso flag is not allowed.");

                    // push update to Auth0
                    var req = new UpdateTenantSettingsRequestContent();
                    ApplyToApi(newSettings, req);
                    if (req.Flags != null)
                        req.Flags.EnableSso = null; // this can never be passed
                    var res = await api.Tenants.Settings.UpdateAsync(req, cancellationToken: cancellationToken);
                    settings = await api.Tenants.Settings.GetAsync(new GetTenantSettingsRequestParameters() { }, cancellationToken: cancellationToken);
                }

                // branding may not be specified, and is only pushed when it differs from the live branding
                if (conf.Branding is { } newBranding && BrandingRequiresUpdate(newBranding, FromApi(branding)))
                {
                    // push update to Auth0
                    var req = new UpdateBrandingRequestContent();
                    ApplyToApi(newBranding, req);
                    var res = await api.Branding.UpdateAsync(req, cancellationToken: cancellationToken);
                    branding = await api.Branding.GetAsync(cancellationToken: cancellationToken);
                }

                // prompts may not be specified, and are only pushed when they differ from the live prompts
                if (conf.Prompts is { } newPrompts && PromptsRequireUpdate(newPrompts, FromApi(prompts)))
                {
                    // push update to Auth0
                    var req = new UpdateSettingsRequestContent();
                    ApplyToApi(newPrompts, req);
                    await api.Prompts.UpdateSettingsAsync(req, cancellationToken: cancellationToken);
                    prompts = await api.Prompts.GetSettingsAsync(cancellationToken: cancellationToken);
                }
            }

            // retrieve and copy new properties to status
            entity.Status.LastConf ??= new V2alpha3TenantConf();
            entity.Status.LastConf.Settings = FromApi(settings);
            entity.Status.LastConf.Branding = FromApi(branding);
            entity.Status.LastConf.Prompts = FromApi(prompts);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
            await ReconcileSuccessAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(V2alpha3Tenant entity, CancellationToken cancellationToken)
        {
            Logger.LogWarning("Unsupported operation deleting entity {Entity}.", entity);
            return Task.CompletedTask;
        }

    }

}
