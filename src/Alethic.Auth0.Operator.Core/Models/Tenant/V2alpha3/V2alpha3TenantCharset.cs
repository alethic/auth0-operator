using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3TenantCharset
{

    [JsonStringEnumMemberName("base20")]
    Base20,

    [JsonStringEnumMemberName("digits")]
    Digits

}
