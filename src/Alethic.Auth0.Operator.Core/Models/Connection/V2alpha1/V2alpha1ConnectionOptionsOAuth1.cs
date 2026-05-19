using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsOAuth1
{

    [JsonPropertyName("accessTokenURL")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessTokenUrl { get; set; }

    [JsonPropertyName("client_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("requestTokenURL")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RequestTokenUrl { get; set; }

    [JsonPropertyName("scripts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionScriptsOAuth1? Scripts { get; set; }

    [JsonPropertyName("signatureMethod")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSignatureMethodOAuth1? SignatureMethod { get; set; }

    [JsonPropertyName("upstream_params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha1ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("userAuthorizationURL")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserAuthorizationUrl { get; set; }

    [JsonPropertyName("non_persistent_attrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
