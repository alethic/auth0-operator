using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3
{

    public record V2alpha3TenantBrandingColors
    {
        
        [JsonPropertyName("primary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Primary { get; set; }

        [JsonPropertyName("pageBackground")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PageBackground { get; set; }

    }

}
