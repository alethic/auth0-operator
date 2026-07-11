using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    public record V2alpha3ResourceServerConf
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
        public V2alpha3ResourceServerScope[]? Scopes { get; set; }

        [JsonPropertyName("signingAlg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerSigningAlgorithm? SigningAlgorithm { get; set; }

        [JsonPropertyName("signingSecret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SigningSecret { get; set; }

        [JsonPropertyName("tokenLifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TokenLifetime { get; set; }

        [JsonPropertyName("tokenLifetimeForWeb")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TokenLifetimeForWeb { get; set; }

        [JsonPropertyName("allowOfflineAccess")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowOfflineAccess { get; set; }

        [JsonPropertyName("allowOnlineAccess")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowOnlineAccess { get; set; }

        [JsonPropertyName("allowOnlineAccessWithEphemeralSessions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowOnlineAccessWithEphemeralSessions { get; set; }

        [JsonPropertyName("skipConsentForVerifiableFirstPartyClients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SkipConsentForVerifiableFirstPartyClients { get; set; }

        [JsonPropertyName("verificationLocation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VerificationLocation { get; set; }

        [JsonPropertyName("tokenDialect")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerTokenDialect? TokenDialect { get; set; }

        [JsonPropertyName("enforcePolicies")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnforcePolicies { get; set; }

        [JsonPropertyName("consentPolicy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerConsentPolicy? ConsentPolicy { get; set; }

        [JsonPropertyName("authorizationDetails")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerAuthorizationDetail[]? AuthorizationDetails { get; set; }

        [JsonPropertyName("authorizationPolicy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerAuthorizationPolicy? AuthorizationPolicy { get; set; }

        [JsonPropertyName("subjectTypeAuthorization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerSubjectTypeAuthorization? SubjectTypeAuthorization { get; set; }

        [JsonPropertyName("tokenEncryption")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerTokenEncryption? TokenEncryption { get; set; }

        [JsonPropertyName("proofOfPossession")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ResourceServerProofOfPossession? ProofOfPossession { get; set; }

    }

}
