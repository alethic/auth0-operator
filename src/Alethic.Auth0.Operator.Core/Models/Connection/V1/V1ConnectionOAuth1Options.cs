using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for a generic OAuth 1.0a connection strategy.
    /// </summary>
    public record V1ConnectionOAuth1Options : V1ConnectionOptionsClientCredentials
    {

        /// <summary>
        /// OAuth 1.0a request token URL.
        /// </summary>
        [JsonPropertyName("requestTokenUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RequestTokenUrl { get; set; }

        /// <summary>
        /// OAuth 1.0a access token URL.
        /// </summary>
        [JsonPropertyName("accessTokenUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccessTokenUrl { get; set; }

        /// <summary>
        /// OAuth 1.0a user authorization URL.
        /// </summary>
        [JsonPropertyName("userAuthorizationUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserAuthorizationUrl { get; set; }

        /// <summary>
        /// Signature method for request signing. Can be <c>HMAC-SHA1</c> or <c>RSA-SHA1</c>.
        /// </summary>
        [JsonPropertyName("signatureMethod")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignatureMethod { get; set; }

        /// <summary>
        /// Custom scripts (e.g. <c>fetchUserProfile</c>) keyed by script name.
        /// </summary>
        [JsonPropertyName("scripts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsScripts? Scripts { get; set; }

        /// <summary>
        /// List of user attributes that will not be persisted in the Auth0 user store after each login.
        /// </summary>
        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        /// <summary>
        /// Upstream parameters that will be sent to the identity provider on each authentication request.
        /// </summary>
        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

    }

}
