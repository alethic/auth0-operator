using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// Security header configuration applied to the tenant's pages.
/// </summary>
public record V2alpha3TenantSecurityHeaders
{

    [JsonPropertyName("contentSecurityPolicy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantContentSecurityPolicy? ContentSecurityPolicy { get; set; }

    [JsonPropertyName("xXssProtection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantXssProtection? XXssProtection { get; set; }

}
