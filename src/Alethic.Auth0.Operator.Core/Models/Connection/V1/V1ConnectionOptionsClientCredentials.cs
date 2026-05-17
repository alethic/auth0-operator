using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Common client credentials (client_id / client_secret) shared by many social and enterprise connection option types.
    /// </summary>
    public record V1ConnectionOptionsClientCredentials
    {

        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientSecret { get; set; }

    }

}
