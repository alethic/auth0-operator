using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V2alpha1 and V2alpha3 Client models.</summary>
    internal static class ClientCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3ClientAddonAws? Convert(V2alpha1ClientAddonAws? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonAws
            {
                Principal = s.Principal,
                Role = s.Role,
                LifetimeInSeconds = s.LifetimeInSeconds,
            };
        }

        public static V2alpha1ClientAddonAws? Revert(V2alpha3ClientAddonAws? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonAws
            {
                Principal = s.Principal,
                Role = s.Role,
                LifetimeInSeconds = s.LifetimeInSeconds,
            };
        }

        public static V2alpha3ClientAddonAzureBlob? Convert(V2alpha1ClientAddonAzureBlob? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonAzureBlob
            {
                AccountName = s.AccountName,
                StorageAccessKey = s.StorageAccessKey,
                ContainerName = s.ContainerName,
                BlobName = s.BlobName,
                Expiration = s.Expiration,
                SignedIdentifier = s.SignedIdentifier,
                BlobRead = s.BlobRead,
                BlobWrite = s.BlobWrite,
                BlobDelete = s.BlobDelete,
                ContainerRead = s.ContainerRead,
                ContainerWrite = s.ContainerWrite,
                ContainerDelete = s.ContainerDelete,
                ContainerList = s.ContainerList,
            };
        }

        public static V2alpha1ClientAddonAzureBlob? Revert(V2alpha3ClientAddonAzureBlob? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonAzureBlob
            {
                AccountName = s.AccountName,
                StorageAccessKey = s.StorageAccessKey,
                ContainerName = s.ContainerName,
                BlobName = s.BlobName,
                Expiration = s.Expiration,
                SignedIdentifier = s.SignedIdentifier,
                BlobRead = s.BlobRead,
                BlobWrite = s.BlobWrite,
                BlobDelete = s.BlobDelete,
                ContainerRead = s.ContainerRead,
                ContainerWrite = s.ContainerWrite,
                ContainerDelete = s.ContainerDelete,
                ContainerList = s.ContainerList,
            };
        }

        public static V2alpha3ClientAddonAzureSb? Convert(V2alpha1ClientAddonAzureSb? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonAzureSb
            {
                Namespace = s.Namespace,
                SasKeyName = s.SasKeyName,
                SasKey = s.SasKey,
                EntityPath = s.EntityPath,
                Expiration = s.Expiration,
            };
        }

        public static V2alpha1ClientAddonAzureSb? Revert(V2alpha3ClientAddonAzureSb? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonAzureSb
            {
                Namespace = s.Namespace,
                SasKeyName = s.SasKeyName,
                SasKey = s.SasKey,
                EntityPath = s.EntityPath,
                Expiration = s.Expiration,
            };
        }

        public static V2alpha3ClientAddonEchoSign? Convert(V2alpha1ClientAddonEchoSign? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonEchoSign
            {
                Domain = s.Domain,
            };
        }

        public static V2alpha1ClientAddonEchoSign? Revert(V2alpha3ClientAddonEchoSign? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonEchoSign
            {
                Domain = s.Domain,
            };
        }

        public static V2alpha3ClientAddonEgnyte? Convert(V2alpha1ClientAddonEgnyte? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonEgnyte
            {
                Domain = s.Domain,
            };
        }

        public static V2alpha1ClientAddonEgnyte? Revert(V2alpha3ClientAddonEgnyte? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonEgnyte
            {
                Domain = s.Domain,
            };
        }

        public static V2alpha3ClientAddonFirebase? Convert(V2alpha1ClientAddonFirebase? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonFirebase
            {
                Secret = s.Secret,
                PrivateKeyId = s.PrivateKeyId,
                PrivateKey = s.PrivateKey,
                ClientEmail = s.ClientEmail,
                LifetimeInSeconds = s.LifetimeInSeconds,
            };
        }

        public static V2alpha1ClientAddonFirebase? Revert(V2alpha3ClientAddonFirebase? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonFirebase
            {
                Secret = s.Secret,
                PrivateKeyId = s.PrivateKeyId,
                PrivateKey = s.PrivateKey,
                ClientEmail = s.ClientEmail,
                LifetimeInSeconds = s.LifetimeInSeconds,
            };
        }

        public static V2alpha3ClientAddonLayer? Convert(V2alpha1ClientAddonLayer? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonLayer
            {
                ProviderId = s.ProviderId,
                KeyId = s.KeyId,
                PrivateKey = s.PrivateKey,
                Principal = s.Principal,
                Expiration = s.Expiration,
            };
        }

        public static V2alpha1ClientAddonLayer? Revert(V2alpha3ClientAddonLayer? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonLayer
            {
                ProviderId = s.ProviderId,
                KeyId = s.KeyId,
                PrivateKey = s.PrivateKey,
                Principal = s.Principal,
                Expiration = s.Expiration,
            };
        }

        public static V2alpha3ClientAddonMscrm? Convert(V2alpha1ClientAddonMscrm? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonMscrm
            {
                Url = s.Url,
            };
        }

        public static V2alpha1ClientAddonMscrm? Revert(V2alpha3ClientAddonMscrm? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonMscrm
            {
                Url = s.Url,
            };
        }

        public static V2alpha3ClientAddonNewRelic? Convert(V2alpha1ClientAddonNewRelic? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonNewRelic
            {
                Account = s.Account,
            };
        }

        public static V2alpha1ClientAddonNewRelic? Revert(V2alpha3ClientAddonNewRelic? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonNewRelic
            {
                Account = s.Account,
            };
        }

        public static V2alpha3ClientAddonOag? Convert(V2alpha1ClientAddonOag? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonOag
            {
            };
        }

        public static V2alpha1ClientAddonOag? Revert(V2alpha3ClientAddonOag? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonOag
            {
            };
        }

        public static V2alpha3ClientAddonOffice365? Convert(V2alpha1ClientAddonOffice365? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonOffice365
            {
                Domain = s.Domain,
                Connection = s.Connection,
            };
        }

        public static V2alpha1ClientAddonOffice365? Revert(V2alpha3ClientAddonOffice365? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonOffice365
            {
                Domain = s.Domain,
                Connection = s.Connection,
            };
        }

        public static V2alpha3ClientAddonRms? Convert(V2alpha1ClientAddonRms? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonRms
            {
                Url = s.Url,
            };
        }

        public static V2alpha1ClientAddonRms? Revert(V2alpha3ClientAddonRms? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonRms
            {
                Url = s.Url,
            };
        }

        public static V2alpha3ClientAddonSalesforce? Convert(V2alpha1ClientAddonSalesforce? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSalesforce
            {
                EntityId = s.EntityId,
            };
        }

        public static V2alpha1ClientAddonSalesforce? Revert(V2alpha3ClientAddonSalesforce? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSalesforce
            {
                EntityId = s.EntityId,
            };
        }

        public static V2alpha3ClientAddonSalesforceApi? Convert(V2alpha1ClientAddonSalesforceApi? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSalesforceApi
            {
                Clientid = s.Clientid,
                Principal = s.Principal,
                CommunityName = s.CommunityName,
                CommunityUrlSection = s.CommunityUrlSection,
            };
        }

        public static V2alpha1ClientAddonSalesforceApi? Revert(V2alpha3ClientAddonSalesforceApi? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSalesforceApi
            {
                Clientid = s.Clientid,
                Principal = s.Principal,
                CommunityName = s.CommunityName,
                CommunityUrlSection = s.CommunityUrlSection,
            };
        }

        public static V2alpha3ClientAddonSalesforceSandboxApi? Convert(V2alpha1ClientAddonSalesforceSandboxApi? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSalesforceSandboxApi
            {
                Clientid = s.Clientid,
                Principal = s.Principal,
                CommunityName = s.CommunityName,
                CommunityUrlSection = s.CommunityUrlSection,
            };
        }

        public static V2alpha1ClientAddonSalesforceSandboxApi? Revert(V2alpha3ClientAddonSalesforceSandboxApi? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSalesforceSandboxApi
            {
                Clientid = s.Clientid,
                Principal = s.Principal,
                CommunityName = s.CommunityName,
                CommunityUrlSection = s.CommunityUrlSection,
            };
        }

        public static V2alpha3ClientAddonSaml? Convert(V2alpha1ClientAddonSaml? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSaml
            {
                Mappings = s.Mappings,
                Audience = s.Audience,
                Recipient = s.Recipient,
                CreateUpnClaim = s.CreateUpnClaim,
                MapUnknownClaimsAsIs = s.MapUnknownClaimsAsIs,
                PassthroughClaimsWithNoMapping = s.PassthroughClaimsWithNoMapping,
                MapIdentities = s.MapIdentities,
                SignatureAlgorithm = s.SignatureAlgorithm,
                DigestAlgorithm = s.DigestAlgorithm,
                Issuer = s.Issuer,
                Destination = s.Destination,
                LifetimeInSeconds = s.LifetimeInSeconds,
                SignResponse = s.SignResponse,
                NameIdentifierFormat = s.NameIdentifierFormat,
                NameIdentifierProbes = s.NameIdentifierProbes,
                AuthnContextClassRef = s.AuthnContextClassRef,
            };
        }

        public static V2alpha1ClientAddonSaml? Revert(V2alpha3ClientAddonSaml? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSaml
            {
                Mappings = s.Mappings,
                Audience = s.Audience,
                Recipient = s.Recipient,
                CreateUpnClaim = s.CreateUpnClaim,
                MapUnknownClaimsAsIs = s.MapUnknownClaimsAsIs,
                PassthroughClaimsWithNoMapping = s.PassthroughClaimsWithNoMapping,
                MapIdentities = s.MapIdentities,
                SignatureAlgorithm = s.SignatureAlgorithm,
                DigestAlgorithm = s.DigestAlgorithm,
                Issuer = s.Issuer,
                Destination = s.Destination,
                LifetimeInSeconds = s.LifetimeInSeconds,
                SignResponse = s.SignResponse,
                NameIdentifierFormat = s.NameIdentifierFormat,
                NameIdentifierProbes = s.NameIdentifierProbes,
                AuthnContextClassRef = s.AuthnContextClassRef,
            };
        }

        public static V2alpha3ClientAddonSapapi? Convert(V2alpha1ClientAddonSapapi? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSapapi
            {
                Clientid = s.Clientid,
                UsernameAttribute = s.UsernameAttribute,
                TokenEndpointUrl = s.TokenEndpointUrl,
                Scope = s.Scope,
                ServicePassword = s.ServicePassword,
                NameIdentifierFormat = s.NameIdentifierFormat,
            };
        }

        public static V2alpha1ClientAddonSapapi? Revert(V2alpha3ClientAddonSapapi? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSapapi
            {
                Clientid = s.Clientid,
                UsernameAttribute = s.UsernameAttribute,
                TokenEndpointUrl = s.TokenEndpointUrl,
                Scope = s.Scope,
                ServicePassword = s.ServicePassword,
                NameIdentifierFormat = s.NameIdentifierFormat,
            };
        }

        public static V2alpha3ClientAddonSentry? Convert(V2alpha1ClientAddonSentry? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSentry
            {
                OrgSlug = s.OrgSlug,
                BaseUrl = s.BaseUrl,
            };
        }

        public static V2alpha1ClientAddonSentry? Revert(V2alpha3ClientAddonSentry? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSentry
            {
                OrgSlug = s.OrgSlug,
                BaseUrl = s.BaseUrl,
            };
        }

        public static V2alpha3ClientAddonSharePoint? Convert(V2alpha1ClientAddonSharePoint? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSharePoint
            {
                Url = s.Url,
                ExternalUrl = s.ExternalUrl,
            };
        }

        public static V2alpha1ClientAddonSharePoint? Revert(V2alpha3ClientAddonSharePoint? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSharePoint
            {
                Url = s.Url,
                ExternalUrl = s.ExternalUrl,
            };
        }

        public static V2alpha3ClientAddonSharePointExternalUrl? Convert(V2alpha1ClientAddonSharePointExternalUrl? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSharePointExternalUrl
            {
                Type = s.Type,
                Value = s.Value,
            };
        }

        public static V2alpha1ClientAddonSharePointExternalUrl? Revert(V2alpha3ClientAddonSharePointExternalUrl? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSharePointExternalUrl
            {
                Type = s.Type,
                Value = s.Value,
            };
        }

        public static V2alpha3ClientAddonSlack? Convert(V2alpha1ClientAddonSlack? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSlack
            {
                Team = s.Team,
            };
        }

        public static V2alpha1ClientAddonSlack? Revert(V2alpha3ClientAddonSlack? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSlack
            {
                Team = s.Team,
            };
        }

        public static V2alpha3ClientAddonSpringCm? Convert(V2alpha1ClientAddonSpringCm? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSpringCm
            {
                Acsurl = s.Acsurl,
            };
        }

        public static V2alpha1ClientAddonSpringCm? Revert(V2alpha3ClientAddonSpringCm? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSpringCm
            {
                Acsurl = s.Acsurl,
            };
        }

        public static V2alpha3ClientAddonSsoIntegration? Convert(V2alpha1ClientAddonSsoIntegration? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonSsoIntegration
            {
                Name = s.Name,
                Version = s.Version,
            };
        }

        public static V2alpha1ClientAddonSsoIntegration? Revert(V2alpha3ClientAddonSsoIntegration? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonSsoIntegration
            {
                Name = s.Name,
                Version = s.Version,
            };
        }

        public static V2alpha3ClientAddonWams? Convert(V2alpha1ClientAddonWams? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonWams
            {
                Masterkey = s.Masterkey,
            };
        }

        public static V2alpha1ClientAddonWams? Revert(V2alpha3ClientAddonWams? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonWams
            {
                Masterkey = s.Masterkey,
            };
        }

        public static V2alpha3ClientAddonZendesk? Convert(V2alpha1ClientAddonZendesk? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonZendesk
            {
                AccountName = s.AccountName,
            };
        }

        public static V2alpha1ClientAddonZendesk? Revert(V2alpha3ClientAddonZendesk? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonZendesk
            {
                AccountName = s.AccountName,
            };
        }

        public static V2alpha3ClientAddonZoom? Convert(V2alpha1ClientAddonZoom? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddonZoom
            {
                Account = s.Account,
            };
        }

        public static V2alpha1ClientAddonZoom? Revert(V2alpha3ClientAddonZoom? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddonZoom
            {
                Account = s.Account,
            };
        }

        public static V2alpha3ClientAddons? Convert(V2alpha1ClientAddons? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientAddons
            {
                Aws = Convert(s.Aws),
                AzureBlob = Convert(s.AzureBlob),
                AzureSb = Convert(s.AzureSb),
                Rms = Convert(s.Rms),
                Mscrm = Convert(s.Mscrm),
                Slack = Convert(s.Slack),
                Sentry = Convert(s.Sentry),
                Box = s.Box,
                Cloudbees = s.Cloudbees,
                Concur = s.Concur,
                Dropbox = s.Dropbox,
                Echosign = Convert(s.Echosign),
                Egnyte = Convert(s.Egnyte),
                Firebase = Convert(s.Firebase),
                Newrelic = Convert(s.Newrelic),
                Office365 = Convert(s.Office365),
                Salesforce = Convert(s.Salesforce),
                SalesforceApi = Convert(s.SalesforceApi),
                SalesforceSandboxApi = Convert(s.SalesforceSandboxApi),
                Samlp = Convert(s.Samlp),
                Layer = Convert(s.Layer),
                SapApi = Convert(s.SapApi),
                Sharepoint = Convert(s.Sharepoint),
                Springcm = Convert(s.Springcm),
                Wams = Convert(s.Wams),
                Wsfed = s.Wsfed,
                Zendesk = Convert(s.Zendesk),
                Zoom = Convert(s.Zoom),
                SsoIntegration = Convert(s.SsoIntegration),
                Oag = Convert(s.Oag),
            };
        }

        public static V2alpha1ClientAddons? Revert(V2alpha3ClientAddons? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientAddons
            {
                Aws = Revert(s.Aws),
                AzureBlob = Revert(s.AzureBlob),
                AzureSb = Revert(s.AzureSb),
                Rms = Revert(s.Rms),
                Mscrm = Revert(s.Mscrm),
                Slack = Revert(s.Slack),
                Sentry = Revert(s.Sentry),
                Box = s.Box,
                Cloudbees = s.Cloudbees,
                Concur = s.Concur,
                Dropbox = s.Dropbox,
                Echosign = Revert(s.Echosign),
                Egnyte = Revert(s.Egnyte),
                Firebase = Revert(s.Firebase),
                Newrelic = Revert(s.Newrelic),
                Office365 = Revert(s.Office365),
                Salesforce = Revert(s.Salesforce),
                SalesforceApi = Revert(s.SalesforceApi),
                SalesforceSandboxApi = Revert(s.SalesforceSandboxApi),
                Samlp = Revert(s.Samlp),
                Layer = Revert(s.Layer),
                SapApi = Revert(s.SapApi),
                Sharepoint = Revert(s.Sharepoint),
                Springcm = Revert(s.Springcm),
                Wams = Revert(s.Wams),
                Wsfed = s.Wsfed,
                Zendesk = Revert(s.Zendesk),
                Zoom = Revert(s.Zoom),
                SsoIntegration = Revert(s.SsoIntegration),
                Oag = Revert(s.Oag),
            };
        }

        public static V2alpha3ClientConf? Convert(V2alpha1ClientConf? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientConf
            {
                SigningKeys = s.SigningKeys?.Select(__x => Convert(__x)!).ToArray(),
                ApplicationType = MapEnumN<V2alpha1ClientAppTypeEnum, V2alpha3ClientAppTypeEnum>(s.ApplicationType),
                TokenEndpointAuthMethod = MapEnumN<V2alpha1ClientTokenEndpointAuthMethodEnum, V2alpha3ClientTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                AddOns = Convert(s.AddOns),
                AllowedClients = s.AllowedClients,
                AllowedLogoutUrls = s.AllowedLogoutUrls,
                AllowedOrigins = s.AllowedOrigins,
                WebOrigins = s.WebOrigins,
                InitiateLoginUri = s.InitiateLoginUri,
                Callbacks = s.Callbacks,
                SkipNonVerifiableCallbackUriConfirmationPrompt = s.SkipNonVerifiableCallbackUriConfirmationPrompt,
                ClientAliases = s.ClientAliases,
                ClientMetaData = s.ClientMetaData,
                IsCustomLoginPageOn = s.IsCustomLoginPageOn,
                IsFirstParty = s.IsFirstParty,
                CustomLoginPage = s.CustomLoginPage,
                CustomLoginPagePreview = s.CustomLoginPagePreview,
                EncryptionKey = Convert(s.EncryptionKey),
                FormTemplate = s.FormTemplate,
                GrantTypes = s.GrantTypes,
                JwtConfiguration = Convert(s.JwtConfiguration),
                Mobile = Convert(s.Mobile),
                Name = s.Name,
                Description = s.Description,
                LogoUri = s.LogoUri,
                OidcConformant = s.OidcConformant,
                OidcLogout = Convert(s.OidcLogout),
                ResourceServers = s.ResourceServers,
                Sso = s.Sso,
                RefreshToken = Convert(s.RefreshToken),
                OrganizationUsage = MapEnumN<V2alpha1ClientOrganizationUsageEnum, V2alpha3ClientOrganizationUsageEnum>(s.OrganizationUsage),
                OrganizationRequireBehavior = MapEnumN<V2alpha1ClientOrganizationRequireBehaviorEnum, V2alpha3ClientOrganizationRequireBehaviorEnum>(s.OrganizationRequireBehavior),
                CrossOriginAuthentication = s.CrossOriginAuthentication,
                RequirePushedAuthorizationRequests = s.RequirePushedAuthorizationRequests,
                DefaultOrganization = Convert(s.DefaultOrganization),
                ComplianceLevel = MapEnumN<V2alpha1ClientComplianceLevelEnum, V2alpha3ClientComplianceLevelEnum>(s.ComplianceLevel),
                RequireProofOfPossession = s.RequireProofOfPossession,
            };
        }

        public static V2alpha1ClientConf? Revert(V2alpha3ClientConf? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientConf
            {
                SigningKeys = s.SigningKeys?.Select(__x => Revert(__x)!).ToArray(),
                ApplicationType = MapEnumN<V2alpha3ClientAppTypeEnum, V2alpha1ClientAppTypeEnum>(s.ApplicationType),
                TokenEndpointAuthMethod = MapEnumN<V2alpha3ClientTokenEndpointAuthMethodEnum, V2alpha1ClientTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                AddOns = Revert(s.AddOns),
                AllowedClients = s.AllowedClients,
                AllowedLogoutUrls = s.AllowedLogoutUrls,
                AllowedOrigins = s.AllowedOrigins,
                WebOrigins = s.WebOrigins,
                InitiateLoginUri = s.InitiateLoginUri,
                Callbacks = s.Callbacks,
                SkipNonVerifiableCallbackUriConfirmationPrompt = s.SkipNonVerifiableCallbackUriConfirmationPrompt,
                ClientAliases = s.ClientAliases,
                ClientMetaData = s.ClientMetaData,
                IsCustomLoginPageOn = s.IsCustomLoginPageOn,
                IsFirstParty = s.IsFirstParty,
                CustomLoginPage = s.CustomLoginPage,
                CustomLoginPagePreview = s.CustomLoginPagePreview,
                EncryptionKey = Revert(s.EncryptionKey),
                FormTemplate = s.FormTemplate,
                GrantTypes = s.GrantTypes,
                JwtConfiguration = Revert(s.JwtConfiguration),
                Mobile = Revert(s.Mobile),
                Name = s.Name,
                Description = s.Description,
                LogoUri = s.LogoUri,
                OidcConformant = s.OidcConformant,
                OidcLogout = Revert(s.OidcLogout),
                ResourceServers = s.ResourceServers,
                Sso = s.Sso,
                RefreshToken = Revert(s.RefreshToken),
                OrganizationUsage = MapEnumN<V2alpha3ClientOrganizationUsageEnum, V2alpha1ClientOrganizationUsageEnum>(s.OrganizationUsage),
                OrganizationRequireBehavior = MapEnumN<V2alpha3ClientOrganizationRequireBehaviorEnum, V2alpha1ClientOrganizationRequireBehaviorEnum>(s.OrganizationRequireBehavior),
                CrossOriginAuthentication = s.CrossOriginAuthentication,
                RequirePushedAuthorizationRequests = s.RequirePushedAuthorizationRequests,
                DefaultOrganization = Revert(s.DefaultOrganization),
                ComplianceLevel = MapEnumN<V2alpha3ClientComplianceLevelEnum, V2alpha1ClientComplianceLevelEnum>(s.ComplianceLevel),
                RequireProofOfPossession = s.RequireProofOfPossession,
            };
        }

        public static V2alpha3ClientDefaultOrganization? Convert(V2alpha1ClientDefaultOrganization? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientDefaultOrganization
            {
                OrganizationId = s.OrganizationId,
                Flows = s.Flows?.Select(__x => MapEnum<V2alpha1ClientDefaultOrganizationFlowsEnum, V2alpha3ClientDefaultOrganizationFlowsEnum>(__x)).ToArray(),
            };
        }

        public static V2alpha1ClientDefaultOrganization? Revert(V2alpha3ClientDefaultOrganization? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientDefaultOrganization
            {
                OrganizationId = s.OrganizationId,
                Flows = s.Flows?.Select(__x => MapEnum<V2alpha3ClientDefaultOrganizationFlowsEnum, V2alpha1ClientDefaultOrganizationFlowsEnum>(__x)).ToArray(),
            };
        }

        public static V2alpha3ClientEncryptionKey? Convert(V2alpha1ClientEncryptionKey? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientEncryptionKey
            {
                Pub = s.Pub,
                Cert = s.Cert,
                Subject = s.Subject,
            };
        }

        public static V2alpha1ClientEncryptionKey? Revert(V2alpha3ClientEncryptionKey? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientEncryptionKey
            {
                Pub = s.Pub,
                Cert = s.Cert,
                Subject = s.Subject,
            };
        }

        public static V2alpha3ClientFind? Convert(V2alpha1ClientFind? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientFind
            {
                ClientId = s.ClientId,
                Name = s.Name,
            };
        }

        public static V2alpha1ClientFind? Revert(V2alpha3ClientFind? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientFind
            {
                ClientId = s.ClientId,
                Name = s.Name,
            };
        }

        public static V2alpha3ClientJwtConfiguration? Convert(V2alpha1ClientJwtConfiguration? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientJwtConfiguration
            {
                LifetimeInSeconds = s.LifetimeInSeconds,
                SecretEncoded = s.SecretEncoded,
                Scopes = s.Scopes,
                Alg = MapEnumN<V2alpha1ClientSigningAlgorithmEnum, V2alpha3ClientSigningAlgorithmEnum>(s.Alg),
            };
        }

        public static V2alpha1ClientJwtConfiguration? Revert(V2alpha3ClientJwtConfiguration? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientJwtConfiguration
            {
                LifetimeInSeconds = s.LifetimeInSeconds,
                SecretEncoded = s.SecretEncoded,
                Scopes = s.Scopes,
                Alg = MapEnumN<V2alpha3ClientSigningAlgorithmEnum, V2alpha1ClientSigningAlgorithmEnum>(s.Alg),
            };
        }

        public static V2alpha3ClientMobile? Convert(V2alpha1ClientMobile? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientMobile
            {
                Android = Convert(s.Android),
                Ios = Convert(s.Ios),
            };
        }

        public static V2alpha1ClientMobile? Revert(V2alpha3ClientMobile? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientMobile
            {
                Android = Revert(s.Android),
                Ios = Revert(s.Ios),
            };
        }

        public static V2alpha3ClientMobileAndroid? Convert(V2alpha1ClientMobileAndroid? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientMobileAndroid
            {
                AppPackageName = s.AppPackageName,
                Sha256CertFingerprints = s.Sha256CertFingerprints,
            };
        }

        public static V2alpha1ClientMobileAndroid? Revert(V2alpha3ClientMobileAndroid? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientMobileAndroid
            {
                AppPackageName = s.AppPackageName,
                Sha256CertFingerprints = s.Sha256CertFingerprints,
            };
        }

        public static V2alpha3ClientMobileiOs? Convert(V2alpha1ClientMobileiOs? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientMobileiOs
            {
                TeamId = s.TeamId,
                AppBundleIdentifier = s.AppBundleIdentifier,
            };
        }

        public static V2alpha1ClientMobileiOs? Revert(V2alpha3ClientMobileiOs? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientMobileiOs
            {
                TeamId = s.TeamId,
                AppBundleIdentifier = s.AppBundleIdentifier,
            };
        }

        public static V2alpha3ClientOidcBackchannelLogoutInitiators? Convert(V2alpha1ClientOidcBackchannelLogoutInitiators? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientOidcBackchannelLogoutInitiators
            {
                Mode = MapEnumN<V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum, V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum>(s.Mode),
                SelectedInitiators = s.SelectedInitiators?.Select(__x => MapEnum<V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum, V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum>(__x)).ToArray(),
            };
        }

        public static V2alpha1ClientOidcBackchannelLogoutInitiators? Revert(V2alpha3ClientOidcBackchannelLogoutInitiators? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientOidcBackchannelLogoutInitiators
            {
                Mode = MapEnumN<V2alpha3ClientOidcBackchannelLogoutInitiatorsModeEnum, V2alpha1ClientOidcBackchannelLogoutInitiatorsModeEnum>(s.Mode),
                SelectedInitiators = s.SelectedInitiators?.Select(__x => MapEnum<V2alpha3ClientOidcBackchannelLogoutInitiatorsEnum, V2alpha1ClientOidcBackchannelLogoutInitiatorsEnum>(__x)).ToArray(),
            };
        }

        public static V2alpha3ClientOidcBackchannelLogoutSessionMetadata? Convert(V2alpha1ClientOidcBackchannelLogoutSessionMetadata? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientOidcBackchannelLogoutSessionMetadata
            {
                Include = s.Include,
            };
        }

        public static V2alpha1ClientOidcBackchannelLogoutSessionMetadata? Revert(V2alpha3ClientOidcBackchannelLogoutSessionMetadata? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientOidcBackchannelLogoutSessionMetadata
            {
                Include = s.Include,
            };
        }

        public static V2alpha3ClientOidcBackchannelLogoutSettings? Convert(V2alpha1ClientOidcBackchannelLogoutSettings? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientOidcBackchannelLogoutSettings
            {
                BackchannelLogoutUrls = s.BackchannelLogoutUrls,
                BackchannelLogoutInitiators = Convert(s.BackchannelLogoutInitiators),
                BackchannelLogoutSessionMetadata = Convert(s.BackchannelLogoutSessionMetadata),
            };
        }

        public static V2alpha1ClientOidcBackchannelLogoutSettings? Revert(V2alpha3ClientOidcBackchannelLogoutSettings? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientOidcBackchannelLogoutSettings
            {
                BackchannelLogoutUrls = s.BackchannelLogoutUrls,
                BackchannelLogoutInitiators = Revert(s.BackchannelLogoutInitiators),
                BackchannelLogoutSessionMetadata = Revert(s.BackchannelLogoutSessionMetadata),
            };
        }

        public static V2alpha3ClientRefreshTokenConfiguration? Convert(V2alpha1ClientRefreshTokenConfiguration? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientRefreshTokenConfiguration
            {
                RotationType = MapEnumN<V2alpha1ClientRefreshTokenRotationTypeEnum, V2alpha3ClientRefreshTokenRotationTypeEnum>(s.RotationType),
                ExpirationType = MapEnumN<V2alpha1ClientRefreshTokenExpirationTypeEnum, V2alpha3ClientRefreshTokenExpirationTypeEnum>(s.ExpirationType),
                Leeway = s.Leeway,
                TokenLifetime = s.TokenLifetime,
                InfiniteTokenLifetime = s.InfiniteTokenLifetime,
                IdleTokenLifetime = s.IdleTokenLifetime,
                InfiniteIdleTokenLifetime = s.InfiniteIdleTokenLifetime,
                Policies = s.Policies?.Select(__x => Convert(__x)!).ToArray(),
            };
        }

        public static V2alpha1ClientRefreshTokenConfiguration? Revert(V2alpha3ClientRefreshTokenConfiguration? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientRefreshTokenConfiguration
            {
                RotationType = MapEnumN<V2alpha3ClientRefreshTokenRotationTypeEnum, V2alpha1ClientRefreshTokenRotationTypeEnum>(s.RotationType),
                ExpirationType = MapEnumN<V2alpha3ClientRefreshTokenExpirationTypeEnum, V2alpha1ClientRefreshTokenExpirationTypeEnum>(s.ExpirationType),
                Leeway = s.Leeway,
                TokenLifetime = s.TokenLifetime,
                InfiniteTokenLifetime = s.InfiniteTokenLifetime,
                IdleTokenLifetime = s.IdleTokenLifetime,
                InfiniteIdleTokenLifetime = s.InfiniteIdleTokenLifetime,
                Policies = s.Policies?.Select(__x => Revert(__x)!).ToArray(),
            };
        }

        public static V2alpha3ClientRefreshTokenPolicy? Convert(V2alpha1ClientRefreshTokenPolicy? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientRefreshTokenPolicy
            {
                Audience = s.Audience,
                Scope = s.Scope,
            };
        }

        public static V2alpha1ClientRefreshTokenPolicy? Revert(V2alpha3ClientRefreshTokenPolicy? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientRefreshTokenPolicy
            {
                Audience = s.Audience,
                Scope = s.Scope,
            };
        }

        public static V2alpha3ClientSigningKey? Convert(V2alpha1ClientSigningKey? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientSigningKey
            {
                Pkcs7 = s.Pkcs7,
                Cert = s.Cert,
                Subject = s.Subject,
            };
        }

        public static V2alpha1ClientSigningKey? Revert(V2alpha3ClientSigningKey? s)
        {
            if (s is null) return null;
            return new V2alpha1ClientSigningKey
            {
                Pkcs7 = s.Pkcs7,
                Cert = s.Cert,
                Subject = s.Subject,
            };
        }

    }

}
