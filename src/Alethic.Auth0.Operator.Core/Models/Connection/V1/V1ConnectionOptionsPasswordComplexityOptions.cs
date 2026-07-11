using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionOptionsPasswordComplexityOptions
    {

        [JsonPropertyName("min_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }

    }

}