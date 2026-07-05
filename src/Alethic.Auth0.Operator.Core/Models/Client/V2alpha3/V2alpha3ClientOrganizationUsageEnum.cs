using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientOrganizationUsageEnum
{

    [JsonStringEnumMemberName("deny")]
    Deny,

    [JsonStringEnumMemberName("allow")]
    Allow,

    [JsonStringEnumMemberName("require")]
    Require

}
