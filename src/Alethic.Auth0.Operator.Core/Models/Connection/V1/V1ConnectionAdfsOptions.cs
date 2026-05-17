using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>adfs</c> (Active Directory Federation Services) connection strategy.
    /// </summary>
    public record V1ConnectionAdfsOptions
    {

        /// <summary>
        /// ADFS server URL (e.g. <c>https://adfs.myserver.com</c>).
        /// </summary>
        [JsonPropertyName("adfsServer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdfsServer { get; set; }

        /// <summary>
        /// ADFS sign-in endpoint URL.
        /// </summary>
        [JsonPropertyName("signInEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignInEndpoint { get; set; }

        /// <summary>
        /// Entity ID / issuer for this connection as configured in ADFS.
        /// </summary>
        [JsonPropertyName("entityId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityId { get; set; }

        /// <summary>
        /// Federation metadata XML document describing the ADFS identity provider.
        /// </summary>
        [JsonPropertyName("fedMetadataXml")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FedMetadataXml { get; set; }

        /// <summary>
        /// Current certificate thumbprints used to validate tokens from the ADFS server.
        /// </summary>
        [JsonPropertyName("thumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Thumbprints { get; set; }

        /// <summary>
        /// Previous certificate thumbprints retained for rollover scenarios.
        /// </summary>
        [JsonPropertyName("prevThumbprints")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? PrevThumbprints { get; set; }

        /// <summary>
        /// URL of the icon to display for this connection in the Universal Login experience.
        /// </summary>
        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

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
        /// SAML attribute that will be mapped to the Auth0 user ID.
        /// </summary>
        [JsonPropertyName("user_id_attribute")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserIdAttribute { get; set; }

        /// <summary>
        /// Determines whether Auth0 should trust the email-verified claim coming from this identity provider.
        /// </summary>
        [JsonPropertyName("should_trust_email_verified_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShouldTrustEmailVerifiedConnection { get; set; }

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
