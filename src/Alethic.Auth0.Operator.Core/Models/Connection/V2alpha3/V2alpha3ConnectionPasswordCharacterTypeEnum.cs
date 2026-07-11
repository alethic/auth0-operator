using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionPasswordCharacterTypeEnum
{

    [JsonStringEnumMemberName("uppercase")]
    Uppercase,

    [JsonStringEnumMemberName("lowercase")]
    Lowercase,

    [JsonStringEnumMemberName("number")]
    Number,

    [JsonStringEnumMemberName("special")]
    Special

}
