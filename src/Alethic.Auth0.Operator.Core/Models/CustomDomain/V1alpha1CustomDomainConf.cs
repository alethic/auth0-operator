using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain
{

    public partial class V1alpha1CustomDomainConf
    {

        [JsonPropertyName("domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Domain { get; set; }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CustomDomainCertificateProvisioning? Type { get; set; }

        [JsonPropertyName("verification_method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CustomDomainVerificationMethod? VerificationMethod { get; set; }

        [JsonPropertyName("primary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Primary { get; set; }

        [JsonPropertyName("tls_policy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TlsPolicy { get; set; }

        [JsonPropertyName("custom_client_ip_header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomClientIpHeader { get; set; }

    }

}
