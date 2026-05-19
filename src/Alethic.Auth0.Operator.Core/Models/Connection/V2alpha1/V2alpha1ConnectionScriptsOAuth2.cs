using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionScriptsOAuth2
{

    [JsonPropertyName("fetchUserProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FetchUserProfile { get; set; }

    [JsonPropertyName("getLogoutUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GetLogoutUrl { get; set; }

}
