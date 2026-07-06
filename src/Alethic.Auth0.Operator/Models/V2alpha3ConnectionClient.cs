using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ConnectionClient.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "ConnectionClient")]
    [KubernetesEntityShortNames("a0cc")]
    public partial class V2alpha3ConnectionClient :
        CustomKubernetesEntity<V2alpha3ConnectionClient.SpecDef, V2alpha3ConnectionClient.StatusDef>,
        V1TenantEntityInstance<V2alpha3ConnectionClient.SpecDef, V2alpha3ConnectionClient.StatusDef, V2alpha3ConnectionClientConf, V2alpha3ConnectionClientConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3ConnectionClientConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3ConnectionClientConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3ConnectionClientConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3ConnectionClientConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3ConnectionClientConf? LastConf { get; set; }

        }

    }

}
