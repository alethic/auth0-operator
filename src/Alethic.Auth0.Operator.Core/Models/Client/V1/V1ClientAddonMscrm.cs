using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Microsoft Dynamics CRM SSO configuration.
/// </summary>
public record V1ClientAddonMscrm
{

    /// <summary>
    /// Microsoft Dynamics CRM application URL.
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

}
