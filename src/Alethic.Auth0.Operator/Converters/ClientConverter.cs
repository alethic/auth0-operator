using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Linq;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V1;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;
using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3ClientEntity = Models.V2alpha3Client;

    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3ClientEntity))]
    public class ClientConverter : ConversionWebhook<V2alpha3ClientEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3ClientEntity>> Converters => [
            new V1ToV2alpha3(),
            new V2alpha1ToV2alpha3()
        ];

        class V1ToV2alpha3 : IEntityConverter<V1Client, V2alpha3ClientEntity>
        {

            static readonly JsonSerializerOptions Auth0JsonSerializerOptions = CreateAuth0JsonSerializerOptions();

            public V2alpha3ClientEntity Convert(V1Client from)
            {
                var result = new V2alpha3ClientEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.SecretRef = from.Spec.SecretRef;
                result.Spec.Find = from.Spec.Find is { } find ? new V2alpha3ClientFind { ClientId = find.ClientId, Name = find.Name } : null;
                result.Spec.Init = ConvertConf(from.Spec.Init);
                result.Spec.Conf = ConvertConf(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = ConvertConf(from.Status.LastConf);
                return result;
            }

            public V1Client Revert(V2alpha3ClientEntity source)
            {
                var result = new V1Client { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.SecretRef = source.Spec.SecretRef;
                result.Spec.Find = source.Spec.Find is { } find ? new V1ClientFind { ClientId = find.ClientId, Name = find.Name } : null;
                result.Spec.Init = RevertConf(source.Spec.Init);
                result.Spec.Conf = RevertConf(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = RevertConf(source.Status.LastConf);
                return result;
            }

            static V2alpha3ClientConf? ConvertConf(V1ClientConf? source)
            {
                if (source is null)
                    return null;

                return new V2alpha3ClientConf
                {
                    AllowedClients = source.AllowedClients,
                    AllowedLogoutUrls = source.AllowedLogoutUrls,
                    AllowedOrigins = source.AllowedOrigins,
                    WebOrigins = source.WebOrigins,
                    InitiateLoginUri = source.InitiateLoginUri,
                    Callbacks = source.Callbacks,
                    ClientAliases = source.ClientAliases,
                    ClientMetaData = source.ClientMetaData,
                    IsCustomLoginPageOn = source.IsCustomLoginPageOn,
                    IsFirstParty = source.IsFirstParty,
                    CustomLoginPage = source.CustomLoginPage,
                    CustomLoginPagePreview = source.CustomLoginPagePreview,
                    FormTemplate = source.FormTemplate,
                    GrantTypes = source.GrantTypes,
                    Name = source.Name,
                    Description = source.Description,
                    LogoUri = source.LogoUri,
                    OidcConformant = source.OidcConformant,
                    Sso = source.Sso,
                    CrossOriginAuthentication = source.CrossOriginAuthentication,
                    RequirePushedAuthorizationRequests = source.RequirePushedAuthorizationRequests,
                    RequireProofOfPossession = source.RequireProofOfPossession,
                    ResourceServers = null,
                    AddOns = source.AddOns is { } addons ? ConvertAddons(addons) : null,
                    ApplicationType = source.ApplicationType is { } applicationType ? ConvertApplicationType(applicationType) : null,
                    ComplianceLevel = source.ComplianceLevel is { } complianceLevel ? ConvertComplianceLevel(complianceLevel) : null,
                    DefaultOrganization = source.DefaultOrganization is { } defaultOrganization ? ConvertWithAuth0<V1ClientDefaultOrganization, ClientDefaultOrganization, V2alpha3ClientDefaultOrganization>(defaultOrganization, V2alpha3ClientController.FromApi) : null,
                    EncryptionKey = source.EncryptionKey is { } encryptionKey ? ConvertWithAuth0<V1ClientEncryptionKey, ClientEncryptionKey, V2alpha3ClientEncryptionKey>(encryptionKey, V2alpha3ClientController.FromApi) : null,
                    JwtConfiguration = source.JwtConfiguration is { } jwtConfiguration ? ConvertWithAuth0<V1ClientJwtConfiguration, ClientJwtConfiguration, V2alpha3ClientJwtConfiguration>(jwtConfiguration, V2alpha3ClientController.FromApi) : null,
                    Mobile = source.Mobile is { } mobile ? ConvertWithAuth0<V1ClientMobile, ClientMobile, V2alpha3ClientMobile>(mobile, V2alpha3ClientController.FromApi) : null,
                    OidcLogout = source.OidcLogout is { } oidcLogout ? ConvertWithAuth0<V1ClientOidcLogoutConfig, ClientOidcBackchannelLogoutSettings, V2alpha3ClientOidcBackchannelLogoutSettings>(oidcLogout, V2alpha3ClientController.FromApi) : null,
                    OrganizationRequireBehavior = source.OrganizationRequireBehavior is { } organizationRequireBehavior ? ConvertOrganizationRequireBehavior(organizationRequireBehavior) : null,
                    OrganizationUsage = source.OrganizationUsage is { } organizationUsage ? ConvertOrganizationUsage(organizationUsage) : null,
                    RefreshToken = source.RefreshToken is { } refreshToken ? ConvertWithAuth0<V1ClientRefreshToken, ClientRefreshTokenConfiguration, V2alpha3ClientRefreshTokenConfiguration>(refreshToken, V2alpha3ClientController.FromApi) : null,
                    SigningKeys = source.SigningKeys is { Length: > 0 } signingKeys ? [.. signingKeys.Select(signingKey => ConvertWithAuth0<V1ClientSigningKey, ClientSigningKey, V2alpha3ClientSigningKey>(signingKey, V2alpha3ClientController.FromApi)!)] : null,
                    TokenEndpointAuthMethod = source.TokenEndpointAuthMethod is { } tokenEndpointAuthMethod ? ConvertTokenEndpointAuthMethod(tokenEndpointAuthMethod) : null,
                };
            }

            static V1ClientConf? RevertConf(V2alpha3ClientConf? source)
            {
                if (source is null)
                    return null;

                return new V1ClientConf
                {
                    AllowedClients = source.AllowedClients,
                    AllowedLogoutUrls = source.AllowedLogoutUrls,
                    AllowedOrigins = source.AllowedOrigins,
                    WebOrigins = source.WebOrigins,
                    InitiateLoginUri = source.InitiateLoginUri,
                    Callbacks = source.Callbacks,
                    ClientAliases = source.ClientAliases,
                    ClientMetaData = source.ClientMetaData,
                    IsCustomLoginPageOn = source.IsCustomLoginPageOn,
                    IsFirstParty = source.IsFirstParty,
                    CustomLoginPage = source.CustomLoginPage,
                    CustomLoginPagePreview = source.CustomLoginPagePreview,
                    FormTemplate = source.FormTemplate,
                    GrantTypes = source.GrantTypes,
                    Name = source.Name,
                    Description = source.Description,
                    LogoUri = source.LogoUri,
                    OidcConformant = source.OidcConformant,
                    Sso = source.Sso,
                    CrossOriginAuthentication = source.CrossOriginAuthentication,
                    RequirePushedAuthorizationRequests = source.RequirePushedAuthorizationRequests,
                    RequireProofOfPossession = source.RequireProofOfPossession,
                    ResourceServers = null,
                    AddOns = source.AddOns is { } addons ? RevertAddons(addons) : null,
                    ApplicationType = source.ApplicationType is { } applicationType ? RevertApplicationType(applicationType) : null,
                    ComplianceLevel = source.ComplianceLevel is { } complianceLevel ? RevertComplianceLevel(complianceLevel) : null,
                    DefaultOrganization = source.DefaultOrganization is { } defaultOrganization ? RevertWithAuth0<V2alpha3ClientDefaultOrganization, ClientDefaultOrganization, V1ClientDefaultOrganization>(defaultOrganization, ToApi) : null,
                    EncryptionKey = source.EncryptionKey is { } encryptionKey ? RevertWithAuth0<V2alpha3ClientEncryptionKey, ClientEncryptionKey, V1ClientEncryptionKey>(encryptionKey, ToApi) : null,
                    JwtConfiguration = source.JwtConfiguration is { } jwtConfiguration ? RevertWithAuth0<V2alpha3ClientJwtConfiguration, ClientJwtConfiguration, V1ClientJwtConfiguration>(jwtConfiguration, ToApi) : null,
                    Mobile = source.Mobile is { } mobile ? RevertWithAuth0<V2alpha3ClientMobile, ClientMobile, V1ClientMobile>(mobile, ToApi) : null,
                    OidcLogout = source.OidcLogout is { } oidcLogout ? RevertWithAuth0<V2alpha3ClientOidcBackchannelLogoutSettings, ClientOidcBackchannelLogoutSettings, V1ClientOidcLogoutConfig>(oidcLogout, ToApi) : null,
                    OrganizationRequireBehavior = source.OrganizationRequireBehavior is { } organizationRequireBehavior ? RevertOrganizationRequireBehavior(organizationRequireBehavior) : null,
                    OrganizationUsage = source.OrganizationUsage is { } organizationUsage ? RevertOrganizationUsage(organizationUsage) : null,
                    RefreshToken = source.RefreshToken is { } refreshToken ? RevertWithAuth0<V2alpha3ClientRefreshTokenConfiguration, ClientRefreshTokenConfiguration, V1ClientRefreshToken>(refreshToken, ToApi) : null,
                    SigningKeys = source.SigningKeys is { Length: > 0 } signingKeys ? [.. signingKeys.Select(RevertSigningKey)] : null,
                    TokenEndpointAuthMethod = source.TokenEndpointAuthMethod is { } tokenEndpointAuthMethod ? RevertTokenEndpointAuthMethod(tokenEndpointAuthMethod) : null,
                };
            }

            static V2alpha3ClientComplianceLevelEnum ConvertComplianceLevel(V1ClientComplianceLevel source) => source switch
            {
                V1ClientComplianceLevel.None => V2alpha3ClientComplianceLevelEnum.None,
                V1ClientComplianceLevel.Fapi1AdvPkjPar => V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar,
                V1ClientComplianceLevel.Fapi1AdvMtlsPar => V2alpha3ClientComplianceLevelEnum.Fapi1AdvMtlsPar,
                _ => throw new NotImplementedException(),
            };

            static V1ClientComplianceLevel RevertComplianceLevel(V2alpha3ClientComplianceLevelEnum source) => source switch
            {
                V2alpha3ClientComplianceLevelEnum.None => V1ClientComplianceLevel.None,
                V2alpha3ClientComplianceLevelEnum.Fapi1AdvPkjPar => V1ClientComplianceLevel.Fapi1AdvPkjPar,
                V2alpha3ClientComplianceLevelEnum.Fapi1AdvMtlsPar => V1ClientComplianceLevel.Fapi1AdvMtlsPar,
                _ => throw new NotImplementedException(),
            };

            static V2alpha3ClientOrganizationRequireBehaviorEnum ConvertOrganizationRequireBehavior(V1ClientOrganizationRequireBehavior source) => source switch
            {
                V1ClientOrganizationRequireBehavior.NoPrompt => V2alpha3ClientOrganizationRequireBehaviorEnum.NoPrompt,
                V1ClientOrganizationRequireBehavior.PreLoginPrompt => V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt,
                V1ClientOrganizationRequireBehavior.PostLoginPrompt => V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt,
                _ => throw new NotImplementedException(),
            };

            static V1ClientOrganizationRequireBehavior RevertOrganizationRequireBehavior(V2alpha3ClientOrganizationRequireBehaviorEnum source) => source switch
            {
                V2alpha3ClientOrganizationRequireBehaviorEnum.NoPrompt => V1ClientOrganizationRequireBehavior.NoPrompt,
                V2alpha3ClientOrganizationRequireBehaviorEnum.PreLoginPrompt => V1ClientOrganizationRequireBehavior.PreLoginPrompt,
                V2alpha3ClientOrganizationRequireBehaviorEnum.PostLoginPrompt => V1ClientOrganizationRequireBehavior.PostLoginPrompt,
                _ => throw new NotImplementedException(),
            };

            static V2alpha3ClientOrganizationUsageEnum ConvertOrganizationUsage(V1ClientOrganizationUsage source) => source switch
            {
                V1ClientOrganizationUsage.Deny => V2alpha3ClientOrganizationUsageEnum.Deny,
                V1ClientOrganizationUsage.Allow => V2alpha3ClientOrganizationUsageEnum.Allow,
                V1ClientOrganizationUsage.Require => V2alpha3ClientOrganizationUsageEnum.Require,
                _ => throw new NotImplementedException(),
            };

            static V1ClientOrganizationUsage RevertOrganizationUsage(V2alpha3ClientOrganizationUsageEnum source) => source switch
            {
                V2alpha3ClientOrganizationUsageEnum.Deny => V1ClientOrganizationUsage.Deny,
                V2alpha3ClientOrganizationUsageEnum.Allow => V1ClientOrganizationUsage.Allow,
                V2alpha3ClientOrganizationUsageEnum.Require => V1ClientOrganizationUsage.Require,
                _ => throw new NotImplementedException(),
            };

            static V2alpha3ClientAppTypeEnum ConvertApplicationType(V1ClientApplicationType source)
            {
                return source switch
                {
                    V1ClientApplicationType.Box => V2alpha3ClientAppTypeEnum.Box,
                    V1ClientApplicationType.Cloudbees => V2alpha3ClientAppTypeEnum.Cloudbees,
                    V1ClientApplicationType.Concur => V2alpha3ClientAppTypeEnum.Concur,
                    V1ClientApplicationType.Dropbox => V2alpha3ClientAppTypeEnum.Dropbox,
                    V1ClientApplicationType.Echosign => V2alpha3ClientAppTypeEnum.Echosign,
                    V1ClientApplicationType.Egnyte => V2alpha3ClientAppTypeEnum.Egnyte,
                    V1ClientApplicationType.MsCrm => V2alpha3ClientAppTypeEnum.Mscrm,
                    V1ClientApplicationType.Native => V2alpha3ClientAppTypeEnum.Native,
                    V1ClientApplicationType.NewRelic => V2alpha3ClientAppTypeEnum.Newrelic,
                    V1ClientApplicationType.NonInteractive => V2alpha3ClientAppTypeEnum.NonInteractive,
                    V1ClientApplicationType.Office365 => V2alpha3ClientAppTypeEnum.Office365,
                    V1ClientApplicationType.RegularWeb => V2alpha3ClientAppTypeEnum.RegularWeb,
                    V1ClientApplicationType.Rms => V2alpha3ClientAppTypeEnum.Rms,
                    V1ClientApplicationType.Salesforce => V2alpha3ClientAppTypeEnum.Salesforce,
                    V1ClientApplicationType.Sentry => V2alpha3ClientAppTypeEnum.Sentry,
                    V1ClientApplicationType.SharePoint => V2alpha3ClientAppTypeEnum.Sharepoint,
                    V1ClientApplicationType.Slack => V2alpha3ClientAppTypeEnum.Slack,
                    V1ClientApplicationType.SpringCm => V2alpha3ClientAppTypeEnum.Springcm,
                    V1ClientApplicationType.Spa => V2alpha3ClientAppTypeEnum.Spa,
                    V1ClientApplicationType.Zendesk => V2alpha3ClientAppTypeEnum.Zendesk,
                    V1ClientApplicationType.Zoom => V2alpha3ClientAppTypeEnum.Zoom,
                    V1ClientApplicationType.ResourceServer => V2alpha3ClientAppTypeEnum.ResourceServer,
                    V1ClientApplicationType.ExpressConfiguration => V2alpha3ClientAppTypeEnum.ExpressConfiguration,
                    V1ClientApplicationType.SsoIntegration => V2alpha3ClientAppTypeEnum.SsoIntegration,
                    V1ClientApplicationType.Oag => V2alpha3ClientAppTypeEnum.Oag,
                    _ => throw new NotImplementedException(),
                };
            }

            static V1ClientApplicationType RevertApplicationType(V2alpha3ClientAppTypeEnum source) => source switch
            {
                V2alpha3ClientAppTypeEnum.Box => V1ClientApplicationType.Box,
                V2alpha3ClientAppTypeEnum.Cloudbees => V1ClientApplicationType.Cloudbees,
                V2alpha3ClientAppTypeEnum.Concur => V1ClientApplicationType.Concur,
                V2alpha3ClientAppTypeEnum.Dropbox => V1ClientApplicationType.Dropbox,
                V2alpha3ClientAppTypeEnum.Echosign => V1ClientApplicationType.Echosign,
                V2alpha3ClientAppTypeEnum.Egnyte => V1ClientApplicationType.Egnyte,
                V2alpha3ClientAppTypeEnum.Mscrm => V1ClientApplicationType.MsCrm,
                V2alpha3ClientAppTypeEnum.Native => V1ClientApplicationType.Native,
                V2alpha3ClientAppTypeEnum.Newrelic => V1ClientApplicationType.NewRelic,
                V2alpha3ClientAppTypeEnum.NonInteractive => V1ClientApplicationType.NonInteractive,
                V2alpha3ClientAppTypeEnum.Office365 => V1ClientApplicationType.Office365,
                V2alpha3ClientAppTypeEnum.RegularWeb => V1ClientApplicationType.RegularWeb,
                V2alpha3ClientAppTypeEnum.Rms => V1ClientApplicationType.Rms,
                V2alpha3ClientAppTypeEnum.Salesforce => V1ClientApplicationType.Salesforce,
                V2alpha3ClientAppTypeEnum.Sentry => V1ClientApplicationType.Sentry,
                V2alpha3ClientAppTypeEnum.Sharepoint => V1ClientApplicationType.SharePoint,
                V2alpha3ClientAppTypeEnum.Slack => V1ClientApplicationType.Slack,
                V2alpha3ClientAppTypeEnum.Springcm => V1ClientApplicationType.SpringCm,
                V2alpha3ClientAppTypeEnum.Spa => V1ClientApplicationType.Spa,
                V2alpha3ClientAppTypeEnum.Zendesk => V1ClientApplicationType.Zendesk,
                V2alpha3ClientAppTypeEnum.Zoom => V1ClientApplicationType.Zoom,
                V2alpha3ClientAppTypeEnum.ResourceServer => V1ClientApplicationType.ResourceServer,
                V2alpha3ClientAppTypeEnum.ExpressConfiguration => V1ClientApplicationType.ExpressConfiguration,
                V2alpha3ClientAppTypeEnum.SsoIntegration => V1ClientApplicationType.SsoIntegration,
                V2alpha3ClientAppTypeEnum.Oag => V1ClientApplicationType.Oag,
                _ => throw new NotImplementedException(),
            };

            static V2alpha3ClientTokenEndpointAuthMethodEnum ConvertTokenEndpointAuthMethod(V1ClientTokenEndpointAuthMethod source) => source switch
            {
                V1ClientTokenEndpointAuthMethod.None => V2alpha3ClientTokenEndpointAuthMethodEnum.None,
                V1ClientTokenEndpointAuthMethod.ClientSecretPost => V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
                V1ClientTokenEndpointAuthMethod.ClientSecretBasic => V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretBasic,
                _ => throw new NotImplementedException(),
            };

            static V1ClientTokenEndpointAuthMethod RevertTokenEndpointAuthMethod(V2alpha3ClientTokenEndpointAuthMethodEnum source) => source switch
            {
                V2alpha3ClientTokenEndpointAuthMethodEnum.None => V1ClientTokenEndpointAuthMethod.None,
                V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost => V1ClientTokenEndpointAuthMethod.ClientSecretPost,
                V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretBasic => V1ClientTokenEndpointAuthMethod.ClientSecretBasic,
                _ => throw new NotImplementedException(),
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

            static ClientEncryptionKey ToApi(V2alpha3ClientEncryptionKey source)
            {
                var result = new ClientEncryptionKey();
                V2alpha3ClientController.ApplyToApi(source, result);
                return result;
            }

            static ClientJwtConfiguration ToApi(V2alpha3ClientJwtConfiguration source)
            {
                var result = new ClientJwtConfiguration();
                V2alpha3ClientController.ApplyToApi(source, result);
                return result;
            }

            static ClientMobile ToApi(V2alpha3ClientMobile source)
            {
                var result = new ClientMobile();
                V2alpha3ClientController.ApplyToApi(source, result);
                return result;
            }

            static ClientOidcBackchannelLogoutSettings ToApi(V2alpha3ClientOidcBackchannelLogoutSettings source)
            {
                var result = new ClientOidcBackchannelLogoutSettings();
                V2alpha3ClientController.ApplyToApi(source, result);
                return result;
            }

            static ClientRefreshTokenConfiguration ToApi(V2alpha3ClientRefreshTokenConfiguration source)
            {
                var result = new ClientRefreshTokenConfiguration
                {
                    RotationType = source.RotationType is { } rotationType ? V2alpha3ClientController.ToApi(rotationType) : new RefreshTokenRotationTypeEnum(RefreshTokenRotationTypeEnum.Values.NonRotating),
                    ExpirationType = source.ExpirationType is { } expirationType ? V2alpha3ClientController.ToApi(expirationType) : new RefreshTokenExpirationTypeEnum(RefreshTokenExpirationTypeEnum.Values.NonExpiring),
                };
                V2alpha3ClientController.ApplyToApi(source, result);
                return result;
            }

            static V1ClientSigningKey RevertSigningKey(V2alpha3ClientSigningKey source)
            {
                var api = new ClientSigningKey
                {
                    Cert = source.Cert,
                    Pkcs7 = source.Pkcs7,
                    Subject = source.Subject,
                };

                return SerializeAndDeserialize<V1ClientSigningKey>(api);
            }

            static V2alpha3ClientAddons? ConvertAddons(V1ClientAddons? source)
            {
                if (source is null)
                    return null;

                var api = Deserialize<ClientAddons>(source);
                return V2alpha3ClientController.FromApi(api);
            }

            static V1ClientAddons? RevertAddons(V2alpha3ClientAddons? source)
            {
                if (source is null)
                    return null;

                return SerializeAndDeserialize<V1ClientAddons>(V2alpha3ClientController.ToApi(source));
            }

            static TTarget? ConvertWithAuth0<TSource, TApi, TTarget>(TSource? source, Func<TApi?, TTarget?> converter)
                where TSource : class
                where TApi : class
                where TTarget : class
            {
                if (source is null)
                    return null;

                var api = Deserialize<TApi>(source);
                return converter(api);
            }

            static TV1? RevertWithAuth0<TSource, TApi, TV1>(TSource? source, Func<TSource, TApi> toApi)
                where TSource : class
                where TApi : class
                where TV1 : class
            {
                if (source is null)
                    return null;

                return SerializeAndDeserialize<TV1>(toApi(source));
            }

            static T Deserialize<T>(object source) where T : class
            {
                return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source, Auth0JsonSerializerOptions), Auth0JsonSerializerOptions)
                    ?? throw new InvalidOperationException($"Unable to deserialize {typeof(T).Name}.");
            }

            static T SerializeAndDeserialize<T>(object source)
            {
                return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source, Auth0JsonSerializerOptions))
                    ?? throw new InvalidOperationException($"Unable to deserialize {typeof(T).Name}.");
            }

            static JsonSerializerOptions CreateAuth0JsonSerializerOptions()
            {
                var type = typeof(GetClientResponseContent).Assembly.GetType("Auth0.ManagementApi.Core.JsonOptions")
                    ?? throw new InvalidOperationException("Unable to locate Auth0.ManagementApi.Core.JsonOptions.");

                return type.GetField("JsonSerializerOptions", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as JsonSerializerOptions
                    ?? throw new InvalidOperationException("Unable to resolve Auth0 JSON serializer options.");
            }

        }

        /// <summary>
        /// Structural (property-copy) conversion between the structurally-identical
        /// <see cref="Models.V2alpha1Client"/> and <see cref="V2alpha3ClientEntity"/> models.
        /// </summary>
        class V2alpha1ToV2alpha3 : IEntityConverter<Models.V2alpha1Client, V2alpha3ClientEntity>
        {

            public V2alpha3ClientEntity Convert(Models.V2alpha1Client from)
            {
                var result = new V2alpha3ClientEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.SecretRef = from.Spec.SecretRef;
                result.Spec.Find = ClientCopy.Convert(from.Spec.Find);
                result.Spec.Init = ClientCopy.Convert(from.Spec.Init);
                result.Spec.Conf = ClientCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = ClientCopy.Convert(from.Status.LastConf);
                return result;
            }

            public Models.V2alpha1Client Revert(V2alpha3ClientEntity source)
            {
                var result = new Models.V2alpha1Client { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.SecretRef = source.Spec.SecretRef;
                result.Spec.Find = ClientCopy.Revert(source.Spec.Find);
                result.Spec.Init = ClientCopy.Revert(source.Spec.Init);
                result.Spec.Conf = ClientCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = ClientCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
