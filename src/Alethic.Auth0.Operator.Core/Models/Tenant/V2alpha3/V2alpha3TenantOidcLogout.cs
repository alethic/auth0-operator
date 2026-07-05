using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

public record V2alpha3TenantOidcLogout
{

    [JsonPropertyName("rpLogoutEndSessionEndpointDiscovery")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RpLogoutEndSessionEndpointDiscovery { get; set; }

}
