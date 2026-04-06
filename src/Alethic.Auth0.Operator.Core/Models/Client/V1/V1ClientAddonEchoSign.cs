using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Adobe EchoSign SSO configuration.
/// </summary>
public class V1ClientAddonEchoSign 
{

    /// <summary>
    /// Your custom domain found in your EchoSign URL. e.g. `https://acme-org.echosign.com` would be `acme-org`.
    /// </summary>
    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Domain { get; set; }

}
