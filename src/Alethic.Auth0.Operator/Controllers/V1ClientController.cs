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
using Auth0.ManagementApi.Core;

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
        internal static V1ClientConf? FromApi(GetClientResponseContent? source) => source is null ? null : new()
        {
            AllowedClients = source.AllowedClients?.ToArray(),
            AllowedLogoutUrls = source.AllowedLogoutUrls?.ToArray(),
            AllowedOrigins = source.AllowedOrigins?.ToArray(),
            WebOrigins = source.WebOrigins?.ToArray(),
            InitiateLoginUri = source.InitiateLoginUri,
            Callbacks = source.Callbacks?.ToArray(),
            ClientAliases = source.ClientAliases?.ToArray(),
            ClientMetaData = source.ClientMetadata,
            IsCustomLoginPageOn = source.CustomLoginPageOn,
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
            AddOns = FromApi(source.Addons),
            ApplicationType = FromApi(source.AppType),
            ComplianceLevel = source.ComplianceLevel.IsDefined ? FromApi(source.ComplianceLevel.Value) : null,
            DefaultOrganization = source.DefaultOrganization.IsDefined ? FromApi(source.DefaultOrganization.Value) : null,
            EncryptionKey = source.EncryptionKey.IsDefined ? FromApi(source.EncryptionKey.Value) : null,
            JwtConfiguration = FromApi(source.JwtConfiguration),
            Mobile = FromApi(source.Mobile),
            OidcLogout = FromApi(source.OidcLogout),
            OrganizationRequireBehavior = FromApi(source.OrganizationRequireBehavior),
            OrganizationUsage = FromApi(source.OrganizationUsage),
            RefreshToken = source.RefreshToken.IsDefined ? FromApi(source.RefreshToken.Value) : null,
            SigningKeys = source.SigningKeys.IsDefined ? source.SigningKeys.Value?.Select(i => FromApi(i)).ToArray() : null,
            TokenEndpointAuthMethod = FromApi(source.TokenEndpointAuthMethod),
        };

        /// <summary>
        /// Transforms the token endpoint authentication method from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientTokenEndpointAuthMethod? FromApi(ClientTokenEndpointAuthMethodEnum? source) => source?.Value switch
        {
            ClientTokenEndpointAuthMethodEnum.Values.None => V1ClientTokenEndpointAuthMethod.None,
            ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost => V1ClientTokenEndpointAuthMethod.ClientSecretPost,
            ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic => V1ClientTokenEndpointAuthMethod.ClientSecretBasic,
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
        internal static V1ClientSigningKey? FromApi(ClientSigningKey? source) => source is null ? null : new()
        {
            Cert = source.Cert,
            Pkcs7 = source.Pkcs7,
        };

        /// <summary>
        /// Transforms the refresh token configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientRefreshToken? FromApi(ClientRefreshTokenConfiguration? source) => source is null ? null : new()
        {
            ExpirationType = FromApi(source.ExpirationType),
            InfiniteIdleTokenLifetime = source.InfiniteIdleTokenLifetime,
            InfiniteTokenLifetime = source.InfiniteTokenLifetime,
            Leeway = source.Leeway,
            RotationType = FromApi(source.RotationType),
            TokenLifetime = source.TokenLifetime,
            IdleTokenLifetime = source.IdleTokenLifetime,
        };

        /// <summary>
        /// Transforms the refresh token rotation type from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientRefreshTokenRotationType? FromApi(RefreshTokenRotationTypeEnum? source) => source?.Value switch
        {
            RefreshTokenRotationTypeEnum.Values.Rotating => V1ClientRefreshTokenRotationType.Rotating,
            RefreshTokenRotationTypeEnum.Values.NonRotating => V1ClientRefreshTokenRotationType.NonRotating,
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
        internal static V1ClientRefreshTokenExpirationType? FromApi(RefreshTokenExpirationTypeEnum? source) => source?.Value switch
        {
            RefreshTokenExpirationTypeEnum.Values.Expiring => V1ClientRefreshTokenExpirationType.Expiring,
            RefreshTokenExpirationTypeEnum.Values.NonExpiring => V1ClientRefreshTokenExpirationType.NonExpiring,
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
        internal static V1ClientOrganizationUsage? FromApi(ClientOrganizationUsageEnum? source) => source?.Value switch
        {
            ClientOrganizationUsageEnum.Values.Deny => V1ClientOrganizationUsage.Deny,
            ClientOrganizationUsageEnum.Values.Allow => V1ClientOrganizationUsage.Allow,
            ClientOrganizationUsageEnum.Values.Require => V1ClientOrganizationUsage.Require,
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
        internal static V1ClientOrganizationRequireBehavior? FromApi(ClientOrganizationRequireBehaviorEnum? source) => source?.Value switch
        {
            ClientOrganizationRequireBehaviorEnum.Values.NoPrompt => V1ClientOrganizationRequireBehavior.NoPrompt,
            ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt => V1ClientOrganizationRequireBehavior.PreLoginPrompt,
            ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt => V1ClientOrganizationRequireBehavior.PostLoginPrompt,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the OIDC logout configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientOidcLogoutConfig? FromApi(ClientOidcBackchannelLogoutSettings? source) => source is null ? null : new()
        {
            BackchannelLogoutUrls = source.BackchannelLogoutUrls?.ToArray(),
            BackchannelLogoutInitiators = FromApi(source.BackchannelLogoutInitiators),
        };

        /// <summary>
        /// Transforms the backchannel logout initiators from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientBackchannelLogoutInitiators? FromApi(ClientOidcBackchannelLogoutInitiators? source) => source is null ? null : new()
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
        internal static V1ClientLogoutInitiators FromApi(ClientOidcBackchannelLogoutInitiatorsEnum source) => source.Value switch
        {
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout => V1ClientLogoutInitiators.RpLogout,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout => V1ClientLogoutInitiators.IdpLogout,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged => V1ClientLogoutInitiators.PasswordChanged,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired => V1ClientLogoutInitiators.SessionExpired,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the logout initiator modes from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientLogoutInitiatorModes? FromApi(ClientOidcBackchannelLogoutInitiatorsModeEnum? source) => source?.Value switch
        {
            ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All => V1ClientLogoutInitiatorModes.All,
            ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom => V1ClientLogoutInitiatorModes.Custom,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the mobile configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientMobile? FromApi(ClientMobile? source) => source is null ? null : new()
        {
            Android = FromApi(source.Android),
            Ios = FromApi(source.Ios),
        };

        /// <summary>
        /// Transforms the iOS mobile configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static V1ClientMobile.MobileIos? FromApi(ClientMobileiOs? source)
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
        internal static V1ClientMobile.MobileAndroid? FromApi(ClientMobileAndroid? source)
        {
            if (source is null)
                return null;
            if (source.AppPackageName is null)
                return null;

            return new()
            {
                AppPackageName = source.AppPackageName,
            };
        }

        /// <summary>
        /// Transforms the client JWT configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientJwtConfiguration? FromApi(ClientJwtConfiguration? source) => source is null ? null : new()
        {
            IsSecretEncoded = source.SecretEncoded,
            LifetimeInSeconds = source.LifetimeInSeconds,
            SigningAlgorithm = source.Alg?.Value,
        };

        /// <summary>
        /// Transforms the client encryption key configuration from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientEncryptionKey? FromApi(ClientEncryptionKey? source) => source is null ? null : new()
        {
            Certificate = source.Cert,
            PublicKey = source.Pub,
            Subject = source.Subject,
        };

        /// <summary>
        /// Extracts the default organization configuration from the API response and transforms it to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientDefaultOrganization? FromApi(ClientDefaultOrganization? source) => source is null ? null : new()
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
        internal static V1ClientFlows FromApi(ClientDefaultOrganizationFlowsEnum source) => source.Value switch
        {
            ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials => V1ClientFlows.ClientCredentials,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the compliance level from the API model to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientComplianceLevel? FromApi(ClientComplianceLevelEnum? source) => source?.Value switch
        {
            ClientComplianceLevelEnum.Values.None => V1ClientComplianceLevel.NONE,
            ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar => V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR,
            ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar => V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR,
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
        internal static V1ClientApplicationType? FromApi(ClientAppTypeEnum? source) => source?.Value switch
        {
            ClientAppTypeEnum.Values.Box => V1ClientApplicationType.Box,
            ClientAppTypeEnum.Values.Cloudbees => V1ClientApplicationType.Cloudbees,
            ClientAppTypeEnum.Values.Concur => V1ClientApplicationType.Concur,
            ClientAppTypeEnum.Values.Dropbox => V1ClientApplicationType.Dropbox,
            ClientAppTypeEnum.Values.Echosign => V1ClientApplicationType.Echosign,
            ClientAppTypeEnum.Values.Egnyte => V1ClientApplicationType.Egnyte,
            ClientAppTypeEnum.Values.Mscrm => V1ClientApplicationType.MsCrm,
            ClientAppTypeEnum.Values.Native => V1ClientApplicationType.Native,
            ClientAppTypeEnum.Values.Newrelic => V1ClientApplicationType.NewRelic,
            ClientAppTypeEnum.Values.NonInteractive => V1ClientApplicationType.NonInteractive,
            ClientAppTypeEnum.Values.Office365 => V1ClientApplicationType.Office365,
            ClientAppTypeEnum.Values.RegularWeb => V1ClientApplicationType.RegularWeb,
            ClientAppTypeEnum.Values.Rms => V1ClientApplicationType.Rms,
            ClientAppTypeEnum.Values.Salesforce => V1ClientApplicationType.Salesforce,
            ClientAppTypeEnum.Values.Sentry => V1ClientApplicationType.Sentry,
            ClientAppTypeEnum.Values.Sharepoint => V1ClientApplicationType.SharePoint,
            ClientAppTypeEnum.Values.Slack => V1ClientApplicationType.Slack,
            ClientAppTypeEnum.Values.Springcm => V1ClientApplicationType.SpringCm,
            ClientAppTypeEnum.Values.Spa => V1ClientApplicationType.Spa,
            ClientAppTypeEnum.Values.Zendesk => V1ClientApplicationType.Zendesk,
            ClientAppTypeEnum.Values.Zoom => V1ClientApplicationType.Zoom,
            ClientAppTypeEnum.Values.ResourceServer => V1ClientApplicationType.ResourceServer,
            ClientAppTypeEnum.Values.ExpressConfiguration => V1ClientApplicationType.ExpressConfiguration,
            ClientAppTypeEnum.Values.SsoIntegration => V1ClientApplicationType.SsoIntegration,
            ClientAppTypeEnum.Values.Oag => V1ClientApplicationType.Oag,
            null => null,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Extracts the add-ons configuration from the API response and transforms it to the operator model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ClientAddons? FromApi(ClientAddons? source) => source is { } addOns ? new V1ClientAddons()
        {
            Aws = FromApiAddonAws(addOns.Aws),
            AzureSb = FromApiAddonAzureSb(addOns.AzureSb),
            Box = addOns.Box,
            Cloudbees = addOns.Cloudbees,
            Concur = addOns.Concur,
            Dropbox = addOns.Dropbox,
            Echosign = FromApiAddonEchoSign(addOns.Echosign),
            Egnyte = FromApiAddonEgnyte(addOns.Egnyte),
            Firebase = FromApiAddonFirebase(addOns.Firebase),
            Newrelic = FromApiAddonNewRelic(addOns.Newrelic),
            Office365 = FromApiAddonOffice365(addOns.Office365),
            Salesforce = FromApiAddonSalesforce(addOns.Salesforce),
            SalesforceApi = FromApiAddonSalesforceApi(addOns.SalesforceApi),
            SalesforceSandboxApi = FromApiAddonSalesforceSandboxApi(addOns.SalesforceSandboxApi),
            Samlp = FromApiAddonSaml(addOns.Samlp),
            SapApi = FromApiAddonSapApi(addOns.SapApi),
            Sharepoint = FromApiAddonSharePoint(addOns.Sharepoint),
            Springcm = FromApiAddonSpringCm(addOns.Springcm),
            Wsfed = addOns.Wsfed,
            Zendesk = FromApiAddonZendesk(addOns.Zendesk),
            Zoom = FromApiAddonZoom(addOns.Zoom),
        } : null;

        internal static V1ClientAddonAws? FromApiAddonAws(ClientAddonAws? o) => o is null ? null : new()
        {
            Principal = o.Principal,
            Role = o.Role,
            LifetimeInSeconds = o.LifetimeInSeconds,
        };

        internal static V1ClientAddonAzureSb? FromApiAddonAzureSb(ClientAddonAzureSb? o) => o is null ? null : new()
        {
            Namespace = o.Namespace,
            SasKeyName = o.SasKeyName,
            SasKey = o.SasKey,
            EntityPath = o.EntityPath,
        };

        internal static V1ClientAddonEchoSign? FromApiAddonEchoSign(ClientAddonEchoSign? o) => o is null ? null : new()
        {
            Domain = o.Domain,
        };

        internal static V1ClientAddonEgnyte? FromApiAddonEgnyte(ClientAddonEgnyte? o) => o is null ? null : new()
        {
            Domain = o.Domain,
        };

        internal static V1ClientAddonFirebase? FromApiAddonFirebase(ClientAddonFirebase? o) => o is null ? null : new()
        {
            Secret = o.Secret,
            PrivateKeyId = o.PrivateKeyId,
            PrivateKey = o.PrivateKey,
            ClientEmail = o.ClientEmail,
            LifetimeInSeconds = o.LifetimeInSeconds,
        };

        internal static V1ClientAddonNewRelic? FromApiAddonNewRelic(ClientAddonNewRelic? o) => o is null ? null : new()
        {
            Account = o.Account,
        };

        internal static V1ClientAddonOffice365? FromApiAddonOffice365(ClientAddonOffice365? o) => o is null ? null : new()
        {
            Domain = o.Domain,
            Connection = o.Connection,
        };

        internal static V1ClientAddonSalesforce? FromApiAddonSalesforce(ClientAddonSalesforce? o) => o is null ? null : new()
        {
            EntityId = o.EntityId,
        };

        internal static V1ClientAddonSalesforceApi? FromApiAddonSalesforceApi(ClientAddonSalesforceApi? o) => o is null ? null : new()
        {
            Clientid = o.Clientid,
            Principal = o.Principal,
            CommunityName = o.CommunityName,
            CommunityUrlSection = o.CommunityUrlSection,
        };

        internal static V1ClientAddonSalesforceSandboxApi? FromApiAddonSalesforceSandboxApi(ClientAddonSalesforceSandboxApi? o) => o is null ? null : new()
        {
            Clientid = o.Clientid,
            Principal = o.Principal,
            CommunityName = o.CommunityName,
            CommunityUrlSection = o.CommunityUrlSection,
        };

        internal static V1ClientAddonSaml? FromApiAddonSaml(ClientAddonSaml? o) => o is null ? null : new()
        {
            Mappings = o.Mappings,
            Audience = o.Audience,
            Recipient = o.Recipient,
            CreateUpnClaim = o.CreateUpnClaim,
            MapUnknownClaimsAsIs = o.MapUnknownClaimsAsIs,
            PassthroughClaimsWithNoMapping = o.PassthroughClaimsWithNoMapping,
            MapIdentities = o.MapIdentities,
            SignatureAlgorithm = o.SignatureAlgorithm,
            DigestAlgorithm = o.DigestAlgorithm,
            Issuer = o.Issuer,
            Destination = o.Destination,
            LifetimeInSeconds = o.LifetimeInSeconds,
            SignResponse = o.SignResponse,
            NameIdentifierFormat = o.NameIdentifierFormat,
            NameIdentifierProbes = o.NameIdentifierProbes?.ToList(),
            AuthnContextClassRef = o.AuthnContextClassRef,
        };

        internal static V1ClientAddonSapapi? FromApiAddonSapApi(ClientAddonSapapi? o) => o is null ? null : new()
        {
            Clientid = o.Clientid,
            UsernameAttribute = o.UsernameAttribute,
            TokenEndpointUrl = o.TokenEndpointUrl,
            Scope = o.Scope,
            ServicePassword = o.ServicePassword,
            NameIdentifierFormat = o.NameIdentifierFormat,
        };

        internal static V1ClientAddonSharePoint? FromApiAddonSharePoint(ClientAddonSharePoint? o) => o is null ? null : new()
        {
            Url = o.Url,
            ExternalUrl = o.ExternalUrl is { } eu ? new V1ClientAddonSharePointExternalUrl { Type = eu.Type, Value = eu.Value?.ToString() } : null,
        };

        internal static V1ClientAddonSpringCm? FromApiAddonSpringCm(ClientAddonSpringCm? o) => o is null ? null : new()
        {
            Acsurl = o.Acsurl,
        };

        internal static V1ClientAddonZendesk? FromApiAddonZendesk(ClientAddonZendesk? o) => o is null ? null : new()
        {
            AccountName = o.AccountName,
        };

        internal static V1ClientAddonZoom? FromApiAddonZoom(ClientAddonZoom? o) => o is null ? null : new()
        {
            Account = o.Account,
        };

        /// <summary>
        /// Applies the add-ons configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientAddons source, ClientAddons target)
        {
            if (source.Aws is { } aws)
            {
                target.Aws ??= new ClientAddonAws();
                target.Aws.Principal = aws.Principal;
                target.Aws.Role = aws.Role;
                target.Aws.LifetimeInSeconds = aws.LifetimeInSeconds;
            }

            if (source.AzureSb is { } azureSb)
            {
                target.AzureSb ??= new ClientAddonAzureSb();
                target.AzureSb.Namespace = azureSb.Namespace;
                target.AzureSb.SasKeyName = azureSb.SasKeyName;
                target.AzureSb.SasKey = azureSb.SasKey;
                target.AzureSb.EntityPath = azureSb.EntityPath;
            }

            if (source.Box is { } box)
                target.Box = box;

            if (source.Cloudbees is { } cloudbees)
                target.Cloudbees = cloudbees;

            if (source.Concur is { } concur)
                target.Concur = concur;

            if (source.Dropbox is { } dropbox)
                target.Dropbox = dropbox;

            if (source.Echosign is { } echosign)
            {
                target.Echosign ??= new ClientAddonEchoSign();
                target.Echosign.Domain = echosign.Domain;
            }

            if (source.Egnyte is { } egnyte)
            {
                target.Egnyte ??= new ClientAddonEgnyte();
                target.Egnyte.Domain = egnyte.Domain;
            }

            if (source.Firebase is { } firebase)
            {
                target.Firebase ??= new ClientAddonFirebase();
                target.Firebase.Secret = firebase.Secret;
                target.Firebase.PrivateKeyId = firebase.PrivateKeyId;
                target.Firebase.PrivateKey = firebase.PrivateKey;
                target.Firebase.ClientEmail = firebase.ClientEmail;
                target.Firebase.LifetimeInSeconds = firebase.LifetimeInSeconds;
            }

            if (source.Newrelic is { } newrelic)
            {
                target.Newrelic ??= new ClientAddonNewRelic();
                target.Newrelic.Account = newrelic.Account;
            }

            if (source.Office365 is { } office365)
            {
                target.Office365 ??= new ClientAddonOffice365();
                target.Office365.Domain = office365.Domain;
                target.Office365.Connection = office365.Connection;
            }

            if (source.Salesforce is { } salesforce)
            {
                target.Salesforce ??= new ClientAddonSalesforce();
                target.Salesforce.EntityId = salesforce.EntityId;
            }

            if (source.SalesforceApi is { } salesforceApi)
            {
                target.SalesforceApi ??= new ClientAddonSalesforceApi();
                target.SalesforceApi.Clientid = salesforceApi.Clientid;
                target.SalesforceApi.Principal = salesforceApi.Principal;
                target.SalesforceApi.CommunityName = salesforceApi.CommunityName;
                target.SalesforceApi.CommunityUrlSection = salesforceApi.CommunityUrlSection;
            }

            if (source.SalesforceSandboxApi is { } salesforceSandboxApi)
            {
                target.SalesforceSandboxApi ??= new ClientAddonSalesforceSandboxApi();
                target.SalesforceSandboxApi.Clientid = salesforceSandboxApi.Clientid;
                target.SalesforceSandboxApi.Principal = salesforceSandboxApi.Principal;
                target.SalesforceSandboxApi.CommunityName = salesforceSandboxApi.CommunityName;
                target.SalesforceSandboxApi.CommunityUrlSection = salesforceSandboxApi.CommunityUrlSection;
            }

            if (source.Samlp is { } samlp)
            {
                target.Samlp ??= new ClientAddonSaml();
                target.Samlp.Mappings = samlp.Mappings;
                target.Samlp.Audience = samlp.Audience;
                target.Samlp.Recipient = samlp.Recipient;
                target.Samlp.CreateUpnClaim = samlp.CreateUpnClaim;
                target.Samlp.MapUnknownClaimsAsIs = samlp.MapUnknownClaimsAsIs;
                target.Samlp.PassthroughClaimsWithNoMapping = samlp.PassthroughClaimsWithNoMapping;
                target.Samlp.MapIdentities = samlp.MapIdentities;
                target.Samlp.SignatureAlgorithm = samlp.SignatureAlgorithm;
                target.Samlp.DigestAlgorithm = samlp.DigestAlgorithm;
                target.Samlp.Issuer = samlp.Issuer;
                target.Samlp.Destination = samlp.Destination;
                target.Samlp.LifetimeInSeconds = samlp.LifetimeInSeconds;
                target.Samlp.SignResponse = samlp.SignResponse;
                target.Samlp.NameIdentifierFormat = samlp.NameIdentifierFormat;
                target.Samlp.NameIdentifierProbes = samlp.NameIdentifierProbes;
                target.Samlp.AuthnContextClassRef = samlp.AuthnContextClassRef;
            }

            if (source.SapApi is { } sapApi)
            {
                target.SapApi ??= new ClientAddonSapapi();
                target.SapApi.Clientid = sapApi.Clientid;
                target.SapApi.UsernameAttribute = sapApi.UsernameAttribute;
                target.SapApi.TokenEndpointUrl = sapApi.TokenEndpointUrl;
                target.SapApi.Scope = sapApi.Scope;
                target.SapApi.ServicePassword = sapApi.ServicePassword;
                target.SapApi.NameIdentifierFormat = sapApi.NameIdentifierFormat;
            }

            if (source.Sharepoint is { } sharepoint)
            {
                target.Sharepoint ??= new ClientAddonSharePoint();
                target.Sharepoint.Url = sharepoint.Url;
            }

            if (source.Springcm is { } springcm)
            {
                target.Springcm ??= new ClientAddonSpringCm();
                target.Springcm.Acsurl = springcm.Acsurl;
            }

            if (source.Wsfed is { } wsfed)
                target.Wsfed = wsfed;

            if (source.Zendesk is { } zendesk)
            {
                target.Zendesk ??= new ClientAddonZendesk();
                target.Zendesk.AccountName = zendesk.AccountName;
            }

            if (source.Zoom is { } zoom)
            {
                target.Zoom ??= new ClientAddonZoom();
                target.Zoom.Account = zoom.Account;
            }
        }

        /// <summary>
        /// Applies the encryption key configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientEncryptionKey source, ClientEncryptionKey target)
        {
            if (source.Certificate is { } cert)
                target.Cert = cert;

            if (source.PublicKey is { } pub)
                target.Pub = pub;

            if (source.Subject is { } subject)
                target.Subject = subject;
        }

        /// <summary>
        /// Applies the JWT configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientJwtConfiguration source, ClientJwtConfiguration target)
        {
            if (source.IsSecretEncoded is { } secret_encoded)
                target.SecretEncoded = secret_encoded;

            if (source.LifetimeInSeconds is { } lifetime_in_seconds)
                target.LifetimeInSeconds = lifetime_in_seconds;

            if (source.SigningAlgorithm is { } alg)
                target.Alg = new SigningAlgorithmEnum(alg);
        }

        /// <summary>
        /// Applies the Android mobile configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientMobile.MobileAndroid source, ClientMobileAndroid target)
        {
            if (source.AppPackageName is { } app_package_name)
                target.AppPackageName = app_package_name;
        }

        /// <summary>
        /// Applies the iOS mobile configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientMobile.MobileIos source, ClientMobileiOs target)
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
        internal static void ApplyToApi(V1ClientMobile source, ClientMobile target)
        {
            if (source.Android is { } android)
                if (source.Android.AppPackageName is not null)
                    ApplyToApi(android, target.Android ??= new ClientMobileAndroid());

            if (source.Ios is { } ios)
                if (source.Ios.AppBundleIdentifier is not null || source.Ios.TeamId is not null)
                    ApplyToApi(ios, target.Ios ??= new ClientMobileiOs());
        }

        /// <summary>
        /// Transforms the compliance level from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientComplianceLevelEnum ToApi(V1ClientComplianceLevel source) => source switch
        {
            V1ClientComplianceLevel.NONE => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.None),
            V1ClientComplianceLevel.FAPI1_ADV_PKJ_PAR => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar),
            V1ClientComplianceLevel.FAPI1_ADV_MTLS_PAR => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the organization require behavior from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientOrganizationRequireBehaviorEnum ToApi(V1ClientOrganizationRequireBehavior source) => source switch
        {
            V1ClientOrganizationRequireBehavior.NoPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.NoPrompt),
            V1ClientOrganizationRequireBehavior.PreLoginPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt),
            V1ClientOrganizationRequireBehavior.PostLoginPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the organization usage from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientOrganizationUsageEnum ToApi(V1ClientOrganizationUsage source) => source switch
        {
            V1ClientOrganizationUsage.Deny => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Deny),
            V1ClientOrganizationUsage.Allow => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Allow),
            V1ClientOrganizationUsage.Require => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationUsagePatchEnum ToApiPatch(V1ClientOrganizationUsage source) => source switch
        {
            V1ClientOrganizationUsage.Deny => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Deny),
            V1ClientOrganizationUsage.Allow => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Allow),
            V1ClientOrganizationUsage.Require => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Require),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationRequireBehaviorPatchEnum ToApiPatch(V1ClientOrganizationRequireBehavior source) => source switch
        {
            V1ClientOrganizationRequireBehavior.NoPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.NoPrompt),
            V1ClientOrganizationRequireBehavior.PreLoginPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.PreLoginPrompt),
            V1ClientOrganizationRequireBehavior.PostLoginPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.PostLoginPrompt),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the refresh token rotation type from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static RefreshTokenRotationTypeEnum ToApi(V1ClientRefreshTokenRotationType source) => source switch
        {
            V1ClientRefreshTokenRotationType.Rotating => new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating),
            V1ClientRefreshTokenRotationType.NonRotating => new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the refresh token expiration type from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static RefreshTokenExpirationTypeEnum ToApi(V1ClientRefreshTokenExpirationType source) => source switch
        {
            V1ClientRefreshTokenExpirationType.Expiring => new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring),
            V1ClientRefreshTokenExpirationType.NonExpiring => new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Applies the refresh token configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientRefreshToken source, ClientRefreshTokenConfiguration target)
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
        internal static ClientOidcBackchannelLogoutInitiatorsModeEnum ToApi(V1ClientLogoutInitiatorModes source) => source switch
        {
            V1ClientLogoutInitiatorModes.All => new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All),
            V1ClientLogoutInitiatorModes.Custom => new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the logout initiators from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientOidcBackchannelLogoutInitiatorsEnum ToApi(V1ClientLogoutInitiators source) => source switch
        {
            V1ClientLogoutInitiators.RpLogout => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout),
            V1ClientLogoutInitiators.IdpLogout => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout),
            V1ClientLogoutInitiators.PasswordChanged => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged),
            V1ClientLogoutInitiators.SessionExpired => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Applies the backchannel logout initiators configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientBackchannelLogoutInitiators source, ClientOidcBackchannelLogoutInitiators target)
        {
            if (source.Mode is { } mode)
                target.Mode = ToApi(mode);

            if (source.SelectedInitiators is { } selected)
                target.SelectedInitiators = [.. selected.Select(ToApi)];
        }

        /// <summary>
        /// Applies the OIDC logout configuration to the API request.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        internal static void ApplyToApi(V1ClientOidcLogoutConfig source, ClientOidcBackchannelLogoutSettings target)
        {
            if (source.BackchannelLogoutUrls is { } backchannel_logout_urls)
                target.BackchannelLogoutUrls = backchannel_logout_urls;

            if (source.BackchannelLogoutInitiators is { } backchannel_logout_initiators)
                ApplyToApi(backchannel_logout_initiators, target.BackchannelLogoutInitiators ??= new());
        }

        /// <summary>
        /// Transforms the application type from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientAppTypeEnum ToApi(V1ClientApplicationType source) => source switch
        {
            V1ClientApplicationType.Box => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Box),
            V1ClientApplicationType.Cloudbees => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Cloudbees),
            V1ClientApplicationType.Concur => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Concur),
            V1ClientApplicationType.Dropbox => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Dropbox),
            V1ClientApplicationType.Echosign => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Echosign),
            V1ClientApplicationType.Egnyte => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Egnyte),
            V1ClientApplicationType.MsCrm => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Mscrm),
            V1ClientApplicationType.Native => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Native),
            V1ClientApplicationType.NewRelic => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Newrelic),
            V1ClientApplicationType.NonInteractive => new ClientAppTypeEnum(ClientAppTypeEnum.Values.NonInteractive),
            V1ClientApplicationType.Office365 => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Office365),
            V1ClientApplicationType.RegularWeb => new ClientAppTypeEnum(ClientAppTypeEnum.Values.RegularWeb),
            V1ClientApplicationType.Rms => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Rms),
            V1ClientApplicationType.Salesforce => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Salesforce),
            V1ClientApplicationType.Sentry => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sentry),
            V1ClientApplicationType.SharePoint => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sharepoint),
            V1ClientApplicationType.Slack => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Slack),
            V1ClientApplicationType.SpringCm => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Springcm),
            V1ClientApplicationType.Spa => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Spa),
            V1ClientApplicationType.Zendesk => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zendesk),
            V1ClientApplicationType.Zoom => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zoom),
            V1ClientApplicationType.ResourceServer => new ClientAppTypeEnum(ClientAppTypeEnum.Values.ResourceServer),
            V1ClientApplicationType.ExpressConfiguration => new ClientAppTypeEnum(ClientAppTypeEnum.Values.ExpressConfiguration),
            V1ClientApplicationType.SsoIntegration => new ClientAppTypeEnum(ClientAppTypeEnum.Values.SsoIntegration),
            V1ClientApplicationType.Oag => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Oag),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Transforms the token endpoint authentication method from the operator model to the API model.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ClientTokenEndpointAuthMethodEnum ToApi(V1ClientTokenEndpointAuthMethod source) => source switch
        {
            V1ClientTokenEndpointAuthMethod.None => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.None),
            V1ClientTokenEndpointAuthMethod.ClientSecretPost => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost),
            V1ClientTokenEndpointAuthMethod.ClientSecretBasic => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic),
            _ => throw new NotImplementedException(),
        };

        internal static ClientTokenEndpointAuthMethodOrNullEnum ToApiOrNull(V1ClientTokenEndpointAuthMethod source) => source switch
        {
            V1ClientTokenEndpointAuthMethod.None => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.None),
            V1ClientTokenEndpointAuthMethod.ClientSecretPost => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.ClientSecretPost),
            V1ClientTokenEndpointAuthMethod.ClientSecretBasic => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.ClientSecretBasic),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Applies the common configuration to either a create or update request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        internal static void ApplyToApiBase(V1ClientConf conf, CreateClientRequestContent request)
        {
            if (conf.AddOns is { } addons)
                ApplyToApi(addons, request.Addons ??= new());

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
                request.ClientMetadata = conf.ClientMetaData;

            if (conf.IsCustomLoginPageOn is not null)
                request.CustomLoginPageOn = conf.IsCustomLoginPageOn;

            if (conf.IsFirstParty is not null)
                request.IsFirstParty = conf.IsFirstParty;

            if (conf.CustomLoginPage is not null)
                request.CustomLoginPage = conf.CustomLoginPage;

            if (conf.CustomLoginPagePreview is not null)
                request.CustomLoginPagePreview = conf.CustomLoginPagePreview;

            if (conf.EncryptionKey is { } encryption_key)
            {
                var ek = new ClientEncryptionKey();
                ApplyToApi(encryption_key, ek);
                request.EncryptionKey = ek;
            }

            if (conf.FormTemplate is not null)
                request.FormTemplate = conf.FormTemplate;

            if (conf.GrantTypes is not null)
                request.GrantTypes = conf.GrantTypes.Distinct().ToArray();

            if (conf.JwtConfiguration is { } jwt_configuration)
                ApplyToApi(jwt_configuration, request.JwtConfiguration ??= new());

            if (conf.Mobile is { } mobile)
            {
                var target = new ClientMobile();
                ApplyToApi(mobile, target);
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
            {
                var rt = new ClientRefreshTokenConfiguration
                {
                    RotationType = refresh_token.RotationType is { } rtype ? ToApi(rtype) : new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
                    ExpirationType = refresh_token.ExpirationType is { } etype ? ToApi(etype) : new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
                };
                ApplyToApi(refresh_token, rt);
                request.RefreshToken = rt;
            }

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
        /// Applies the common configuration to an update request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        internal static void ApplyToApiBase(V1ClientConf conf, UpdateClientRequestContent request)
        {
            if (conf.AddOns is { } addons)
                ApplyToApi(addons, request.Addons ??= new());

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
                request.ClientMetadata = conf.ClientMetaData;

            if (conf.IsCustomLoginPageOn is not null)
                request.CustomLoginPageOn = conf.IsCustomLoginPageOn;

            if (conf.IsFirstParty is not null)
                request.IsFirstParty = conf.IsFirstParty;

            if (conf.CustomLoginPage is not null)
                request.CustomLoginPage = conf.CustomLoginPage;

            if (conf.CustomLoginPagePreview is not null)
                request.CustomLoginPagePreview = conf.CustomLoginPagePreview;

            if (conf.EncryptionKey is { } encryption_key)
            {
                var ek = new ClientEncryptionKey();
                ApplyToApi(encryption_key, ek);
                request.EncryptionKey = ek;
            }

            if (conf.FormTemplate is not null)
                request.FormTemplate = conf.FormTemplate;

            if (conf.GrantTypes is not null)
                request.GrantTypes = conf.GrantTypes.Distinct().ToArray();

            if (conf.JwtConfiguration is { } jwt_configuration)
                ApplyToApi(jwt_configuration, request.JwtConfiguration ??= new());

            if (conf.Mobile is { } mobile)
            {
                var target = new ClientMobile();
                ApplyToApi(mobile, target);
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
            {
                var rt = new ClientRefreshTokenConfiguration
                {
                    RotationType = refresh_token.RotationType is { } rtype ? ToApi(rtype) : new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
                    ExpirationType = refresh_token.ExpirationType is { } etype ? ToApi(etype) : new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
                };
                ApplyToApi(refresh_token, rt);
                request.RefreshToken = rt;
            }

            if (conf.OrganizationUsage is { } organization_usage)
                request.OrganizationUsage = ToApiPatch(organization_usage);

            if (conf.OrganizationRequireBehavior is { } organization_require_behavior)
                request.OrganizationRequireBehavior = ToApiPatch(organization_require_behavior);

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
        /// Applies client configuration settings to the specified API client creation request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        internal static void ApplyToApi(V1ClientConf conf, CreateClientRequestContent request)
        {
            if (conf.ApplicationType is { } app_type)
                request.AppType = ToApi(app_type);

            if (conf.TokenEndpointAuthMethod is { } token_endpoint_auth_method)
                request.TokenEndpointAuthMethod = ToApi(token_endpoint_auth_method);

            ApplyToApiBase(conf, request);
        }

        /// <summary>
        /// Applies the specified client configuration to the API update request.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="request"></param>
        internal static void ApplyToApi(V1ClientConf conf, UpdateClientRequestContent request)
        {
            if (conf.ApplicationType is { } app_type)
                request.AppType = ToApi(app_type);

            if (conf.TokenEndpointAuthMethod is { } token_endpoint_auth_method)
                request.TokenEndpointAuthMethod = ToApiOrNull(token_endpoint_auth_method);

            ApplyToApiBase(conf, request);
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
                return FromApi(await api.Clients.GetAsync(id, new GetClientRequestParameters(), null, cancellationToken));
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
                        var client = await api.Clients.GetAsync(clientId, new GetClientRequestParameters { Fields = "client_id,name" }, null, cancellationToken);
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
                    var pager = await api.Clients.ListAsync(new ListClientsRequestParameters { Fields = "client_id,name" }, null, cancellationToken);
                    var self = pager.CurrentPage.Items.FirstOrDefault(i => i.Name == name);
                    return self?.ClientId;
                }
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is { Name: string name })
                {
                    var pager = await api.Clients.ListAsync(new ListClientsRequestParameters { Fields = "client_id,name" }, null, cancellationToken);
                    var self = pager.CurrentPage.Items.FirstOrDefault(i => i.Name == name);
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

            var req = new CreateClientRequestContent() { Name = conf.Name ?? throw new InvalidOperationException("Missing client name.") };
            ApplyToApi(conf, req);

            var self = await api.Clients.CreateAsync(req, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created client in Auth0 with ID: {ClientId} and name: {ClientName}", EntityTypeName, self.ClientId, conf.Name);
            return self.ClientId;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ClientConf? last, V1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);

            var req = new UpdateClientRequestContent();
            ApplyToApi(conf, req);

            // explicitly null out missing metadata if previously present
            if (last is not null && last.ClientMetaData != null && conf.ClientMetaData != null)
                foreach (string key in last.ClientMetaData.Keys)
                    if (conf.ClientMetaData.ContainsKey(key) == false)
                        (req.ClientMetadata ??= new Dictionary<string, object?>())[key] = null;

            await api.Clients.UpdateAsync(id, req, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);
        }

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V1Client entity, V1ClientConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // Always attempt to apply secret if secretRef is specified, regardless of whether we have the clientSecret value
            // This ensures secret resources are created for existing clients even when Auth0 API doesn't return the secret
            if (entity.Spec.SecretRef is not null && entity.Status.Id is not null)
            {
                var client = await api.Clients.GetAsync(entity.Status.Id, new GetClientRequestParameters { Fields = "client_id,client_secret" }, null, cancellationToken);
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
            await api.Clients.DeleteAsync(id, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted client from Auth0 with ID: {ClientId}", EntityTypeName, id);
        }

    }

}
