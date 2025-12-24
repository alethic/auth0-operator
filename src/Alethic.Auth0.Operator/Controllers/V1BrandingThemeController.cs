using System.Collections;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.BrandingTheme;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
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
    [EntityRbac(typeof(V1BrandingTheme), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1BrandingThemeController :
        V1TenantEntityController<V1BrandingTheme, V1BrandingTheme.SpecDef, V1BrandingTheme.StatusDef, BrandingThemeConf>,
        IEntityController<V1BrandingTheme>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public V1BrandingThemeController(IKubernetesClient kube, EntityRequeue<V1BrandingTheme> requeue, IMemoryCache cache, ILogger<V1BrandingThemeController> logger, IOptions<OperatorOptions> options) :
            base(kube, requeue, cache, logger, options)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "BrandingTheme";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return TransformToSystemTextJson<Hashtable>(await api.Branding.GetBrandingThemeAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1BrandingTheme entity, V1BrandingTheme.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.Id is string id)
                {
                    try
                    {
                        var theme = await api.Branding.GetBrandingThemeAsync(id, cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing theme: {DisplayName}", EntityTypeName, entity.Namespace(), entity.Name(), theme.DisplayName);
                        return theme.ThemeId;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find theme with id {ThemeId}.", EntityTypeName, entity.Namespace(), entity.Name(), id);
                        return null;
                    }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(BrandingThemeConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating theme in Auth0 with name: {ThemeName}", EntityTypeName, conf.DisplayName);
            var self = await api.Branding.CreateBrandingThemeAsync(TransformToNewtonsoftJson<BrandingThemeConf, BrandingThemeCreateRequest>(conf), cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created theme in Auth0 with ID: {ThemeId} and name: {ThemeName}", EntityTypeName, self.ThemeId, conf.DisplayName);
            return self.ThemeId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, BrandingThemeConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);

            // transform initial request
            var req = TransformToNewtonsoftJson<BrandingThemeConf, BrandingThemeUpdateRequest>(conf);
            await api.Branding.UpdateBrandingThemeAsync(id, req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated theme in Auth0 with id: {ThemeId} and name: {ThemeName}", EntityTypeName, id, conf.DisplayName);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1BrandingTheme entity, Hashtable lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting theme from Auth0 with ID: {ThemeId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Clients.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted theme from Auth0 with ID: {ThemeId}", EntityTypeName, id);
        }

    }

}
