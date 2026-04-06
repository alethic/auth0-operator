using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1
{

    public class V2alpha1TenantPrompts
    {

        [JsonPropertyName("universal_login_experience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1TenantUniversalLoginExperience? UniversalLoginExperience { get; set; }

        [JsonPropertyName("identifier_first")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IdentifierFirst { get; set; }

        [JsonPropertyName("webauthn_platform_first_factor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WebauthnPlatformFirstFactor { get; set; }

    }

}
