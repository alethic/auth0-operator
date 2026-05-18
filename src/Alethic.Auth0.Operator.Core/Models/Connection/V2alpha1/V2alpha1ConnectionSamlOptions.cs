using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for a <c>samlp</c> (SAML Identity Provider) connection strategy.
    /// </summary>
    public record V2alpha1ConnectionSamlOptions
    {

        /// <summary>
        /// SAML single sign-on URL of the identity provider.
        /// </summary>
        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        /// <summary>
        /// SAML single logout URL of the identity provider.
        /// </summary>
        [JsonPropertyName("signOutEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignOutEndpoint { get; set; }

        /// <summary>
        /// When <c>true</c>, Auth0 will not send a logout request to the identity provider on user logout.
        /// </summary>
        [JsonPropertyName("disableSignout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSignout { get; set; }

        /// <summary>
        /// Destination URL included in SAML authentication requests.
        /// </summary>
        [JsonPropertyName("destinationUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DestinationUrl { get; set; }

        /// <summary>
        /// Recipient URL included in SAML authentication requests.
        /// </summary>
        [JsonPropertyName("recipientUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RecipientUrl { get; set; }

        /// <summary>
        /// PEM-encoded X.509 certificate from the identity provider used to verify SAML assertions.
        /// </summary>
        [JsonPropertyName("cert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Cert { get; set; }

        /// <summary>
        /// Certificate thumbprints of the identity provider certificate.
        /// </summary>
        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        /// <summary>
        /// URL pointing to the SAML identity provider metadata document.
        /// </summary>
        [JsonPropertyName("metadataUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MetadataUrl { get; set; }

        /// <summary>
        /// Inline SAML identity provider metadata XML.
        /// </summary>
        [JsonPropertyName("metadataXml")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MetadataXml { get; set; }

        /// <summary>
        /// Entity ID (issuer) of the SAML identity provider.
        /// </summary>
        [JsonPropertyName("entityId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityId { get; set; }

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
        /// Custom SAML request template (Liquid syntax).
        /// </summary>
        [JsonPropertyName("requestTemplate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RequestTemplate { get; set; }

        /// <summary>
        /// When <c>true</c>, additional debug information is included in SAML errors.
        /// </summary>
        [JsonPropertyName("debug")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Debug { get; set; }

        /// <summary>
        /// When <c>true</c>, DEFLATE encoding is used for SAML requests.
        /// </summary>
        [JsonPropertyName("deflate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Deflate { get; set; }

        /// <summary>
        /// Configuration for IdP-initiated SSO login flow.
        /// </summary>
        [JsonPropertyName("idpinitiated")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsIdpinitiated? Idpinitiated { get; set; }

        /// <summary>
        /// PEM-encoded certificate used to sign outgoing SAML requests.
        /// </summary>
        [JsonPropertyName("signingCert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SigningCert { get; set; }

        /// <summary>
        /// Private key used to sign outgoing SAML requests.
        /// </summary>
        [JsonPropertyName("signing_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsKeyPair? SigningKey { get; set; }

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
        /// Mapping of SAML attribute names to Auth0 user profile fields.
        /// </summary>
        [JsonPropertyName("fieldsMap")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string?>? FieldsMap { get; set; }

        /// <summary>
        /// SAML attribute that will be mapped to the Auth0 user ID.
        /// </summary>
        [JsonPropertyName("user_id_attribute")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserIdAttribute { get; set; }

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

        /// <summary>
        /// JWT issuer claim used for global token revocation.
        /// </summary>
        [JsonPropertyName("global_token_revocation_jwt_iss")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? GlobalTokenRevocationJwtIss { get; set; }

        /// <summary>
        /// JWT subject claim used for global token revocation.
        /// </summary>
        [JsonPropertyName("global_token_revocation_jwt_sub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? GlobalTokenRevocationJwtSub { get; set; }

    }

}
