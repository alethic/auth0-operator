using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>ad</c> (Active Directory / LDAP) connection strategy.
    /// </summary>
    public record V1ConnectionAdOptions
    {

        /// <summary>
        /// LDAP/AD sign-in endpoint URL used by the Auth0 AD/LDAP connector.
        /// </summary>
        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        /// <summary>
        /// List of domain aliases for the connection (e.g. additional email domains that map to this connection).
        /// </summary>
        [JsonPropertyName("domain_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DomainAliases { get; set; }

        /// <summary>
        /// Primary tenant domain for the connection.
        /// </summary>
        [JsonPropertyName("tenant_domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TenantDomain { get; set; }

        /// <summary>
        /// URL of the icon to display for this connection in the Universal Login experience.
        /// </summary>
        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

        /// <summary>
        /// Certificate thumbprints used to verify the identity of the AD/LDAP connector.
        /// </summary>
        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        /// <summary>
        /// When <c>true</c>, enables certificate-based authentication for the connection.
        /// </summary>
        [JsonPropertyName("certAuth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CertAuth { get; set; }

        /// <summary>
        /// PEM-encoded certificates used for client certificate authentication.
        /// </summary>
        [JsonPropertyName("certs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Certs { get; set; }

        /// <summary>
        /// IP address ranges allowed to use this connection.
        /// </summary>
        [JsonPropertyName("ips")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Ips { get; set; }

        /// <summary>
        /// IP address of the AD/LDAP connector agent.
        /// </summary>
        [JsonPropertyName("agentIP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AgentIp { get; set; }

        /// <summary>
        /// Mode in which the AD/LDAP connector agent operates.
        /// </summary>
        [JsonPropertyName("agentMode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AgentMode { get; set; }

        /// <summary>
        /// Version of the AD/LDAP connector agent.
        /// </summary>
        [JsonPropertyName("agentVersion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AgentVersion { get; set; }

        /// <summary>
        /// Kerberos configuration for the connection.
        /// </summary>
        [JsonPropertyName("kerberos")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Kerberos { get; set; }

        /// <summary>
        /// When <c>true</c>, caching of group membership information is disabled.
        /// </summary>
        [JsonPropertyName("disable_cache")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableCache { get; set; }

        /// <summary>
        /// When <c>true</c>, Auth0 will lock user accounts temporarily after too many consecutive failed login attempts.
        /// </summary>
        [JsonPropertyName("brute_force_protection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BruteForceProtection { get; set; }

        /// <summary>
        /// When <c>true</c>, users cannot change their password from the hosted login page.
        /// </summary>
        [JsonPropertyName("disable_self_service_change_password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSelfServiceChangePassword { get; set; }

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
        public Dictionary<string, V1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

    }

}
