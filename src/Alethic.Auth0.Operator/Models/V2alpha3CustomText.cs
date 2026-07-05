using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.CustomText.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "CustomText")]
    [KubernetesEntityShortNames("a0customtext")]
    public partial class V2alpha3CustomText :
        CustomKubernetesEntity<V2alpha3CustomText.SpecDef, V2alpha3CustomText.StatusDef>,
        V1TenantEntity<V2alpha3CustomText.SpecDef, V2alpha3CustomText.StatusDef, V2alpha3CustomTextConf, V2alpha3CustomTextConf>
    {

        public class SpecDef : V1TenantEntitySpec<V2alpha3CustomTextConf>
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
            public V2alpha3CustomTextConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityStatus<V2alpha3CustomTextConf>
        {

            [JsonPropertyName("prompt")]
            public string? Prompt { get; set; }

            [JsonPropertyName("language")]
            public string? Language { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3CustomTextConf? LastConf { get; set; }

        }

    }

}
