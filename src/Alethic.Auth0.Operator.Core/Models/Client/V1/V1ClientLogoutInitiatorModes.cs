using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientLogoutInitiatorModes
    {

        [JsonStringEnumMemberName("all")]
        All,

        [JsonStringEnumMemberName("custom")]
        Custom

    }

}
