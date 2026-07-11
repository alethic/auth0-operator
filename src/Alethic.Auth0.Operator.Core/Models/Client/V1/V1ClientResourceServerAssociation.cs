using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientResourceServerAssociation
    {

        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identifier { get; set; }

        [JsonPropertyName("scopes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Scopes { get; set; }

    }

}
