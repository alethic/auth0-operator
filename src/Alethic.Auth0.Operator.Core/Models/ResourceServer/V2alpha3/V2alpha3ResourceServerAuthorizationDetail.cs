using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    public record V2alpha3ResourceServerAuthorizationDetail
    {

        [JsonPropertyName("type")]
        public string? Type { get; set; }

    }

}
