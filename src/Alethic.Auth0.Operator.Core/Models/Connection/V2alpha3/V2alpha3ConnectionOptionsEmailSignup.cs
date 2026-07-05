using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Signup configuration for the email attribute, controlling its requirement status and verification behaviour.
    /// </summary>
    public record V2alpha3ConnectionOptionsEmailSignup
    {

        /// <summary>
        /// Indicates whether the email attribute is required, optional, or inactive during signup.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAttributeStatus? Status { get; set; }

        /// <summary>
        /// Email verification settings applied after signup.
        /// </summary>
        [JsonPropertyName("verification")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsVerification? Verification { get; set; }

    }

}