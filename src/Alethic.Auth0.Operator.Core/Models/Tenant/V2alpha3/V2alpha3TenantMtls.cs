using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

public record V2alpha3TenantMtls
{

    [JsonPropertyName("enableEndpointAliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableEndpointAliases { get; set; }

}
