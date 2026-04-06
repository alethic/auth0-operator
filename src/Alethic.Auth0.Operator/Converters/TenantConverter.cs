using System.Collections.Generic;
using System.Runtime.Versioning;

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
                    EnabledLocales = conf.EnabledLocales,
                    ChangePassword = conf.ChangePassword is { } cp ? new()
                    {
                        Enabled = cp.Enabled,
                        Html = cp.Html
                    } : null,
                    DeviceFlow = conf.DeviceFlow is { } df ? new()
                    {
                        Charset = df.Charset is { } c ? Convert(c) : null,
                        Mask = df.Mask
                    } : null,
                    Flags = conf.Flags is { } f ? new()
                    {
                        AllowLegacyRoGrantTypes = f.AllowLegacyRoGrantTypes,
                        AllowLegacyDelegationGrantTypes = f.AllowLegacyDelegationGrantTypes,
                        AllowLegacyTokeninfoEndpoint = f.AllowLegacyTokeninfoEndpoint,
                        ChangePwdFlowV1 = f.ChangePwdFlowV1,
                        DisableClickjackProtectionHeaders = f.DisableClickjackProtectionHeaders,
                        DisableManagementApiSmsObfuscation = f.DisableManagementApiSmsObfuscation,
                        EnableAdfsWaadEmailVerification = f.EnableAdfsWaadEmailVerification,
                        EnableAPIsSection = f.EnableAPIsSection,
                        EnableClientConnections = f.EnableClientConnections,
                        EnableCustomDomainInEmails = f.EnableCustomDomainInEmails,
                        EnableDynamicClientRegistration = f.EnableDynamicClientRegistration,
                        EnableIdTokenApi2 = f.EnableIdTokenApi2,
                        EnableLegacyProfile = f.EnableLegacyProfile,
                        EnablePipeline2 = f.EnablePipeline2,
                        EnablePublicSignupUserExistsError = f.EnablePublicSignupUserExistsError,
                        EnableSSO = f.EnableSSO,
                        EnforceClientAuthenticationOnPasswordlessStart = f.EnforceClientAuthenticationOnPasswordlessStart,
                        NoDiscloseEnterpriseConnections = f.NoDiscloseEnterpriseConnections,
                        RemoveAlgFromJwks = f.RemoveAlgFromJwks,
                        RequirePushedAuthorizationRequests = f.RequirePushedAuthorizationRequests,
                        RevokeRefreshTokenGrant = f.RevokeRefreshTokenGrant,
                        TrustAzureAdfsEmailVerifiedConnectionProperty = f.TrustAzureAdfsEmailVerifiedConnectionProperty,
                        DashboardLogStreamsNext = f.DashboardLogStreamsNext,
                        DashboardInsightsView = f.DashboardInsightsView,
                        DisableFieldsMapFix = f.DisableFieldsMapFix,
                        MfaShowFactorListOnEnrollment = f.MfaShowFactorListOnEnrollment,
                        ImprovedSignupBotDetectionInClassic = f.ImprovedSignupBotDetectionInClassic,
                        GenaiTrial = f.GenaiTrial,
                        CustomDomainsProvisioning = f.CustomDomainsProvisioning,
                    } : null,
                    GuardianMfaPage = conf.GuardianMfaPage is { } g ? new()
                    {
                        Enabled = g.Enabled,
                        Html = g.Html
                    } : null,
                    DefaultAudience = conf.DefaultAudience,
                    DefaultDirectory = conf.DefaultDirectory,
                    ErrorPage = conf.ErrorPage is { } e ? new()
                    {
                        Html = e.Html,
                        ShowLogLink = e.ShowLogLink,
                        Url = e.Url
                    } : null,
                    PictureUrl = conf.PictureUrl,
                    AllowedLogoutUrls = conf.AllowedLogoutUrls,
                    SessionLifetime = conf.SessionLifetime,
                    IdleSessionLifetime = conf.IdleSessionLifetime,
                    SandboxVersion = conf.SandboxVersion,
                    SandboxVersionsAvailable = conf.SandboxVersionsAvailable,
                    SessionCookie = conf.SessionCookie is { } sc ? new()
                    {
                        Mode = sc.Mode
                    } : null,
                    CustomizeMfaInPostLoginAction = conf.CustomizeMfaInPostLoginAction,
                    AcrValuesSupported = conf.AcrValuesSupported,
                    PushedAuthorizationRequestsSupported = conf.PushedAuthorizationRequestsSupported,
                    Mtls = conf.Mtls is { } m ? new()
                    {
                        EnableEndpointAliases = m.EnableEndpointAliases
                    } : null,
                };
            }

            V2alpha1TenantCharset? Convert(V1TenantCharset? source) => source switch
            {
                V1TenantCharset.Base20 => V2alpha1TenantCharset.Base20,
                V1TenantCharset.Digits => V2alpha1TenantCharset.Digits,
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
                    EnabledLocales = source.EnabledLocales,
                    ChangePassword = source.ChangePassword is { } cp ? new()
                    {
                        Enabled = cp.Enabled,
                        Html = cp.Html,
                    } : null,
                    DeviceFlow = source.DeviceFlow is { } df ? new()
                    {
                        Charset = df.Charset is { } c ? Revert(c) : null,
                        Mask = df.Mask,
                    } : null,
                    Flags = source.Flags is { } f ? new()
                    {
                        AllowLegacyRoGrantTypes = f.AllowLegacyRoGrantTypes,
                        AllowLegacyDelegationGrantTypes = f.AllowLegacyDelegationGrantTypes,
                        AllowLegacyTokeninfoEndpoint = f.AllowLegacyTokeninfoEndpoint,
                        ChangePwdFlowV1 = f.ChangePwdFlowV1,
                        DisableClickjackProtectionHeaders = f.DisableClickjackProtectionHeaders,
                        DisableManagementApiSmsObfuscation = f.DisableManagementApiSmsObfuscation,
                        EnableAdfsWaadEmailVerification = f.EnableAdfsWaadEmailVerification,
                        EnableAPIsSection = f.EnableAPIsSection,
                        EnableClientConnections = f.EnableClientConnections,
                        EnableCustomDomainInEmails = f.EnableCustomDomainInEmails,
                        EnableDynamicClientRegistration = f.EnableDynamicClientRegistration,
                        EnableIdTokenApi2 = f.EnableIdTokenApi2,
                        EnableLegacyProfile = f.EnableLegacyProfile,
                        EnablePipeline2 = f.EnablePipeline2,
                        EnablePublicSignupUserExistsError = f.EnablePublicSignupUserExistsError,
                        EnableSSO = f.EnableSSO,
                        EnforceClientAuthenticationOnPasswordlessStart = f.EnforceClientAuthenticationOnPasswordlessStart,
                        NoDiscloseEnterpriseConnections = f.NoDiscloseEnterpriseConnections,
                        RemoveAlgFromJwks = f.RemoveAlgFromJwks,
                        RequirePushedAuthorizationRequests = f.RequirePushedAuthorizationRequests,
                        RevokeRefreshTokenGrant = f.RevokeRefreshTokenGrant,
                        TrustAzureAdfsEmailVerifiedConnectionProperty = f.TrustAzureAdfsEmailVerifiedConnectionProperty,
                        DashboardLogStreamsNext = f.DashboardLogStreamsNext,
                        DashboardInsightsView = f.DashboardInsightsView,
                        DisableFieldsMapFix = f.DisableFieldsMapFix,
                        MfaShowFactorListOnEnrollment = f.MfaShowFactorListOnEnrollment,
                        ImprovedSignupBotDetectionInClassic = f.ImprovedSignupBotDetectionInClassic,
                        GenaiTrial = f.GenaiTrial,
                        CustomDomainsProvisioning = f.CustomDomainsProvisioning,
                    } : null,
                    GuardianMfaPage = source.GuardianMfaPage is { } g ? new()
                    {
                        Enabled = g.Enabled,
                        Html = g.Html
                    } : null,
                    DefaultAudience = source.DefaultAudience,
                    DefaultDirectory = source.DefaultDirectory,
                    ErrorPage = source.ErrorPage is { } e ? new()
                    {
                        Html = e.Html,
                        ShowLogLink = e.ShowLogLink,
                        Url = e.Url
                    } : null,
                    PictureUrl = source.PictureUrl,
                    AllowedLogoutUrls = source.AllowedLogoutUrls,
                    SessionLifetime = source.SessionLifetime,
                    IdleSessionLifetime = source.IdleSessionLifetime,
                    SandboxVersion = source.SandboxVersion,
                    SandboxVersionsAvailable = source.SandboxVersionsAvailable,
                    SessionCookie = source.SessionCookie is { } sc ? new()
                    {
                        Mode = sc.Mode
                    } : null,
                    CustomizeMfaInPostLoginAction = source.CustomizeMfaInPostLoginAction,
                    AcrValuesSupported = source.AcrValuesSupported,
                    PushedAuthorizationRequestsSupported = source.PushedAuthorizationRequestsSupported,
                    Mtls = source.Mtls is { } m ? new()
                    {
                        EnableEndpointAliases = m.EnableEndpointAliases
                    } : null,
                };
            }

            V1TenantCharset? Revert(V2alpha1TenantCharset source) => source switch
            {
                V2alpha1TenantCharset.Base20 => V1TenantCharset.Base20,
                V2alpha1TenantCharset.Digits => V1TenantCharset.Digits,
                _ => null
            };

        }

    }

}
