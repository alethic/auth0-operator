using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1
{

    public record V1alpha1BrandingThemeConf
    {

        [JsonPropertyName("displayName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        [JsonPropertyName("borders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeBorders? Borders { get; set; }

        [JsonPropertyName("colors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeColors? Colors { get; set; }

        [JsonPropertyName("fonts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeFonts? Fonts { get; set; }

        [JsonPropertyName("page_background")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemePageBackground? PageBackground { get; set; }

        [JsonPropertyName("widget")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeWidget? Widget { get; set; }

    }

}
