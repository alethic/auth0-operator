using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Settings that control decryption of incoming SAML assertions.
    /// </summary>
    public record V1ConnectionOptionsAssertionDecryptionSettings
    {

        /// <summary>
        /// When <c>true</c>, Auth0 will attempt to decrypt incoming SAML assertions.
        /// </summary>
        [JsonPropertyName("decryptAssertion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DecryptAssertion { get; set; }

        /// <summary>
        /// Symmetric algorithm used to decrypt the assertion content (e.g. <c>aes128-cbc</c>, <c>aes256-cbc</c>).
        /// </summary>
        [JsonPropertyName("decryptionAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DecryptionAlgorithm { get; set; }

        /// <summary>
        /// Algorithm used to decrypt the encrypted key in the assertion (e.g. <c>rsa-oaep-mgf1p</c>, <c>rsa1_5</c>).
        /// </summary>
        [JsonPropertyName("keyEncryptionAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? KeyEncryptionAlgorithm { get; set; }

    }

}
