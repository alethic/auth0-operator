using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3TenantCspPolicyModeEnum
{

    [JsonStringEnumMemberName("enforcing")]
    Enforcing,

    [JsonStringEnumMemberName("reporting")]
    Reporting

}
