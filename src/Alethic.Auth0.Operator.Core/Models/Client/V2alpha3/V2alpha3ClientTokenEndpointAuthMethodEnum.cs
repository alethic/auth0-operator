using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientTokenEndpointAuthMethodEnum
{

    [JsonStringEnumMemberName("none")]
    None,

    [JsonStringEnumMemberName("clientSecretPost")]
    ClientSecretPost,

    [JsonStringEnumMemberName("clientSecretBasic")]
    ClientSecretBasic

}
