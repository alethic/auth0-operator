using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>ad</c> (Active Directory / LDAP) connection strategy.
    /// </summary>
    public record V2alpha1ConnectionAdOptions : V2alpha1ConnectionOptionsBase
    {

        [JsonPropertyName("agentIP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AgentIp { get; set; }

        [JsonPropertyName("agentMode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AgentMode { get; set; }

        [JsonPropertyName("agentVersion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AgentVersion { get; set; }

        [JsonPropertyName("brute_force_protection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BruteForceProtection { get; set; }

        /// <summary>
        /// Enables client SSL certificate authentication for the AD connector, requiring HTTPS on the sign-in endpoint
        /// </summary>
        [JsonPropertyName("certAuth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CertAuth { get; set; }

        [JsonPropertyName("certs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Certs { get; set; }

        /// <summary>
        /// When enabled, disables caching of AD connector authentication results to ensure real-time validation against the directory
        /// </summary>
        [JsonPropertyName("disable_cache")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableCache { get; set; }

        /// <summary>
        /// When enabled, hides the 'Forgot Password' link on login pages to prevent users from initiating self-service password resets
        /// </summary>
        [JsonPropertyName("disable_self_service_change_password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSelfServiceChangePassword { get; set; }

        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        [JsonPropertyName("ips")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Ips { get; set; }

        /// <summary>
        /// Enables Windows Integrated Authentication (Kerberos) for seamless SSO when users authenticate from within the corporate network IP ranges
        /// </summary>
        [JsonPropertyName("kerberos")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Kerberos { get; set; }

        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

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

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

    }

}
