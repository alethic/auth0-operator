using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    public class BrandingColors
    {
        
        [JsonPropertyName("primary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Primary { get; set; }

        [JsonPropertyName("page_background")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PageBackground { get; set; }

    }

}
