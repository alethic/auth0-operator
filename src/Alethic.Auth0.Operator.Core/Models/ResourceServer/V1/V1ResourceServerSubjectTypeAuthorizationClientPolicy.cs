using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ResourceServerSubjectTypeAuthorizationClientPolicy
    {

        [JsonStringEnumMemberName("deny_all")]
        DenyAll,

        [JsonStringEnumMemberName("require_client_grant")]
        RequireClientGrant,

    }

}