using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Identifier attribute used as the primary login identifier when multiple attributes are configured.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ConnectionOptionsPrecedence
    {

        /// <summary>Email address is the primary identifier.</summary>
        [JsonStringEnumMemberName("email")]
        Email,

        /// <summary>Phone number is the primary identifier.</summary>
        [JsonStringEnumMemberName("phoneNumber")]
        PhoneNumber,

        /// <summary>Username is the primary identifier.</summary>
        [JsonStringEnumMemberName("username")]
        UserName

    }

}