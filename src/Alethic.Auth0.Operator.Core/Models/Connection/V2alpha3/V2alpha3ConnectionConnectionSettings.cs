using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionConnectionSettings
{

    [JsonPropertyName("pkce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionConnectionSettingsPkceEnum? Pkce { get; set; }

}
