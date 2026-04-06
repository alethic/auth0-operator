using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Windows Azure Mobile Services addon configuration.
/// </summary>
public record V1ClientAddonWams
{

    /// <summary>
    /// Your master key for Windows Azure Mobile Services.
    /// </summary>
    [JsonPropertyName("masterkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Masterkey { get; set; }

}
