using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsAzureAd
{

    [JsonPropertyName("apiEnableUsers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ApiEnableUsers { get; set; }

    [JsonPropertyName("appDomain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppDomain { get; set; }

    [JsonPropertyName("appId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppId { get; set; }

    [JsonPropertyName("basicProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BasicProfile { get; set; }

    [JsonPropertyName("clientId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("domainAliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DomainAliases { get; set; }

    [JsonPropertyName("extAccessToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAccessToken { get; set; }

    [JsonPropertyName("extAccountEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAccountEnabled { get; set; }

    [JsonPropertyName("extAdmin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAdmin { get; set; }

    [JsonPropertyName("extAgreedTerms")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAgreedTerms { get; set; }

    [JsonPropertyName("extAssignedLicenses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAssignedLicenses { get; set; }

    [JsonPropertyName("extAssignedPlans")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAssignedPlans { get; set; }

    [JsonPropertyName("extAzureId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAzureId { get; set; }

    [JsonPropertyName("extCity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtCity { get; set; }

    [JsonPropertyName("extCountry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtCountry { get; set; }

    [JsonPropertyName("extDepartment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtDepartment { get; set; }

    [JsonPropertyName("extDirSyncEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtDirSyncEnabled { get; set; }

    [JsonPropertyName("extEmail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtEmail { get; set; }

    [JsonPropertyName("extExpiresIn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtExpiresIn { get; set; }

    [JsonPropertyName("extFamilyName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtFamilyName { get; set; }

    [JsonPropertyName("extFax")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtFax { get; set; }

    [JsonPropertyName("extGivenName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGivenName { get; set; }

    [JsonPropertyName("extGroupIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGroupIds { get; set; }

    [JsonPropertyName("extGroups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGroups { get; set; }

    [JsonPropertyName("extIsSuspended")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtIsSuspended { get; set; }

    [JsonPropertyName("extJobTitle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtJobTitle { get; set; }

    [JsonPropertyName("extLastSync")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtLastSync { get; set; }

    [JsonPropertyName("extMobile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtMobile { get; set; }

    [JsonPropertyName("extName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtName { get; set; }

    [JsonPropertyName("extNestedGroups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtNestedGroups { get; set; }

    [JsonPropertyName("extNickname")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtNickname { get; set; }

    [JsonPropertyName("extOid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtOid { get; set; }

    [JsonPropertyName("extPhone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPhone { get; set; }

    [JsonPropertyName("extPhysicalDeliveryOfficeName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPhysicalDeliveryOfficeName { get; set; }

    [JsonPropertyName("extPostalCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPostalCode { get; set; }

    [JsonPropertyName("extPreferredLanguage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPreferredLanguage { get; set; }

    [JsonPropertyName("extProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProfile { get; set; }

    [JsonPropertyName("extProvisionedPlans")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProvisionedPlans { get; set; }

    [JsonPropertyName("extProvisioningErrors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProvisioningErrors { get; set; }

    [JsonPropertyName("extProxyAddresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProxyAddresses { get; set; }

    [JsonPropertyName("extPuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPuid { get; set; }

    [JsonPropertyName("extRefreshToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtRefreshToken { get; set; }

    [JsonPropertyName("extRoles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtRoles { get; set; }

    [JsonPropertyName("extState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtState { get; set; }

    [JsonPropertyName("extStreet")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtStreet { get; set; }

    [JsonPropertyName("extTelephoneNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtTelephoneNumber { get; set; }

    [JsonPropertyName("extTenantid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtTenantid { get; set; }

    [JsonPropertyName("extUpn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtUpn { get; set; }

    [JsonPropertyName("extUsageLocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtUsageLocation { get; set; }

    [JsonPropertyName("extUserId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtUserId { get; set; }

    [JsonPropertyName("federatedConnectionsAccessTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionFederatedConnectionsAccessTokens? FederatedConnectionsAccessTokens { get; set; }

    [JsonPropertyName("granted")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Granted { get; set; }

    [JsonPropertyName("iconUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("identityApi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionIdentityApiEnumAzureAd? IdentityApi { get; set; }

    [JsonPropertyName("maxGroupsToRetrieve")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MaxGroupsToRetrieve { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("setUserRootAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("shouldTrustEmailVerifiedConnection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum? ShouldTrustEmailVerifiedConnection { get; set; }

    [JsonPropertyName("tenantDomain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantDomain { get; set; }

    [JsonPropertyName("tenantId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantId { get; set; }

    [JsonPropertyName("thumbprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Thumbprints { get; set; }

    [JsonPropertyName("upstreamParams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("useWsfed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseWsfed { get; set; }

    [JsonPropertyName("useCommonEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseCommonEndpoint { get; set; }

    [JsonPropertyName("useridAttribute")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionUseridAttributeEnumAzureAd? UseridAttribute { get; set; }

    [JsonPropertyName("waadProtocol")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionWaadProtocolEnumAzureAd? WaadProtocol { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
