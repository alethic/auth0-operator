using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionDpopSigningAlgEnum
{

    [JsonStringEnumMemberName("ES256")]
    Es256,

    [JsonStringEnumMemberName("Ed25519")]
    Ed25519

}
