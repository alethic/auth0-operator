using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromptScreenRenderingMode
    {

        [JsonStringEnumMemberName("standard")]
        Standard,

        [JsonStringEnumMemberName("advanced")]
        Advanced,

    }

}