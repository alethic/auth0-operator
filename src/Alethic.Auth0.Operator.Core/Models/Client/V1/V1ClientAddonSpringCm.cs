using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// SpringCM SSO configuration.
/// </summary>
public record V1ClientAddonSpringCm
{

    /// <summary>
    /// SpringCM ACS URL, e.g. `https://na11.springcm.com/atlas/sso/SSOEndpoint.ashx`.
    /// </summary>
    [JsonPropertyName("acsurl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Acsurl { get; set; }

}
