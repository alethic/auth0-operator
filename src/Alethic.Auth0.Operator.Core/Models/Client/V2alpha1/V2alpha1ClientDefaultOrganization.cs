using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientDefaultOrganization
{

    [JsonPropertyName("organization_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("flows")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientDefaultOrganizationFlowsEnum[]? Flows { get; set; }

}
