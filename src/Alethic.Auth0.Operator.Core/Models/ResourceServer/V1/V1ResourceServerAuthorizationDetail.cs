using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    public record V1ResourceServerAuthorizationDetail
    {

        [JsonPropertyName("type")]
        public string? Type { get; set; }

    }

}
