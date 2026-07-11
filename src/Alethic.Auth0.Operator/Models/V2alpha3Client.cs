using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "Client")]
    [KubernetesEntityShortNames("a0app")]
    public partial class V2alpha3Client :
        CustomKubernetesEntity<V2alpha3Client.SpecDef, V2alpha3Client.StatusDef>,
        V1TenantEntityInstance<V2alpha3Client.SpecDef, V2alpha3Client.StatusDef, V2alpha3ClientConf, V2alpha3ClientConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3ClientConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("secretRef")]
            public V1SecretReference? SecretRef { get; set; }

            [JsonPropertyName("find")]
            public V2alpha3ClientFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3ClientConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3ClientConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3ClientConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3ClientConf? LastConf { get; set; }

        }

    }

}
