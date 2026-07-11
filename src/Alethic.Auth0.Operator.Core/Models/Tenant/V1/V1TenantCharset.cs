using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1TenantCharset
    {

        [JsonStringEnumMemberName("base20")]
        Base20,

        [JsonStringEnumMemberName("digits")]
        Digits

    }

}
