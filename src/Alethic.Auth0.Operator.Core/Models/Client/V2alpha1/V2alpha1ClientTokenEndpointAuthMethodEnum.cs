using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientTokenEndpointAuthMethodEnum
{

    [JsonStringEnumMemberName("none")]
    None,

    [JsonStringEnumMemberName("client_secret_post")]
    ClientSecretPost,

    [JsonStringEnumMemberName("client_secret_basic")]
    ClientSecretBasic

}
