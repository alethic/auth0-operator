using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionUsernameAttributeIdentifier
{

    [JsonPropertyName("active")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

    /// <summary>
    /// Retained for schema compatibility with the former shared attribute identifier type; Auth0 does not define
    /// a default method for username identifiers and the value is never applied.
    /// </summary>
    [JsonPropertyName("defaultMethod")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionDefaultMethodEmailIdentifierEnum? DefaultMethod { get; set; }

}
