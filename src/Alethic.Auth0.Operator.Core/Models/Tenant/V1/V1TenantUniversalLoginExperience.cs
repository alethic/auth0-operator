using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1TenantUniversalLoginExperience
    {

        [JsonStringEnumMemberName("new")]
        New,

        [JsonStringEnumMemberName("classic")]
        Classic

    }

}