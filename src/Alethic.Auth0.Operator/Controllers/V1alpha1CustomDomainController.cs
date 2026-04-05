using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.CustomDomain.V1alpha1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1alpha1CustomDomain), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1alpha1CustomDomainController :
        V1TenantEntityInstanceController<V1alpha1CustomDomain, V1alpha1CustomDomain.SpecDef, V1alpha1CustomDomain.StatusDef, V1alpha1CustomDomainConf, V1alpha1CustomDomainConf>,
        IEntityController<V1alpha1CustomDomain>
    {

        /// <summary>
        /// Transforms the specified certificate provisioning method to the API representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static CustomDomainCertificateProvisioning ToApi(V1alpha1CustomDomainCertificateProvisioning value) => value switch
        {
            V1alpha1CustomDomainCertificateProvisioning.Auth0ManagedCertificate => CustomDomainCertificateProvisioning.Auth0ManagedCertificate,
            V1alpha1CustomDomainCertificateProvisioning.SelfManagedCertificate => CustomDomainCertificateProvisioning.SelfManagedCertificate,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified verification method to the API representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static string ToApi(V1alpha1CustomDomainVerificationMethod value) => value switch
        {
            V1alpha1CustomDomainVerificationMethod.TXT => "txt",
            V1alpha1CustomDomainVerificationMethod.CNAME => "cname",
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        static void ApplyToApi(V1alpha1CustomDomainConf? source, CustomDomainCreateRequest target)
        {
            if (source is null)
                return;

            if (source.Domain is not null)
                target.Domain = source.Domain;

            if (source.Type is not null)
                target.Type = ToApi(source.Type.Value);

            if (source.VerificationMethod is not null)
                target.VerificationMethod = ToApi(source.VerificationMethod.Value);

            if (source.TlsPolicy is not null)
                target.TlsPolicy = source.TlsPolicy;

            if (source.CustomClientIpHeader is not null)
                target.CustomClientIpHeader = source.CustomClientIpHeader;
        }

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        static void ApplyToApi(V1alpha1CustomDomainConf? source, CustomDomainUpdateRequest target)
        {
            if (source is null)
                return;

            if (source.TlsPolicy is not null)
                target.TlsPolicy = source.TlsPolicy;

            if (source.CustomClientIpHeader is not null)
                target.CustomClientIpHeader = source.CustomClientIpHeader;
        }

        /// <summary>
        /// Transforms the specified certificate provisioning method from the API representation to the internal representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static V1alpha1CustomDomainCertificateProvisioning? FromApi(CustomDomainCertificateProvisioning? value) => value switch
        {
            CustomDomainCertificateProvisioning.Auth0ManagedCertificate => V1alpha1CustomDomainCertificateProvisioning.Auth0ManagedCertificate,
            CustomDomainCertificateProvisioning.SelfManagedCertificate => V1alpha1CustomDomainCertificateProvisioning.SelfManagedCertificate,
            null => null,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified verification method from the API representation to the internal representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static V1alpha1CustomDomainVerificationMethod? FromApi(string? value) => value?.Trim()?.ToLowerInvariant() switch
        {
            "txt" => V1alpha1CustomDomainVerificationMethod.TXT,
            "cname" => V1alpha1CustomDomainVerificationMethod.CNAME,
            "" => null,
            null => null,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified API representation to the internal representation.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static V1alpha1CustomDomainConf FromApi(CustomDomain source) => new()
        {
            Domain = source.Domain,
            Type = FromApi(source.Type),
            VerificationMethod = source.Verification.Methods.Length > 0 ? FromApi(source.Verification.Methods[0].Name) : null,
            TlsPolicy = source.TlsPolicy,
            CustomClientIpHeader = source.CustomClientIpHeader,
            Primary = source.Primary
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1alpha1CustomDomainController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1alpha1CustomDomainController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "CustomDomain";

        /// <inheritdoc />
        protected override async Task<V1alpha1CustomDomainConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.CustomDomains.GetAsync(id, cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1alpha1CustomDomain entity, V1alpha1CustomDomain.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            var list = await api.CustomDomains.GetAllAsync(cancellationToken);
            var self = list.FirstOrDefault(i => i.Domain == conf.Domain);
            return self?.CustomDomainId;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1alpha1CustomDomainConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1alpha1CustomDomainConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating custom domain in Auth0 with name: {Domain}", EntityTypeName, conf.Domain);

            var req = new CustomDomainCreateRequest();
            ApplyToApi(conf, req);

            var self = await api.CustomDomains.CreateAsync(req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created custom domain in Auth0 with ID: {CustomDomainId} and name: {Domain}", EntityTypeName, self.CustomDomainId, conf.Domain);
            return self.CustomDomainId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1alpha1CustomDomainConf? last, V1alpha1CustomDomainConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating custom domain in Auth0 with id: {CustomDomainId} and name: {Domain}", EntityTypeName, id, conf.Domain);

            // apply last conf to request to ensure we don't overwrite any properties not managed by us
            var req = new CustomDomainUpdateRequest();
            ApplyToApi(last, req);
            ApplyToApi(conf, req);

            await api.CustomDomains.UpdateAsync(id, req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated custom domain in Auth0 with id: {CustomDomainId} and name: {Domain}", EntityTypeName, id, conf.Domain);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1alpha1CustomDomain entity, V1alpha1CustomDomainConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting custom domain from Auth0 with ID: {Domain} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.CustomDomains.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted custom domain from Auth0 with ID: {Domain}", EntityTypeName, id);
        }

    }

}
