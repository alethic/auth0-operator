using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>auth0-oidc</c> connection strategy (another Auth0 tenant used as an OIDC identity provider).
    /// </summary>
    public record V2alpha1ConnectionAuth0OidcOptions : V2alpha1ConnectionOptionsClientCredentials
    {

        /// <summary>
        /// Authorization endpoint URL of the Auth0 OIDC identity provider.
        /// </summary>
        [JsonPropertyName("authorizationEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Token endpoint URL of the Auth0 OIDC identity provider.
        /// </summary>
        [JsonPropertyName("tokenEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpoint { get; set; }

        /// <summary>
        /// UserInfo endpoint URL of the Auth0 OIDC identity provider.
        /// </summary>
        [JsonPropertyName("userinfoEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserinfoEndpoint { get; set; }

        /// <summary>
        /// JWKS URI of the Auth0 OIDC identity provider used to verify ID token signatures.
        /// </summary>
        [JsonPropertyName("jwksUri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? JwksUri { get; set; }

        /// <summary>
        /// OpenID Connect discovery document URL. When provided, other endpoint fields are populated automatically.
        /// </summary>
        [JsonPropertyName("discoveryUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DiscoveryUrl { get; set; }

        /// <summary>
        /// Issuer identifier of the Auth0 OIDC identity provider.
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
        [JsonPropertyName("tokenEndpointAuthMethod")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenEndpointAuthMethod { get; set; }

        /// <summary>
        /// Connection type identifier.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }

        /// <summary>
        /// Additional connection settings passed to the identity provider.
        /// </summary>
        [JsonPropertyName("connectionSettings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? ConnectionSettings { get; set; }

        /// <summary>
        /// Mapping of identity provider claims to Auth0 user profile attributes.
        /// </summary>
        [JsonPropertyName("attributeMap")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? AttributeMap { get; set; }

        /// <summary>
        /// Additional OIDC metadata from the discovery document.
        /// </summary>
        [JsonPropertyName("oidcMetadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? OidcMetadata { get; set; }

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
        public V2alpha1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        /// <summary>
        /// Upstream parameters that will be sent to the identity provider on each authentication request.
        /// </summary>
        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V2alpha1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

    }

}
