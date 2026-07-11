using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientAddonSalesforceApi
{

    [JsonPropertyName("clientid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Clientid { get; set; }

    [JsonPropertyName("principal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Principal { get; set; }

    [JsonPropertyName("communityName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CommunityName { get; set; }

    [JsonPropertyName("communityUrlSection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CommunityUrlSection { get; set; }

}
