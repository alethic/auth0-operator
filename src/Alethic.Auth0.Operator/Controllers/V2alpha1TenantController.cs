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

    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha1TenantController :
        ControllerBase<V2alpha1Tenant, V2alpha1Tenant.SpecDef, V2alpha1Tenant.StatusDef, V2alpha1TenantConf>,
        IEntityController<V2alpha1Tenant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha1TenantController(IKubernetesClient kube, EntityRequeue<V2alpha1Tenant> requeue, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V2alpha1TenantController> logger) :
            base(kube, requeue, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Tenant";

        /// <inheritdoc />
        protected override async Task Reconcile(V2alpha1Tenant entity, CancellationToken cancellationToken)
        {
            var api = await GetTenantApiClientAsync(entity, cancellationToken);
            if (api == null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}:{entity.Name()} failed to retrieve API client.");

            var settings = await api.TenantSettings.GetAsync(cancellationToken: cancellationToken);
            if (settings is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} settings cannot be loaded from API.");

            var branding = await api.Branding.GetAsync(cancellationToken: cancellationToken);
            if (branding is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} branding cannot be loaded from API.");

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                // settings may not be specified
                if (conf.Settings is { } newSettings)
                {
                    // verify that no changes to enable_sso are being made
                    if (newSettings != null && newSettings.Flags != null && newSettings.Flags.EnableSSO != null && settings.Flags.EnableSSO != null && newSettings.Flags.EnableSSO != settings.Flags.EnableSSO)
                        throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()}: updating the enable_sso flag is not allowed.");

                    // push update to Auth0
                    var req = TransformToNewtonsoftJson<TenantConfSettings, TenantSettingsUpdateRequest>(newSettings!);
                    req.Flags.EnableSSO = null; // this can never be passed
                    settings = await api.TenantSettings.UpdateAsync(req, cancellationToken);
                }

                // branding may not be specified
                if (conf.Branding is { } newBranding)
                {
                    // push update to Auth0
                    var req = TransformToNewtonsoftJson<TenantConfBranding, BrandingUpdateRequest>(newBranding!);
                    branding = await api.Branding.UpdateAsync(req, cancellationToken);
                }
            }

            // retrieve and copy new properties to status
            entity.Status.LastConf ??= new Hashtable();
            entity.Status.LastConf["settings"] = TransformToSystemTextJson<Hashtable>(settings);
            entity.Status.LastConf["branding"] = TransformToSystemTextJson<Hashtable>(branding);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);

            await ReconcileSuccessAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        public override Task DeletedAsync(V2alpha1Tenant entity, CancellationToken cancellationToken)
        {
            Logger.LogWarning("Unsupported operation deleting entity {Entity}.", entity);
            return Task.CompletedTask;
        }

    }

}
