using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    public record V2alpha1ConnectionPasswordOptionsHistory
    {

        /// <summary>
        /// Enables password history checking to prevent users from reusing recent passwords
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Number of previous passwords to remember and prevent reuse (1-24)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("size")]
        public int? Size { get; set; }

    }

}
