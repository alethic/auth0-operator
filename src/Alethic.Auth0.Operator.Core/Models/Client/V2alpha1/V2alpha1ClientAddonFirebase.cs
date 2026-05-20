using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientAddonFirebase
{

    [JsonPropertyName("secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Secret { get; set; }

    [JsonPropertyName("private_key_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateKeyId { get; set; }

    [JsonPropertyName("private_key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateKey { get; set; }

    [JsonPropertyName("client_email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientEmail { get; set; }

    [JsonPropertyName("lifetime_in_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LifetimeInSeconds { get; set; }

}
