using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomText.V2alpha3
{

    public record V2alpha3CustomTextConf
    {

        [JsonPropertyName("screens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V2alpha3CustomTextScreen>? Screens { get; set; }

    }

}
