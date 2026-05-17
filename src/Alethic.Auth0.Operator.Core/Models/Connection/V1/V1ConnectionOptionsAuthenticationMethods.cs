using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Specifies which authentication methods are enabled for the connection.
    /// </summary>
    public record V1ConnectionOptionsAuthenticationMethods
    {

        /// <summary>
        /// Password authentication method configuration.
        /// </summary>
        [JsonPropertyName("password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordAuthenticationMethod? Password { get; set; }

        /// <summary>
        /// Passkey (WebAuthn) authentication method configuration.
        /// </summary>
        [JsonPropertyName("passkey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasskeyAuthenticationMethod? Passkey { get; set; }

    }

}