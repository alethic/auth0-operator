using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    public class PromptScreenPartialInsertionPoints
    {

        [JsonPropertyName("form_content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormContent { get; set; }

        [JsonPropertyName("form_content_emd")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormContentEnd { get; set; }

        [JsonPropertyName("form_content_start")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormContentStart { get; set; }

        [JsonPropertyName("form_footer_end")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormFooterEnd { get; set; }

        [JsonPropertyName("form_footer_start")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FormFooterStart { get; set; }

        [JsonPropertyName("secondary_actions_end")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SecondaryActionsEnd { get; set; }

        [JsonPropertyName("secondary_actions_start")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SecondaryActionsStart { get; set; }

    }

}