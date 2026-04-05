using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    public class V1ResourceServerTokenEncryption
    {

        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerTokenFormat? Format { get; set; }

        [JsonPropertyName("encryption_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerTokenEncryptionKey? EncryptionKey { get; set; }

    }

}
