using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Controls when Auth0 updates root-level user profile attributes from the identity provider.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionSetUserRootAttributes
    {

        /// <summary>Root attributes are updated on every login.</summary>
        [JsonStringEnumMemberName("on_each_login")]
        OnEachLogin,

        /// <summary>Root attributes are only set on the user's first login.</summary>
        [JsonStringEnumMemberName("on_first_login")]
        OnFirstLogin,

        /// <summary>Root attributes are never updated from the identity provider.</summary>
        [JsonStringEnumMemberName("never_on_login")]
        NeverOnLogin

    }

}
