using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;
using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V2alpha1 and V2alpha3 Tenant models.</summary>
    internal static class TenantCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3TenantBranding? Convert(V2alpha1TenantBranding? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantBranding
            {
                Colors = Convert(s.Colors),
                FaviconUrl = s.FaviconUrl,
                LogoUrl = s.LogoUrl,
                Font = Convert(s.Font),
            };
        }

        public static V2alpha1TenantBranding? Revert(V2alpha3TenantBranding? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantBranding
            {
                Colors = Revert(s.Colors),
                FaviconUrl = s.FaviconUrl,
                LogoUrl = s.LogoUrl,
                Font = Revert(s.Font),
            };
        }

        public static V2alpha3TenantBrandingColors? Convert(V2alpha1TenantBrandingColors? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantBrandingColors
            {
                Primary = s.Primary,
                PageBackground = s.PageBackground,
            };
        }

        public static V2alpha1TenantBrandingColors? Revert(V2alpha3TenantBrandingColors? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantBrandingColors
            {
                Primary = s.Primary,
                PageBackground = s.PageBackground,
            };
        }

        public static V2alpha3TenantBrandingFont? Convert(V2alpha1TenantBrandingFont? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantBrandingFont
            {
                Url = s.Url,
            };
        }

        public static V2alpha1TenantBrandingFont? Revert(V2alpha3TenantBrandingFont? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantBrandingFont
            {
                Url = s.Url,
            };
        }

        public static V2alpha3TenantChangePassword? Convert(V2alpha1TenantChangePassword? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantChangePassword
            {
                Enabled = s.Enabled,
                Html = s.Html,
            };
        }

        public static V2alpha1TenantChangePassword? Revert(V2alpha3TenantChangePassword? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantChangePassword
            {
                Enabled = s.Enabled,
                Html = s.Html,
            };
        }

        public static V2alpha3TenantConf? Convert(V2alpha1TenantConf? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantConf
            {
                Settings = Convert(s.Settings),
                Branding = Convert(s.Branding),
                Prompts = Convert(s.Prompts),
            };
        }

        public static V2alpha1TenantConf? Revert(V2alpha3TenantConf? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantConf
            {
                Settings = Revert(s.Settings),
                Branding = Revert(s.Branding),
                Prompts = Revert(s.Prompts),
            };
        }

        public static V2alpha3TenantDefaultTokenQuota? Convert(V2alpha1TenantDefaultTokenQuota? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantDefaultTokenQuota
            {
                Clients = Convert(s.Clients),
                Organizations = Convert(s.Organizations),
            };
        }

        public static V2alpha1TenantDefaultTokenQuota? Revert(V2alpha3TenantDefaultTokenQuota? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantDefaultTokenQuota
            {
                Clients = Revert(s.Clients),
                Organizations = Revert(s.Organizations),
            };
        }

        public static V2alpha3TenantDeviceFlow? Convert(V2alpha1TenantDeviceFlow? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantDeviceFlow
            {
                Charset = MapEnumN<V2alpha1TenantCharset, V2alpha3TenantCharset>(s.Charset),
                Mask = s.Mask,
            };
        }

        public static V2alpha1TenantDeviceFlow? Revert(V2alpha3TenantDeviceFlow? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantDeviceFlow
            {
                Charset = MapEnumN<V2alpha3TenantCharset, V2alpha1TenantCharset>(s.Charset),
                Mask = s.Mask,
            };
        }

        public static V2alpha3TenantErrorPage? Convert(V2alpha1TenantErrorPage? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantErrorPage
            {
                Html = s.Html,
                ShowLogLink = s.ShowLogLink,
                Url = s.Url,
            };
        }

        public static V2alpha1TenantErrorPage? Revert(V2alpha3TenantErrorPage? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantErrorPage
            {
                Html = s.Html,
                ShowLogLink = s.ShowLogLink,
                Url = s.Url,
            };
        }

        public static V2alpha3TenantFlags? Convert(V2alpha1TenantFlags? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantFlags
            {
                ChangePwdFlowV1 = s.ChangePwdFlowV1,
                EnableApisSection = s.EnableApisSection,
                DisableImpersonation = s.DisableImpersonation,
                EnableClientConnections = s.EnableClientConnections,
                EnablePipeline2 = s.EnablePipeline2,
                AllowLegacyDelegationGrantTypes = s.AllowLegacyDelegationGrantTypes,
                AllowLegacyRoGrantTypes = s.AllowLegacyRoGrantTypes,
                AllowLegacyTokeninfoEndpoint = s.AllowLegacyTokeninfoEndpoint,
                EnableLegacyProfile = s.EnableLegacyProfile,
                EnableIdtokenApi2 = s.EnableIdtokenApi2,
                EnablePublicSignupUserExistsError = s.EnablePublicSignupUserExistsError,
                EnableSso = s.EnableSso,
                AllowChangingEnableSso = s.AllowChangingEnableSso,
                DisableClickjackProtectionHeaders = s.DisableClickjackProtectionHeaders,
                NoDiscloseEnterpriseConnections = s.NoDiscloseEnterpriseConnections,
                EnforceClientAuthenticationOnPasswordlessStart = s.EnforceClientAuthenticationOnPasswordlessStart,
                EnableAdfsWaadEmailVerification = s.EnableAdfsWaadEmailVerification,
                RevokeRefreshTokenGrant = s.RevokeRefreshTokenGrant,
                DashboardLogStreamsNext = s.DashboardLogStreamsNext,
                DashboardInsightsView = s.DashboardInsightsView,
                DisableFieldsMapFix = s.DisableFieldsMapFix,
                MfaShowFactorListOnEnrollment = s.MfaShowFactorListOnEnrollment,
                RemoveAlgFromJwks = s.RemoveAlgFromJwks,
                ImprovedSignupBotDetectionInClassic = s.ImprovedSignupBotDetectionInClassic,
                GenaiTrial = s.GenaiTrial,
                EnableDynamicClientRegistration = s.EnableDynamicClientRegistration,
                DisableManagementApiSmsObfuscation = s.DisableManagementApiSmsObfuscation,
                TrustAzureAdfsEmailVerifiedConnectionProperty = s.TrustAzureAdfsEmailVerifiedConnectionProperty,
                CustomDomainsProvisioning = s.CustomDomainsProvisioning,
            };
        }

        public static V2alpha1TenantFlags? Revert(V2alpha3TenantFlags? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantFlags
            {
                ChangePwdFlowV1 = s.ChangePwdFlowV1,
                EnableApisSection = s.EnableApisSection,
                DisableImpersonation = s.DisableImpersonation,
                EnableClientConnections = s.EnableClientConnections,
                EnablePipeline2 = s.EnablePipeline2,
                AllowLegacyDelegationGrantTypes = s.AllowLegacyDelegationGrantTypes,
                AllowLegacyRoGrantTypes = s.AllowLegacyRoGrantTypes,
                AllowLegacyTokeninfoEndpoint = s.AllowLegacyTokeninfoEndpoint,
                EnableLegacyProfile = s.EnableLegacyProfile,
                EnableIdtokenApi2 = s.EnableIdtokenApi2,
                EnablePublicSignupUserExistsError = s.EnablePublicSignupUserExistsError,
                EnableSso = s.EnableSso,
                AllowChangingEnableSso = s.AllowChangingEnableSso,
                DisableClickjackProtectionHeaders = s.DisableClickjackProtectionHeaders,
                NoDiscloseEnterpriseConnections = s.NoDiscloseEnterpriseConnections,
                EnforceClientAuthenticationOnPasswordlessStart = s.EnforceClientAuthenticationOnPasswordlessStart,
                EnableAdfsWaadEmailVerification = s.EnableAdfsWaadEmailVerification,
                RevokeRefreshTokenGrant = s.RevokeRefreshTokenGrant,
                DashboardLogStreamsNext = s.DashboardLogStreamsNext,
                DashboardInsightsView = s.DashboardInsightsView,
                DisableFieldsMapFix = s.DisableFieldsMapFix,
                MfaShowFactorListOnEnrollment = s.MfaShowFactorListOnEnrollment,
                RemoveAlgFromJwks = s.RemoveAlgFromJwks,
                ImprovedSignupBotDetectionInClassic = s.ImprovedSignupBotDetectionInClassic,
                GenaiTrial = s.GenaiTrial,
                EnableDynamicClientRegistration = s.EnableDynamicClientRegistration,
                DisableManagementApiSmsObfuscation = s.DisableManagementApiSmsObfuscation,
                TrustAzureAdfsEmailVerifiedConnectionProperty = s.TrustAzureAdfsEmailVerifiedConnectionProperty,
                CustomDomainsProvisioning = s.CustomDomainsProvisioning,
            };
        }

        public static V2alpha3TenantGuardianMfaPage? Convert(V2alpha1TenantGuardianMfaPage? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantGuardianMfaPage
            {
                Enabled = s.Enabled,
                Html = s.Html,
            };
        }

        public static V2alpha1TenantGuardianMfaPage? Revert(V2alpha3TenantGuardianMfaPage? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantGuardianMfaPage
            {
                Enabled = s.Enabled,
                Html = s.Html,
            };
        }

        public static V2alpha3TenantMtls? Convert(V2alpha1TenantMtls? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantMtls
            {
                EnableEndpointAliases = s.EnableEndpointAliases,
            };
        }

        public static V2alpha1TenantMtls? Revert(V2alpha3TenantMtls? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantMtls
            {
                EnableEndpointAliases = s.EnableEndpointAliases,
            };
        }

        public static V2alpha3TenantOidcLogout? Convert(V2alpha1TenantOidcLogout? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantOidcLogout
            {
                RpLogoutEndSessionEndpointDiscovery = s.RpLogoutEndSessionEndpointDiscovery,
            };
        }

        public static V2alpha1TenantOidcLogout? Revert(V2alpha3TenantOidcLogout? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantOidcLogout
            {
                RpLogoutEndSessionEndpointDiscovery = s.RpLogoutEndSessionEndpointDiscovery,
            };
        }

        public static V2alpha3TenantPrompts? Convert(V2alpha1TenantPrompts? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantPrompts
            {
                UniversalLoginExperience = MapEnumN<V2alpha1TenantUniversalLoginExperience, V2alpha3TenantUniversalLoginExperience>(s.UniversalLoginExperience),
                IdentifierFirst = s.IdentifierFirst,
                WebauthnPlatformFirstFactor = s.WebauthnPlatformFirstFactor,
            };
        }

        public static V2alpha1TenantPrompts? Revert(V2alpha3TenantPrompts? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantPrompts
            {
                UniversalLoginExperience = MapEnumN<V2alpha3TenantUniversalLoginExperience, V2alpha1TenantUniversalLoginExperience>(s.UniversalLoginExperience),
                IdentifierFirst = s.IdentifierFirst,
                WebauthnPlatformFirstFactor = s.WebauthnPlatformFirstFactor,
            };
        }

        public static V2alpha3TenantSessionCookie? Convert(V2alpha1TenantSessionCookie? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantSessionCookie
            {
                Mode = MapEnumN<V2alpha1TenantSessionCookieModeEnum, V2alpha3TenantSessionCookieModeEnum>(s.Mode),
            };
        }

        public static V2alpha1TenantSessionCookie? Revert(V2alpha3TenantSessionCookie? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantSessionCookie
            {
                Mode = MapEnumN<V2alpha3TenantSessionCookieModeEnum, V2alpha1TenantSessionCookieModeEnum>(s.Mode),
            };
        }

        public static V2alpha3TenantSessions? Convert(V2alpha1TenantSessions? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantSessions
            {
                OidcLogoutPromptEnabled = s.OidcLogoutPromptEnabled,
            };
        }

        public static V2alpha1TenantSessions? Revert(V2alpha3TenantSessions? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantSessions
            {
                OidcLogoutPromptEnabled = s.OidcLogoutPromptEnabled,
            };
        }

        public static V2alpha3TenantSettings? Convert(V2alpha1TenantSettings? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantSettings
            {
                ChangePassword = Convert(s.ChangePassword),
                DeviceFlow = Convert(s.DeviceFlow),
                GuardianMfaPage = Convert(s.GuardianMfaPage),
                DefaultAudience = s.DefaultAudience,
                DefaultDirectory = s.DefaultDirectory,
                ErrorPage = Convert(s.ErrorPage),
                DefaultTokenQuota = Convert(s.DefaultTokenQuota),
                Flags = Convert(s.Flags),
                FriendlyName = s.FriendlyName,
                PictureUrl = s.PictureUrl,
                SupportEmail = s.SupportEmail,
                SupportUrl = s.SupportUrl,
                AllowedLogoutUrls = s.AllowedLogoutUrls,
                SessionLifetime = s.SessionLifetime,
                IdleSessionLifetime = s.IdleSessionLifetime,
                EphemeralSessionLifetime = s.EphemeralSessionLifetime,
                IdleEphemeralSessionLifetime = s.IdleEphemeralSessionLifetime,
                SandboxVersion = s.SandboxVersion,
                LegacySandboxVersion = s.LegacySandboxVersion,
                DefaultRedirectionUri = s.DefaultRedirectionUri,
                EnabledLocales = s.EnabledLocales,
                SessionCookie = Convert(s.SessionCookie),
                Sessions = Convert(s.Sessions),
                OidcLogout = Convert(s.OidcLogout),
                CustomizeMfaInPostloginAction = s.CustomizeMfaInPostloginAction,
                AllowOrganizationNameInAuthenticationApi = s.AllowOrganizationNameInAuthenticationApi,
                AcrValuesSupported = s.AcrValuesSupported,
                Mtls = Convert(s.Mtls),
                PushedAuthorizationRequestsSupported = s.PushedAuthorizationRequestsSupported,
                AuthorizationResponseIssParameterSupported = s.AuthorizationResponseIssParameterSupported,
                SkipNonVerifiableCallbackUriConfirmationPrompt = s.SkipNonVerifiableCallbackUriConfirmationPrompt,
                ResourceParameterProfile = MapEnumN<V2alpha1TenantResourceParameterProfile, V2alpha3TenantResourceParameterProfile>(s.ResourceParameterProfile),
                ClientIdMetadataDocumentSupported = s.ClientIdMetadataDocumentSupported,
                EnableAiGuide = s.EnableAiGuide,
                PhoneConsolidatedExperience = s.PhoneConsolidatedExperience,
                DynamicClientRegistrationSecurityMode = MapEnumN<V2alpha1TenantDynamicClientRegistrationSecurityMode, V2alpha3TenantDynamicClientRegistrationSecurityMode>(s.DynamicClientRegistrationSecurityMode),
            };
        }

        public static V2alpha1TenantSettings? Revert(V2alpha3TenantSettings? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantSettings
            {
                ChangePassword = Revert(s.ChangePassword),
                DeviceFlow = Revert(s.DeviceFlow),
                GuardianMfaPage = Revert(s.GuardianMfaPage),
                DefaultAudience = s.DefaultAudience,
                DefaultDirectory = s.DefaultDirectory,
                ErrorPage = Revert(s.ErrorPage),
                DefaultTokenQuota = Revert(s.DefaultTokenQuota),
                Flags = Revert(s.Flags),
                FriendlyName = s.FriendlyName,
                PictureUrl = s.PictureUrl,
                SupportEmail = s.SupportEmail,
                SupportUrl = s.SupportUrl,
                AllowedLogoutUrls = s.AllowedLogoutUrls,
                SessionLifetime = s.SessionLifetime,
                IdleSessionLifetime = s.IdleSessionLifetime,
                EphemeralSessionLifetime = s.EphemeralSessionLifetime,
                IdleEphemeralSessionLifetime = s.IdleEphemeralSessionLifetime,
                SandboxVersion = s.SandboxVersion,
                LegacySandboxVersion = s.LegacySandboxVersion,
                DefaultRedirectionUri = s.DefaultRedirectionUri,
                EnabledLocales = s.EnabledLocales,
                SessionCookie = Revert(s.SessionCookie),
                Sessions = Revert(s.Sessions),
                OidcLogout = Revert(s.OidcLogout),
                CustomizeMfaInPostloginAction = s.CustomizeMfaInPostloginAction,
                AllowOrganizationNameInAuthenticationApi = s.AllowOrganizationNameInAuthenticationApi,
                AcrValuesSupported = s.AcrValuesSupported,
                Mtls = Revert(s.Mtls),
                PushedAuthorizationRequestsSupported = s.PushedAuthorizationRequestsSupported,
                AuthorizationResponseIssParameterSupported = s.AuthorizationResponseIssParameterSupported,
                SkipNonVerifiableCallbackUriConfirmationPrompt = s.SkipNonVerifiableCallbackUriConfirmationPrompt,
                ResourceParameterProfile = MapEnumN<V2alpha3TenantResourceParameterProfile, V2alpha1TenantResourceParameterProfile>(s.ResourceParameterProfile),
                ClientIdMetadataDocumentSupported = s.ClientIdMetadataDocumentSupported,
                EnableAiGuide = s.EnableAiGuide,
                PhoneConsolidatedExperience = s.PhoneConsolidatedExperience,
                DynamicClientRegistrationSecurityMode = MapEnumN<V2alpha3TenantDynamicClientRegistrationSecurityMode, V2alpha1TenantDynamicClientRegistrationSecurityMode>(s.DynamicClientRegistrationSecurityMode),
            };
        }

        public static V2alpha3TenantTokenQuotaClientCredentials? Convert(V2alpha1TenantTokenQuotaClientCredentials? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantTokenQuotaClientCredentials
            {
                Enforce = s.Enforce,
                PerDay = s.PerDay,
                PerHour = s.PerHour,
            };
        }

        public static V2alpha1TenantTokenQuotaClientCredentials? Revert(V2alpha3TenantTokenQuotaClientCredentials? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantTokenQuotaClientCredentials
            {
                Enforce = s.Enforce,
                PerDay = s.PerDay,
                PerHour = s.PerHour,
            };
        }

        public static V2alpha3TenantTokenQuotaConfiguration? Convert(V2alpha1TenantTokenQuotaConfiguration? s)
        {
            if (s is null) return null;
            return new V2alpha3TenantTokenQuotaConfiguration
            {
                ClientCredentials = Convert(s.ClientCredentials),
            };
        }

        public static V2alpha1TenantTokenQuotaConfiguration? Revert(V2alpha3TenantTokenQuotaConfiguration? s)
        {
            if (s is null) return null;
            return new V2alpha1TenantTokenQuotaConfiguration
            {
                ClientCredentials = Revert(s.ClientCredentials),
            };
        }

    }

}
