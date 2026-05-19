using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionPasswordAuthenticationMethod
{

    [JsonPropertyName("enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Enabled { get; set; }

    [JsonPropertyName("api_behavior")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionApiBehaviorEnum? ApiBehavior { get; set; }

    [JsonPropertyName("signup_behavior")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSignupBehaviorEnum? SignupBehavior { get; set; }

}
