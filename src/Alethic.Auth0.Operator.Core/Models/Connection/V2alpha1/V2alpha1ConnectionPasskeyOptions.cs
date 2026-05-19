using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
public record V2alpha1ConnectionPasskeyOptions
{
    [JsonPropertyName("challenge_ui")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasskeyChallengeUiEnum? ChallengeUi { get; set; }

    [JsonPropertyName("progressive_enrollment_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ProgressiveEnrollmentEnabled { get; set; }

    [JsonPropertyName("local_enrollment_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LocalEnrollmentEnabled { get; set; }
}
