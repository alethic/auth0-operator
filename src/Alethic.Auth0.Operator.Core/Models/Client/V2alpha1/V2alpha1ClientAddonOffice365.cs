using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientAddonOffice365
{

    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Domain { get; set; }

    [JsonPropertyName("connection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Connection { get; set; }

}
