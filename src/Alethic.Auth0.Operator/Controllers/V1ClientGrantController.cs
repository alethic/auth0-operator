using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ClientGrant.V1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

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
        /// Converts from an API <see cref="OrganizationUsage"/> to a local <see cref="V1ClientGrantOrganizationUsage"/>.
        /// </summary>
        internal static V1ClientGrantOrganizationUsage? FromApi(OrganizationUsage? source) => source switch
        {
            OrganizationUsage.Deny => V1ClientGrantOrganizationUsage.Deny,
            OrganizationUsage.Allow => V1ClientGrantOrganizationUsage.Allow,
            OrganizationUsage.Require => V1ClientGrantOrganizationUsage.Require,
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Converts relevant fields from a <see cref="ClientGrant"/> API response to a <see cref="V1ClientGrantConf"/>.
        /// Note: <see cref="V1ClientGrantConf.ClientRef"/> and <see cref="V1ClientGrantConf.Audience"/> cannot be
        /// populated from the API response and are left null.
        /// </summary>
        internal static V1ClientGrantConf? FromApi(ClientGrant? source)
        {
            if (source is null)
                return null;

            return new V1ClientGrantConf
            {
                Scope = source.Scope?.ToArray(),
                OrganizationUsage = FromApi(source.OrganizationUsage),
                AllowAnyOrganization = source.AllowAnyOrganization,
            };
        }

        /// <summary>
        /// Converts from a local <see cref="V1ClientGrantOrganizationUsage"/> to an API <see cref="OrganizationUsage"/>.
        /// </summary>
        internal static OrganizationUsage? ToApi(V1ClientGrantOrganizationUsage? source) => source switch
        {
            V1ClientGrantOrganizationUsage.Deny => OrganizationUsage.Deny,
            V1ClientGrantOrganizationUsage.Allow => OrganizationUsage.Allow,
            V1ClientGrantOrganizationUsage.Require => OrganizationUsage.Require,
            null => null,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="ClientGrantBase"/> (create request).
        /// </summary>
        internal static void ApplyToApi(V1ClientGrantConf conf, ClientGrantBase request)
        {
            request.Scope = conf.Scope?.ToList() ?? null!;
            request.AllowAnyOrganization = conf.AllowAnyOrganization;
            request.OrganizationUsage = ToApi(conf.OrganizationUsage);
        }

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="ClientGrantUpdateRequest"/>.
        /// </summary>
        internal static void ApplyToApi(V1ClientGrantConf conf, ClientGrantUpdateRequest request)
        {
            request.Scope = conf.Scope?.ToList() ?? null!;
            request.AllowAnyOrganization = conf.AllowAnyOrganization;
            request.OrganizationUsage = ToApi(conf.OrganizationUsage);
        }

        /// <inheritdoc />
        protected override string EntityTypeName => "ClientGrant";

        /// <inheritdoc />
        protected override async Task<V1ClientGrantConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            var list = await api.ClientGrants.GetAllAsync(new GetClientGrantsRequest(), cancellationToken: cancellationToken);
            var self = list.FirstOrDefault(i => i.Id == id);
            return FromApi(self);
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

            var list = await api.ClientGrants.GetAllAsync(new GetClientGrantsRequest() { ClientId = clientId }, null!, cancellationToken);
            return list.Where(i => i.ClientId == clientId && i.Audience == audience).Select(i => i.Id).FirstOrDefault();
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
            var req = new ClientGrantCreateRequest();
            req.ClientId = await ResolveClientRefToId(api, conf.ClientRef, defaultNamespace, cancellationToken) ?? null!;
            req.Audience = await ResolveResourceServerRefToIdentifier(api, conf.Audience, defaultNamespace, cancellationToken) ?? null!;
            ApplyToApi(conf, req);

            var self = await api.ClientGrants.CreateAsync(req, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ClientGrantConf? last, V1ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new ClientGrantUpdateRequest();
            ApplyToApi(conf, req);

            await api.ClientGrants.UpdateAsync(id, req, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.ClientGrants.DeleteAsync(id, cancellationToken);
        }

    }

}
