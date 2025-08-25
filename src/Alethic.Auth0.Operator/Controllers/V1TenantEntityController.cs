using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Defines how field filtering should be applied during drift detection.
    /// </summary>
    public enum DriftDetectionMode
    {
        /// <summary>
        /// Compare entire configuration without any field filtering.
        /// </summary>
        CompareAll,
        
        /// <summary>
        /// Only compare specific fields defined by the entity controller.
        /// </summary>
        IncludeSpecificFields,
        
        /// <summary>
        /// Compare all fields except those explicitly excluded by the entity controller.
        /// </summary>
        ExcludeSpecificFields
    }

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

        /// <summary>
        /// Gets the drift detection mode for this entity type.
        /// </summary>
        /// <returns>The drift detection mode to use</returns>
        protected virtual DriftDetectionMode GetDriftDetectionMode() => DriftDetectionMode.ExcludeSpecificFields;

        /// <summary>
        /// Gets the list of fields to include in drift detection when using IncludeSpecificFields mode.
        /// </summary>
        /// <returns>Array of field names to include in comparison</returns>
        protected virtual string[] GetIncludedFields() => Array.Empty<string>();

        /// <summary>
        /// Gets the list of fields to exclude from drift detection when using ExcludeSpecificFields mode.
        /// This is combined with the default volatile fields list.
        /// </summary>
        /// <returns>Array of field names to exclude from comparison</returns>
        protected virtual string[] GetExcludedFields() => Array.Empty<string>();

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
                {
                    // Check if configuration has changed by comparing with last known state
                    var hasChanges = HasConfigurationChanged(entity.Status.LastConf, conf);
                    if (hasChanges)
                    {
                        Logger.LogInformation("{EntityTypeName} {Namespace}/{Name} configuration changes detected - applying updates to Auth0", EntityTypeName, entity.Namespace(), entity.Name());
                        await Update(api, entity.Status.Id, lastConf, conf, entity.Namespace(), cancellationToken);
                    }
                    else
                    {
                        Logger.LogDebug("{EntityTypeName} {Namespace}/{Name} no configuration changes detected - skipping update", EntityTypeName, entity.Namespace(), entity.Name());
                    }
                }
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

        /// <summary>
        /// Determines if the configuration has changed by comparing the last known state with the desired configuration.
        /// </summary>
        /// <param name="lastConf">The last known configuration from Auth0</param>
        /// <param name="desiredConf">The desired configuration from the Kubernetes spec</param>
        /// <returns>True if changes are detected, false if configurations match</returns>
        private bool HasConfigurationChanged(Hashtable? lastConf, TConf desiredConf)
        {
            try
            {
                if (lastConf is null)
                {
                    Logger.LogDebug("{EntityTypeName} no previous configuration available - assuming changes exist", EntityTypeName);
                    return true;
                }

                // Convert desired configuration to Auth0 API format for comparison
                var desiredJson = TransformToNewtonsoftJson<TConf, object>(desiredConf);
                var desiredHashtable = TransformToSystemTextJson<Hashtable>(desiredJson);
                
                // Filter fields based on the entity's drift detection configuration
                var filteredLast = FilterFieldsForComparison(lastConf);
                var filteredDesired = FilterFieldsForComparison(desiredHashtable);

                // Compare the filtered configurations
                var result = !AreHashtablesEqual(filteredLast, filteredDesired);
                
                if (result)
                {
                    LogConfigurationDifferences(filteredLast, filteredDesired);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "{EntityTypeName} error comparing configurations, assuming changes exist: {Message}", EntityTypeName, ex.Message);
                return true; // Safe fallback: assume changes exist
            }
        }

        /// <summary>
        /// Filters fields for comparison based on the entity's drift detection configuration.
        /// </summary>
        /// <param name="config">The configuration hashtable to filter</param>
        /// <returns>A new hashtable with fields filtered according to the entity's drift detection mode</returns>
        private Hashtable FilterFieldsForComparison(Hashtable config)
        {
            var mode = GetDriftDetectionMode();
            var filtered = new Hashtable();

            switch (mode)
            {
                case DriftDetectionMode.CompareAll:
                    // Return all fields without filtering
                    foreach (DictionaryEntry entry in config)
                    {
                        filtered[entry.Key] = entry.Value;
                    }
                    break;

                case DriftDetectionMode.IncludeSpecificFields:
                    // Only include specified fields
                    var includedFields = new HashSet<string>(GetIncludedFields(), StringComparer.OrdinalIgnoreCase);
                    foreach (DictionaryEntry entry in config)
                    {
                        if (entry.Key is string key && includedFields.Contains(key))
                        {
                            filtered[key] = entry.Value;
                        }
                    }
                    break;

                case DriftDetectionMode.ExcludeSpecificFields:
                default:
                    // Exclude specified fields plus default volatile fields
                    var excludedFields = GetDefaultVolatileFields()
                        .Concat(GetExcludedFields())
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);
                    
                    foreach (DictionaryEntry entry in config)
                    {
                        if (entry.Key is string key && !excludedFields.Contains(key))
                        {
                            filtered[key] = entry.Value;
                        }
                    }
                    break;
            }

            return filtered;
        }

        /// <summary>
        /// Gets the default list of volatile fields that Auth0 manages internally.
        /// </summary>
        /// <returns>Array of default volatile field names</returns>
        private static string[] GetDefaultVolatileFields()
        {
            return new[]
            {
                // Auth0 system fields
                "client_id", "tenant", "created_at", "updated_at", "revision",
                // Client-specific volatile fields
                "client_secret", "global", "is_first_party", "cross_origin_auth",
                // Tenant-specific volatile fields
                "id", "domain", "management_api_identifier",
                // Connection-specific volatile fields
                "provisioning_ticket_url", "realms",
                // Resource server volatile fields
                "identifier", "is_system",
                // Client grant volatile fields
                "id", "client_id", "audience"
            };
        }

        /// <summary>
        /// Performs deep comparison of two hashtables to detect configuration differences.
        /// </summary>
        /// <param name="left">First hashtable to compare</param>
        /// <param name="right">Second hashtable to compare</param>
        /// <returns>True if hashtables are equal, false otherwise</returns>
        private static bool AreHashtablesEqual(Hashtable left, Hashtable right)
        {
            if (left.Count != right.Count)
                return false;

            foreach (DictionaryEntry entry in left)
            {
                if (!right.ContainsKey(entry.Key))
                    return false;

                if (!AreValuesEqual(entry.Value, right[entry.Key]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two values for equality, handling nested hashtables and arrays.
        /// </summary>
        /// <param name="left">First value to compare</param>
        /// <param name="right">Second value to compare</param>
        /// <returns>True if values are equal, false otherwise</returns>
        private static bool AreValuesEqual(object? left, object? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            // Handle nested hashtables
            if (left is Hashtable leftHash && right is Hashtable rightHash)
                return AreHashtablesEqual(leftHash, rightHash);

            // Handle arrays
            if (left is IEnumerable leftEnum && right is IEnumerable rightEnum && 
                !(left is string) && !(right is string))
            {
                var leftArray = leftEnum.Cast<object>().ToArray();
                var rightArray = rightEnum.Cast<object>().ToArray();

                if (leftArray.Length != rightArray.Length)
                    return false;

                for (int i = 0; i < leftArray.Length; i++)
                {
                    if (!AreValuesEqual(leftArray[i], rightArray[i]))
                        return false;
                }

                return true;
            }

            // Use standard equality comparison
            return left.Equals(right);
        }

        /// <summary>
        /// Logs detected configuration differences with detailed field-by-field changes.
        /// </summary>
        /// <param name="last">Last known configuration</param>
        /// <param name="desired">Desired configuration</param>
        private void LogConfigurationDifferences(Hashtable last, Hashtable desired)
        {
            var addedFields = new List<string>();
            var modifiedFields = new List<string>();
            var removedFields = new List<string>();

            // Check for added or modified fields
            foreach (DictionaryEntry entry in desired)
            {
                var key = entry.Key.ToString()!;
                if (!last.ContainsKey(entry.Key))
                {
                    addedFields.Add($"{key} = {FormatValueForLogging(entry.Value)}");
                }
                else if (!AreValuesEqual(entry.Value, last[entry.Key]))
                {
                    var oldValue = FormatValueForLogging(last[entry.Key]);
                    var newValue = FormatValueForLogging(entry.Value);
                    modifiedFields.Add($"{key}: {oldValue} → {newValue}");
                }
            }

            // Check for removed fields
            foreach (DictionaryEntry entry in last)
            {
                if (!desired.ContainsKey(entry.Key))
                {
                    var key = entry.Key.ToString()!;
                    removedFields.Add($"{key} = {FormatValueForLogging(entry.Value)}");
                }
            }

            // Log changes by category with detailed before/after values
            if (addedFields.Count > 0)
            {
                Logger.LogInformation("{EntityTypeName} configuration changes - ADDED fields: {AddedFields}", 
                    EntityTypeName, string.Join(", ", addedFields));
            }

            if (modifiedFields.Count > 0)
            {
                Logger.LogInformation("{EntityTypeName} configuration changes - MODIFIED fields: {ModifiedFields}", 
                    EntityTypeName, string.Join(", ", modifiedFields));
            }

            if (removedFields.Count > 0)
            {
                Logger.LogInformation("{EntityTypeName} configuration changes - REMOVED fields: {RemovedFields}", 
                    EntityTypeName, string.Join(", ", removedFields));
            }

            // Also log a summary count
            var totalChanges = addedFields.Count + modifiedFields.Count + removedFields.Count;
            if (totalChanges > 0)
            {
                Logger.LogInformation("{EntityTypeName} configuration drift detected: {TotalChanges} field changes ({AddedCount} added, {ModifiedCount} modified, {RemovedCount} removed)", 
                    EntityTypeName, totalChanges, addedFields.Count, modifiedFields.Count, removedFields.Count);
            }
        }

        /// <summary>
        /// Formats a value for logging, providing detailed representation with truncation for long values.
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <returns>Formatted string representation with type information</returns>
        private static string FormatValueForLogging(object? value)
        {
            if (value is null)
                return "(null)";

            // Handle different value types with more detail
            switch (value)
            {
                case string stringValue:
                    var quotedStr = $"\"{stringValue}\"";
                    return quotedStr.Length > 100 ? $"\"{stringValue[..95]}...\"" : quotedStr;
                
                case bool boolean:
                    return boolean.ToString().ToLowerInvariant();
                
                case int or long or float or double or decimal:
                    return value.ToString() ?? "(null)";
                
                case IEnumerable enumerable when value is not string:
                    var items = enumerable.Cast<object>().Take(5).Select(FormatValueForLogging);
                    var arrayPreview = string.Join(", ", items);
                    var count = enumerable.Cast<object>().Count();
                    return count > 5 ? $"[{arrayPreview}, ...] (total: {count} items)" : $"[{arrayPreview}]";
                
                case Hashtable hashtable:
                    var entries = hashtable.Cast<DictionaryEntry>().Take(3)
                        .Select(entry => $"{entry.Key}: {FormatValueForLogging(entry.Value)}");
                    var hashPreview = string.Join(", ", entries);
                    return hashtable.Count > 3 ? $"{{{hashPreview}, ...}} (total: {hashtable.Count} fields)" : $"{{{hashPreview}}}";
                
                default:
                    var objectStr = value.ToString() ?? "(null)";
                    var typeName = value.GetType().Name;
                    return objectStr.Length > 80 ? $"({typeName}) {objectStr[..75]}..." : $"({typeName}) {objectStr}";
            }
        }

    }

}
