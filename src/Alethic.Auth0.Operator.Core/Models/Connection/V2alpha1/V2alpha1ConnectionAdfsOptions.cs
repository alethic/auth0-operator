using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>adfs</c> (Active Directory Federation Services) connection strategy.
    /// </summary>
    public record V2alpha1ConnectionAdfsOptions : V2alpha1ConnectionOptionsBase
    {

        /// <summary>
        /// ADFS federation metadata host or XML URL used to discover WS-Fed endpoints and certificates. Errors if adfs_server and fedMetadataXml are both absent.
        /// </summary>
        [JsonPropertyName("adfs_server")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdfsServer { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        /// <summary>
        /// The entity identifier (Issuer) for the ADFS Service Provider. When not provided, defaults to 'urn:auth0:{tenant}:{connection}'.
        /// </summary>
        [JsonPropertyName("entityId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityId { get; set; }

        [JsonPropertyName("fedMetadataXml")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FedMetadataXml { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("prev_thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? PrevThumbprints { get; set; }

        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        [JsonPropertyName("should_trust_email_verified_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionShouldTrustEmailVerifiedConnection? ShouldTrustEmailVerifiedConnection { get; set; }

        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V2alpha1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

        /// <summary>
        /// Custom ADFS claim to use as the unique user identifier. When provided, this attribute is prepended to the default user_id mapping list with highest priority. Accepts a string (single ADFS claim name).
        /// </summary>
        [JsonPropertyName("user_id_attribute")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserIdAttribute { get; set; }

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object?> AdditionalProperties { get; set; } = new();

    }

}
