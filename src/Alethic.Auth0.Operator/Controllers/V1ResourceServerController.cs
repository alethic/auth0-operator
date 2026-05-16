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
        internal static V1ResourceServerConf? FromApi(ResourceServer? source) => source is null ? null : new()
        {
            Id = source.Id,
            Identifier = source.Identifier,
            Name = source.Name,
            Scopes = source.Scopes?.Select(FromApi).ToList(),
            SigningAlgorithm = FromApi(source.SigningAlgorithm),
            SigningSecret = source.SigningSecret,
            TokenLifetime = source.TokenLifetime,
            TokenLifetimeForWeb = source.TokenLifetimeForWeb,
            AllowOfflineAccess = source.AllowOfflineAccess,
            SkipConsentForVerifiableFirstPartyClients = source.SkipConsentForVerifiableFirstPartyClients,
            VerificationLocation = source.VerificationLocation,
            TokenDialect = FromApi(source.TokenDialect),
            EnforcePolicies = source.EnforcePolicies,
            ConsentPolicy = FromApi(source.ConsentPolicy),
            AuthorizationDetails = source.AuthorizationDetails?.Select(FromApi).ToList(),
            TokenEncryption = FromApi(source.TokenEncryption),
            ProofOfPossession = FromApi(source.ProofOfPossession),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerScope? FromApi(ResourceServerScope? source) => source is null ? null : new()
        {
            Value = source.Value,
            Description = source.Description,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerSigningAlgorithm? FromApi(SigningAlgorithm? source) => source switch
        {
            SigningAlgorithm.HS256 => V1ResourceServerSigningAlgorithm.HS256,
            SigningAlgorithm.RS256 => V1ResourceServerSigningAlgorithm.RS256,
            SigningAlgorithm.PS256 => V1ResourceServerSigningAlgorithm.PS256,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenDialect? FromApi(TokenDialect? source) => source switch
        {
            TokenDialect.AccessToken => V1ResourceServerTokenDialect.AccessToken,
            TokenDialect.AccessTokenAuthZ => V1ResourceServerTokenDialect.AccessTokenAuthZ,
            TokenDialect.Rfc9068Profile => V1ResourceServerTokenDialect.Rfc9068Profile,
            TokenDialect.Rfc9068ProfileAuthz => V1ResourceServerTokenDialect.Rfc9068ProfileAuthz,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerConsentPolicy? FromApi(ConsentPolicy? source) => source switch
        {
            ConsentPolicy.TransactionalAuthorizationWithMfa => V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerAuthorizationDetail? FromApi(ResourceServerAuthorizationDetail? source) => source is null ? null : new()
        {
            Type = source.Type,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenEncryption? FromApi(TokenEncryption? source) => source is null ? null : new()
        {
            Format = FromApi(source.Format),
            EncryptionKey = FromApi(source.EncryptionKey),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenFormat? FromApi(TokenFormat source) => source switch
        {
            TokenFormat.CompactNestedJwe => V1ResourceServerTokenFormat.CompactNestedJwe,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerTokenEncryptionKey? FromApi(TokenEncryptionKey? source) => source is null ? null : new()
        {
            Name = source.Name,
            Algorithm = source.Algorithm,
            Kid = source.Kid,
            Pem = source.Pem,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ResourceServerProofOfPossession? FromApi(ProofOfPossession? source) => source is null ? null : new()
        {
            Required = source.Required,
            Mechanism = FromApi(source.Mechanism),
        };

        internal static V1ResourceServerMechanism? FromApi(Mechanism source) => source switch
        {
            Mechanism.Mtls => V1ResourceServerMechanism.Mtls,
            _ => throw new NotImplementedException(),
        };

        internal static SigningAlgorithm ToApi(V1ResourceServerSigningAlgorithm source) => source switch
        {
            V1ResourceServerSigningAlgorithm.HS256 => SigningAlgorithm.HS256,
            V1ResourceServerSigningAlgorithm.RS256 => SigningAlgorithm.RS256,
            V1ResourceServerSigningAlgorithm.PS256 => SigningAlgorithm.PS256,
            _ => throw new NotImplementedException(),
        };

        internal static TokenDialect ToApi(V1ResourceServerTokenDialect source) => source switch
        {
            V1ResourceServerTokenDialect.AccessToken => TokenDialect.AccessToken,
            V1ResourceServerTokenDialect.AccessTokenAuthZ => TokenDialect.AccessTokenAuthZ,
            V1ResourceServerTokenDialect.Rfc9068Profile => TokenDialect.Rfc9068Profile,
            V1ResourceServerTokenDialect.Rfc9068ProfileAuthz => TokenDialect.Rfc9068ProfileAuthz,
            _ => throw new NotImplementedException(),
        };

        internal static ConsentPolicy ToApi(V1ResourceServerConsentPolicy source) => source switch
        {
            V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa => ConsentPolicy.TransactionalAuthorizationWithMfa,
            _ => throw new NotImplementedException(),
        };

        internal static TokenFormat ToApi(V1ResourceServerTokenFormat source) => source switch
        {
            V1ResourceServerTokenFormat.CompactNestedJwe => TokenFormat.CompactNestedJwe,
            _ => throw new NotImplementedException(),
        };

        internal static Mechanism ToApi(V1ResourceServerMechanism source) => source switch
        {
            V1ResourceServerMechanism.Mtls => Mechanism.Mtls,
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerScope ToApi(V1ResourceServerScope source) => new()
        {
            Value = source.Value,
            Description = source.Description,
        };

        internal static ResourceServerAuthorizationDetail ToApi(V1ResourceServerAuthorizationDetail source) => new()
        {
            Type = source.Type,
        };

        internal static void ApplyToApi(V1ResourceServerConf conf, ResourceServerBase request)
        {
            if (conf.Name is not null)
                request.Name = conf.Name;

            if (conf.Scopes is not null)
                request.Scopes = conf.Scopes.Select(ToApi).ToList();

            if (conf.SigningAlgorithm is { } signing_alg)
                request.SigningAlgorithm = ToApi(signing_alg);

            if (conf.SigningSecret is not null)
                request.SigningSecret = conf.SigningSecret;

            if (conf.TokenLifetime is not null)
                request.TokenLifetime = conf.TokenLifetime;

            if (conf.TokenLifetimeForWeb is not null)
                request.TokenLifetimeForWeb = conf.TokenLifetimeForWeb;

            if (conf.AllowOfflineAccess is not null)
                request.AllowOfflineAccess = conf.AllowOfflineAccess;

            if (conf.SkipConsentForVerifiableFirstPartyClients is not null)
                request.SkipConsentForVerifiableFirstPartyClients = conf.SkipConsentForVerifiableFirstPartyClients;

            if (conf.VerificationLocation is not null)
                request.VerificationLocation = conf.VerificationLocation;

            if (conf.TokenDialect is { } token_dialect)
                request.TokenDialect = ToApi(token_dialect);

            if (conf.EnforcePolicies is not null)
                request.EnforcePolicies = conf.EnforcePolicies;

            if (conf.ConsentPolicy is { } consent_policy)
                request.ConsentPolicy = ToApi(consent_policy);

            if (conf.AuthorizationDetails is not null)
                request.AuthorizationDetails = conf.AuthorizationDetails.Select(ToApi).ToList<ResourceServerAuthorizationDetail>();

            if (conf.TokenEncryption is { } token_encryption)
            {
                request.TokenEncryption = new TokenEncryption
                {
                    Format = ToApi(token_encryption.Format ?? default),
                    EncryptionKey = token_encryption.EncryptionKey is { } key ? new TokenEncryptionKey
                    {
                        Name = key.Name,
                        Algorithm = key.Algorithm,
                        Kid = key.Kid,
                        Pem = key.Pem,
                    } : null,
                };
            }

            if (conf.ProofOfPossession is { } pop)
            {
                request.ProofOfPossession = new ProofOfPossession
                {
                    Required = pop.Required,
                    Mechanism = pop.Mechanism is { } mech ? ToApi(mech) : default,
                };
            }
        }

        internal static void ApplyToApi(V1ResourceServerConf conf, ResourceServerCreateRequest request)
        {
            if (conf.Identifier is not null)
                request.Identifier = conf.Identifier;

            ApplyToApi(conf, (ResourceServerBase)request);
        }

        internal static void ApplyToApi(V1ResourceServerConf conf, ResourceServerUpdateRequest request)
        {
            ApplyToApi(conf, (ResourceServerBase)request);
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
                return FromApi(await api.ResourceServers.GetAsync(id, cancellationToken: cancellationToken));
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

            var list = await api.ResourceServers.GetAllAsync(new ResourceServerGetRequest() { }, cancellationToken: cancellationToken);
            var self = list.FirstOrDefault(i => i.Identifier == conf.Identifier);
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
            var req = new ResourceServerCreateRequest();
            ApplyToApi(conf, req);
            var self = await api.ResourceServers.CreateAsync(req, cancellationToken);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ResourceServerConf? last, V1ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new ResourceServerUpdateRequest();
            ApplyToApi(conf, req);
            await api.ResourceServers.UpdateAsync(id, req, cancellationToken);
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
            return api.ResourceServers.DeleteAsync(id, cancellationToken);
        }

    }

}
