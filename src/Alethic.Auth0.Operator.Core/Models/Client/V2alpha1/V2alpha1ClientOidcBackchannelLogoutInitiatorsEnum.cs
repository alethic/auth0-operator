using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum
{

    [JsonStringEnumMemberName("rp_logout")]
    RpLogout,

    [JsonStringEnumMemberName("idp_logout")]
    IdpLogout,

    [JsonStringEnumMemberName("password_changed")]
    PasswordChanged,

    [JsonStringEnumMemberName("session_expired")]
    SessionExpired,

    [JsonStringEnumMemberName("session_revoked")]
    SessionRevoked,

    [JsonStringEnumMemberName("account_deleted")]
    AccountDeleted,

    [JsonStringEnumMemberName("email_identifier_changed")]
    EmailIdentifierChanged,

    [JsonStringEnumMemberName("mfa_phone_unenrolled")]
    MfaPhoneUnenrolled,

    [JsonStringEnumMemberName("account_deactivated")]
    AccountDeactivated

}
