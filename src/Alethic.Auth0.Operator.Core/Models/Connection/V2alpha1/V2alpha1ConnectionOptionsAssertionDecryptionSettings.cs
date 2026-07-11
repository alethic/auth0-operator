using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Settings that control decryption of incoming SAML assertions.
    /// </summary>
    public record V2alpha1ConnectionOptionsAssertionDecryptionSettings
    {

        /// <summary>
        /// When <c>true</c>, Auth0 will attempt to decrypt incoming SAML assertions.
        /// </summary>
        [JsonPropertyName("decryptAssertion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DecryptAssertion { get; set; }

        /// <summary>
        /// Algorithm profile used to decrypt the assertion content.
        /// </summary>
        [JsonPropertyName("decryptionAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionAssertionDecryptionAlgorithmProfile? DecryptionAlgorithm { get; set; }

        /// <summary>
        /// Algorithm used to decrypt the encrypted key in the assertion (e.g. <c>rsa-oaep-mgf1p</c>, <c>rsa1_5</c>).
        /// </summary>
        [JsonPropertyName("keyEncryptionAlgorithm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? KeyEncryptionAlgorithm { get; set; }

    }

}
