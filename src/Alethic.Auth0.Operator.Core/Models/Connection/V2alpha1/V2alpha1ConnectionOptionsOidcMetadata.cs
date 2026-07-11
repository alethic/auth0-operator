using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsOidcMetadata
{

    [JsonPropertyName("acr_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AcrValuesSupported { get; set; }

    [JsonPropertyName("authorization_endpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthorizationEndpoint { get; set; }

    [JsonPropertyName("claim_types_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ClaimTypesSupported { get; set; }

    [JsonPropertyName("claims_locales_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ClaimsLocalesSupported { get; set; }

    [JsonPropertyName("claims_parameter_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClaimsParameterSupported { get; set; }

    [JsonPropertyName("claims_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ClaimsSupported { get; set; }

    [JsonPropertyName("display_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DisplayValuesSupported { get; set; }

    [JsonPropertyName("dpop_signing_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DpopSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("end_session_endpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EndSessionEndpoint { get; set; }

    [JsonPropertyName("grant_types_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTypesSupported { get; set; }

    [JsonPropertyName("id_token_encryption_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? IdTokenEncryptionAlgValuesSupported { get; set; }

    [JsonPropertyName("id_token_encryption_enc_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? IdTokenEncryptionEncValuesSupported { get; set; }

    [JsonPropertyName("id_token_signing_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? IdTokenSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("issuer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Issuer { get; set; }

    [JsonPropertyName("jwks_uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? JwksUri { get; set; }

    [JsonPropertyName("op_policy_uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpPolicyUri { get; set; }

    [JsonPropertyName("op_tos_uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpTosUri { get; set; }

    [JsonPropertyName("registration_endpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RegistrationEndpoint { get; set; }

    [JsonPropertyName("request_object_encryption_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? RequestObjectEncryptionAlgValuesSupported { get; set; }

    [JsonPropertyName("request_object_encryption_enc_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? RequestObjectEncryptionEncValuesSupported { get; set; }

    [JsonPropertyName("request_object_signing_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? RequestObjectSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("request_parameter_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequestParameterSupported { get; set; }

    [JsonPropertyName("request_uri_parameter_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequestUriParameterSupported { get; set; }

    [JsonPropertyName("require_request_uri_registration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequireRequestUriRegistration { get; set; }

    [JsonPropertyName("response_modes_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ResponseModesSupported { get; set; }

    [JsonPropertyName("response_types_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ResponseTypesSupported { get; set; }

    [JsonPropertyName("scopes_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ScopesSupported { get; set; }

    [JsonPropertyName("service_documentation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ServiceDocumentation { get; set; }

    [JsonPropertyName("subject_types_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? SubjectTypesSupported { get; set; }

    [JsonPropertyName("token_endpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TokenEndpoint { get; set; }

    [JsonPropertyName("token_endpoint_auth_methods_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? TokenEndpointAuthMethodsSupported { get; set; }

    [JsonPropertyName("token_endpoint_auth_signing_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? TokenEndpointAuthSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("ui_locales_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UiLocalesSupported { get; set; }

    [JsonPropertyName("userinfo_encryption_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UserinfoEncryptionAlgValuesSupported { get; set; }

    [JsonPropertyName("userinfo_encryption_enc_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UserinfoEncryptionEncValuesSupported { get; set; }

    [JsonPropertyName("userinfo_endpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserinfoEndpoint { get; set; }

    [JsonPropertyName("userinfo_signing_alg_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UserinfoSigningAlgValuesSupported { get; set; }

}
