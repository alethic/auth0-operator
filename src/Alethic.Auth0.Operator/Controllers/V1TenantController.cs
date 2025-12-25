using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Tenant;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

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

    [EntityRbac(typeof(V1Tenant), Verbs = RbacVerb.All)]
    public class V1TenantController :
        ControllerBase<V1Tenant, V1Tenant.SpecDef, V1Tenant.StatusDef, V1TenantConf, Hashtable>,
        IEntityController<V1Tenant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1TenantController(IKubernetesClient kube, EntityRequeue<V1Tenant> requeue, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1TenantController> logger) :
            base(kube, requeue, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Tenant";

        /// <inheritdoc />
        protected override async Task Reconcile(V1Tenant entity, CancellationToken cancellationToken)
        {
            var api = await GetTenantApiClientAsync(entity, cancellationToken);
            if (api == null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}:{entity.Name()} failed to retrieve API client.");

            var settings = await api.TenantSettings.GetAsync(cancellationToken: cancellationToken);
            if (settings is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} cannot be loaded from API.");

            // apply lastconf to status before attempting update
            entity.Status.LastConf = TransformToSystemTextJson<Hashtable>(settings);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                // verify that no changes to enable_sso are being made
                if (conf.Flags != null && conf.Flags.EnableSSO != null && settings.Flags.EnableSSO != null && conf.Flags.EnableSSO != settings.Flags.EnableSSO)
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()}: updating the enable_sso flag is not allowed.");

                // push update to Auth0
                var req = TransformToNewtonsoftJson<V1TenantConf, TenantSettingsUpdateRequest>(conf);
                req.Flags ??= new();
                req.Flags.EnableSSO = null;
                settings = await api.TenantSettings.UpdateAsync(req, cancellationToken);
            }

            // retrieve and copy applied settings to status
            entity.Status.LastConf = TransformToSystemTextJson<Hashtable>(settings);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
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
