using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Client;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Client), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ClientController :
        V1TenantEntityInstanceController<V1Client, V1Client.SpecDef, V1Client.StatusDef, V1ClientConf, Hashtable>,
        IEntityController<V1Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ClientController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ClientController> logger) :
            base(kube, cache, options, logger)
        {

        }

        void ApplyToApi(ClientAddons source, Addons target)
        {
            if (source.AmazonWebServices is { } aws)
                target.AmazonWebServices = aws;

            if (source.AzureMobileServices is { } wams)
                target.AzureMobileServices = wams;

            if (source.AzureServiceBus is { } azure_sb)
                target.AzureServiceBus = azure_sb;

            if (source.Box is { } box)
                target.Box = box;

            if (source.CloudBees is { } cloudbees)
                target.CloudBees = cloudbees;

            if (source.Concur is { } concur)
                target.Concur = concur;

            if (source.DropBox is { } dropbox)
                target.DropBox = dropbox;

            if (source.EchoSign is { } echosign)
                target.EchoSign = echosign;

            if (source.Egnyte is { } egnyte)
                target.Egnyte = egnyte;

            if (source.FireBase is { } firebase)
                target.FireBase = firebase;

            if (source.NewRelic is { } newrelic)
                target.NewRelic = newrelic;

            if (source.Office365 is { } office365)
                target.Office365 = office365;

            if (source.SalesForce is { } salesforce)
                target.SalesForce = salesforce;

            if (source.SalesForceApi is { } salesforce_api)
                target.SalesForceApi = salesforce_api;

            if (source.SalesForceSandboxApi is { } salesforce_sandbox_api)
                target.SalesForceSandboxApi = salesforce_sandbox_api;

            if (source.SamlP is { } samlp)
                target.SamlP = samlp;

            if (source.SapApi is { } sap_api)
                target.SapApi = sap_api;

            if (source.SharePoint is { } sharepoint)
                target.SharePoint = sharepoint;

            if (source.SpringCM is { } springcm)
                target.SpringCM = springcm;

            if (source.WebApi is { } webapi)
                target.WebApi = webapi;

            if (source.WsFed is { } wsfed)
                target.WsFed = wsfed;

            if (source.Zendesk is { } zendesk)
                target.Zendesk = zendesk;

            if (source.Zoom is { } zoom)
                target.Zoom = zoom;
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.EncryptionKey source, EncryptionKey target)
        {
            if (source.Certificate is { } cert)
                target.Certificate = cert;

            if (source.PublicKey is { } pub)
                target.PublicKey = pub;

            if (source.Subject is { } subject)
                target.Subject = subject;
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.ScopeEntry source, ScopeEntry target)
        {
            if (source.Actions is { } actions)
                target.Actions = actions;
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.Scopes source, Scopes target)
        {
            if (source.Users is { } users)
                ApplyToApi(users, target.Users ??= new ScopeEntry());

            if (source.UsersAppMetadata is { } users_app_metadata)
                ApplyToApi(users_app_metadata, target.UsersAppMetadata ??= new ScopeEntry());

            if (source.Clients is { } clients)
                ApplyToApi(clients, target.Clients ??= new ScopeEntry());

            if (source.ClientKeys is { } client_keys)
                ApplyToApi(client_keys, target.ClientKeys ??= new ScopeEntry());

            if (source.Tokens is { } tokens)
                ApplyToApi(tokens, target.Tokens ??= new ScopeEntry());

            if (source.Stats is { } stats)
                ApplyToApi(stats, target.Stats ??= new ScopeEntry());
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.JwtConfiguration source, JwtConfiguration target)
        {
            if (source.IsSecretEncoded is { } secret_encoded)
                target.IsSecretEncoded = secret_encoded;

            if (source.LifetimeInSeconds is { } lifetime_in_seconds)
                target.LifetimeInSeconds = lifetime_in_seconds;

            if (source.Scopes is { } scopes)
                ApplyToApi(scopes, target.Scopes ??= new Scopes());

            if (source.SigningAlgorithm is { } alg)
                target.SigningAlgorithm = alg;
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.Mobile.MobileAndroid source, Mobile.MobileAndroid target)
        {
            if (source.AppPackageName is { } app_package_name)
                target.AppPackageName = app_package_name;

            if (source.KeystoreHash is { } keystore_hash)
                target.KeystoreHash = keystore_hash;
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.Mobile.MobileIos source, Mobile.MobileIos target)
        {
            if (source.AppBundleIdentifier is { } app_bundle_identifier)
                target.AppBundleIdentifier = app_bundle_identifier;

            if (source.TeamId is { } team_id)
                target.TeamId = team_id;
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.Mobile source, Mobile target)
        {
            if (source.Android is { } android)
                ApplyToApi(android, target.Android ??= new Mobile.MobileAndroid());

            if (source.Ios is { } ios)
                ApplyToApi(ios, target.Ios ??= new Mobile.MobileIos());
        }

        global::Auth0.ManagementApi.Models.ComplianceLevel ToApi(Core.Models.Client.ComplianceLevel source)
        {
            return source switch
            {
                Core.Models.Client.ComplianceLevel.NONE => global::Auth0.ManagementApi.Models.ComplianceLevel.NONE,
                Core.Models.Client.ComplianceLevel.FAPI1_ADV_PKJ_PAR => global::Auth0.ManagementApi.Models.ComplianceLevel.FAPI1_ADV_PKJ_PAR,
                Core.Models.Client.ComplianceLevel.FAPI1_ADV_MTLS_PAR => global::Auth0.ManagementApi.Models.ComplianceLevel.FAPI1_ADV_MTLS_PAR,
                _ => throw new NotImplementedException(),
            };
        }

        OrganizationRequireBehavior ToApi(Core.Models.Organization.OrganizationRequireBehavior source)
        {
            return source switch
            {
                Core.Models.Organization.OrganizationRequireBehavior.NoPrompt => global::Auth0.ManagementApi.Models.OrganizationRequireBehavior.NoPrompt,
                Core.Models.Organization.OrganizationRequireBehavior.PreLoginPrompt => global::Auth0.ManagementApi.Models.OrganizationRequireBehavior.PreLoginPrompt,
                Core.Models.Organization.OrganizationRequireBehavior.PostLoginPrompt => global::Auth0.ManagementApi.Models.OrganizationRequireBehavior.PostLoginPrompt,
                _ => throw new NotImplementedException(),
            };
        }

        OrganizationUsage ToApi(Core.Models.OrganizationUsage source)
        {
            return source switch
            {
                Core.Models.OrganizationUsage.Deny => OrganizationUsage.Deny,
                Core.Models.OrganizationUsage.Allow => OrganizationUsage.Allow,
                Core.Models.OrganizationUsage.Require => OrganizationUsage.Require,
                _ => throw new NotImplementedException(),
            };
        }

        global::Auth0.ManagementApi.Models.RefreshTokenRotationType ToApi(Core.Models.Client.RefreshTokenRotationType source)
        {
            return source switch
            {
                Auth0.Operator.Core.Models.Client.RefreshTokenRotationType.Rotating => global::Auth0.ManagementApi.Models.RefreshTokenRotationType.Rotating,
                Auth0.Operator.Core.Models.Client.RefreshTokenRotationType.NonRotating => global::Auth0.ManagementApi.Models.RefreshTokenRotationType.NonRotating,
                _ => throw new NotImplementedException(),
            };
        }

        global::Auth0.ManagementApi.Models.RefreshTokenExpirationType ToApi(Core.Models.RefreshTokenExpirationType source)
        {
            return source switch
            {
                Auth0.Operator.Core.Models.RefreshTokenExpirationType.Expiring => global::Auth0.ManagementApi.Models.RefreshTokenExpirationType.Expiring,
                Auth0.Operator.Core.Models.RefreshTokenExpirationType.NonExpiring => global::Auth0.ManagementApi.Models.RefreshTokenExpirationType.NonExpiring,
                _ => throw new NotImplementedException(),
            };
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.Client.RefreshToken source, global::Auth0.ManagementApi.Models.RefreshToken target)
        {
            if (source.RotationType is { } rotation_type)
                target.RotationType = ToApi(rotation_type);

            if (source.ExpirationType is { } expiration_type)
                target.ExpirationType = ToApi(expiration_type);

            if (source.Leeway is { } leeway)
                target.Leeway = leeway;

            if (source.TokenLifetime is { } token_lifetime)
                target.TokenLifetime = token_lifetime;

            if (source.InfiniteTokenLifetime is { } infinite_token_lifetime)
                target.InfiniteTokenLifetime = infinite_token_lifetime;

            if (source.IdleTokenLifetime is { } idle_token_lifetime)
                target.IdleTokenLifetime = idle_token_lifetime;

            if (source.InfiniteIdleTokenLifetime is { } infinite_idle_token_lifetime)
                target.InfiniteIdleTokenLifetime = infinite_idle_token_lifetime;
        }

        global::Auth0.ManagementApi.Models.LogoutInitiatorModes ToApi(Core.Models.LogoutInitiatorModes source)
        {
            return source switch
            {
                Auth0.Operator.Core.Models.LogoutInitiatorModes.All => global::Auth0.ManagementApi.Models.LogoutInitiatorModes.All,
                Auth0.Operator.Core.Models.LogoutInitiatorModes.Custom => global::Auth0.ManagementApi.Models.LogoutInitiatorModes.Custom,
                _ => throw new NotImplementedException(),
            };
        }

        global::Auth0.ManagementApi.Models.LogoutInitiators ToApi(Core.Models.LogoutInitiators source)
        {
            return source switch
            {
                Auth0.Operator.Core.Models.LogoutInitiators.RpLogout => global::Auth0.ManagementApi.Models.LogoutInitiators.RpLogout,
                Auth0.Operator.Core.Models.LogoutInitiators.IdpLogout => global::Auth0.ManagementApi.Models.LogoutInitiators.IdpLogout,
                Auth0.Operator.Core.Models.LogoutInitiators.PasswordChanged => global::Auth0.ManagementApi.Models.LogoutInitiators.PasswordChanged,
                Auth0.Operator.Core.Models.LogoutInitiators.SessionExpired => global::Auth0.ManagementApi.Models.LogoutInitiators.SessionExpired,
                _ => throw new NotImplementedException(),
            };
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.BackchannelLogoutInitiators source, global::Auth0.ManagementApi.Models.BackchannelLogoutInitiators target)
        {
            if (source.Mode is { } backchannel_logout_urls)
                target.Mode = ToApi(backchannel_logout_urls);

            if (source.SelectedInitiators is { } backchannel_logout_initiators)
                target.SelectedInitiators = [.. backchannel_logout_initiators.Select(ToApi)];
        }

        void ApplyToApi(Alethic.Auth0.Operator.Core.Models.OidcLogoutConfig source, global::Auth0.ManagementApi.Models.OidcLogoutConfig target)
        {
            if (source.BackchannelLogoutUrls is { } backchannel_logout_urls)
                target.BackchannelLogoutUrls = backchannel_logout_urls;

            if (source.BackchannelLogoutInitiators is { } backchannel_logout_initiators)
                ApplyToApi(backchannel_logout_initiators, target.BackchannelLogoutInitiators ??= new());
        }

        /// <summary>
        /// Applies the configuration to the API request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        void ApplyToApi(V1ClientConf conf, ClientBase request)
        {
            if (conf.AddOns is { } addons)
                ApplyToApi(addons, request.AddOns ??= new());

            if (conf.AllowedClients is not null)
                request.AllowedClients = conf.AllowedClients;

            if (conf.AllowedLogoutUrls is not null)
                request.AllowedLogoutUrls = conf.AllowedLogoutUrls;

            if (conf.AllowedOrigins is not null)
                request.AllowedOrigins = conf.AllowedOrigins;

            if (conf.WebOrigins is not null)
                request.WebOrigins = conf.WebOrigins;

            if (conf.InitiateLoginUri is not null)
                request.InitiateLoginUri = conf.InitiateLoginUri;

            if (conf.Callbacks is not null)
                request.Callbacks = conf.Callbacks;

            if (conf.ClientAliases is not null)
                request.ClientAliases = conf.ClientAliases;

            if (conf.ClientMetaData is not null)
                request.ClientMetaData = conf.ClientMetaData;

            if (conf.IsCustomLoginPageOn is not null)
                request.IsCustomLoginPageOn = conf.IsCustomLoginPageOn;

            if (conf.IsFirstParty is not null)
                request.IsFirstParty = conf.IsFirstParty;

            if (conf.CustomLoginPage is not null)
                request.CustomLoginPage = conf.CustomLoginPage;

            if (conf.CustomLoginPagePreview is not null)
                request.CustomLoginPagePreview = conf.CustomLoginPagePreview;

            if (conf.EncryptionKey is { } encryption_key)
                ApplyToApi(encryption_key, request.EncryptionKey ??= new());

            if (conf.FormTemplate is not null)
                request.FormTemplate = conf.FormTemplate;

            if (conf.GrantTypes is not null)
                request.GrantTypes = conf.GrantTypes;

            if (conf.JwtConfiguration is { } jwt_configuration)
                ApplyToApi(jwt_configuration, request.JwtConfiguration ??= new());

            if (conf.Mobile is { } mobile)
                ApplyToApi(mobile, request.Mobile ??= new());

            if (conf.Name is not null)
                request.Name = conf.Name;

            if (conf.Description is not null)
                request.Description = conf.Description;

            if (conf.LogoUri is not null)
                request.LogoUri = conf.LogoUri;

            if (conf.OidcConformant is not null)
                request.OidcConformant = conf.OidcConformant;

            if (conf.OidcLogout is { } oidc_logout)
                ApplyToApi(oidc_logout, request.OidcLogout ??= new());

            if (conf.Sso is not null)
                request.Sso = conf.Sso;

            if (conf.RefreshToken is { } refresh_token)
                ApplyToApi(refresh_token, request.RefreshToken ??= new());

            if (conf.OrganizationUsage is { } organization_usage)
                request.OrganizationUsage = ToApi(organization_usage);

            if (conf.OrganizationRequireBehavior is { } organization_require_behavior)
                request.OrganizationRequireBehavior = ToApi(organization_require_behavior);

            if (conf.CrossOriginAuthentication is not null)
                request.CrossOriginAuthentication = conf.CrossOriginAuthentication;

            if (conf.RequirePushedAuthorizationRequests is not null)
                request.RequirePushedAuthorizationRequests = conf.RequirePushedAuthorizationRequests;

            if (conf.ComplianceLevel is { } compliance_level)
                request.ComplianceLevel = ToApi(compliance_level);

            if (conf.RequireProofOfPossession is not null)
                request.RequireProofOfPossession = conf.RequireProofOfPossession;
        }

        global::Auth0.ManagementApi.Models.ClientApplicationType ToApi(Core.Models.Client.ClientApplicationType source)
        {
            return source switch
            {
                Core.Models.Client.ClientApplicationType.Box => global::Auth0.ManagementApi.Models.ClientApplicationType.Box,
                Core.Models.Client.ClientApplicationType.Cloudbees => global::Auth0.ManagementApi.Models.ClientApplicationType.Cloudbees,
                Core.Models.Client.ClientApplicationType.Concur => global::Auth0.ManagementApi.Models.ClientApplicationType.Concur,
                Core.Models.Client.ClientApplicationType.Dropbox => global::Auth0.ManagementApi.Models.ClientApplicationType.Dropbox,
                Core.Models.Client.ClientApplicationType.Echosign => global::Auth0.ManagementApi.Models.ClientApplicationType.Echosign,
                Core.Models.Client.ClientApplicationType.Egnyte => global::Auth0.ManagementApi.Models.ClientApplicationType.Egnyte,
                Core.Models.Client.ClientApplicationType.MsCrm => global::Auth0.ManagementApi.Models.ClientApplicationType.MsCrm,
                Core.Models.Client.ClientApplicationType.Native => global::Auth0.ManagementApi.Models.ClientApplicationType.Native,
                Core.Models.Client.ClientApplicationType.NewRelic => global::Auth0.ManagementApi.Models.ClientApplicationType.NewRelic,
                Core.Models.Client.ClientApplicationType.NonInteractive => global::Auth0.ManagementApi.Models.ClientApplicationType.NonInteractive,
                Core.Models.Client.ClientApplicationType.Office365 => global::Auth0.ManagementApi.Models.ClientApplicationType.Office365,
                Core.Models.Client.ClientApplicationType.RegularWeb => global::Auth0.ManagementApi.Models.ClientApplicationType.RegularWeb,
                Core.Models.Client.ClientApplicationType.Rms => global::Auth0.ManagementApi.Models.ClientApplicationType.Rms,
                Core.Models.Client.ClientApplicationType.Salesforce => global::Auth0.ManagementApi.Models.ClientApplicationType.Salesforce,
                Core.Models.Client.ClientApplicationType.Sentry => global::Auth0.ManagementApi.Models.ClientApplicationType.Sentry,
                Core.Models.Client.ClientApplicationType.SharePoint => global::Auth0.ManagementApi.Models.ClientApplicationType.SharePoint,
                Core.Models.Client.ClientApplicationType.Slack => global::Auth0.ManagementApi.Models.ClientApplicationType.Slack,
                Core.Models.Client.ClientApplicationType.SpringCm => global::Auth0.ManagementApi.Models.ClientApplicationType.SpringCm,
                Core.Models.Client.ClientApplicationType.Spa => global::Auth0.ManagementApi.Models.ClientApplicationType.Spa,
                Core.Models.Client.ClientApplicationType.Zendesk => global::Auth0.ManagementApi.Models.ClientApplicationType.Zendesk,
                Core.Models.Client.ClientApplicationType.Zoom => global::Auth0.ManagementApi.Models.ClientApplicationType.Zoom,
                _ => throw new NotImplementedException(),
            };
        }

        global::Auth0.ManagementApi.Models.TokenEndpointAuthMethod ToApi(Core.Models.Client.TokenEndpointAuthMethod source)
        {
            return source switch
            {
                Core.Models.Client.TokenEndpointAuthMethod.None => global::Auth0.ManagementApi.Models.TokenEndpointAuthMethod.None,
                Core.Models.Client.TokenEndpointAuthMethod.ClientSecretPost => global::Auth0.ManagementApi.Models.TokenEndpointAuthMethod.ClientSecretPost,
                Core.Models.Client.TokenEndpointAuthMethod.ClientSecretBasic => global::Auth0.ManagementApi.Models.TokenEndpointAuthMethod.ClientSecretBasic,
                _ => throw new NotImplementedException(),
            };
        }

        void ApplyToApi(V1ClientConf conf, ClientCreateRequest request)
        {
            if (conf.ApplicationType is { } app_type)
                request.ApplicationType = ToApi(app_type);

            if (conf.TokenEndpointAuthMethod is { } token_endpoint_auth_method)
                request.TokenEndpointAuthMethod = ToApi(token_endpoint_auth_method);

            ApplyToApi(conf, (ClientBase)request);
        }

        void ApplyToApi(V1ClientConf conf, ClientUpdateRequest request)
        {
            if (conf.ApplicationType is { } app_type)
                request.ApplicationType = ToApi(app_type);

            if (conf.TokenEndpointAuthMethod is { } token_endpoint_auth_method)
                request.TokenEndpointAuthMethod = ToApi(token_endpoint_auth_method);

            ApplyToApi(conf, (ClientBase)request);
        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Client";

        /// <inheritdoc />
        protected override async Task<Hashtable?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return TransformToSystemTextJson<Hashtable>(await api.Clients.GetAsync(id, cancellationToken: cancellationToken));
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Client entity, V1Client.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                // attempt to search by client ID
                if (spec.Find.ClientId is string clientId)
                {
                    try
                    {
                        var client = await api.Clients.GetAsync(clientId, "client_id,name", cancellationToken: cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing client: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), client.Name);
                        return client.ClientId;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find client with id {ClientId}.", EntityTypeName, entity.Namespace(), entity.Name(), clientId);
                    }
                }

                // attempt to search by name
                if (spec.Find.Name is string name)
                {
                    var list = await api.Clients.GetAllAsync(new GetClientsRequest() { Fields = "client_id,name" }, cancellationToken: cancellationToken);
                    var self = list.FirstOrDefault(i => i.Name == name);
                    return self?.ClientId;
                }
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is null)
                    return null;

                var list = await api.Clients.GetAllAsync(new GetClientsRequest() { Fields = "client_id,name" }, cancellationToken: cancellationToken);
                var self = list.FirstOrDefault(i => i.Name == conf.Name);
                return self?.ClientId;
            }

            return null;
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1ClientConf conf)
        {
            if (conf.ApplicationType == null)
                return "missing a value for application type";

            return null;
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating client in Auth0 with name: {ClientName}", EntityTypeName, conf.Name);

            // transform request
            var req = new ClientCreateRequest();
            ApplyToApi(conf, req);

            var self = await api.Clients.CreateAsync(req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created client in Auth0 with ID: {ClientId} and name: {ClientName}", EntityTypeName, self.ClientId, conf.Name);
            return self.ClientId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, Hashtable? last, V1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);

            // transform request
            var req = new ClientUpdateRequest();
            ApplyToApi(conf, req);

            // explicitely null out missing metadata if previously present
            if (last is not null && last.ContainsKey("client_metadata") && conf.ClientMetaData != null)
                foreach (string key in ((Hashtable)last["client_metadata"]!).Keys)
                    if (conf.ClientMetaData.ContainsKey(key) == false)
                        req.ClientMetaData[key] = null;

            await api.Clients.UpdateAsync(id, req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1Client entity, Hashtable lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // Always attempt to apply secret if secretRef is specified, regardless of whether we have the clientSecret value
            // This ensures secret resources are created for existing clients even when Auth0 API doesn't return the secret
            if (entity.Spec.SecretRef is not null)
            {
                var clientId = (string?)lastConf["client_id"];
                var clientSecret = (string?)lastConf["client_secret"];
                await ApplySecret(entity, clientId, clientSecret, defaultNamespace, cancellationToken);
            }

            lastConf.Remove("client_id");
            lastConf.Remove("client_secret");
            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <summary>
        /// Applies the client secret.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task ApplySecret(V1Client entity, string? clientId, string? clientSecret, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (entity.Spec.SecretRef is null)
                return;

            // find existing secret or create
            var secret = await ResolveSecretRef(entity.Spec.SecretRef, entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, cancellationToken);
            if (secret is null)
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName} which does not exist: creating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                secret = await Kube.CreateAsync(
                    new V1Secret() { Metadata = new V1ObjectMeta() { NamespaceProperty = entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, Name = entity.Spec.SecretRef.Name } }
                        .WithOwnerReference(entity),
                    cancellationToken);
            }

            // only apply actual values if we are the owner
            if (secret.IsOwnedBy(entity))
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName}: updating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                secret.StringData ??= new Dictionary<string, string>();

                // Always set clientId if available
                if (clientId is not null)
                {
                    secret.StringData["clientId"] = clientId;
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} updated secret {SecretName} with clientId", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }
                else if (!secret.StringData.ContainsKey("clientId"))
                {
                    // Initialize empty clientId field if not present and no value available
                    secret.StringData["clientId"] = "";
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} initialized empty clientId in secret {SecretName}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }

                // Handle clientSecret - for existing clients, Auth0 API doesn't return the secret
                if (clientSecret is not null)
                {
                    secret.StringData["clientSecret"] = clientSecret;
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} updated secret {SecretName} with clientSecret", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }
                else if (!secret.StringData.ContainsKey("clientSecret"))
                {
                    // Initialize empty clientSecret field if not present and no value available
                    // Note: For existing clients, Auth0 API doesn't return the secret value for security reasons
                    secret.StringData["clientSecret"] = "";
                    Logger.LogDebug("{EntityTypeName} {EntityNamespace}/{EntityName} initialized empty clientSecret in secret {SecretName} (Auth0 API does not return secrets for existing clients)", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                }

                secret = await Kube.UpdateAsync(secret, cancellationToken);
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} successfully updated secret {SecretName}", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
            }
            else
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} secret {SecretName} exists but is not owned by this client, skipping update", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
            }
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting client from Auth0 with ID: {ClientId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Clients.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted client from Auth0 with ID: {ClientId}", EntityTypeName, id);
        }

    }

}
