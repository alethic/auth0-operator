using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum
{

    [JsonStringEnumMemberName("rpLogout")]
    RpLogout,

    [JsonStringEnumMemberName("idpLogout")]
    IdpLogout,

    [JsonStringEnumMemberName("passwordChanged")]
    PasswordChanged,

    [JsonStringEnumMemberName("sessionExpired")]
    SessionExpired,

    [JsonStringEnumMemberName("sessionRevoked")]
    SessionRevoked,

    [JsonStringEnumMemberName("accountDeleted")]
    AccountDeleted,

    [JsonStringEnumMemberName("emailIdentifierChanged")]
    EmailIdentifierChanged,

    [JsonStringEnumMemberName("mfaPhoneUnenrolled")]
    MfaPhoneUnenrolled,

    [JsonStringEnumMemberName("accountDeactivated")]
    AccountDeactivated

}
