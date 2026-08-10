using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

/// <summary>
/// FedCM (Federated Credential Management) login configuration for the client.
/// </summary>
public record V2alpha3ClientFedCmLogin
{

    [JsonPropertyName("google")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientFedCmLoginGoogle? Google { get; set; }

}
