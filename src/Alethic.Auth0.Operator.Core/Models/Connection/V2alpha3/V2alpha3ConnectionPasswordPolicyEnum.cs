using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionPasswordPolicyEnum
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
