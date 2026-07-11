using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3BrandingThemeLogoPosition
    {

        [JsonStringEnumMemberName("center")]
        Center,

        [JsonStringEnumMemberName("left")]
        Left,

        [JsonStringEnumMemberName("right")]
        Right,

        [JsonStringEnumMemberName("none")]
        None
    }

}
