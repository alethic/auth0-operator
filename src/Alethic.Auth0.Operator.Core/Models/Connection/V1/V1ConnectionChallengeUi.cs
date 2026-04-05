using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionChallengeUi
    {

        [JsonStringEnumMemberName("both")]
        Both,

        [JsonStringEnumMemberName("autofill")]
        AutoFill,

        [JsonStringEnumMemberName("button")]
        Button

    }

}
