using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientOrganizationRequireBehaviorEnum
{

    [JsonStringEnumMemberName("no_prompt")]
    NoPrompt,

    [JsonStringEnumMemberName("pre_login_prompt")]
    PreLoginPrompt,

    [JsonStringEnumMemberName("post_login_prompt")]
    PostLoginPrompt

}
