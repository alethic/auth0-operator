using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

public record V2alpha1TenantSessionCookie
{

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantSessionCookieModeEnum? Mode { get; set; }

}
