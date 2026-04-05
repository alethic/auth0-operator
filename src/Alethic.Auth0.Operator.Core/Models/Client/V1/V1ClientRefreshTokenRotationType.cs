using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientRefreshTokenRotationType
    {

        [JsonStringEnumMemberName("rotating")]
        Rotating,

        [JsonStringEnumMemberName("non-rotating")]
        NonRotating

    }

}
