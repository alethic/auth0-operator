using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Password strength policy level enforced for a database connection.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionOptionsPasswordPolicy
    {

        /// <summary>No password policy is enforced.</summary>
        [JsonStringEnumMemberName("none")]
        None,

        /// <summary>Low strength: at least 1 character of any type.</summary>
        [JsonStringEnumMemberName("low")]
        Low,

        /// <summary>Fair strength: at least 6 characters including a lower-case letter, an upper-case letter, and a number.</summary>
        [JsonStringEnumMemberName("fair")]
        Fair,

        /// <summary>Good strength: at least 8 characters including a lower-case letter, an upper-case letter, a number, and a special character.</summary>
        [JsonStringEnumMemberName("good")]
        Good,

        /// <summary>Excellent strength: at least 10 characters including a lower-case letter, an upper-case letter, a number, and at least 3 different special characters.</summary>
        [JsonStringEnumMemberName("excellent")]
        Excellent

    }

}