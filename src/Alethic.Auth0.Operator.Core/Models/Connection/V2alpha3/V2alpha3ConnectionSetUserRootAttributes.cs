using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Controls when Auth0 updates root-level user profile attributes from the identity provider.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ConnectionSetUserRootAttributes
    {

        /// <summary>Root attributes are updated on every login.</summary>
        [JsonStringEnumMemberName("onEachLogin")]
        OnEachLogin,

        /// <summary>Root attributes are only set on the user's first login.</summary>
        [JsonStringEnumMemberName("onFirstLogin")]
        OnFirstLogin,

        /// <summary>Root attributes are never updated from the identity provider.</summary>
        [JsonStringEnumMemberName("neverOnLogin")]
        NeverOnLogin

    }

}
