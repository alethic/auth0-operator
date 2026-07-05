using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Controls whether Auth0 rejects passwords that contain personal information such as the user's name or email address.
    /// </summary>
    public record V2alpha3ConnectionOptionsPasswordNoPersonalInfo
    {

        /// <summary>
        /// When <c>true</c>, passwords that contain the user's personal information are rejected.
        /// </summary>
        [JsonPropertyName("enable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enable { get; set; }

    }

}
