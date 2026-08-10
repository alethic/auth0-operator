using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionDefaultMethodPhoneIdentifierEnum
{

    [JsonStringEnumMemberName("password")]
    Password,

    /// <summary>
    /// Retained for schema compatibility with the former shared attribute identifier type; not a valid phone
    /// identifier method in Auth0 and never applied.
    /// </summary>
    [JsonStringEnumMemberName("emailOtp")]
    EmailOtp,

    [JsonStringEnumMemberName("phoneOtp")]
    PhoneOtp

}
