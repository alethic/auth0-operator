using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientOidcLogoutConfig
    {

        [JsonPropertyName("backchannel_logout_urls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? BackchannelLogoutUrls { get; set; }

        [JsonPropertyName("backchannel_logout_initiators")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientBackchannelLogoutInitiators? BackchannelLogoutInitiators { get; set; }

    }

}
