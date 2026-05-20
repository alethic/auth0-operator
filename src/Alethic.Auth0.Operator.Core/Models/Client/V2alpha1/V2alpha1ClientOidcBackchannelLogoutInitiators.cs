using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientOidcBackchannelLogoutInitiators
{

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum? Mode { get; set; }

    [JsonPropertyName("selected_initiators")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum[]? SelectedInitiators { get; set; }

}
