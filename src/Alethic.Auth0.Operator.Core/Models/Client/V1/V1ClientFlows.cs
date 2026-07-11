using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientFlows
    {

        [JsonStringEnumMemberName("client_credentials")]
        ClientCredentials

    }

}
