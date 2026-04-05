using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Connection), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ConnectionController :
        V1TenantEntityInstanceController<V1Connection, V1Connection.SpecDef, V1Connection.StatusDef, V1ConnectionConf, Hashtable>,
        IEntityController<V1Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ConnectionController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ConnectionController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Connection";

        /// <summary>
        /// Gets the list of enabled client IDs for the specified connection. This populates the legacy field using the newer API.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="connectionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<string[]> GetEnabledClientsAsync(IManagementApiClient api, string connectionId, CancellationToken cancellationToken)
        {
            var clients = await api.Connections.GetEnabledClientsAsync(new global::Auth0.ManagementApi.Models.Connections.EnabledClientsGetRequest() { ConnectionId = connectionId }, cancellationToken: cancellationToken);

            var l = new List<string>(clients.Count);
            foreach (var client in clients)
                if (client.ClientId is not null)
                    l.Add(client.ClientId);

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
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
                dict["enabled_clients"] = await GetEnabledClientsAsync(api, self.Id, cancellationToken);
                dict["options"] = TransformToSystemTextJson<Hashtable?>(self.Options);
                dict["metadata"] = TransformToSystemTextJson<Hashtable?>(self.Metadata);
                return dict;
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Connection entity, V1Connection.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.ConnectionId is string connectionId)
                {
                    try
                    {
                        var connection = await api.Connections.GetAsync(connectionId, cancellationToken: cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing connection: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), connection.Name);
                        return connection.Id;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find connection with id {ConnectionId}.", EntityTypeName, entity.Namespace(), entity.Name(), connectionId);
                        return null;
                    }
                }

                return null;
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is null || string.IsNullOrEmpty(conf.Name))
                    return null;

                var list = await api.Connections.GetAllAsync(new GetConnectionsRequest(), (PaginationInfo?)null!, cancellationToken: cancellationToken);
                var self = list.FirstOrDefault(i => i.Name == conf.Name);
                if (self is not null)
                    Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing connection by name: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);

                return self?.Id;
            }
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1ConnectionConf conf)
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
        async Task<string[]> ResolveClientRefsToIds(IManagementApiClient api, V1ClientReference[]? refs, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (refs is null)
                return Array.Empty<string>();

            var l = new List<string>(refs.Length);

            foreach (var i in refs)
                l.Add(await ResolveClientRefToId(api, i, defaultNamespace, cancellationToken) ?? throw new InvalidOperationException());

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating connection in Auth0 with name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, conf.Name, conf.Strategy);
            var req = new ConnectionCreateRequest();
            await ApplyConfToRequest(api, req, conf, defaultNamespace, cancellationToken);

            if (conf.Strategy is null)
                throw new InvalidOperationException("Missing connection strategy.");

            // calculate options, depends on strategy
            var options = conf.Strategy == "auth0" ? (dynamic?)TransformToNewtonsoftJson<V1ConnectionOptions, global::Auth0.ManagementApi.Models.Connections.ConnectionOptions>(JsonSerializer.Deserialize<V1ConnectionOptions>(JsonSerializer.Serialize(conf.Options))) : conf.Options;
            if (options is null)
                throw new InvalidOperationException("Missing connection options.");

            // configure strategy and options
            req.Strategy = conf.Strategy;
            req.Options = options;

            var self = await api.Connections.CreateAsync(req, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            Logger.LogInformation("{EntityTypeName} successfully created connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, self.Id, conf.Name, conf.Strategy);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
            var req = new ConnectionUpdateRequest();
            await ApplyConfToRequest(api, req, conf, defaultNamespace, cancellationToken);

            // name has to be cleared for an update
            req.Name = null!;

            // calculate options: depends on current strategy, possibly null, which means no apply
            var strategy = last?["strategy"] as string ?? conf.Strategy;
            var options = strategy == "auth0" && conf.Options is not null ? (dynamic?)TransformToNewtonsoftJson<V1ConnectionOptions, global::Auth0.ManagementApi.Models.Connections.ConnectionOptions>(JsonSerializer.Deserialize<V1ConnectionOptions>(JsonSerializer.Serialize(conf.Options))) : conf.Options;
            if (options is not null)
                req.Options = options;

            await api.Connections.UpdateAsync(id, req, cancellationToken);
            await UpdateEnabledClientsAsync(api, id, conf, defaultNamespace, cancellationToken);

            Logger.LogInformation("{EntityTypeName} successfully updated connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
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
        async Task ApplyConfToRequest(IManagementApiClient api, ConnectionBase req, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (conf.Name is null)
                throw new InvalidOperationException("Missing name.");

            req.Name = conf.Name;
            if (conf.DisplayName is not null)
                req.DisplayName = conf.DisplayName;
            req.Metadata = conf.Metadata ?? null!;
            req.Realms = conf.Realms ?? [];
            req.IsDomainConnection = conf.IsDomainConnection ?? false;
            req.ShowAsButton = conf.ShowAsButton;
        }

        /// <summary>
        /// Applies the update of enabled clients. This is a separate call in the API, so we need to handle it separately.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task UpdateEnabledClientsAsync(IManagementApiClient api, string id, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (conf.EnabledClients is not null)
            {
                var req = new List<global::Auth0.ManagementApi.Models.Connections.EnabledClientsToUpdate>();

                // apply existing clients, disabled by default
                foreach (var current in await api.Connections.GetEnabledClientsAsync(new() { ConnectionId = id }, cancellationToken: cancellationToken))
                    if (current.ClientId is not null)
                        req.Add(new() { ClientId = current.ClientId, Status = false });

                // add or enable clients specified in the configuration
                foreach (var clientId in await ResolveClientRefsToIds(api, conf.EnabledClients, defaultNamespace, cancellationToken))
                {
                    var existing = req.FirstOrDefault(i => i.ClientId == clientId);
                    if (existing is null)
                        req.Add(existing = new() { ClientId = clientId });

                    existing.Status = true;
                }

                // apply update
                if (req.Count > 0)
                    await api.Connections.UpdateEnabledClientsAsync(id, new() { EnabledClients = req }, cancellationToken: cancellationToken);
            }
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting connection from Auth0 with ID: {ConnectionId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Connections.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted connection from Auth0 with ID: {ConnectionId}", EntityTypeName, id);
        }

    }

}
