using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Lookup criteria used to locate an existing Auth0 connection resource.
    /// </summary>
    public record V2alpha1ConnectionFind
    {

        /// <summary>
        /// The ID of the connection to find.
        /// </summary>
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ConnectionId { get; set; }

    }

}
