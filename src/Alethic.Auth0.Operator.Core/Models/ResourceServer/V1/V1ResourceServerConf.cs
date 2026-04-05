using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    public partial class V1ResourceServerConf
    {

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identifier { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("scopes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<V1ResourceServerScope>? Scopes { get; set; }

        [JsonPropertyName("signing_alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerSigningAlgorithm? SigningAlgorithm { get; set; }

        [JsonPropertyName("signing_secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SigningSecret { get; set; }

        [JsonPropertyName("token_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TokenLifetime { get; set; }

        [JsonPropertyName("token_lifetime_for_web")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TokenLifetimeForWeb { get; set; }

        [JsonPropertyName("allow_offline_access")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowOfflineAccess { get; set; }

        [JsonPropertyName("skip_consent_for_verifiable_first_party_clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SkipConsentForVerifiableFirstPartyClients { get; set; }

        [JsonPropertyName("verificationLocation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VerificationLocation { get; set; }

        [JsonPropertyName("token_dialect")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerTokenDialect? TokenDialect { get; set; }

        [JsonPropertyName("enforce_policies")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnforcePolicies { get; set; }

        [JsonPropertyName("consent_policy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerConsentPolicy? ConsentPolicy { get; set; }

        [JsonPropertyName("authorization_details")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<V1ResourceServerAuthorizationDetail>? AuthorizationDetails { get; set; }

        [JsonPropertyName("token_encryption")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerTokenEncryption? TokenEncryption { get; set; }

        [JsonPropertyName("proof_of_possession")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerProofOfPossession? ProofOfPossession { get; set; }

    }

}
