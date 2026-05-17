using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Common options shared by most social connection strategies.
    /// </summary>
    public record V1ConnectionSocialOptions : V1ConnectionOptionsClientCredentials
    {

        /// <summary>
        /// Space-separated list of OAuth 2.0 scopes to request from the social identity provider.
        /// </summary>
        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        /// <summary>
        /// When <c>true</c>, allows entering any custom scope string instead of a predefined list.
        /// </summary>
        [JsonPropertyName("freeform_scopes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FreeformScopes { get; set; }

        /// <summary>
        /// List of user attributes that will not be persisted in the Auth0 user store after each login.
        /// </summary>
        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        /// <summary>
        /// Controls when root profile attributes (<c>name</c>, <c>given_name</c>, etc.) are updated from the identity provider.
        /// </summary>
        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        /// <summary>
        /// Upstream parameters that will be sent to the identity provider on each authentication request.
        /// </summary>
        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? UpstreamParams { get; set; }

    }

}
