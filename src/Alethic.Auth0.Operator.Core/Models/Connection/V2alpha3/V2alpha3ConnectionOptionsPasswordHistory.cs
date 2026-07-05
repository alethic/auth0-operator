using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Controls whether previously used passwords are remembered and rejected during password changes.
    /// </summary>
    public record V2alpha3ConnectionOptionsPasswordHistory
    {

        /// <summary>
        /// When <c>true</c>, Auth0 keeps a history of past passwords and prevents reuse.
        /// </summary>
        [JsonPropertyName("enable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enable { get; set; }

        /// <summary>
        /// Number of previous passwords to retain and compare against.
        /// </summary>
        [JsonPropertyName("size")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Size { get; set; }

    }

}