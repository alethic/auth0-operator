using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Kerberos configuration for an Active Directory connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsKerberos
    {

        /// <summary>
        /// When <c>true</c>, Kerberos integrated Windows authentication is enabled for the connection.
        /// </summary>
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enabled { get; set; }

        /// <summary>
        /// IP address ranges (CIDR notation) from which Kerberos authentication is allowed.
        /// </summary>
        [JsonPropertyName("ips")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Ips { get; set; }

    }

}
