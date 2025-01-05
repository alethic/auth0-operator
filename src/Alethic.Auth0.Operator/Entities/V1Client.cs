﻿using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Models.Client;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Entities
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v1", Kind = "Client")]
    [KubernetesEntityShortNames("a0app")]
    public partial class V1Client :
        CustomKubernetesEntity<V1Client.SpecDef, V1Client.StatusDef>,
        V1TenantEntity<V1Client.SpecDef, V1Client.StatusDef, ClientConf>
    {

        public class SpecDef : V1TenantEntitySpec<ClientConf>
        {

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantRef? TenantRef { get; set; }

            [JsonPropertyName("conf")]
            public ClientConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityStatus<ClientConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public ClientConf? LastConf { get; set; }

        }

    }

}
