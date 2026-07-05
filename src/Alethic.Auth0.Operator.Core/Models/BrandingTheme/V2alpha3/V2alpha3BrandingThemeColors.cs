using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    public record V2alpha3BrandingThemeColors
    {

        [JsonPropertyName("baseFocusColor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BaseFocusColor { get; set; }

        [JsonPropertyName("baseHoverColor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BaseHoverColor { get; set; }

        [JsonPropertyName("bodyText")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BodyText { get; set; }

        [JsonPropertyName("captchaWidgetTheme")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeCaptchaWidgetTheme? CaptchaWidgetTheme { get; set; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Error { get; set; }

        [JsonPropertyName("header")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Header { get; set; }

        [JsonPropertyName("icons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Icons { get; set; }

        [JsonPropertyName("inputBackground")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputBackground { get; set; }

        [JsonPropertyName("inputBorder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputBorder { get; set; }

        [JsonPropertyName("inputFilledText")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputFilledText { get; set; }

        [JsonPropertyName("inputLabelsPlaceholders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputLabelsPlaceholders { get; set; }

        [JsonPropertyName("linksFocusedComponents")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LinksFocusedComponents { get; set; }

        [JsonPropertyName("primaryButton")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PrimaryButton { get; set; }

        [JsonPropertyName("primaryButtonLabel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PrimaryButtonLabel { get; set; }

        [JsonPropertyName("secondaryButtonBorder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SecondaryButtonBorder { get; set; }

        [JsonPropertyName("secondaryButtonLabel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SecondaryButtonLabel { get; set; }

        [JsonPropertyName("success")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Success { get; set; }

        [JsonPropertyName("widgetBackground")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WidgetBackground { get; set; }

        [JsonPropertyName("widgetBorder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WidgetBorder { get; set; }

    }

}