using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientMobile
{

    [JsonPropertyName("android")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientMobileAndroid? Android { get; set; }

    [JsonPropertyName("ios")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientMobileiOs? Ios { get; set; }

}
