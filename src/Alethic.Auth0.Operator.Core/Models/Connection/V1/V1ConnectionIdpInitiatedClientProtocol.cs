using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Protocol used when Auth0 acts as the service provider in an IdP-initiated SSO flow.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionIdpInitiatedClientProtocol
    {

        /// <summary>OpenID Connect protocol.</summary>
        [JsonStringEnumMemberName("oidc")]
        Oidc,

        /// <summary>SAML 2.0 protocol.</summary>
        [JsonStringEnumMemberName("samlp")]
        Samlp,

        /// <summary>WS-Federation protocol.</summary>
        [JsonStringEnumMemberName("wsfed")]
        WsFed,

    }

}
