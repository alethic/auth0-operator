using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionTokenEndpointAuthMethodEnum
{
    [JsonStringEnumMemberName("client_secret_post")]
    ClientSecretPost,
    [JsonStringEnumMemberName("private_key_jwt")]
    PrivateKeyJwt
}
