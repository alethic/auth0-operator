using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3CustomDomainCertificateProvisioning
    {

        [JsonStringEnumMemberName("auth0ManagedCerts")]
        Auth0ManagedCertificate,

        [JsonStringEnumMemberName("selfManagedCerts")]
        SelfManagedCertificate

    }

}
