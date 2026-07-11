using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models
{

    public record V1TenantReference
    {

        [JsonPropertyName("namespace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Namespace { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public string? Name { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Namespace}/{Name}";
        }

    }

}
