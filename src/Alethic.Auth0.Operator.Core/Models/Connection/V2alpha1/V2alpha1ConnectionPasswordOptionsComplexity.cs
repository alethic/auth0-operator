using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
public record V2alpha1ConnectionPasswordOptionsComplexity
{
    [JsonPropertyName("min_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinLength { get; set; }

    [JsonPropertyName("character_types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1PasswordCharacterTypeEnum[]? CharacterTypes { get; set; }

    [JsonPropertyName("character_type_rule")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1PasswordCharacterTypeRulePolicyEnum? CharacterTypeRule { get; set; }

    [JsonPropertyName("identical_characters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1PasswordIdenticalCharactersPolicyEnum? IdenticalCharacters { get; set; }

    [JsonPropertyName("sequential_characters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1PasswordSequentialCharactersPolicyEnum? SequentialCharacters { get; set; }

    [JsonPropertyName("max_length_exceeded")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1PasswordMaxLengthExceededPolicyEnum? MaxLengthExceeded { get; set; }
}
