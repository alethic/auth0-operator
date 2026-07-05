using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Validation rules for a user attribute, including length constraints and allowed character types.
    /// </summary>
    public record V2alpha3ConnectionOptionsAttributeValidation
    {

        /// <summary>
        /// Minimum number of characters allowed for the attribute value.
        /// </summary>
        [JsonPropertyName("minLength")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }

        /// <summary>
        /// Maximum number of characters allowed for the attribute value.
        /// </summary>
        [JsonPropertyName("maxLength")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxLength { get; set; }

        /// <summary>
        /// Restricts which character types (email, phone number) are permitted in the attribute value.
        /// </summary>
        [JsonPropertyName("allowedTypes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAttributeAllowedTypes? AllowedTypes { get; set; }

    }

}