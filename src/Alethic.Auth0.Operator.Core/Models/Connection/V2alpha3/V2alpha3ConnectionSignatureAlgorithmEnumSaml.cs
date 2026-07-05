using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionSignatureAlgorithmEnumSaml
{

    [JsonStringEnumMemberName("rsa-sha1")]
    RsaSha1,

    [JsonStringEnumMemberName("rsa-sha256")]
    RsaSha256

}
