using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1alpha1BrandingThemeLogoPosition
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
