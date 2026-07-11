using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionPasswordOptions
{

    [JsonPropertyName("complexity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordOptionsComplexity? Complexity { get; set; }

    [JsonPropertyName("dictionary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordOptionsDictionary? Dictionary { get; set; }

    [JsonPropertyName("history")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordOptionsHistory? History { get; set; }

    [JsonPropertyName("profileData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordOptionsProfileData? ProfileData { get; set; }

}
