using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration for the phone number attribute on a database connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsPhoneNumberAttribute
    {

        /// <summary>
        /// Signup status and verification settings for the phone number attribute.
        /// </summary>
        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPhoneNumberSignup? Signup { get; set; }


    }

}
