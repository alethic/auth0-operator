using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.CustomText.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Auth0.ManagementApi;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3CustomText), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3CustomTextController :
        V1TenantEntityController<V2alpha3CustomText, V2alpha3CustomText.SpecDef, V2alpha3CustomText.StatusDef, V2alpha3CustomTextConf, V2alpha3CustomTextConf>,
        IEntityController<V2alpha3CustomText>
    {

        static V2alpha3CustomTextScreen FromApiScreen(Dictionary<string, object> source)
        {
            var screen = new V2alpha3CustomTextScreen();

            foreach (var kvp in source)
                screen[kvp.Key] = kvp.Value?.ToString() ?? string.Empty;

            return screen;
        }

        static Dictionary<string, V2alpha3CustomTextScreen>? FromApiScreens(Dictionary<string, object>? source)
        {
            if (source is null)
                return null;

            var result = new Dictionary<string, V2alpha3CustomTextScreen>();

            foreach (var kvp in source)
                if (kvp.Value is Dictionary<string, object> screenDict)
                    result[kvp.Key] = FromApiScreen(screenDict);

            return result;
        }

        /// <summary>
        /// Determines whether a desired screen differs from the screen reported by the API. SetAsync is a full
        /// replace, so equality must be exact in both directions: the key sets must match and every value must
        /// be equal.
        /// </summary>
        internal static bool ScreenRequiresUpdate(V2alpha3CustomTextScreen conf, V2alpha3CustomTextScreen last)
        {
            if (conf.Count != last.Count)
                return true;

            foreach (var entry in conf)
            {
                if (last.TryGetValue(entry.Key, out var lastValue) == false)
                    return true;
                if (entry.Value != lastValue)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the desired screens differ from the screens reported by the API. SetAsync is a
        /// full replace, so equality must be exact in both directions: a screen or key removed from the spec
        /// must still trigger the write.
        /// </summary>
        internal static bool ScreensRequireUpdate(Dictionary<string, V2alpha3CustomTextScreen> conf, Dictionary<string, V2alpha3CustomTextScreen>? last)
        {
            if (last is null)
                return true;
            if (conf.Count != last.Count)
                return true;

            foreach (var screen in conf)
            {
                if (last.TryGetValue(screen.Key, out var lastScreen) == false)
                    return true;
                if (ScreenRequiresUpdate(screen.Value, lastScreen))
                    return true;
            }

            return false;
        }

        static Dictionary<string, object> ToApiScreens(Dictionary<string, V2alpha3CustomTextScreen> source)
        {
            var result = new Dictionary<string, object>();

            foreach (var kvp in source)
            {
                var screen = new Dictionary<string, object>();
                foreach (var entry in kvp.Value)
                    screen[entry.Key] = entry.Value;

                result[kvp.Key] = screen;
            }

            return result;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3CustomTextController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3CustomTextController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "CustomText";

        /// <inheritdoc />
        protected override async Task<V2alpha3CustomText> ReconcileAsync(IManagementApiClient api, V2alpha3Tenant tenant, V2alpha3CustomText entity, CancellationToken cancellationToken)
        {
            if (entity.Spec.Prompt is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing prompt.");

            if (entity.Spec.Language is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing language.");

            var prompt = new PromptGroupNameEnum(entity.Spec.Prompt);
            var language = new PromptLanguageEnum(entity.Spec.Language);

            var screens = await api.Prompts.CustomText.GetAsync(prompt, language, cancellationToken: cancellationToken);
            if (screens is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} custom text cannot be loaded from API.");

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                // screens may not be specified; SetAsync is a full replace, so a screen or key removed from the
                // spec must still be pushed — compare for exact equality rather than as a subset
                if (conf.Screens is { } newScreens && ScreensRequireUpdate(newScreens, FromApiScreens(screens)))
                {
                    // push changes to Auth0 and reload resulting configuration
                    await api.Prompts.CustomText.SetAsync(prompt, language, ToApiScreens(newScreens), cancellationToken: cancellationToken);
                    screens = await api.Prompts.CustomText.GetAsync(prompt, language, cancellationToken: cancellationToken);
                }
            }

            // apply resulting values to status
            entity.Status.LastConf ??= new V2alpha3CustomTextConf();
            entity.Status.LastConf.Screens = FromApiScreens(screens);
            return entity;
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(IManagementApiClient api, V2alpha3Tenant tenant, V2alpha3CustomText entity, CancellationToken cancellationToken)
        {
            Logger.LogWarning("Unsupported operation deleting entity {Entity}.", entity);
            return Task.CompletedTask;
        }

    }

}
