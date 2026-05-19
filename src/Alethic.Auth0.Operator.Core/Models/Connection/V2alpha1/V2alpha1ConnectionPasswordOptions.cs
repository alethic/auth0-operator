using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionPasswordOptions
{

    [JsonPropertyName("complexity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordOptionsComplexity? Complexity { get; set; }

    [JsonPropertyName("dictionary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordOptionsDictionary? Dictionary { get; set; }

    [JsonPropertyName("history")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordOptionsHistory? History { get; set; }

    [JsonPropertyName("profile_data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordOptionsProfileData? ProfileData { get; set; }

}
