using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>salesforce</c> (and <c>salesforce-sandbox</c>) social connection strategy.
    /// </summary>
    public record V1ConnectionSalesforceOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("community_base_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CommunityBaseUrl { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
