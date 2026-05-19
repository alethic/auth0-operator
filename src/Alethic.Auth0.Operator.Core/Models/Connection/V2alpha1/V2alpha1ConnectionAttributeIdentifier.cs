using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionAttributeIdentifier
{

    [JsonPropertyName("active")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

    [JsonPropertyName("default_method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionDefaultMethodEmailIdentifierEnum? DefaultMethod { get; set; }

}
