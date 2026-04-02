using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CustomDomainCertificateProvisioning
    {

        [JsonStringEnumMemberName("auth0_managed_certs")]
        Auth0ManagedCertificate,

        [JsonStringEnumMemberName("digself_managed_certsits")]
        SelfManagedCertificate

    }

}
