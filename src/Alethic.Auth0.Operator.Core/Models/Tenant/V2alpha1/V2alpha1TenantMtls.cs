using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1
{

    public class V2alpha1TenantMtls
    {

        [JsonPropertyName("enable_endpoint_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableEndpointAliases { get; set; }

    }

}
