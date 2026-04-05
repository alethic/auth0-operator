using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientLogoutInitiators
    {

        [JsonStringEnumMemberName("rp-logout")]
        RpLogout,

        [JsonStringEnumMemberName("idp-logout")]
        IdpLogout,

        [JsonStringEnumMemberName("password-changed")]
        PasswordChanged,

        [JsonStringEnumMemberName("session-expired")]
        SessionExpired

    }

}
