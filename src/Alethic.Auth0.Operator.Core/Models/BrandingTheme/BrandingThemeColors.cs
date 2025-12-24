using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    public class BrandingThemeColors
    {

        [JsonPropertyName("base_focus_color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BaseFocusColor { get; set; }

        [JsonPropertyName("base_hover_color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BaseHoverColor { get; set; }

        [JsonPropertyName("body_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BodyText { get; set; }

        [JsonPropertyName("captcha_widget_theme")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CaptchaWidgetTheme? CaptchaWidgetTheme { get; set; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Error { get; set; }

        [JsonPropertyName("header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Header { get; set; }

        [JsonPropertyName("icons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Icons { get; set; }

        [JsonPropertyName("input_background")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputBackground { get; set; }

        [JsonPropertyName("input_border")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputBorder { get; set; }

        [JsonPropertyName("input_filled_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputFilledText { get; set; }

        [JsonPropertyName("input_labels_placeholders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputLabelsPlaceholders { get; set; }

        [JsonPropertyName("links_focused_components")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LinksFocusedComponents { get; set; }

        [JsonPropertyName("primary_button")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PrimaryButton { get; set; }

        [JsonPropertyName("primary_button_label")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PrimaryButtonLabel { get; set; }

        [JsonPropertyName("secondary_button_border")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SecondaryButtonBorder { get; set; }

        [JsonPropertyName("secondary_button_label")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SecondaryButtonLabel { get; set; }

        [JsonPropertyName("success")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Success { get; set; }

        [JsonPropertyName("widget_background")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WidgetBackground { get; set; }

        [JsonPropertyName("widget_border")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WidgetBorder { get; set; }

    }

}