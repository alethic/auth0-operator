using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientOidcBackchannelLogoutSettings
{

    [JsonPropertyName("backchannelLogoutUrls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? BackchannelLogoutUrls { get; set; }

    [JsonPropertyName("backchannelLogoutInitiators")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientOidcBackchannelLogoutInitiators? BackchannelLogoutInitiators { get; set; }

    [JsonPropertyName("backchannelLogoutSessionMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientOidcBackchannelLogoutSessionMetadata? BackchannelLogoutSessionMetadata { get; set; }

}
