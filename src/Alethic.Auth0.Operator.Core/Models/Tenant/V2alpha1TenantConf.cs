using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    public partial class V2alpha1TenantConf
    {

        [JsonPropertyName("settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantConfSettings? Settings { get; set; }

        [JsonPropertyName("branding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantConfBranding? Branding { get; set; }

    }

}
