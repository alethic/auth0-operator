using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V1alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1alpha1BrandingThemeLinksStyle
    {

        [JsonStringEnumMemberName("normal")]
        Normal,

        [JsonStringEnumMemberName("underlined")]
        Underlined,

    }

}
