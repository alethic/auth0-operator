using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// SAML2 addon indicator (no configuration settings needed for SAML2 addon).
/// </summary>
public record V1ClientAddonSaml
{

    [JsonPropertyName("mappings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Mappings { get; set; }

    [JsonPropertyName("audience")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Audience { get; set; }

    [JsonPropertyName("recipient")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Recipient { get; set; }

    [JsonPropertyName("createUpnClaim")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CreateUpnClaim { get; set; }

    [JsonPropertyName("mapUnknownClaimsAsIs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MapUnknownClaimsAsIs { get; set; }

    [JsonPropertyName("passthroughClaimsWithNoMapping")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PassthroughClaimsWithNoMapping { get; set; }

    [JsonPropertyName("mapIdentities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MapIdentities { get; set; }

    [JsonPropertyName("signatureAlgorithm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SignatureAlgorithm { get; set; }

    [JsonPropertyName("digestAlgorithm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DigestAlgorithm { get; set; }

    [JsonPropertyName("issuer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Issuer { get; set; }

    [JsonPropertyName("destination")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Destination { get; set; }

    [JsonPropertyName("lifetimeInSeconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LifetimeInSeconds { get; set; }

    [JsonPropertyName("signResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SignResponse { get; set; }

    [JsonPropertyName("nameIdentifierFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NameIdentifierFormat { get; set; }

    [JsonPropertyName("nameIdentifierProbes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? NameIdentifierProbes { get; set; }

    [JsonPropertyName("authnContextClassRef")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthnContextClassRef { get; set; }

}
