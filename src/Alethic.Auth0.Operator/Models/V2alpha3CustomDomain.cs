using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "CustomDomain")]
    [KubernetesEntityShortNames("a0domain")]
    public partial class V2alpha3CustomDomain :
        CustomKubernetesEntity<V2alpha3CustomDomain.SpecDef, V2alpha3CustomDomain.StatusDef>,
        V1TenantEntityInstance<V2alpha3CustomDomain.SpecDef, V2alpha3CustomDomain.StatusDef, V2alpha3CustomDomainConf, V2alpha3CustomDomainConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3CustomDomainConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("secretRef")]
            public V1SecretReference? SecretRef { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3CustomDomainConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3CustomDomainConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3CustomDomainConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3CustomDomainConf? LastConf { get; set; }

        }

    }

}
