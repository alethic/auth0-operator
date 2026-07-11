using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    public record V2alpha3ResourceServerSubjectTypeAuthorization
    {

        [JsonPropertyName("client")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerSubjectTypeAuthorizationClient? Client { get; set; }

        [JsonPropertyName("user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerSubjectTypeAuthorizationUser? User { get; set; }

    }

}