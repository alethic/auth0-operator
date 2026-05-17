using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionPingFederateOptions
    {

        [JsonPropertyName("pingFederateBaseUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PingFederateBaseUrl { get; set; }

        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        [JsonPropertyName("entityId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityId { get; set; }

        [JsonPropertyName("cert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Cert { get; set; }

        [JsonPropertyName("signingCert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SigningCert { get; set; }

        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        [JsonPropertyName("signatureAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignatureAlgorithm { get; set; }

        [JsonPropertyName("digestAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DigestAlgorithm { get; set; }

        [JsonPropertyName("signSAMLRequest")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SignSamlRequest { get; set; }

        [JsonPropertyName("protocolBinding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProtocolBinding { get; set; }

        [JsonPropertyName("idpinitiated")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Idpinitiated { get; set; }

        [JsonPropertyName("decryptionKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? DecryptionKey { get; set; }

        [JsonPropertyName("assertion_decryption_settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? AssertionDecryptionSettings { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

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
