using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models.Organization;

namespace Alethic.Auth0.Operator.Core.Models.Client
{

    public partial class V1ClientConf
    {

        [JsonPropertyName("signing_keys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SigningKey[]? SigningKeys { get; set; }

        [JsonPropertyName("app_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ClientApplicationType? ApplicationType { get; set; }

        [JsonPropertyName("token_endpoint_auth_method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TokenEndpointAuthMethod? TokenEndpointAuthMethod { get; set; }

        [JsonPropertyName("client_authentication_methods")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ClientAuthenticationMethods? ClientAuthenticationMethods { get; set; }

        [JsonPropertyName("signed_request_object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SignedRequestObject? SignedRequestObject { get; set; }

        [JsonPropertyName("addons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Addons? AddOns { get; set; }

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

        [JsonPropertyName("client_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ClientAliases { get; set; }

        [JsonPropertyName("client_metadata")]
        [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Hashtable? ClientMetaData { get; set; }

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
        public EncryptionKey? EncryptionKey { get; set; }

        [JsonPropertyName("form_template")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormTemplate { get; set; }

        [JsonPropertyName("grant_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? GrantTypes { get; set; }

        [JsonPropertyName("jwt_configuration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JwtConfiguration? JwtConfiguration { get; set; }

        [JsonPropertyName("mobile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Mobile? Mobile { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("logo_uri")]
        public string? LogoUri { get; set; }

        [JsonPropertyName("oidc_conformant")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? OidcConformant { get; set; }

        [JsonPropertyName("oidc_logout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OidcLogoutConfig? OidcLogout { get; set; }

        [JsonPropertyName("resource_servers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ClientResourceServerAssociation[]? ResourceServers { get; set; }

        [JsonPropertyName("sso")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Sso { get; set; }

        [JsonPropertyName("refresh_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshToken? RefreshToken { get; set; }

        [JsonPropertyName("organization_usage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrganizationUsage? OrganizationUsage { get; set; }

        [JsonPropertyName("organization_require_behavior")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrganizationRequireBehavior? OrganizationRequireBehavior { get; set; }

        [JsonPropertyName("cross_origin_authentication")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CrossOriginAuthentication { get; set; }

        [JsonPropertyName("require_pushed_authorization_requests")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequirePushedAuthorizationRequests { get; set; }

        [JsonPropertyName("default_organization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DefaultOrganization? DefaultOrganization { get; set; }

        [JsonPropertyName("compliance_level")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComplianceLevel? ComplianceLevel { get; set; }

        [JsonPropertyName("require_proof_of_possession")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequireProofOfPossession { get; set; }

    }

}
