using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ResourceServerTokenDialect
    {

        [JsonStringEnumMemberName("accessToken")]
        AccessToken,

        [JsonStringEnumMemberName("accessTokenAuthz")]
        AccessTokenAuthZ,

        [JsonStringEnumMemberName("rfc9068Profile")]
        Rfc9068Profile,

        [JsonStringEnumMemberName("rfc9068ProfileAuthz")]
        Rfc9068ProfileAuthz

    }

}
