using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionConnectionSettings
{

    [JsonPropertyName("pkce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionConnectionSettingsPkceEnum? Pkce { get; set; }

}
