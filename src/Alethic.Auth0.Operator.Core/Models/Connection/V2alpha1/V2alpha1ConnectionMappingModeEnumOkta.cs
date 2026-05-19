using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionMappingModeEnumOkta
{
    [JsonStringEnumMemberName("basic_profile")]
    BasicProfile,
    [JsonStringEnumMemberName("use_map")]
    UseMap
}
