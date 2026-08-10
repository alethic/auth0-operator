using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    public record V2alpha3BrandingThemeConf
    {

        [JsonPropertyName("displayName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        [JsonPropertyName("borders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeBorders? Borders { get; set; }

        [JsonPropertyName("colors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeColors? Colors { get; set; }

        [JsonPropertyName("fonts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeFonts? Fonts { get; set; }

        [JsonPropertyName("identifiers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeIdentifiers? Identifiers { get; set; }

        [JsonPropertyName("pageBackground")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemePageBackground? PageBackground { get; set; }

        [JsonPropertyName("widget")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeWidget? Widget { get; set; }

    }

}
