using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Algorithm profile used to decrypt incoming SAML assertion content.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha1ConnectionAssertionDecryptionAlgorithmProfile
    {

        /// <summary>The v2026-1 algorithm profile.</summary>
        [JsonStringEnumMemberName("v2026-1")]
        V20261,

    }

}
