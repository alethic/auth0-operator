using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha1", Kind = "Client")]
    public partial class V2alpha1Client :
        CustomKubernetesEntity<V2alpha1Client.SpecDef, V2alpha1Client.StatusDef>,
        V1TenantEntityInstance<V2alpha1Client.SpecDef, V2alpha1Client.StatusDef, V2alpha1ClientConf, V2alpha1ClientConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha1ClientConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("secretRef")]
            public V1SecretReference? SecretRef { get; set; }

            [JsonPropertyName("find")]
            public V2alpha1ClientFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V2alpha1ClientConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha1ClientConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha1ClientConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha1ClientConf? LastConf { get; set; }

        }

    }

}
