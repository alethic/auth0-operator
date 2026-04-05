using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.CustomText.V1alpha1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

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

    [EntityRbac(typeof(V1alpha1CustomText), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1alpha1CustomTextController :
        V1TenantEntityController<V1alpha1CustomText, V1alpha1CustomText.SpecDef, V1alpha1CustomText.StatusDef, V1alpha1CustomTextConf, V1alpha1CustomTextConf>,
        IEntityController<V1alpha1CustomText>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1alpha1CustomTextController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1alpha1CustomTextController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "CustomText";

        /// <inheritdoc />
        protected override async Task<V1alpha1CustomText> ReconcileAsync(IManagementApiClient api, V2alpha1Tenant tenant, V1alpha1CustomText entity, CancellationToken cancellationToken)
        {
            if (entity.Spec.Prompt is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing prompt.");

            if (entity.Spec.Language is null)
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} missing language.");

            var screens = await api.Prompts.GetCustomTextForPromptAsync(entity.Spec.Prompt, entity.Spec.Language, cancellationToken: cancellationToken);
            if (screens is null)
                throw new RetryException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} custom text cannot be loaded from API.");

            // configuration was specified
            if (entity.Spec.Conf is { } conf)
            {
                // screens may not be specified
                if (conf.Screens is { } newScreens)
                {
                    // push changes to Auth0 and reload resulting configuration
                    await api.Prompts.SetCustomTextForPromptAsync(entity.Spec.Prompt, entity.Spec.Language, newScreens, cancellationToken);
                    screens = await api.Prompts.GetCustomTextForPromptAsync(entity.Spec.Prompt, entity.Spec.Language, cancellationToken: cancellationToken);
                }
            }

            // apply resulting values to status
            entity.Status.LastConf ??= new V1alpha1CustomTextConf();
            entity.Status.LastConf.Screens = TransformToSystemTextJson<Dictionary<string, V1alpha1CustomTextScreen>>(screens);
            return entity;
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(IManagementApiClient api, V2alpha1Tenant tenant, V1alpha1CustomText entity, CancellationToken cancellationToken)
        {
            Logger.LogWarning("Unsupported operation deleting entity {Entity}.", entity);
            return Task.CompletedTask;
        }

    }

}
