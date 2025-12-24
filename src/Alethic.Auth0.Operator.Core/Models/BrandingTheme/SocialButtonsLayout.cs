using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SocialButtonsLayout
    {

        [JsonStringEnumMemberName("bottom")]
        Bottom,

        [JsonStringEnumMemberName("top")]
        Top

    }

}
