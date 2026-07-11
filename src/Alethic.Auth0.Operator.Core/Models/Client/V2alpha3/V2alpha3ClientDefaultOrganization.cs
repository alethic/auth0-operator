using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientDefaultOrganization
{

    [JsonPropertyName("organizationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("flows")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientDefaultOrganizationFlowsEnum[]? Flows { get; set; }

}
