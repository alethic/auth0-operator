using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration for the username attribute on a database connection.
    /// </summary>
    public record V1ConnectionOptionsUsernameAttribute
    {

        /// <summary>
        /// Controls whether the username can be used as a login identifier.
        /// </summary>
        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeIdentifier? Identifier { get; set; }

        /// <summary>
        /// When <c>true</c>, the username is required on the user profile.
        /// </summary>
        [JsonPropertyName("profile_required")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProfileRequired { get; set; }

        /// <summary>
        /// Signup status for the username attribute.
        /// </summary>
        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsUsernameSignup? Signup { get; set; }

        /// <summary>
        /// Length and character-type validation rules for the username.
        /// </summary>
        [JsonPropertyName("validation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeValidation? Validation { get; set; }

    }

}