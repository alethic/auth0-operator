using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionShouldTrustEmailVerifiedConnectionEnum
{
    [JsonStringEnumMemberName("never_set_emails_as_verified")]
    NeverSetEmailsAsVerified,
    [JsonStringEnumMemberName("always_set_emails_as_verified")]
    AlwaysSetEmailsAsVerified
}
