using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{
    public record V1ConnectionGoogleAppsFederatedConnectionsAccessTokens
    {

        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

    }

}
