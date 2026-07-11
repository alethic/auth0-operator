using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    public record V2alpha3BrandingThemeWidget
    {

        [JsonPropertyName("headerTextAlignment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeHeaderTextAlignment? HeaderTextAlignment { get; set; }

        [JsonPropertyName("logoHeight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? LogoHeight { get; set; }

        [JsonPropertyName("logoPosition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeLogoPosition? LogoPosition { get; set; }

        [JsonPropertyName("logoUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUrl { get; set; }

        [JsonPropertyName("socialButtonsLayout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeSocialButtonsLayout? SocialButtonsLayout { get; set; }
    }

}