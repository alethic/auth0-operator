using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientRefreshTokenConfiguration
{

    [JsonPropertyName("rotation_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientRefreshTokenRotationTypeEnum? RotationType { get; set; }

    [JsonPropertyName("expiration_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientRefreshTokenExpirationTypeEnum? ExpirationType { get; set; }

    [JsonPropertyName("leeway")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Leeway { get; set; }

    [JsonPropertyName("token_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TokenLifetime { get; set; }

    [JsonPropertyName("infinite_token_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? InfiniteTokenLifetime { get; set; }

    [JsonPropertyName("idle_token_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IdleTokenLifetime { get; set; }

    [JsonPropertyName("infinite_idle_token_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? InfiniteIdleTokenLifetime { get; set; }

    [JsonPropertyName("policies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ClientRefreshTokenPolicy[]? Policies { get; set; }

}
