using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

/// <summary>
/// Cross-App Access settings for the resource-application side of the connection.
/// </summary>
public record V2alpha3ConnectionCrossAppAccessResourceApp
{

    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum? Status { get; set; }

}
