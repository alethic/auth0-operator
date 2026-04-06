using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha1TenantCharset
    {

        [JsonStringEnumMemberName("base20")]
        Base20,

        [JsonStringEnumMemberName("digits")]
        Digits

    }

}
