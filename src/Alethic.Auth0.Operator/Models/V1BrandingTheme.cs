using System.Collections;
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
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "BrandingTheme")]
    [KubernetesEntityShortNames("a0theme")]
    public partial class V1BrandingTheme :
        CustomKubernetesEntity<V1BrandingTheme.SpecDef, V1BrandingTheme.StatusDef>,
        V1TenantEntity<V1BrandingTheme.SpecDef, V1BrandingTheme.StatusDef, BrandingThemeConf>
    {

        public class SpecDef : V1TenantEntitySpec<BrandingThemeConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("find")]
            public BrandingThemeFind? Find { get; set; }

            [JsonPropertyName("init")]
            public BrandingThemeConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public BrandingThemeConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityStatus
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
            public Hashtable? LastConf { get; set; }

        }

    }

}
