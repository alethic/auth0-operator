using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    public partial class BrandingThemeConf
    {

        [JsonPropertyName("displayName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        [JsonPropertyName("borders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeBorders? Borders { get; set; }

        [JsonPropertyName("colors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeColors? Colors { get; set; }

        [JsonPropertyName("fonts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeFonts? Fonts { get; set; }

        [JsonPropertyName("page_background")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemePageBackground? PageBackground { get; set; }

        [JsonPropertyName("widget")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BrandingThemeWidgets? Widget { get; set; }

    }

}
