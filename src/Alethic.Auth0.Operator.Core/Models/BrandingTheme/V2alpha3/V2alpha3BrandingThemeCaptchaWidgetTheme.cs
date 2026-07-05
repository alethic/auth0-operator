using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3BrandingThemeCaptchaWidgetTheme
    {

        [JsonStringEnumMemberName("auto")]
        Auto,

        [JsonStringEnumMemberName("dark")]
        Dark,

        [JsonStringEnumMemberName("light")]
        Light

    }

}
