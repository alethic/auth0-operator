using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionOidcOptions : V1ConnectionOptionsClientCredentials
    {

        [JsonPropertyName("discovery_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DiscoveryUrl { get; set; }

        [JsonPropertyName("authorization_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AuthorizationEndpoint { get; set; }

        [JsonPropertyName("token_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpoint { get; set; }

        [JsonPropertyName("userinfo_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserinfoEndpoint { get; set; }

        [JsonPropertyName("jwks_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? JwksUri { get; set; }

        [JsonPropertyName("issuer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Issuer { get; set; }

        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        [JsonPropertyName("token_endpoint_auth_method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointAuthMethod { get; set; }

        [JsonPropertyName("token_endpoint_auth_signing_alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointAuthSigningAlg { get; set; }

        [JsonPropertyName("token_endpoint_jwtca_aud_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointJwtcaAudFormat { get; set; }

        [JsonPropertyName("dpop_signing_alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DpopSigningAlg { get; set; }

        [JsonPropertyName("id_token_signed_response_algs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? IdTokenSignedResponseAlgs { get; set; }

        [JsonPropertyName("send_back_channel_nonce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SendBackChannelNonce { get; set; }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }

        [JsonPropertyName("oidc_metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? OidcMetadata { get; set; }

        [JsonPropertyName("attribute_map")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? AttributeMap { get; set; }

        [JsonPropertyName("connection_settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? ConnectionSettings { get; set; }

        [JsonPropertyName("federated_connections_access_tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? FederatedConnectionsAccessTokens { get; set; }

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? UpstreamParams { get; set; }

    }

}
