using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    public class TenantBranding
    {

        [JsonPropertyName("colors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingColors? Colors { get; set; }

        [JsonPropertyName("favicon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FaviconUrl { get; set; }

        [JsonPropertyName("logo_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUrl { get; set; }

        [JsonPropertyName("font")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingFont? Font { get; set; }

    }

}