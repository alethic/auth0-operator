using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// MFA configuration for a database connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsMfa
    {

        /// <summary>
        /// When <c>true</c>, MFA is active for the connection.
        /// </summary>
        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

        /// <summary>
        /// When <c>true</c>, enrollment settings are returned to the client during authentication.
        /// </summary>
        [JsonPropertyName("returnEnrollSettings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReturnEnrollSettings { get; set; }

    }

}
