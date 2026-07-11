using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionOptionsPasswordDictionary
    {

        [JsonPropertyName("enable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enable { get; set; }

        [JsonPropertyName("dictionary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Dictionary { get; set; }

    }

}