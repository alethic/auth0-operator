using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Desired configuration for an Auth0 connection resource.
    /// </summary>
    public record V2alpha1ConnectionConf
    {

        /// <summary>
        /// The name of the connection. Must be unique for the tenant. Max length 35 characters and must start and end with an alphanumeric character and can only contain alphanumeric characters and '-'.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// The display name of the connection, shown to end users in the Universal Login experience.
        /// </summary>
        [JsonPropertyName("display_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// The identity provider identifier. Determines which strategy-specific options object is used.
        /// </summary>
        [JsonPropertyName("strategy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Strategy { get; set; }

        /// <summary>
        /// Provisioning ticket URL used for enterprise connections during setup.
        /// </summary>
        [JsonPropertyName("provisioning_ticket_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProvisioningTicketUrl { get; set; }

        /// <summary>
        /// Metadata associated with the connection in the form of an object with string values (max 255 chars). A maximum of 10 metadata properties are allowed.
        /// </summary>
        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
        public Hashtable? Metadata { get; set; }

        /// <summary>
        /// Defines the realms for which the connection will be used (e.g. email domains). If not specified, the connection name will be added as realm.
        /// </summary>
        [JsonPropertyName("realms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Realms { get; set; }

        /// <summary>
        /// The list of clients for which the connection is enabled.
        /// </summary>
        [JsonPropertyName("enabled_clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? EnabledClients { get; set; }

        /// <summary>
        /// Whether the connection is shown as a button. Only for enterprise connections.
        /// </summary>
        [JsonPropertyName("show_as_button")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowAsButton { get; set; }

        /// <summary>
        /// True if the connection is a domain level connection, false otherwise.
        /// </summary>
        [JsonPropertyName("is_domain_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsDomainConnection { get; set; } = false;

        /// <summary>
        /// A typed union of options.
        /// </summary>
        [JsonPropertyName("options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptions? Options { get; set; }

    }

}
