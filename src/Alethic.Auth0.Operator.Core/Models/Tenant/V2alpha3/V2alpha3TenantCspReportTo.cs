using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// Report-To header group used by the Content-Security-Policy reporting infrastructure.
/// </summary>
public record V2alpha3TenantCspReportTo
{

    [JsonPropertyName("endpoints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspReportToEndpoint[]? Endpoints { get; set; }

    [JsonPropertyName("group")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group { get; set; }

    [JsonPropertyName("maxAge")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxAge { get; set; }

}
