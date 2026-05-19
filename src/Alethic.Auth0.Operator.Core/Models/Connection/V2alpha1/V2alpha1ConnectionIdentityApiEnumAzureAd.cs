using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionIdentityApiEnumAzureAd
{
    [JsonStringEnumMemberName("microsoft_identity_platform_v20")]
    MicrosoftIdentityPlatformV20,
    [JsonStringEnumMemberName("azure_active_directory_v10")]
    AzureActiveDirectoryV10
}
