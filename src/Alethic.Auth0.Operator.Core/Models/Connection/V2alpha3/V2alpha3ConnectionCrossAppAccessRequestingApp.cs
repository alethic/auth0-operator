using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

/// <summary>
/// Cross-App Access settings for the requesting-application side of the connection.
/// </summary>
public record V2alpha3ConnectionCrossAppAccessRequestingApp
{

    [JsonPropertyName("active")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

}
