using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionAssertionDecryptionSettings
{

    [JsonPropertyName("algorithmProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum? AlgorithmProfile { get; set; }

    [JsonPropertyName("algorithmExceptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AlgorithmExceptions { get; set; }

}
