using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;

namespace Alethic.Auth0.Operator.Core.Models.Connection
{

    public class V1ConnectionConf
    {

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("display_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        [JsonPropertyName("strategy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Strategy { get; set; }

        [JsonPropertyName("options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
        public Hashtable? Options { get; set; }

        [JsonPropertyName("provisioning_ticket_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProvisioningTicketUrl { get; set; }

        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
        public Hashtable? Metadata { get; set; }

        [JsonPropertyName("realms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Realms { get; set; }

        [JsonPropertyName("enabled_clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? EnabledClients { get; set; }

        [JsonPropertyName("show_as_button")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowAsButton { get; set; }

        [JsonPropertyName("is_domain_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsDomainConnection { get; set; } = false;

    }

}
