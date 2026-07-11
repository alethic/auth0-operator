using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Role.V2alpha1
{

    /// <summary>
    /// Associates a single permission (scope) defined on a resource server with a role.
    /// </summary>
    public record V2alpha1RolePermission
    {

        /// <summary>
        /// Reference to the resource server (API) that defines the permission.
        /// </summary>
        [JsonPropertyName("resourceServerRef")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ResourceServerReference? ResourceServerRef { get; set; }

        /// <summary>
        /// The name of the permission (scope value) as defined on the referenced resource server.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

    }

}
