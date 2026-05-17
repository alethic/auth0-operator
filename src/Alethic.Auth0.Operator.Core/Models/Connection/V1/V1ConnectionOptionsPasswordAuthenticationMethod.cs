using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Enables or disables password-based authentication for the connection.
    /// </summary>
    public record V1ConnectionOptionsPasswordAuthenticationMethod
    {

        /// <summary>
        /// When <c>true</c>, password authentication is allowed for this connection.
        /// </summary>
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enabled { get; set; }

    }

}