using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3
{

    public record V2alpha3TenantPrompts
    {

        [JsonPropertyName("universalLoginExperience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3TenantUniversalLoginExperience? UniversalLoginExperience { get; set; }

        [JsonPropertyName("identifierFirst")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IdentifierFirst { get; set; }

        [JsonPropertyName("webauthnPlatformFirstFactor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WebauthnPlatformFirstFactor { get; set; }

    }

}
