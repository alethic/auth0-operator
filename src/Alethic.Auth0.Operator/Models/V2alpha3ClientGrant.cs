using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "ClientGrant")]
    [KubernetesEntityShortNames("a0cgr")]
    public partial class V2alpha3ClientGrant :
        CustomKubernetesEntity<V2alpha3ClientGrant.SpecDef, V2alpha3ClientGrant.StatusDef>,
        V1TenantEntityInstance<V2alpha3ClientGrant.SpecDef, V2alpha3ClientGrant.StatusDef, V2alpha3ClientGrantConf, V2alpha3ClientGrantConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3ClientGrantConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3ClientGrantConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3ClientGrantConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3ClientGrantConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3ClientGrantConf? LastConf { get; set; }

        }

    }

}
