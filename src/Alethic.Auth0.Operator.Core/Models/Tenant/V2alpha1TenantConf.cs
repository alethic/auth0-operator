using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    public partial class V2alpha1TenantConf
    {

        [JsonPropertyName("settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantSettings? Settings { get; set; }

        [JsonPropertyName("branding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantBranding? Branding { get; set; }

        [JsonPropertyName("prompts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantPrompts? Prompts { get; set; }

    }

}
