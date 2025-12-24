using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1alpha1", Kind = "BrandingTheme")]
    [KubernetesEntityShortNames("a0theme")]
    public partial class V1alpha1BrandingTheme :
        CustomKubernetesEntity<V1alpha1BrandingTheme.SpecDef, V1alpha1BrandingTheme.StatusDef>,
        V1TenantEntity<V1alpha1BrandingTheme.SpecDef, V1alpha1BrandingTheme.StatusDef, V1alpha1BrandingThemeConf, V1alpha1BrandingThemeConf>
    {

        public class SpecDef : V1TenantEntitySpec<V1alpha1BrandingThemeConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("find")]
            public V1alpha1BrandingThemeFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V1alpha1BrandingThemeConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V1alpha1BrandingThemeConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityStatus<V1alpha1BrandingThemeConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
            public V1alpha1BrandingThemeConf? LastConf { get; set; }

        }

    }

}
