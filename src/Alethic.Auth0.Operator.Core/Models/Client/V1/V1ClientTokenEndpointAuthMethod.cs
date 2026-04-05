using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientTokenEndpointAuthMethod
    {

        [JsonStringEnumMemberName("none")]
        None,

        [JsonStringEnumMemberName("client_secret_post")]
        ClientSecretPost,

        [JsonStringEnumMemberName("client_secret_basic")]
        ClientSecretBasic

    }

}
