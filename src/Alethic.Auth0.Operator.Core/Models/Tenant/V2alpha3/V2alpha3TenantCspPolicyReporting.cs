using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// Reporting destination for a single Content-Security-Policy policy.
/// </summary>
public record V2alpha3TenantCspPolicyReporting
{

    [JsonPropertyName("reportToGroup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReportToGroup { get; set; }

    [JsonPropertyName("reportUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReportUri { get; set; }

}
