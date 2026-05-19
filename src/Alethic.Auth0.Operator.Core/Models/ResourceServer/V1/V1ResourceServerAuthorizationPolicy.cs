using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    public record V1ResourceServerAuthorizationPolicy
    {

        [JsonPropertyName("policy_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PolicyId { get; set; }

    }

}