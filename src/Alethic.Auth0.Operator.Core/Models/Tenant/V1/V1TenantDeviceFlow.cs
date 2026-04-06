using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V1
{

    public partial class V1TenantDeviceFlow
    {

        [JsonPropertyName("charset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1TenantCharset? Charset { get; set; }

        [JsonPropertyName("mask")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Mask { get; set; }

    }

}
