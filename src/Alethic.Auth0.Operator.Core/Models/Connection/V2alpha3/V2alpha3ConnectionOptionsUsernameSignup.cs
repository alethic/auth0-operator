using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Signup configuration for the username attribute, controlling whether it is required, optional, or inactive.
    /// </summary>
    public record V2alpha3ConnectionOptionsUsernameSignup
    {

        /// <summary>
        /// Indicates whether the username attribute is required, optional, or inactive during signup.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAttributeStatus? Status { get; set; }

    }

}