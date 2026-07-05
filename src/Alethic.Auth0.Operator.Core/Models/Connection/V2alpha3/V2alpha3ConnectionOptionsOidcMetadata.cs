using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsOidcMetadata
{

    [JsonPropertyName("acrValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AcrValuesSupported { get; set; }

    [JsonPropertyName("authorizationEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthorizationEndpoint { get; set; }

    [JsonPropertyName("claimTypesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ClaimTypesSupported { get; set; }

    [JsonPropertyName("claimsLocalesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ClaimsLocalesSupported { get; set; }

    [JsonPropertyName("claimsParameterSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClaimsParameterSupported { get; set; }

    [JsonPropertyName("claimsSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ClaimsSupported { get; set; }

    [JsonPropertyName("displayValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DisplayValuesSupported { get; set; }

    [JsonPropertyName("dpopSigningAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DpopSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("endSessionEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EndSessionEndpoint { get; set; }

    [JsonPropertyName("grantTypesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTypesSupported { get; set; }

    [JsonPropertyName("idTokenEncryptionAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? IdTokenEncryptionAlgValuesSupported { get; set; }

    [JsonPropertyName("idTokenEncryptionEncValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? IdTokenEncryptionEncValuesSupported { get; set; }

    [JsonPropertyName("idTokenSigningAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? IdTokenSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("issuer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Issuer { get; set; }

    [JsonPropertyName("jwksUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? JwksUri { get; set; }

    [JsonPropertyName("opPolicyUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpPolicyUri { get; set; }

    [JsonPropertyName("opTosUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpTosUri { get; set; }

    [JsonPropertyName("registrationEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RegistrationEndpoint { get; set; }

    [JsonPropertyName("requestObjectEncryptionAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? RequestObjectEncryptionAlgValuesSupported { get; set; }

    [JsonPropertyName("requestObjectEncryptionEncValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? RequestObjectEncryptionEncValuesSupported { get; set; }

    [JsonPropertyName("requestObjectSigningAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? RequestObjectSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("requestParameterSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequestParameterSupported { get; set; }

    [JsonPropertyName("requestUriParameterSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequestUriParameterSupported { get; set; }

    [JsonPropertyName("requireRequestUriRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequireRequestUriRegistration { get; set; }

    [JsonPropertyName("responseModesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ResponseModesSupported { get; set; }

    [JsonPropertyName("responseTypesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ResponseTypesSupported { get; set; }

    [JsonPropertyName("scopesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ScopesSupported { get; set; }

    [JsonPropertyName("serviceDocumentation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ServiceDocumentation { get; set; }

    [JsonPropertyName("subjectTypesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? SubjectTypesSupported { get; set; }

    [JsonPropertyName("tokenEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TokenEndpoint { get; set; }

    [JsonPropertyName("tokenEndpointAuthMethodsSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? TokenEndpointAuthMethodsSupported { get; set; }

    [JsonPropertyName("tokenEndpointAuthSigningAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? TokenEndpointAuthSigningAlgValuesSupported { get; set; }

    [JsonPropertyName("uiLocalesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UiLocalesSupported { get; set; }

    [JsonPropertyName("userinfoEncryptionAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UserinfoEncryptionAlgValuesSupported { get; set; }

    [JsonPropertyName("userinfoEncryptionEncValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UserinfoEncryptionEncValuesSupported { get; set; }

    [JsonPropertyName("userinfoEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserinfoEndpoint { get; set; }

    [JsonPropertyName("userinfoSigningAlgValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? UserinfoSigningAlgValuesSupported { get; set; }

}
