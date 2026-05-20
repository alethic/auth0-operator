using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientAddonAzureSb
{

    [JsonPropertyName("namespace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Namespace { get; set; }

    [JsonPropertyName("sasKeyName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SasKeyName { get; set; }

    [JsonPropertyName("sasKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SasKey { get; set; }

    [JsonPropertyName("entityPath")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntityPath { get; set; }

    [JsonPropertyName("expiration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Expiration { get; set; }

}
