using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain.V1alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1alpha1CustomDomainCertificateProvisioning
    {

        [JsonStringEnumMemberName("auth0_managed_certs")]
        Auth0ManagedCertificate,

        [JsonStringEnumMemberName("digself_managed_certsits")]
        SelfManagedCertificate

    }

}
