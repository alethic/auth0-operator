using System;
using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>google-apps</c> (Google Workspace) enterprise connection strategy.
    /// Each boolean property enables the corresponding Google API OAuth scope.
    /// </summary>
    public record V1ConnectionGoogleAppsOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Domain { get; set; }

        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Email { get; set; }

        [JsonPropertyName("profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Profile { get; set; }

        [JsonPropertyName("gmail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Gmail { get; set; }

        [JsonPropertyName("calendar")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Calendar { get; set; }

        [JsonPropertyName("admin_directory_user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminDirectoryUser { get; set; }

        [JsonPropertyName("admin_directory_user_readonly")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminDirectoryUserReadonly { get; set; }

        [JsonPropertyName("admin_directory_group")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminDirectoryGroup { get; set; }

        [JsonPropertyName("admin_directory_group_readonly")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminDirectoryGroupReadonly { get; set; }

        [JsonPropertyName("google_plus")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? GooglePlus { get; set; }

        [JsonPropertyName("api_enable_users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ApiEnableUsers { get; set; }

        [JsonPropertyName("userinfoEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserinfoEndpoint { get; set; }

        [JsonPropertyName("allowed_audiences")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedAudiences { get; set; }

        [JsonPropertyName("map_user_id_to_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? MapUserIdToId { get; set; }

        [JsonPropertyName("basic_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BasicProfile { get; set; }

        [JsonPropertyName("should_trust_email_verified_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShouldTrustEmailVerifiedConnection { get; set; }

        [JsonPropertyName("idpinitiated")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Idpinitiated { get; set; }

        [JsonPropertyName("admin_access_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdminAccessToken { get; set; }

        [JsonPropertyName("admin_access_token_expiresin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? AdminAccessTokenExpiresin { get; set; }

        [JsonPropertyName("admin_refresh_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdminRefreshToken { get; set; }

        [JsonPropertyName("allow_setting_login_scopes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowSettingLoginScopes { get; set; }

        [JsonPropertyName("api_enable_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ApiEnableGroups { get; set; }

        [JsonPropertyName("ext_agreed_terms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAgreedTerms { get; set; }

        [JsonPropertyName("ext_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGroups { get; set; }

        [JsonPropertyName("ext_groups_extended")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGroupsExtended { get; set; }

        [JsonPropertyName("ext_is_admin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtIsAdmin { get; set; }

        [JsonPropertyName("ext_is_suspended")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtIsSuspended { get; set; }

        [JsonPropertyName("federated_connections_access_tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleAppsFederatedConnectionsAccessTokens? FederatedConnectionsAccessTokens { get; set; }

        [JsonPropertyName("handle_login_from_social")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? HandleLoginFromSocial { get; set; }

    }

}
