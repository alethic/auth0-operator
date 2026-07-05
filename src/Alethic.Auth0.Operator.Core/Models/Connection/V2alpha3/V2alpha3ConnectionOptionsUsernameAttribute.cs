using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Configuration for the username attribute on a database connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsUsernameAttribute
    {

        /// <summary>
        /// Controls whether the username can be used as a login identifier.
        /// </summary>
        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAttributeIdentifier? Identifier { get; set; }

        /// <summary>
        /// When <c>true</c>, the username is required on the user profile.
        /// </summary>
        [JsonPropertyName("profileRequired")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProfileRequired { get; set; }

        /// <summary>
        /// Signup status for the username attribute.
        /// </summary>
        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsUsernameSignup? Signup { get; set; }

        /// <summary>
        /// Length and character-type validation rules for the username.
        /// </summary>
        [JsonPropertyName("validation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAttributeValidation? Validation { get; set; }

    }

}