using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionPasswordPolicyEnum
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
