using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionEmailEmail
{

    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Body { get; set; }

    [JsonPropertyName("from")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? From { get; set; }

    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subject { get; set; }

    [JsonPropertyName("syntax")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionEmailEmailSyntax? Syntax { get; set; }

}
