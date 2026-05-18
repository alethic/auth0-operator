using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    public class V2alpha1ConnectionPasswordOptionsComplexity
    {

        /// <summary>
        /// Minimum password length required (1-72 characters)
        /// </summary>
        [JsonPropertyName("min_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }

        /// <summary>
        /// Required character types that must be present in passwords. Valid options: uppercase, lowercase, number, special
        /// </summary>
        [JsonPropertyName("character_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionPasswordCharacterType[]? CharacterTypes { get; set; }

        [JsonPropertyName("character_type_rule")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionPasswordCharacterTypeRulePolicy? CharacterTypeRule { get; set; }

        [JsonPropertyName("identical_characters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionPasswordIdenticalCharactersPolicy? IdenticalCharacters { get; set; }

        [JsonPropertyName("sequential_characters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionPasswordSequentialCharactersPolicy? SequentialCharacters { get; set; }

        [JsonPropertyName("max_length_exceeded")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionPasswordMaxLengthExceededPolicy? MaxLengthExceeded { get; set; }

    }

}
