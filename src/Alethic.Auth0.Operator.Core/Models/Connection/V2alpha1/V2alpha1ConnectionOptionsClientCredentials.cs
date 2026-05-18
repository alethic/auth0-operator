using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Common client credentials (<c>client_id</c> / <c>client_secret</c>) shared by many social and enterprise connection option types.
    /// </summary>
    public record V2alpha1ConnectionOptionsClientCredentials : V2alpha1ConnectionOptionsBase
    {

        /// <summary>
        /// OAuth 2.0 client ID registered with the identity provider.
        /// </summary>
        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientId { get; set; }

        /// <summary>
        /// OAuth 2.0 client secret registered with the identity provider.
        /// </summary>
        [JsonPropertyName("client_secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientSecret { get; set; }

    }

}
