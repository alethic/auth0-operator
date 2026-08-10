using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

using Auth0.ManagementApi;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V2alpha3ResourceServer), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3ResourceServerController :
        V1TenantEntityInstanceController<V2alpha3ResourceServer, V2alpha3ResourceServer.SpecDef, V2alpha3ResourceServer.StatusDef, V2alpha3ResourceServerConf, V2alpha3ResourceServerConf>,
        IEntityController<V2alpha3ResourceServer>
    {

        /// <summary>
        /// Transforms the Auth0 Management API resource server model to the operator's resource server configuration model.
        /// </summary>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerConf? FromApi(GetResourceServerResponseContent? source) => source is null ? null : new()
        {
            Id = source.Id,
            Identifier = source.Identifier,
            Name = source.Name,
            Scopes = source.Scopes?.Select(i => FromApi(i)).ToArray(),
            SigningAlgorithm = FromApi(source.SigningAlg),
            SigningSecret = source.SigningSecret,
            TokenLifetime = source.TokenLifetime,
            TokenLifetimeForWeb = source.TokenLifetimeForWeb,
            AllowOfflineAccess = source.AllowOfflineAccess,
            AllowOnlineAccess = source.AllowOnlineAccess,
            AllowOnlineAccessWithEphemeralSessions = source.AllowOnlineAccessWithEphemeralSessions,
            SkipConsentForVerifiableFirstPartyClients = source.SkipConsentForVerifiableFirstPartyClients,
            TokenDialect = source.TokenDialect is { } td ? FromApi(td) : null,
            EnforcePolicies = source.EnforcePolicies,
            ConsentPolicy = source.ConsentPolicy.IsDefined && source.ConsentPolicy.Value is { } cp ? FromApi(cp) : null,
            AuthorizationDetails = source.AuthorizationDetails.IsDefined && source.AuthorizationDetails.Value is { } ad ? ad.Select(FromApiAuthorizationDetail).ToArray() : null,
            AuthorizationPolicy = source.AuthorizationPolicy.IsDefined && source.AuthorizationPolicy.Value is { } ap ? FromApi(ap) : null,
            SubjectTypeAuthorization = FromApi(source.SubjectTypeAuthorization),
            TokenEncryption = source.TokenEncryption.IsDefined && source.TokenEncryption.Value is { } te ? FromApi(te) : null,
            ProofOfPossession = source.ProofOfPossession.IsDefined && source.ProofOfPossession.Value is { } pop ? FromApi(pop) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerConf? FromApi(ResourceServer? source) => source is null ? null : new()
        {
            Id = source.Id,
            Identifier = source.Identifier,
            Name = source.Name,
            Scopes = source.Scopes?.Select(i => FromApi(i)).ToArray(),
            SigningAlgorithm = FromApi(source.SigningAlg),
            SigningSecret = source.SigningSecret,
            TokenLifetime = source.TokenLifetime,
            TokenLifetimeForWeb = source.TokenLifetimeForWeb,
            AllowOfflineAccess = source.AllowOfflineAccess,
            AllowOnlineAccess = source.AllowOnlineAccess,
            AllowOnlineAccessWithEphemeralSessions = source.AllowOnlineAccessWithEphemeralSessions,
            SkipConsentForVerifiableFirstPartyClients = source.SkipConsentForVerifiableFirstPartyClients,
            TokenDialect = source.TokenDialect is { } td ? FromApi(td) : null,
            EnforcePolicies = source.EnforcePolicies,
            ConsentPolicy = source.ConsentPolicy.IsDefined && source.ConsentPolicy.Value is { } cp ? FromApi(cp) : null,
            AuthorizationPolicy = source.AuthorizationPolicy.IsDefined && source.AuthorizationPolicy.Value is { } ap ? FromApi(ap) : null,
            SubjectTypeAuthorization = FromApi(source.SubjectTypeAuthorization),
            TokenEncryption = source.TokenEncryption.IsDefined && source.TokenEncryption.Value is { } te ? FromApi(te) : null,
            ProofOfPossession = source.ProofOfPossession.IsDefined && source.ProofOfPossession.Value is { } pop ? FromApi(pop) : null,
        };

        /// <summary>
        /// The SDK surfaces authorization details as untyped objects; extract the modeled property from the
        /// element's JSON form.
        /// </summary>
        internal static V2alpha3ResourceServerAuthorizationDetail FromApiAuthorizationDetail(object? source) => new()
        {
            Type = source is JsonElement element && element.ValueKind == JsonValueKind.Object && element.TryGetProperty("type", out var type) ? type.GetString() : null,
        };

        /// <summary>
        /// Converts an authorization detail to the untyped object shape the SDK request expects.
        /// </summary>
        internal static object ToApiAuthorizationDetail(V2alpha3ResourceServerAuthorizationDetail source) => new Dictionary<string, object?>()
        {
            ["type"] = source.Type,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerAuthorizationPolicy? FromApi(ResourceServerAuthorizationPolicy? source) => source is null ? null : new()
        {
            PolicyId = source.PolicyId,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerSubjectTypeAuthorization? FromApi(ResourceServerSubjectTypeAuthorization? source) => source is null ? null : new()
        {
            Client = FromApi(source.Client),
            User = FromApi(source.User),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerSubjectTypeAuthorizationClient? FromApi(ResourceServerSubjectTypeAuthorizationClient? source) => source is null ? null : new()
        {
            Policy = source.Policy is { } policy ? FromApi(policy) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerSubjectTypeAuthorizationUser? FromApi(ResourceServerSubjectTypeAuthorizationUser? source) => source is null ? null : new()
        {
            Policy = source.Policy is { } policy ? FromApi(policy) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerScope? FromApi(ResourceServerScope? source) => source is null ? null : new()
        {
            Value = source.Value,
            Description = source.Description,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerSigningAlgorithm? FromApi(SigningAlgorithmEnum? source) => source?.Value switch
        {
            SigningAlgorithmEnum.Values.Hs256 => V2alpha3ResourceServerSigningAlgorithm.HS256,
            SigningAlgorithmEnum.Values.Rs256 => V2alpha3ResourceServerSigningAlgorithm.RS256,
            SigningAlgorithmEnum.Values.Ps256 => V2alpha3ResourceServerSigningAlgorithm.PS256,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerTokenDialect? FromApi(ResourceServerTokenDialectResponseEnum source) => source.Value switch
        {
            ResourceServerTokenDialectResponseEnum.Values.AccessToken => V2alpha3ResourceServerTokenDialect.AccessToken,
            ResourceServerTokenDialectResponseEnum.Values.AccessTokenAuthz => V2alpha3ResourceServerTokenDialect.AccessTokenAuthZ,
            ResourceServerTokenDialectResponseEnum.Values.Rfc9068Profile => V2alpha3ResourceServerTokenDialect.Rfc9068Profile,
            ResourceServerTokenDialectResponseEnum.Values.Rfc9068ProfileAuthz => V2alpha3ResourceServerTokenDialect.Rfc9068ProfileAuthz,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerConsentPolicy? FromApi(ResourceServerConsentPolicyEnum source) => source.Value switch
        {
            ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa => V2alpha3ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerTokenEncryption? FromApi(ResourceServerTokenEncryption? source) => source is null ? null : new()
        {
            Format = FromApi(source.Format),
            EncryptionKey = FromApi(source.EncryptionKey),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerTokenFormat? FromApi(ResourceServerTokenEncryptionFormatEnum source) => source.Value switch
        {
            ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe => V2alpha3ResourceServerTokenFormat.CompactNestedJwe,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerTokenEncryptionKey? FromApi(ResourceServerTokenEncryptionKey? source) => source is null ? null : new()
        {
            Name = source.Name,
            Algorithm = source.Alg?.Value,
            Kid = source.Kid,
            Pem = source.Pem,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ResourceServerProofOfPossession? FromApi(ResourceServerProofOfPossession? source) => source is null ? null : new()
        {
            Required = source.Required,
            Mechanism = FromApi(source.Mechanism),
        };

        internal static V2alpha3ResourceServerMechanism? FromApi(ResourceServerProofOfPossessionMechanismEnum source) => source.Value switch
        {
            ResourceServerProofOfPossessionMechanismEnum.Values.Dpop => V2alpha3ResourceServerMechanism.Dpop,
            ResourceServerProofOfPossessionMechanismEnum.Values.Mtls => V2alpha3ResourceServerMechanism.Mtls,
            _ => throw new NotImplementedException(),
        };

        internal static V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy? FromApi(ResourceServerSubjectTypeAuthorizationClientPolicyEnum source) => source.Value switch
        {
            ResourceServerSubjectTypeAuthorizationClientPolicyEnum.Values.DenyAll => V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy.DenyAll,
            ResourceServerSubjectTypeAuthorizationClientPolicyEnum.Values.RequireClientGrant => V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy.RequireClientGrant,
            _ => throw new NotImplementedException(),
        };

        internal static V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy? FromApi(ResourceServerSubjectTypeAuthorizationUserPolicyEnum source) => source.Value switch
        {
            ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.AllowAll => V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.AllowAll,
            ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.DenyAll => V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.DenyAll,
            ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.RequireClientGrant => V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.RequireClientGrant,
            _ => throw new NotImplementedException(),
        };

        internal static SigningAlgorithmEnum ToApi(V2alpha3ResourceServerSigningAlgorithm source) => source switch
        {
            V2alpha3ResourceServerSigningAlgorithm.HS256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Hs256),
            V2alpha3ResourceServerSigningAlgorithm.RS256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256),
            V2alpha3ResourceServerSigningAlgorithm.PS256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Ps256),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerTokenDialectSchemaEnum ToApi(V2alpha3ResourceServerTokenDialect source) => source switch
        {
            V2alpha3ResourceServerTokenDialect.AccessToken => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.AccessToken),
            V2alpha3ResourceServerTokenDialect.AccessTokenAuthZ => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.AccessTokenAuthz),
            V2alpha3ResourceServerTokenDialect.Rfc9068Profile => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.Rfc9068Profile),
            V2alpha3ResourceServerTokenDialect.Rfc9068ProfileAuthz => new ResourceServerTokenDialectSchemaEnum(ResourceServerTokenDialectSchemaEnum.Values.Rfc9068ProfileAuthz),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerConsentPolicyEnum ToApi(V2alpha3ResourceServerConsentPolicy source) => source switch
        {
            V2alpha3ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa => new ResourceServerConsentPolicyEnum(ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerAuthorizationPolicy ToApi(V2alpha3ResourceServerAuthorizationPolicy source) => new()
        {
            PolicyId = source.PolicyId ?? throw new InvalidOperationException("AuthorizationPolicy is missing a policy ID."),
        };

        internal static ResourceServerSubjectTypeAuthorization ToApi(V2alpha3ResourceServerSubjectTypeAuthorization source) => new()
        {
            Client = source.Client is { } client ? ToApi(client) : null,
            User = source.User is { } user ? ToApi(user) : null,
        };

        internal static ResourceServerSubjectTypeAuthorizationClient ToApi(V2alpha3ResourceServerSubjectTypeAuthorizationClient source) => new()
        {
            Policy = source.Policy is { } policy ? ToApi(policy) : null,
        };

        internal static ResourceServerSubjectTypeAuthorizationUser ToApi(V2alpha3ResourceServerSubjectTypeAuthorizationUser source) => new()
        {
            Policy = source.Policy is { } policy ? ToApi(policy) : null,
        };

        internal static ResourceServerTokenEncryptionFormatEnum ToApi(V2alpha3ResourceServerTokenFormat source) => source switch
        {
            V2alpha3ResourceServerTokenFormat.CompactNestedJwe => new ResourceServerTokenEncryptionFormatEnum(ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerTokenEncryption ToApi(V2alpha3ResourceServerTokenEncryption source) => new()
        {
            Format = ToApi(source.Format ?? default),
            EncryptionKey = source.EncryptionKey is { } key ? ToApi(key) : throw new InvalidOperationException("TokenEncryption is missing an encryption key."),
        };

        internal static ResourceServerTokenEncryptionKey ToApi(V2alpha3ResourceServerTokenEncryptionKey source) => new()
        {
            Name = source.Name,
            Kid = source.Kid,
            Pem = source.Pem,
        };

        internal static ResourceServerProofOfPossessionMechanismEnum ToApi(V2alpha3ResourceServerMechanism source) => source switch
        {
            V2alpha3ResourceServerMechanism.Dpop => new ResourceServerProofOfPossessionMechanismEnum(ResourceServerProofOfPossessionMechanismEnum.Values.Dpop),
            V2alpha3ResourceServerMechanism.Mtls => new ResourceServerProofOfPossessionMechanismEnum(ResourceServerProofOfPossessionMechanismEnum.Values.Mtls),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerSubjectTypeAuthorizationClientPolicyEnum ToApi(V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy source) => source switch
        {
            V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy.DenyAll => new ResourceServerSubjectTypeAuthorizationClientPolicyEnum(ResourceServerSubjectTypeAuthorizationClientPolicyEnum.Values.DenyAll),
            V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy.RequireClientGrant => new ResourceServerSubjectTypeAuthorizationClientPolicyEnum(ResourceServerSubjectTypeAuthorizationClientPolicyEnum.Values.RequireClientGrant),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerSubjectTypeAuthorizationUserPolicyEnum ToApi(V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy source) => source switch
        {
            V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.AllowAll => new ResourceServerSubjectTypeAuthorizationUserPolicyEnum(ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.AllowAll),
            V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.DenyAll => new ResourceServerSubjectTypeAuthorizationUserPolicyEnum(ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.DenyAll),
            V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.RequireClientGrant => new ResourceServerSubjectTypeAuthorizationUserPolicyEnum(ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.RequireClientGrant),
            _ => throw new NotImplementedException(),
        };

        internal static ResourceServerProofOfPossession ToApi(V2alpha3ResourceServerProofOfPossession source) => new()
        {
            Required = source.Required ?? false,
            Mechanism = source.Mechanism is { } mech ? ToApi(mech) : default!,
        };

        internal static ResourceServerScope ToApi(V2alpha3ResourceServerScope source) => new()
        {
            Value = source.Value ?? throw new InvalidOperationException("Scope is missing a value."),
            Description = source.Description,
        };

        internal static void ApplyToApi(V2alpha3ResourceServerConf conf, CreateResourceServerRequestContent request)
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

            if (conf.AllowOnlineAccess is not null)
                request.AllowOnlineAccess = conf.AllowOnlineAccess;

            if (conf.AllowOnlineAccessWithEphemeralSessions is not null)
                request.AllowOnlineAccessWithEphemeralSessions = conf.AllowOnlineAccessWithEphemeralSessions;

            if (conf.SkipConsentForVerifiableFirstPartyClients is not null)
                request.SkipConsentForVerifiableFirstPartyClients = conf.SkipConsentForVerifiableFirstPartyClients;

            if (conf.TokenDialect is { } tokenDialect)
                request.TokenDialect = ToApi(tokenDialect);

            if (conf.EnforcePolicies is not null)
                request.EnforcePolicies = conf.EnforcePolicies;

            if (conf.ConsentPolicy is { } consentPolicy)
                request.ConsentPolicy = ToApi(consentPolicy);

            if (conf.AuthorizationDetails is { } authorizationDetails)
                request.AuthorizationDetails = authorizationDetails.Select(ToApiAuthorizationDetail).ToList();

            if (conf.AuthorizationPolicy is { } authorizationPolicy)
                request.AuthorizationPolicy = ToApi(authorizationPolicy);

            if (conf.SubjectTypeAuthorization is { } subjectTypeAuthorization)
                request.SubjectTypeAuthorization = ToApi(subjectTypeAuthorization);

            if (conf.TokenEncryption is { } tokenEncryption)
                request.TokenEncryption = ToApi(tokenEncryption);

            if (conf.ProofOfPossession is { } pop)
                request.ProofOfPossession = ToApi(pop);
        }

        internal static void ApplyToApi(V2alpha3ResourceServerConf conf, UpdateResourceServerRequestContent request)
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

            if (conf.AllowOnlineAccess is not null)
                request.AllowOnlineAccess = conf.AllowOnlineAccess;

            if (conf.AllowOnlineAccessWithEphemeralSessions is not null)
                request.AllowOnlineAccessWithEphemeralSessions = conf.AllowOnlineAccessWithEphemeralSessions;

            if (conf.SkipConsentForVerifiableFirstPartyClients is not null)
                request.SkipConsentForVerifiableFirstPartyClients = conf.SkipConsentForVerifiableFirstPartyClients;

            if (conf.TokenDialect is { } tokenDialect)
                request.TokenDialect = ToApi(tokenDialect);

            if (conf.EnforcePolicies is not null)
                request.EnforcePolicies = conf.EnforcePolicies;

            if (conf.ConsentPolicy is { } consentPolicy)
                request.ConsentPolicy = ToApi(consentPolicy);

            if (conf.AuthorizationDetails is { } authorizationDetails)
                request.AuthorizationDetails = authorizationDetails.Select(ToApiAuthorizationDetail).ToList();

            if (conf.AuthorizationPolicy is { } authorizationPolicy)
                request.AuthorizationPolicy = ToApi(authorizationPolicy);

            if (conf.SubjectTypeAuthorization is { } subjectTypeAuthorization)
                request.SubjectTypeAuthorization = ToApi(subjectTypeAuthorization);

            if (conf.TokenEncryption is { } tokenEncryption)
                request.TokenEncryption = ToApi(tokenEncryption);

            if (conf.ProofOfPossession is { } pop)
                request.ProofOfPossession = ToApi(pop);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3ResourceServerController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3ResourceServerController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "ResourceServer";

        /// <inheritdoc />
        protected override async Task<V2alpha3ResourceServerConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.ResourceServers.GetAsync(id, new GetResourceServerRequestParameters(), null, cancellationToken));
            }
            catch (NotFoundError)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3ResourceServer entity, V2alpha3ResourceServer.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            var conf = spec.Init ?? spec.Conf;
            if (conf is null)
                return null;

            var pager = await api.ResourceServers.ListAsync(new ListResourceServerRequestParameters(), null, cancellationToken);
            var self = pager.CurrentPage.Items?.FirstOrDefault(i => i.Identifier == conf.Identifier);
            return self?.Id;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V2alpha3ResourceServerConf conf)
        {
            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new CreateResourceServerRequestContent { Identifier = conf.Identifier ?? throw new InvalidOperationException("Identifier is required.") };
            ApplyToApi(conf, req);
            var self = await api.ResourceServers.CreateAsync(req, null, cancellationToken);
            return self.Id ?? throw new InvalidOperationException("Create response missing resource server ID.");
        }

        /// <summary>
        /// Determines whether the desired scope list differs from the scope list reported by Auth0. Scopes are a
        /// set keyed by value — ordering carries no meaning, so Auth0 returning them in a different order than the
        /// spec declares them is not drift.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerScope[]? conf, V2alpha3ResourceServerScope[]? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Length != last.Length)
                return true;

            foreach (var scope in conf)
            {
                var match = last.FirstOrDefault(i => i.Value == scope.Value);
                if (match is null)
                    return true;
                if (scope.Description is not null && scope.Description != match.Description)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the desired authorization details differ from those reported by Auth0. Details are a
        /// set keyed by type — ordering carries no meaning.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerAuthorizationDetail[]? conf, V2alpha3ResourceServerAuthorizationDetail[]? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Length != last.Length)
                return true;

            foreach (var detail in conf)
                if (last.Any(i => i.Type == detail.Type) == false)
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired authorization policy differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerAuthorizationPolicy? conf, V2alpha3ResourceServerAuthorizationPolicy? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.PolicyId is not null && conf.PolicyId != last.PolicyId)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired client subject type authorization differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerSubjectTypeAuthorizationClient? conf, V2alpha3ResourceServerSubjectTypeAuthorizationClient? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Policy is not null && conf.Policy != last.Policy)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired user subject type authorization differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerSubjectTypeAuthorizationUser? conf, V2alpha3ResourceServerSubjectTypeAuthorizationUser? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Policy is not null && conf.Policy != last.Policy)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired subject type authorization differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerSubjectTypeAuthorization? conf, V2alpha3ResourceServerSubjectTypeAuthorization? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (RequiresUpdate(conf.Client, last.Client))
                return true;
            if (RequiresUpdate(conf.User, last.User))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired token encryption key differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerTokenEncryptionKey? conf, V2alpha3ResourceServerTokenEncryptionKey? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Name is not null && conf.Name != last.Name)
                return true;
            if (conf.Algorithm is not null && conf.Algorithm != last.Algorithm)
                return true;
            if (conf.Kid is not null && conf.Kid != last.Kid)
                return true;
            if (conf.Pem is not null && conf.Pem != last.Pem)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired token encryption differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerTokenEncryption? conf, V2alpha3ResourceServerTokenEncryption? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Format is not null && conf.Format != last.Format)
                return true;
            if (RequiresUpdate(conf.EncryptionKey, last.EncryptionKey))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the desired proof of possession differs from the value reported by Auth0.
        /// </summary>
        internal static bool RequiresUpdate(V2alpha3ResourceServerProofOfPossession? conf, V2alpha3ResourceServerProofOfPossession? last)
        {
            if (conf is null)
                return false;
            if (last is null)
                return true;
            if (conf.Required is not null && conf.Required != last.Required)
                return true;
            if (conf.Mechanism is not null && conf.Mechanism != last.Mechanism)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the resource server differs from the desired configuration. Compares exactly the
        /// properties transmitted by <see cref="ApplyToApi(V2alpha3ResourceServerConf, UpdateResourceServerRequestContent)"/>;
        /// a property left null on the desired configuration is unmanaged and ignored. Excluded: id is read-only,
        /// identifier is create-only and immutable, tokenLifetimeForWeb is response-only in the SDK, and
        /// verificationLocation is not supported by the SDK at all; a skew in those can never be corrected by
        /// updating.
        /// </summary>
        internal static bool ConfRequiresUpdate(V2alpha3ResourceServerConf conf, V2alpha3ResourceServerConf last)
        {
            if (conf.Name is not null && conf.Name != last.Name)
                return true;
            if (RequiresUpdate(conf.Scopes, last.Scopes))
                return true;
            if (conf.SigningAlgorithm is not null && conf.SigningAlgorithm != last.SigningAlgorithm)
                return true;
            if (conf.SigningSecret is not null && conf.SigningSecret != last.SigningSecret)
                return true;
            if (conf.TokenLifetime is not null && conf.TokenLifetime != last.TokenLifetime)
                return true;
            if (conf.AllowOfflineAccess is not null && conf.AllowOfflineAccess != last.AllowOfflineAccess)
                return true;
            if (conf.AllowOnlineAccess is not null && conf.AllowOnlineAccess != last.AllowOnlineAccess)
                return true;
            if (conf.AllowOnlineAccessWithEphemeralSessions is not null && conf.AllowOnlineAccessWithEphemeralSessions != last.AllowOnlineAccessWithEphemeralSessions)
                return true;
            if (conf.SkipConsentForVerifiableFirstPartyClients is not null && conf.SkipConsentForVerifiableFirstPartyClients != last.SkipConsentForVerifiableFirstPartyClients)
                return true;
            if (conf.TokenDialect is not null && conf.TokenDialect != last.TokenDialect)
                return true;
            if (conf.EnforcePolicies is not null && conf.EnforcePolicies != last.EnforcePolicies)
                return true;
            if (conf.ConsentPolicy is not null && conf.ConsentPolicy != last.ConsentPolicy)
                return true;
            if (RequiresUpdate(conf.AuthorizationDetails, last.AuthorizationDetails))
                return true;
            if (RequiresUpdate(conf.AuthorizationPolicy, last.AuthorizationPolicy))
                return true;
            if (RequiresUpdate(conf.SubjectTypeAuthorization, last.SubjectTypeAuthorization))
                return true;
            if (RequiresUpdate(conf.TokenEncryption, last.TokenEncryption))
                return true;
            if (RequiresUpdate(conf.ProofOfPossession, last.ProofOfPossession))
                return true;

            return false;
        }

        /// <inheritdoc />
        protected override bool NeedsUpdate(V2alpha3ResourceServerConf conf, V2alpha3ResourceServerConf last)
        {
            return ConfRequiresUpdate(conf, last);
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3ResourceServerConf? last, V2alpha3ResourceServerConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            var req = new UpdateResourceServerRequestContent();
            ApplyToApi(conf, req);
            await api.ResourceServers.UpdateAsync(id, req, null, cancellationToken);
        }

        /// <inheritdoc />
        protected override Task ApplyStatus(IManagementApiClient api, V2alpha3ResourceServer entity, V2alpha3ResourceServerConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
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
