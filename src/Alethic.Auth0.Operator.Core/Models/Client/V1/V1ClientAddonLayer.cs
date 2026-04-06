using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Layer addon configuration.
/// </summary>
public record V1ClientAddonLayer
{

    /// <summary>
    /// Provider ID of your Layer account
    /// </summary>
    [JsonPropertyName("providerId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public  string? ProviderId { get; set; }

    /// <summary>
    /// Authentication Key identifier used to sign the Layer token.
    /// </summary>
    [JsonPropertyName("keyId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public  string? KeyId { get; set; }

    /// <summary>
    /// Private key for signing the Layer token.
    /// </summary>
    [JsonPropertyName("privateKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public  string? PrivateKey { get; set; }

    /// <summary>
    /// Name of the property used as the unique user id in Layer. If not specified `user_id` is used.
    /// </summary>
    [JsonPropertyName("principal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Principal { get; set; }

    /// <summary>
    /// Optional expiration in minutes for the generated token. Defaults to 5 minutes.
    /// </summary>
    [JsonPropertyName("expiration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Expiration { get; set; }

}
