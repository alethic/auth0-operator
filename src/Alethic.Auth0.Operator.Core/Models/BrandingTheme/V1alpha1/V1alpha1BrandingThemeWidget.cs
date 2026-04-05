using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1
{

    public class V1alpha1BrandingThemeWidget
    {

        [JsonPropertyName("header_text_alignment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeHeaderTextAlignment? HeaderTextAlignment { get; set; }

        [JsonPropertyName("logo_height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? LogoHeight { get; set; }

        [JsonPropertyName("logo_position")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeLogoPosition? LogoPosition { get; set; }

        [JsonPropertyName("logo_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUrl { get; set; }

        [JsonPropertyName("social_buttons_layout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemeSocialButtonsLayout? SocialButtonsLayout { get; set; }
    }

}