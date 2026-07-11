using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    public record V2alpha3ResourceServerTokenEncryption
    {

        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerTokenFormat? Format { get; set; }

        [JsonPropertyName("encryptionKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerTokenEncryptionKey? EncryptionKey { get; set; }

    }

}
