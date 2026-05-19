using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionSetUserRootAttributesEnum
{

    [JsonStringEnumMemberName("on_each_login")]
    OnEachLogin,

    [JsonStringEnumMemberName("on_first_login")]
    OnFirstLogin,

    [JsonStringEnumMemberName("never_on_login")]
    NeverOnLogin

}
