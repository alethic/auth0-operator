using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>pingfederate</c> enterprise connection strategy.
    /// </summary>
    public record V2alpha1ConnectionPingFederateOptions
    {

        /// <summary>
        /// Base URL of the PingFederate server (e.g. <c>https://ping.example.com</c>).
        /// </summary>
        [JsonPropertyName("pingFederateBaseUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PingFederateBaseUrl { get; set; }

        /// <summary>
        /// SAML sign-in endpoint URL of the PingFederate server.
        /// </summary>
        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        /// <summary>
        /// Entity ID (issuer) of the PingFederate identity provider.
        /// </summary>
        [JsonPropertyName("entityId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityId { get; set; }

        /// <summary>
        /// PEM-encoded X.509 certificate used to verify SAML assertions from PingFederate.
        /// </summary>
        [JsonPropertyName("cert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Cert { get; set; }

        /// <summary>
        /// PEM-encoded certificate used by Auth0 to sign outgoing SAML requests.
        /// </summary>
        [JsonPropertyName("signingCert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SigningCert { get; set; }

        /// <summary>
        /// Certificate thumbprints of the PingFederate identity provider certificate.
        /// </summary>
        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        /// <summary>
        /// Algorithm used to verify SAML assertion signatures.
        /// </summary>
        [JsonPropertyName("signatureAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionSamlSignatureAlgorithm? SignatureAlgorithm { get; set; }

        /// <summary>
        /// Digest algorithm used when signing SAML requests.
        /// </summary>
        [JsonPropertyName("digestAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionSamlDigestAlgorithm? DigestAlgorithm { get; set; }

        /// <summary>
        /// When <c>true</c>, Auth0 will sign outgoing SAML authentication requests.
        /// </summary>
        [JsonPropertyName("signSAMLRequest")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SignSamlRequest { get; set; }

        /// <summary>
        /// SAML protocol binding to use.
        /// </summary>
        [JsonPropertyName("protocolBinding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionSamlProtocolBinding? ProtocolBinding { get; set; }

        /// <summary>
        /// Configuration for IdP-initiated SSO login flow.
        /// </summary>
        [JsonPropertyName("idpinitiated")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsIdpinitiated? Idpinitiated { get; set; }

        /// <summary>
        /// Private key used to decrypt incoming SAML assertions.
        /// </summary>
        [JsonPropertyName("decryptionKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsKeyPair? DecryptionKey { get; set; }

        /// <summary>
        /// Settings for decryption of SAML assertion content.
        /// </summary>
        [JsonPropertyName("assertion_decryption_settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsAssertionDecryptionSettings? AssertionDecryptionSettings { get; set; }

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
