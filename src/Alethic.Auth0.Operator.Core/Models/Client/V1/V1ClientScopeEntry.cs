using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public record V1ClientScopeEntry
    {

        [JsonPropertyName("actions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Actions { get; set; }

    }

}
