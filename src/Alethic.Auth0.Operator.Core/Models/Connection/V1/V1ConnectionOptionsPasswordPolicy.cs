using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionOptionsPasswordPolicy
    {

        [JsonStringEnumMemberName("none")]
        None,

        [JsonStringEnumMemberName("low")]
        Low,

        [JsonStringEnumMemberName("fair")]
        Fair,

        [JsonStringEnumMemberName("good")]
        Good,

        [JsonStringEnumMemberName("excellent")]
        Excellent

    }

}