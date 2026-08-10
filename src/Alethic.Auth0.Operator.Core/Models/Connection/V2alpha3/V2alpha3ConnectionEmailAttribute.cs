using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionEmailAttribute
{

    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionEmailAttributeIdentifier? Identifier { get; set; }

    [JsonPropertyName("unique")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Unique { get; set; }

    [JsonPropertyName("profileRequired")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ProfileRequired { get; set; }

    [JsonPropertyName("verificationMethod")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionVerificationMethodEnum? VerificationMethod { get; set; }

    [JsonPropertyName("signup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionSignupVerified? Signup { get; set; }

}
