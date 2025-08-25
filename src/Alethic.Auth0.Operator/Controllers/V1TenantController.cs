using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Tenant;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi.Models;

using k8s.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Queue;
using KubeOps.Abstractions.Rbac;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Tenant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1TenantController :
        V1Controller<V1Tenant, V1Tenant.SpecDef, V1Tenant.StatusDef, TenantConf>,
        IEntityController<V1Tenant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public V1TenantController(IKubernetesClient kube, EntityRequeue<V1Tenant> requeue, IMemoryCache cache, ILogger<V1TenantController> logger) :
            base(kube, requeue, cache, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "A0Tenant";

        /// <inheritdoc />
        protected override async Task Reconcile(V1Tenant entity, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} starting reconciliation", EntityTypeName, entity.Namespace(), entity.Name());
            
            var api = await GetTenantApiClientAsync(entity, cancellationToken);
            if (api == null)
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} failed to retrieve API client", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}:{entity.Name()} failed to retrieve API client.");
            }

            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} fetching tenant settings from Auth0 API", EntityTypeName, entity.Namespace(), entity.Name());
            var settings = await api.TenantSettings.GetAsync(cancellationToken: cancellationToken);
            if (settings is null)
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} tenant settings not found in Auth0 API", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} cannot be loaded from API.");
            }
            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} successfully retrieved tenant settings from Auth0", EntityTypeName, entity.Namespace(), entity.Name());

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} configuration change detected - validating and updating Auth0", EntityTypeName, entity.Namespace(), entity.Name());
                
                // verify that no changes to enable_sso are being made
                if (conf.Flags != null && conf.Flags.EnableSSO != null && settings.Flags.EnableSSO != null && conf.Flags.EnableSSO != settings.Flags.EnableSSO)
                {
                    Logger.LogError("{EntityTypeName} {Namespace}/{Name} attempted to modify enable_sso flag from {CurrentValue} to {NewValue} - operation not allowed", 
                        EntityTypeName, entity.Namespace(), entity.Name(), settings.Flags.EnableSSO, conf.Flags.EnableSSO);
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()}: updating the enable_sso flag is not allowed.");
                }

                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} updating tenant settings in Auth0", EntityTypeName, entity.Namespace(), entity.Name());
                // push update to Auth0
                var req = TransformToNewtonsoftJson<TenantConf, TenantSettingsUpdateRequest>(conf);
                req.Flags.EnableSSO = null;
                settings = await api.TenantSettings.UpdateAsync(req, cancellationToken);
                Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} successfully updated tenant settings in Auth0", EntityTypeName, entity.Namespace(), entity.Name());
            }

            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} retrieving final tenant settings from Auth0 for status update", EntityTypeName, entity.Namespace(), entity.Name());
            // retrieve and copy applied settings to status
            settings = await api.TenantSettings.GetAsync(cancellationToken: cancellationToken);
            entity.Status.LastConf = TransformToSystemTextJson<Hashtable>(settings);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);

            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} reconciliation completed successfully", EntityTypeName, entity.Namespace(), entity.Name());
            await ReconcileSuccessAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        public override Task DeletedAsync(V1Tenant entity, CancellationToken cancellationToken)
        {
            Logger.LogWarning("Unsupported operation deleting entity {Entity}.", entity);
            return Task.CompletedTask;
        }

    }

}
