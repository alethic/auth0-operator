using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.ManagementApi;

using k8s;
using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Rbac;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public abstract class V1TenantEntityController<TEntity, TSpec, TStatus, TConf, TLastConf> : ControllerBase<TEntity, TSpec, TStatus, TConf, TLastConf>
        where TEntity : IKubernetesObject<V1ObjectMeta>, V1TenantEntity<TSpec, TStatus, TConf, TLastConf>
        where TSpec : V1TenantEntitySpec<TConf>
        where TStatus : V1TenantEntityStatus<TLastConf>
        where TConf : class
        where TLastConf : class
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public V1TenantEntityController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <summary>
        /// Performs reconciliation of the specified entity with the current state in the management API for the given tenant.
        /// </summary>
        /// <param name="api">The management API client used to query and update the entity state. Cannot be null.</param>
        /// <param name="tenant">The tenant context in which the reconciliation is performed. Cannot be null.</param>
        /// <param name="entity">The entity to reconcile. Represents the desired state to be applied.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous reconciliation operation. The task result contains the reconciled
        /// entity reflecting the current state after reconciliation.</returns>
        protected abstract Task<TEntity> ReconcileAsync(IManagementApiClient api, V2alpha1Tenant tenant, TEntity entity, CancellationToken cancellationToken);

        /// <inheritdoc />
        protected sealed override async Task Reconcile(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.Spec.TenantRef is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing a tenant reference.");

            // resolve the tenant object
            var tenant = await ResolveV2alpha1TenantRef(entity.Spec.TenantRef, entity.Namespace(), cancellationToken);
            if (tenant is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing a tenant.");

            // tenant is in the same namespace, ensure we have an owner reference to it for automatic cleanup
            if (tenant.Namespace() == entity.Namespace())
            {
                entity = await Kube.UpdateAsync(entity.WithOwnerReference(tenant), cancellationToken);
                if (entity.Spec.TenantRef is null)
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing a tenant reference.");
            }

            // get API client for tenant
            var api = await GetTenantApiClientAsync(entity, entity.Spec.TenantRef, cancellationToken);
            if (api is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} failed to retrieve API client.");

            entity = await ReconcileAsync(api, tenant, entity, cancellationToken);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Handles logic to be executed after an entity has been deleted from the specified tenant.
        /// </summary>
        /// <param name="api">The management API client used to perform operations related to the deletion.</param>
        /// <param name="tenant">The tenant from which the entity was deleted. Cannot be null.</param>
        /// <param name="entity">The entity instance that was deleted. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected abstract Task DeletedAsync(IManagementApiClient api, V2alpha1Tenant tenant, TEntity entity, CancellationToken cancellationToken);

        /// <inheritdoc />
        protected sealed override async Task DeletedAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.Spec.TenantRef is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing a tenant reference.");

            // resolve the tenant object
            var tenant = await ResolveV2alpha1TenantRef(entity.Spec.TenantRef, entity.Namespace(), cancellationToken);
            if (tenant is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing a tenant.");

            // tenant is in the same namespace, ensure we have an owner reference to it for automatic cleanup
            if (tenant.Namespace() == entity.Namespace())
            {
                entity = await Kube.UpdateAsync(entity.WithOwnerReference(tenant), cancellationToken);
                if (entity.Spec.TenantRef is null)
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing a tenant reference.");
            }

            var api = await GetTenantApiClientAsync(entity, entity.Spec.TenantRef, cancellationToken);
            if (api is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} failed to retrieve API client.");

            await DeletedAsync(api, tenant, entity, cancellationToken);
        }

    }

}
