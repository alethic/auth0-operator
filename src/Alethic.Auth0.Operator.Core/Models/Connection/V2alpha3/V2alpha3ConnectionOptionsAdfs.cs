using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsAdfs
{

    [JsonPropertyName("adfsServer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdfsServer { get; set; }

    [JsonPropertyName("domainAliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DomainAliases { get; set; }

    [JsonPropertyName("entityId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntityId { get; set; }

    [JsonPropertyName("fedMetadataXml")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FedMetadataXml { get; set; }

    [JsonPropertyName("iconUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("prevThumbprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? PrevThumbprints { get; set; }

    [JsonPropertyName("setUserRootAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("shouldTrustEmailVerifiedConnection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum? ShouldTrustEmailVerifiedConnection { get; set; }

    [JsonPropertyName("signInEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SignInEndpoint { get; set; }

    [JsonPropertyName("tenantDomain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantDomain { get; set; }

    [JsonPropertyName("thumbprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Thumbprints { get; set; }

    [JsonPropertyName("upstreamParams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("userIdAttribute")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserIdAttribute { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
