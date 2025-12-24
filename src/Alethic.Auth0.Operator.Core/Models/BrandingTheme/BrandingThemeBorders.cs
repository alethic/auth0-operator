using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    public class BrandingThemeBorders
    {

        [JsonPropertyName("button_border_radius")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ButtonBorderRadius { get; set; }

        [JsonPropertyName("button_border_weight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ButtonBorderWeight { get; set; }

        [JsonPropertyName("buttons_style")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ButtonsStyle? ButtonsStyle { get; set; }

        [JsonPropertyName("input_border_radius")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? InputBorderRadius { get; set; }

        [JsonPropertyName("input_border_weight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? InputBorderWeight { get; set; }

        [JsonPropertyName("inputs_style")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ButtonsStyle? InputsStyle { get; set; }

        [JsonPropertyName("show_widget_shadow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowWidgetShadow { get; set; }

        [JsonPropertyName("widget_border_weight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? WidgetBorderWeight { get; set; }

        [JsonPropertyName("widget_corner_radius")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? WidgetCornerRadius { get; set; }

    }

}