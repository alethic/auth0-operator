using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// XML digest algorithm used when generating SAML message digests.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ConnectionSamlDigestAlgorithm
    {

        /// <summary>SHA-1 digest.</summary>
        [JsonStringEnumMemberName("sha1")]
        Sha1,

        /// <summary>SHA-256 digest.</summary>
        [JsonStringEnumMemberName("sha256")]
        Sha256,

    }

}
