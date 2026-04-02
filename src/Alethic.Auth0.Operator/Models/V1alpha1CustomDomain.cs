using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1alpha1", Kind = "CustomDomain")]
    [KubernetesEntityShortNames("a0app")]
    public partial class V1alpha1CustomDomain :
        CustomKubernetesEntity<V1alpha1CustomDomain.SpecDef, V1alpha1CustomDomain.StatusDef>,
        V1TenantEntity<V1alpha1CustomDomain.SpecDef, V1alpha1CustomDomain.StatusDef, V1alpha1CustomDomainConf, Hashtable>
    {

        public class SpecDef : V1TenantEntitySpec<V1alpha1CustomDomainConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("secretRef")]
            public V1SecretReference? SecretRef { get; set; }

            [JsonPropertyName("find")]
            public V1alpha1CustomDomainFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V1alpha1CustomDomainConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V1alpha1CustomDomainConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityStatus<Hashtable>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
            public Hashtable? LastConf { get; set; }

        }

    }

}
