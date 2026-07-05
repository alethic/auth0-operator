using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionIdentityApiEnumAzureAd
{

    [JsonStringEnumMemberName("microsoft-identity-platform-v2.0")]
    MicrosoftIdentityPlatformV20,

    [JsonStringEnumMemberName("azure-active-directory-v1.0")]
    AzureActiveDirectoryV10

}
