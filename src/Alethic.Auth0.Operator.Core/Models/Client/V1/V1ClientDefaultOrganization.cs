using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientDefaultOrganization
    {

        [JsonPropertyName("organization_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OrganizationId { get; set; }

        [JsonPropertyName("flows")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientFlows[]? Flows { get; set; }

    }

}
