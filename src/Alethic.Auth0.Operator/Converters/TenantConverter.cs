using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

using Auth0.ManagementApi;

using Alethic.Auth0.Operator.Core.Models.Tenant.V1;
using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    /// <summary>
    /// Provides conversions targeting <see cref="V2alpha1Tenant"/>.
    /// </summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha1Tenant))]
    public class TenantConverter : ConversionWebhook<V2alpha1Tenant>
    {

        protected override IEnumerable<IEntityConverter<V2alpha1Tenant>> Converters => [
            new V1ToV2alpha1()
        ];

        /// <summary>
        /// Converts from <see cref="V2alpha1Tenant"/> to <see cref="V1Tenant"/>.
        /// </summary>
        class V1ToV2alpha1 : IEntityConverter<V1Tenant, V2alpha1Tenant>
        {

            public V2alpha1Tenant Convert(V1Tenant from)
            {
                var result = new V2alpha1Tenant { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.Name = from.Spec.Name;
                result.Spec.Auth = from.Spec.Auth is { } auth ? new() { SecretRef = auth.SecretRef, Domain = auth.Domain } : null;
                result.Spec.Init = new() { Settings = Convert(from.Spec.Init) };
                result.Spec.Conf = new() { Settings = Convert(from.Spec.Conf) };
                result.Status.LastConf = new() { Settings = Convert(from.Status.LastConf) };
                return result;
            }

            V2alpha1TenantSettings? Convert(V1TenantConf? conf)
            {
                if (conf is null)
                    return null;

                return new()
                {
                    FriendlyName = conf.FriendlyName,
                    SupportEmail = conf.SupportEmail,
                    SupportUrl = conf.SupportUrl,
                    EnabledLocales = conf.EnabledLocales?.ToArray(),
                    ChangePassword = conf.ChangePassword is { } change_password ? new()
                    {
                        Enabled = change_password.Enabled,
                        Html = change_password.Html
                    } : null,
                    DeviceFlow = conf.DeviceFlow is { } device_flow ? new()
                    {
                        Charset = device_flow.Charset is { } charset ? Convert(charset) : null,
                        Mask = device_flow.Mask
                    } : null,
                    Flags = conf.Flags is { } flags ? new()
                    {
                        AllowLegacyRoGrantTypes = flags.AllowLegacyRoGrantTypes,
                        AllowLegacyDelegationGrantTypes = flags.AllowLegacyDelegationGrantTypes,
                        AllowLegacyTokeninfoEndpoint = flags.AllowLegacyTokeninfoEndpoint,
                        ChangePwdFlowV1 = flags.ChangePwdFlowV1,
                        DisableClickjackProtectionHeaders = flags.DisableClickjackProtectionHeaders,
                        DisableManagementApiSmsObfuscation = flags.DisableManagementApiSmsObfuscation,
                        EnableAdfsWaadEmailVerification = flags.EnableAdfsWaadEmailVerification,
                        EnableApisSection = flags.EnableAPIsSection,
                        EnableClientConnections = flags.EnableClientConnections,
                        EnableDynamicClientRegistration = flags.EnableDynamicClientRegistration,
                        EnableIdtokenApi2 = flags.EnableIdTokenApi2,
                        EnableLegacyProfile = flags.EnableLegacyProfile,
                        EnablePipeline2 = flags.EnablePipeline2,
                        EnablePublicSignupUserExistsError = flags.EnablePublicSignupUserExistsError,
                        EnableSso = flags.EnableSSO,
                        EnforceClientAuthenticationOnPasswordlessStart = flags.EnforceClientAuthenticationOnPasswordlessStart,
                        NoDiscloseEnterpriseConnections = flags.NoDiscloseEnterpriseConnections,
                        RemoveAlgFromJwks = flags.RemoveAlgFromJwks,
                        RevokeRefreshTokenGrant = flags.RevokeRefreshTokenGrant,
                        TrustAzureAdfsEmailVerifiedConnectionProperty = flags.TrustAzureAdfsEmailVerifiedConnectionProperty,
                        DashboardLogStreamsNext = flags.DashboardLogStreamsNext,
                        DashboardInsightsView = flags.DashboardInsightsView,
                        DisableFieldsMapFix = flags.DisableFieldsMapFix,
                        MfaShowFactorListOnEnrollment = flags.MfaShowFactorListOnEnrollment,
                        ImprovedSignupBotDetectionInClassic = flags.ImprovedSignupBotDetectionInClassic,
                        GenaiTrial = flags.GenaiTrial,
                        CustomDomainsProvisioning = flags.CustomDomainsProvisioning,
                    } : null,
                    GuardianMfaPage = conf.GuardianMfaPage is { } guardian_mfa_page ? new()
                    {
                        Enabled = guardian_mfa_page.Enabled,
                        Html = guardian_mfa_page.Html
                    } : null,
                    DefaultAudience = conf.DefaultAudience,
                    DefaultDirectory = conf.DefaultDirectory,
                    ErrorPage = conf.ErrorPage is { } error_page ? new()
                    {
                        Html = error_page.Html,
                        ShowLogLink = error_page.ShowLogLink,
                        Url = error_page.Url
                    } : null,
                    PictureUrl = conf.PictureUrl,
                    AllowedLogoutUrls = conf.AllowedLogoutUrls,
                    SessionLifetime = conf.SessionLifetime is { } session_lifetime ? (int?)session_lifetime : null,
                    IdleSessionLifetime = conf.IdleSessionLifetime is { } idle_session_lifetime ? (int?)idle_session_lifetime : null,
                    SandboxVersion = conf.SandboxVersion,
                    SessionCookie = conf.SessionCookie is { } session_cookie ? new()
                    {
                        Mode = Convert(session_cookie.Mode)
                    } : null,
                    CustomizeMfaInPostloginAction = conf.CustomizeMfaInPostLoginAction,
                    AcrValuesSupported = conf.AcrValuesSupported,
                    PushedAuthorizationRequestsSupported = conf.PushedAuthorizationRequestsSupported,
                    Mtls = conf.Mtls is { } mtls ? new()
                    {
                        EnableEndpointAliases = mtls.EnableEndpointAliases
                    } : null,
                };
            }

            V2alpha1TenantCharset? Convert(V1TenantCharset? source) => source switch
            {
                V1TenantCharset.Base20 => V2alpha1TenantCharset.Base20,
                V1TenantCharset.Digits => V2alpha1TenantCharset.Digits,
                _ => null
            };

            V2alpha1TenantSessionCookieModeEnum? Convert(string? source) => source switch
            {
                SessionCookieModeEnum.Values.Persistent => V2alpha1TenantSessionCookieModeEnum.Persistent,
                SessionCookieModeEnum.Values.NonPersistent => V2alpha1TenantSessionCookieModeEnum.NonPersistent,
                null => null,
                _ => null
            };

            public V1Tenant Revert(V2alpha1Tenant source)
            {
                var result = new V1Tenant { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.Name = source.Spec.Name;
                result.Spec.Auth = source.Spec.Auth is { } auth ? new()
                {
                    SecretRef = auth.SecretRef,
                    Domain = auth.Domain
                } : null;
                result.Spec.Init = Revert(source.Spec.Init?.Settings);
                result.Spec.Conf = Revert(source.Spec.Conf?.Settings);
                result.Status.LastConf = Revert(source.Status.LastConf?.Settings);
                return result;
            }

            V1TenantConf? Revert(V2alpha1TenantSettings? source)
            {
                if (source is null)
                    return null;

                return new()
                {
                    FriendlyName = source.FriendlyName,
                    SupportEmail = source.SupportEmail,
                    SupportUrl = source.SupportUrl,
                    EnabledLocales = source.EnabledLocales?.ToList(),
                    ChangePassword = source.ChangePassword is { } change_password ? new()
                    {
                        Enabled = change_password.Enabled,
                        Html = change_password.Html,
                    } : null,
                    DeviceFlow = source.DeviceFlow is { } device_flow ? new()
                    {
                        Charset = device_flow.Charset is { } charset ? Revert(charset) : null,
                        Mask = device_flow.Mask,
                    } : null,
                    Flags = source.Flags is { } flags ? new()
                    {
                        AllowLegacyRoGrantTypes = flags.AllowLegacyRoGrantTypes,
                        AllowLegacyDelegationGrantTypes = flags.AllowLegacyDelegationGrantTypes,
                        AllowLegacyTokeninfoEndpoint = flags.AllowLegacyTokeninfoEndpoint,
                        ChangePwdFlowV1 = flags.ChangePwdFlowV1,
                        DisableClickjackProtectionHeaders = flags.DisableClickjackProtectionHeaders,
                        DisableManagementApiSmsObfuscation = flags.DisableManagementApiSmsObfuscation,
                        EnableAdfsWaadEmailVerification = flags.EnableAdfsWaadEmailVerification,
                        EnableAPIsSection = flags.EnableApisSection,
                        EnableClientConnections = flags.EnableClientConnections,
                        EnableDynamicClientRegistration = flags.EnableDynamicClientRegistration,
                        EnableIdTokenApi2 = flags.EnableIdtokenApi2,
                        EnableLegacyProfile = flags.EnableLegacyProfile,
                        EnablePipeline2 = flags.EnablePipeline2,
                        EnablePublicSignupUserExistsError = flags.EnablePublicSignupUserExistsError,
                        EnableSSO = flags.EnableSso,
                        EnforceClientAuthenticationOnPasswordlessStart = flags.EnforceClientAuthenticationOnPasswordlessStart,
                        NoDiscloseEnterpriseConnections = flags.NoDiscloseEnterpriseConnections,
                        RemoveAlgFromJwks = flags.RemoveAlgFromJwks,
                        RevokeRefreshTokenGrant = flags.RevokeRefreshTokenGrant,
                        TrustAzureAdfsEmailVerifiedConnectionProperty = flags.TrustAzureAdfsEmailVerifiedConnectionProperty,
                        DashboardLogStreamsNext = flags.DashboardLogStreamsNext,
                        DashboardInsightsView = flags.DashboardInsightsView,
                        DisableFieldsMapFix = flags.DisableFieldsMapFix,
                        MfaShowFactorListOnEnrollment = flags.MfaShowFactorListOnEnrollment,
                        ImprovedSignupBotDetectionInClassic = flags.ImprovedSignupBotDetectionInClassic,
                        GenaiTrial = flags.GenaiTrial,
                        CustomDomainsProvisioning = flags.CustomDomainsProvisioning,
                    } : null,
                    GuardianMfaPage = source.GuardianMfaPage is { } guardian_mfa_page ? new()
                    {
                        Enabled = guardian_mfa_page.Enabled,
                        Html = guardian_mfa_page.Html
                    } : null,
                    DefaultAudience = source.DefaultAudience,
                    DefaultDirectory = source.DefaultDirectory,
                    ErrorPage = source.ErrorPage is { } error_page ? new()
                    {
                        Html = error_page.Html,
                        ShowLogLink = error_page.ShowLogLink,
                        Url = error_page.Url
                    } : null,
                    PictureUrl = source.PictureUrl,
                    AllowedLogoutUrls = source.AllowedLogoutUrls,
                    SessionLifetime = source.SessionLifetime,
                    IdleSessionLifetime = source.IdleSessionLifetime,
                    SandboxVersion = source.SandboxVersion,
                    SessionCookie = source.SessionCookie is { } session_cookie ? new()
                    {
                        Mode = Revert(session_cookie.Mode)
                    } : null,
                    CustomizeMfaInPostLoginAction = source.CustomizeMfaInPostloginAction,
                    AcrValuesSupported = source.AcrValuesSupported,
                    PushedAuthorizationRequestsSupported = source.PushedAuthorizationRequestsSupported,
                    Mtls = source.Mtls is { } mtls ? new()
                    {
                        EnableEndpointAliases = mtls.EnableEndpointAliases
                    } : null,
                };
            }

            V1TenantCharset? Revert(V2alpha1TenantCharset source) => source switch
            {
                V2alpha1TenantCharset.Base20 => V1TenantCharset.Base20,
                V2alpha1TenantCharset.Digits => V1TenantCharset.Digits,
                _ => null
            };

            string? Revert(V2alpha1TenantSessionCookieModeEnum? source) => source switch
            {
                V2alpha1TenantSessionCookieModeEnum.Persistent => SessionCookieModeEnum.Values.Persistent,
                V2alpha1TenantSessionCookieModeEnum.NonPersistent => SessionCookieModeEnum.Values.NonPersistent,
                null => null,
                _ => null
            };

        }

    }

}
