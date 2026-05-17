using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Passkey-specific options controlling the challenge UI and enrollment behaviour for the connection.
    /// </summary>
    public record V1ConnectionOptionsPasskeyOptions
    {

        /// <summary>
        /// Determines the UI presented to users when authenticating with a passkey (<c>both</c>, <c>autofill</c>, or <c>button</c>).
        /// </summary>
        [JsonPropertyName("challenge_ui")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionChallengeUi? ChallengeUi { get; set; }

        /// <summary>
        /// When <c>true</c>, users who log in with a password are offered the option to enroll a passkey.
        /// </summary>
        [JsonPropertyName("progressive_enrollment_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProgressiveEnrollmentEnabled { get; set; }

        /// <summary>
        /// When <c>true</c>, users can enroll passkeys that are bound to their local device.
        /// </summary>
        [JsonPropertyName("local_enrollment_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? LocalEnrollmentEnabled { get; set; }

    }

}