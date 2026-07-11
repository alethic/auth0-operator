using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Complexity requirements applied to passwords on a database connection.
    /// </summary>
    public record V2alpha3ConnectionOptionsPasswordComplexityOptions
    {

        /// <summary>
        /// Minimum number of characters required in a password.
        /// </summary>
        [JsonPropertyName("minLength")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }

    }

}