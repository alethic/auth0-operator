using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Enables or disables passkey (WebAuthn) authentication for the connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsPasskeyAuthenticationMethod
    {

        /// <summary>
        /// When <c>true</c>, passkey authentication is allowed for this connection.
        /// </summary>
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enabled { get; set; }

    }

}