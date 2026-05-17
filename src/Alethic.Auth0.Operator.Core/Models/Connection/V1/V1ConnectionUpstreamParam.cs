using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Represents a single upstream parameter mapping forwarded to an identity provider on each authentication request.
    /// </summary>
    public record V1ConnectionUpstreamParam
    {

        /// <summary>
        /// The name of the parameter as expected by the upstream identity provider.
        /// </summary>
        [JsonPropertyName("alias")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Alias { get; set; }

    }

}
