using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientDefaultOrganizationFlowsEnum
{

    [JsonStringEnumMemberName("client_credentials")]
    ClientCredentials

}
