using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V1;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "ClientGrant")]
    [KubernetesEntityShortNames("a0cgr")]
    public partial class V1ClientGrant :
        CustomKubernetesEntity<V1ClientGrant.SpecDef, V1ClientGrant.StatusDef>,
        V1TenantEntityInstance<V1ClientGrant.SpecDef, V1ClientGrant.StatusDef, V1ClientGrantConf, V1ClientGrantConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V1ClientGrantConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public V1ClientGrantConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V1ClientGrantConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V1ClientGrantConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V1ClientGrantConf? LastConf { get; set; }

        }

    }

}
