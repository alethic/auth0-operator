using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models.Tenant;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha1", Kind = "Tenant")]
    [KubernetesEntityShortNames("a0tenant")]
    public partial class V2alpha1Tenant :
        CustomKubernetesEntity<V2alpha1Tenant.SpecDef, V2alpha1Tenant.StatusDef>,
        ApiEntity<V2alpha1Tenant.SpecDef, V2alpha1Tenant.StatusDef, V2alpha1TenantConf, V2alpha1TenantConf>
    {

        public class SpecDef : ApiEntitySpec<V2alpha1TenantConf>
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
            public V2alpha1TenantConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha1TenantConf? Conf { get; set; }

        }

        public class StatusDef : ApiEntityStatus<V2alpha1TenantConf>
        {

            [JsonPropertyName("lastConf")]
            public V2alpha1TenantConf? LastConf { get; set; }

        }

    }

}
