using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Core;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3CustomDomain), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3CustomDomainController :
        V1TenantEntityInstanceController<V2alpha3CustomDomain, V2alpha3CustomDomain.SpecDef, V2alpha3CustomDomain.StatusDef, V2alpha3CustomDomainConf, V2alpha3CustomDomainConf>,
        IEntityController<V2alpha3CustomDomain>
    {

        /// <summary>
        /// Transforms the specified certificate provisioning method to the API representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static CustomDomainProvisioningTypeEnum ToApi(V2alpha3CustomDomainCertificateProvisioning value) => value switch
        {
            V2alpha3CustomDomainCertificateProvisioning.Auth0ManagedCertificate => CustomDomainProvisioningTypeEnum.Auth0ManagedCerts,
            V2alpha3CustomDomainCertificateProvisioning.SelfManagedCertificate => CustomDomainProvisioningTypeEnum.SelfManagedCerts,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified verification method to the API representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static string ToApi(V2alpha3CustomDomainVerificationMethod value) => value switch
        {
            V2alpha3CustomDomainVerificationMethod.TXT => "txt",
            V2alpha3CustomDomainVerificationMethod.CNAME => "cname",
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Builds a <see cref="CreateCustomDomainRequestContent"/> from the specified configuration.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static CreateCustomDomainRequestContent ToCreateRequest(V2alpha3CustomDomainConf source) => new()
        {
            Domain = source.Domain ?? throw new InvalidOperationException("Domain is required."),
            Type = source.Type is not null ? ToApi(source.Type.Value) : throw new InvalidOperationException("Type is required."),
            VerificationMethod = source.VerificationMethod is not null ? new CustomDomainVerificationMethodEnum(ToApi(source.VerificationMethod.Value)) : null,
            TlsPolicy = source.TlsPolicy is not null ? new CustomDomainTlsPolicyEnum(source.TlsPolicy) : null,
            CustomClientIpHeader = source.CustomClientIpHeader is not null ? Optional<CustomDomainCustomClientIpHeaderEnum?>.Of(new CustomDomainCustomClientIpHeaderEnum(source.CustomClientIpHeader)) : default,
        };

        /// <summary>
        /// Applies the specified configuration to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        static void ApplyToApi(V2alpha3CustomDomainConf? source, UpdateCustomDomainRequestContent target)
        {
            if (source is null)
                return;

            if (source.TlsPolicy is not null)
                target.TlsPolicy = new CustomDomainTlsPolicyEnum(source.TlsPolicy);

            if (source.CustomClientIpHeader is not null)
                target.CustomClientIpHeader = Optional<CustomDomainCustomClientIpHeaderEnum?>.Of(new CustomDomainCustomClientIpHeaderEnum(source.CustomClientIpHeader));
        }

        /// <summary>
        /// Transforms the specified certificate provisioning method from the API representation to the internal representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static V2alpha3CustomDomainCertificateProvisioning? FromApi(CustomDomainTypeEnum? value) => value?.Value switch
        {
            CustomDomainTypeEnum.Values.Auth0ManagedCerts => V2alpha3CustomDomainCertificateProvisioning.Auth0ManagedCertificate,
            CustomDomainTypeEnum.Values.SelfManagedCerts => V2alpha3CustomDomainCertificateProvisioning.SelfManagedCertificate,
            null => null,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified verification method from the API representation to the internal representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static V2alpha3CustomDomainVerificationMethod? FromApi(string? value) => value?.Trim()?.ToLowerInvariant() switch
        {
            "txt" => V2alpha3CustomDomainVerificationMethod.TXT,
            "cname" => V2alpha3CustomDomainVerificationMethod.CNAME,
            "" => null,
            null => null,
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Transforms the specified API representation to the internal representation.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static V2alpha3CustomDomainConf FromApi(GetCustomDomainResponseContent source) => new()
        {
            Domain = source.Domain,
            Type = FromApi(source.Type),
            VerificationMethod = source.Verification?.Methods?.FirstOrDefault() is { } m ? FromApi(m.Name.Value) : null,
            TlsPolicy = source.TlsPolicy,
            CustomClientIpHeader = source.CustomClientIpHeader.Value,  // Optional<string?> → string?
            Primary = source.Primary
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3CustomDomainController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V2alpha3CustomDomainController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "CustomDomain";

        /// <inheritdoc />
        protected override async Task<V2alpha3CustomDomainConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.CustomDomains.GetAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3CustomDomain entity, V2alpha3CustomDomain.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            var list = await api.CustomDomains.ListAsync(new ListCustomDomainsRequestParameters(), cancellationToken: cancellationToken);
            var self = list.FirstOrDefault(i => i.Domain == conf.Domain);
            return self?.CustomDomainId;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V2alpha3CustomDomainConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3CustomDomainConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating custom domain in Auth0 with name: {Domain}", EntityTypeName, conf.Domain);

            var req = ToCreateRequest(conf);

            var self = await api.CustomDomains.CreateAsync(req, cancellationToken: cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created custom domain in Auth0 with ID: {CustomDomainId} and name: {Domain}", EntityTypeName, self.CustomDomainId, conf.Domain);
            return self.CustomDomainId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3CustomDomainConf? last, V2alpha3CustomDomainConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating custom domain in Auth0 with id: {CustomDomainId} and name: {Domain}", EntityTypeName, id, conf.Domain);

            var req = new UpdateCustomDomainRequestContent();
            ApplyToApi(last, req);
            ApplyToApi(conf, req);

            await api.CustomDomains.UpdateAsync(id, req, cancellationToken: cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated custom domain in Auth0 with id: {CustomDomainId} and name: {Domain}", EntityTypeName, id, conf.Domain);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V2alpha3CustomDomain entity, V2alpha3CustomDomainConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting custom domain from Auth0 with ID: {Domain} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.CustomDomains.DeleteAsync(id, cancellationToken: cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted custom domain from Auth0 with ID: {Domain}", EntityTypeName, id);
        }

    }

}
