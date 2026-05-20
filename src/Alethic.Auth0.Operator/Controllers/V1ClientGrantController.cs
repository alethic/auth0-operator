using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ClientGrant.V1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

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

    [EntityRbac(typeof(V1ClientGrant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ClientGrantController :
        V1TenantEntityInstanceController<V1ClientGrant, V1ClientGrant.SpecDef, V1ClientGrant.StatusDef, V1ClientGrantConf, V1ClientGrantConf>,
        IEntityController<V1ClientGrant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ClientGrantController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ClientGrantController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <summary>
        /// Converts from an API <see cref="ClientGrantOrganizationNullableUsageEnum"/> to a local <see cref="V1ClientGrantOrganizationUsage"/>.
        /// </summary>
        internal static V1ClientGrantOrganizationUsage? FromApi(ClientGrantOrganizationNullableUsageEnum? source) => source?.Value switch
        {
            ClientGrantOrganizationNullableUsageEnum.Values.Deny => V1ClientGrantOrganizationUsage.Deny,
            ClientGrantOrganizationNullableUsageEnum.Values.Allow => V1ClientGrantOrganizationUsage.Allow,
            ClientGrantOrganizationNullableUsageEnum.Values.Require => V1ClientGrantOrganizationUsage.Require,
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Converts from an API <see cref="ClientGrantOrganizationUsageEnum"/> to a local <see cref="V1ClientGrantOrganizationUsage"/>.
        /// </summary>
        internal static V1ClientGrantOrganizationUsage? FromApi(ClientGrantOrganizationUsageEnum? source) => source?.Value switch
        {
            ClientGrantOrganizationUsageEnum.Values.Deny => V1ClientGrantOrganizationUsage.Deny,
            ClientGrantOrganizationUsageEnum.Values.Allow => V1ClientGrantOrganizationUsage.Allow,
            ClientGrantOrganizationUsageEnum.Values.Require => V1ClientGrantOrganizationUsage.Require,
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Converts relevant fields from a <see cref="ClientGrantResponseContent"/> API response to a <see cref="V1ClientGrantConf"/>.
        /// Note: <see cref="V1ClientGrantConf.ClientRef"/> and <see cref="V1ClientGrantConf.Audience"/> cannot be
        /// populated from the API response and are left null.
        /// </summary>
        internal static V1ClientGrantConf? FromApi(GetClientGrantResponseContent? source)
        {
            if (source is null)
                return null;

            return new V1ClientGrantConf
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
        /// Converts from a local <see cref="V1ClientGrantOrganizationUsage"/> to an API <see cref="ClientGrantOrganizationUsageEnum"/>.
        /// </summary>
        internal static ClientGrantOrganizationUsageEnum? ToApi(V1ClientGrantOrganizationUsage? source) => source switch
        {
            V1ClientGrantOrganizationUsage.Deny => new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Deny),
            V1ClientGrantOrganizationUsage.Allow => new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Allow),
            V1ClientGrantOrganizationUsage.Require => new ClientGrantOrganizationUsageEnum(ClientGrantOrganizationUsageEnum.Values.Require),
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="CreateClientGrantRequestContent"/>.
        /// </summary>
        internal static void ApplyToApi(V1ClientGrantConf conf, CreateClientGrantRequestContent request)
        {
            request.Scope = conf.Scope;
            request.AllowAnyOrganization = conf.AllowAnyOrganization;
            request.OrganizationUsage = ToApi(conf.OrganizationUsage);
        }

        /// <summary>
        /// Converts from a local <see cref="V1ClientGrantOrganizationUsage"/> to an API <see cref="ClientGrantOrganizationNullableUsageEnum"/>.
        /// </summary>
        internal static ClientGrantOrganizationNullableUsageEnum? ToApiNullable(V1ClientGrantOrganizationUsage? source) => source switch
        {
            V1ClientGrantOrganizationUsage.Deny => new ClientGrantOrganizationNullableUsageEnum(ClientGrantOrganizationNullableUsageEnum.Values.Deny),
            V1ClientGrantOrganizationUsage.Allow => new ClientGrantOrganizationNullableUsageEnum(ClientGrantOrganizationNullableUsageEnum.Values.Allow),
            V1ClientGrantOrganizationUsage.Require => new ClientGrantOrganizationNullableUsageEnum(ClientGrantOrganizationNullableUsageEnum.Values.Require),
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="UpdateClientGrantRequestContent"/>.
        /// </summary>
        internal static void ApplyToApi(V1ClientGrantConf conf, UpdateClientGrantRequestContent request)
        {
            request.Scope = conf.Scope;
            request.AllowAnyOrganization = conf.AllowAnyOrganization;
            if (conf.OrganizationUsage is not null)
                request.OrganizationUsage = ToApiNullable(conf.OrganizationUsage);
        }

        /// <inheritdoc />
        protected override string EntityTypeName => "ClientGrant";

        /// <inheritdoc />
        protected override async Task<V1ClientGrantConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            var self = await api.ClientGrants.GetAsync(id, null, cancellationToken);
            return FromApi((GetClientGrantResponseContent?)self);
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1ClientGrant entity, V1ClientGrant.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
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
        protected override string? ValidateCreate(V1ClientGrantConf conf)
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
        protected override async Task<string> Create(IManagementApiClient api, V1ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
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
        protected override async Task Update(IManagementApiClient api, string id, V1ClientGrantConf? last, V1ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
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
