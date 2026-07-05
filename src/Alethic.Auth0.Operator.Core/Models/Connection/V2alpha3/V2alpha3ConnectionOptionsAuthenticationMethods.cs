using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Specifies which authentication methods are enabled for the connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsAuthenticationMethods
    {

        /// <summary>
        /// Password authentication method configuration.
        /// </summary>
        [JsonPropertyName("password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPasswordAuthenticationMethod? Password { get; set; }

        /// <summary>
        /// Passkey (WebAuthn) authentication method configuration.
        /// </summary>
        [JsonPropertyName("passkey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPasskeyAuthenticationMethod? Passkey { get; set; }

    }

}