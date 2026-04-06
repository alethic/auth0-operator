using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// AWS addon configuration.
/// </summary>
public record V1ClientAddonAws
{

    /// <summary>
    /// AWS principal ARN, e.g. `arn:aws:iam::010616021751:saml-provider/idpname`
    /// </summary>
    [JsonPropertyName("principal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Principal { get; set; }

    /// <summary>
    /// AWS role ARN, e.g. `arn:aws:iam::010616021751:role/foo`
    /// </summary>
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Role { get; set; }

    /// <summary>
    /// AWS token lifetime in seconds
    /// </summary>
    [JsonPropertyName("lifetime_in_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LifetimeInSeconds { get; set; }

}
