using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha1;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha1", Kind = "Role")]
    public partial class V2alpha1Role :
        CustomKubernetesEntity<V2alpha1Role.SpecDef, V2alpha1Role.StatusDef>,
        V1TenantEntityInstance<V2alpha1Role.SpecDef, V2alpha1Role.StatusDef, V2alpha1RoleConf, V2alpha1RoleConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha1RoleConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public V2alpha1RoleConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha1RoleConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha1RoleConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha1RoleConf? LastConf { get; set; }

        }

    }

}
