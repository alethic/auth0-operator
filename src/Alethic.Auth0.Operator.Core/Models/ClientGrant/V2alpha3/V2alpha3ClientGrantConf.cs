using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3
{

    public record V2alpha3ClientGrantConf
    {

        [JsonPropertyName("clientRef")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public V1ClientReference? ClientRef { get; set; }

        [JsonPropertyName("audience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public V1ResourceServerReference? Audience { get; set; }

        [JsonPropertyName("organizationUsage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ClientGrantOrganizationUsage? OrganizationUsage { get; set; }

        [JsonPropertyName("allowAnyOrganization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowAnyOrganization { get; set; }

        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public string[]? Scope { get; set; }

    }

}
