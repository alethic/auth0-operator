using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Username validation constraints applied when creating or updating a user in a database connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsValidation
    {

        /// <summary>
        /// Minimum and maximum length rules for usernames.
        /// </summary>
        [JsonPropertyName("username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsUserName? UserName { get; set; }

    }

}