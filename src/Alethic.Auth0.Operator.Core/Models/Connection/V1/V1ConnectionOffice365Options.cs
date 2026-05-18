using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>office365</c> enterprise connection strategy.
    /// </summary>
    public record V1ConnectionOffice365Options : V1ConnectionSocialOptions
    {

        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("adfs_server")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdfsServer { get; set; }

        [JsonPropertyName("identity_api")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IdentityApi { get; set; }

        [JsonPropertyName("waad_protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WaadProtocol { get; set; }

        [JsonPropertyName("use_wsfed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UseWsfed { get; set; }

        [JsonPropertyName("use_common_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UseCommonEndpoint { get; set; }

        [JsonPropertyName("api_enable_users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ApiEnableUsers { get; set; }

        [JsonPropertyName("basic_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BasicProfile { get; set; }

        [JsonPropertyName("should_trust_email_verified_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShouldTrustEmailVerifiedConnection { get; set; }

        [JsonPropertyName("idpinitiated")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsIdpinitiated? Idpinitiated { get; set; }

    }

}
