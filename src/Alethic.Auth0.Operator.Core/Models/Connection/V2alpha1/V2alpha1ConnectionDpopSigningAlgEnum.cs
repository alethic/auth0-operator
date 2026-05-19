using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionDpopSigningAlgEnum
{
    [JsonStringEnumMemberName("es256")]
    Es256,
    [JsonStringEnumMemberName("ed25519")]
    Ed25519
}
