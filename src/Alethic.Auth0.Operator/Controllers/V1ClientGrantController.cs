﻿using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ClientGrant;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

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
    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1ResourceServer), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1ClientGrant), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.Create)]
    public class V1ClientGrantController :
        V1TenantEntityController<V1ClientGrant, V1ClientGrant.SpecDef, V1ClientGrant.StatusDef, ClientGrantConf>,
        IEntityController<V1ClientGrant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="requeue"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public V1ClientGrantController(IKubernetesClient kube, EntityRequeue<V1ClientGrant> requeue, IMemoryCache cache, ILogger<V1ClientGrantController> logger, IOptions<OperatorOptions> options) :
            base(kube, requeue, cache, logger, options)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "A0ClientGrant";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                var list = await api.ClientGrants.GetAllAsync(new GetClientGrantsRequest(), cancellationToken: cancellationToken);
                var self = list.FirstOrDefault(i => i.Id == id);
                if (self == null)
                    return null;

                return TransformToSystemTextJson<Hashtable>(self);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error retrieving {EntityTypeName} with ID {Id}: {Message}", EntityTypeName, id, e.Message);
                throw;
            }
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
        protected override string? ValidateCreate(ClientGrantConf conf)
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
        protected override async Task<string> Create(IManagementApiClient api, ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new ClientGrantCreateRequest();
            req.ClientId = await ResolveClientRefToId(api, conf.ClientRef, defaultNamespace, cancellationToken);
            req.Audience = await ResolveResourceServerRefToIdentifier(api, conf.Audience, defaultNamespace, cancellationToken);
            req.Scope = conf.Scope?.ToList();
            req.AllowAnyOrganization = conf.AllowAnyOrganization;
            req.OrganizationUsage = Convert(conf.OrganizationUsage);

            var self = await api.ClientGrants.CreateAsync(req, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, ClientGrantConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new ClientGrantUpdateRequest();
            req.Scope = conf.Scope?.ToList();
            req.AllowAnyOrganization = conf.AllowAnyOrganization;
            req.OrganizationUsage = Convert(conf.OrganizationUsage);

            await api.ClientGrants.UpdateAsync(id, req, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task Delete(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.ClientGrants.DeleteAsync(id, cancellationToken);
        }

        /// <summary>
        /// Converts a from a local model type to a request type.
        /// </summary>
        /// <param name="organizationUsage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        global::Auth0.ManagementApi.Models.OrganizationUsage? Convert(global::Alethic.Auth0.Operator.Core.Models.OrganizationUsage? organizationUsage)
        {
            return organizationUsage switch
            {
                Core.Models.OrganizationUsage.Deny => global::Auth0.ManagementApi.Models.OrganizationUsage.Deny,
                Core.Models.OrganizationUsage.Allow => global::Auth0.ManagementApi.Models.OrganizationUsage.Allow,
                Core.Models.OrganizationUsage.Require => global::Auth0.ManagementApi.Models.OrganizationUsage.Require,
                null => null,
                _ => throw new InvalidOperationException(),
            };
        }

    }

}
