using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionAuthenticationMethods
{

    [JsonPropertyName("password")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordAuthenticationMethod? Password { get; set; }

    [JsonPropertyName("passkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasskeyAuthenticationMethod? Passkey { get; set; }

    [JsonPropertyName("emailOtp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionEmailOtpAuthenticationMethod? EmailOtp { get; set; }

    [JsonPropertyName("phoneOtp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPhoneOtpAuthenticationMethod? PhoneOtp { get; set; }

}
