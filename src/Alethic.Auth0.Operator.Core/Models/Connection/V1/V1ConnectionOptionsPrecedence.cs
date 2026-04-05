using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionOptionsPrecedence
    {

        [JsonStringEnumMemberName("email")]
        Email,

        [JsonStringEnumMemberName("phone_number")]
        PhoneNumber,

        [JsonStringEnumMemberName("username")]
        UserName

    }

}