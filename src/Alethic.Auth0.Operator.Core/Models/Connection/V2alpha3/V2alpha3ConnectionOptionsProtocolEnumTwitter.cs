using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionOptionsProtocolEnumTwitter
{

    [JsonStringEnumMemberName("oauth1")]
    Oauth1,

    [JsonStringEnumMemberName("oauth2")]
    Oauth2

}
