using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientOrganizationRequireBehaviorEnum
{

    [JsonStringEnumMemberName("noPrompt")]
    NoPrompt,

    [JsonStringEnumMemberName("preLoginPrompt")]
    PreLoginPrompt,

    [JsonStringEnumMemberName("postLoginPrompt")]
    PostLoginPrompt

}
