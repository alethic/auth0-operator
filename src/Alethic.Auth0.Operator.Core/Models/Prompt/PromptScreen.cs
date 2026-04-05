using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    public class PromptScreen
    {

        [JsonPropertyName("renderer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PromptScreenRenderer? Renderer { get; set; }

        [JsonPropertyName("partials")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, PromptScreenPartial> Partials { get; set; }

    }

}
