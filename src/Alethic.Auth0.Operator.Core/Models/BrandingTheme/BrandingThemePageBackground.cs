using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    public class BrandingThemePageBackground
    {

        [JsonPropertyName("background_color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BackgroundColor { get; set; }

        [JsonPropertyName("background_image_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BackgroundImageUrl { get; set; }

        [JsonPropertyName("page_layout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PageLayout? PageLayout { get; set; }

    }

}