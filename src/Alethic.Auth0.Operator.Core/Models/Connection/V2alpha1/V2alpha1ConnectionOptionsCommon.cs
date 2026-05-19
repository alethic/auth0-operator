using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsCommon
{

    [JsonPropertyName("non_persistent_attrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
