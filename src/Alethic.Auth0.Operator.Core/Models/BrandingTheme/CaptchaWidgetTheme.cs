using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CaptchaWidgetTheme
    {

        [JsonStringEnumMemberName("auto")]
        Auto,

        [JsonStringEnumMemberName("dark")]
        Dark,

        [JsonStringEnumMemberName("light")]
        Light

    }

}
