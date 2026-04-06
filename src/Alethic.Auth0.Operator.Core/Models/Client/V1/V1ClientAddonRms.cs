using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Active Directory Rights Management Service SSO configuration.
/// </summary>
public record V1ClientAddonRms
{

    /// <summary>
    /// URL of your Rights Management Server. It can be internal or external, but users will have to be able to reach it.
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

}
