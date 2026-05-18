using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionSetUserRootAttributes
    {

        [JsonStringEnumMemberName("on_each_login")]
        OnEachLogin,

        [JsonStringEnumMemberName("on_first_login")]
        OnFirstLogin,

        [JsonStringEnumMemberName("never_on_login")]
        NeverOnLogin

    }

}
