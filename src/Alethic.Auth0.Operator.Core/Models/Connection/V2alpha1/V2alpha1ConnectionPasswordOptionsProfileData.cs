using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    public record V2alpha1ConnectionPasswordOptionsProfileData
    {

        /// <summary>
        /// Prevents users from including profile data (like name, email) in their passwords
        /// </summary>
        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

        /// <summary>
        /// Blocked profile fields. An array of up to 12 entries.
        /// </summary>
        [JsonPropertyName("blocked_fields")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? BlockedFields { get; set; }

    }

}
