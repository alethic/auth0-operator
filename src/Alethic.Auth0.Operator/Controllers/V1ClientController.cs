using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Client.V1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

using Newtonsoft.Json.Linq;

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
        V1TenantEntityInstanceController<V1Client, V1Client.SpecDef, V1Client.StatusDef, V1ClientConf, V1ClientConf>,
        IEntityController<V1Client>
    {

        /// <summary>
        /// Transforms the Auth0 Management API client model to the operator's client configuration model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientConf? FromApi(Client? source) => source is null ? null : new()
        {
            AllowedClients = source.AllowedClients?.ToArray(),
            AllowedLogoutUrls = source.AllowedLogoutUrls?.ToArray(),
            AllowedOrigins = source.AllowedOrigins?.ToArray(),
            WebOrigins = source.WebOrigins?.ToArray(),
            InitiateLoginUri = source.InitiateLoginUri,
            Callbacks = source.Callbacks?.ToArray(),
            ClientAliases = source.ClientAliases?.ToArray(),
            ClientMetaData = source.ClientMetaData,
            IsCustomLoginPageOn = source.IsCustomLoginPageOn,
            IsFirstParty = source.IsFirstParty,
            CustomLoginPage = source.CustomLoginPage,
            CustomLoginPagePreview = source.CustomLoginPagePreview,
            FormTemplate = source.FormTemplate,
            GrantTypes = source.GrantTypes?.ToArray(),
            Name = source.Name,
            Description = source.Description,
            LogoUri = source.LogoUri,
            OidcConformant = source.OidcConformant,
            Sso = source.Sso,
            CrossOriginAuthentication = source.CrossOriginAuthentication,
            RequirePushedAuthorizationRequests = source.RequirePushedAuthorizationRequests,
            RequireProofOfPossession = source.RequireProofOfPossession,
            AddOns = FromApi(source.AddOns),
            ApplicationType = FromApi(source.ApplicationType),
            ComplianceLevel = FromApi(source.ComplianceLevel),
            DefaultOrganization = FromApi(source.DefaultOrganization),
            EncryptionKey = FromApi(source.EncryptionKey),
            JwtConfiguration = FromApi(source.JwtConfiguration),
            Mobile = FromApi(source.Mobile),
            OidcLogout = FromApi(source.OidcLogout),
            OrganizationRequireBehavior = FromApi(source.OrganizationRequireBehavior),
            OrganizationUsage = FromApi(source.OrganizationUsage),
            RefreshToken = FromApi(source.RefreshToken),
            ResourceServers = source.ResourceServers?.Select(i => FromApi(i)).ToArray(),
            SigningKeys = source.SigningKeys?.Select(i => FromApi(i)).ToArray(),
            TokenEndpointAuthMethod = FromApi(source.TokenEndpointAuthMethod),
        };

        /// <summary>
        /// Transforms the token endpoint authentication method from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientTokenEndpointAuthMethod? FromApi(TokenEndpointAuthMethod? source) => source switch
        {
            TokenEndpointAuthMethod.None => V1ClientTokenEndpointAuthMethod.None,
            TokenEndpointAuthMethod.ClientSecretPost => V1ClientTokenEndpointAuthMethod.ClientSecretPost,
            TokenEndpointAuthMethod.ClientSecretBasic => V1ClientTokenEndpointAuthMethod.ClientSecretBasic,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the client signing keys from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientSigningKey? FromApi(SigningKey? source) => source is null ? null : new()
        {
            Cert = source.Cert,
            Key = source.Key,
            Pkcs7 = source.Pkcs7,
        };

        /// <summary>
        /// Transforms the client resource server association from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientResourceServerAssociation? FromApi(ClientResourceServerAssociation? source) => source is null ? null : new()
        {
            Identifier = source.Identifier,
            Scopes = source.Scopes,
        };

        /// <summary>
        /// Transforms the refresh token configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientRefreshToken? FromApi(RefreshToken? source) => source is null ? null : new()
        {
            ExpirationType = FromApi(source.ExpirationType),
            InfiniteIdleTokenLifetime = source.InfiniteIdleTokenLifetime,
            InfiniteTokenLifetime = source.InfiniteTokenLifetime,
            Leeway = source.Leeway,
            RotationType = FromApi(source.RotationType),
            TokenLifetime = source.TokenLifetime,
        };

        /// <summary>
        /// Transforms the refresh token rotation type from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientRefreshTokenRotationType? FromApi(RefreshTokenRotationType? source) => source switch
        {
            RefreshTokenRotationType.Rotating => V1ClientRefreshTokenRotationType.Rotating,
            RefreshTokenRotationType.NonRotating => V1ClientRefreshTokenRotationType.NonRotating,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the refresh token expiration type from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientRefreshTokenExpirationType? FromApi(RefreshTokenExpirationType? source) => source switch
        {
            RefreshTokenExpirationType.Expiring => V1ClientRefreshTokenExpirationType.Expiring,
            RefreshTokenExpirationType.NonExpiring => V1ClientRefreshTokenExpirationType.NonExpiring,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the organization usage from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientOrganizationUsage? FromApi(OrganizationUsage? source) => source switch
        {
            OrganizationUsage.Deny => V1ClientOrganizationUsage.Deny,
            OrganizationUsage.Allow => V1ClientOrganizationUsage.Allow,
            OrganizationUsage.Require => V1ClientOrganizationUsage.Require,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the organization require behavior from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientOrganizationRequireBehavior? FromApi(OrganizationRequireBehavior? source) => source switch
        {
            OrganizationRequireBehavior.NoPrompt => V1ClientOrganizationRequireBehavior.NoPrompt,
            OrganizationRequireBehavior.PreLoginPrompt => V1ClientOrganizationRequireBehavior.PreLoginPrompt,
            OrganizationRequireBehavior.PostLoginPrompt => V1ClientOrganizationRequireBehavior.PostLoginPrompt,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the OIDC logout configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientOidcLogoutConfig? FromApi(OidcLogoutConfig? source) => source is null ? null : new()
        {
            BackchannelLogoutUrls = source.BackchannelLogoutUrls,
            BackchannelLogoutInitiators = FromApi(source.BackchannelLogoutInitiators),
        };

        /// <summary>
        /// Transforms the backchannel logout initiators from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientBackchannelLogoutInitiators? FromApi(BackchannelLogoutInitiators? source) => source is null ? null : new()
        {
            Mode = FromApi(source.Mode),
            SelectedInitiators = source.SelectedInitiators?.Select(FromApi).ToArray(),
        };

        /// <summary>
        /// Transforms the logout initiators from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static V1ClientLogoutInitiators FromApi(LogoutInitiators source) => source switch
        {
            LogoutInitiators.RpLogout => V1ClientLogoutInitiators.RpLogout,
            LogoutInitiators.IdpLogout => V1ClientLogoutInitiators.IdpLogout,
            LogoutInitiators.PasswordChanged => V1ClientLogoutInitiators.PasswordChanged,
            LogoutInitiators.SessionExpired => V1ClientLogoutInitiators.SessionExpired,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the logout initiator modes from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientLogoutInitiatorModes? FromApi(LogoutInitiatorModes? source) => source switch
        {
            LogoutInitiatorModes.All => V1ClientLogoutInitiatorModes.All,
            LogoutInitiatorModes.Custom => V1ClientLogoutInitiatorModes.Custom,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the mobile configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientMobile? FromApi(Mobile? source) => source is null ? null : new()
        {
            Android = FromApi(source.Android),
            Ios = FromApi(source.Ios),
        };

        /// <summary>
        /// Transforms the iOS mobile configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1ClientMobile.MobileIos? FromApi(Mobile.MobileIos? source)
        {
            if (source is null)
                return null;
            if (source.AppBundleIdentifier is null && source.TeamId is null)
                return null;

            return new()
            {
                AppBundleIdentifier = source.AppBundleIdentifier,
                TeamId = source.TeamId,
            };
        }

        /// <summary>
        /// Transforms the Android mobile configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1ClientMobile.MobileAndroid? FromApi(Mobile.MobileAndroid? source)
        {
            if (source is null)
                return null;
            if (source.AppPackageName is null && source.KeystoreHash is null)
                return null;

            return new()
            {
                AppPackageName = source.AppPackageName,
                KeystoreHash = source.KeystoreHash,
            };
        }

        /// <summary>
        /// Transforms the client JWT configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientJwtConfiguration? FromApi(JwtConfiguration? source) => source is null ? null : new()
        {
            IsSecretEncoded = source.IsSecretEncoded,
            LifetimeInSeconds = source.LifetimeInSeconds,
            Scopes = FromApi(source.Scopes),
            SigningAlgorithm = source.SigningAlgorithm,
        };

        /// <summary>
        /// Transforms the client scopes from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientScopes? FromApi(Scopes? source) => source is null ? null : new()
        {
            Users = FromApi(source.Users),
            UsersAppMetadata = FromApi(source.UsersAppMetadata),
            Clients = FromApi(source.Clients),
            ClientKeys = FromApi(source.ClientKeys),
            Tokens = FromApi(source.Tokens),
            Stats = FromApi(source.Stats),
        };

        /// <summary>
        /// Transforms the client scope entry from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientScopeEntry? FromApi(ScopeEntry? source) => source is null ? null : new()
        {
            Actions = source.Actions,
        };

        /// <summary>
        /// Transforms the client encryption key configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientEncryptionKey? FromApi(EncryptionKey? source) => source is null ? null : new()
        {
            Certificate = source.Certificate,
            PublicKey = source.PublicKey,
            Subject = source.Subject,
        };

        /// <summary>
        /// Extracts the default organization configuration from the API response and transforms it to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientDefaultOrganization? FromApi(DefaultOrganization? source) => source is null ? null : new()
        {
            OrganizationId = source.OrganizationId,
            Flows = source.Flows?.Select(FromApi).ToArray(),
        };

        /// <summary>
        /// Transforms the client flow from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientFlows FromApi(Flows source) => source switch
        {
            Flows.ClientCredentials => V1ClientFlows.ClientCredentials,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the compliance level from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientComplianceLevel? FromApi(ComplianceLevel? source) => source switch
        {
            ComplianceLevel.NONE => V1ClientComplianceLevel.NONE,
            ComplianceLevel.FAPI1_ADV_PKJ_PAR => V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR,
            ComplianceLevel.FAPI1_ADV_MTLS_PAR => V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the application type from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientApplicationType? FromApi(ClientApplicationType? source) => source switch
        {
            ClientApplicationType.Box => V1ClientApplicationType.Box,
            ClientApplicationType.Cloudbees => V1ClientApplicationType.Cloudbees,
            ClientApplicationType.Concur => V1ClientApplicationType.Concur,
            ClientApplicationType.Dropbox => V1ClientApplicationType.Dropbox,
            ClientApplicationType.Echosign => V1ClientApplicationType.Echosign,
            ClientApplicationType.Egnyte => V1ClientApplicationType.Egnyte,
            ClientApplicationType.MsCrm => V1ClientApplicationType.MsCrm,
            ClientApplicationType.Native => V1ClientApplicationType.Native,
            ClientApplicationType.NewRelic => V1ClientApplicationType.NewRelic,
            ClientApplicationType.NonInteractive => V1ClientApplicationType.NonInteractive,
            ClientApplicationType.Office365 => V1ClientApplicationType.Office365,
            ClientApplicationType.RegularWeb => V1ClientApplicationType.RegularWeb,
            ClientApplicationType.Rms => V1ClientApplicationType.Rms,
            ClientApplicationType.Salesforce => V1ClientApplicationType.Salesforce,
            ClientApplicationType.Sentry => V1ClientApplicationType.Sentry,
            ClientApplicationType.SharePoint => V1ClientApplicationType.SharePoint,
            ClientApplicationType.Slack => V1ClientApplicationType.Slack,
            ClientApplicationType.SpringCm => V1ClientApplicationType.SpringCm,
            ClientApplicationType.Spa => V1ClientApplicationType.Spa,
            ClientApplicationType.Zendesk => V1ClientApplicationType.Zendesk,
            ClientApplicationType.Zoom => V1ClientApplicationType.Zoom,
            ClientApplicationType.ResourceServer => V1ClientApplicationType.ResourceServer,
            ClientApplicationType.ExpressConfiguration => V1ClientApplicationType.ExpressConfiguration,
            ClientApplicationType.SsoIntegration => V1ClientApplicationType.SsoIntegration,
            ClientApplicationType.Oag => V1ClientApplicationType.Oag,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Extracts the add-ons configuration from the API response and transforms it to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientAddons? FromApi(Addons? source) => source is { } addOns ? new V1ClientAddons()
        {
            Aws = FromApiAddonAws(addOns.AmazonWebServices as JObject),
            AzureSb = FromApiAddonAzureSb(addOns.AzureServiceBus as JObject),
            Box = FromApiAddonDictionary(addOns.Box as JObject),
            Cloudbees = FromApiAddonDictionary(addOns.CloudBees as JObject),
            Concur = FromApiAddonDictionary(addOns.Concur as JObject),
            Dropbox = FromApiAddonDictionary(addOns.DropBox as JObject),
            Echosign = FromApiAddonEchoSign(addOns.EchoSign as JObject),
            Egnyte = FromApiAddonEgnyte(addOns.Egnyte as JObject),
            Firebase = FromApiAddonFirebase(addOns.FireBase as JObject),
            Newrelic = FromApiAddonNewRelic(addOns.NewRelic as JObject),
            Office365 = FromApiAddonOffice365(addOns.Office365 as JObject),
            Salesforce = FromApiAddonSalesforce(addOns.SalesForce as JObject),
            SalesforceApi = FromApiAddonSalesforceApi(addOns.SalesForceApi as JObject),
            SalesforceSandboxApi = FromApiAddonSalesforceSandboxApi(addOns.SalesForceSandboxApi as JObject),
            Samlp = FromApiAddonSaml(addOns.SamlP as JObject),
            SapApi = FromApiAddonSapApi(addOns.SapApi as JObject),
            Sharepoint = FromApiAddonSharePoint(addOns.SharePoint as JObject),
            Springcm = FromApiAddonSpringCm(addOns.SpringCM as JObject),
            Wsfed = FromApiAddonDictionary(addOns.WsFed as JObject),
            Zendesk = FromApiAddonZendesk(addOns.Zendesk as JObject),
            Zoom = FromApiAddonZoom(addOns.Zoom as JObject),
        } : null;

        internal static V1ClientAddonAws? FromApiAddonAws(JObject? o) => o is null ? null : new()
        {
            Principal = (string?)o["principal"],
            Role = (string?)o["role"],
            LifetimeInSeconds = (int?)o["lifetime_in_seconds"],
        };

        internal static V1ClientAddonAzureSb? FromApiAddonAzureSb(JObject? o) => o is null ? null : new()
        {
            Namespace = (string?)o["namespace"],
            SasKeyName = (string?)o["sasKeyName"],
            SasKey = (string?)o["sasKey"],
            EntityPath = (string?)o["entityPath"],
        };

        internal static Dictionary<string, object?>? FromApiAddonDictionary(JObject? o)
        {
            return o?.ToObject<Dictionary<string, object?>>();
        }

        internal static V1ClientAddonEchoSign? FromApiAddonEchoSign(JObject? o) => o is null ? null : new()
        {
            Domain = (string?)o["domain"],
        };

        internal static V1ClientAddonEgnyte? FromApiAddonEgnyte(JObject? o) => o is null ? null : new()
        {
            Domain = (string?)o["domain"],
        };

        internal static V1ClientAddonFirebase? FromApiAddonFirebase(JObject? o) => o is null ? null : new()
        {
            Secret = (string?)o["secret"],
            PrivateKeyId = (string?)o["private_key_id"],
            PrivateKey = (string?)o["private_key"],
            ClientEmail = (string?)o["client_email"],
            LifetimeInSeconds = (int?)o["lifetime_in_seconds"],
        };

        internal static V1ClientAddonNewRelic? FromApiAddonNewRelic(JObject? o) => o is null ? null : new()
        {
            Account = (string?)o["account"],
        };

        internal static V1ClientAddonOffice365? FromApiAddonOffice365(JObject? o) => o is null ? null : new()
        {
            Domain = (string?)o["domain"],
            Connection = (string?)o["connection"],
        };

        internal static V1ClientAddonSalesforce? FromApiAddonSalesforce(JObject? o) => o is null ? null : new()
        {
            EntityId = (string?)o["entity_id"],
        };

        internal static V1ClientAddonSalesforceApi? FromApiAddonSalesforceApi(JObject? o) => o is null ? null : new()
        {
            Clientid = (string?)o["clientid"],
            Principal = (string?)o["principal"],
            CommunityName = (string?)o["communityName"],
            CommunityUrlSection = (string?)o["community_url_section"],
        };

        internal static V1ClientAddonSalesforceSandboxApi? FromApiAddonSalesforceSandboxApi(JObject? o) => o is null ? null : new()
        {
            Clientid = (string?)o["clientid"],
            Principal = (string?)o["principal"],
            CommunityName = (string?)o["communityName"],
            CommunityUrlSection = (string?)o["community_url_section"],
        };

        internal static V1ClientAddonSaml? FromApiAddonSaml(JObject? o) => o is null ? null : new()
        {
            Mappings = o["mappings"]?.ToObject<Dictionary<string, object?>>(),
            Audience = (string?)o["audience"],
            Recipient = (string?)o["recipient"],
            CreateUpnClaim = (bool?)o["createUpnClaim"],
            MapUnknownClaimsAsIs = (bool?)o["mapUnknownClaimsAsIs"],
            PassthroughClaimsWithNoMapping = (bool?)o["passthroughClaimsWithNoMapping"],
            MapIdentities = (bool?)o["mapIdentities"],
            SignatureAlgorithm = (string?)o["signatureAlgorithm"],
            DigestAlgorithm = (string?)o["digestAlgorithm"],
            Issuer = (string?)o["issuer"],
            Destination = (string?)o["destination"],
            LifetimeInSeconds = (int?)o["lifetimeInSeconds"],
            SignResponse = (bool?)o["signResponse"],
            NameIdentifierFormat = (string?)o["nameIdentifierFormat"],
            NameIdentifierProbes = o["nameIdentifierProbes"]?.ToObject<List<string>>(),
            AuthnContextClassRef = (string?)o["authnContextClassRef"],
        };

        internal static V1ClientAddonSapapi? FromApiAddonSapApi(JObject? o) => o is null ? null : new()
        {
            Clientid = (string?)o["clientid"],
            UsernameAttribute = (string?)o["usernameAttribute"],
            TokenEndpointUrl = (string?)o["tokenEndpointUrl"],
            Scope = (string?)o["scope"],
            ServicePassword = (string?)o["servicePassword"],
            NameIdentifierFormat = (string?)o["nameIdentifierFormat"],
        };

        internal static V1ClientAddonSharePoint? FromApiAddonSharePoint(JObject? o) => o is null ? null : new()
        {
            Url = (string?)o["url"],
            ExternalUrl = (o["external_url"] as JObject)?.ToObject<V1ClientAddonSharePointExternalUrl>(),
        };

        internal static V1ClientAddonSpringCm? FromApiAddonSpringCm(JObject? o) => o is null ? null : new()
        {
            Acsurl = (string?)o["acsurl"],
        };

        internal static V1ClientAddonZendesk? FromApiAddonZendesk(JObject? o) => o is null ? null : new()
        {
            AccountName = (string?)o["accountName"],
        };

        internal static V1ClientAddonZoom? FromApiAddonZoom(JObject? o) => o is null ? null : new()
        {
            Account = (string?)o["account"],
        };

        /// <summary>
        /// Applies the add-ons configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientAddons source, Addons target)
        {
            if (source.Aws is { } aws)
                target.AmazonWebServices = aws;

            if (source.AzureSb is { } azure_sb)
                target.AzureServiceBus = azure_sb;

            if (source.Box is { } box)
                target.Box = box;

            if (source.Cloudbees is { } cloudbees)
                target.CloudBees = cloudbees;

            if (source.Concur is { } concur)
                target.Concur = concur;

            if (source.Dropbox is { } dropbox)
                target.DropBox = dropbox;

            if (source.Echosign is { } echosign)
                target.EchoSign = echosign;

            if (source.Egnyte is { } egnyte)
                target.Egnyte = egnyte;

            if (source.Firebase is { } firebase)
                target.FireBase = firebase;

            if (source.Newrelic is { } newrelic)
                target.NewRelic = newrelic;

            if (source.Office365 is { } office365)
                target.Office365 = office365;

            if (source.Salesforce is { } salesforce)
                target.SalesForce = salesforce;

            if (source.SalesforceApi is { } salesforce_api)
                target.SalesForceApi = salesforce_api;

            if (source.SalesforceSandboxApi is { } salesforce_sandbox_api)
                target.SalesForceSandboxApi = salesforce_sandbox_api;

            if (source.Samlp is { } samlp)
                target.SamlP = samlp;

            if (source.SapApi is { } sap_api)
                target.SapApi = sap_api;

            if (source.Sharepoint is { } sharepoint)
                target.SharePoint = sharepoint;

            if (source.Springcm is { } springcm)
                target.SpringCM = springcm;

            if (source.Wsfed is { } wsfed)
                target.WsFed = wsfed;

            if (source.Zendesk is { } zendesk)
                target.Zendesk = zendesk;

            if (source.Zoom is { } zoom)
                target.Zoom = zoom;
        }

        /// <summary>
        /// Applies the encryption key configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientEncryptionKey source, EncryptionKey target)
        {
            if (source.Certificate is { } cert)
                target.Certificate = cert;

            if (source.PublicKey is { } pub)
                target.PublicKey = pub;

            if (source.Subject is { } subject)
                target.Subject = subject;
        }

        /// <summary>
        /// Applies the scope entry configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientScopeEntry source, ScopeEntry target)
        {
            if (source.Actions is { } actions)
                target.Actions = actions;
        }

        /// <summary>
        /// Applies the scopes configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientScopes source, Scopes target)
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

        /// <summary>
        /// Applies the JWT configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientJwtConfiguration source, JwtConfiguration target)
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

        /// <summary>
        /// Applies the mobile configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientMobile.MobileAndroid source, Mobile.MobileAndroid target)
        {
            if (source.AppPackageName is { } app_package_name)
                target.AppPackageName = app_package_name;

            if (source.KeystoreHash is { } keystore_hash)
                target.KeystoreHash = keystore_hash;
        }

        /// <summary>
        /// Applies the iOS mobile configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientMobile.MobileIos source, Mobile.MobileIos target)
        {
            if (source.AppBundleIdentifier is { } app_bundle_identifier)
                target.AppBundleIdentifier = app_bundle_identifier;

            if (source.TeamId is { } team_id)
                target.TeamId = team_id;
        }

        /// <summary>
        /// Applies the mobile configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientMobile source, Mobile target)
        {
            if (source.Android is { } android)
                if (source.Android.AppPackageName is not null || source.Android.KeystoreHash is not null)
                    ApplyToApi(android, target.Android ??= new Mobile.MobileAndroid());

            if (source.Ios is { } ios)
                if (source.Ios.AppBundleIdentifier is not null || source.Ios.TeamId is not null)
                    ApplyToApi(ios, target.Ios ??= new Mobile.MobileIos());
        }

        /// <summary>
        /// Transforms the compliance level from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ComplianceLevel ToApi(V1ClientComplianceLevel source) => source switch
        {
            V1ClientComplianceLevel.NONE => ComplianceLevel.NONE,
            V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR => ComplianceLevel.FAPI1_ADV_PKJ_PAR,
            V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR => ComplianceLevel.FAPI1_ADV_MTLS_PAR,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the organization require behavior from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static OrganizationRequireBehavior ToApi(V1ClientOrganizationRequireBehavior source) => source switch
        {
            V1ClientOrganizationRequireBehavior.NoPrompt => OrganizationRequireBehavior.NoPrompt,
            V1ClientOrganizationRequireBehavior.PreLoginPrompt => OrganizationRequireBehavior.PreLoginPrompt,
            V1ClientOrganizationRequireBehavior.PostLoginPrompt => OrganizationRequireBehavior.PostLoginPrompt,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the organization usage from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static OrganizationUsage ToApi(V1ClientOrganizationUsage source) => source switch
        {
            V1ClientOrganizationUsage.Deny => OrganizationUsage.Deny,
            V1ClientOrganizationUsage.Allow => OrganizationUsage.Allow,
            V1ClientOrganizationUsage.Require => OrganizationUsage.Require,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the refresh token rotation type from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static RefreshTokenRotationType ToApi(V1ClientRefreshTokenRotationType source) => source switch
        {
            V1ClientRefreshTokenRotationType.Rotating => RefreshTokenRotationType.Rotating,
            V1ClientRefreshTokenRotationType.NonRotating => RefreshTokenRotationType.NonRotating,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the refresh token expiration type from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static RefreshTokenExpirationType ToApi(V1ClientRefreshTokenExpirationType source) => source switch
        {
            V1ClientRefreshTokenExpirationType.Expiring => RefreshTokenExpirationType.Expiring,
            V1ClientRefreshTokenExpirationType.NonExpiring => RefreshTokenExpirationType.NonExpiring,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Applies the refresh token configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientRefreshToken source, RefreshToken target)
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

        /// <summary>
        /// Transforms the logout initiator modes from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static LogoutInitiatorModes ToApi(V1ClientLogoutInitiatorModes source)
        {
            return source switch
            {
                V1ClientLogoutInitiatorModes.All => LogoutInitiatorModes.All,
                V1ClientLogoutInitiatorModes.Custom => LogoutInitiatorModes.Custom,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Transforms the logout initiators from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static LogoutInitiators ToApi(V1ClientLogoutInitiators source) => source switch
        {
            V1ClientLogoutInitiators.RpLogout => LogoutInitiators.RpLogout,
            V1ClientLogoutInitiators.IdpLogout => LogoutInitiators.IdpLogout,
            V1ClientLogoutInitiators.PasswordChanged => LogoutInitiators.PasswordChanged,
            V1ClientLogoutInitiators.SessionExpired => LogoutInitiators.SessionExpired,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Applies the backchannel logout initiators configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientBackchannelLogoutInitiators source, BackchannelLogoutInitiators target)
        {
            if (source.Mode is { } backchannel_logout_urls)
                target.Mode = ToApi(backchannel_logout_urls);

            if (source.SelectedInitiators is { } backchannel_logout_initiators)
                target.SelectedInitiators = [.. backchannel_logout_initiators.Select(ToApi)];
        }

        /// <summary>
        /// Applies the OIDC logout configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientOidcLogoutConfig source, OidcLogoutConfig target)
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
        internal static void ApplyToApi(V1ClientConf conf, ClientBase request)
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
                request.GrantTypes = conf.GrantTypes.Distinct().ToArray();

            if (conf.JwtConfiguration is { } jwt_configuration)
                ApplyToApi(jwt_configuration, request.JwtConfiguration ??= new());

            if (conf.Mobile is { } mobile)
            {
                var target = new Mobile();
                ApplyToApi(mobile, target);

                // only set on request if at least one of the mobile configurations is set, as the API will reject an empty mobile object
                if (target.Android is not null || target.Ios is not null)
                    request.Mobile = target;
            }

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

        /// <summary>
        /// Transforms the application type from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientApplicationType ToApi(V1ClientApplicationType source) => source switch
        {
            V1ClientApplicationType.Box => ClientApplicationType.Box,
            V1ClientApplicationType.Cloudbees => ClientApplicationType.Cloudbees,
            V1ClientApplicationType.Concur => ClientApplicationType.Concur,
            V1ClientApplicationType.Dropbox => ClientApplicationType.Dropbox,
            V1ClientApplicationType.Echosign => ClientApplicationType.Echosign,
            V1ClientApplicationType.Egnyte => ClientApplicationType.Egnyte,
            V1ClientApplicationType.MsCrm => ClientApplicationType.MsCrm,
            V1ClientApplicationType.Native => ClientApplicationType.Native,
            V1ClientApplicationType.NewRelic => ClientApplicationType.NewRelic,
            V1ClientApplicationType.NonInteractive => ClientApplicationType.NonInteractive,
            V1ClientApplicationType.Office365 => ClientApplicationType.Office365,
            V1ClientApplicationType.RegularWeb => ClientApplicationType.RegularWeb,
            V1ClientApplicationType.Rms => ClientApplicationType.Rms,
            V1ClientApplicationType.Salesforce => ClientApplicationType.Salesforce,
            V1ClientApplicationType.Sentry => ClientApplicationType.Sentry,
            V1ClientApplicationType.SharePoint => ClientApplicationType.SharePoint,
            V1ClientApplicationType.Slack => ClientApplicationType.Slack,
            V1ClientApplicationType.SpringCm => ClientApplicationType.SpringCm,
            V1ClientApplicationType.Spa => ClientApplicationType.Spa,
            V1ClientApplicationType.Zendesk => ClientApplicationType.Zendesk,
            V1ClientApplicationType.Zoom => ClientApplicationType.Zoom,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the token endpoint authentication method from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static TokenEndpointAuthMethod ToApi(V1ClientTokenEndpointAuthMethod source) => source switch
        {
            V1ClientTokenEndpointAuthMethod.None => TokenEndpointAuthMethod.None,
            V1ClientTokenEndpointAuthMethod.ClientSecretPost => TokenEndpointAuthMethod.ClientSecretPost,
            V1ClientTokenEndpointAuthMethod.ClientSecretBasic => TokenEndpointAuthMethod.ClientSecretBasic,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Applies client configuration settings to the specified API client creation request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        internal static void ApplyToApi(V1ClientConf conf, ClientCreateRequest request)
        {
            if (conf.ApplicationType is { } app_type)
                request.ApplicationType = ToApi(app_type);

            if (conf.TokenEndpointAuthMethod is { } token_endpoint_auth_method)
                request.TokenEndpointAuthMethod = ToApi(token_endpoint_auth_method);

            ApplyToApi(conf, (ClientBase)request);
        }

        /// <summary>
        /// Applies the specified client configuration to the API update request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        internal static void ApplyToApi(V1ClientConf conf, ClientUpdateRequest request)
        {
            if (conf.ApplicationType is { } app_type)
                request.ApplicationType = ToApi(app_type);

            if (conf.TokenEndpointAuthMethod is { } token_endpoint_auth_method)
                request.TokenEndpointAuthMethod = ToApi(token_endpoint_auth_method);

            ApplyToApi(conf, (ClientBase)request);
        }

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

        /// <inheritdoc />
        protected override string EntityTypeName => "Client";

        /// <inheritdoc />
        protected override async Task<V1ClientConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                return FromApi(await api.Clients.GetAsync(id, cancellationToken: cancellationToken));
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
                if (conf is { Name: string name })
                {
                    var list = await api.Clients.GetAllAsync(new GetClientsRequest() { Fields = "client_id,name" }, cancellationToken: cancellationToken);
                    var self = list.FirstOrDefault(i => i.Name == name);
                    return self?.ClientId;
                }
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
        protected override async Task Update(IManagementApiClient api, string id, V1ClientConf? last, V1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);

            // transform request
            var req = new ClientUpdateRequest();
            ApplyToApi(conf, req);

            // explicitely null out missing metadata if previously present
            if (last is not null && last.ClientMetaData != null && conf.ClientMetaData != null)
                foreach (string key in last.ClientMetaData.Keys)
                    if (conf.ClientMetaData.ContainsKey(key) == false)
                        req.ClientMetaData[key] = null;

            await api.Clients.UpdateAsync(id, req, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1Client entity, V1ClientConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // Always attempt to apply secret if secretRef is specified, regardless of whether we have the clientSecret value
            // This ensures secret resources are created for existing clients even when Auth0 API doesn't return the secret
            if (entity.Spec.SecretRef is not null && entity.Status.Id is not null)
            {
                var client = await api.Clients.GetAsync(entity.Status.Id, "client_id,client_secret", cancellationToken: cancellationToken);
                await ApplySecret(entity, client.ClientId, client.ClientSecret, defaultNamespace, cancellationToken);
            }

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
