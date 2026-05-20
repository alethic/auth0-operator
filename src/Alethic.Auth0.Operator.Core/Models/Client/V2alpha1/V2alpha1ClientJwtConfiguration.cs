using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientJwtConfiguration
{

    [JsonPropertyName("lifetime_in_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LifetimeInSeconds { get; set; }

    [JsonPropertyName("secret_encoded")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SecretEncoded { get; set; }

    [JsonPropertyName("scopes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Scopes { get; set; }

    [JsonPropertyName("alg")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientSigningAlgorithmEnum? Alg { get; set; }

}
