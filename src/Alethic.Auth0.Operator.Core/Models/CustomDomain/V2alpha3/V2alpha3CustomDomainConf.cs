using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3
{

    public record V2alpha3CustomDomainConf
    {

        [JsonPropertyName("domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Domain { get; set; }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3CustomDomainCertificateProvisioning? Type { get; set; }

        [JsonPropertyName("verificationMethod")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3CustomDomainVerificationMethod? VerificationMethod { get; set; }

        [JsonPropertyName("primary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Primary { get; set; }

        [JsonPropertyName("tlsPolicy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TlsPolicy { get; set; }

        [JsonPropertyName("customClientIpHeader")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomClientIpHeader { get; set; }

    }

}
