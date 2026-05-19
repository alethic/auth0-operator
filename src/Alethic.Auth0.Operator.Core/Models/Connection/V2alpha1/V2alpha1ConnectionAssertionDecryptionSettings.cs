using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionAssertionDecryptionSettings
{

    [JsonPropertyName("algorithm_profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionAssertionDecryptionAlgorithmProfileEnum? AlgorithmProfile { get; set; }

    [JsonPropertyName("algorithm_exceptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AlgorithmExceptions { get; set; }

}
