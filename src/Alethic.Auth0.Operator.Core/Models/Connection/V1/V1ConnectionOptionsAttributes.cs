using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration for user identifier attributes (email, phone number, and username) supported by the connection.
    /// </summary>
    public record V1ConnectionOptionsAttributes
    {

        /// <summary>
        /// Email attribute configuration, including identifier and signup settings.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsEmailAttribute? Email { get; set; }

        /// <summary>
        /// Phone number attribute configuration, including signup settings.
        /// </summary>
        [JsonPropertyName("phone_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPhoneNumberAttribute? PhoneNumber { get; set; }

        /// <summary>
        /// Username attribute configuration, including identifier, profile requirement, signup status, and validation rules.
        /// </summary>
        [JsonPropertyName("username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsUsernameAttribute? Username { get; set; }

    }

}