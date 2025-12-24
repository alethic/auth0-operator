using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ClientGrant;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "ClientGrant")]
    [KubernetesEntityShortNames("a0cgr")]
    public partial class V1ClientGrant :
        CustomKubernetesEntity<V1ClientGrant.SpecDef, V1ClientGrant.StatusDef>,
        V1TenantEntity<V1ClientGrant.SpecDef, V1ClientGrant.StatusDef, ClientGrantConf, Hashtable>
    {

        public class SpecDef : V1TenantEntitySpec<ClientGrantConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("init")]
            public ClientGrantConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public ClientGrantConf? Conf { get; set; }

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
