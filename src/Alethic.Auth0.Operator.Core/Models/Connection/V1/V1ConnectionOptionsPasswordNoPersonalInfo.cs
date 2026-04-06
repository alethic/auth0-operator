using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionOptionsPasswordNoPersonalInfo
    {

        [JsonPropertyName("enable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enable { get; set; }

    }

}
