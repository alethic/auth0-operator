using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Sentry SSO configuration.
/// </summary>
public record V1ClientAddonSentry
{

    /// <summary>
    /// Generated slug for your Sentry organization. Found in your Sentry URL. e.g. `https://sentry.acme.com/acme-org/` would be `acme-org`.
    /// </summary>
    [JsonPropertyName("org_slug")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrgSlug { get; set; }

    /// <summary>
    /// URL prefix only if running Sentry Community Edition, otherwise leave should be blank.
    /// </summary>
    [JsonPropertyName("base_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BaseUrl { get; set; }

}
