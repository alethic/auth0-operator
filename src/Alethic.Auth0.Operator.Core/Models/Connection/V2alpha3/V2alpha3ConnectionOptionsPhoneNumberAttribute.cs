using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Configuration for the phone number attribute on a database connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsPhoneNumberAttribute
    {

        /// <summary>
        /// Signup status and verification settings for the phone number attribute.
        /// </summary>
        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPhoneNumberSignup? Signup { get; set; }


    }

}
