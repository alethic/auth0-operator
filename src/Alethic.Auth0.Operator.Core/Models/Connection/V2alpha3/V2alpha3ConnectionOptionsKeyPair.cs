using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// A PEM-encoded key/certificate pair used for signing or decryption on a SAML connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsKeyPair
    {

        /// <summary>
        /// PEM-encoded private key.
        /// </summary>
        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Key { get; set; }

        /// <summary>
        /// PEM-encoded X.509 certificate.
        /// </summary>
        [JsonPropertyName("cert")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Cert { get; set; }

    }

}
