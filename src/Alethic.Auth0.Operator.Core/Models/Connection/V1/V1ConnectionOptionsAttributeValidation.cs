using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Validation rules for a user attribute, including length constraints and allowed character types.
    /// </summary>
    public record V1ConnectionOptionsAttributeValidation
    {

        /// <summary>
        /// Minimum number of characters allowed for the attribute value.
        /// </summary>
        [JsonPropertyName("min_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }

        /// <summary>
        /// Maximum number of characters allowed for the attribute value.
        /// </summary>
        [JsonPropertyName("max_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxLength { get; set; }

        /// <summary>
        /// Restricts which character types (email, phone number) are permitted in the attribute value.
        /// </summary>
        [JsonPropertyName("allowed_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeAllowedTypes? AllowedTypes { get; set; }

    }

}