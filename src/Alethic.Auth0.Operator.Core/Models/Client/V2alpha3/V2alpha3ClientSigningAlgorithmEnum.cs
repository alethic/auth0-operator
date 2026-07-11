using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientSigningAlgorithmEnum
{

    [JsonStringEnumMemberName("HS256")]
    Hs256,

    [JsonStringEnumMemberName("RS256")]
    Rs256,

    [JsonStringEnumMemberName("RS512")]
    Rs512,

    [JsonStringEnumMemberName("PS256")]
    Ps256

}
