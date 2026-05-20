using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1TenantSessionCookieModeEnum
{

    [JsonStringEnumMemberName("persistent")]
    Persistent,

    [JsonStringEnumMemberName("non_persistent")]
    NonPersistent

}
