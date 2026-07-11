using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ResourceServerSigningAlgorithm
    {

        [JsonStringEnumMemberName("HS256")]
        HS256,

        [JsonStringEnumMemberName("RS256")]
        RS256,

        [JsonStringEnumMemberName("PS256")]
        PS256

    }

}
