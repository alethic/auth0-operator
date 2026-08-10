using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

/// <summary>
/// Native social login configuration for the client.
/// </summary>
public record V2alpha3ClientNativeSocialLogin
{

    [JsonPropertyName("apple")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientNativeSocialLoginProvider? Apple { get; set; }

    [JsonPropertyName("facebook")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientNativeSocialLoginProvider? Facebook { get; set; }

    [JsonPropertyName("google")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientNativeSocialLoginProvider? Google { get; set; }

}
