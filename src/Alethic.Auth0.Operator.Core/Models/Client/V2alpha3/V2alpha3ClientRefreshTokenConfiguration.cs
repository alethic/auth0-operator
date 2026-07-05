using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientRefreshTokenConfiguration
{

    [JsonPropertyName("rotationType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientRefreshTokenRotationTypeEnum? RotationType { get; set; }

    [JsonPropertyName("expirationType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientRefreshTokenExpirationTypeEnum? ExpirationType { get; set; }

    [JsonPropertyName("leeway")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Leeway { get; set; }

    [JsonPropertyName("tokenLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TokenLifetime { get; set; }

    [JsonPropertyName("infiniteTokenLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? InfiniteTokenLifetime { get; set; }

    [JsonPropertyName("idleTokenLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IdleTokenLifetime { get; set; }

    [JsonPropertyName("infiniteIdleTokenLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? InfiniteIdleTokenLifetime { get; set; }

    [JsonPropertyName("policies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientRefreshTokenPolicy[]? Policies { get; set; }

}
