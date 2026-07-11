using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    public record V2alpha3BrandingThemeBorders
    {

        [JsonPropertyName("buttonBorderRadius")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ButtonBorderRadius { get; set; }

        [JsonPropertyName("buttonBorderWeight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ButtonBorderWeight { get; set; }

        [JsonPropertyName("buttonsStyle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeButtonsStyle? ButtonsStyle { get; set; }

        [JsonPropertyName("inputBorderRadius")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? InputBorderRadius { get; set; }

        [JsonPropertyName("inputBorderWeight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? InputBorderWeight { get; set; }

        [JsonPropertyName("inputsStyle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeButtonsStyle? InputsStyle { get; set; }

        [JsonPropertyName("showWidgetShadow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowWidgetShadow { get; set; }

        [JsonPropertyName("widgetBorderWeight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? WidgetBorderWeight { get; set; }

        [JsonPropertyName("widgetCornerRadius")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? WidgetCornerRadius { get; set; }

    }

}