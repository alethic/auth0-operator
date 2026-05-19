using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionEmailAttribute
{

    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionAttributeIdentifier? Identifier { get; set; }

    [JsonPropertyName("unique")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Unique { get; set; }

    [JsonPropertyName("profile_required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ProfileRequired { get; set; }

    [JsonPropertyName("verification_method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionVerificationMethodEnum? VerificationMethod { get; set; }

    [JsonPropertyName("signup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSignupVerified? Signup { get; set; }

}
