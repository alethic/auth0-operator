using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionDigestAlgorithmEnumSaml
{

    [JsonStringEnumMemberName("sha1")]
    Sha1,

    [JsonStringEnumMemberName("sha256")]
    Sha256

}
