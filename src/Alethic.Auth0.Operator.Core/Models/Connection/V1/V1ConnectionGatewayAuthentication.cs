using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Authentication settings used when Auth0 calls a custom SMS or email gateway.
    /// </summary>
    public record V1ConnectionGatewayAuthentication
    {

        /// <summary>
        /// Authentication method to use when calling the gateway. Can be <c>bearer</c> or <c>query</c>.
        /// </summary>
        [JsonPropertyName("method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Method { get; set; }

        /// <summary>
        /// Subject claim for JWT bearer tokens sent to the gateway.
        /// </summary>
        [JsonPropertyName("subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Subject { get; set; }

        /// <summary>
        /// Audience claim for JWT bearer tokens sent to the gateway.
        /// </summary>
        [JsonPropertyName("audience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Audience { get; set; }

        /// <summary>
        /// Shared secret used to sign JWT bearer tokens sent to the gateway.
        /// </summary>
        [JsonPropertyName("secret")]
        public string? Secret { get; set; }

        /// <summary>
        /// When <c>true</c>, the shared secret is Base64-encoded.
        /// </summary>
        [JsonPropertyName("secret_base64_encoded")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SecretBase64Encoded { get; set; }

    }

}
