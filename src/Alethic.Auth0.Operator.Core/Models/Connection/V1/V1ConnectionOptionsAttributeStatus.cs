using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionOptionsAttributeStatus
    {

        [JsonStringEnumMemberName("required")]
        Required,

        [JsonStringEnumMemberName("optional")]
        Optional,

        [JsonStringEnumMemberName("inactive")]
        Inactive

    }

}