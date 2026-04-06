using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V1
{

    public class V1TenantPrompts
    {

        [JsonPropertyName("universal_login_experience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1TenantUniversalLoginExperience? UniversalLoginExperience { get; set; }

        [JsonPropertyName("identifier_first")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IdentifierFirst { get; set; }

        [JsonPropertyName("webauthn_platform_first_factor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WebauthnPlatformFirstFactor { get; set; }

    }

}
