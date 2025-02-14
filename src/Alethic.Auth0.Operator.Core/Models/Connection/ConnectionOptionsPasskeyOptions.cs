﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection
{

    public class ConnectionOptionsPasskeyOptions
    {

        [JsonPropertyName("challenge_ui")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChallengeUi? ChallengeUi { get; set; }

        [JsonPropertyName("progressive_enrollment_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProgressiveEnrollmentEnabled { get; set; }

        [JsonPropertyName("local_enrollment_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? LocalEnrollmentEnabled { get; set; }

    }

}