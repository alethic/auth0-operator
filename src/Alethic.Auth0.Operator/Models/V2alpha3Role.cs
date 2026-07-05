using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "Role")]
    [KubernetesEntityShortNames("a0role")]
    public partial class V2alpha3Role :
        CustomKubernetesEntity<V2alpha3Role.SpecDef, V2alpha3Role.StatusDef>,
        V1TenantEntityInstance<V2alpha3Role.SpecDef, V2alpha3Role.StatusDef, V2alpha3RoleConf, V2alpha3RoleConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3RoleConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3RoleConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3RoleConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3RoleConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3RoleConf? LastConf { get; set; }

        }

    }

}
