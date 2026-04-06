using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// External SharePoint application URLs if exposed to the Internet.
/// </summary>
public record V1ClientAddonSharePointExternalUrl
{

    /// <summary>
    /// Type discriminator
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; internal set; }

    /// <summary>
    /// Union value
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Value { get; internal set; }

}