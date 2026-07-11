using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientOrganizationUsage
    {

        [JsonStringEnumMemberName("deny")]
        Deny,

        [JsonStringEnumMemberName("allow")]
        Allow,

        [JsonStringEnumMemberName("require")]
        Require

    }

}
