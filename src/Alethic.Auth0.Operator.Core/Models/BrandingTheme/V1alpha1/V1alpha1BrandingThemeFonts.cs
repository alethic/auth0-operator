using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1
{

    public class V1alpha1BrandingThemeFonts
    {

        [JsonPropertyName("body_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFont? BodyText { get; set; }

        [JsonPropertyName("buttons_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFont? ButtonsText { get; set; }

        [JsonPropertyName("font_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FontUrl { get; set; }

        [JsonPropertyName("input_labels")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFont? InputLabels { get; set; }

        [JsonPropertyName("links")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFont? Links { get; set; }

        [JsonPropertyName("links_style")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeLinksStyle? LinksStyle { get; set; }

        [JsonPropertyName("reference_text_size")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ReferenceTextSize { get; set; }

        [JsonPropertyName("subtitle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFont? Subtitle { get; set; }

        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFont? Title { get; set; }

    }

}