using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// Restricts the country codes available for phone-based flows on the tenant.
/// </summary>
public record V2alpha3TenantCountryCodes
{

    [JsonPropertyName("list")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? List { get; set; }

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCountryCodesModeEnum? Mode { get; set; }

}
