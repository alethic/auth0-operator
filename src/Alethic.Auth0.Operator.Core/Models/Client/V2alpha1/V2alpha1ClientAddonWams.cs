using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientAddonWams
{

    [JsonPropertyName("masterkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Masterkey { get; set; }

}
