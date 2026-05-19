using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionMappingModeEnumOidc
{
    [JsonStringEnumMemberName("bind_all")]
    BindAll,
    [JsonStringEnumMemberName("use_map")]
    UseMap
}
