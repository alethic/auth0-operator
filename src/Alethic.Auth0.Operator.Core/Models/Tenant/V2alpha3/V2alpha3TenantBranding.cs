using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3
{

    public record V2alpha3TenantBranding
    {

        [JsonPropertyName("colors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3TenantBrandingColors? Colors { get; set; }

        [JsonPropertyName("faviconUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FaviconUrl { get; set; }

        [JsonPropertyName("logoUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUrl { get; set; }

        [JsonPropertyName("font")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3TenantBrandingFont? Font { get; set; }

    }

}