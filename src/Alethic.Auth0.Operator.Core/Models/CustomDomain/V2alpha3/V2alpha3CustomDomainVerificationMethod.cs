using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3CustomDomainVerificationMethod
    {

        [JsonStringEnumMemberName("txt")]
        TXT,

        [JsonStringEnumMemberName("cname")]
        CNAME,

    }

}
