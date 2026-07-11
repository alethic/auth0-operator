using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Configuration for the email address attribute on a database connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsEmailAttribute
    {

        /// <summary>
        /// Controls whether email can be used as a login identifier.
        /// </summary>
        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAttributeIdentifier? Identifier { get; set; }

        /// <summary>
        /// When <c>true</c>, the email address is required on the user profile.
        /// </summary>
        [JsonPropertyName("profileRequired")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProfileRequired { get; set; }

        /// <summary>
        /// Signup status and verification settings for the email attribute.
        /// </summary>
        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsEmailSignup? Signup { get; set; }

    }

}
