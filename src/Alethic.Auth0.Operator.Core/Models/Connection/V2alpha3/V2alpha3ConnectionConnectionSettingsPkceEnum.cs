using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionConnectionSettingsPkceEnum
{

    [JsonStringEnumMemberName("auto")]
    Auto,

    [JsonStringEnumMemberName("S256")]
    S256,

    [JsonStringEnumMemberName("plain")]
    Plain,

    [JsonStringEnumMemberName("disabled")]
    Disabled

}
