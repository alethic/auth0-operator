using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HeaderTextAlignment
    {

        [JsonStringEnumMemberName("center")]
        Center,

        [JsonStringEnumMemberName("left")]
        Left,

        [JsonStringEnumMemberName("right")]
        Right,

    }

}
