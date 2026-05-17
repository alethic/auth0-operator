using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Signup configuration for the email attribute, controlling its requirement status and verification behaviour.
    /// </summary>
    public record V1ConnectionOptionsEmailSignup
    {

        /// <summary>
        /// Indicates whether the email attribute is required, optional, or inactive during signup.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeStatus? Status { get; set; }

        /// <summary>
        /// Email verification settings applied after signup.
        /// </summary>
        [JsonPropertyName("verification")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsVerification? Verification { get; set; }

    }

}