using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Enables or disables passkey (WebAuthn) authentication for the connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsPasskeyAuthenticationMethod
    {

        /// <summary>
        /// When <c>true</c>, passkey authentication is allowed for this connection.
        /// </summary>
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enabled { get; set; }

    }

}