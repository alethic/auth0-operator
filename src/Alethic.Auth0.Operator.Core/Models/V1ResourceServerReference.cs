using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models
{

    public record V1ResourceServerReference
    {

        [JsonPropertyName("namespace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Namespace { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identifier { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Id is not null ? Id : $"{Namespace}/{Name}";
        }

    }

}
