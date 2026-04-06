using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1
{

    public record V1alpha1BrandingThemePageBackground
    {

        [JsonPropertyName("background_color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BackgroundColor { get; set; }

        [JsonPropertyName("background_image_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BackgroundImageUrl { get; set; }

        [JsonPropertyName("page_layout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1alpha1BrandingThemePageLayout? PageLayout { get; set; }

    }

}