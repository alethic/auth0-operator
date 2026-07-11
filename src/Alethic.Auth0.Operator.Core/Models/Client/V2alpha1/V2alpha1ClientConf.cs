using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1
{

    public record V2alpha1ClientConf
    {

        [JsonPropertyName("signing_keys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientSigningKey[]? SigningKeys { get; set; }

        [JsonPropertyName("app_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientAppTypeEnum? ApplicationType { get; set; }

        [JsonPropertyName("token_endpoint_auth_method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientTokenEndpointAuthMethodEnum? TokenEndpointAuthMethod { get; set; }

        [JsonPropertyName("addons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientAddons? AddOns { get; set; }

        [JsonPropertyName("allowed_clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedClients { get; set; }

        [JsonPropertyName("allowed_logout_urls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedLogoutUrls { get; set; }

        [JsonPropertyName("allowed_origins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedOrigins { get; set; }

        [JsonPropertyName("web_origins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? WebOrigins { get; set; }

        [JsonPropertyName("initiate_login_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InitiateLoginUri { get; set; }

        [JsonPropertyName("callbacks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Callbacks { get; set; }

        [JsonPropertyName("skip_non_verifiable_callback_uri_confirmation_prompt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SkipNonVerifiableCallbackUriConfirmationPrompt { get; set; }

        [JsonPropertyName("client_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ClientAliases { get; set; }

        [JsonPropertyName("client_metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object?>? ClientMetaData { get; set; }

        [JsonPropertyName("custom_login_page_on")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsCustomLoginPageOn { get; set; }

        [JsonPropertyName("is_first_party")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsFirstParty { get; set; }

        [JsonPropertyName("custom_login_page")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomLoginPage { get; set; }

        [JsonPropertyName("custom_login_page_preview")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomLoginPagePreview { get; set; }

        [JsonPropertyName("encryption_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientEncryptionKey? EncryptionKey { get; set; }

        [JsonPropertyName("form_template")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormTemplate { get; set; }

        [JsonPropertyName("grant_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? GrantTypes { get; set; }

        [JsonPropertyName("jwt_configuration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientJwtConfiguration? JwtConfiguration { get; set; }

        [JsonPropertyName("mobile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientMobile? Mobile { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        [JsonPropertyName("logo_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUri { get; set; }

        [JsonPropertyName("oidc_conformant")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? OidcConformant { get; set; }

        [JsonPropertyName("oidc_logout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientOidcBackchannelLogoutSettings? OidcLogout { get; set; }

        [JsonPropertyName("resource_servers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? ResourceServers { get; set; }

        [JsonPropertyName("sso")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Sso { get; set; }

        [JsonPropertyName("refresh_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientRefreshTokenConfiguration? RefreshToken { get; set; }

        [JsonPropertyName("organization_usage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientOrganizationUsageEnum? OrganizationUsage { get; set; }

        [JsonPropertyName("organization_require_behavior")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientOrganizationRequireBehaviorEnum? OrganizationRequireBehavior { get; set; }

        [JsonPropertyName("cross_origin_authentication")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CrossOriginAuthentication { get; set; }

        [JsonPropertyName("require_pushed_authorization_requests")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequirePushedAuthorizationRequests { get; set; }

        [JsonPropertyName("default_organization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientDefaultOrganization? DefaultOrganization { get; set; }

        [JsonPropertyName("compliance_level")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ClientComplianceLevelEnum? ComplianceLevel { get; set; }

        [JsonPropertyName("require_proof_of_possession")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequireProofOfPossession { get; set; }

    }

}
