using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionIdTokenSignedResponseAlgEnum
{

    [JsonStringEnumMemberName("es256")]
    Es256,

    [JsonStringEnumMemberName("es384")]
    Es384,

    [JsonStringEnumMemberName("ps256")]
    Ps256,

    [JsonStringEnumMemberName("ps384")]
    Ps384,

    [JsonStringEnumMemberName("rs256")]
    Rs256,

    [JsonStringEnumMemberName("rs384")]
    Rs384,

    [JsonStringEnumMemberName("rs512")]
    Rs512

}
