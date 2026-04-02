using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CustomDomainVerificationMethod
    {

        [JsonStringEnumMemberName("txt")]
        TXT,

    }

}
