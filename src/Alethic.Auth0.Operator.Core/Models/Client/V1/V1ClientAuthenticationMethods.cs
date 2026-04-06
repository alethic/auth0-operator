using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientAuthenticationMethods
    {

        public record CredentialIdDef
        {

            [JsonPropertyName("id")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Id { get; set; }

        }

        public record PrivateKeyJwtDef
        {

            [JsonPropertyName("credentials")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public CredentialIdDef[]? Credentials { get; set; }

        }

        public record TlsClientAuthDef
        {

            [JsonPropertyName("credentials")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public CredentialIdDef[]? Credentials { get; set; }

        }
        public record SelfSignedTlsClientAuthDef
        {

            [JsonPropertyName("credentials")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public CredentialIdDef[]? Credentials { get; set; }

        }

        [JsonPropertyName("private_key_jwt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PrivateKeyJwtDef? PrivateKeyJwt { get; set; }

        [JsonPropertyName("tls_client_auth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TlsClientAuthDef? TlsClientAuth { get; set; }

        [JsonPropertyName("self_signed_tls_client_auth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SelfSignedTlsClientAuthDef? SelfSignedTlsClientAuth { get; set; }

    }

}
