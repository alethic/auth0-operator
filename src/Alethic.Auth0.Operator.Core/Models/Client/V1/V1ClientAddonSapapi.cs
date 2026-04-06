using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// SAP API addon configuration.
/// </summary>
public record V1ClientAddonSapapi
{

    /// <summary>
    /// If activated in the OAuth 2.0 client configuration (transaction SOAUTH2) the SAML attribute client_id must be set and equal the client_id form parameter of the access token request.
    /// </summary>
    [JsonPropertyName("clientid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Clientid { get; set; }

    /// <summary>
    /// Name of the property in the user object that maps to a SAP username. e.g. `email`.
    /// </summary>
    [JsonPropertyName("usernameAttribute")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UsernameAttribute { get; set; }

    /// <summary>
    /// Your SAP OData server OAuth2 token endpoint URL.
    /// </summary>
    [JsonPropertyName("tokenEndpointUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TokenEndpointUrl { get; set; }

    /// <summary>
    /// Requested scope for SAP APIs.
    /// </summary>
    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Scope { get; set; }

    /// <summary>
    /// Service account password to use to authenticate API calls to the token endpoint.
    /// </summary>
    [JsonPropertyName("servicePassword")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ServicePassword { get; set; }

    /// <summary>
    /// NameID element of the Subject which can be used to express the user's identity. Defaults to `urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified`.
    /// </summary>
    [JsonPropertyName("nameIdentifierFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NameIdentifierFormat { get; set; }

}
