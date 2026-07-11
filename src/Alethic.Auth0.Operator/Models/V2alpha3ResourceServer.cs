using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "ResourceServer")]
    [KubernetesEntityShortNames("a0api")]
    public partial class V2alpha3ResourceServer :
        CustomKubernetesEntity<V2alpha3ResourceServer.SpecDef, V2alpha3ResourceServer.StatusDef>,
        V1TenantEntityInstance<V2alpha3ResourceServer.SpecDef, V2alpha3ResourceServer.StatusDef, V2alpha3ResourceServerConf, V2alpha3ResourceServerConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3ResourceServerConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3ResourceServerConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3ResourceServerConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3ResourceServerConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("identifier")]
            public string? Identifier { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3ResourceServerConf? LastConf { get; set; }

        }

    }

}
