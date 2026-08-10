using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.Role.V2alpha3
{

    /// <summary>
    /// Desired configuration for an Auth0 role resource.
    /// </summary>
    public record V2alpha3RoleConf
    {

        /// <summary>
        /// The name of the role. Must be unique for the tenant.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// A description of the role.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        /// <summary>
        /// The permissions (scopes) associated with the role. When set, the operator reconciles the associated
        /// permissions to exactly match this list. When null, the permissions are left unmanaged.
        /// </summary>
        [JsonPropertyName("permissions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3RolePermission[]? Permissions { get; set; }

        /// <summary>
        /// The type of the role. Tenant roles apply tenant-wide while organization roles are scoped to an
        /// organization. Auth0 only accepts this value at creation time; it cannot be changed afterwards.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3RoleTypeEnum? Type { get; set; }

        /// <summary>
        /// The identifier of the entity that owns the role. Auth0 only accepts this value at creation time; it
        /// cannot be changed afterwards.
        /// </summary>
        [JsonPropertyName("ownerId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OwnerId { get; set; }

    }

}
