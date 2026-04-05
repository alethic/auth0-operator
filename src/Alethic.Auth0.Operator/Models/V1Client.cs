using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V1;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "Client")]
    [KubernetesEntityShortNames("a0app")]
    public partial class V1Client :
        CustomKubernetesEntity<V1Client.SpecDef, V1Client.StatusDef>,
        V1TenantEntityInstance<V1Client.SpecDef, V1Client.StatusDef, V1ClientConf, V1ClientConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V1ClientConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("secretRef")]
            public V1SecretReference? SecretRef { get; set; }

            [JsonPropertyName("find")]
            public V1ClientFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V1ClientConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V1ClientConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V1ClientConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V1ClientConf? LastConf { get; set; }

        }

    }

}
