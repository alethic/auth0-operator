using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Restricts the character types permitted in a username attribute value.
    /// </summary>
    public record V2alpha3ConnectionOptionsAttributeAllowedTypes
    {

        /// <summary>
        /// When <c>true</c>, the attribute value may be an email address.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Email { get; set; }

        /// <summary>
        /// When <c>true</c>, the attribute value may be a phone number.
        /// </summary>
        [JsonPropertyName("phoneNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PhoneNumber { get; set; }

    }

}
