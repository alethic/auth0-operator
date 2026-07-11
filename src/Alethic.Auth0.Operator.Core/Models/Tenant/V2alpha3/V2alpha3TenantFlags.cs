using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

public record V2alpha3TenantFlags
{

    [JsonPropertyName("changePwdFlowV1")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ChangePwdFlowV1 { get; set; }

    [JsonPropertyName("enableApisSection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableApisSection { get; set; }

    [JsonPropertyName("disableImpersonation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableImpersonation { get; set; }

    [JsonPropertyName("enableClientConnections")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableClientConnections { get; set; }

    [JsonPropertyName("enablePipeline2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnablePipeline2 { get; set; }

    [JsonPropertyName("allowLegacyDelegationGrantTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowLegacyDelegationGrantTypes { get; set; }

    [JsonPropertyName("allowLegacyRoGrantTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowLegacyRoGrantTypes { get; set; }

    [JsonPropertyName("allowLegacyTokeninfoEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowLegacyTokeninfoEndpoint { get; set; }

    [JsonPropertyName("enableLegacyProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableLegacyProfile { get; set; }

    [JsonPropertyName("enableIdtokenApi2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableIdtokenApi2 { get; set; }

    [JsonPropertyName("enablePublicSignupUserExistsError")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnablePublicSignupUserExistsError { get; set; }

    [JsonPropertyName("enableSso")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableSso { get; set; }

    [JsonPropertyName("allowChangingEnableSso")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowChangingEnableSso { get; set; }

    [JsonPropertyName("disableClickjackProtectionHeaders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableClickjackProtectionHeaders { get; set; }

    [JsonPropertyName("noDiscloseEnterpriseConnections")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? NoDiscloseEnterpriseConnections { get; set; }

    [JsonPropertyName("enforceClientAuthenticationOnPasswordlessStart")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnforceClientAuthenticationOnPasswordlessStart { get; set; }

    [JsonPropertyName("enableAdfsWaadEmailVerification")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableAdfsWaadEmailVerification { get; set; }

    [JsonPropertyName("revokeRefreshTokenGrant")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RevokeRefreshTokenGrant { get; set; }

    [JsonPropertyName("dashboardLogStreamsNext")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DashboardLogStreamsNext { get; set; }

    [JsonPropertyName("dashboardInsightsView")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DashboardInsightsView { get; set; }

    [JsonPropertyName("disableFieldsMapFix")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableFieldsMapFix { get; set; }

    [JsonPropertyName("mfaShowFactorListOnEnrollment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MfaShowFactorListOnEnrollment { get; set; }

    [JsonPropertyName("removeAlgFromJwks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RemoveAlgFromJwks { get; set; }

    [JsonPropertyName("improvedSignupBotDetectionInClassic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ImprovedSignupBotDetectionInClassic { get; set; }

    [JsonPropertyName("genaiTrial")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GenaiTrial { get; set; }

    [JsonPropertyName("enableDynamicClientRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableDynamicClientRegistration { get; set; }

    [JsonPropertyName("disableManagementApiSmsObfuscation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableManagementApiSmsObfuscation { get; set; }

    [JsonPropertyName("trustAzureAdfsEmailVerifiedConnectionProperty")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TrustAzureAdfsEmailVerifiedConnectionProperty { get; set; }

    [JsonPropertyName("customDomainsProvisioning")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CustomDomainsProvisioning { get; set; }

}
