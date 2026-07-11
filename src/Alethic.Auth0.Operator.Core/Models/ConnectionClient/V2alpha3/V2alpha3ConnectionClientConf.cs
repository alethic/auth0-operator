using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.ConnectionClient.V2alpha3
{

    public record V2alpha3ConnectionClientConf
    {

        [JsonPropertyName("connectionRef")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public V1ConnectionReference? ConnectionRef { get; set; }

        [JsonPropertyName("clientRef")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public V1ClientReference? ClientRef { get; set; }

    }

}
