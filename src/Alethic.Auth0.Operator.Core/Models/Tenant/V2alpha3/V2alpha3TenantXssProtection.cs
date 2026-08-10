using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// X-XSS-Protection header configuration for the tenant's pages.
/// </summary>
public record V2alpha3TenantXssProtection
{

    [JsonPropertyName("enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Enabled { get; set; }

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantXssProtectionModeEnum? Mode { get; set; }

    [JsonPropertyName("reportUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReportUri { get; set; }

}
