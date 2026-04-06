using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1
{

    public record V2alpha1TenantDeviceFlow
    {

        [JsonPropertyName("charset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1TenantCharset? Charset { get; set; }

        [JsonPropertyName("mask")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Mask { get; set; }

    }

}
