using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

/// <summary>
/// A single Content-Security-Policy policy applied to the tenant's pages.
/// </summary>
public record V2alpha3TenantCspPolicy
{

    /// <summary>
    /// CSP directive names mapped to their source lists.
    /// </summary>
    [JsonPropertyName("directives")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string[]>? Directives { get; set; }

    [JsonPropertyName("flags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspFlagEnum[]? Flags { get; set; }

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspPolicyModeEnum? Mode { get; set; }

    [JsonPropertyName("reporting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCspPolicyReporting? Reporting { get; set; }

}
