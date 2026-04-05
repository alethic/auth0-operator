using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain.V1alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1alpha1CustomDomainVerificationMethod
    {

        [JsonStringEnumMemberName("txt")]
        TXT,

        [JsonStringEnumMemberName("cname")]
        CNAME,

    }

}
