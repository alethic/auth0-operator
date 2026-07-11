using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

public record V2alpha1TenantSessions
{

    [JsonPropertyName("oidc_logout_prompt_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? OidcLogoutPromptEnabled { get; set; }

}
