using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.Role.V2alpha1
{

    /// <summary>
    /// Desired configuration for an Auth0 role resource.
    /// </summary>
    public record V2alpha1RoleConf
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
        public V2alpha1RolePermission[]? Permissions { get; set; }

    }

}
