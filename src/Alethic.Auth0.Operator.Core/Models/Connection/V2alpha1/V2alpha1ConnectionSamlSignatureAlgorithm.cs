using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// XML signature algorithm used when signing SAML requests or validating SAML responses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha1ConnectionSamlSignatureAlgorithm
    {

        /// <summary>RSA with SHA-1.</summary>
        [JsonStringEnumMemberName("rsa-sha1")]
        RsaSha1,

        /// <summary>RSA with SHA-256.</summary>
        [JsonStringEnumMemberName("rsa-sha256")]
        RsaSha256,

    }

}
