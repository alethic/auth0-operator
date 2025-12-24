using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models.Tenant;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "Tenant")]
    [KubernetesEntityShortNames("a0tenant")]
    public partial class V1Tenant :
        CustomKubernetesEntity<V1Tenant.SpecDef, V1Tenant.StatusDef>,
        V1Entity<V1Tenant.SpecDef, V1Tenant.StatusDef, V1TenantConf, Hashtable>
    {

        public class SpecDef : V1EntitySpec<V1TenantConf>
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
            public V1TenantConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V1TenantConf? Conf { get; set; }

        }

        public class StatusDef : V1EntityStatus<Hashtable>
        {

            [JsonPropertyName("lastConf")]
            [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
            public Hashtable? LastConf { get; set; }

        }

    }

}
