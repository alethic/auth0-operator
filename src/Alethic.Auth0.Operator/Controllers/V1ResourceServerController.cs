using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ResourceServer.V1;
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

    [EntityRbac(typeof(V1ResourceServer), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ResourceServerController :
        V1TenantEntityInstanceController<V1ResourceServer, V1ResourceServer.SpecDef, V1ResourceServer.StatusDef, V1ResourceServerConf, V1ResourceServerConf>,
        IEntityController<V1ResourceServer>
    {

        /// <summary>
        /// Transforms the Auth0 Management API resource server model to the operator's resource server configuration model.
        /// </summary>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerConf? FromApi(GetResourceServerResponseContent? source) => source is null ? null : new()
        {
            Id = source.Id,
            Identifier = source.Identifier,
            Name = source.Name,
            Scopes = source.Scopes?.Select(FromApi).ToList(),
            SigningAlgorithm = FromApi(source.SigningAlg),
            SigningSecret = source.SigningSecret,
            TokenLifetime = source.TokenLifetime,
            TokenLifetimeForWeb = source.TokenLifetimeForWeb,
            AllowOfflineAccess = source.AllowOfflineAccess,
            SkipConsentForVerifiableFirstPartyClients = source.SkipConsentForVerifiableFirstPartyClients,
            TokenDialect = source.TokenDialect is { } td ? FromApi(td) : null,
            EnforcePolicies = source.EnforcePolicies,
            ConsentPolicy = source.ConsentPolicy.IsDefined && source.ConsentPolicy.Value is { } cp ? FromApi(cp) : null,
            AuthorizationDetails = null,
            TokenEncryption = source.TokenEncryption.IsDefined && source.TokenEncryption.Value is { } te ? FromApi(te) : null,
            ProofOfPossession = source.ProofOfPossession.IsDefined && source.ProofOfPossession.Value is { } pop ? FromApi(pop) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerConf? FromApi(ResourceServer? source) => source is null ? null : new()
        {
            Id = source.Id,
            Identifier = source.Identifier,
            Name = source.Name,
            Scopes = source.Scopes?.Select(FromApi).ToList(),
            SigningAlgorithm = FromApi(source.SigningAlg),
            SigningSecret = source.SigningSecret,
            TokenLifetime = source.TokenLifetime,
            TokenLifetimeForWeb = source.TokenLifetimeForWeb,
            AllowOfflineAccess = source.AllowOfflineAccess,
            SkipConsentForVerifiableFirstPartyClients = source.SkipConsentForVerifiableFirstPartyClients,
            TokenDialect = source.TokenDialect is { } td ? FromApi(td) : null,
            EnforcePolicies = source.EnforcePolicies,
            ConsentPolicy = source.ConsentPolicy.IsDefined && source.ConsentPolicy.Value is { } cp ? FromApi(cp) : null,
            TokenEncryption = source.TokenEncryption.IsDefined && source.TokenEncryption.Value is { } te ? FromApi(te) : null,
            ProofOfPossession = source.ProofOfPossession.IsDefined && source.ProofOfPossession.Value is { } pop ? FromApi(pop) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerScope? FromApi(ResourceServerScope? source) => source is null ? null : new()
        {
            Value = source.Value,
            Description = source.Description,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerSigningAlgorithm? FromApi(SigningAlgorithmEnum? source) => source?.Value switch
        {
            SigningAlgorithmEnum.Values.Hs256 => V1ResourceServerSigningAlgorithm.HS256,
            SigningAlgorithmEnum.Values.Rs256 => V1ResourceServerSigningAlgorithm.RS256,
            SigningAlgorithmEnum.Values.Ps256 => V1ResourceServerSigningAlgorithm.PS256,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenDialect? FromApi(ResourceServerTokenDialectResponseEnum source) => source.Value switch
        {
            ResourceServerTokenDialectResponseEnum.Values.AccessToken => V1ResourceServerTokenDialect.AccessToken,
            ResourceServerTokenDialectResponseEnum.Values.AccessTokenAuthz => V1ResourceServerTokenDialect.AccessTokenAuthZ,
            ResourceServerTokenDialectResponseEnum.Values.Rfc9068Profile => V1ResourceServerTokenDialect.Rfc9068Profile,
            ResourceServerTokenDialectResponseEnum.Values.Rfc9068ProfileAuthz => V1ResourceServerTokenDialect.Rfc9068ProfileAuthz,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerConsentPolicy? FromApi(ResourceServerConsentPolicyEnum source) => source.Value switch
        {
            ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa => V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenEncryption? FromApi(ResourceServerTokenEncryption? source) => source is null ? null : new()
        {
            Format = FromApi(source.Format),
            EncryptionKey = FromApi(source.EncryptionKey),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenFormat? FromApi(ResourceServerTokenEncryptionFormatEnum source) => source.Value switch
        {
            ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe => V1ResourceServerTokenFormat.CompactNestedJwe,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenEncryptionKey? FromApi(ResourceServerTokenEncryptionKey? source) => source is null ? null : new()
        {
            Name = source.Name,
            Algorithm = source.Alg?.Value,
            Kid = source.Kid,
            Pem = source.Pem,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerProofOfPossession? FromApi(ResourceServerProofOfPossession? source) => source is null ? null : new()
        {
            Required = source.Required,
            Mechanism = FromApi(source.Mechanism),
        };

        internal static V1ResourceServerMechanism? FromApi(ResourceServerProofOfPossessionMechanismEnum source) => source.Value switch
        {
            ResourceServerProofOfPossessionMechanismEnum.Values.Mtls => V1ResourceServerMechanism.Mtls,
            _ => throw new NotImplementedException(),
        };

        internal static SigningAlgorithmEnum ToApi(V1ResourceServerSigningAlgorithm source) => source switch
        {
            V1ResourceServerSigningAlgorithm.HS256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Hs256),
            V1ResourceServerSigningAlgorithm.RS256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256),
            V1ResourceServerSigningAlgorithm.PS256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Ps256),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerTokenDialectSchemaEnum ToApi(V1ResourceServerTokenDialect source) => source switch
        {
            V1ResourceServerTokenDialect.AccessToken => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.AccessToken),
            V1ResourceServerTokenDialect.AccessTokenAuthZ => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.AccessTokenAuthz),
            V1ResourceServerTokenDialect.Rfc9068Profile => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.Rfc9068Profile),
            V1ResourceServerTokenDialect.Rfc9068ProfileAuthz => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.Rfc9068ProfileAuthz),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerConsentPolicyEnum ToApi(V1ResourceServerConsentPolicy source) => source switch
        {
            V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa => new ResourceServerConsentPolicyEnum(ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerTokenEncryptionFormatEnum ToApi(V1ResourceServerTokenFormat source) => source switch
        {
            V1ResourceServerTokenFormat.CompactNestedJwe => new ResourceServerTokenEncryptionFormatEnum(ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerProofOfPossessionMechanismEnum ToApi(V1ResourceServerMechanism source) => source switch
        {
            V1ResourceServerMechanism.Mtls => new ResourceServerProofOfPossessionMechanismEnum(ResourceServerProofOfPossessionMechanismEnum.Values.Mtls),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerScope ToApi(V1ResourceServerScope source) => new()
        {
            Value = source.Value,
            Description = source.Description,
        };

        internal static void ApplyToApi(V1ResourceServerConf conf, CreateResourceServerRequestContent request)
        {
            if (conf.Identifier is not null)
                request.Identifier = conf.Identifier;

            if (conf.Name is not null)
                request.Name = conf.Name;

            if (conf.Scopes is not null)
                request.Scopes = conf.Scopes.Select(ToApi).ToList();

            if (conf.SigningAlgorithm is { } signingAlg)
                request.SigningAlg = ToApi(signingAlg);

            if (conf.SigningSecret is not null)
                request.SigningSecret = conf.SigningSecret;

            if (conf.TokenLifetime is not null)
                request.TokenLifetime = conf.TokenLifetime;

            if (conf.AllowOfflineAccess is not null)
                request.AllowOfflineAccess = conf.AllowOfflineAccess;

            if (conf.SkipConsentForVerifiableFirstPartyClients is not null)
                request.SkipConsentForVerifiableFirstPartyClients = conf.SkipConsentForVerifiableFirstPartyClients;

            if (conf.TokenDialect is { } tokenDialect)
                request.TokenDialect = ToApi(tokenDialect);

            if (conf.EnforcePolicies is not null)
                request.EnforcePolicies = conf.EnforcePolicies;

            if (conf.ConsentPolicy is { } consentPolicy)
                request.ConsentPolicy = Optional<ResourceServerConsentPolicyEnum?>.Of(ToApi(consentPolicy));

            if (conf.TokenEncryption is { } tokenEncryption)
                request.TokenEncryption = Optional<ResourceServerTokenEncryption?>.Of(new ResourceServerTokenEncryption
                {
                    Format = ToApi(tokenEncryption.Format ?? default),
                    EncryptionKey = tokenEncryption.EncryptionKey is { } key ? new ResourceServerTokenEncryptionKey
                    {
                        Name = key.Name,
                        Kid = key.Kid,
                        Pem = key.Pem,
                    } : null,
                });

            if (conf.ProofOfPossession is { } pop)
                request.ProofOfPossession = Optional<ResourceServerProofOfPossession?>.Of(new ResourceServerProofOfPossession
                {
                    Required = pop.Required ?? false,
                    Mechanism = pop.Mechanism is { } mech ? ToApi(mech) : default!,
                });
        }

        internal static void ApplyToApi(V1ResourceServerConf conf, UpdateResourceServerRequestContent request)
        {
            if (conf.Name is not null)
                request.Name = conf.Name;

            if (conf.Scopes is not null)
                request.Scopes = conf.Scopes.Select(ToApi).ToList();

            if (conf.SigningAlgorithm is { } signingAlg)
                request.SigningAlg = ToApi(signingAlg);

            if (conf.SigningSecret is not null)
                request.SigningSecret = conf.SigningSecret;

            if (conf.TokenLifetime is not null)
                request.TokenLifetime = conf.TokenLifetime;

            if (conf.AllowOfflineAccess is not null)
                request.AllowOfflineAccess = conf.AllowOfflineAccess;

            if (conf.SkipConsentForVerifiableFirstPartyClients is not null)
                request.SkipConsentForVerifiableFirstPartyClients = conf.SkipConsentForVerifiableFirstPartyClients;

            if (conf.TokenDialect is { } tokenDialect)
                request.TokenDialect = ToApi(tokenDialect);

            if (conf.EnforcePolicies is not null)
                request.EnforcePolicies = conf.EnforcePolicies;

            if (conf.ConsentPolicy is { } consentPolicy)
                request.ConsentPolicy = Optional<ResourceServerConsentPolicyEnum?>.Of(ToApi(consentPolicy));

            if (conf.TokenEncryption is { } tokenEncryption)
                request.TokenEncryption = Optional<ResourceServerTokenEncryption?>.Of(new ResourceServerTokenEncryption
                {
                    Format = ToApi(tokenEncryption.Format ?? default),
                    EncryptionKey = tokenEncryption.EncryptionKey is { } key ? new ResourceServerTokenEncryptionKey
                    {
                        Name = key.Name,
                        Kid = key.Kid,
                        Pem = key.Pem,
                    } : null,
                });

            if (conf.ProofOfPossession is { } pop)
                request.ProofOfPossession = Optional<ResourceServerProofOfPossession?>.Of(new ResourceServerProofOfPossession
                {
                    Required = pop.Required ?? false,
                    Mechanism = pop.Mechanism is { } mech ? ToApi(mech) : default!,
                });
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ResourceServerController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ResourceServerController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "ResourceServer";

        /// <inheritdoc />
        protected override async Task<V1ResourceServerConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.ResourceServers.GetAsync(id, new GetResourceServerRequestParameters(), null, cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1ResourceServer entity, V1ResourceServer.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            var pager = await api.ResourceServers.ListAsync(new ListResourceServerRequestParameters(), null, cancellationToken);
            var self = pager.CurrentPage.Items?.FirstOrDefault(i => i.Identifier == conf.Identifier);
            return self?.Id;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1ResourceServerConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new CreateResourceServerRequestContent { Identifier = conf.Identifier ?? throw new InvalidOperationException("Identifier is required.") };
            ApplyToApi(conf, req);
            var self = await api.ResourceServers.CreateAsync(req, null, cancellationToken);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ResourceServerConf? last, V1ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new UpdateResourceServerRequestContent();
            ApplyToApi(conf, req);
            await api.ResourceServers.UpdateAsync(id, req, null, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task ApplyStatus(IManagementApiClient api, V1ResourceServer entity, V1ResourceServerConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(lastConf.Identifier))
                throw new InvalidOperationException($"{EntityTypeName} {entity.Namespace()}/{entity.Name()} has missing Identifier.");

            entity.Status.Identifier = lastConf.Identifier;
            return base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            return api.ResourceServers.DeleteAsync(id, null, cancellationToken);
        }

    }

}
