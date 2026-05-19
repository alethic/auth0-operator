using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionScriptsOAuth1
{

    [JsonPropertyName("fetchUserProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FetchUserProfile { get; set; }

}
