using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3
{

    public record V2alpha3ClientConf
    {

        [JsonPropertyName("signingKeys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientSigningKey[]? SigningKeys { get; set; }

        [JsonPropertyName("appType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientAppTypeEnum? ApplicationType { get; set; }

        [JsonPropertyName("tokenEndpointAuthMethod")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientTokenEndpointAuthMethodEnum? TokenEndpointAuthMethod { get; set; }

        [JsonPropertyName("addons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientAddons? AddOns { get; set; }

        [JsonPropertyName("allowedClients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedClients { get; set; }

        [JsonPropertyName("allowedLogoutUrls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedLogoutUrls { get; set; }

        [JsonPropertyName("allowedOrigins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedOrigins { get; set; }

        [JsonPropertyName("webOrigins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? WebOrigins { get; set; }

        [JsonPropertyName("initiateLoginUri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InitiateLoginUri { get; set; }

        [JsonPropertyName("callbacks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Callbacks { get; set; }

        [JsonPropertyName("skipNonVerifiableCallbackUriConfirmationPrompt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SkipNonVerifiableCallbackUriConfirmationPrompt { get; set; }

        [JsonPropertyName("clientAliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ClientAliases { get; set; }

        [JsonPropertyName("clientMetadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object?>? ClientMetaData { get; set; }

        [JsonPropertyName("customLoginPageOn")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsCustomLoginPageOn { get; set; }

        [JsonPropertyName("isFirstParty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsFirstParty { get; set; }

        [JsonPropertyName("customLoginPage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomLoginPage { get; set; }

        [JsonPropertyName("customLoginPagePreview")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomLoginPagePreview { get; set; }

        [JsonPropertyName("encryptionKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientEncryptionKey? EncryptionKey { get; set; }

        [JsonPropertyName("formTemplate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormTemplate { get; set; }

        [JsonPropertyName("grantTypes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? GrantTypes { get; set; }

        [JsonPropertyName("jwtConfiguration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientJwtConfiguration? JwtConfiguration { get; set; }

        [JsonPropertyName("mobile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientMobile? Mobile { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        [JsonPropertyName("logoUri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUri { get; set; }

        [JsonPropertyName("oidcConformant")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? OidcConformant { get; set; }

        [JsonPropertyName("oidcLogout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientOidcBackchannelLogoutSettings? OidcLogout { get; set; }

        [JsonPropertyName("resourceServers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? ResourceServers { get; set; }

        [JsonPropertyName("sso")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Sso { get; set; }

        [JsonPropertyName("refreshToken")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientRefreshTokenConfiguration? RefreshToken { get; set; }

        [JsonPropertyName("organizationUsage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientOrganizationUsageEnum? OrganizationUsage { get; set; }

        [JsonPropertyName("organizationRequireBehavior")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientOrganizationRequireBehaviorEnum? OrganizationRequireBehavior { get; set; }

        [JsonPropertyName("crossOriginAuthentication")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CrossOriginAuthentication { get; set; }

        [JsonPropertyName("requirePushedAuthorizationRequests")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequirePushedAuthorizationRequests { get; set; }

        [JsonPropertyName("defaultOrganization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientDefaultOrganization? DefaultOrganization { get; set; }

        [JsonPropertyName("complianceLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientComplianceLevelEnum? ComplianceLevel { get; set; }

        [JsonPropertyName("requireProofOfPossession")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequireProofOfPossession { get; set; }

        [JsonPropertyName("fedcmLogin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientFedCmLogin? FedCmLogin { get; set; }

        [JsonPropertyName("identityAssertionAuthorizationGrant")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientIdentityAssertionAuthorizationGrant? IdentityAssertionAuthorizationGrant { get; set; }

        [JsonPropertyName("nativeSocialLogin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientNativeSocialLogin? NativeSocialLogin { get; set; }

    }

}
