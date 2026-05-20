using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionPasswordOptionsComplexity
{

    [JsonPropertyName("min_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinLength { get; set; }

    [JsonPropertyName("character_types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordCharacterTypeEnum[]? CharacterTypes { get; set; }

    [JsonPropertyName("character_type_rule")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordCharacterTypeRulePolicyEnum? CharacterTypeRule { get; set; }

    [JsonPropertyName("identical_characters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordIdenticalCharactersPolicyEnum? IdenticalCharacters { get; set; }

    [JsonPropertyName("sequential_characters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordSequentialCharactersPolicyEnum? SequentialCharacters { get; set; }

    [JsonPropertyName("max_length_exceeded")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordMaxLengthExceededPolicyEnum? MaxLengthExceeded { get; set; }

}
