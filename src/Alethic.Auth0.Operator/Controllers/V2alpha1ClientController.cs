using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;
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

    using V2alpha1ClientEntity = Models.V2alpha1Client;

    [EntityRbac(typeof(V2alpha1ClientEntity), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha1ClientController :
        V1TenantEntityInstanceController<V2alpha1ClientEntity, V2alpha1ClientEntity.SpecDef, V2alpha1ClientEntity.StatusDef, V2alpha1ClientConf, V2alpha1ClientConf>,
        IEntityController<V2alpha1ClientEntity>
    {

        internal static TTo? JsonConvertTo<TTo>(object? source)
        {
            return JsonSerializer.Deserialize<TTo>(JsonSerializer.Serialize(source));
        }

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientConf? FromApi(GetClientResponseContent? source) => source is null ? null : new()
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
        internal static V2alpha1ClientTokenEndpointAuthMethodEnum? FromApi(ClientTokenEndpointAuthMethodEnum? source) => source?.Value switch
        {
            ClientTokenEndpointAuthMethodEnum.Values.None => V2alpha1ClientTokenEndpointAuthMethodEnum.None,
            ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost => V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
            ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic => V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretBasic,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientSigningKey? FromApi(ClientSigningKey? source) => source is null ? null : new()
        {
            Cert = source.Cert,
            Pkcs7 = source.Pkcs7,
            Subject = source.Subject,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientRefreshTokenConfiguration? FromApi(ClientRefreshTokenConfiguration? source) => source is null ? null : new()
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
        internal static V2alpha1ClientRefreshTokenRotationTypeEnum? FromApi(RefreshTokenRotationTypeEnum? source) => source?.Value switch
        {
            RefreshTokenRotationTypeEnum.Values.Rotating => V2alpha1ClientRefreshTokenRotationTypeEnum.Rotating,
            RefreshTokenRotationTypeEnum.Values.NonRotating => V2alpha1ClientRefreshTokenRotationTypeEnum.NonRotating,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientRefreshTokenExpirationTypeEnum? FromApi(RefreshTokenExpirationTypeEnum? source) => source?.Value switch
        {
            RefreshTokenExpirationTypeEnum.Values.Expiring => V2alpha1ClientRefreshTokenExpirationTypeEnum.Expiring,
            RefreshTokenExpirationTypeEnum.Values.NonExpiring => V2alpha1ClientRefreshTokenExpirationTypeEnum.NonExpiring,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientOrganizationUsageEnum? FromApi(ClientOrganizationUsageEnum? source) => source?.Value switch
        {
            ClientOrganizationUsageEnum.Values.Deny => V2alpha1ClientOrganizationUsageEnum.Deny,
            ClientOrganizationUsageEnum.Values.Allow => V2alpha1ClientOrganizationUsageEnum.Allow,
            ClientOrganizationUsageEnum.Values.Require => V2alpha1ClientOrganizationUsageEnum.Require,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientOrganizationRequireBehaviorEnum? FromApi(ClientOrganizationRequireBehaviorEnum? source) => source?.Value switch
        {
            ClientOrganizationRequireBehaviorEnum.Values.NoPrompt => V2alpha1ClientOrganizationRequireBehaviorEnum.NoPrompt,
            ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt => V2alpha1ClientOrganizationRequireBehaviorEnum.PreLoginPrompt,
            ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt => V2alpha1ClientOrganizationRequireBehaviorEnum.PostLoginPrompt,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientOidcBackchannelLogoutSettings? FromApi(ClientOidcBackchannelLogoutSettings? source) => source is null ? null : new()
        {
            BackchannelLogoutUrls = source.BackchannelLogoutUrls?.ToArray(),
            BackchannelLogoutInitiators = source.BackchannelLogoutInitiators is { } initiators ? FromApi(initiators) : null,
            BackchannelLogoutSessionMetadata = source.BackchannelLogoutSessionMetadata.IsDefined && source.BackchannelLogoutSessionMetadata.Value is { } sessionMetadata
                ? JsonConvertTo<V2alpha1ClientOidcBackchannelLogoutSessionMetadata>(sessionMetadata)
                : null,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientOidcBackchannelLogoutInitiators? FromApi(ClientOidcBackchannelLogoutInitiators? source) => source is null ? null : new()
        {
            Mode = FromApi(source.Mode),
            SelectedInitiators = source.SelectedInitiators?.Select(FromApi).ToArray(),
        };

        internal static V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum FromApi(ClientOidcBackchannelLogoutInitiatorsEnum source) => source.Value switch
        {
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout => V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout => V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.IdpLogout,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged => V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.PasswordChanged,
            ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired => V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.SessionExpired,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum? FromApi(ClientOidcBackchannelLogoutInitiatorsModeEnum? source) => source?.Value switch
        {
            ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All => V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.All,
            ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom => V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientMobile? FromApi(ClientMobile? source) => source is null ? null : new()
        {
            Android = source.Android is { } android ? FromApi(android) : null,
            Ios = source.Ios is { } ios ? FromApi(ios) : null,
        };

        internal static V2alpha1ClientMobileiOs? FromApi(ClientMobileiOs? source)
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

        internal static V2alpha1ClientMobileAndroid? FromApi(ClientMobileAndroid? source)
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
        internal static V2alpha1ClientJwtConfiguration? FromApi(ClientJwtConfiguration? source) => source is null ? null : new()
        {
            SecretEncoded = source.SecretEncoded,
            LifetimeInSeconds = source.LifetimeInSeconds,
            Alg = FromApi(source.Alg),
            Scopes = source.Scopes,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientSigningAlgorithmEnum? FromApi(SigningAlgorithmEnum? source) => source?.Value switch
        {
            SigningAlgorithmEnum.Values.Hs256 => V2alpha1ClientSigningAlgorithmEnum.Hs256,
            SigningAlgorithmEnum.Values.Rs256 => V2alpha1ClientSigningAlgorithmEnum.Rs256,
            SigningAlgorithmEnum.Values.Rs512 => V2alpha1ClientSigningAlgorithmEnum.Rs512,
            SigningAlgorithmEnum.Values.Ps256 => V2alpha1ClientSigningAlgorithmEnum.Ps256,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientEncryptionKey? FromApi(ClientEncryptionKey? source) => source is null ? null : new()
        {
            Cert = source.Cert,
            Pub = source.Pub,
            Subject = source.Subject,
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientDefaultOrganization? FromApi(ClientDefaultOrganization? source) => source is null ? null : new()
        {
            OrganizationId = source.OrganizationId,
            Flows = source.Flows?.Select(FromApi).ToArray(),
        };

        internal static V2alpha1ClientDefaultOrganizationFlowsEnum FromApi(ClientDefaultOrganizationFlowsEnum source) => source.Value switch
        {
            ClientDefaultOrganizationFlowsEnum.Values.ClientCredentials => V2alpha1ClientDefaultOrganizationFlowsEnum.ClientCredentials,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientComplianceLevelEnum? FromApi(ClientComplianceLevelEnum? source) => source?.Value switch
        {
            ClientComplianceLevelEnum.Values.None => V2alpha1ClientComplianceLevelEnum.None,
            ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar => V2alpha1ClientComplianceLevelEnum.Fapi1AdvPkjPar,
            ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar => V2alpha1ClientComplianceLevelEnum.Fapi1AdvMtlsPar,
            ClientComplianceLevelEnum.Values.Fapi2SpPkjMtls => V2alpha1ClientComplianceLevelEnum.Fapi2SpPkjMtls,
            ClientComplianceLevelEnum.Values.Fapi2SpMtlsMtls => V2alpha1ClientComplianceLevelEnum.Fapi2SpMtlsMtls,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientAppTypeEnum? FromApi(ClientAppTypeEnum? source) => source?.Value switch
        {
            ClientAppTypeEnum.Values.Box => V2alpha1ClientAppTypeEnum.Box,
            ClientAppTypeEnum.Values.Cloudbees => V2alpha1ClientAppTypeEnum.Cloudbees,
            ClientAppTypeEnum.Values.Concur => V2alpha1ClientAppTypeEnum.Concur,
            ClientAppTypeEnum.Values.Dropbox => V2alpha1ClientAppTypeEnum.Dropbox,
            ClientAppTypeEnum.Values.Echosign => V2alpha1ClientAppTypeEnum.Echosign,
            ClientAppTypeEnum.Values.Egnyte => V2alpha1ClientAppTypeEnum.Egnyte,
            ClientAppTypeEnum.Values.Mscrm => V2alpha1ClientAppTypeEnum.Mscrm,
            ClientAppTypeEnum.Values.Native => V2alpha1ClientAppTypeEnum.Native,
            ClientAppTypeEnum.Values.Newrelic => V2alpha1ClientAppTypeEnum.Newrelic,
            ClientAppTypeEnum.Values.NonInteractive => V2alpha1ClientAppTypeEnum.NonInteractive,
            ClientAppTypeEnum.Values.Office365 => V2alpha1ClientAppTypeEnum.Office365,
            ClientAppTypeEnum.Values.RegularWeb => V2alpha1ClientAppTypeEnum.RegularWeb,
            ClientAppTypeEnum.Values.Rms => V2alpha1ClientAppTypeEnum.Rms,
            ClientAppTypeEnum.Values.Salesforce => V2alpha1ClientAppTypeEnum.Salesforce,
            ClientAppTypeEnum.Values.Sentry => V2alpha1ClientAppTypeEnum.Sentry,
            ClientAppTypeEnum.Values.Sharepoint => V2alpha1ClientAppTypeEnum.Sharepoint,
            ClientAppTypeEnum.Values.Slack => V2alpha1ClientAppTypeEnum.Slack,
            ClientAppTypeEnum.Values.Springcm => V2alpha1ClientAppTypeEnum.Springcm,
            ClientAppTypeEnum.Values.Spa => V2alpha1ClientAppTypeEnum.Spa,
            ClientAppTypeEnum.Values.Zendesk => V2alpha1ClientAppTypeEnum.Zendesk,
            ClientAppTypeEnum.Values.Zoom => V2alpha1ClientAppTypeEnum.Zoom,
            ClientAppTypeEnum.Values.ResourceServer => V2alpha1ClientAppTypeEnum.ResourceServer,
            ClientAppTypeEnum.Values.ExpressConfiguration => V2alpha1ClientAppTypeEnum.ExpressConfiguration,
            ClientAppTypeEnum.Values.SsoIntegration => V2alpha1ClientAppTypeEnum.SsoIntegration,
            ClientAppTypeEnum.Values.Oag => V2alpha1ClientAppTypeEnum.Oag,
            null => null,
            _ => throw new NotImplementedException(),
        };

        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha1ClientAddons? FromApi(ClientAddons? source) => JsonConvertTo<V2alpha1ClientAddons>(source);

        internal static ClientTokenEndpointAuthMethodEnum ToApi(V2alpha1ClientTokenEndpointAuthMethodEnum source) => source switch
        {
            V2alpha1ClientTokenEndpointAuthMethodEnum.None => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.None),
            V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretPost),
            V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretBasic => new ClientTokenEndpointAuthMethodEnum(ClientTokenEndpointAuthMethodEnum.Values.ClientSecretBasic),
            _ => throw new NotImplementedException(),
        };

        internal static ClientTokenEndpointAuthMethodOrNullEnum ToApiOrNull(V2alpha1ClientTokenEndpointAuthMethodEnum source) => source switch
        {
            V2alpha1ClientTokenEndpointAuthMethodEnum.None => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.None),
            V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.ClientSecretPost),
            V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretBasic => new ClientTokenEndpointAuthMethodOrNullEnum(ClientTokenEndpointAuthMethodOrNullEnum.Values.ClientSecretBasic),
            _ => throw new NotImplementedException(),
        };

        internal static ClientAppTypeEnum ToApi(V2alpha1ClientAppTypeEnum source) => source switch
        {
            V2alpha1ClientAppTypeEnum.Box => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Box),
            V2alpha1ClientAppTypeEnum.Cloudbees => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Cloudbees),
            V2alpha1ClientAppTypeEnum.Concur => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Concur),
            V2alpha1ClientAppTypeEnum.Dropbox => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Dropbox),
            V2alpha1ClientAppTypeEnum.Echosign => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Echosign),
            V2alpha1ClientAppTypeEnum.Egnyte => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Egnyte),
            V2alpha1ClientAppTypeEnum.Mscrm => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Mscrm),
            V2alpha1ClientAppTypeEnum.Native => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Native),
            V2alpha1ClientAppTypeEnum.Newrelic => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Newrelic),
            V2alpha1ClientAppTypeEnum.NonInteractive => new ClientAppTypeEnum(ClientAppTypeEnum.Values.NonInteractive),
            V2alpha1ClientAppTypeEnum.Office365 => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Office365),
            V2alpha1ClientAppTypeEnum.RegularWeb => new ClientAppTypeEnum(ClientAppTypeEnum.Values.RegularWeb),
            V2alpha1ClientAppTypeEnum.Rms => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Rms),
            V2alpha1ClientAppTypeEnum.Salesforce => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Salesforce),
            V2alpha1ClientAppTypeEnum.Sentry => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sentry),
            V2alpha1ClientAppTypeEnum.Sharepoint => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Sharepoint),
            V2alpha1ClientAppTypeEnum.Slack => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Slack),
            V2alpha1ClientAppTypeEnum.Springcm => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Springcm),
            V2alpha1ClientAppTypeEnum.Spa => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Spa),
            V2alpha1ClientAppTypeEnum.Zendesk => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zendesk),
            V2alpha1ClientAppTypeEnum.Zoom => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Zoom),
            V2alpha1ClientAppTypeEnum.ResourceServer => new ClientAppTypeEnum(ClientAppTypeEnum.Values.ResourceServer),
            V2alpha1ClientAppTypeEnum.ExpressConfiguration => new ClientAppTypeEnum(ClientAppTypeEnum.Values.ExpressConfiguration),
            V2alpha1ClientAppTypeEnum.SsoIntegration => new ClientAppTypeEnum(ClientAppTypeEnum.Values.SsoIntegration),
            V2alpha1ClientAppTypeEnum.Oag => new ClientAppTypeEnum(ClientAppTypeEnum.Values.Oag),
            _ => throw new NotImplementedException(),
        };

        internal static SigningAlgorithmEnum ToApi(V2alpha1ClientSigningAlgorithmEnum source) => source switch
        {
            V2alpha1ClientSigningAlgorithmEnum.Hs256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Hs256),
            V2alpha1ClientSigningAlgorithmEnum.Rs256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256),
            V2alpha1ClientSigningAlgorithmEnum.Rs512 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs512),
            V2alpha1ClientSigningAlgorithmEnum.Ps256 => new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Ps256),
            _ => throw new NotImplementedException(),
        };

        internal static ClientComplianceLevelEnum ToApi(V2alpha1ClientComplianceLevelEnum source) => source switch
        {
            V2alpha1ClientComplianceLevelEnum.None => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.None),
            V2alpha1ClientComplianceLevelEnum.Fapi1AdvPkjPar => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvPkjPar),
            V2alpha1ClientComplianceLevelEnum.Fapi1AdvMtlsPar => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi1AdvMtlsPar),
            V2alpha1ClientComplianceLevelEnum.Fapi2SpPkjMtls => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi2SpPkjMtls),
            V2alpha1ClientComplianceLevelEnum.Fapi2SpMtlsMtls => new ClientComplianceLevelEnum(ClientComplianceLevelEnum.Values.Fapi2SpMtlsMtls),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationRequireBehaviorEnum ToApi(V2alpha1ClientOrganizationRequireBehaviorEnum source) => source switch
        {
            V2alpha1ClientOrganizationRequireBehaviorEnum.NoPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.NoPrompt),
            V2alpha1ClientOrganizationRequireBehaviorEnum.PreLoginPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PreLoginPrompt),
            V2alpha1ClientOrganizationRequireBehaviorEnum.PostLoginPrompt => new ClientOrganizationRequireBehaviorEnum(ClientOrganizationRequireBehaviorEnum.Values.PostLoginPrompt),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationRequireBehaviorPatchEnum ToApiPatch(V2alpha1ClientOrganizationRequireBehaviorEnum source) => source switch
        {
            V2alpha1ClientOrganizationRequireBehaviorEnum.NoPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.NoPrompt),
            V2alpha1ClientOrganizationRequireBehaviorEnum.PreLoginPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.PreLoginPrompt),
            V2alpha1ClientOrganizationRequireBehaviorEnum.PostLoginPrompt => new ClientOrganizationRequireBehaviorPatchEnum(ClientOrganizationRequireBehaviorPatchEnum.Values.PostLoginPrompt),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationUsageEnum ToApi(V2alpha1ClientOrganizationUsageEnum source) => source switch
        {
            V2alpha1ClientOrganizationUsageEnum.Deny => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Deny),
            V2alpha1ClientOrganizationUsageEnum.Allow => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Allow),
            V2alpha1ClientOrganizationUsageEnum.Require => new ClientOrganizationUsageEnum(ClientOrganizationUsageEnum.Values.Require),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOrganizationUsagePatchEnum ToApiPatch(V2alpha1ClientOrganizationUsageEnum source) => source switch
        {
            V2alpha1ClientOrganizationUsageEnum.Deny => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Deny),
            V2alpha1ClientOrganizationUsageEnum.Allow => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Allow),
            V2alpha1ClientOrganizationUsageEnum.Require => new ClientOrganizationUsagePatchEnum(ClientOrganizationUsagePatchEnum.Values.Require),
            _ => throw new NotImplementedException(),
        };

        internal static RefreshTokenRotationTypeEnum ToApi(V2alpha1ClientRefreshTokenRotationTypeEnum source) => source switch
        {
            V2alpha1ClientRefreshTokenRotationTypeEnum.Rotating => new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.Rotating),
            V2alpha1ClientRefreshTokenRotationTypeEnum.NonRotating => new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
            _ => throw new NotImplementedException(),
        };

        internal static RefreshTokenExpirationTypeEnum ToApi(V2alpha1ClientRefreshTokenExpirationTypeEnum source) => source switch
        {
            V2alpha1ClientRefreshTokenExpirationTypeEnum.Expiring => new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.Expiring),
            V2alpha1ClientRefreshTokenExpirationTypeEnum.NonExpiring => new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOidcBackchannelLogoutInitiatorsModeEnum ToApi(V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum source) => source switch
        {
            V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.All => new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.All),
            V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum.Custom => new ClientOidcBackchannelLogoutInitiatorsModeEnum(ClientOidcBackchannelLogoutInitiatorsModeEnum.Values.Custom),
            _ => throw new NotImplementedException(),
        };

        internal static ClientOidcBackchannelLogoutInitiatorsEnum ToApi(V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum source) => source switch
        {
            V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.RpLogout => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.RpLogout),
            V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.IdpLogout => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.IdpLogout),
            V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.PasswordChanged => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.PasswordChanged),
            V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum.SessionExpired => new ClientOidcBackchannelLogoutInitiatorsEnum(ClientOidcBackchannelLogoutInitiatorsEnum.Values.SessionExpired),
            _ => throw new NotImplementedException(),
        };

        internal static void ApplyToApi(V2alpha1ClientRefreshTokenConfiguration source, ClientRefreshTokenConfiguration target)
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

        internal static void ApplyToApi(V2alpha1ClientOidcBackchannelLogoutInitiators source, ClientOidcBackchannelLogoutInitiators target)
        {
            if (source.Mode is { } mode)
                target.Mode = ToApi(mode);

            if (source.SelectedInitiators is { } selected)
                target.SelectedInitiators = [.. selected.Select(ToApi)];
        }

        internal static void ApplyToApi(V2alpha1ClientOidcBackchannelLogoutSettings source, ClientOidcBackchannelLogoutSettings target)
        {
            if (source.BackchannelLogoutUrls is { } backchannelLogoutUrls)
                target.BackchannelLogoutUrls = backchannelLogoutUrls;

            if (source.BackchannelLogoutInitiators is { } initiators)
                ApplyToApi(initiators, target.BackchannelLogoutInitiators ??= new());

            if (source.BackchannelLogoutSessionMetadata is { } sessionMetadata)
                target.BackchannelLogoutSessionMetadata = JsonConvertTo<ClientOidcBackchannelLogoutSessionMetadata>(sessionMetadata);
        }

        internal static void ApplyToApi(V2alpha1ClientEncryptionKey source, ClientEncryptionKey target)
        {
            if (source.Cert is { } cert)
                target.Cert = cert;

            if (source.Pub is { } pub)
                target.Pub = pub;

            if (source.Subject is { } subject)
                target.Subject = subject;
        }

        internal static void ApplyToApi(V2alpha1ClientJwtConfiguration source, ClientJwtConfiguration target)
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

        internal static void ApplyToApi(V2alpha1ClientMobileAndroid source, ClientMobileAndroid target)
        {
            if (source.AppPackageName is { } appPackageName)
                target.AppPackageName = appPackageName;
        }

        internal static void ApplyToApi(V2alpha1ClientMobileiOs source, ClientMobileiOs target)
        {
            if (source.AppBundleIdentifier is { } appBundleIdentifier)
                target.AppBundleIdentifier = appBundleIdentifier;

            if (source.TeamId is { } teamId)
                target.TeamId = teamId;
        }

        internal static void ApplyToApi(V2alpha1ClientMobile source, ClientMobile target)
        {
            if (source.Android is { } android && android.AppPackageName is not null)
                ApplyToApi(android, target.Android ??= new ClientMobileAndroid());

            if (source.Ios is { } ios && (ios.AppBundleIdentifier is not null || ios.TeamId is not null))
                ApplyToApi(ios, target.Ios ??= new ClientMobileiOs());
        }

        internal static void ApplyToApi(V2alpha1ClientConf conf, CreateClientRequestContent request)
        {
            if (conf.ApplicationType is { } appType)
                request.AppType = ToApi(appType);

            if (conf.TokenEndpointAuthMethod is { } tokenEndpointAuthMethod)
                request.TokenEndpointAuthMethod = ToApi(tokenEndpointAuthMethod);

            ApplyToApiBase(conf, request);
        }

        internal static void ApplyToApi(V2alpha1ClientConf conf, UpdateClientRequestContent request)
        {
            if (conf.ApplicationType is { } appType)
                request.AppType = ToApi(appType);

            if (conf.TokenEndpointAuthMethod is { } tokenEndpointAuthMethod)
                request.TokenEndpointAuthMethod = ToApiOrNull(tokenEndpointAuthMethod);

            ApplyToApiBase(conf, request);
        }

        internal static void ApplyToApiBase(V2alpha1ClientConf conf, CreateClientRequestContent request)
        {
            if (conf.AddOns is { } addons)
                request.Addons = JsonConvertTo<ClientAddons>(addons);

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
                request.DefaultOrganization = JsonConvertTo<ClientDefaultOrganization>(defaultOrganization);

            if (conf.ComplianceLevel is { } complianceLevel)
                request.ComplianceLevel = ToApi(complianceLevel);

            if (conf.RequireProofOfPossession is not null)
                request.RequireProofOfPossession = conf.RequireProofOfPossession;
        }

        internal static void ApplyToApiBase(V2alpha1ClientConf conf, UpdateClientRequestContent request)
        {
            if (conf.AddOns is { } addons)
                request.Addons = JsonConvertTo<ClientAddons>(addons);

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
                request.DefaultOrganization = JsonConvertTo<ClientDefaultOrganization>(defaultOrganization);

            if (conf.ComplianceLevel is { } complianceLevel)
                request.ComplianceLevel = ToApi(complianceLevel);

            if (conf.RequireProofOfPossession is not null)
                request.RequireProofOfPossession = conf.RequireProofOfPossession;
        }

        public V2alpha1ClientController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V2alpha1ClientController> logger) :
            base(kube, cache, options, logger)
        {
        }

        protected override string EntityTypeName => "Client";

        protected override async Task<V2alpha1ClientConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
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

        protected override async Task<string?> Find(IManagementApiClient api, V2alpha1ClientEntity entity, V2alpha1ClientEntity.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
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

        protected override string? ValidateCreate(V2alpha1ClientConf conf)
        {
            if (conf.ApplicationType == null)
                return "missing a value for application type";

            return null;
        }

        protected override async Task<string> Create(IManagementApiClient api, V2alpha1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating client in Auth0 with name: {ClientName}", EntityTypeName, conf.Name);

            var req = new CreateClientRequestContent { Name = conf.Name ?? throw new InvalidOperationException("Missing client name.") };
            ApplyToApi(conf, req);

            var self = await api.Clients.CreateAsync(req, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully created client in Auth0 with ID: {ClientId} and name: {ClientName}", EntityTypeName, self.ClientId, conf.Name);
            return self.ClientId;
        }

        protected override async Task Update(IManagementApiClient api, string id, V2alpha1ClientConf? last, V2alpha1ClientConf conf, string defaultNamespace, CancellationToken cancellationToken)
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

        protected override async Task ApplyStatus(IManagementApiClient api, V2alpha1ClientEntity entity, V2alpha1ClientConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (entity.Spec.SecretRef is not null && entity.Status.Id is not null)
            {
                var client = await api.Clients.GetAsync(entity.Status.Id, new GetClientRequestParameters { Fields = "client_id,client_secret" }, null, cancellationToken);
                await ApplySecret(entity, client.ClientId, client.ClientSecret, defaultNamespace, cancellationToken);
            }

            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        async Task ApplySecret(V2alpha1ClientEntity entity, string? clientId, string? clientSecret, string defaultNamespace, CancellationToken cancellationToken)
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

            if (secret.IsOwnedBy(entity))
            {
                Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} referenced secret {SecretName}: updating.", EntityTypeName, entity.Namespace(), entity.Name(), entity.Spec.SecretRef.Name);
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

        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting client from Auth0 with ID: {ClientId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Clients.DeleteAsync(id, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted client from Auth0 with ID: {ClientId}", EntityTypeName, id);
        }

    }

}
