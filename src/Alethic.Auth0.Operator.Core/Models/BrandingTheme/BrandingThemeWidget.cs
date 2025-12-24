using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    public class BrandingThemeWidget
    {

        [JsonPropertyName("header_text_alignment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HeaderTextAlignment? HeaderTextAlignment { get; set; }

        [JsonPropertyName("logo_height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? LogoHeight { get; set; }

        [JsonPropertyName("logo_position")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LogoPosition? LogoPosition { get; set; }

        [JsonPropertyName("logo_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogoUrl { get; set; }

        [JsonPropertyName("social_buttons_layout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SocialButtonsLayout? SocialButtonsLayout { get; set; }
    }

}