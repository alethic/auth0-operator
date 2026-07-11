using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

public record V1ClientAddonSsoIntegration 
{

    /// <summary>
    /// SSO integration name
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    /// <summary>
    /// SSO integration version installed
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Version { get; set; }

}
