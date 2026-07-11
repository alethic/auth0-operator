using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    public record V2alpha3ResourceServerProofOfPossession
    {

        [JsonPropertyName("required")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Required { get; set; }

        [JsonPropertyName("mechanism")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerMechanism? Mechanism { get; set; }

    }

}
