﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;

using k8s.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Queue;
using KubeOps.Abstractions.Rbac;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Connection), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ConnectionController :
        V1TenantEntityController<V1Connection, V1Connection.SpecDef, V1Connection.StatusDef, ConnectionConf>,
        IEntityController<V1Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public V1ConnectionController(IKubernetesClient kube, EntityRequeue<V1Connection> requeue, IMemoryCache cache, ILogger<V1ConnectionController> logger) :
            base(kube, requeue, cache, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Connection";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            var self = await api.Connections.GetAsync(id, cancellationToken: cancellationToken);
            if (self == null)
                return null;

            var dict = new Hashtable();
            dict["id"] = self.Id;
            dict["name"] = self.Name;
            dict["display_name"] = self.DisplayName;
            dict["strategy"] = self.Strategy;
            dict["realms"] = self.Realms;
            dict["is_domain_connection"] = self.IsDomainConnection;
            dict["show_as_button"] = self.ShowAsButton;
            dict["provisioning_ticket_url"] = self.ProvisioningTicketUrl;
            dict["enabled_clients"] = self.EnabledClients;
            dict["options"] = TransformToSystemTextJson<Hashtable?>(self.Options);
            dict["metadata"] = TransformToSystemTextJson<Hashtable?>(self.Metadata);
            return dict;
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var list = await api.Connections.GetAllAsync(new GetConnectionsRequest() { Fields = "id,name" }, pagination: (PaginationInfo?)null, cancellationToken: cancellationToken);
            var self = list.FirstOrDefault(i => i.Name == conf.Name);
            return self?.Id;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(ConnectionConf conf)
        {
            return null;
        }

        /// <summary>
        /// Attempts to resolve the list of client references to client IDs.
        /// </summary>
        /// <param name="refs"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        async Task<string[]?> ResolveClientRefsToIds(IManagementApiClient api, V1ClientReference[]? refs, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (refs is null)
                return Array.Empty<string>();

            var l = new List<string>(refs.Length);

            foreach (var i in refs)
            {
                var r = await ResolveClientRefToId(api, i, defaultNamespace, cancellationToken);
                if (r is null)
                    throw new InvalidOperationException();

                l.Add(r);
            }

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new ConnectionCreateRequest();
            await ApplyConfToRequest(api, req, conf, defaultNamespace, cancellationToken);
            req.Strategy = conf.Strategy;
            req.Options = conf.Strategy == "auth0" ? TransformToNewtonsoftJson<ConnectionOptions, global::Auth0.ManagementApi.Models.Connections.ConnectionOptions>(JsonSerializer.Deserialize<ConnectionOptions>(JsonSerializer.Serialize(conf.Options))) : conf.Options;

            var self = await api.Connections.CreateAsync(req, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new ConnectionUpdateRequest();
            await ApplyConfToRequest(api, req, conf, defaultNamespace, cancellationToken);
            req.Name = null;
            req.Options = conf.Strategy == "auth0" ? TransformToNewtonsoftJson<ConnectionOptions, global::Auth0.ManagementApi.Models.Connections.ConnectionOptions>(JsonSerializer.Deserialize<ConnectionOptions>(JsonSerializer.Serialize(conf.Options))) : conf.Options;
            await api.Connections.UpdateAsync(id, req, cancellationToken);
        }

        /// <summary>
        /// Applies the specified configuration to the request object.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="req"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task ApplyConfToRequest(IManagementApiClient api, ConnectionBase req, ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            req.Name = conf.Name;
            req.DisplayName = conf.DisplayName;
            req.Metadata = conf.Metadata;
            req.Realms = conf.Realms;
            req.IsDomainConnection = conf.IsDomainConnection ?? false;
            req.ShowAsButton = conf.ShowAsButton;
            req.EnabledClients = await ResolveClientRefsToIds(api, conf.EnabledClients, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.Connections.DeleteAsync(id, cancellationToken);
        }

    }

}
