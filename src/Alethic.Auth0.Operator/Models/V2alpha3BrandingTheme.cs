using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "BrandingTheme")]
    [KubernetesEntityShortNames("a0theme")]
    public partial class V2alpha3BrandingTheme :
        CustomKubernetesEntity<V2alpha3BrandingTheme.SpecDef, V2alpha3BrandingTheme.StatusDef>,
        V1TenantEntityInstance<V2alpha3BrandingTheme.SpecDef, V2alpha3BrandingTheme.StatusDef, V2alpha3BrandingThemeConf, V2alpha3BrandingThemeConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3BrandingThemeConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("find")]
            public V2alpha3BrandingThemeFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3BrandingThemeConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public V2alpha3BrandingThemeConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3BrandingThemeConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3BrandingThemeConf? LastConf { get; set; }

        }

    }

}
