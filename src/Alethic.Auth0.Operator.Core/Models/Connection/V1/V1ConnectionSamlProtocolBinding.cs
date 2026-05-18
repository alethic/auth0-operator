using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// SAML protocol binding used for authentication requests and responses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionSamlProtocolBinding
    {

        /// <summary>HTTP POST binding.</summary>
        [JsonStringEnumMemberName("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST")]
        HttpPost,

        /// <summary>HTTP Redirect binding.</summary>
        [JsonStringEnumMemberName("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect")]
        HttpRedirect,

    }

}
