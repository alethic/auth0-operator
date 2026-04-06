using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientJwtConfiguration
    {

        [JsonPropertyName("secret_encoded")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsSecretEncoded { get; set; }

        [JsonPropertyName("lifetime_in_seconds")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LifetimeInSeconds { get; set; }

        [JsonPropertyName("scopes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopes? Scopes { get; set; }

        [JsonPropertyName("alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SigningAlgorithm { get; set; }

    }

}
