using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientAddonSentry
{

    [JsonPropertyName("org_slug")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrgSlug { get; set; }

    [JsonPropertyName("base_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BaseUrl { get; set; }

}
