using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// Reporting infrastructure shared by the tenant's Content-Security-Policy policies.
/// </summary>
public record V2alpha3TenantCspReportingInfrastructure
{

    /// <summary>
    /// Reporting endpoint names mapped to their URLs.
    /// </summary>
    [JsonPropertyName("reportingEndpoints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? ReportingEndpoints { get; set; }

    [JsonPropertyName("reportTo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspReportTo? ReportTo { get; set; }

}
