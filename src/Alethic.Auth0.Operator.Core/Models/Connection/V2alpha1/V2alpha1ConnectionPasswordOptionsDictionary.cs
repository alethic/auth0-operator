using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionPasswordOptionsDictionary
{

    [JsonPropertyName("active")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

    [JsonPropertyName("custom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Custom { get; set; }

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionPasswordDefaultDictionariesEnum? Default { get; set; }

}
