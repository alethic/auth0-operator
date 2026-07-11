using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

public record V2alpha3TenantErrorPage
{

    [JsonPropertyName("html")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Html { get; set; }

    [JsonPropertyName("showLogLink")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ShowLogLink { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

}
