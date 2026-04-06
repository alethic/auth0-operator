using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1
{

    public record V2alpha1TenantGuardianMfaPage
    {

        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enabled { get; set; }

        [JsonPropertyName("html")]
        public string? Html { get; set; }

    }

}
