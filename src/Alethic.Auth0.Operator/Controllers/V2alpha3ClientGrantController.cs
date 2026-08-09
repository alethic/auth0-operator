using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Auth0.ManagementApi;
using Auth0.ManagementApi.ClientGrants;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3ClientGrant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3ClientGrantController :
        V1TenantEntityInstanceController<V2alpha3ClientGrant, V2alpha3ClientGrant.SpecDef, V2alpha3ClientGrant.StatusDef, V2alpha3ClientGrantConf, V2alpha3ClientGrantConf>,
        IEntityController<V2alpha3ClientGrant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3ClientGrantController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3ClientGrantController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
        {

        }

        /// <summary>
        /// Converts from an API <see cref="ClientGrantOrganizationNullableUsageEnum"/> to a local <see cref="V2alpha3ClientGrantOrganizationUsage"/>.
        /// </summary>
        internal static V2alpha3ClientGrantOrganizationUsage? FromApi(ClientGrantOrganizationNullableUsageEnum? source) => source?.Value switch
        {
            ClientGrantOrganizationNullableUsageEnum.Values.Deny => V2alpha3ClientGrantOrganizationUsage.Deny,
            ClientGrantOrganizationNullableUsageEnum.Values.Allow => V2alpha3ClientGrantOrganizationUsage.Allow,
            ClientGrantOrganizationNullableUsageEnum.Values.Require => V2alpha3ClientGrantOrganizationUsage.Require,
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Converts from an API <see cref="ClientGrantOrganizationUsageEnum"/> to a local <see cref="V2alpha3ClientGrantOrganizationUsage"/>.
        /// </summary>
        internal static V2alpha3ClientGrantOrganizationUsage? FromApi(ClientGrantOrganizationUsageEnum? source) => source?.Value switch
        {
            ClientGrantOrganizationUsageEnum.Values.Deny => V2alpha3ClientGrantOrganizationUsage.Deny,
            ClientGrantOrganizationUsageEnum.Values.Allow => V2alpha3ClientGrantOrganizationUsage.Allow,
            ClientGrantOrganizationUsageEnum.Values.Require => V2alpha3ClientGrantOrganizationUsage.Require,
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Converts relevant fields from a <see cref="ClientGrantResponseContent"/> API response to a <see cref="V2alpha3ClientGrantConf"/>.
        /// Note: <see cref="V2alpha3ClientGrantConf.ClientRef"/> and <see cref="V2alpha3ClientGrantConf.Audience"/> cannot be
        /// populated from the API response and are left null.
        /// </summary>
        internal static V2alpha3ClientGrantConf? FromApi(GetClientGrantResponseContent? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ClientGrantConf
            {
                Scope = source.Scope?.ToArray(),
                OrganizationUsage = FromApi(source.OrganizationUsage),
                AllowAnyOrganization = source.AllowAnyOrganization,
                Audience = new Core.Models.V1ResourceServerReference()
                {
                    Identifier = source.Audience
                },
                ClientRef = new Core.Models.V1ClientReference()
                {
                    Id = source.ClientId
                }
            };
        }

        /// <summary>
        /// Converts from a local <see cref="V2alpha3ClientGrantOrganizationUsage"/> to an API <see cref="ClientGrantOrganizationUsageEnum"/>.
        /// </summary>
        internal static ClientGrantOrganizationUsageEnum? ToApi(V2alpha3ClientGrantOrganizationUsage? source) => source switch
        {
            V2alpha3ClientGrantOrganizationUsage.Deny => new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Deny),
            V2alpha3ClientGrantOrganizationUsage.Allow => new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Allow),
            V2alpha3ClientGrantOrganizationUsage.Require => new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Require),
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="CreateClientGrantRequestContent"/>.
        /// </summary>
        internal static void ApplyToApi(V2alpha3ClientGrantConf conf, CreateClientGrantRequestContent request)
        {
            request.Scope = conf.Scope;
            request.AllowAnyOrganization = conf.AllowAnyOrganization;
            request.OrganizationUsage = ToApi(conf.OrganizationUsage);
        }

        /// <summary>
        /// Converts from a local <see cref="V2alpha3ClientGrantOrganizationUsage"/> to an API <see cref="ClientGrantOrganizationNullableUsageEnum"/>.
        /// </summary>
        internal static ClientGrantOrganizationNullableUsageEnum? ToApiNullable(V2alpha3ClientGrantOrganizationUsage? source) => source switch
        {
            V2alpha3ClientGrantOrganizationUsage.Deny => new ClientGrantOrganizationNullableUsageEnum(ClientGrantOrganizationNullableUsageEnum.Values.Deny),
            V2alpha3ClientGrantOrganizationUsage.Allow => new ClientGrantOrganizationNullableUsageEnum(ClientGrantOrganizationNullableUsageEnum.Values.Allow),
            V2alpha3ClientGrantOrganizationUsage.Require => new ClientGrantOrganizationNullableUsageEnum(ClientGrantOrganizationNullableUsageEnum.Values.Require),
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="UpdateClientGrantRequestContent"/>.
        /// </summary>
        internal static void ApplyToApi(V2alpha3ClientGrantConf conf, UpdateClientGrantRequestContent request)
        {
            request.Scope = conf.Scope;
            request.AllowAnyOrganization = conf.AllowAnyOrganization;
            if (conf.OrganizationUsage is not null)
                request.OrganizationUsage = ToApiNullable(conf.OrganizationUsage);
        }

        /// <inheritdoc />
        protected override string EntityTypeName => "ClientGrant";

        /// <inheritdoc />
        protected override async Task<V2alpha3ClientGrantConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                var self = await api.ClientGrants.GetAsync(id, null, cancellationToken);
                return FromApi((GetClientGrantResponseContent?)self);
            }
            catch (NotFoundError)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3ClientGrant entity, V2alpha3ClientGrant.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            if (conf.ClientRef is null)
                throw new InvalidOperationException("ClientRef is required.");

            var clientId = await ResolveClientRefToId(api, conf.ClientRef, defaultNamespace, cancellationToken);
            if (string.IsNullOrWhiteSpace(clientId))
                throw new InvalidOperationException();

            if (conf.Audience is null)
                throw new InvalidOperationException("Audience is required.");

            var audience = await ResolveResourceServerRefToIdentifier(api, conf.Audience, defaultNamespace, cancellationToken);
            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException();

            var pager = await api.ClientGrants.ListAsync(new ListClientGrantsRequestParameters { ClientId = clientId }, null, cancellationToken);
            return pager.CurrentPage.Items?.Where(i => i.ClientId == clientId && i.Audience == audience).Select(i => i.Id).FirstOrDefault();
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V2alpha3ClientGrantConf conf)
        {
            if (conf.ClientRef is null)
                return "missing a value for ClientRef";
            if (conf.Audience is null)
                return "missing a value for Audience";
            if (conf.Scope is null)
                return "missing a value for Scope";

            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var clientId = await ResolveClientRefToId(api, conf.ClientRef, defaultNamespace, cancellationToken) ?? null!;
            var audience = await ResolveResourceServerRefToIdentifier(api, conf.Audience, defaultNamespace, cancellationToken) ?? null!;
            var req = new CreateClientGrantRequestContent { ClientId = clientId, Audience = audience };
            ApplyToApi(conf, req);

            var self = await api.ClientGrants.CreateAsync(req, null, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3ClientGrantConf? last, V2alpha3ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new UpdateClientGrantRequestContent();
            ApplyToApi(conf, req);

            await api.ClientGrants.UpdateAsync(id, req, null, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.ClientGrants.DeleteAsync(id, null, cancellationToken);
        }

    }

}
