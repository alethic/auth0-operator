using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Google Firebase addon configuration.
/// </summary>
public class V1ClientAddonFirebase
{

    /// <summary>
    /// Google Firebase Secret. (SDK 2 only).
    /// </summary>
    [JsonPropertyName("secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Secret { get; set; }

    /// <summary>
    /// Optional ID of the private key to obtain kid header in the issued token (SDK v3+ tokens only).
    /// </summary>
    [JsonPropertyName("private_key_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateKeyId { get; set; }

    /// <summary>
    /// Private Key for signing the token (SDK v3+ tokens only).
    /// </summary>
    [JsonPropertyName("private_key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateKey { get; set; }

    /// <summary>
    /// ID of the Service Account you have created (shown as `client_email` in the generated JSON file, SDK v3+ tokens only).
    /// </summary>
    [JsonPropertyName("client_email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientEmail { get; set; }

    /// <summary>
    /// Optional expiration in seconds for the generated token. Defaults to 3600 seconds (SDK v3+ tokens only).
    /// </summary>
    [JsonPropertyName("lifetime_in_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LifetimeInSeconds { get; set; }

}
