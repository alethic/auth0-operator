using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Roles;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3Role), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1ResourceServer), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3RoleController :
        V1TenantEntityInstanceController<V2alpha3Role, V2alpha3Role.SpecDef, V2alpha3Role.StatusDef, V2alpha3RoleConf, V2alpha3RoleConf>,
        IEntityController<V2alpha3Role>
    {

        /// <summary>
        /// Transforms the Auth0 Management API role model and its associated permissions to the operator's role configuration model.
        /// Note: <see cref="V2alpha3RolePermission.ResourceServerRef"/> is populated with the resource server identifier only; it cannot
        /// be correlated back to a specific operator-managed resource.
        /// </summary>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3RoleConf? FromApi(GetRoleResponseContent? source, IEnumerable<PermissionsResponsePayload>? permissions) => source is null ? null : new()
        {
            Name = source.Name,
            Description = source.Description,
            Permissions = permissions?.Select(FromApi).ToArray(),
        };

        /// <summary>
        /// Transforms an Auth0 Management API permission payload to the operator's role permission model.
        /// </summary>
        internal static V2alpha3RolePermission FromApi(PermissionsResponsePayload source) => new()
        {
            Name = source.PermissionName,
            ResourceServerRef = new V1ResourceServerReference() { Identifier = source.ResourceServerIdentifier },
        };

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to a <see cref="CreateRoleRequestContent"/>.
        /// </summary>
        internal static void ApplyToApi(V2alpha3RoleConf conf, CreateRoleRequestContent request)
        {
            if (conf.Name is not null)
                request.Name = conf.Name;

            if (conf.Description is not null)
                request.Description = conf.Description;
        }

        /// <summary>
        /// Applies the fields of <paramref name="conf"/> to an <see cref="UpdateRoleRequestContent"/>.
        /// </summary>
        internal static void ApplyToApi(V2alpha3RoleConf conf, UpdateRoleRequestContent request)
        {
            if (conf.Name is not null)
                request.Name = conf.Name;

            if (conf.Description is not null)
                request.Description = conf.Description;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3RoleController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V2alpha3RoleController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Role";

        /// <inheritdoc />
        protected override async Task<V2alpha3RoleConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            GetRoleResponseContent? self;

            try
            {
                self = await api.Roles.GetAsync(id, null, cancellationToken);
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (self is null)
                return null;

            // retrieve all associated permissions, paging through the full set
            var permissions = new List<PermissionsResponsePayload>();
            var pager = await api.Roles.Permissions.ListAsync(id, new ListRolePermissionsRequestParameters(), null, cancellationToken);
            await foreach (var permission in pager.WithCancellation(cancellationToken))
                permissions.Add(permission);

            return FromApi(self, permissions);
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3Role entity, V2alpha3Role.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            if (string.IsNullOrWhiteSpace(conf.Name))
                return null;

            var pager = await api.Roles.ListAsync(new ListRolesRequestParameters { NameFilter = conf.Name }, null, cancellationToken);
            await foreach (var role in pager.WithCancellation(cancellationToken))
                if (role.Name == conf.Name)
                    return role.Id;

            return null;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V2alpha3RoleConf conf)
        {
            if (string.IsNullOrWhiteSpace(conf.Name))
                return "missing a value for Name";

            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3RoleConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new CreateRoleRequestContent { Name = conf.Name ?? throw new InvalidOperationException("Name is required.") };
            ApplyToApi(conf, req);

            var self = await api.Roles.CreateAsync(req, null, cancellationToken);
            if (self is null || string.IsNullOrWhiteSpace(self.Id))
                throw new InvalidOperationException();

            await ApplyPermissions(api, self.Id, null, conf, defaultNamespace, cancellationToken);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3RoleConf? last, V2alpha3RoleConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new UpdateRoleRequestContent();
            ApplyToApi(conf, req);
            await api.Roles.UpdateAsync(id, req, null, cancellationToken);

            await ApplyPermissions(api, id, last, conf, defaultNamespace, cancellationToken);
        }

        /// <summary>
        /// Reconciles the permissions associated with the role so that they match <paramref name="conf"/>. When the configured
        /// permissions are null the associated permissions are left unmanaged. The <paramref name="last"/> configuration represents
        /// the permissions currently reported by the API and is used to determine which permissions to add or remove.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="last"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        async Task ApplyPermissions(IManagementApiClient api, string id, V2alpha3RoleConf? last, V2alpha3RoleConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // permissions are left unmanaged when omitted
            if (conf.Permissions is null)
                return;

            // resolve desired permissions to concrete (identifier, name) pairs
            var desired = new Dictionary<(string, string), PermissionRequestPayload>();
            foreach (var permission in conf.Permissions)
            {
                if (permission is null)
                    continue;

                if (string.IsNullOrWhiteSpace(permission.Name))
                    throw new InvalidOperationException($"{EntityTypeName} permission is missing a name.");

                var identifier = await ResolveResourceServerRefToIdentifier(api, permission.ResourceServerRef, defaultNamespace, cancellationToken);
                if (string.IsNullOrWhiteSpace(identifier))
                    throw new InvalidOperationException($"{EntityTypeName} permission '{permission.Name}' is missing a resource server reference.");

                desired[(identifier, permission.Name)] = new PermissionRequestPayload { ResourceServerIdentifier = identifier, PermissionName = permission.Name };
            }

            // existing permissions as reported by the API
            var existing = new HashSet<(string, string)>();
            if (last?.Permissions is { } lastPermissions)
                foreach (var permission in lastPermissions)
                    if (permission.ResourceServerRef?.Identifier is { } identifier && permission.Name is { } name)
                        existing.Add((identifier, name));

            // determine required additions and removals
            var toAdd = desired.Where(i => existing.Contains(i.Key) == false).Select(i => i.Value).ToList();
            var toRemove = existing.Where(i => desired.ContainsKey(i) == false).Select(i => new PermissionRequestPayload { ResourceServerIdentifier = i.Item1, PermissionName = i.Item2 }).ToList();

            if (toRemove.Count > 0)
                await api.Roles.Permissions.DeleteAsync(id, new DeleteRolePermissionsRequestContent { Permissions = toRemove }, null, cancellationToken);

            if (toAdd.Count > 0)
                await api.Roles.Permissions.AddAsync(id, new AddRolePermissionsRequestContent { Permissions = toAdd }, null, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.Roles.DeleteAsync(id, null, cancellationToken);
        }

    }

}
