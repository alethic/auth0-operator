using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
public record V2alpha1ConnectionAuthenticationMethods
{
    [JsonPropertyName("password")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordAuthenticationMethod? Password { get; set; }

    [JsonPropertyName("passkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasskeyAuthenticationMethod? Passkey { get; set; }

    [JsonPropertyName("email_otp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionEmailOtpAuthenticationMethod? EmailOtp { get; set; }

    [JsonPropertyName("phone_otp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPhoneOtpAuthenticationMethod? PhoneOtp { get; set; }
}
