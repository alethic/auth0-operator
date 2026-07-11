using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientOrganizationRequireBehavior
    {

        [JsonStringEnumMemberName("no_prompt")]
        NoPrompt,

        [JsonStringEnumMemberName("pre_login_prompt")]
        PreLoginPrompt,

        [JsonStringEnumMemberName("post_login_prompt")]
        PostLoginPrompt

    }

}
