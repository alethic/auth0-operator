using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsAzureAd
{

    [JsonPropertyName("api_enable_users")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ApiEnableUsers { get; set; }

    [JsonPropertyName("app_domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppDomain { get; set; }

    [JsonPropertyName("app_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppId { get; set; }

    [JsonPropertyName("basic_profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BasicProfile { get; set; }

    [JsonPropertyName("client_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("domain_aliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DomainAliases { get; set; }

    [JsonPropertyName("ext_access_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAccessToken { get; set; }

    [JsonPropertyName("ext_account_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAccountEnabled { get; set; }

    [JsonPropertyName("ext_admin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAdmin { get; set; }

    [JsonPropertyName("ext_agreed_terms")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAgreedTerms { get; set; }

    [JsonPropertyName("ext_assigned_licenses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAssignedLicenses { get; set; }

    [JsonPropertyName("ext_assigned_plans")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAssignedPlans { get; set; }

    [JsonPropertyName("ext_azure_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAzureId { get; set; }

    [JsonPropertyName("ext_city")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtCity { get; set; }

    [JsonPropertyName("ext_country")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtCountry { get; set; }

    [JsonPropertyName("ext_department")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtDepartment { get; set; }

    [JsonPropertyName("ext_dir_sync_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtDirSyncEnabled { get; set; }

    [JsonPropertyName("ext_email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtEmail { get; set; }

    [JsonPropertyName("ext_expires_in")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtExpiresIn { get; set; }

    [JsonPropertyName("ext_family_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtFamilyName { get; set; }

    [JsonPropertyName("ext_fax")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtFax { get; set; }

    [JsonPropertyName("ext_given_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGivenName { get; set; }

    [JsonPropertyName("ext_group_ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGroupIds { get; set; }

    [JsonPropertyName("ext_groups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGroups { get; set; }

    [JsonPropertyName("ext_is_suspended")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtIsSuspended { get; set; }

    [JsonPropertyName("ext_job_title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtJobTitle { get; set; }

    [JsonPropertyName("ext_last_sync")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtLastSync { get; set; }

    [JsonPropertyName("ext_mobile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtMobile { get; set; }

    [JsonPropertyName("ext_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtName { get; set; }

    [JsonPropertyName("ext_nested_groups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtNestedGroups { get; set; }

    [JsonPropertyName("ext_nickname")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtNickname { get; set; }

    [JsonPropertyName("ext_oid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtOid { get; set; }

    [JsonPropertyName("ext_phone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPhone { get; set; }

    [JsonPropertyName("ext_physical_delivery_office_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPhysicalDeliveryOfficeName { get; set; }

    [JsonPropertyName("ext_postal_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPostalCode { get; set; }

    [JsonPropertyName("ext_preferred_language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPreferredLanguage { get; set; }

    [JsonPropertyName("ext_profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProfile { get; set; }

    [JsonPropertyName("ext_provisioned_plans")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProvisionedPlans { get; set; }

    [JsonPropertyName("ext_provisioning_errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProvisioningErrors { get; set; }

    [JsonPropertyName("ext_proxy_addresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtProxyAddresses { get; set; }

    [JsonPropertyName("ext_puid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtPuid { get; set; }

    [JsonPropertyName("ext_refresh_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtRefreshToken { get; set; }

    [JsonPropertyName("ext_roles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtRoles { get; set; }

    [JsonPropertyName("ext_state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtState { get; set; }

    [JsonPropertyName("ext_street")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtStreet { get; set; }

    [JsonPropertyName("ext_telephoneNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtTelephoneNumber { get; set; }

    [JsonPropertyName("ext_tenantid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtTenantid { get; set; }

    [JsonPropertyName("ext_upn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtUpn { get; set; }

    [JsonPropertyName("ext_usage_location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtUsageLocation { get; set; }

    [JsonPropertyName("ext_user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtUserId { get; set; }

    [JsonPropertyName("federated_connections_access_tokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionFederatedConnectionsAccessTokens? FederatedConnectionsAccessTokens { get; set; }

    [JsonPropertyName("granted")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Granted { get; set; }

    [JsonPropertyName("icon_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("identity_api")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionIdentityApiEnumAzureAd? IdentityApi { get; set; }

    [JsonPropertyName("max_groups_to_retrieve")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MaxGroupsToRetrieve { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("set_user_root_attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("should_trust_email_verified_connection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionShouldTrustEmailVerifiedConnectionEnum? ShouldTrustEmailVerifiedConnection { get; set; }

    [JsonPropertyName("tenant_domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantDomain { get; set; }

    [JsonPropertyName("tenantId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantId { get; set; }

    [JsonPropertyName("thumbprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Thumbprints { get; set; }

    [JsonPropertyName("upstream_params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha1ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("use_wsfed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseWsfed { get; set; }

    [JsonPropertyName("useCommonEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseCommonEndpoint { get; set; }

    [JsonPropertyName("userid_attribute")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionUseridAttributeEnumAzureAd? UseridAttribute { get; set; }

    [JsonPropertyName("waad_protocol")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionWaadProtocolEnumAzureAd? WaadProtocol { get; set; }

    [JsonPropertyName("non_persistent_attrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
