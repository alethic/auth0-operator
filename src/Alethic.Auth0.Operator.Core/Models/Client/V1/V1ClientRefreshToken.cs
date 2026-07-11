using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientRefreshToken
    {

        [JsonPropertyName("rotation_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientRefreshTokenRotationType? RotationType { get; set; }

        [JsonPropertyName("expiration_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientRefreshTokenExpirationType? ExpirationType { get; set; }

        [JsonPropertyName("leeway")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Leeway { get; set; }

        [JsonPropertyName("token_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TokenLifetime { get; set; }

        [JsonPropertyName("infinite_token_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? InfiniteTokenLifetime { get; set; }

        [JsonPropertyName("idle_token_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? IdleTokenLifetime { get; set; }

        [JsonPropertyName("infinite_idle_token_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? InfiniteIdleTokenLifetime { get; set; }
    }

}
