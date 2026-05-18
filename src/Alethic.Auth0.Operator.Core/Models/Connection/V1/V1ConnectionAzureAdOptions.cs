using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>waad</c> (Azure Active Directory / Microsoft Entra ID) connection strategy.
    /// </summary>
    public record V1ConnectionAzureAdOptions : V1ConnectionOptionsClientCredentials
    {

        /// <summary>
        /// Azure AD tenant ID (GUID or domain name).
        /// </summary>
        [JsonPropertyName("tenantId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantId { get; set; }

        /// <summary>
        /// Azure AD application (client) ID.
        /// </summary>
        [JsonPropertyName("app_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AppId { get; set; }

        /// <summary>
        /// The Azure AD domain name (e.g. <c>contoso.onmicrosoft.com</c>).
        /// </summary>
        [JsonPropertyName("app_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AppDomain { get; set; }

        /// <summary>
        /// List of domain aliases for the connection.
        /// </summary>
        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        /// <summary>
        /// Primary tenant domain for the connection.
        /// </summary>
        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        /// <summary>
        /// URL of the icon to display for this connection in the Universal Login experience.
        /// </summary>
        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        /// <summary>
        /// Microsoft identity platform API version to use (<c>v1</c> or <c>v2</c>).
        /// </summary>
        [JsonPropertyName("identity_api")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IdentityApi { get; set; }

        /// <summary>
        /// Protocol to use with Azure AD. Can be <c>openid-connect</c> or <c>wsfed</c>.
        /// </summary>
        [JsonPropertyName("waad_protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WaadProtocol { get; set; }

        /// <summary>
        /// When <c>true</c>, uses WS-Federation protocol instead of OpenID Connect.
        /// </summary>
        [JsonPropertyName("use_wsfed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UseWsfed { get; set; }

        /// <summary>
        /// When <c>true</c>, uses the common Azure AD endpoint instead of a tenant-specific endpoint.
        /// </summary>
        [JsonPropertyName("useCommonEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UseCommonEndpoint { get; set; }

        /// <summary>
        /// Azure AD attribute to use as the Auth0 user ID.
        /// </summary>
        [JsonPropertyName("userid_attribute")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UseridAttribute { get; set; }

        /// <summary>
        /// Space-separated list of OAuth 2.0 scopes to request from Azure AD.
        /// </summary>
        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        /// <summary>
        /// Certificate thumbprints used to validate tokens from the Azure AD server.
        /// </summary>
        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        /// <summary>
        /// When <c>true</c>, admin consent has been granted for the configured permissions.
        /// </summary>
        [JsonPropertyName("granted")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Granted { get; set; }

        /// <summary>
        /// Maximum number of groups to retrieve from Azure AD for a user.
        /// </summary>
        [JsonPropertyName("max_groups_to_retrieve")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MaxGroupsToRetrieve { get; set; }

        /// <summary>
        /// When <c>true</c>, the Azure AD Users API is enabled to retrieve additional user information.
        /// </summary>
        [JsonPropertyName("api_enable_users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ApiEnableUsers { get; set; }

        /// <summary>
        /// When <c>true</c>, the basic profile scope is requested.
        /// </summary>
        [JsonPropertyName("basic_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BasicProfile { get; set; }

        /// <summary>
        /// Determines whether Auth0 should trust the email-verified claim from this identity provider.
        /// </summary>
        [JsonPropertyName("should_trust_email_verified_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShouldTrustEmailVerifiedConnection { get; set; }

        /// <summary>
        /// Configuration for federated connection access tokens.
        /// </summary>
        [JsonPropertyName("federated_connections_access_tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsFederatedConnectionsAccessTokens? FederatedConnectionsAccessTokens { get; set; }

        /// <summary>When <c>true</c>, includes the user's profile information in the token.</summary>
        [JsonPropertyName("ext_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtProfile { get; set; }

        /// <summary>When <c>true</c>, includes the user's group memberships in the token.</summary>
        [JsonPropertyName("ext_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGroups { get; set; }

        /// <summary>When <c>true</c>, includes nested/transitive group memberships in the token.</summary>
        [JsonPropertyName("ext_nested_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtNestedGroups { get; set; }

        /// <summary>When <c>true</c>, includes extended group information in the token.</summary>
        [JsonPropertyName("ext_groups_extended")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGroupsExtended { get; set; }

        /// <summary>When <c>true</c>, includes group object IDs in the token.</summary>
        [JsonPropertyName("ext_group_ids")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGroupIds { get; set; }

        /// <summary>When <c>true</c>, includes whether the user is a tenant admin in the token.</summary>
        [JsonPropertyName("ext_is_admin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtIsAdmin { get; set; }

        /// <summary>When <c>true</c>, includes whether the user account is suspended in the token.</summary>
        [JsonPropertyName("ext_is_suspended")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtIsSuspended { get; set; }

        /// <summary>When <c>true</c>, includes whether the user has agreed to terms in the token.</summary>
        [JsonPropertyName("ext_agreed_terms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAgreedTerms { get; set; }

        /// <summary>When <c>true</c>, includes admin role information in the token.</summary>
        [JsonPropertyName("ext_admin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAdmin { get; set; }

        /// <summary>When <c>true</c>, includes the Azure AD user ID in the token.</summary>
        [JsonPropertyName("ext_user_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtUserId { get; set; }

        /// <summary>When <c>true</c>, includes the user's email address in the token.</summary>
        [JsonPropertyName("ext_email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtEmail { get; set; }

        /// <summary>When <c>true</c>, includes the user's given (first) name in the token.</summary>
        [JsonPropertyName("ext_given_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGivenName { get; set; }

        /// <summary>When <c>true</c>, includes the user's family (last) name in the token.</summary>
        [JsonPropertyName("ext_family_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtFamilyName { get; set; }

        /// <summary>When <c>true</c>, includes the user's full display name in the token.</summary>
        [JsonPropertyName("ext_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtName { get; set; }

        /// <summary>When <c>true</c>, includes the user's nickname in the token.</summary>
        [JsonPropertyName("ext_nickname")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtNickname { get; set; }

        /// <summary>When <c>true</c>, includes the user's phone number in the token.</summary>
        [JsonPropertyName("ext_phone")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtPhone { get; set; }

        /// <summary>When <c>true</c>, includes the user's state/province in the token.</summary>
        [JsonPropertyName("ext_state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtState { get; set; }

        /// <summary>When <c>true</c>, includes the user's city in the token.</summary>
        [JsonPropertyName("ext_city")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtCity { get; set; }

        /// <summary>When <c>true</c>, includes the user's country in the token.</summary>
        [JsonPropertyName("ext_country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtCountry { get; set; }

        /// <summary>When <c>true</c>, includes the user's street address in the token.</summary>
        [JsonPropertyName("ext_street")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtStreet { get; set; }

        /// <summary>When <c>true</c>, includes the user's postal/ZIP code in the token.</summary>
        [JsonPropertyName("ext_postal_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtPostalCode { get; set; }

        /// <summary>When <c>true</c>, includes the user's fax number in the token.</summary>
        [JsonPropertyName("ext_fax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtFax { get; set; }

        /// <summary>When <c>true</c>, includes the user's mobile phone number in the token.</summary>
        [JsonPropertyName("ext_mobile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtMobile { get; set; }

        /// <summary>When <c>true</c>, includes the user's job title in the token.</summary>
        [JsonPropertyName("ext_job_title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtJobTitle { get; set; }

        /// <summary>When <c>true</c>, includes the user's department in the token.</summary>
        [JsonPropertyName("ext_department")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtDepartment { get; set; }

        /// <summary>When <c>true</c>, includes the user's assigned roles in the token.</summary>
        [JsonPropertyName("ext_roles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtRoles { get; set; }

        /// <summary>When <c>true</c>, includes the user's physical delivery office name in the token.</summary>
        [JsonPropertyName("ext_physical_delivery_office_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtPhysicalDeliveryOfficeName { get; set; }

        /// <summary>When <c>true</c>, includes the user's preferred language in the token.</summary>
        [JsonPropertyName("ext_preferred_language")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtPreferredLanguage { get; set; }

        /// <summary>When <c>true</c>, includes the Azure AD object ID in the token.</summary>
        [JsonPropertyName("ext_azure_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAzureId { get; set; }

        /// <summary>When <c>true</c>, includes the Azure AD OID claim in the token.</summary>
        [JsonPropertyName("ext_oid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtOid { get; set; }

        /// <summary>When <c>true</c>, includes the user's UPN (User Principal Name) in the token.</summary>
        [JsonPropertyName("ext_upn")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtUpn { get; set; }

        /// <summary>When <c>true</c>, includes the user's tenant ID in the token.</summary>
        [JsonPropertyName("ext_tenantid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtTenantid { get; set; }

        /// <summary>When <c>true</c>, includes the user's usage location in the token.</summary>
        [JsonPropertyName("ext_usage_location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtUsageLocation { get; set; }

        /// <summary>When <c>true</c>, includes whether the user account is enabled in the token.</summary>
        [JsonPropertyName("ext_account_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAccountEnabled { get; set; }

        /// <summary>When <c>true</c>, includes the user's assigned plans in the token.</summary>
        [JsonPropertyName("ext_assigned_plans")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAssignedPlans { get; set; }

        /// <summary>When <c>true</c>, includes the user's assigned licenses in the token.</summary>
        [JsonPropertyName("ext_assigned_licenses")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAssignedLicenses { get; set; }

        /// <summary>When <c>true</c>, includes the user's provisioned plans in the token.</summary>
        [JsonPropertyName("ext_provisioned_plans")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtProvisionedPlans { get; set; }

        /// <summary>When <c>true</c>, includes the user's provisioning errors in the token.</summary>
        [JsonPropertyName("ext_provisioning_errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtProvisioningErrors { get; set; }

        /// <summary>When <c>true</c>, includes whether directory sync is enabled for the user in the token.</summary>
        [JsonPropertyName("ext_dir_sync_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtDirSyncEnabled { get; set; }

        /// <summary>When <c>true</c>, includes the last directory sync timestamp in the token.</summary>
        [JsonPropertyName("ext_last_sync")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtLastSync { get; set; }

        /// <summary>When <c>true</c>, includes the user's proxy addresses in the token.</summary>
        [JsonPropertyName("ext_proxy_addresses")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtProxyAddresses { get; set; }

        /// <summary>When <c>true</c>, includes the upstream access token in the token.</summary>
        [JsonPropertyName("ext_access_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAccessToken { get; set; }

        /// <summary>When <c>true</c>, includes the upstream refresh token in the token.</summary>
        [JsonPropertyName("ext_refresh_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtRefreshToken { get; set; }

        /// <summary>When <c>true</c>, includes the upstream token expiry in the token.</summary>
        [JsonPropertyName("ext_expires_in")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtExpiresIn { get; set; }

        /// <summary>When <c>true</c>, includes the user's telephone number in the token.</summary>
        [JsonPropertyName("ext_telephoneNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtTelephoneNumber { get; set; }

        /// <summary>When <c>true</c>, includes the user's PUID (Passport Unique Identifier) in the token.</summary>
        [JsonPropertyName("ext_puid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtPuid { get; set; }

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

    }

}
