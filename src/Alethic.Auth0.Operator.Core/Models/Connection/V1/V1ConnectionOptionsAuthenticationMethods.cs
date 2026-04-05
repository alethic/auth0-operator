using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public class V1ConnectionOptionsAuthenticationMethods
    {

        [JsonPropertyName("password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordAuthenticationMethod? Password { get; set; }

        [JsonPropertyName("passkey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasskeyAuthenticationMethod? Passkey { get; set; }

    }

}