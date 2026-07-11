using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// SharePoint SSO configuration.
/// </summary>
public record V1ClientAddonSharePoint
{

    /// <summary>
    /// Internal SharePoint application URL.
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    [JsonPropertyName("external_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSharePointExternalUrl? ExternalUrl { get; set; }

}
