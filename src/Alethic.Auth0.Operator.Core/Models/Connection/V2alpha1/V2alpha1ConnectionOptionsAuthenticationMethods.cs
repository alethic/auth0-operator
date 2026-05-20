using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Specifies which authentication methods are enabled for the connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsAuthenticationMethods
    {

        /// <summary>
        /// Password authentication method configuration.
        /// </summary>
        [JsonPropertyName("password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPasswordAuthenticationMethod? Password { get; set; }

        /// <summary>
        /// Passkey (WebAuthn) authentication method configuration.
        /// </summary>
        [JsonPropertyName("passkey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPasskeyAuthenticationMethod? Passkey { get; set; }

    }

}