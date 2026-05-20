using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientOidcBackchannelLogoutSettings
{

    [JsonPropertyName("backchannel_logout_urls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? BackchannelLogoutUrls { get; set; }

    [JsonPropertyName("backchannel_logout_initiators")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientOidcBackchannelLogoutInitiators? BackchannelLogoutInitiators { get; set; }

    [JsonPropertyName("backchannel_logout_session_metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientOidcBackchannelLogoutSessionMetadata? BackchannelLogoutSessionMetadata { get; set; }

}
