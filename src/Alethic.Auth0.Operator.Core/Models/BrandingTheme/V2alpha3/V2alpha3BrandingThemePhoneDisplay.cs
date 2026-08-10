using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    /// <summary>
    /// Phone number display settings for the identifier-first login experience.
    /// </summary>
    public record V2alpha3BrandingThemePhoneDisplay
    {

        [JsonPropertyName("formatting")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemePhoneFormatting? Formatting { get; set; }

        [JsonPropertyName("masking")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemePhoneMasking? Masking { get; set; }

    }

}
