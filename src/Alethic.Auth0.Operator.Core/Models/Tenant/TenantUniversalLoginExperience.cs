using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TenantUniversalLoginExperience
    {

        [JsonStringEnumMemberName("new")]
        New,

        [JsonStringEnumMemberName("classic")]
        Classic

    }

}