using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{
    /// <summary>
    /// Authentication parameters appended to the magic link URL (e.g. scope, response_type).
    /// </summary>
    public record V2alpha1ConnectionEmailAuthParams
    {

        /// <summary>
        /// OAuth 2.0 scope requested in the magic link.
        /// </summary>
        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        /// <summary>
        /// OAuth 2.0 response type requested in the magic link (e.g. <c>"token"</c> or <c>"code"</c>).
        /// </summary>
        [JsonPropertyName("response_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ResponseType { get; set; }

    }

}
