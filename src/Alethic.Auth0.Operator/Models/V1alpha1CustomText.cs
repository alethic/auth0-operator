using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.CustomText;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1alpha1", Kind = "CustomText")]
    [KubernetesEntityShortNames("a0customtext")]
    public partial class V1alpha1CustomText :
        CustomKubernetesEntity<V1alpha1CustomText.SpecDef, V1alpha1CustomText.StatusDef>,
        V1TenantEntity<V1alpha1CustomText.SpecDef, V1alpha1CustomText.StatusDef, V1alpha1CustomTextConf, V1alpha1CustomTextConf>
    {

        public class SpecDef : V1TenantEntitySpec<V1alpha1CustomTextConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("prompt")]
            [Required]
            public string? Prompt { get; set; }

            [JsonPropertyName("language")]
            [Required]
            public string? Language { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V1alpha1CustomTextConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityStatus<V1alpha1CustomTextConf>
        {

            [JsonPropertyName("prompt")]
            public string? Prompt { get; set; }

            [JsonPropertyName("language")]
            public string? Language { get; set; }

            [JsonPropertyName("lastConf")]
            public V1alpha1CustomTextConf? LastConf { get; set; }

        }

    }

}
