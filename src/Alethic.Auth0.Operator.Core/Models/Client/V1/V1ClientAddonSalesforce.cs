using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Salesforce SSO configuration.
/// </summary>
public record V1ClientAddonSalesforce
{

    /// <summary>
    /// Arbitrary logical URL that identifies the Saleforce resource. e.g. `https://acme-org.com`.
    /// </summary>
    [JsonPropertyName("entity_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntityId { get; set; }

}
