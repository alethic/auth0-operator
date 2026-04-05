using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    public class PromptScreenPartial
    {

        [JsonPropertyName("renderer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PromptScreenPartialInsertionPoints? InsertionPoints { get; set; }

    }

}