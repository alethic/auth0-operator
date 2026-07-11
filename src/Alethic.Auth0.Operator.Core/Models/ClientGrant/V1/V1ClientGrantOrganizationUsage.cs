using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ClientGrant.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientGrantOrganizationUsage
    {

        [JsonStringEnumMemberName("deny")]
        Deny,

        [JsonStringEnumMemberName("allow")]
        Allow,

        [JsonStringEnumMemberName("require")]
        Require

    }

}
