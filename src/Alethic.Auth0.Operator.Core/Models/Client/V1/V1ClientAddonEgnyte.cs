using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Egnyte SSO configuration.
/// </summary>
public record V1ClientAddonEgnyte
{

    /// <summary>
    /// Your custom domain found in your Egnyte URL. e.g. `https://acme-org.egnyte.com` would be `acme-org`.
    /// </summary>
    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Domain { get; set; }

}
