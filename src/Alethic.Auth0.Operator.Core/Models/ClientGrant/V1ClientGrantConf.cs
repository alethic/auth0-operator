using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.ClientGrant
{

    public partial class V1ClientGrantConf
    {

        [JsonPropertyName("clientRef")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public V1ClientReference? ClientRef { get; set; }

        [JsonPropertyName("audience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public V1ResourceServerReference? Audience { get; set; }

        [JsonPropertyName("organization_usage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrganizationUsage? OrganizationUsage { get; set; }

        [JsonPropertyName("allow_any_organization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowAnyOrganization { get; set; }

        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public string[]? Scope { get; set; }

    }

}
