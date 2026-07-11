using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Salesforce API addon configuration.
/// </summary>
public record V1ClientAddonSalesforceApi
{

    /// <summary>
    /// Consumer Key assigned by Salesforce to the Connected App.
    /// </summary>
    [JsonPropertyName("clientid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Clientid { get; set; }

    /// <summary>
    /// Name of the property in the user object that maps to a Salesforce username. e.g. `email`.
    /// </summary>
    [JsonPropertyName("principal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Principal { get; set; }

    /// <summary>
    /// Community name.
    /// </summary>
    [JsonPropertyName("communityName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CommunityName { get; set; }

    /// <summary>
    /// Community url section.
    /// </summary>
    [JsonPropertyName("community_url_section")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CommunityUrlSection { get; set; }

}
