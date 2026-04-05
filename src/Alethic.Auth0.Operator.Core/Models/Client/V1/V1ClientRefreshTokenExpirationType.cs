using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientRefreshTokenExpirationType
    {

        [JsonStringEnumMemberName("expiring")]
        Expiring,

        [JsonStringEnumMemberName("non-expiring")]
        NonExpiring

    }

}
