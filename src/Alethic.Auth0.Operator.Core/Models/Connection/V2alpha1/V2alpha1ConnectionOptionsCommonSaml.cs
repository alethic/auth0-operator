using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsCommonSaml
{

    [JsonPropertyName("assertion_decryption_settings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionAssertionDecryptionSettings? AssertionDecryptionSettings { get; set; }

    [JsonPropertyName("cert")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Cert { get; set; }

    [JsonPropertyName("decryptionKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionDecryptionKeySaml? DecryptionKey { get; set; }

    [JsonPropertyName("digestAlgorithm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionDigestAlgorithmEnumSaml? DigestAlgorithm { get; set; }

    [JsonPropertyName("domain_aliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DomainAliases { get; set; }

    [JsonPropertyName("entityId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntityId { get; set; }

    [JsonPropertyName("icon_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("idpinitiated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionOptionsIdpinitiatedSaml? Idpinitiated { get; set; }

    [JsonPropertyName("protocolBinding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionProtocolBindingEnumSaml? ProtocolBinding { get; set; }

    [JsonPropertyName("set_user_root_attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("signInEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SignInEndpoint { get; set; }

    [JsonPropertyName("signSAMLRequest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SignSamlRequest { get; set; }

    [JsonPropertyName("signatureAlgorithm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSignatureAlgorithmEnumSaml? SignatureAlgorithm { get; set; }

    [JsonPropertyName("tenant_domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantDomain { get; set; }

    [JsonPropertyName("thumbprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Thumbprints { get; set; }

    [JsonPropertyName("upstream_params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha1ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

}
