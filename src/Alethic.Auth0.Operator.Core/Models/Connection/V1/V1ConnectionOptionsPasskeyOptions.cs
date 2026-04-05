using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public class V1ConnectionOptionsPasskeyOptions
    {

        [JsonPropertyName("challenge_ui")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionChallengeUi? ChallengeUi { get; set; }

        [JsonPropertyName("progressive_enrollment_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProgressiveEnrollmentEnabled { get; set; }

        [JsonPropertyName("local_enrollment_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? LocalEnrollmentEnabled { get; set; }

    }

}