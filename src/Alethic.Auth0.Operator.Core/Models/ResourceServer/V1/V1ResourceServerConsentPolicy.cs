using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ResourceServerConsentPolicy
    {

        [JsonStringEnumMemberName("transactional-authorization-with-mfa")]
        TransactionalAuthorizationWithMfa,

    }

}
