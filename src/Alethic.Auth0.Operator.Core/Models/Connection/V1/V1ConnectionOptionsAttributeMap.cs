using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Mapping configuration that controls how identity provider claims are translated to Auth0 user profile attributes.
    /// </summary>
    public record V1ConnectionOptionsAttributeMap
    {

        /// <summary>
        /// Controls how claims are mapped. Can be <c>use_map</c> to apply the custom <see cref="Attributes"/> mapping,
        /// or <c>bind_all</c> to pass all claims through without mapping.
        /// </summary>
        [JsonPropertyName("mapping_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MappingMode { get; set; }

        /// <summary>
        /// Space-separated list of claims to request from the UserInfo endpoint.
        /// </summary>
        [JsonPropertyName("userinfo_scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserinfoScope { get; set; }

        /// <summary>
        /// Maps Auth0 user profile attribute names to identity provider claim names.
        /// </summary>
        [JsonPropertyName("attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public System.Collections.Generic.Dictionary<string, string?>? Attributes { get; set; }

    }

}
