using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha3ResourceServerConsentPolicy
    {

        [JsonStringEnumMemberName("transactionalAuthorizationWithMfa")]
        TransactionalAuthorizationWithMfa,

    }

}
