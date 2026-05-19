using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionPasswordCharacterTypeEnum
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
