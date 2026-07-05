using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ClientGrantOrganizationUsage
    {

        [JsonStringEnumMemberName("deny")]
        Deny,

        [JsonStringEnumMemberName("allow")]
        Allow,

        [JsonStringEnumMemberName("require")]
        Require

    }

}
