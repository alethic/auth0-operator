using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>box</c> social connection strategy.
    /// Each boolean property enables the corresponding Box API permission scope.
    /// </summary>
    public record V1ConnectionBoxOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("read")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Read { get; set; }

        [JsonPropertyName("write")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Write { get; set; }

        [JsonPropertyName("manage_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ManageGroups { get; set; }

        [JsonPropertyName("manage_webhooks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ManageWebhooks { get; set; }

        [JsonPropertyName("manage_enterprise_properties")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ManageEnterpriseProperties { get; set; }

        [JsonPropertyName("manage_data_retention")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ManageDataRetention { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
