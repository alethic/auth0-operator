using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Slack team or workspace name usually first segment in your Slack URL. e.g. `https://acme-org.slack.com` would be `acme-org`.
/// </summary>
public record V1ClientAddonSlack
{

    /// <summary>
    /// Slack team name.
    /// </summary>
    [JsonPropertyName("team")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Team { get; set; }

}
