using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientOidcBackchannelLogoutInitiators
{

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum? Mode { get; set; }

    [JsonPropertyName("selectedInitiators")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum[]? SelectedInitiators { get; set; }

}
