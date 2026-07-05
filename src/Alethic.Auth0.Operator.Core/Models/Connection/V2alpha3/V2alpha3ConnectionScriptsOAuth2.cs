using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionScriptsOAuth2
{

    [JsonPropertyName("fetchUserProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FetchUserProfile { get; set; }

    [JsonPropertyName("getLogoutUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GetLogoutUrl { get; set; }

}
