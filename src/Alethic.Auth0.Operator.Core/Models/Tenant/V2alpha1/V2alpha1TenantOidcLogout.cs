using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

public record V2alpha1TenantOidcLogout
{

    [JsonPropertyName("rp_logout_end_session_endpoint_discovery")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RpLogoutEndSessionEndpointDiscovery { get; set; }

}
