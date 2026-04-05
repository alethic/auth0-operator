using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{
    public class V1ClientScopes
    {

        [JsonPropertyName("users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopeEntry? Users { get; set; }

        [JsonPropertyName("users_app_metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopeEntry? UsersAppMetadata { get; set; }

        [JsonPropertyName("clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopeEntry? Clients { get; set; }

        [JsonPropertyName("client_keys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopeEntry? ClientKeys { get; set; }

        [JsonPropertyName("tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopeEntry? Tokens { get; set; }

        [JsonPropertyName("stats")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientScopeEntry? Stats { get; set; }

    }

}
