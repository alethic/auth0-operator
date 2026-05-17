using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionAdfsOptions
    {

        [JsonPropertyName("adfsServer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdfsServer { get; set; }

        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        [JsonPropertyName("entityId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityId { get; set; }

        [JsonPropertyName("fedMetadataXml")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FedMetadataXml { get; set; }

        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        [JsonPropertyName("prevThumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? PrevThumbprints { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        [JsonPropertyName("user_id_attribute")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserIdAttribute { get; set; }

        [JsonPropertyName("should_trust_email_verified_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShouldTrustEmailVerifiedConnection { get; set; }

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? UpstreamParams { get; set; }

    }

}
