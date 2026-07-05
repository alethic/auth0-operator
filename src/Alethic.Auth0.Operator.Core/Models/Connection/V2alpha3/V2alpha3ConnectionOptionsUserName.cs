using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Minimum and maximum length constraints for usernames in a database connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsUserName
    {

        /// <summary>
        /// Minimum allowed username length.
        /// </summary>
        [JsonPropertyName("min")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Min { get; set; }

        /// <summary>
        /// Maximum allowed username length.
        /// </summary>
        [JsonPropertyName("max")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Max { get; set; }

    }

}