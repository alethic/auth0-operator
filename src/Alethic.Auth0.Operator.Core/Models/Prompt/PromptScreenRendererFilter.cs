using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    public class PromptScreenRendererFilter
    {

        [JsonPropertyName("match_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PromptScreenRendererFilterMatchType? MatchType { get; set; }

        [JsonPropertyName("clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? Clients { get; set; }

        [JsonPropertyName("domains")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1CustomDomainReference[]? Domains { get; set; }

    }

}