using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    public record V1ResourceServerProofOfPossession
    {

        [JsonPropertyName("required")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Required { get; set; }

        [JsonPropertyName("mechanism")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerMechanism? Mechanism { get; set; }

    }

}
