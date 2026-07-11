using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Zendesk SSO configuration.
/// </summary>
public record V1ClientAddonZendesk 
{

    /// <summary>
    /// Zendesk account name usually first segment in your Zendesk URL. e.g. `https://acme-org.zendesk.com` would be `acme-org`.
    /// </summary>
    [JsonPropertyName("accountName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccountName { get; set; }

}
