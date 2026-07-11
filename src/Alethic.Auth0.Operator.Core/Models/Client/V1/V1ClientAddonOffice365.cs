using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Microsoft Office 365 SSO configuration.
/// </summary>
public record V1ClientAddonOffice365
{

    /// <summary>
    /// Your Office 365 domain name. e.g. `acme-org.com`.
    /// </summary>
    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Domain { get; set; }

    /// <summary>
    /// Optional Auth0 database connection for testing an already-configured Office 365 tenant.
    /// </summary>
    [JsonPropertyName("connection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Connection { get; set; }

}
