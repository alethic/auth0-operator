using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

public record V2alpha1TenantTokenQuotaConfiguration
{

    [JsonPropertyName("client_credentials")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantTokenQuotaClientCredentials? ClientCredentials { get; set; }

}
