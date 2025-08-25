using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
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
using Microsoft.Extensions.Options;

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
        /// <param name="options"></param>
        public V1ConnectionController(IKubernetesClient kube, EntityRequeue<V1Connection> requeue, IMemoryCache cache, ILogger<V1ConnectionController> logger, IOptions<OperatorOptions> options) :
            base(kube, requeue, cache, logger, options)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "A0Connection";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("{EntityTypeName} fetching connection from Auth0 with ID {Id}", EntityTypeName, id);
                var self = await api.Connections.GetAsync(id, cancellationToken: cancellationToken);
                if (self == null)
                {
                    Logger.LogWarning("{EntityTypeName} connection with ID {Id} not found in Auth0", EntityTypeName, id);
                    return null;
                }

                Logger.LogInformation("{EntityTypeName} successfully retrieved connection from Auth0 with ID {Id} and name {Name}", EntityTypeName, id, self.Name);
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
            catch (ErrorApiException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Logger.LogWarning("{EntityTypeName} connection with ID {Id} not found in Auth0 (404)", EntityTypeName, id);
                return null;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error retrieving {EntityTypeName} with ID {Id}: {Message}", EntityTypeName, id, e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Connection entity, V1Connection.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} using find criteria for connection lookup", EntityTypeName, entity.Namespace(), entity.Name());
                
                if (spec.Find.ConnectionId is string connectionId)
                {
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} searching Auth0 for connection with ID {ConnectionId}", EntityTypeName, entity.Namespace(), entity.Name(), connectionId);
                    try
                    {
                        var connection = await api.Connections.GetAsync(connectionId, cancellationToken: cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} found existing connection with ID {ConnectionId} and name {Name}", EntityTypeName, entity.Namespace(), entity.Name(), connectionId, connection.Name);
                        return connection.Id;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Logger.LogWarning("{EntityTypeName} {Namespace}/{Name} could not find connection with ID {ConnectionId}", EntityTypeName, entity.Namespace(), entity.Name(), connectionId);
                        return null;
                    }
                }

                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} no valid connection ID provided in find criteria", EntityTypeName, entity.Namespace(), entity.Name());
                return null;
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is null || string.IsNullOrEmpty(conf.Name))
                {
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} no configuration or connection name available for find operation", EntityTypeName, entity.Namespace(), entity.Name());
                    return null;
                }

                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} searching Auth0 for connection with name {ConnectionName}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);
                var list = await api.Connections.GetAllAsync(new GetConnectionsRequest(), (PaginationInfo?)null, cancellationToken);
                var self = list.FirstOrDefault(i => i.Name == conf.Name);
                if (self is not null)
                {
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} found existing connection with name {ConnectionName} and ID {Id}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Name, self.Id);
                }
                else
                {
                    Logger.LogWarning("{EntityTypeName} {Namespace}/{Name} no existing connection found with name {ConnectionName}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);
                }
                return self?.Id;
            }
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
            Logger.LogInformation("{EntityTypeName} creating connection in Auth0 with name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, conf.Name, conf.Strategy);
            var req = new ConnectionCreateRequest();
            await ApplyConfToRequest(api, req, conf, defaultNamespace, cancellationToken);
            req.Strategy = conf.Strategy;
            req.Options = conf.Strategy == "auth0" ? TransformToNewtonsoftJson<ConnectionOptions, global::Auth0.ManagementApi.Models.Connections.ConnectionOptions>(JsonSerializer.Deserialize<ConnectionOptions>(JsonSerializer.Serialize(conf.Options))) : conf.Options;

            var self = await api.Connections.CreateAsync(req, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            Logger.LogInformation("{EntityTypeName} successfully created connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, self.Id, conf.Name, conf.Strategy);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
            var req = new ConnectionUpdateRequest();
            await ApplyConfToRequest(api, req, conf, defaultNamespace, cancellationToken);
            req.Name = null;
            req.Options = conf.Strategy == "auth0" ? TransformToNewtonsoftJson<ConnectionOptions, global::Auth0.ManagementApi.Models.Connections.ConnectionOptions>(JsonSerializer.Deserialize<ConnectionOptions>(JsonSerializer.Serialize(conf.Options))) : conf.Options;
            await api.Connections.UpdateAsync(id, req, cancellationToken);
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
        protected override async Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting connection from Auth0 with ID: {ConnectionId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Connections.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted connection from Auth0 with ID: {ConnectionId}", EntityTypeName, id);
        }

    }

}
