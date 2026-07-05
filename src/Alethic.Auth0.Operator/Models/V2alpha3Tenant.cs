using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "Tenant")]
    [KubernetesEntityShortNames("a0tenant")]
    public partial class V2alpha3Tenant :
        CustomKubernetesEntity<V2alpha3Tenant.SpecDef, V2alpha3Tenant.StatusDef>,
        ApiEntity<V2alpha3Tenant.SpecDef, V2alpha3Tenant.StatusDef, V2alpha3TenantConf, V2alpha3TenantConf>
    {

        public class SpecDef : ApiEntitySpec<V2alpha3TenantConf>
        {

            public class AuthDef
            {

                [JsonPropertyName("domain")]
                [Required]
                public string? Domain { get; set; }

                [JsonPropertyName("secretRef")]
                [Required]
                public V1SecretReference? SecretRef { get; set; }

            }

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("name")]
            [Required]
            public string Name { get; set; } = "";

            [JsonPropertyName("auth")]
            [Required]
            public AuthDef? Auth { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3TenantConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3TenantConf? Conf { get; set; }

        }

        public class StatusDef : ApiEntityStatus<V2alpha3TenantConf>
        {

            [JsonPropertyName("lastConf")]
            public V2alpha3TenantConf? LastConf { get; set; }

        }

    }

}
