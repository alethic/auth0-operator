using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionPasswordDictionaryOptions
{

    [JsonPropertyName("enable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Enable { get; set; }

    [JsonPropertyName("dictionary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Dictionary { get; set; }

}
