using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V1
{

    public record V1TenantMtls
    {

        [JsonPropertyName("enable_endpoint_aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableEndpointAliases { get; set; }

    }

}
