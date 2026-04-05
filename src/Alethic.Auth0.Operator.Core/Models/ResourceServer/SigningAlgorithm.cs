using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SigningAlgorithm
    {

        [JsonStringEnumMemberName("HS256")]
        HS256,

        [JsonStringEnumMemberName("RS256")]
        RS256,

        [JsonStringEnumMemberName("PS256")]
        PS256

    }

}
