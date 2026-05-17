using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Controls whether a verification step (e.g. email or SMS OTP) is active for a given signup attribute.
    /// </summary>
    public record V1ConnectionOptionsVerification
    {

        /// <summary>
        /// When <c>true</c>, a verification message is sent after signup for this attribute.
        /// </summary>
        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

    }

}