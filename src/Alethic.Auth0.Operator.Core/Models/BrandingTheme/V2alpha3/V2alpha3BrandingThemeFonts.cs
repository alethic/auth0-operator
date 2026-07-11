using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    public record V2alpha3BrandingThemeFonts
    {

        [JsonPropertyName("bodyText")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFont? BodyText { get; set; }

        [JsonPropertyName("buttonsText")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFont? ButtonsText { get; set; }

        [JsonPropertyName("fontUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FontUrl { get; set; }

        [JsonPropertyName("inputLabels")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFont? InputLabels { get; set; }

        [JsonPropertyName("links")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFont? Links { get; set; }

        [JsonPropertyName("linksStyle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeLinksStyle? LinksStyle { get; set; }

        [JsonPropertyName("referenceTextSize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ReferenceTextSize { get; set; }

        [JsonPropertyName("subtitle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFont? Subtitle { get; set; }

        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFont? Title { get; set; }

    }

}