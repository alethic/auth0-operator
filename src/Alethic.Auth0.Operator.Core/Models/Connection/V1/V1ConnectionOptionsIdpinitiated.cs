using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration for IdP-initiated SSO login flow on a SAML connection.
    /// </summary>
    public record V1ConnectionOptionsIdpinitiated
    {

        /// <summary>
        /// Client ID of the application to which the user is redirected after IdP-initiated login.
        /// </summary>
        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientId { get; set; }

        /// <summary>
        /// Protocol to use for the IdP-initiated callback. Can be <c>oauth2</c> or <c>samlp</c>.
        /// </summary>
        [JsonPropertyName("client_protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientProtocol { get; set; }

        /// <summary>
        /// Additional query string parameters to append to the IdP-initiated authorization request.
        /// </summary>
        [JsonPropertyName("client_authorizequery")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientAuthorizequery { get; set; }

    }

}
