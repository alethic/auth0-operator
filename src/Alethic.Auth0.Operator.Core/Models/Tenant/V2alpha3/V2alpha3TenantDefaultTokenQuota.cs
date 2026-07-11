using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

public record V2alpha3TenantDefaultTokenQuota
{

    [JsonPropertyName("clients")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantTokenQuotaConfiguration? Clients { get; set; }

    [JsonPropertyName("organizations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantTokenQuotaConfiguration? Organizations { get; set; }

}
