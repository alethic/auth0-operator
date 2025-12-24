using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LinksStyle
    {

        [JsonStringEnumMemberName("normal")]
        Normal,

        [JsonStringEnumMemberName("underlined")]
        Underlined,

    }

}
