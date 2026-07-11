using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Represents a single upstream parameter mapping forwarded to an identity provider on each authentication request.
    /// </summary>
    public record V2alpha3ConnectionUpstreamParam
    {

        /// <summary>
        /// The name of the parameter as expected by the upstream identity provider.
        /// </summary>
        [JsonPropertyName("alias")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Alias { get; set; }

        /// <summary>
        /// The name of the parameter as expected by the upstream identity provider.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Value { get; set; }

    }

}
