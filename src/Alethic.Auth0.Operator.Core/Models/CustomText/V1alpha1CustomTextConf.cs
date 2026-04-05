using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.CustomText
{

    public partial class V1alpha1CustomTextConf
    {

        [JsonPropertyName("screens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, CustomTextScreen>? Screens { get; set; }

    }

}
