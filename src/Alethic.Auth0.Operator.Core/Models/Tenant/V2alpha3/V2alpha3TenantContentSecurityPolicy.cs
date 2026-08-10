using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// Content-Security-Policy configuration for the tenant's pages.
/// </summary>
public record V2alpha3TenantContentSecurityPolicy
{

    [JsonPropertyName("enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Enabled { get; set; }

    [JsonPropertyName("policies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspPolicy[]? Policies { get; set; }

    [JsonPropertyName("reportingInfrastructure")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspReportingInfrastructure? ReportingInfrastructure { get; set; }

}
