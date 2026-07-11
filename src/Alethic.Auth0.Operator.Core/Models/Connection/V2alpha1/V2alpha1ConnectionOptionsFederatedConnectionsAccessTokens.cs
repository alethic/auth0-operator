using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Controls whether federated connection access tokens are enabled for the connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsFederatedConnectionsAccessTokens
    {

        /// <summary>
        /// When <c>true</c>, federated connection access tokens are enabled.
        /// </summary>
        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

    }

}
