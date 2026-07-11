using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// New Relic SSO configuration.
/// </summary>
public record V1ClientAddonNewRelic
{

    /// <summary>
    /// Your New Relic Account ID found in your New Relic URL after the `/accounts/` path. e.g. `https://rpm.newrelic.com/accounts/123456/query` would be `123456`.
    /// </summary>
    [JsonPropertyName("account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Account { get; set; }

}
