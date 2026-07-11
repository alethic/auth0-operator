using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Passkey-specific options controlling the challenge UI and enrollment behaviour for the connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsPasskeyOptions
    {

        /// <summary>
        /// Determines the UI presented to users when authenticating with a passkey (<c>both</c>, <c>autofill</c>, or <c>button</c>).
        /// </summary>
        [JsonPropertyName("challengeUi")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionChallengeUi? ChallengeUi { get; set; }

        /// <summary>
        /// When <c>true</c>, users who log in with a password are offered the option to enroll a passkey.
        /// </summary>
        [JsonPropertyName("progressiveEnrollmentEnabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProgressiveEnrollmentEnabled { get; set; }

        /// <summary>
        /// When <c>true</c>, users can enroll passkeys that are bound to their local device.
        /// </summary>
        [JsonPropertyName("localEnrollmentEnabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? LocalEnrollmentEnabled { get; set; }

    }

}