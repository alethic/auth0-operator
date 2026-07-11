using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionIdTokenSignedResponseAlgEnum
{

    [JsonStringEnumMemberName("ES256")]
    Es256,

    [JsonStringEnumMemberName("ES384")]
    Es384,

    [JsonStringEnumMemberName("PS256")]
    Ps256,

    [JsonStringEnumMemberName("PS384")]
    Ps384,

    [JsonStringEnumMemberName("RS256")]
    Rs256,

    [JsonStringEnumMemberName("RS384")]
    Rs384,

    [JsonStringEnumMemberName("RS512")]
    Rs512

}
