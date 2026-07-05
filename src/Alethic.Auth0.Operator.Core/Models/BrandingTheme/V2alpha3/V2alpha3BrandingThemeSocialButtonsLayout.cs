using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3BrandingThemeSocialButtonsLayout
    {

        [JsonStringEnumMemberName("bottom")]
        Bottom,

        [JsonStringEnumMemberName("top")]
        Top

    }

}
