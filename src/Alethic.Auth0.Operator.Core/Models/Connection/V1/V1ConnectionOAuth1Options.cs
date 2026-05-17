using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionOAuth1Options : V1ConnectionOptionsClientCredentials
    {

        [JsonPropertyName("requestTokenUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RequestTokenUrl { get; set; }

        [JsonPropertyName("accessTokenUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccessTokenUrl { get; set; }

        [JsonPropertyName("userAuthorizationUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserAuthorizationUrl { get; set; }

        [JsonPropertyName("signatureMethod")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SignatureMethod { get; set; }

        [JsonPropertyName("scripts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Scripts { get; set; }

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? UpstreamParams { get; set; }

    }

}
