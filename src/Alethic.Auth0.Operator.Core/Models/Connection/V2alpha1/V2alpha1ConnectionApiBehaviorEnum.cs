using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionApiBehaviorEnum
{
    [JsonStringEnumMemberName("required")]
    Required,
    [JsonStringEnumMemberName("optional")]
    Optional
}
