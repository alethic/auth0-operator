using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Complexity requirements applied to passwords on a database connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsPasswordComplexityOptions
    {

        /// <summary>
        /// Minimum number of characters required in a password.
        /// </summary>
        [JsonPropertyName("min_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }

    }

}