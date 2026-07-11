using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration for IdP-initiated SSO login flow on a SAML connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsIdpinitiated
    {

        /// <summary>
        /// Client ID of the application to which the user is redirected after IdP-initiated login.
        /// </summary>
        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientId { get; set; }

        /// <summary>
        /// Protocol to use for the IdP-initiated callback.
        /// </summary>
        [JsonPropertyName("client_protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionIdpInitiatedClientProtocol? ClientProtocol { get; set; }

        /// <summary>
        /// Additional query string parameters to append to the IdP-initiated authorization request.
        /// </summary>
        [JsonPropertyName("client_authorizequery")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientAuthorizequery { get; set; }

    }

}
