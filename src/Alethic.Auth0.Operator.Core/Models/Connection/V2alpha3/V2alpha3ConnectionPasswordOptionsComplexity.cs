using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionPasswordOptionsComplexity
{

    [JsonPropertyName("minLength")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinLength { get; set; }

    [JsonPropertyName("characterTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordCharacterTypeEnum[]? CharacterTypes { get; set; }

    [JsonPropertyName("characterTypeRule")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordCharacterTypeRulePolicyEnum? CharacterTypeRule { get; set; }

    [JsonPropertyName("identicalCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordIdenticalCharactersPolicyEnum? IdenticalCharacters { get; set; }

    [JsonPropertyName("sequentialCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordSequentialCharactersPolicyEnum? SequentialCharacters { get; set; }

    [JsonPropertyName("maxLengthExceeded")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordMaxLengthExceededPolicyEnum? MaxLengthExceeded { get; set; }

}
