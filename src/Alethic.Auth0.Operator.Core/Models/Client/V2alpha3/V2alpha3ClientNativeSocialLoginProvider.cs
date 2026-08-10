using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

/// <summary>
/// Native social login settings for a single provider.
/// </summary>
public record V2alpha3ClientNativeSocialLoginProvider
{

    [JsonPropertyName("enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Enabled { get; set; }

}
