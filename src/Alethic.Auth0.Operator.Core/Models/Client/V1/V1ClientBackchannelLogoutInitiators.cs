using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public class V1ClientBackchannelLogoutInitiators
    {

        [JsonPropertyName("mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientLogoutInitiatorModes? Mode { get; set; }

        [JsonPropertyName("selected_initiators")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientLogoutInitiators[]? SelectedInitiators { get; set; }

    }

}
