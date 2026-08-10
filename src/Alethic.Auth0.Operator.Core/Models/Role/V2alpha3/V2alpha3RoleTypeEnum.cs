using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Role.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3RoleTypeEnum
{

    [JsonStringEnumMemberName("tenant")]
    Tenant,

    [JsonStringEnumMemberName("organization")]
    Organization

}
