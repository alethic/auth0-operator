using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

public record V2alpha1TenantDefaultTokenQuota
{

    [JsonPropertyName("clients")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantTokenQuotaConfiguration? Clients { get; set; }

    [JsonPropertyName("organizations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantTokenQuotaConfiguration? Organizations { get; set; }

}
