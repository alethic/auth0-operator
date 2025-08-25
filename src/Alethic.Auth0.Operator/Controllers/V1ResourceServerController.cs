using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ResourceServer;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

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
    [EntityRbac(typeof(V1ResourceServer), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ResourceServerController :
        V1TenantEntityController<V1ResourceServer, V1ResourceServer.SpecDef, V1ResourceServer.StatusDef, ResourceServerConf>,
        IEntityController<V1ResourceServer>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public V1ResourceServerController(IKubernetesClient kube, EntityRequeue<V1ResourceServer> requeue, IMemoryCache cache, ILogger<V1ResourceServerController> logger, IOptions<OperatorOptions> options) :
            base(kube, requeue, cache, logger, options)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "A0ResourceServer";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("{EntityTypeName} fetching resource server from Auth0 with ID {Id}", EntityTypeName, id);
                var result = await api.ResourceServers.GetAsync(id, cancellationToken: cancellationToken);
                Logger.LogInformation("{EntityTypeName} successfully retrieved resource server from Auth0 with ID {Id}", EntityTypeName, id);
                return TransformToSystemTextJson<Hashtable>(result);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error retrieving {EntityTypeName} with ID {Id}: {Message}", EntityTypeName, id, e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1ResourceServer entity, V1ResourceServer.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
            {
                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} no configuration available for find operation", EntityTypeName, entity.Namespace(), entity.Name());
                return null;
            }

            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} searching Auth0 for resource server with identifier {Identifier}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Identifier);
            var list = await api.ResourceServers.GetAllAsync(new ResourceServerGetRequest() { }, cancellationToken: cancellationToken);
            var self = list.FirstOrDefault(i => i.Identifier == conf.Identifier);
            
            if (self != null)
            {
                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} found existing resource server with identifier {Identifier} and ID {Id}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Identifier, self.Id);
            }
            else
            {
                Logger.LogWarning("{EntityTypeName} {Namespace}/{Name} no existing resource server found with identifier {Identifier}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Identifier);
            }
            
            return self?.Id;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(ResourceServerConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating resource server in Auth0 with identifier {Identifier} and name {Name}", EntityTypeName, conf.Identifier, conf.Name);
            var self = await api.ResourceServers.CreateAsync(TransformToNewtonsoftJson<ResourceServerConf, ResourceServerCreateRequest>(conf), cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created resource server in Auth0 with ID {Id} and identifier {Identifier}", EntityTypeName, self.Id, self.Identifier);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating resource server in Auth0 with ID {Id} and identifier {Identifier}", EntityTypeName, id, conf.Identifier);
            await api.ResourceServers.UpdateAsync(id, TransformToNewtonsoftJson<ResourceServerConf, ResourceServerUpdateRequest>(conf), cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated resource server in Auth0 with ID {Id}", EntityTypeName, id);
        }

        /// <inheritdoc />
        protected override Task ApplyStatus(IManagementApiClient api, V1ResourceServer entity, Hashtable lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var identifier = (string?)lastConf["identifier"];
            if (string.IsNullOrWhiteSpace(identifier))
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} has missing Identifier.");

            entity.Status.Identifier = identifier;
            return base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting resource server from Auth0 with ID {Id}", EntityTypeName, id);
            await api.ResourceServers.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted resource server from Auth0 with ID {Id}", EntityTypeName, id);
        }

    }

}
