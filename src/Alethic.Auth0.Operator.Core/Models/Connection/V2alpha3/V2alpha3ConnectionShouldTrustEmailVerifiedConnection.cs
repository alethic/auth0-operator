using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ConnectionShouldTrustEmailVerifiedConnection
    {

        [JsonStringEnumMemberName("neverSetEmailsAsVerified")]
        NeverSetEmailsAsVerified,

        [JsonStringEnumMemberName("alwaysSetEmailsAsVerified")]
        AlwaysSetEmailsAsVerified,

    }

}