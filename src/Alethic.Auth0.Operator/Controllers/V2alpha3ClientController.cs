using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;

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

    [EntityRbac(typeof(V2alpha3Client), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3ClientController :
        V1TenantEntityInstanceController<V2alpha3Client, V2alpha3Client.SpecDef, V2alpha3Client.StatusDef, V2alpha3ClientConf, V2alpha3ClientConf>,
        IEntityController<V2alpha3Client>
    {

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientConf? FromApi(GetClientResponseContent? source) => source is null ? null : new()
        {
            AllowedClients = source.AllowedClients?.ToArray(),
            AllowedLogoutUrls = source.AllowedLogoutUrls?.ToArray(),
            AllowedOrigins = source.AllowedOrigins?.ToArray(),
            WebOrigins = source.WebOrigins?.ToArray(),
            InitiateLoginUri = source.InitiateLoginUri,
            Callbacks = source.Callbacks?.ToArray(),
            SkipNonVerifiableCallbackUriConfirmationPrompt = source.SkipNonVerifiableCallbackUriConfirmationPrompt,
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
            AddOns = source.Addons is { } addons ? FromApi(addons) : null,
            ApplicationType = FromApi(source.AppType),
            ComplianceLevel = source.ComplianceLevel.IsDefined && source.ComplianceLevel.Value is { } complianceLevel ? FromApi(complianceLevel) : null,
            DefaultOrganization = source.DefaultOrganization.IsDefined && source.DefaultOrganization.Value is { } defaultOrganization ? FromApi(defaultOrganization) : null,
            EncryptionKey = source.EncryptionKey.IsDefined && source.EncryptionKey.Value is { } encryptionKey ? FromApi(encryptionKey) : null,
            JwtConfiguration = source.JwtConfiguration is { } jwtConfiguration ? FromApi(jwtConfiguration) : null,
            Mobile = source.Mobile is { } mobile ? FromApi(mobile) : null,
            OidcLogout = source.OidcLogout is { } oidcLogout ? FromApi(oidcLogout) : null,
            OrganizationRequireBehavior = FromApi(source.OrganizationRequireBehavior),
            OrganizationUsage = FromApi(source.OrganizationUsage),
            RefreshToken = source.RefreshToken.IsDefined && source.RefreshToken.Value is { } refreshToken ? FromApi(refreshToken) : null,
            SigningKeys = source.SigningKeys.IsDefined ? source.SigningKeys.Value?.Select(FromApi).ToArray() : null,
            TokenEndpointAuthMethod = FromApi(source.TokenEndpointAuthMethod),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientTokenEndpointAuthMethodEnum? FromApi(ClientTokenEndpointAuthMethodEnum? source) => source?.Value switch
        {
            ClientTokenEndpointAuthMethodEnum.Values.None => V2alpha3ClientTokenEndpointAuthMethodEnum.None,
            ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost => V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
            ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic => V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretBasic,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientSigningKey? FromApi(ClientSigningKey? source) => source is null ? null : new()
        {
            Cert = source.Cert,
            Pkcs7 = source.Pkcs7,
            Subject = source.Subject,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientRefreshTokenConfiguration? FromApi(ClientRefreshTokenConfiguration? source) => source is null ? null : new()
        {
            ExpirationType = FromApi(source.ExpirationType),
            InfiniteIdleTokenLifetime = source.InfiniteIdleTokenLifetime,
            InfiniteTokenLifetime = source.InfiniteTokenLifetime,
            Leeway = source.Leeway,
            RotationType = FromApi(source.RotationType),
            TokenLifetime = source.TokenLifetime,
            IdleTokenLifetime = source.IdleTokenLifetime,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientRefreshTokenRotationTypeEnum? FromApi(RefreshTokenRotationTypeEnum? source) => source?.Value switch
        {
            RefreshTokenRotationTypeEnum.Values.Rotating => V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating,
            RefreshTokenRotationTypeEnum.Values.NonRotating => V2alpha3ClientRefreshTokenRotationTypeEnum.NonRotating,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientRefreshTokenExpirationTypeEnum? FromApi(RefreshTokenExpirationTypeEnum? source) => source?.Value switch
        {
            RefreshTokenExpirationTypeEnum.Values.Expiring => V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring,
            RefreshTokenExpirationTypeEnum.Values.NonExpiring => V2alpha3ClientRefreshTokenExpirationTypeEnum.NonExpiring,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientOrganizationUsageEnum? FromApi(ClientOrganizationUsageEnum? source) => source?.Value switch
        {
            ClientOrganizationUsageEnum.Values.Deny => V2alpha3ClientOrganizationUsageEnum.Deny,
            ClientOrganizationUsageEnum.Values.Allow => V2alpha3ClientOrganizationUsageEnum.Allow,
            ClientOrganizationUsageEnum.Values.Require => V2alpha3ClientOrganizationUsageEnum.Require,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientOrganizationRequireBehaviorEnum? FromApi(ClientOrganizationRequireBehaviorEnum? source) => source?.Value switch
        {
            ClientOrganizationRequireBehaviorEnum.Values.NoPrompt => V2alpha3ClientOrganizationRequireBehaviorEnum.NoPrompt,
            ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt => V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt,
            ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt => V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientOidcBackchannelLogoutSettings? FromApi(ClientOidcBackchannelLogoutSettings? source) => source is null ? null : new()
        {
            BackchannelLogoutUrls = source.BackchannelLogoutUrls?.ToArray(),
            BackchannelLogoutInitiators = source.BackchannelLogoutInitiators is { } initiators ? FromApi(initiators) : null,
            BackchannelLogoutSessionMetadata = source.BackchannelLogoutSessionMetadata.IsDefined && source.BackchannelLogoutSessionMetadata.Value is { } sessionMetadata
                ? FromApi(sessionMetadata)
                : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientOidcBackchannelLogoutSessionMetadata? FromApi(ClientOidcBackchannelLogoutSessionMetadata? source) => source is null ? null : new()
        {
            Include = source.Include,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientOidcBackchannelLogoutInitiators? FromApi(ClientOidcBackchannelLogoutInitiators? source) => source is null ? null : new()
        {
            Mode = FromApi(source.Mode),
            SelectedInitiators = source.SelectedInitiators?.Select(FromApi).ToArray(),
        };

        internal static V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum FromApi(ClientOidcBackchannelLogoutInitiatorsEnum source) => source.Value switch
        {
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout => V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout => V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.IdpLogout,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged => V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.PasswordChanged,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired => V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.SessionExpired,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum? FromApi(ClientOidcBackchannelLogoutInitiatorsModeEnum? source) => source?.Value switch
        {
            ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All => V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.All,
            ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom => V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientMobile? FromApi(ClientMobile? source) => source is null ? null : new()
        {
            Android = source.Android is { } android ? FromApi(android) : null,
            Ios = source.Ios is { } ios ? FromApi(ios) : null,
        };

        internal static V2alpha3ClientMobileiOs? FromApi(ClientMobileiOs? source)
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

        internal static V2alpha3ClientMobileAndroid? FromApi(ClientMobileAndroid? source)
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

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientJwtConfiguration? FromApi(ClientJwtConfiguration? source) => source is null ? null : new()
        {
            SecretEncoded = source.SecretEncoded,
            LifetimeInSeconds = source.LifetimeInSeconds,
            Alg = FromApi(source.Alg),
            Scopes = source.Scopes,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientSigningAlgorithmEnum? FromApi(SigningAlgorithmEnum? source) => source?.Value switch
        {
            SigningAlgorithmEnum.Values.Hs256 => V2alpha3ClientSigningAlgorithmEnum.Hs256,
            SigningAlgorithmEnum.Values.Rs256 => V2alpha3ClientSigningAlgorithmEnum.Rs256,
            SigningAlgorithmEnum.Values.Rs512 => V2alpha3ClientSigningAlgorithmEnum.Rs512,
            SigningAlgorithmEnum.Values.Ps256 => V2alpha3ClientSigningAlgorithmEnum.Ps256,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientEncryptionKey? FromApi(ClientEncryptionKey? source) => source is null ? null : new()
        {
            Cert = source.Cert,
            Pub = source.Pub,
            Subject = source.Subject,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientDefaultOrganization? FromApi(ClientDefaultOrganization? source) => source is null ? null : new()
        {
            OrganizationId = source.OrganizationId,
            Flows = source.Flows?.Select(FromApi).ToArray(),
        };

        internal static V2alpha3ClientDefaultOrganizationFlowsEnum FromApi(ClientDefaultOrganizationFlowsEnum source) => source.Value switch
        {
            ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials => V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientComplianceLevelEnum? FromApi(ClientComplianceLevelEnum? source) => source?.Value switch
        {
            ClientComplianceLevelEnum.Values.None => V2alpha3ClientComplianceLevelEnum.None,
            ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar => V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar,
            ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar => V2alpha3ClientComplianceLevelEnum.Fapi1AdvMtlsPar,
            ClientComplianceLevelEnum.Values.Fapi2SpPkjMtls => V2alpha3ClientComplianceLevelEnum.Fapi2SpPkjMtls,
            ClientComplianceLevelEnum.Values.Fapi2SpMtlsMtls => V2alpha3ClientComplianceLevelEnum.Fapi2SpMtlsMtls,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAppTypeEnum? FromApi(ClientAppTypeEnum? source) => source?.Value switch
        {
            ClientAppTypeEnum.Values.Box => V2alpha3ClientAppTypeEnum.Box,
            ClientAppTypeEnum.Values.Cloudbees => V2alpha3ClientAppTypeEnum.Cloudbees,
            ClientAppTypeEnum.Values.Concur => V2alpha3ClientAppTypeEnum.Concur,
            ClientAppTypeEnum.Values.Dropbox => V2alpha3ClientAppTypeEnum.Dropbox,
            ClientAppTypeEnum.Values.Echosign => V2alpha3ClientAppTypeEnum.Echosign,
            ClientAppTypeEnum.Values.Egnyte => V2alpha3ClientAppTypeEnum.Egnyte,
            ClientAppTypeEnum.Values.Mscrm => V2alpha3ClientAppTypeEnum.Mscrm,
            ClientAppTypeEnum.Values.Native => V2alpha3ClientAppTypeEnum.Native,
            ClientAppTypeEnum.Values.Newrelic => V2alpha3ClientAppTypeEnum.Newrelic,
            ClientAppTypeEnum.Values.NonInteractive => V2alpha3ClientAppTypeEnum.NonInteractive,
            ClientAppTypeEnum.Values.Office365 => V2alpha3ClientAppTypeEnum.Office365,
            ClientAppTypeEnum.Values.RegularWeb => V2alpha3ClientAppTypeEnum.RegularWeb,
            ClientAppTypeEnum.Values.Rms => V2alpha3ClientAppTypeEnum.Rms,
            ClientAppTypeEnum.Values.Salesforce => V2alpha3ClientAppTypeEnum.Salesforce,
            ClientAppTypeEnum.Values.Sentry => V2alpha3ClientAppTypeEnum.Sentry,
            ClientAppTypeEnum.Values.Sharepoint => V2alpha3ClientAppTypeEnum.Sharepoint,
            ClientAppTypeEnum.Values.Slack => V2alpha3ClientAppTypeEnum.Slack,
            ClientAppTypeEnum.Values.Springcm => V2alpha3ClientAppTypeEnum.Springcm,
            ClientAppTypeEnum.Values.Spa => V2alpha3ClientAppTypeEnum.Spa,
            ClientAppTypeEnum.Values.Zendesk => V2alpha3ClientAppTypeEnum.Zendesk,
            ClientAppTypeEnum.Values.Zoom => V2alpha3ClientAppTypeEnum.Zoom,
            ClientAppTypeEnum.Values.ResourceServer => V2alpha3ClientAppTypeEnum.ResourceServer,
            ClientAppTypeEnum.Values.ExpressConfiguration => V2alpha3ClientAppTypeEnum.ExpressConfiguration,
            ClientAppTypeEnum.Values.SsoIntegration => V2alpha3ClientAppTypeEnum.SsoIntegration,
            ClientAppTypeEnum.Values.Oag => V2alpha3ClientAppTypeEnum.Oag,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddons? FromApi(ClientAddons? source) => source is null ? null : new()
        {
            Aws = source.Aws is { } aws ? FromApi(aws) : null,
            AzureBlob = source.AzureBlob is { } azureBlob ? FromApi(azureBlob) : null,
            AzureSb = source.AzureSb is { } azureSb ? FromApi(azureSb) : null,
            Rms = source.Rms is { } rms ? FromApi(rms) : null,
            Mscrm = source.Mscrm is { } mscrm ? FromApi(mscrm) : null,
            Slack = source.Slack is { } slack ? FromApi(slack) : null,
            Sentry = source.Sentry is { } sentry ? FromApi(sentry) : null,
            Box = source.Box,
            Cloudbees = source.Cloudbees,
            Concur = source.Concur,
            Dropbox = source.Dropbox,
            Echosign = source.Echosign is { } echosign ? FromApi(echosign) : null,
            Egnyte = source.Egnyte is { } egnyte ? FromApi(egnyte) : null,
            Firebase = source.Firebase is { } firebase ? FromApi(firebase) : null,
            Newrelic = source.Newrelic is { } newrelic ? FromApi(newrelic) : null,
            Office365 = source.Office365 is { } office365 ? FromApi(office365) : null,
            Salesforce = source.Salesforce is { } salesforce ? FromApi(salesforce) : null,
            SalesforceApi = source.SalesforceApi is { } salesforceApi ? FromApi(salesforceApi) : null,
            SalesforceSandboxApi = source.SalesforceSandboxApi is { } salesforceSandboxApi ? FromApi(salesforceSandboxApi) : null,
            Samlp = source.Samlp is { } samlp ? FromApi(samlp) : null,
            Layer = source.Layer is { } layer ? FromApi(layer) : null,
            SapApi = source.SapApi is { } sapApi ? FromApi(sapApi) : null,
            Sharepoint = source.Sharepoint is { } sharepoint ? FromApi(sharepoint) : null,
            Springcm = source.Springcm is { } springcm ? FromApi(springcm) : null,
            Wams = source.Wams is { } wams ? FromApi(wams) : null,
            Wsfed = source.Wsfed,
            Zendesk = source.Zendesk is { } zendesk ? FromApi(zendesk) : null,
            Zoom = source.Zoom is { } zoom ? FromApi(zoom) : null,
            SsoIntegration = source.SsoIntegration is { } ssoIntegration ? FromApi(ssoIntegration) : null,
            Oag = source.Oag.IsDefined && source.Oag.Value is { } oag ? FromApi(oag) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonAws? FromApi(ClientAddonAws? source) => source is null ? null : new()
        {
            Principal = source.Principal,
            Role = source.Role,
            LifetimeInSeconds = source.LifetimeInSeconds,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonAzureBlob? FromApi(ClientAddonAzureBlob? source) => source is null ? null : new()
        {
            AccountName = source.AccountName,
            StorageAccessKey = source.StorageAccessKey,
            ContainerName = source.ContainerName,
            BlobName = source.BlobName,
            Expiration = source.Expiration,
            SignedIdentifier = source.SignedIdentifier,
            BlobRead = source.BlobRead,
            BlobWrite = source.BlobWrite,
            BlobDelete = source.BlobDelete,
            ContainerRead = source.ContainerRead,
            ContainerWrite = source.ContainerWrite,
            ContainerDelete = source.ContainerDelete,
            ContainerList = source.ContainerList,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonAzureSb? FromApi(ClientAddonAzureSb? source) => source is null ? null : new()
        {
            Namespace = source.Namespace,
            SasKeyName = source.SasKeyName,
            SasKey = source.SasKey,
            EntityPath = source.EntityPath,
            Expiration = source.Expiration,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonRms? FromApi(ClientAddonRms? source) => source is null ? null : new()
        {
            Url = source.Url,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonMscrm? FromApi(ClientAddonMscrm? source) => source is null ? null : new()
        {
            Url = source.Url,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSlack? FromApi(ClientAddonSlack? source) => source is null ? null : new()
        {
            Team = source.Team,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSentry? FromApi(ClientAddonSentry? source) => source is null ? null : new()
        {
            OrgSlug = source.OrgSlug,
            BaseUrl = source.BaseUrl,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonEchoSign? FromApi(ClientAddonEchoSign? source) => source is null ? null : new()
        {
            Domain = source.Domain,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonEgnyte? FromApi(ClientAddonEgnyte? source) => source is null ? null : new()
        {
            Domain = source.Domain,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonFirebase? FromApi(ClientAddonFirebase? source) => source is null ? null : new()
        {
            Secret = source.Secret,
            PrivateKeyId = source.PrivateKeyId,
            PrivateKey = source.PrivateKey,
            ClientEmail = source.ClientEmail,
            LifetimeInSeconds = source.LifetimeInSeconds,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonNewRelic? FromApi(ClientAddonNewRelic? source) => source is null ? null : new()
        {
            Account = source.Account,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonOffice365? FromApi(ClientAddonOffice365? source) => source is null ? null : new()
        {
            Domain = source.Domain,
            Connection = source.Connection,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSalesforce? FromApi(ClientAddonSalesforce? source) => source is null ? null : new()
        {
            EntityId = source.EntityId,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSalesforceApi? FromApi(ClientAddonSalesforceApi? source) => source is null ? null : new()
        {
            Clientid = source.Clientid,
            Principal = source.Principal,
            CommunityName = source.CommunityName,
            CommunityUrlSection = source.CommunityUrlSection,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSalesforceSandboxApi? FromApi(ClientAddonSalesforceSandboxApi? source) => source is null ? null : new()
        {
            Clientid = source.Clientid,
            Principal = source.Principal,
            CommunityName = source.CommunityName,
            CommunityUrlSection = source.CommunityUrlSection,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSaml? FromApi(ClientAddonSaml? source) => source is null ? null : new()
        {
            Mappings = source.Mappings,
            Audience = source.Audience,
            Recipient = source.Recipient,
            CreateUpnClaim = source.CreateUpnClaim,
            MapUnknownClaimsAsIs = source.MapUnknownClaimsAsIs,
            PassthroughClaimsWithNoMapping = source.PassthroughClaimsWithNoMapping,
            MapIdentities = source.MapIdentities,
            SignatureAlgorithm = source.SignatureAlgorithm,
            DigestAlgorithm = source.DigestAlgorithm,
            Issuer = source.Issuer,
            Destination = source.Destination,
            LifetimeInSeconds = source.LifetimeInSeconds,
            SignResponse = source.SignResponse,
            NameIdentifierFormat = source.NameIdentifierFormat,
            NameIdentifierProbes = source.NameIdentifierProbes?.ToArray(),
            AuthnContextClassRef = source.AuthnContextClassRef,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonLayer? FromApi(ClientAddonLayer? source) => source is null ? null : new()
        {
            ProviderId = source.ProviderId,
            KeyId = source.KeyId,
            PrivateKey = source.PrivateKey,
            Principal = source.Principal,
            Expiration = source.Expiration,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSapapi? FromApi(ClientAddonSapapi? source) => source is null ? null : new()
        {
            Clientid = source.Clientid,
            UsernameAttribute = source.UsernameAttribute,
            TokenEndpointUrl = source.TokenEndpointUrl,
            Scope = source.Scope,
            ServicePassword = source.ServicePassword,
            NameIdentifierFormat = source.NameIdentifierFormat,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSharePoint? FromApi(ClientAddonSharePoint? source) => source is null ? null : new()
        {
            Url = source.Url,
            ExternalUrl = source.ExternalUrl is { } externalUrl ? FromApi(externalUrl) : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static string[]? FromApi(ClientAddonSharePointExternalUrl? source)
        {
            if (source is null)
                return null;

            if (source.TryGetListOfString(out var values))
                return values?.ToArray();

            if (source.TryGetString(out var value))
                return value is null ? null : [value];

            throw new NotImplementedException();
        }

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSpringCm? FromApi(ClientAddonSpringCm? source) => source is null ? null : new()
        {
            Acsurl = source.Acsurl,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonWams? FromApi(ClientAddonWams? source) => source is null ? null : new()
        {
            Masterkey = source.Masterkey,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonZendesk? FromApi(ClientAddonZendesk? source) => source is null ? null : new()
        {
            AccountName = source.AccountName,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonZoom? FromApi(ClientAddonZoom? source) => source is null ? null : new()
        {
            Account = source.Account,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonSsoIntegration? FromApi(ClientAddonSsoIntegration? source) => source is null ? null : new()
        {
            Name = source.Name,
            Version = source.Version,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ClientAddonOag? FromApi(ClientAddonOag? source) => source is null ? null : new();

        internal static ClientTokenEndpointAuthMethodEnum ToApi(V2alpha3ClientTokenEndpointAuthMethodEnum source) => source switch
        {
            V2alpha3ClientTokenEndpointAuthMethodEnum.None => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.None),
            V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost),
            V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretBasic => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic),
            _ => throw new NotImplementedException(),
        };

        internal static ClientTokenEndpointAuthMethodOrNullEnum ToApiOrNull(V2alpha3ClientTokenEndpointAuthMethodEnum source) => source switch
        {
            V2alpha3ClientTokenEndpointAuthMethodEnum.None => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.None),
            V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.ClientSecretPost),
            V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretBasic => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.ClientSecretBasic),
            _ => throw new NotImplementedException(),
        };

        internal static ClientAppTypeEnum ToApi(V2alpha3ClientAppTypeEnum source) => source switch
        {
            V2alpha3ClientAppTypeEnum.Box => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Box),
            V2alpha3ClientAppTypeEnum.Cloudbees => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Cloudbees),
            V2alpha3ClientAppTypeEnum.Concur => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Concur),
            V2alpha3ClientAppTypeEnum.Dropbox => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Dropbox),
            V2alpha3ClientAppTypeEnum.Echosign => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Echosign),
            V2alpha3ClientAppTypeEnum.Egnyte => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Egnyte),
            V2alpha3ClientAppTypeEnum.Mscrm => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Mscrm),
            V2alpha3ClientAppTypeEnum.Native => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Native),
            V2alpha3ClientAppTypeEnum.Newrelic => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Newrelic),
            V2alpha3ClientAppTypeEnum.NonInteractive => new ClientAppTypeEnum(ClientAppTypeEnum.Values.NonInteractive),
            V2alpha3ClientAppTypeEnum.Office365 => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Office365),
            V2alpha3ClientAppTypeEnum.RegularWeb => new ClientAppTypeEnum(ClientAppTypeEnum.Values.RegularWeb),
            V2alpha3ClientAppTypeEnum.Rms => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Rms),
            V2alpha3ClientAppTypeEnum.Salesforce => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Salesforce),
            V2alpha3ClientAppTypeEnum.Sentry => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sentry),
            V2alpha3ClientAppTypeEnum.Sharepoint => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sharepoint),
            V2alpha3ClientAppTypeEnum.Slack => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Slack),
            V2alpha3ClientAppTypeEnum.Springcm => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Springcm),
            V2alpha3ClientAppTypeEnum.Spa => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Spa),
            V2alpha3ClientAppTypeEnum.Zendesk => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zendesk),
            V2alpha3ClientAppTypeEnum.Zoom => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zoom),
            V2alpha3ClientAppTypeEnum.ResourceServer => new ClientAppTypeEnum(ClientAppTypeEnum.Values.ResourceServer),
            V2alpha3ClientAppTypeEnum.ExpressConfiguration => new ClientAppTypeEnum(ClientAppTypeEnum.Values.ExpressConfiguration),
            V2alpha3ClientAppTypeEnum.SsoIntegration => new ClientAppTypeEnum(ClientAppTypeEnum.Values.SsoIntegration),
            V2alpha3ClientAppTypeEnum.Oag => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Oag),
            _ => throw new NotImplementedException(),
        };

        internal static SigningAlgorithmEnum ToApi(V2alpha3ClientSigningAlgorithmEnum source) => source switch
        {
            V2alpha3ClientSigningAlgorithmEnum.Hs256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Hs256),
            V2alpha3ClientSigningAlgorithmEnum.Rs256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256),
            V2alpha3ClientSigningAlgorithmEnum.Rs512 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs512),
            V2alpha3ClientSigningAlgorithmEnum.Ps256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Ps256),
            _ => throw new NotImplementedException(),
        };

        internal static ClientComplianceLevelEnum ToApi(V2alpha3ClientComplianceLevelEnum source) => source switch
        {
            V2alpha3ClientComplianceLevelEnum.None => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.None),
            V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar),
            V2alpha3ClientComplianceLevelEnum.Fapi1AdvMtlsPar => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar),
            V2alpha3ClientComplianceLevelEnum.Fapi2SpPkjMtls => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi2SpPkjMtls),
            V2alpha3ClientComplianceLevelEnum.Fapi2SpMtlsMtls => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi2SpMtlsMtls),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationRequireBehaviorEnum ToApi(V2alpha3ClientOrganizationRequireBehaviorEnum source) => source switch
        {
            V2alpha3ClientOrganizationRequireBehaviorEnum.NoPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.NoPrompt),
            V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt),
            V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationRequireBehaviorPatchEnum ToApiPatch(V2alpha3ClientOrganizationRequireBehaviorEnum source) => source switch
        {
            V2alpha3ClientOrganizationRequireBehaviorEnum.NoPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.NoPrompt),
            V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.PreLoginPrompt),
            V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.PostLoginPrompt),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationUsageEnum ToApi(V2alpha3ClientOrganizationUsageEnum source) => source switch
        {
            V2alpha3ClientOrganizationUsageEnum.Deny => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Deny),
            V2alpha3ClientOrganizationUsageEnum.Allow => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Allow),
            V2alpha3ClientOrganizationUsageEnum.Require => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationUsagePatchEnum ToApiPatch(V2alpha3ClientOrganizationUsageEnum source) => source switch
        {
            V2alpha3ClientOrganizationUsageEnum.Deny => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Deny),
            V2alpha3ClientOrganizationUsageEnum.Allow => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Allow),
            V2alpha3ClientOrganizationUsageEnum.Require => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Require),
            _ => throw new NotImplementedException(),
        };

        internal static RefreshTokenRotationTypeEnum ToApi(V2alpha3ClientRefreshTokenRotationTypeEnum source) => source switch
        {
            V2alpha3ClientRefreshTokenRotationTypeEnum.Rotating => new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating),
            V2alpha3ClientRefreshTokenRotationTypeEnum.NonRotating => new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
            _ => throw new NotImplementedException(),
        };

        internal static RefreshTokenExpirationTypeEnum ToApi(V2alpha3ClientRefreshTokenExpirationTypeEnum source) => source switch
        {
            V2alpha3ClientRefreshTokenExpirationTypeEnum.Expiring => new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring),
            V2alpha3ClientRefreshTokenExpirationTypeEnum.NonExpiring => new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOidcBackchannelLogoutInitiatorsModeEnum ToApi(V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum source) => source switch
        {
            V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.All => new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All),
            V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom => new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOidcBackchannelLogoutInitiatorsEnum ToApi(V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum source) => source switch
        {
            V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout),
            V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.IdpLogout => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout),
            V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.PasswordChanged => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged),
            V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum.SessionExpired => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired),
            _ => throw new NotImplementedException(),
        };

        static ClientOidcBackchannelLogoutSessionMetadata ToApi(V2alpha3ClientOidcBackchannelLogoutSessionMetadata source) => new()
        {
            Include = source.Include,
        };

        static ClientDefaultOrganization ToApi(V2alpha3ClientDefaultOrganization source) => new()
        {
            OrganizationId = source.OrganizationId,
            Flows = source.Flows?.Select(ToApi).ToArray(),
        };

        static ClientDefaultOrganizationFlowsEnum ToApi(V2alpha3ClientDefaultOrganizationFlowsEnum source) => source switch
        {
            V2alpha3ClientDefaultOrganizationFlowsEnum.ClientCredentials => new ClientDefaultOrganizationFlowsEnum(ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials),
            _ => throw new NotImplementedException(),
        };

        internal static ClientAddons ToApi(V2alpha3ClientAddons source) => new()
        {
            Aws = source.Aws is { } aws ? ToApi(aws) : null,
            AzureBlob = source.AzureBlob is { } azureBlob ? ToApi(azureBlob) : null,
            AzureSb = source.AzureSb is { } azureSb ? ToApi(azureSb) : null,
            Rms = source.Rms is { } rms ? ToApi(rms) : null,
            Mscrm = source.Mscrm is { } mscrm ? ToApi(mscrm) : null,
            Slack = source.Slack is { } slack ? ToApi(slack) : null,
            Sentry = source.Sentry is { } sentry ? ToApi(sentry) : null,
            Box = source.Box,
            Cloudbees = source.Cloudbees,
            Concur = source.Concur,
            Dropbox = source.Dropbox,
            Echosign = source.Echosign is { } echosign ? ToApi(echosign) : null,
            Egnyte = source.Egnyte is { } egnyte ? ToApi(egnyte) : null,
            Firebase = source.Firebase is { } firebase ? ToApi(firebase) : null,
            Newrelic = source.Newrelic is { } newrelic ? ToApi(newrelic) : null,
            Office365 = source.Office365 is { } office365 ? ToApi(office365) : null,
            Salesforce = source.Salesforce is { } salesforce ? ToApi(salesforce) : null,
            SalesforceApi = source.SalesforceApi is { } salesforceApi ? ToApi(salesforceApi) : null,
            SalesforceSandboxApi = source.SalesforceSandboxApi is { } salesforceSandboxApi ? ToApi(salesforceSandboxApi) : null,
            Samlp = source.Samlp is { } samlp ? ToApi(samlp) : null,
            Layer = source.Layer is { } layer ? ToApi(layer) : null,
            SapApi = source.SapApi is { } sapApi ? ToApi(sapApi) : null,
            Sharepoint = source.Sharepoint is { } sharepoint ? ToApi(sharepoint) : null,
            Springcm = source.Springcm is { } springcm ? ToApi(springcm) : null,
            Wams = source.Wams is { } wams ? ToApi(wams) : null,
            Wsfed = source.Wsfed,
            Zendesk = source.Zendesk is { } zendesk ? ToApi(zendesk) : null,
            Zoom = source.Zoom is { } zoom ? ToApi(zoom) : null,
            SsoIntegration = source.SsoIntegration is { } ssoIntegration ? ToApi(ssoIntegration) : null,
            Oag = source.Oag is { } oag ? ToApi(oag) : null,
        };

        static ClientAddonAws ToApi(V2alpha3ClientAddonAws source) => new()
        {
            Principal = source.Principal,
            Role = source.Role,
            LifetimeInSeconds = source.LifetimeInSeconds,
        };

        static ClientAddonAzureBlob ToApi(V2alpha3ClientAddonAzureBlob source) => new()
        {
            AccountName = source.AccountName,
            StorageAccessKey = source.StorageAccessKey,
            ContainerName = source.ContainerName,
            BlobName = source.BlobName,
            Expiration = source.Expiration,
            SignedIdentifier = source.SignedIdentifier,
            BlobRead = source.BlobRead,
            BlobWrite = source.BlobWrite,
            BlobDelete = source.BlobDelete,
            ContainerRead = source.ContainerRead,
            ContainerWrite = source.ContainerWrite,
            ContainerDelete = source.ContainerDelete,
            ContainerList = source.ContainerList,
        };

        static ClientAddonAzureSb ToApi(V2alpha3ClientAddonAzureSb source) => new()
        {
            Namespace = source.Namespace,
            SasKeyName = source.SasKeyName,
            SasKey = source.SasKey,
            EntityPath = source.EntityPath,
            Expiration = source.Expiration,
        };

        static ClientAddonRms ToApi(V2alpha3ClientAddonRms source) => new()
        {
            Url = source.Url,
        };

        static ClientAddonMscrm ToApi(V2alpha3ClientAddonMscrm source) => new()
        {
            Url = source.Url,
        };

        static ClientAddonSlack ToApi(V2alpha3ClientAddonSlack source) => new()
        {
            Team = source.Team,
        };

        static ClientAddonSentry ToApi(V2alpha3ClientAddonSentry source) => new()
        {
            OrgSlug = source.OrgSlug,
            BaseUrl = source.BaseUrl,
        };

        static ClientAddonEchoSign ToApi(V2alpha3ClientAddonEchoSign source) => new()
        {
            Domain = source.Domain,
        };

        static ClientAddonEgnyte ToApi(V2alpha3ClientAddonEgnyte source) => new()
        {
            Domain = source.Domain,
        };

        static ClientAddonFirebase ToApi(V2alpha3ClientAddonFirebase source) => new()
        {
            Secret = source.Secret,
            PrivateKeyId = source.PrivateKeyId,
            PrivateKey = source.PrivateKey,
            ClientEmail = source.ClientEmail,
            LifetimeInSeconds = source.LifetimeInSeconds,
        };

        static ClientAddonNewRelic ToApi(V2alpha3ClientAddonNewRelic source) => new()
        {
            Account = source.Account,
        };

        static ClientAddonOffice365 ToApi(V2alpha3ClientAddonOffice365 source) => new()
        {
            Domain = source.Domain,
            Connection = source.Connection,
        };

        static ClientAddonSalesforce ToApi(V2alpha3ClientAddonSalesforce source) => new()
        {
            EntityId = source.EntityId,
        };

        static ClientAddonSalesforceApi ToApi(V2alpha3ClientAddonSalesforceApi source) => new()
        {
            Clientid = source.Clientid,
            Principal = source.Principal,
            CommunityName = source.CommunityName,
            CommunityUrlSection = source.CommunityUrlSection,
        };

        static ClientAddonSalesforceSandboxApi ToApi(V2alpha3ClientAddonSalesforceSandboxApi source) => new()
        {
            Clientid = source.Clientid,
            Principal = source.Principal,
            CommunityName = source.CommunityName,
            CommunityUrlSection = source.CommunityUrlSection,
        };

        static ClientAddonSaml ToApi(V2alpha3ClientAddonSaml source) => new()
        {
            Mappings = source.Mappings,
            Audience = source.Audience,
            Recipient = source.Recipient,
            CreateUpnClaim = source.CreateUpnClaim,
            MapUnknownClaimsAsIs = source.MapUnknownClaimsAsIs,
            PassthroughClaimsWithNoMapping = source.PassthroughClaimsWithNoMapping,
            MapIdentities = source.MapIdentities,
            SignatureAlgorithm = source.SignatureAlgorithm,
            DigestAlgorithm = source.DigestAlgorithm,
            Issuer = source.Issuer,
            Destination = source.Destination,
            LifetimeInSeconds = source.LifetimeInSeconds,
            SignResponse = source.SignResponse,
            NameIdentifierFormat = source.NameIdentifierFormat,
            NameIdentifierProbes = source.NameIdentifierProbes,
            AuthnContextClassRef = source.AuthnContextClassRef,
        };

        static ClientAddonLayer ToApi(V2alpha3ClientAddonLayer source) => new()
        {
            ProviderId = source.ProviderId,
            KeyId = source.KeyId,
            PrivateKey = source.PrivateKey,
            Principal = source.Principal,
            Expiration = source.Expiration,
        };

        static ClientAddonSapapi ToApi(V2alpha3ClientAddonSapapi source) => new()
        {
            Clientid = source.Clientid,
            UsernameAttribute = source.UsernameAttribute,
            TokenEndpointUrl = source.TokenEndpointUrl,
            Scope = source.Scope,
            ServicePassword = source.ServicePassword,
            NameIdentifierFormat = source.NameIdentifierFormat,
        };

        static ClientAddonSharePoint ToApi(V2alpha3ClientAddonSharePoint source)
        {
            var target = new ClientAddonSharePoint
            {
                Url = source.Url,
            };

            if (source.ExternalUrl is not null)
            {
                target.ExternalUrl = ToApi(source.ExternalUrl);
            }

            return target;
        }

        static ClientAddonSharePointExternalUrl ToApi(string[] source)
        {
            return ClientAddonSharePointExternalUrl.FromListOfString(source);
        }

        static ClientAddonSpringCm ToApi(V2alpha3ClientAddonSpringCm source) => new()
        {
            Acsurl = source.Acsurl,
        };

        static ClientAddonWams ToApi(V2alpha3ClientAddonWams source) => new()
        {
            Masterkey = source.Masterkey,
        };

        static ClientAddonZendesk ToApi(V2alpha3ClientAddonZendesk source) => new()
        {
            AccountName = source.AccountName,
        };

        static ClientAddonZoom ToApi(V2alpha3ClientAddonZoom source) => new()
        {
            Account = source.Account,
        };

        static ClientAddonSsoIntegration ToApi(V2alpha3ClientAddonSsoIntegration source) => new()
        {
            Name = source.Name,
            Version = source.Version,
        };

        static ClientAddonOag ToApi(V2alpha3ClientAddonOag source) => new();

        internal static void ApplyToApi(V2alpha3ClientRefreshTokenConfiguration source, ClientRefreshTokenConfiguration target)
        {
            if (source.RotationType is { } rotationType)
                target.RotationType = ToApi(rotationType);

            if (source.ExpirationType is { } expirationType)
                target.ExpirationType = ToApi(expirationType);

            if (source.Leeway is { } leeway)
                target.Leeway = leeway;

            if (source.TokenLifetime is { } tokenLifetime)
                target.TokenLifetime = tokenLifetime;

            if (source.InfiniteTokenLifetime is { } infiniteTokenLifetime)
                target.InfiniteTokenLifetime = infiniteTokenLifetime;

            if (source.IdleTokenLifetime is { } idleTokenLifetime)
                target.IdleTokenLifetime = idleTokenLifetime;

            if (source.InfiniteIdleTokenLifetime is { } infiniteIdleTokenLifetime)
                target.InfiniteIdleTokenLifetime = infiniteIdleTokenLifetime;
        }

        internal static void ApplyToApi(V2alpha3ClientOidcBackchannelLogoutInitiators source, ClientOidcBackchannelLogoutInitiators target)
        {
            if (source.Mode is { } mode)
                target.Mode = ToApi(mode);

            if (source.SelectedInitiators is { } selected)
                target.SelectedInitiators = [.. selected.Select(ToApi)];
        }

        internal static void ApplyToApi(V2alpha3ClientOidcBackchannelLogoutSettings source, ClientOidcBackchannelLogoutSettings target)
        {
            if (source.BackchannelLogoutUrls is { } backchannelLogoutUrls)
                target.BackchannelLogoutUrls = backchannelLogoutUrls;

            if (source.BackchannelLogoutInitiators is { } initiators)
                ApplyToApi(initiators, target.BackchannelLogoutInitiators ??= new());

            if (source.BackchannelLogoutSessionMetadata is { } sessionMetadata)
                target.BackchannelLogoutSessionMetadata = ToApi(sessionMetadata);
        }

        internal static void ApplyToApi(V2alpha3ClientEncryptionKey source, ClientEncryptionKey target)
        {
            if (source.Cert is { } cert)
                target.Cert = cert;

            if (source.Pub is { } pub)
                target.Pub = pub;

            if (source.Subject is { } subject)
                target.Subject = subject;
        }

        internal static void ApplyToApi(V2alpha3ClientJwtConfiguration source, ClientJwtConfiguration target)
        {
            if (source.SecretEncoded is { } secretEncoded)
                target.SecretEncoded = secretEncoded;

            if (source.LifetimeInSeconds is { } lifetimeInSeconds)
                target.LifetimeInSeconds = lifetimeInSeconds;

            if (source.Alg is { } alg)
                target.Alg = ToApi(alg);

            if (source.Scopes is { } scopes)
                target.Scopes = scopes;
        }

        internal static void ApplyToApi(V2alpha3ClientMobileAndroid source, ClientMobileAndroid target)
        {
            if (source.AppPackageName is { } appPackageName)
                target.AppPackageName = appPackageName;
        }

        internal static void ApplyToApi(V2alpha3ClientMobileiOs source, ClientMobileiOs target)
        {
            if (source.AppBundleIdentifier is { } appBundleIdentifier)
                target.AppBundleIdentifier = appBundleIdentifier;

            if (source.TeamId is { } teamId)
                target.TeamId = teamId;
        }

        internal static void ApplyToApi(V2alpha3ClientMobile source, ClientMobile target)
        {
            if (source.Android is { } android && android.AppPackageName is not null)
                ApplyToApi(android, target.Android ??= new ClientMobileAndroid());

            if (source.Ios is { } ios && (ios.AppBundleIdentifier is not null || ios.TeamId is not null))
                ApplyToApi(ios, target.Ios ??= new ClientMobileiOs());
        }

        internal static void ApplyToApi(V2alpha3ClientConf conf, CreateClientRequestContent request)
        {
            if (conf.ApplicationType is { } appType)
                request.AppType = ToApi(appType);

            if (conf.TokenEndpointAuthMethod is { } tokenEndpointAuthMethod)
                request.TokenEndpointAuthMethod = ToApi(tokenEndpointAuthMethod);

            ApplyToApiBase(conf, request);
        }

        internal static void ApplyToApi(V2alpha3ClientConf conf, UpdateClientRequestContent request)
        {
            if (conf.ApplicationType is { } appType)
                request.AppType = ToApi(appType);

            if (conf.TokenEndpointAuthMethod is { } tokenEndpointAuthMethod)
                request.TokenEndpointAuthMethod = ToApiOrNull(tokenEndpointAuthMethod);

            ApplyToApiBase(conf, request);
        }

        internal static void ApplyToApiBase(V2alpha3ClientConf conf, CreateClientRequestContent request)
        {
            if (conf.AddOns is { } addons)
                request.Addons = ToApi(addons);

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

            if (conf.SkipNonVerifiableCallbackUriConfirmationPrompt is not null)
                request.SkipNonVerifiableCallbackUriConfirmationPrompt = conf.SkipNonVerifiableCallbackUriConfirmationPrompt;

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

            if (conf.EncryptionKey is { } encryptionKey)
            {
                var target = new ClientEncryptionKey();
                ApplyToApi(encryptionKey, target);
                request.EncryptionKey = target;
            }

            if (conf.FormTemplate is not null)
                request.FormTemplate = conf.FormTemplate;

            if (conf.GrantTypes is not null)
                request.GrantTypes = conf.GrantTypes.Distinct().ToArray();

            if (conf.JwtConfiguration is { } jwtConfiguration)
                ApplyToApi(jwtConfiguration, request.JwtConfiguration ??= new());

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

            if (conf.OidcLogout is { } oidcLogout)
                ApplyToApi(oidcLogout, request.OidcLogout ??= new());

            if (conf.Sso is not null)
                request.Sso = conf.Sso;

            if (conf.RefreshToken is { } refreshToken)
            {
                var target = new ClientRefreshTokenConfiguration
                {
                    RotationType = refreshToken.RotationType is { } rotationType ? ToApi(rotationType) : new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
                    ExpirationType = refreshToken.ExpirationType is { } expirationType ? ToApi(expirationType) : new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
                };
                ApplyToApi(refreshToken, target);
                request.RefreshToken = target;
            }

            if (conf.OrganizationUsage is { } organizationUsage)
                request.OrganizationUsage = ToApi(organizationUsage);

            if (conf.OrganizationRequireBehavior is { } organizationRequireBehavior)
                request.OrganizationRequireBehavior = ToApi(organizationRequireBehavior);

            if (conf.CrossOriginAuthentication is not null)
                request.CrossOriginAuthentication = conf.CrossOriginAuthentication;

            if (conf.RequirePushedAuthorizationRequests is not null)
                request.RequirePushedAuthorizationRequests = conf.RequirePushedAuthorizationRequests;

            if (conf.DefaultOrganization is { } defaultOrganization)
                request.DefaultOrganization = ToApi(defaultOrganization);

            if (conf.ComplianceLevel is { } complianceLevel)
                request.ComplianceLevel = ToApi(complianceLevel);

            if (conf.RequireProofOfPossession is not null)
                request.RequireProofOfPossession = conf.RequireProofOfPossession;
        }

        internal static void ApplyToApiBase(V2alpha3ClientConf conf, UpdateClientRequestContent request)
        {
            if (conf.AddOns is { } addons)
                request.Addons = ToApi(addons);

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

            if (conf.SkipNonVerifiableCallbackUriConfirmationPrompt is not null)
                request.SkipNonVerifiableCallbackUriConfirmationPrompt = conf.SkipNonVerifiableCallbackUriConfirmationPrompt;

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

            if (conf.EncryptionKey is { } encryptionKey)
            {
                var target = new ClientEncryptionKey();
                ApplyToApi(encryptionKey, target);
                request.EncryptionKey = target;
            }

            if (conf.FormTemplate is not null)
                request.FormTemplate = conf.FormTemplate;

            if (conf.GrantTypes is not null)
                request.GrantTypes = conf.GrantTypes.Distinct().ToArray();

            if (conf.JwtConfiguration is { } jwtConfiguration)
                ApplyToApi(jwtConfiguration, request.JwtConfiguration ??= new());

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

            if (conf.OidcLogout is { } oidcLogout)
                ApplyToApi(oidcLogout, request.OidcLogout ??= new());

            if (conf.Sso is not null)
                request.Sso = conf.Sso;

            if (conf.RefreshToken is { } refreshToken)
            {
                var target = new ClientRefreshTokenConfiguration
                {
                    RotationType = refreshToken.RotationType is { } rotationType ? ToApi(rotationType) : new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
                    ExpirationType = refreshToken.ExpirationType is { } expirationType ? ToApi(expirationType) : new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
                };
                ApplyToApi(refreshToken, target);
                request.RefreshToken = target;
            }

            if (conf.OrganizationUsage is { } organizationUsage)
                request.OrganizationUsage = ToApiPatch(organizationUsage);

            if (conf.OrganizationRequireBehavior is { } organizationRequireBehavior)
                request.OrganizationRequireBehavior = ToApiPatch(organizationRequireBehavior);

            if (conf.CrossOriginAuthentication is not null)
                request.CrossOriginAuthentication = conf.CrossOriginAuthentication;

            if (conf.RequirePushedAuthorizationRequests is not null)
                request.RequirePushedAuthorizationRequests = conf.RequirePushedAuthorizationRequests;

            if (conf.DefaultOrganization is { } defaultOrganization)
                request.DefaultOrganization = ToApi(defaultOrganization);

            if (conf.ComplianceLevel is { } complianceLevel)
                request.ComplianceLevel = ToApi(complianceLevel);

            if (conf.RequireProofOfPossession is not null)
                request.RequireProofOfPossession = conf.RequireProofOfPossession;
        }

        public V2alpha3ClientController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V2alpha3ClientController> logger) :
            base(kube, cache, options, logger)
        {
        }

        protected override string EntityTypeName => "Client";

        protected override async Task<V2alpha3ClientConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
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

        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3Client entity, V2alpha3Client.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
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

        protected override string? ValidateCreate(V2alpha3ClientConf conf)
        {
            if (conf.ApplicationType == null)
                return "missing a value for application type";

            return null;
        }

        protected override async Task<string> Create(IManagementApiClient api, V2alpha3ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating client in Auth0 with name: {ClientName}", EntityTypeName, conf.Name);

            var req = new CreateClientRequestContent { Name = conf.Name ?? throw new InvalidOperationException("Missing client name.") };
            ApplyToApi(conf, req);

            var self = await api.Clients.CreateAsync(req, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created client in Auth0 with ID: {ClientId} and name: {ClientName}", EntityTypeName, self.ClientId, conf.Name);
            return self.ClientId;
        }

        protected override async Task Update(IManagementApiClient api, string id, V2alpha3ClientConf? last, V2alpha3ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);

            var req = new UpdateClientRequestContent();
            ApplyToApi(conf, req);

            if (last is not null && last.ClientMetaData != null && conf.ClientMetaData != null)
                foreach (string key in last.ClientMetaData.Keys)
                    if (conf.ClientMetaData.ContainsKey(key) == false)
                        (req.ClientMetadata ??= new Dictionary<string, object?>())[key] = null;

            await api.Clients.UpdateAsync(id, req, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully updated client in Auth0 with id: {ClientId} and name: {ClientName}", EntityTypeName, id, conf.Name);
        }

        protected override async Task ApplyStatus(IManagementApiClient api, V2alpha3Client entity, V2alpha3ClientConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (entity.Spec.SecretRef is not null && entity.Status.Id is not null)
            {
                var client = await api.Clients.GetAsync(entity.Status.Id, new GetClientRequestParameters { Fields = "client_id,client_secret" }, null, cancellationToken);
                await ApplySecret(entity, client.ClientId, client.ClientSecret, defaultNamespace, cancellationToken);
            }

            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        async Task ApplySecret(V2alpha3Client entity, string? clientId, string? clientSecret, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (entity.Spec.SecretRef is null)
                return;

            var secret = await ResolveSecretRef(entity.Spec.SecretRef, entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, cancellationToken);
            if (secret is null)
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName} which does not exist: creating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
                secret = await Kube.CreateAsync(
                    new V1Secret { Metadata = new V1ObjectMeta { NamespaceProperty = entity.Spec.SecretRef.NamespaceProperty ?? defaultNamespace, Name = entity.Spec.SecretRef.Name } }
                        .WithOwnerReference(entity),
                    cancellationToken);
            }

            if (IsOwnedBySelf(secret, entity))
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName}: updating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);

                // Heal owner references written by an earlier storage/hub version (e.g. v1 -> v2alpha3): the recorded
                // apiVersion/kind/name are refreshed to the current entity so external tooling and eventual removal of
                // the old served version keep a valid reference.
                foreach (var owner in secret.Metadata.OwnerReferences.Where(r => r.Uid == entity.Uid()))
                {
                    owner.ApiVersion = entity.ApiVersion;
                    owner.Kind = entity.Kind;
                    owner.Name = entity.Name();
                }

                secret.StringData ??= new Dictionary<string, string>();

                if (clientId is not null)
                    secret.StringData["clientId"] = clientId;
                else if (!secret.StringData.ContainsKey("clientId"))
                    secret.StringData["clientId"] = "";

                if (clientSecret is not null)
                    secret.StringData["clientSecret"] = clientSecret;
                else if (!secret.StringData.ContainsKey("clientSecret"))
                    secret.StringData["clientSecret"] = "";

                await Kube.UpdateAsync(secret, cancellationToken);
            }
            else
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} secret {SecretName} exists but is not owned by this client, skipping update", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
            }
        }

        /// <summary>
        /// Determines whether <paramref name="secret"/> is owned by <paramref name="entity"/>, matching on UID only.
        /// The built-in <see cref="k8s.Models.ModelExtensions.IsOwnedBy"/> also compares the owner reference's
        /// apiVersion (and kind/name), which drifts when the entity's storage/hub version changes: an existing secret
        /// keeps the apiVersion written by the operator generation that created it (e.g. <c>v1</c>), so a strict match
        /// would treat the operator's own secret as foreign and refuse to update it. The UID is the stable identity
        /// Kubernetes garbage collection itself keys on, so matching on UID alone is both correct and version-agnostic.
        /// </summary>
        static bool IsOwnedBySelf(V1Secret secret, V2alpha3Client entity)
        {
            var uid = entity.Uid();
            if (string.IsNullOrEmpty(uid))
                return false;

            return secret.Metadata?.OwnerReferences?.Any(r => r.Uid == uid) ?? false;
        }

        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting client from Auth0 with ID: {ClientId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Clients.DeleteAsync(id, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted client from Auth0 with ID: {ClientId}", EntityTypeName, id);
        }

    }

}
