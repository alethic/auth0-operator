using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    public class BrandingThemeFonts
    {

        [JsonPropertyName("body_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFont? BodyText { get; set; }

        [JsonPropertyName("buttons_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFont? ButtonsText { get; set; }

        [JsonPropertyName("font_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FontUrl { get; set; }

        [JsonPropertyName("input_labels")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFont? InputLabels { get; set; }

        [JsonPropertyName("links")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFont? Links { get; set; }

        [JsonPropertyName("links_style")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LinksStyle? LinksStyle { get; set; }

        [JsonPropertyName("reference_text_size")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ReferenceTextSize { get; set; }

        [JsonPropertyName("subtitle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFont? Subtitle { get; set; }

        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFont? Title { get; set; }

    }

}