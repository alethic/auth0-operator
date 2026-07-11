using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using k8s;
using k8s.Autorest;
using k8s.Models;

using KubeOps.KubernetesClient;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alethic.Auth0.Operator
{

    /// <summary>
    /// Rewrites every operator-owned custom resource at startup so it is persisted in etcd at the current storage
    /// (hub) version. Objects written under an earlier hub (e.g. a Tenant stored as <c>v2alpha1</c> after the hub
    /// moved to <c>v2alpha3</c>) otherwise remain stored at that old version indefinitely, and any read at a third
    /// served version (e.g. <c>v1</c>) then requires a two-hop conversion (stored → hub → requested) that the KubeOps
    /// conversion webhook does not perform — it routes single-hop only, silently returning zero converted objects.
    /// The API server surfaces that as <c>conversion webhook ... returned 0 objects, expected 1</c>, breaking every
    /// consumer that reads at the affected version (kubectl's preferred version, controller informers, Helm/Flux).
    /// Rewriting the objects at the hub version makes every subsequent conversion single-hop, which is the same
    /// remedy the Kubernetes storage version migrator applies after a storage version change.
    /// </summary>
    /// <remarks>
    /// The rewrite is a no-op update: listing at the hub version and putting the object back unchanged re-encodes it
    /// at the storage version. Objects already stored at the hub are byte-identical on write, so the API server skips
    /// the etcd write entirely — repeated startups cost nothing. Rewritten objects do emit a Modified watch event, but
    /// the update never changes <c>metadata.generation</c>, so the watcher's generation-based dedup does not schedule
    /// any additional reconciliation. Migration waits for the application to have started because listing old-stored
    /// objects at the hub version calls back into this operator's own conversion webhook.
    /// </remarks>
    public class StorageVersionMigrationService : BackgroundService
    {

        /// <summary>
        /// Maximum attempts to list a kind before giving up. Listing requires the operator's own conversion webhook to
        /// be reachable from the API server, which lags application start by the readiness probe and endpoint
        /// propagation, so early attempts are expected to fail.
        /// </summary>
        const int MaxListAttempts = 30;

        static readonly TimeSpan ListRetryDelay = TimeSpan.FromSeconds(10);

        readonly IKubernetesClient _kube;
        readonly IHostApplicationLifetime _lifetime;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        public StorageVersionMigrationService(IKubernetesClient kube, IHostApplicationLifetime lifetime, ILogger<StorageVersionMigrationService> logger)
        {
            _kube = kube ?? throw new ArgumentNullException(nameof(kube));
            _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // hosted services begin before the web host serves requests; wait for full application start so the
            // conversion webhook (served by this process) can satisfy the hub-version list calls below
            var started = new TaskCompletionSource();
            await using var registration = _lifetime.ApplicationStarted.Register(() => started.TrySetResult());
            if (_lifetime.ApplicationStarted.IsCancellationRequested)
                started.TrySetResult();

            await started.Task.WaitAsync(stoppingToken);

            _logger.LogInformation("Beginning storage version migration of Auth0 operator resources.");

            await MigrateAsync<V2alpha3Tenant>(stoppingToken);
            await MigrateAsync<V2alpha3Client>(stoppingToken);
            await MigrateAsync<V2alpha3Connection>(stoppingToken);
            await MigrateAsync<V2alpha3ResourceServer>(stoppingToken);
            await MigrateAsync<V2alpha3ClientGrant>(stoppingToken);
            await MigrateAsync<V2alpha3Role>(stoppingToken);
            await MigrateAsync<V2alpha3BrandingTheme>(stoppingToken);
            await MigrateAsync<V2alpha3CustomDomain>(stoppingToken);
            await MigrateAsync<V2alpha3CustomText>(stoppingToken);
            await MigrateAsync<V2alpha3ConnectionClient>(stoppingToken);

            _logger.LogInformation("Completed storage version migration of Auth0 operator resources.");
        }

        /// <summary>
        /// Rewrites all instances of the specified kind at the hub version. Failures are logged and skipped: a missed
        /// object is simply migrated on the next operator start, and must never prevent the operator from running.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task MigrateAsync<TEntity>(CancellationToken cancellationToken)
            where TEntity : IKubernetesObject<V1ObjectMeta>
        {
            var list = await ListWithRetryAsync<TEntity>(cancellationToken);
            if (list is null)
                return;

            var count = 0;

            foreach (var entity in list)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await _kube.UpdateAsync(entity, cancellationToken);
                    count++;
                }
                catch (HttpOperationException e) when (e.Response.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.NotFound)
                {
                    // a concurrent write has itself re-persisted the object at the storage version, or it is gone
                    _logger.LogDebug("Skipping storage version rewrite of {Kind} {Namespace}/{Name}: {StatusCode}.", typeof(TEntity).Name, entity.Namespace(), entity.Name(), e.Response.StatusCode);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Failed storage version rewrite of {Kind} {Namespace}/{Name}; it will be retried on next startup.", typeof(TEntity).Name, entity.Namespace(), entity.Name());
                }
            }

            _logger.LogInformation("Storage version migration rewrote {Count} instance(s) of {Kind}.", count, typeof(TEntity).Name);
        }

        /// <summary>
        /// Lists all instances of the specified kind at the hub version, retrying while the operator's conversion
        /// webhook becomes reachable. Returns <c>null</c> when the list could not be obtained.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<System.Collections.Generic.IList<TEntity>?> ListWithRetryAsync<TEntity>(CancellationToken cancellationToken)
            where TEntity : IKubernetesObject<V1ObjectMeta>
        {
            for (var attempt = 1; attempt <= MaxListAttempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    return await _kube.ListAsync<TEntity>(cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception e)
                {
                    if (attempt == MaxListAttempts)
                    {
                        _logger.LogWarning(e, "Failed to list {Kind} for storage version migration after {Attempts} attempts; skipping (it will be retried on next startup).", typeof(TEntity).Name, attempt);
                        return null;
                    }

                    _logger.LogDebug(e, "Failed to list {Kind} for storage version migration (attempt {Attempt}/{Attempts}); retrying.", typeof(TEntity).Name, attempt, MaxListAttempts);
                    await Task.Delay(ListRetryDelay, cancellationToken);
                }
            }

            return null;
        }

    }

}
