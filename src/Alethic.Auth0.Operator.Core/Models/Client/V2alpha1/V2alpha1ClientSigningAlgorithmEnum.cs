using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientSigningAlgorithmEnum
{

    [JsonStringEnumMemberName("hs256")]
    Hs256,

    [JsonStringEnumMemberName("rs256")]
    Rs256,

    [JsonStringEnumMemberName("rs512")]
    Rs512,

    [JsonStringEnumMemberName("ps256")]
    Ps256

}
