using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Specifies how the passkey challenge is presented to the user in the Universal Login experience.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionChallengeUi
    {

        /// <summary>Both autofill and button challenge UI options are shown.</summary>
        [JsonStringEnumMemberName("both")]
        Both,

        /// <summary>Only the browser autofill (conditional mediation) UI is shown.</summary>
        [JsonStringEnumMemberName("autofill")]
        AutoFill,

        /// <summary>Only the explicit passkey button UI is shown.</summary>
        [JsonStringEnumMemberName("button")]
        Button

    }

}
