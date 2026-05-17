using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>okta</c> enterprise connection strategy.
    /// </summary>
    public record V1ConnectionOktaOptions : V1ConnectionOptionsClientCredentials
    {

        /// <summary>
        /// Okta domain (e.g. <c>your-org.okta.com</c>).
        /// </summary>
        [JsonPropertyName("domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Domain { get; set; }

        /// <summary>
        /// Authorization endpoint URL of the Okta identity provider.
        /// </summary>
        [JsonPropertyName("authorization_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Token endpoint URL of the Okta identity provider.
        /// </summary>
        [JsonPropertyName("token_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpoint { get; set; }

        /// <summary>
        /// UserInfo endpoint URL of the Okta identity provider.
        /// </summary>
        [JsonPropertyName("userinfo_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserinfoEndpoint { get; set; }

        /// <summary>
        /// JWKS URI of the Okta identity provider used to verify ID token signatures.
        /// </summary>
        [JsonPropertyName("jwks_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? JwksUri { get; set; }

        /// <summary>
        /// Issuer identifier of the Okta identity provider.
        /// </summary>
        [JsonPropertyName("issuer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Issuer { get; set; }

        /// <summary>
        /// Space-separated list of OAuth 2.0 scopes to request.
        /// </summary>
        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        /// <summary>
        /// URL of the icon to display for this connection in the Universal Login experience.
        /// </summary>
        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

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
        /// Token endpoint authentication method. Can be <c>client_secret_basic</c>, <c>client_secret_post</c>, or <c>private_key_jwt</c>.
        /// </summary>
        [JsonPropertyName("token_endpoint_auth_method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointAuthMethod { get; set; }

        /// <summary>
        /// Signing algorithm used for <c>private_key_jwt</c> client assertions at the token endpoint.
        /// </summary>
        [JsonPropertyName("token_endpoint_auth_signing_alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointAuthSigningAlg { get; set; }

        /// <summary>
        /// Audience format for JWT client assertions at the token endpoint.
        /// </summary>
        [JsonPropertyName("token_endpoint_jwtca_aud_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointJwtcaAudFormat { get; set; }

        /// <summary>
        /// Signing algorithm to use for DPoP (Demonstrating Proof-of-Possession) proofs.
        /// </summary>
        [JsonPropertyName("dpop_signing_alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DpopSigningAlg { get; set; }

        /// <summary>
        /// List of accepted signing algorithms for ID tokens issued by this connection.
        /// </summary>
        [JsonPropertyName("id_token_signed_response_algs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? IdTokenSignedResponseAlgs { get; set; }

        /// <summary>
        /// When <c>true</c>, a nonce will be sent in back-channel requests.
        /// </summary>
        [JsonPropertyName("send_back_channel_nonce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SendBackChannelNonce { get; set; }

        /// <summary>
        /// Connection type identifier.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }

        /// <summary>
        /// Additional OIDC metadata from the Okta discovery document.
        /// </summary>
        [JsonPropertyName("oidc_metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string?>? OidcMetadata { get; set; }

        /// <summary>
        /// Mapping of Okta claims to Auth0 user profile attributes.
        /// </summary>
        [JsonPropertyName("attribute_map")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeMap? AttributeMap { get; set; }

        /// <summary>
        /// Additional connection settings passed to Okta.
        /// </summary>
        [JsonPropertyName("connection_settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsConnectionSettings? ConnectionSettings { get; set; }

        /// <summary>
        /// Configuration for federated connection access tokens.
        /// </summary>
        [JsonPropertyName("federated_connections_access_tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsFederatedConnectionsAccessTokens? FederatedConnectionsAccessTokens { get; set; }

        /// <summary>
        /// List of user attributes that will not be persisted in the Auth0 user store after each login.
        /// </summary>
        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        /// <summary>
        /// Controls when root profile attributes (<c>name</c>, <c>given_name</c>, etc.) are updated from the identity provider.
        /// </summary>
        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        /// <summary>
        /// Upstream parameters that will be sent to the identity provider on each authentication request.
        /// </summary>
        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

    }

}
