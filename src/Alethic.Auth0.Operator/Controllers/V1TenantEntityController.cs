﻿using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;

using k8s;
using k8s.Models;

using KubeOps.Abstractions.Queue;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    public abstract class V1TenantEntityController<TEntity, TSpec, TStatus, TConf> : V1Controller<TEntity, TSpec, TStatus, TConf>
        where TEntity : IKubernetesObject<V1ObjectMeta>, V1TenantEntity<TSpec, TStatus, TConf>
        where TSpec : V1TenantEntitySpec<TConf>
        where TStatus : V1TenantEntityStatus
        where TConf : class
    {

        readonly IOptions<OperatorOptions> _options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public V1TenantEntityController(IKubernetesClient kube, EntityRequeue<TEntity> requeue, IMemoryCache cache, ILogger logger, IOptions<OperatorOptions> options) :
            base(kube, requeue, cache, logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Attempts to perform a get operation through the API.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to locate a matching API element by the given configuration.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="entity"></param>
        /// <param name="spec"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task<string?> Find(IManagementApiClient api, TEntity entity, TSpec spec, string defaultNamespace, CancellationToken cancellationToken);

        /// <summary>
        /// Performs a validation on the <paramref name="conf"/> parameter for usage in create operations.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        protected abstract string? ValidateCreate(TConf conf);

        /// <summary>
        /// Attempts to perform a creation through the API. If successful returns the new ID value.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task<string> Create(IManagementApiClient api, TConf conf, string defaultNamespace, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to perform an update through the API.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="last"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task Update(IManagementApiClient api, string id, Hashtable? last, TConf conf, string defaultNamespace, CancellationToken cancellationToken);

        /// <inheritdoc />
        protected override async Task Reconcile(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.Spec.TenantRef is null)
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} missing a tenant reference.", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing a tenant reference.");
            }

            if (entity.Spec.Conf is null)
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} missing configuration.", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing configuration.");
            }

            var tenant = await ResolveTenantRef(entity.Spec.TenantRef, entity.Namespace(), cancellationToken);
            if (tenant is null)
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} missing a tenant.", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing a tenant.");
            }

            var api = await GetTenantApiClientAsync(tenant, cancellationToken);
            if (api is null)
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} failed to retrieve API client.", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} failed to retrieve API client.");
            }

            // ensure we hold a reference to the tenant
            var md = entity.EnsureMetadata();
            var an = md.EnsureAnnotations();
            an["kubernetes.auth0.com/tenant-uid"] = tenant.Uid();

            // we have not resolved a remote entity
            if (string.IsNullOrWhiteSpace(entity.Status.Id))
            {
                Logger.LogWarning("{EntityTypeName} {Namespace}/{Name} has not yet been reconciled, checking if entity exists in Auth0.", EntityTypeName, entity.Namespace(), entity.Name());

                // find existing remote entity
                var entityId = await Find(api, entity, entity.Spec, entity.Namespace(), cancellationToken);
                if (entityId is null)
                {
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} could not be located, creating.", EntityTypeName, entity.Namespace(), entity.Name());

                    // reject creation if disallowed
                    if (entity.HasPolicy(V1EntityPolicyType.Create) == false)
                    {
                        Logger.LogWarning("{EntityTypeName} {Namespace}/{Name} does not support creation.", EntityTypeName, entity.Namespace(), entity.Name());
                        return;
                    }

                    // validate configuration version used for initialization
                    var init = entity.Spec.Init ?? entity.Spec.Conf;
                    if (ValidateCreate(init) is string msg)
                    {
                        Logger.LogError("{EntityTypeName} {Namespace}/{Name} is invalid: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), msg);
                        throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is invalid: {msg}");
                    }

                    // create new entity and associate
                    entity.Status.Id = await Create(api, init, entity.Namespace(), cancellationToken);
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} created with {Id}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Status.Id);
                    entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
                }
                else
                {
                    entity.Status.Id = entityId;
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} found with {Id}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Status.Id);
                    entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
                }
            }

            // at this point we must have a reference to an entity
            if (string.IsNullOrWhiteSpace(entity.Status.Id))
            {
                Logger.LogError("{EntityTypeName} {Namespace}/{Name} reconciliation failed - ID is still not set after attempting to find or create entity.", EntityTypeName, entity.Namespace(), entity.Name());
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing an existing ID.");
            }

            // attempt to retrieve existing entity
            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} checking if entity exists in Auth0 with ID {Id}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Status.Id);
            var lastConf = await Get(api, entity.Status.Id, entity.Namespace(), cancellationToken);
            if (lastConf is null)
            {
                // no matching remote entity that correlates directly with ID, reset and retry to go back to Find/Create
                Logger.LogWarning("{EntityTypeName} {Namespace}/{Name} not found in Auth0, clearing status and scheduling recreation", EntityTypeName, entity.Namespace(), entity.Name());
                entity.Status.LastConf = null;
                entity.Status.Id = null;
                entity = await Kube.UpdateStatusAsync(entity, cancellationToken);
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} has missing API object, invalidating.");
            }

            // apply updates if allowed
            if (entity.HasPolicy(V1EntityPolicyType.Update))
            {
                if (entity.Spec.Conf is { } conf)
                    await Update(api, entity.Status.Id, lastConf, conf, entity.Namespace(), cancellationToken);
            }
            else
            {
                Logger.LogDebug("{EntityTypeName} {Namespace}/{Name} does not support update.", EntityTypeName, entity.Namespace(), entity.Name());
            }

            // apply new configuration
            await ApplyStatus(api, entity, lastConf, entity.Namespace(), cancellationToken);
            entity = await Kube.UpdateStatusAsync(entity, cancellationToken);

            // schedule periodic reconciliation to detect external changes (e.g., manual deletion from Auth0)
            var interval = _options.Value.Reconciliation.Interval;
            Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} scheduling next reconciliation in {IntervalSeconds}s", EntityTypeName, entity.Namespace(), entity.Name(), interval.TotalSeconds);
            Requeue(entity, interval);
        }

        /// <summary>
        /// Applies any modification to the entity status just before saving it.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="entity"></param>
        /// <param name="lastConf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task ApplyStatus(IManagementApiClient api, TEntity entity, Hashtable lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            entity.Status.LastConf = lastConf;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Implement this method to delete a specific entity from the API.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken);

        /// <inheritdoc />
        public override sealed async Task DeletedAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity.Spec.TenantRef is null)
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing a tenant reference.");

                var tenant = await ResolveTenantRef(entity.Spec.TenantRef, entity.Namespace(), cancellationToken);
                if (tenant is null)
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} is missing a tenant.");

                var api = await GetTenantApiClientAsync(tenant, cancellationToken);
                if (api is null)
                    throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} failed to retrieve API client.");

                if (string.IsNullOrWhiteSpace(entity.Status.Id))
                {
                    Logger.LogWarning("{EntityTypeName} {EntityNamespace}/{EntityName} has no known ID, skipping delete (reason: entity was never successfully created in Auth0).", EntityTypeName, entity.Namespace(), entity.Name());
                    return;
                }

                var self = await Get(api, entity.Status.Id, entity.Namespace(), cancellationToken);
                if (self is null)
                {
                    Logger.LogWarning("{EntityTypeName} {EntityNamespace}/{EntityName} with ID {Id} not found in Auth0, skipping delete (reason: already deleted externally).", EntityTypeName, entity.Namespace(), entity.Name(), entity.Status.Id);
                    return;
                }

                // reject deletion if disallowed by policy
                if (entity.HasPolicy(V1EntityPolicyType.Delete) == false)
                {
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} does not support delete (reason: Delete policy not enabled).", EntityTypeName, entity.Namespace(), entity.Name());
                }
                else
                {
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} initiating deletion from Auth0 with ID: {Id} (reason: Kubernetes entity was deleted)", EntityTypeName, entity.Namespace(), entity.Name(), entity.Status.Id);
                    await Delete(api, entity.Status.Id, cancellationToken);
                    Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} deletion completed successfully", EntityTypeName, entity.Namespace(), entity.Name());
                }
            }
            catch (ErrorApiException e)
            {
                try
                {
                    Logger.LogError(e, "API error deleting {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.ApiError?.Message);
                    await DeletingWarningAsync(entity, "ApiError", e.ApiError?.Message ?? "", cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }
            }
            catch (RateLimitApiException e)
            {
                try
                {
                    Logger.LogError(e, "Rate limit hit deleting {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.ApiError?.Message ?? e.Message);
                    await DeletingWarningAsync(entity, "RateLimit", e.ApiError?.Message ?? "", cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                // calculate next attempt time, floored to one minute
                var n = e.RateLimit?.Reset is DateTimeOffset r ? r - DateTimeOffset.Now : TimeSpan.FromMinutes(1);
                if (n < TimeSpan.FromMinutes(1))
                    n = TimeSpan.FromMinutes(1);

                Logger.LogInformation("Rescheduling delete after {TimeSpan}.", n);
                Requeue(entity, n);
            }
            catch (RetryException e)
            {
                try
                {
                    Logger.LogError(e, "Retry hit deleting {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.Message);
                    await DeletingWarningAsync(entity, "Retry", e.Message, cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                Logger.LogInformation("Rescheduling delete after {TimeSpan}.", TimeSpan.FromMinutes(1));
                Requeue(entity, TimeSpan.FromMinutes(1));
            }
            catch (Exception e)
            {
                try
                {
                    Logger.LogError(e, "Unexpected exception deleting {EntityTypeName} {EntityNamespace}/{EntityName}: {Message}", EntityTypeName, entity.Namespace(), entity.Name(), e.Message);
                    await DeletingWarningAsync(entity, "Unknown", e.Message, cancellationToken);
                }
                catch (Exception e2)
                {
                    Logger.LogCritical(e2, "Unexpected exception creating event.");
                }

                throw;
            }
        }

    }

}
