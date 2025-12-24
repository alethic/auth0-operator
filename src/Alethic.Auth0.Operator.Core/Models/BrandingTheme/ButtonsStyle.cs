using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ButtonsStyle
    {

        [JsonStringEnumMemberName("pill")]
        Pill,

        [JsonStringEnumMemberName("rounded")]
        Rounded,

        [JsonStringEnumMemberName("sharp")]
        Sharp,

    }

}
