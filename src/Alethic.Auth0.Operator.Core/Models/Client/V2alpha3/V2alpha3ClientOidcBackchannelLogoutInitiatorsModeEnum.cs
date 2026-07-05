using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum
{

    [JsonStringEnumMemberName("custom")]
    Custom,

    [JsonStringEnumMemberName("all")]
    All

}
