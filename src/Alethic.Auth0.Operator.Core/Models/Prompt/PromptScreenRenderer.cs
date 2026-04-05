using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    public class PromptScreenRenderer
    {

        [JsonPropertyName("rendering_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PromptScreenRenderingMode? RenderingMode { get; set; }

        [JsonPropertyName("default_head_tags_disabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DefaultHeadTagsDisabled { get; set; }

        [JsonPropertyName("filters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PromptScreenRendererFilter[]? Filters { get; set; }

        [JsonPropertyName("context_configuration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? ContextConfiguration { get; set; }

        [JsonPropertyName("head_tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? HeadTags { get; set; }

        [JsonPropertyName("use_page_template")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UsePageTemplate { get; set; }

    }

}