using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Prompt
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromptScreenRendererFilterMatchType
    {

        [JsonStringEnumMemberName("includes_any")]
        IncludesAny,

        [JsonStringEnumMemberName("excludes_any")]
        ExcludesAny,

    }

}