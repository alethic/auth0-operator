using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    public record V1ResourceServerSubjectTypeAuthorizationClient
    {

        [JsonPropertyName("policy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerSubjectTypeAuthorizationClientPolicy? Policy { get; set; }

    }

}
