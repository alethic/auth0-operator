using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    public record V2alpha3ResourceServerSubjectTypeAuthorizationUser
    {

        [JsonPropertyName("policy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy? Policy { get; set; }

    }

}
