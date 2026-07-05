using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Additional connection-level settings for an OIDC connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsConnectionSettings
    {

        /// <summary>
        /// PKCE (Proof Key for Code Exchange) configuration. Can be <c>auto</c>, <c>disabled</c>, or <c>forced</c>.
        /// </summary>
        [JsonPropertyName("pkce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Pkce { get; set; }

    }

}
