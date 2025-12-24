using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "Connection")]
    [KubernetesEntityShortNames("a0con")]
    public partial class V1Connection :
        CustomKubernetesEntity<V1Connection.SpecDef, V1Connection.StatusDef>,
        V1TenantEntity<V1Connection.SpecDef, V1Connection.StatusDef, ConnectionConf, Hashtable>
    {

        public class SpecDef : V1TenantEntitySpec<ConnectionConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("find")]
            public ConnectionFind? Find { get; set; }

            [JsonPropertyName("init")]
            public ConnectionConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            public ConnectionConf? Conf { get; set; }

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
