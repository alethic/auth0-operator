using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionPasskeyOptions
{

    [JsonPropertyName("challengeUi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasskeyChallengeUiEnum? ChallengeUi { get; set; }

    [JsonPropertyName("progressiveEnrollmentEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ProgressiveEnrollmentEnabled { get; set; }

    [JsonPropertyName("localEnrollmentEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LocalEnrollmentEnabled { get; set; }

}
