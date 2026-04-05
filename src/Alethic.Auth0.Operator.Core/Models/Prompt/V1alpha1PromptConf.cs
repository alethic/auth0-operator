using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    public partial class V1alpha1PromptConf
    {

        [JsonPropertyName("partials")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Partials { get; set; }

        [JsonPropertyName("screens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, PromptScreen> Screens { get; set; }

    }

}
