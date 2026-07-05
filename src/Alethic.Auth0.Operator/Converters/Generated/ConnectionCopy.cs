using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V2alpha1 and V2alpha3 Connection models.</summary>
    internal static class ConnectionCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3ConnectionAssertionDecryptionSettings? Convert(V2alpha1ConnectionAssertionDecryptionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionAssertionDecryptionSettings
            {
                AlgorithmProfile = MapEnumN<V2alpha1ConnectionAssertionDecryptionAlgorithmProfileEnum, V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum>(s.AlgorithmProfile),
                AlgorithmExceptions = s.AlgorithmExceptions,
            };
        }

        public static V2alpha1ConnectionAssertionDecryptionSettings? Revert(V2alpha3ConnectionAssertionDecryptionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionAssertionDecryptionSettings
            {
                AlgorithmProfile = MapEnumN<V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum, V2alpha1ConnectionAssertionDecryptionAlgorithmProfileEnum>(s.AlgorithmProfile),
                AlgorithmExceptions = s.AlgorithmExceptions,
            };
        }

        public static V2alpha3ConnectionAttributeIdentifier? Convert(V2alpha1ConnectionAttributeIdentifier? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionAttributeIdentifier
            {
                Active = s.Active,
                DefaultMethod = MapEnumN<V2alpha1ConnectionDefaultMethodEmailIdentifierEnum, V2alpha3ConnectionDefaultMethodEmailIdentifierEnum>(s.DefaultMethod),
            };
        }

        public static V2alpha1ConnectionAttributeIdentifier? Revert(V2alpha3ConnectionAttributeIdentifier? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionAttributeIdentifier
            {
                Active = s.Active,
                DefaultMethod = MapEnumN<V2alpha3ConnectionDefaultMethodEmailIdentifierEnum, V2alpha1ConnectionDefaultMethodEmailIdentifierEnum>(s.DefaultMethod),
            };
        }

        public static V2alpha3ConnectionAttributeMapOidc? Convert(V2alpha1ConnectionAttributeMapOidc? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionAttributeMapOidc
            {
                Attributes = s.Attributes,
                MappingMode = MapEnumN<V2alpha1ConnectionMappingModeEnumOidc, V2alpha3ConnectionMappingModeEnumOidc>(s.MappingMode),
                UserinfoScope = s.UserinfoScope,
            };
        }

        public static V2alpha1ConnectionAttributeMapOidc? Revert(V2alpha3ConnectionAttributeMapOidc? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionAttributeMapOidc
            {
                Attributes = s.Attributes,
                MappingMode = MapEnumN<V2alpha3ConnectionMappingModeEnumOidc, V2alpha1ConnectionMappingModeEnumOidc>(s.MappingMode),
                UserinfoScope = s.UserinfoScope,
            };
        }

        public static V2alpha3ConnectionAttributeMapOkta? Convert(V2alpha1ConnectionAttributeMapOkta? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionAttributeMapOkta
            {
                Attributes = s.Attributes,
                MappingMode = MapEnumN<V2alpha1ConnectionMappingModeEnumOkta, V2alpha3ConnectionMappingModeEnumOkta>(s.MappingMode),
                UserinfoScope = s.UserinfoScope,
            };
        }

        public static V2alpha1ConnectionAttributeMapOkta? Revert(V2alpha3ConnectionAttributeMapOkta? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionAttributeMapOkta
            {
                Attributes = s.Attributes,
                MappingMode = MapEnumN<V2alpha3ConnectionMappingModeEnumOkta, V2alpha1ConnectionMappingModeEnumOkta>(s.MappingMode),
                UserinfoScope = s.UserinfoScope,
            };
        }

        public static V2alpha3ConnectionAttributes? Convert(V2alpha1ConnectionAttributes? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionAttributes
            {
                Email = Convert(s.Email),
                PhoneNumber = Convert(s.PhoneNumber),
                Username = Convert(s.Username),
            };
        }

        public static V2alpha1ConnectionAttributes? Revert(V2alpha3ConnectionAttributes? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionAttributes
            {
                Email = Revert(s.Email),
                PhoneNumber = Revert(s.PhoneNumber),
                Username = Revert(s.Username),
            };
        }

        public static V2alpha3ConnectionAuthenticationMethods? Convert(V2alpha1ConnectionAuthenticationMethods? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionAuthenticationMethods
            {
                Password = Convert(s.Password),
                Passkey = Convert(s.Passkey),
                EmailOtp = Convert(s.EmailOtp),
                PhoneOtp = Convert(s.PhoneOtp),
            };
        }

        public static V2alpha1ConnectionAuthenticationMethods? Revert(V2alpha3ConnectionAuthenticationMethods? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionAuthenticationMethods
            {
                Password = Revert(s.Password),
                Passkey = Revert(s.Passkey),
                EmailOtp = Revert(s.EmailOtp),
                PhoneOtp = Revert(s.PhoneOtp),
            };
        }

        public static V2alpha3ConnectionConf? Convert(V2alpha1ConnectionConf? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionConf
            {
                Name = s.Name,
                DisplayName = s.DisplayName,
                Strategy = MapEnumN<V2alpha1ConnectionStrategy, V2alpha3ConnectionStrategy>(s.Strategy),
                ProvisioningTicketUrl = s.ProvisioningTicketUrl,
                Metadata = s.Metadata,
                Realms = s.Realms,
                EnabledClients = s.EnabledClients,
                ShowAsButton = s.ShowAsButton,
                IsDomainConnection = s.IsDomainConnection,
                Options = Convert(s.Options),
            };
        }

        public static V2alpha1ConnectionConf? Revert(V2alpha3ConnectionConf? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionConf
            {
                Name = s.Name,
                DisplayName = s.DisplayName,
                Strategy = MapEnumN<V2alpha3ConnectionStrategy, V2alpha1ConnectionStrategy>(s.Strategy),
                ProvisioningTicketUrl = s.ProvisioningTicketUrl,
                Metadata = s.Metadata,
                Realms = s.Realms,
                EnabledClients = s.EnabledClients,
                ShowAsButton = s.ShowAsButton,
                IsDomainConnection = s.IsDomainConnection,
                Options = Revert(s.Options),
            };
        }

        public static V2alpha3ConnectionConnectionSettings? Convert(V2alpha1ConnectionConnectionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionConnectionSettings
            {
                Pkce = MapEnumN<V2alpha1ConnectionConnectionSettingsPkceEnum, V2alpha3ConnectionConnectionSettingsPkceEnum>(s.Pkce),
            };
        }

        public static V2alpha1ConnectionConnectionSettings? Revert(V2alpha3ConnectionConnectionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionConnectionSettings
            {
                Pkce = MapEnumN<V2alpha3ConnectionConnectionSettingsPkceEnum, V2alpha1ConnectionConnectionSettingsPkceEnum>(s.Pkce),
            };
        }

        public static V2alpha3ConnectionCustomScripts? Convert(V2alpha1ConnectionCustomScripts? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionCustomScripts
            {
                Login = s.Login,
                GetUser = s.GetUser,
                Delete = s.Delete,
                ChangePassword = s.ChangePassword,
                Verify = s.Verify,
                Create = s.Create,
                ChangeUsername = s.ChangeUsername,
                ChangeEmail = s.ChangeEmail,
                ChangePhoneNumber = s.ChangePhoneNumber,
            };
        }

        public static V2alpha1ConnectionCustomScripts? Revert(V2alpha3ConnectionCustomScripts? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionCustomScripts
            {
                Login = s.Login,
                GetUser = s.GetUser,
                Delete = s.Delete,
                ChangePassword = s.ChangePassword,
                Verify = s.Verify,
                Create = s.Create,
                ChangeUsername = s.ChangeUsername,
                ChangeEmail = s.ChangeEmail,
                ChangePhoneNumber = s.ChangePhoneNumber,
            };
        }

        public static V2alpha3ConnectionDecryptionKeySaml? Convert(V2alpha1ConnectionDecryptionKeySaml? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionDecryptionKeySaml
            {
                PrivateKey = s.PrivateKey,
                KeyPair = Convert(s.KeyPair),
            };
        }

        public static V2alpha1ConnectionDecryptionKeySaml? Revert(V2alpha3ConnectionDecryptionKeySaml? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionDecryptionKeySaml
            {
                PrivateKey = s.PrivateKey,
                KeyPair = Revert(s.KeyPair),
            };
        }

        public static V2alpha3ConnectionDecryptionKeySamlCert? Convert(V2alpha1ConnectionDecryptionKeySamlCert? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionDecryptionKeySamlCert
            {
                Cert = s.Cert,
                Key = s.Key,
            };
        }

        public static V2alpha1ConnectionDecryptionKeySamlCert? Revert(V2alpha3ConnectionDecryptionKeySamlCert? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionDecryptionKeySamlCert
            {
                Cert = s.Cert,
                Key = s.Key,
            };
        }

        public static V2alpha3ConnectionEmailAttribute? Convert(V2alpha1ConnectionEmailAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionEmailAttribute
            {
                Identifier = Convert(s.Identifier),
                Unique = s.Unique,
                ProfileRequired = s.ProfileRequired,
                VerificationMethod = MapEnumN<V2alpha1ConnectionVerificationMethodEnum, V2alpha3ConnectionVerificationMethodEnum>(s.VerificationMethod),
                Signup = Convert(s.Signup),
            };
        }

        public static V2alpha1ConnectionEmailAttribute? Revert(V2alpha3ConnectionEmailAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionEmailAttribute
            {
                Identifier = Revert(s.Identifier),
                Unique = s.Unique,
                ProfileRequired = s.ProfileRequired,
                VerificationMethod = MapEnumN<V2alpha3ConnectionVerificationMethodEnum, V2alpha1ConnectionVerificationMethodEnum>(s.VerificationMethod),
                Signup = Revert(s.Signup),
            };
        }

        public static V2alpha3ConnectionEmailEmail? Convert(V2alpha1ConnectionEmailEmail? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionEmailEmail
            {
                Body = s.Body,
                From = s.From,
                Subject = s.Subject,
                Syntax = MapEnumN<V2alpha1ConnectionEmailEmailSyntax, V2alpha3ConnectionEmailEmailSyntax>(s.Syntax),
            };
        }

        public static V2alpha1ConnectionEmailEmail? Revert(V2alpha3ConnectionEmailEmail? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionEmailEmail
            {
                Body = s.Body,
                From = s.From,
                Subject = s.Subject,
                Syntax = MapEnumN<V2alpha3ConnectionEmailEmailSyntax, V2alpha1ConnectionEmailEmailSyntax>(s.Syntax),
            };
        }

        public static V2alpha3ConnectionEmailMessage? Convert(V2alpha1ConnectionEmailMessage? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionEmailMessage
            {
                From = s.From,
                Subject = s.Subject,
                Body = s.Body,
                Syntax = s.Syntax,
            };
        }

        public static V2alpha1ConnectionEmailMessage? Revert(V2alpha3ConnectionEmailMessage? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionEmailMessage
            {
                From = s.From,
                Subject = s.Subject,
                Body = s.Body,
                Syntax = s.Syntax,
            };
        }

        public static V2alpha3ConnectionEmailOtpAuthenticationMethod? Convert(V2alpha1ConnectionEmailOtpAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionEmailOtpAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha1ConnectionEmailOtpAuthenticationMethod? Revert(V2alpha3ConnectionEmailOtpAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionEmailOtpAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha3ConnectionEmailTotp? Convert(V2alpha1ConnectionEmailTotp? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionEmailTotp
            {
                Length = s.Length,
                TimeStep = s.TimeStep,
            };
        }

        public static V2alpha1ConnectionEmailTotp? Revert(V2alpha3ConnectionEmailTotp? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionEmailTotp
            {
                Length = s.Length,
                TimeStep = s.TimeStep,
            };
        }

        public static V2alpha3ConnectionFederatedConnectionsAccessTokens? Convert(V2alpha1ConnectionFederatedConnectionsAccessTokens? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionFederatedConnectionsAccessTokens
            {
                Active = s.Active,
            };
        }

        public static V2alpha1ConnectionFederatedConnectionsAccessTokens? Revert(V2alpha3ConnectionFederatedConnectionsAccessTokens? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionFederatedConnectionsAccessTokens
            {
                Active = s.Active,
            };
        }

        public static V2alpha3ConnectionFind? Convert(V2alpha1ConnectionFind? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionFind
            {
                ConnectionId = s.ConnectionId,
            };
        }

        public static V2alpha1ConnectionFind? Revert(V2alpha3ConnectionFind? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionFind
            {
                ConnectionId = s.ConnectionId,
            };
        }

        public static V2alpha3ConnectionGatewayAuthentication? Convert(V2alpha1ConnectionGatewayAuthentication? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionGatewayAuthentication
            {
                Method = s.Method,
                Subject = s.Subject,
                Audience = s.Audience,
                Secret = s.Secret,
                SecretBase64Encoded = s.SecretBase64Encoded,
            };
        }

        public static V2alpha1ConnectionGatewayAuthentication? Revert(V2alpha3ConnectionGatewayAuthentication? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionGatewayAuthentication
            {
                Method = s.Method,
                Subject = s.Subject,
                Audience = s.Audience,
                Secret = s.Secret,
                SecretBase64Encoded = s.SecretBase64Encoded,
            };
        }

        public static V2alpha3ConnectionGatewayAuthenticationSms? Convert(V2alpha1ConnectionGatewayAuthenticationSms? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionGatewayAuthenticationSms
            {
                Audience = s.Audience,
                Method = s.Method,
                Secret = s.Secret,
                SecretBase64Encoded = s.SecretBase64Encoded,
                Subject = s.Subject,
            };
        }

        public static V2alpha1ConnectionGatewayAuthenticationSms? Revert(V2alpha3ConnectionGatewayAuthenticationSms? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionGatewayAuthenticationSms
            {
                Audience = s.Audience,
                Method = s.Method,
                Secret = s.Secret,
                SecretBase64Encoded = s.SecretBase64Encoded,
                Subject = s.Subject,
            };
        }

        public static V2alpha3ConnectionGoogleAppsFederatedConnectionsAccessTokens? Convert(V2alpha1ConnectionGoogleAppsFederatedConnectionsAccessTokens? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionGoogleAppsFederatedConnectionsAccessTokens
            {
                Active = s.Active,
            };
        }

        public static V2alpha1ConnectionGoogleAppsFederatedConnectionsAccessTokens? Revert(V2alpha3ConnectionGoogleAppsFederatedConnectionsAccessTokens? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionGoogleAppsFederatedConnectionsAccessTokens
            {
                Active = s.Active,
            };
        }

        public static V2alpha3ConnectionMfa? Convert(V2alpha1ConnectionMfa? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionMfa
            {
                Active = s.Active,
                ReturnEnrollSettings = s.ReturnEnrollSettings,
            };
        }

        public static V2alpha1ConnectionMfa? Revert(V2alpha3ConnectionMfa? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionMfa
            {
                Active = s.Active,
                ReturnEnrollSettings = s.ReturnEnrollSettings,
            };
        }

        public static V2alpha3ConnectionOptions? Convert(V2alpha1ConnectionOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptions
            {
                Auth0 = Convert(s.Auth0),
                Ad = Convert(s.Ad),
                Adfs = Convert(s.Adfs),
                Amazon = Convert(s.Amazon),
                Apple = Convert(s.Apple),
                Auth0Oidc = Convert(s.Auth0Oidc),
                AzureAd = Convert(s.AzureAd),
                Baidu = Convert(s.Baidu),
                Bitbucket = Convert(s.Bitbucket),
                Bitly = Convert(s.Bitly),
                Box = Convert(s.Box),
                Daccount = Convert(s.Daccount),
                Dropbox = Convert(s.Dropbox),
                Dwolla = Convert(s.Dwolla),
                Email = Convert(s.Email),
                Evernote = Convert(s.Evernote),
                EvernoteSandbox = Convert(s.EvernoteSandbox),
                Exact = Convert(s.Exact),
                Facebook = Convert(s.Facebook),
                Fitbit = Convert(s.Fitbit),
                GitHub = Convert(s.GitHub),
                GoogleApps = Convert(s.GoogleApps),
                GoogleOAuth2 = Convert(s.GoogleOAuth2),
                Instagram = Convert(s.Instagram),
                Line = Convert(s.Line),
                Linkedin = Convert(s.Linkedin),
                OAuth1 = Convert(s.OAuth1),
                OAuth2 = Convert(s.OAuth2),
                Office365 = Convert(s.Office365),
                Oidc = Convert(s.Oidc),
                Okta = Convert(s.Okta),
                Paypal = Convert(s.Paypal),
                PaypalSandbox = Convert(s.PaypalSandbox),
                PingFederate = Convert(s.PingFederate),
                PlanningCenter = Convert(s.PlanningCenter),
                Salesforce = Convert(s.Salesforce),
                SalesforceCommunity = Convert(s.SalesforceCommunity),
                SalesforceSandbox = Convert(s.SalesforceSandbox),
                Saml = Convert(s.Saml),
                Sharepoint = Convert(s.Sharepoint),
                Shop = Convert(s.Shop),
                Shopify = Convert(s.Shopify),
                Sms = Convert(s.Sms),
                Soundcloud = Convert(s.Soundcloud),
                ThirtySevenSignals = Convert(s.ThirtySevenSignals),
                Twitter = Convert(s.Twitter),
                Untappd = Convert(s.Untappd),
                Vkontakte = Convert(s.Vkontakte),
                Weibo = Convert(s.Weibo),
                WindowsLive = Convert(s.WindowsLive),
                Wordpress = Convert(s.Wordpress),
                Yahoo = Convert(s.Yahoo),
                Yandex = Convert(s.Yandex),
            };
        }

        public static V2alpha1ConnectionOptions? Revert(V2alpha3ConnectionOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptions
            {
                Auth0 = Revert(s.Auth0),
                Ad = Revert(s.Ad),
                Adfs = Revert(s.Adfs),
                Amazon = Revert(s.Amazon),
                Apple = Revert(s.Apple),
                Auth0Oidc = Revert(s.Auth0Oidc),
                AzureAd = Revert(s.AzureAd),
                Baidu = Revert(s.Baidu),
                Bitbucket = Revert(s.Bitbucket),
                Bitly = Revert(s.Bitly),
                Box = Revert(s.Box),
                Daccount = Revert(s.Daccount),
                Dropbox = Revert(s.Dropbox),
                Dwolla = Revert(s.Dwolla),
                Email = Revert(s.Email),
                Evernote = Revert(s.Evernote),
                EvernoteSandbox = Revert(s.EvernoteSandbox),
                Exact = Revert(s.Exact),
                Facebook = Revert(s.Facebook),
                Fitbit = Revert(s.Fitbit),
                GitHub = Revert(s.GitHub),
                GoogleApps = Revert(s.GoogleApps),
                GoogleOAuth2 = Revert(s.GoogleOAuth2),
                Instagram = Revert(s.Instagram),
                Line = Revert(s.Line),
                Linkedin = Revert(s.Linkedin),
                OAuth1 = Revert(s.OAuth1),
                OAuth2 = Revert(s.OAuth2),
                Office365 = Revert(s.Office365),
                Oidc = Revert(s.Oidc),
                Okta = Revert(s.Okta),
                Paypal = Revert(s.Paypal),
                PaypalSandbox = Revert(s.PaypalSandbox),
                PingFederate = Revert(s.PingFederate),
                PlanningCenter = Revert(s.PlanningCenter),
                Salesforce = Revert(s.Salesforce),
                SalesforceCommunity = Revert(s.SalesforceCommunity),
                SalesforceSandbox = Revert(s.SalesforceSandbox),
                Saml = Revert(s.Saml),
                Sharepoint = Revert(s.Sharepoint),
                Shop = Revert(s.Shop),
                Shopify = Revert(s.Shopify),
                Sms = Revert(s.Sms),
                Soundcloud = Revert(s.Soundcloud),
                ThirtySevenSignals = Revert(s.ThirtySevenSignals),
                Twitter = Revert(s.Twitter),
                Untappd = Revert(s.Untappd),
                Vkontakte = Revert(s.Vkontakte),
                Weibo = Revert(s.Weibo),
                WindowsLive = Revert(s.WindowsLive),
                Wordpress = Revert(s.Wordpress),
                Yahoo = Revert(s.Yahoo),
                Yandex = Revert(s.Yandex),
            };
        }

        public static V2alpha3ConnectionOptionsAd? Convert(V2alpha1ConnectionOptionsAd? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAd
            {
                AgentIp = s.AgentIp,
                AgentMode = s.AgentMode,
                AgentVersion = s.AgentVersion,
                BruteForceProtection = s.BruteForceProtection,
                CertAuth = s.CertAuth,
                Certs = s.Certs,
                DisableCache = s.DisableCache,
                DisableSelfServiceChangePassword = s.DisableSelfServiceChangePassword,
                DomainAliases = s.DomainAliases,
                IconUrl = s.IconUrl,
                Ips = s.Ips,
                Kerberos = s.Kerberos,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsAd? Revert(V2alpha3ConnectionOptionsAd? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAd
            {
                AgentIp = s.AgentIp,
                AgentMode = s.AgentMode,
                AgentVersion = s.AgentVersion,
                BruteForceProtection = s.BruteForceProtection,
                CertAuth = s.CertAuth,
                Certs = s.Certs,
                DisableCache = s.DisableCache,
                DisableSelfServiceChangePassword = s.DisableSelfServiceChangePassword,
                DomainAliases = s.DomainAliases,
                IconUrl = s.IconUrl,
                Ips = s.Ips,
                Kerberos = s.Kerberos,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsAdfs? Convert(V2alpha1ConnectionOptionsAdfs? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAdfs
            {
                AdfsServer = s.AdfsServer,
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                FedMetadataXml = s.FedMetadataXml,
                IconUrl = s.IconUrl,
                PrevThumbprints = s.PrevThumbprints,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                ShouldTrustEmailVerifiedConnection = MapEnumN<V2alpha1ConnectionShouldTrustEmailVerifiedConnectionEnum, V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum>(s.ShouldTrustEmailVerifiedConnection),
                SignInEndpoint = s.SignInEndpoint,
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UserIdAttribute = s.UserIdAttribute,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsAdfs? Revert(V2alpha3ConnectionOptionsAdfs? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAdfs
            {
                AdfsServer = s.AdfsServer,
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                FedMetadataXml = s.FedMetadataXml,
                IconUrl = s.IconUrl,
                PrevThumbprints = s.PrevThumbprints,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                ShouldTrustEmailVerifiedConnection = MapEnumN<V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum, V2alpha1ConnectionShouldTrustEmailVerifiedConnectionEnum>(s.ShouldTrustEmailVerifiedConnection),
                SignInEndpoint = s.SignInEndpoint,
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UserIdAttribute = s.UserIdAttribute,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsAmazon? Convert(V2alpha1ConnectionOptionsAmazon? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAmazon
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                PostalCode = s.PostalCode,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsAmazon? Revert(V2alpha3ConnectionOptionsAmazon? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAmazon
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                PostalCode = s.PostalCode,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsApple? Convert(V2alpha1ConnectionOptionsApple? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsApple
            {
                AppSecret = s.AppSecret,
                ClientId = s.ClientId,
                Email = s.Email,
                FreeformScopes = s.FreeformScopes,
                Kid = s.Kid,
                Name = s.Name,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TeamId = s.TeamId,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsApple? Revert(V2alpha3ConnectionOptionsApple? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsApple
            {
                AppSecret = s.AppSecret,
                ClientId = s.ClientId,
                Email = s.Email,
                FreeformScopes = s.FreeformScopes,
                Kid = s.Kid,
                Name = s.Name,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TeamId = s.TeamId,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsAssertionDecryptionSettings? Convert(V2alpha1ConnectionOptionsAssertionDecryptionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAssertionDecryptionSettings
            {
                DecryptAssertion = s.DecryptAssertion,
                DecryptionAlgorithm = MapEnumN<V2alpha1ConnectionAssertionDecryptionAlgorithmProfile, V2alpha3ConnectionAssertionDecryptionAlgorithmProfile>(s.DecryptionAlgorithm),
                KeyEncryptionAlgorithm = s.KeyEncryptionAlgorithm,
            };
        }

        public static V2alpha1ConnectionOptionsAssertionDecryptionSettings? Revert(V2alpha3ConnectionOptionsAssertionDecryptionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAssertionDecryptionSettings
            {
                DecryptAssertion = s.DecryptAssertion,
                DecryptionAlgorithm = MapEnumN<V2alpha3ConnectionAssertionDecryptionAlgorithmProfile, V2alpha1ConnectionAssertionDecryptionAlgorithmProfile>(s.DecryptionAlgorithm),
                KeyEncryptionAlgorithm = s.KeyEncryptionAlgorithm,
            };
        }

        public static V2alpha3ConnectionOptionsAttributeAllowedTypes? Convert(V2alpha1ConnectionOptionsAttributeAllowedTypes? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAttributeAllowedTypes
            {
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
            };
        }

        public static V2alpha1ConnectionOptionsAttributeAllowedTypes? Revert(V2alpha3ConnectionOptionsAttributeAllowedTypes? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAttributeAllowedTypes
            {
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
            };
        }

        public static V2alpha3ConnectionOptionsAttributeIdentifier? Convert(V2alpha1ConnectionOptionsAttributeIdentifier? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAttributeIdentifier
            {
                Active = s.Active,
            };
        }

        public static V2alpha1ConnectionOptionsAttributeIdentifier? Revert(V2alpha3ConnectionOptionsAttributeIdentifier? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAttributeIdentifier
            {
                Active = s.Active,
            };
        }

        public static V2alpha3ConnectionOptionsAttributeMap? Convert(V2alpha1ConnectionOptionsAttributeMap? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAttributeMap
            {
                MappingMode = s.MappingMode,
                UserinfoScope = s.UserinfoScope,
                Attributes = s.Attributes,
            };
        }

        public static V2alpha1ConnectionOptionsAttributeMap? Revert(V2alpha3ConnectionOptionsAttributeMap? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAttributeMap
            {
                MappingMode = s.MappingMode,
                UserinfoScope = s.UserinfoScope,
                Attributes = s.Attributes,
            };
        }

        public static V2alpha3ConnectionOptionsAttributeValidation? Convert(V2alpha1ConnectionOptionsAttributeValidation? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAttributeValidation
            {
                MinLength = s.MinLength,
                MaxLength = s.MaxLength,
                AllowedTypes = Convert(s.AllowedTypes),
            };
        }

        public static V2alpha1ConnectionOptionsAttributeValidation? Revert(V2alpha3ConnectionOptionsAttributeValidation? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAttributeValidation
            {
                MinLength = s.MinLength,
                MaxLength = s.MaxLength,
                AllowedTypes = Revert(s.AllowedTypes),
            };
        }

        public static V2alpha3ConnectionOptionsAuth0? Convert(V2alpha1ConnectionOptionsAuth0? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAuth0
            {
                Attributes = Convert(s.Attributes),
                AuthenticationMethods = Convert(s.AuthenticationMethods),
                BruteForceProtection = s.BruteForceProtection,
                Configuration = s.Configuration,
                CustomScripts = Convert(s.CustomScripts),
                DisableSelfServiceChangePassword = s.DisableSelfServiceChangePassword,
                DisableSignup = s.DisableSignup,
                EnableScriptContext = s.EnableScriptContext,
                EnabledDatabaseCustomization = s.EnabledDatabaseCustomization,
                ImportMode = s.ImportMode,
                Mfa = Convert(s.Mfa),
                PasskeyOptions = Convert(s.PasskeyOptions),
                PasswordPolicy = MapEnumN<V2alpha1ConnectionPasswordPolicyEnum, V2alpha3ConnectionPasswordPolicyEnum>(s.PasswordPolicy),
                PasswordComplexityOptions = Convert(s.PasswordComplexityOptions),
                PasswordDictionary = Convert(s.PasswordDictionary),
                PasswordHistory = Convert(s.PasswordHistory),
                PasswordNoPersonalInfo = Convert(s.PasswordNoPersonalInfo),
                PasswordOptions = Convert(s.PasswordOptions),
                Precedence = s.Precedence?.Select(__x => MapEnum<V2alpha1ConnectionIdentifierPrecedenceEnum, V2alpha3ConnectionIdentifierPrecedenceEnum>(__x)).ToArray(),
                RealmFallback = s.RealmFallback,
                RequiresUsername = s.RequiresUsername,
                Validation = Convert(s.Validation),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsAuth0? Revert(V2alpha3ConnectionOptionsAuth0? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAuth0
            {
                Attributes = Revert(s.Attributes),
                AuthenticationMethods = Revert(s.AuthenticationMethods),
                BruteForceProtection = s.BruteForceProtection,
                Configuration = s.Configuration,
                CustomScripts = Revert(s.CustomScripts),
                DisableSelfServiceChangePassword = s.DisableSelfServiceChangePassword,
                DisableSignup = s.DisableSignup,
                EnableScriptContext = s.EnableScriptContext,
                EnabledDatabaseCustomization = s.EnabledDatabaseCustomization,
                ImportMode = s.ImportMode,
                Mfa = Revert(s.Mfa),
                PasskeyOptions = Revert(s.PasskeyOptions),
                PasswordPolicy = MapEnumN<V2alpha3ConnectionPasswordPolicyEnum, V2alpha1ConnectionPasswordPolicyEnum>(s.PasswordPolicy),
                PasswordComplexityOptions = Revert(s.PasswordComplexityOptions),
                PasswordDictionary = Revert(s.PasswordDictionary),
                PasswordHistory = Revert(s.PasswordHistory),
                PasswordNoPersonalInfo = Revert(s.PasswordNoPersonalInfo),
                PasswordOptions = Revert(s.PasswordOptions),
                Precedence = s.Precedence?.Select(__x => MapEnum<V2alpha3ConnectionIdentifierPrecedenceEnum, V2alpha1ConnectionIdentifierPrecedenceEnum>(__x)).ToArray(),
                RealmFallback = s.RealmFallback,
                RequiresUsername = s.RequiresUsername,
                Validation = Revert(s.Validation),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsAuth0Oidc? Convert(V2alpha1ConnectionOptionsAuth0Oidc? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAuth0Oidc
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
            };
        }

        public static V2alpha1ConnectionOptionsAuth0Oidc? Revert(V2alpha3ConnectionOptionsAuth0Oidc? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAuth0Oidc
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
            };
        }

        public static V2alpha3ConnectionOptionsAuthenticationMethods? Convert(V2alpha1ConnectionOptionsAuthenticationMethods? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAuthenticationMethods
            {
                Password = Convert(s.Password),
                Passkey = Convert(s.Passkey),
            };
        }

        public static V2alpha1ConnectionOptionsAuthenticationMethods? Revert(V2alpha3ConnectionOptionsAuthenticationMethods? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAuthenticationMethods
            {
                Password = Revert(s.Password),
                Passkey = Revert(s.Passkey),
            };
        }

        public static V2alpha3ConnectionOptionsAzureAd? Convert(V2alpha1ConnectionOptionsAzureAd? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsAzureAd
            {
                ApiEnableUsers = s.ApiEnableUsers,
                AppDomain = s.AppDomain,
                AppId = s.AppId,
                BasicProfile = s.BasicProfile,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                DomainAliases = s.DomainAliases,
                ExtAccessToken = s.ExtAccessToken,
                ExtAccountEnabled = s.ExtAccountEnabled,
                ExtAdmin = s.ExtAdmin,
                ExtAgreedTerms = s.ExtAgreedTerms,
                ExtAssignedLicenses = s.ExtAssignedLicenses,
                ExtAssignedPlans = s.ExtAssignedPlans,
                ExtAzureId = s.ExtAzureId,
                ExtCity = s.ExtCity,
                ExtCountry = s.ExtCountry,
                ExtDepartment = s.ExtDepartment,
                ExtDirSyncEnabled = s.ExtDirSyncEnabled,
                ExtEmail = s.ExtEmail,
                ExtExpiresIn = s.ExtExpiresIn,
                ExtFamilyName = s.ExtFamilyName,
                ExtFax = s.ExtFax,
                ExtGivenName = s.ExtGivenName,
                ExtGroupIds = s.ExtGroupIds,
                ExtGroups = s.ExtGroups,
                ExtIsSuspended = s.ExtIsSuspended,
                ExtJobTitle = s.ExtJobTitle,
                ExtLastSync = s.ExtLastSync,
                ExtMobile = s.ExtMobile,
                ExtName = s.ExtName,
                ExtNestedGroups = s.ExtNestedGroups,
                ExtNickname = s.ExtNickname,
                ExtOid = s.ExtOid,
                ExtPhone = s.ExtPhone,
                ExtPhysicalDeliveryOfficeName = s.ExtPhysicalDeliveryOfficeName,
                ExtPostalCode = s.ExtPostalCode,
                ExtPreferredLanguage = s.ExtPreferredLanguage,
                ExtProfile = s.ExtProfile,
                ExtProvisionedPlans = s.ExtProvisionedPlans,
                ExtProvisioningErrors = s.ExtProvisioningErrors,
                ExtProxyAddresses = s.ExtProxyAddresses,
                ExtPuid = s.ExtPuid,
                ExtRefreshToken = s.ExtRefreshToken,
                ExtRoles = s.ExtRoles,
                ExtState = s.ExtState,
                ExtStreet = s.ExtStreet,
                ExtTelephoneNumber = s.ExtTelephoneNumber,
                ExtTenantid = s.ExtTenantid,
                ExtUpn = s.ExtUpn,
                ExtUsageLocation = s.ExtUsageLocation,
                ExtUserId = s.ExtUserId,
                FederatedConnectionsAccessTokens = Convert(s.FederatedConnectionsAccessTokens),
                Granted = s.Granted,
                IconUrl = s.IconUrl,
                IdentityApi = MapEnumN<V2alpha1ConnectionIdentityApiEnumAzureAd, V2alpha3ConnectionIdentityApiEnumAzureAd>(s.IdentityApi),
                MaxGroupsToRetrieve = s.MaxGroupsToRetrieve,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                ShouldTrustEmailVerifiedConnection = MapEnumN<V2alpha1ConnectionShouldTrustEmailVerifiedConnectionEnum, V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum>(s.ShouldTrustEmailVerifiedConnection),
                TenantDomain = s.TenantDomain,
                TenantId = s.TenantId,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UseWsfed = s.UseWsfed,
                UseCommonEndpoint = s.UseCommonEndpoint,
                UseridAttribute = MapEnumN<V2alpha1ConnectionUseridAttributeEnumAzureAd, V2alpha3ConnectionUseridAttributeEnumAzureAd>(s.UseridAttribute),
                WaadProtocol = MapEnumN<V2alpha1ConnectionWaadProtocolEnumAzureAd, V2alpha3ConnectionWaadProtocolEnumAzureAd>(s.WaadProtocol),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsAzureAd? Revert(V2alpha3ConnectionOptionsAzureAd? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsAzureAd
            {
                ApiEnableUsers = s.ApiEnableUsers,
                AppDomain = s.AppDomain,
                AppId = s.AppId,
                BasicProfile = s.BasicProfile,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                DomainAliases = s.DomainAliases,
                ExtAccessToken = s.ExtAccessToken,
                ExtAccountEnabled = s.ExtAccountEnabled,
                ExtAdmin = s.ExtAdmin,
                ExtAgreedTerms = s.ExtAgreedTerms,
                ExtAssignedLicenses = s.ExtAssignedLicenses,
                ExtAssignedPlans = s.ExtAssignedPlans,
                ExtAzureId = s.ExtAzureId,
                ExtCity = s.ExtCity,
                ExtCountry = s.ExtCountry,
                ExtDepartment = s.ExtDepartment,
                ExtDirSyncEnabled = s.ExtDirSyncEnabled,
                ExtEmail = s.ExtEmail,
                ExtExpiresIn = s.ExtExpiresIn,
                ExtFamilyName = s.ExtFamilyName,
                ExtFax = s.ExtFax,
                ExtGivenName = s.ExtGivenName,
                ExtGroupIds = s.ExtGroupIds,
                ExtGroups = s.ExtGroups,
                ExtIsSuspended = s.ExtIsSuspended,
                ExtJobTitle = s.ExtJobTitle,
                ExtLastSync = s.ExtLastSync,
                ExtMobile = s.ExtMobile,
                ExtName = s.ExtName,
                ExtNestedGroups = s.ExtNestedGroups,
                ExtNickname = s.ExtNickname,
                ExtOid = s.ExtOid,
                ExtPhone = s.ExtPhone,
                ExtPhysicalDeliveryOfficeName = s.ExtPhysicalDeliveryOfficeName,
                ExtPostalCode = s.ExtPostalCode,
                ExtPreferredLanguage = s.ExtPreferredLanguage,
                ExtProfile = s.ExtProfile,
                ExtProvisionedPlans = s.ExtProvisionedPlans,
                ExtProvisioningErrors = s.ExtProvisioningErrors,
                ExtProxyAddresses = s.ExtProxyAddresses,
                ExtPuid = s.ExtPuid,
                ExtRefreshToken = s.ExtRefreshToken,
                ExtRoles = s.ExtRoles,
                ExtState = s.ExtState,
                ExtStreet = s.ExtStreet,
                ExtTelephoneNumber = s.ExtTelephoneNumber,
                ExtTenantid = s.ExtTenantid,
                ExtUpn = s.ExtUpn,
                ExtUsageLocation = s.ExtUsageLocation,
                ExtUserId = s.ExtUserId,
                FederatedConnectionsAccessTokens = Revert(s.FederatedConnectionsAccessTokens),
                Granted = s.Granted,
                IconUrl = s.IconUrl,
                IdentityApi = MapEnumN<V2alpha3ConnectionIdentityApiEnumAzureAd, V2alpha1ConnectionIdentityApiEnumAzureAd>(s.IdentityApi),
                MaxGroupsToRetrieve = s.MaxGroupsToRetrieve,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                ShouldTrustEmailVerifiedConnection = MapEnumN<V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum, V2alpha1ConnectionShouldTrustEmailVerifiedConnectionEnum>(s.ShouldTrustEmailVerifiedConnection),
                TenantDomain = s.TenantDomain,
                TenantId = s.TenantId,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UseWsfed = s.UseWsfed,
                UseCommonEndpoint = s.UseCommonEndpoint,
                UseridAttribute = MapEnumN<V2alpha3ConnectionUseridAttributeEnumAzureAd, V2alpha1ConnectionUseridAttributeEnumAzureAd>(s.UseridAttribute),
                WaadProtocol = MapEnumN<V2alpha3ConnectionWaadProtocolEnumAzureAd, V2alpha1ConnectionWaadProtocolEnumAzureAd>(s.WaadProtocol),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsBaidu? Convert(V2alpha1ConnectionOptionsBaidu? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsBaidu
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsBaidu? Revert(V2alpha3ConnectionOptionsBaidu? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsBaidu
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsBitbucket? Convert(V2alpha1ConnectionOptionsBitbucket? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsBitbucket
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsBitbucket? Revert(V2alpha3ConnectionOptionsBitbucket? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsBitbucket
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsBitly? Convert(V2alpha1ConnectionOptionsBitly? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsBitly
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsBitly? Revert(V2alpha3ConnectionOptionsBitly? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsBitly
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsBox? Convert(V2alpha1ConnectionOptionsBox? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsBox
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsBox? Revert(V2alpha3ConnectionOptionsBox? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsBox
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsClientCredentials? Convert(V2alpha1ConnectionOptionsClientCredentials? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsClientCredentials
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
            };
        }

        public static V2alpha1ConnectionOptionsClientCredentials? Revert(V2alpha3ConnectionOptionsClientCredentials? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsClientCredentials
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
            };
        }

        public static V2alpha3ConnectionOptionsCommon? Convert(V2alpha1ConnectionOptionsCommon? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsCommon
            {
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsCommon? Revert(V2alpha3ConnectionOptionsCommon? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsCommon
            {
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsCommonOidc? Convert(V2alpha1ConnectionOptionsCommonOidc? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsCommonOidc
            {
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                ConnectionSettings = Convert(s.ConnectionSettings),
                DomainAliases = s.DomainAliases,
                DpopSigningAlg = MapEnumN<V2alpha1ConnectionDpopSigningAlgEnum, V2alpha3ConnectionDpopSigningAlgEnum>(s.DpopSigningAlg),
                FederatedConnectionsAccessTokens = Convert(s.FederatedConnectionsAccessTokens),
                IconUrl = s.IconUrl,
                IdTokenSignedResponseAlgs = s.IdTokenSignedResponseAlgs?.Select(__x => MapEnum<V2alpha1ConnectionIdTokenSignedResponseAlgEnum, V2alpha3ConnectionIdTokenSignedResponseAlgEnum>(__x)).ToArray(),
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OidcMetadata = Convert(s.OidcMetadata),
                Scope = s.Scope,
                SendBackChannelNonce = s.SendBackChannelNonce,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethod = MapEnumN<V2alpha1ConnectionTokenEndpointAuthMethodEnum, V2alpha3ConnectionTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                TokenEndpointAuthSigningAlg = MapEnumN<V2alpha1ConnectionTokenEndpointAuthSigningAlgEnum, V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum>(s.TokenEndpointAuthSigningAlg),
                TokenEndpointJwtcaAudFormat = MapEnumN<V2alpha1ConnectionTokenEndpointJwtcaAudFormatEnumOidc, V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc>(s.TokenEndpointJwtcaAudFormat),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UserinfoEndpoint = s.UserinfoEndpoint,
            };
        }

        public static V2alpha1ConnectionOptionsCommonOidc? Revert(V2alpha3ConnectionOptionsCommonOidc? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsCommonOidc
            {
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                ConnectionSettings = Revert(s.ConnectionSettings),
                DomainAliases = s.DomainAliases,
                DpopSigningAlg = MapEnumN<V2alpha3ConnectionDpopSigningAlgEnum, V2alpha1ConnectionDpopSigningAlgEnum>(s.DpopSigningAlg),
                FederatedConnectionsAccessTokens = Revert(s.FederatedConnectionsAccessTokens),
                IconUrl = s.IconUrl,
                IdTokenSignedResponseAlgs = s.IdTokenSignedResponseAlgs?.Select(__x => MapEnum<V2alpha3ConnectionIdTokenSignedResponseAlgEnum, V2alpha1ConnectionIdTokenSignedResponseAlgEnum>(__x)).ToArray(),
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OidcMetadata = Revert(s.OidcMetadata),
                Scope = s.Scope,
                SendBackChannelNonce = s.SendBackChannelNonce,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethod = MapEnumN<V2alpha3ConnectionTokenEndpointAuthMethodEnum, V2alpha1ConnectionTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                TokenEndpointAuthSigningAlg = MapEnumN<V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum, V2alpha1ConnectionTokenEndpointAuthSigningAlgEnum>(s.TokenEndpointAuthSigningAlg),
                TokenEndpointJwtcaAudFormat = MapEnumN<V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc, V2alpha1ConnectionTokenEndpointJwtcaAudFormatEnumOidc>(s.TokenEndpointJwtcaAudFormat),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UserinfoEndpoint = s.UserinfoEndpoint,
            };
        }

        public static V2alpha3ConnectionOptionsCommonSaml? Convert(V2alpha1ConnectionOptionsCommonSaml? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsCommonSaml
            {
                AssertionDecryptionSettings = Convert(s.AssertionDecryptionSettings),
                Cert = s.Cert,
                DecryptionKey = Convert(s.DecryptionKey),
                DigestAlgorithm = MapEnumN<V2alpha1ConnectionDigestAlgorithmEnumSaml, V2alpha3ConnectionDigestAlgorithmEnumSaml>(s.DigestAlgorithm),
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                IconUrl = s.IconUrl,
                Idpinitiated = Convert(s.Idpinitiated),
                ProtocolBinding = MapEnumN<V2alpha1ConnectionProtocolBindingEnumSaml, V2alpha3ConnectionProtocolBindingEnumSaml>(s.ProtocolBinding),
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                SignSamlRequest = s.SignSamlRequest,
                SignatureAlgorithm = MapEnumN<V2alpha1ConnectionSignatureAlgorithmEnumSaml, V2alpha3ConnectionSignatureAlgorithmEnumSaml>(s.SignatureAlgorithm),
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
            };
        }

        public static V2alpha1ConnectionOptionsCommonSaml? Revert(V2alpha3ConnectionOptionsCommonSaml? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsCommonSaml
            {
                AssertionDecryptionSettings = Revert(s.AssertionDecryptionSettings),
                Cert = s.Cert,
                DecryptionKey = Revert(s.DecryptionKey),
                DigestAlgorithm = MapEnumN<V2alpha3ConnectionDigestAlgorithmEnumSaml, V2alpha1ConnectionDigestAlgorithmEnumSaml>(s.DigestAlgorithm),
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                IconUrl = s.IconUrl,
                Idpinitiated = Revert(s.Idpinitiated),
                ProtocolBinding = MapEnumN<V2alpha3ConnectionProtocolBindingEnumSaml, V2alpha1ConnectionProtocolBindingEnumSaml>(s.ProtocolBinding),
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                SignSamlRequest = s.SignSamlRequest,
                SignatureAlgorithm = MapEnumN<V2alpha3ConnectionSignatureAlgorithmEnumSaml, V2alpha1ConnectionSignatureAlgorithmEnumSaml>(s.SignatureAlgorithm),
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
            };
        }

        public static V2alpha3ConnectionOptionsConnectionSettings? Convert(V2alpha1ConnectionOptionsConnectionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsConnectionSettings
            {
                Pkce = s.Pkce,
            };
        }

        public static V2alpha1ConnectionOptionsConnectionSettings? Revert(V2alpha3ConnectionOptionsConnectionSettings? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsConnectionSettings
            {
                Pkce = s.Pkce,
            };
        }

        public static V2alpha3ConnectionOptionsCustomScripts? Convert(V2alpha1ConnectionOptionsCustomScripts? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsCustomScripts
            {
                Login = s.Login,
                GetUser = s.GetUser,
                Delete = s.Delete,
                ChangePassword = s.ChangePassword,
                Verify = s.Verify,
                Create = s.Create,
                ChangeUsername = s.ChangeUsername,
                ChangeEmail = s.ChangeEmail,
                ChangePhoneNumber = s.ChangePhoneNumber,
            };
        }

        public static V2alpha1ConnectionOptionsCustomScripts? Revert(V2alpha3ConnectionOptionsCustomScripts? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsCustomScripts
            {
                Login = s.Login,
                GetUser = s.GetUser,
                Delete = s.Delete,
                ChangePassword = s.ChangePassword,
                Verify = s.Verify,
                Create = s.Create,
                ChangeUsername = s.ChangeUsername,
                ChangeEmail = s.ChangeEmail,
                ChangePhoneNumber = s.ChangePhoneNumber,
            };
        }

        public static V2alpha3ConnectionOptionsDaccount? Convert(V2alpha1ConnectionOptionsDaccount? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsDaccount
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsDaccount? Revert(V2alpha3ConnectionOptionsDaccount? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsDaccount
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsDropbox? Convert(V2alpha1ConnectionOptionsDropbox? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsDropbox
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsDropbox? Revert(V2alpha3ConnectionOptionsDropbox? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsDropbox
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsDwolla? Convert(V2alpha1ConnectionOptionsDwolla? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsDwolla
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsDwolla? Revert(V2alpha3ConnectionOptionsDwolla? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsDwolla
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsEmail? Convert(V2alpha1ConnectionOptionsEmail? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsEmail
            {
                AuthParams = s.AuthParams,
                BruteForceProtection = s.BruteForceProtection,
                DisableSignup = s.DisableSignup,
                Email = Convert(s.Email),
                Name = s.Name,
                Totp = Convert(s.Totp),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsEmail? Revert(V2alpha3ConnectionOptionsEmail? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsEmail
            {
                AuthParams = s.AuthParams,
                BruteForceProtection = s.BruteForceProtection,
                DisableSignup = s.DisableSignup,
                Email = Revert(s.Email),
                Name = s.Name,
                Totp = Revert(s.Totp),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsEmailAttribute? Convert(V2alpha1ConnectionOptionsEmailAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsEmailAttribute
            {
                Identifier = Convert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Convert(s.Signup),
            };
        }

        public static V2alpha1ConnectionOptionsEmailAttribute? Revert(V2alpha3ConnectionOptionsEmailAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsEmailAttribute
            {
                Identifier = Revert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Revert(s.Signup),
            };
        }

        public static V2alpha3ConnectionOptionsEmailSignup? Convert(V2alpha1ConnectionOptionsEmailSignup? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsEmailSignup
            {
                Status = MapEnumN<V2alpha1ConnectionOptionsAttributeStatus, V2alpha3ConnectionOptionsAttributeStatus>(s.Status),
                Verification = Convert(s.Verification),
            };
        }

        public static V2alpha1ConnectionOptionsEmailSignup? Revert(V2alpha3ConnectionOptionsEmailSignup? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsEmailSignup
            {
                Status = MapEnumN<V2alpha3ConnectionOptionsAttributeStatus, V2alpha1ConnectionOptionsAttributeStatus>(s.Status),
                Verification = Revert(s.Verification),
            };
        }

        public static V2alpha3ConnectionOptionsEvernote? Convert(V2alpha1ConnectionOptionsEvernote? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsEvernote
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsEvernote? Revert(V2alpha3ConnectionOptionsEvernote? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsEvernote
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsExact? Convert(V2alpha1ConnectionOptionsExact? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsExact
            {
                BaseUrl = s.BaseUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Profile = s.Profile,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsExact? Revert(V2alpha3ConnectionOptionsExact? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsExact
            {
                BaseUrl = s.BaseUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Profile = s.Profile,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsFacebook? Convert(V2alpha1ConnectionOptionsFacebook? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsFacebook
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                AdsManagement = s.AdsManagement,
                AdsRead = s.AdsRead,
                AllowContextProfileField = s.AllowContextProfileField,
                BusinessManagement = s.BusinessManagement,
                Email = s.Email,
                GroupsAccessMemberInfo = s.GroupsAccessMemberInfo,
                LeadsRetrieval = s.LeadsRetrieval,
                ManageNotifications = s.ManageNotifications,
                ManagePages = s.ManagePages,
                PagesManageCta = s.PagesManageCta,
                PagesManageInstantArticles = s.PagesManageInstantArticles,
                PagesMessaging = s.PagesMessaging,
                PagesMessagingPhoneNumber = s.PagesMessagingPhoneNumber,
                PagesMessagingSubscriptions = s.PagesMessagingSubscriptions,
                PagesShowList = s.PagesShowList,
                PublicProfile = s.PublicProfile,
                PublishActions = s.PublishActions,
                PublishPages = s.PublishPages,
                PublishToGroups = s.PublishToGroups,
                PublishVideo = s.PublishVideo,
                ReadAudienceNetworkInsights = s.ReadAudienceNetworkInsights,
                ReadInsights = s.ReadInsights,
                ReadMailbox = s.ReadMailbox,
                ReadPageMailboxes = s.ReadPageMailboxes,
                ReadStream = s.ReadStream,
                UserAgeRange = s.UserAgeRange,
                UserBirthday = s.UserBirthday,
                UserEvents = s.UserEvents,
                UserFriends = s.UserFriends,
                UserGender = s.UserGender,
                UserGroups = s.UserGroups,
                UserHometown = s.UserHometown,
                UserLikes = s.UserLikes,
                UserLink = s.UserLink,
                UserLocation = s.UserLocation,
                UserManagedGroups = s.UserManagedGroups,
                UserPhotos = s.UserPhotos,
                UserPosts = s.UserPosts,
                UserStatus = s.UserStatus,
                UserTaggedPlaces = s.UserTaggedPlaces,
                UserVideos = s.UserVideos,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsFacebook? Revert(V2alpha3ConnectionOptionsFacebook? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsFacebook
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                AdsManagement = s.AdsManagement,
                AdsRead = s.AdsRead,
                AllowContextProfileField = s.AllowContextProfileField,
                BusinessManagement = s.BusinessManagement,
                Email = s.Email,
                GroupsAccessMemberInfo = s.GroupsAccessMemberInfo,
                LeadsRetrieval = s.LeadsRetrieval,
                ManageNotifications = s.ManageNotifications,
                ManagePages = s.ManagePages,
                PagesManageCta = s.PagesManageCta,
                PagesManageInstantArticles = s.PagesManageInstantArticles,
                PagesMessaging = s.PagesMessaging,
                PagesMessagingPhoneNumber = s.PagesMessagingPhoneNumber,
                PagesMessagingSubscriptions = s.PagesMessagingSubscriptions,
                PagesShowList = s.PagesShowList,
                PublicProfile = s.PublicProfile,
                PublishActions = s.PublishActions,
                PublishPages = s.PublishPages,
                PublishToGroups = s.PublishToGroups,
                PublishVideo = s.PublishVideo,
                ReadAudienceNetworkInsights = s.ReadAudienceNetworkInsights,
                ReadInsights = s.ReadInsights,
                ReadMailbox = s.ReadMailbox,
                ReadPageMailboxes = s.ReadPageMailboxes,
                ReadStream = s.ReadStream,
                UserAgeRange = s.UserAgeRange,
                UserBirthday = s.UserBirthday,
                UserEvents = s.UserEvents,
                UserFriends = s.UserFriends,
                UserGender = s.UserGender,
                UserGroups = s.UserGroups,
                UserHometown = s.UserHometown,
                UserLikes = s.UserLikes,
                UserLink = s.UserLink,
                UserLocation = s.UserLocation,
                UserManagedGroups = s.UserManagedGroups,
                UserPhotos = s.UserPhotos,
                UserPosts = s.UserPosts,
                UserStatus = s.UserStatus,
                UserTaggedPlaces = s.UserTaggedPlaces,
                UserVideos = s.UserVideos,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsFederatedConnectionsAccessTokens? Convert(V2alpha1ConnectionOptionsFederatedConnectionsAccessTokens? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsFederatedConnectionsAccessTokens
            {
                Active = s.Active,
            };
        }

        public static V2alpha1ConnectionOptionsFederatedConnectionsAccessTokens? Revert(V2alpha3ConnectionOptionsFederatedConnectionsAccessTokens? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsFederatedConnectionsAccessTokens
            {
                Active = s.Active,
            };
        }

        public static V2alpha3ConnectionOptionsFitbit? Convert(V2alpha1ConnectionOptionsFitbit? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsFitbit
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsFitbit? Revert(V2alpha3ConnectionOptionsFitbit? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsFitbit
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsGitHub? Convert(V2alpha1ConnectionOptionsGitHub? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsGitHub
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                AdminOrg = s.AdminOrg,
                AdminPublicKey = s.AdminPublicKey,
                AdminRepoHook = s.AdminRepoHook,
                DeleteRepo = s.DeleteRepo,
                Email = s.Email,
                Follow = s.Follow,
                Gist = s.Gist,
                Notifications = s.Notifications,
                Profile = s.Profile,
                PublicRepo = s.PublicRepo,
                ReadOrg = s.ReadOrg,
                ReadPublicKey = s.ReadPublicKey,
                ReadRepoHook = s.ReadRepoHook,
                ReadUser = s.ReadUser,
                Repo = s.Repo,
                RepoDeployment = s.RepoDeployment,
                RepoStatus = s.RepoStatus,
                WriteOrg = s.WriteOrg,
                WritePublicKey = s.WritePublicKey,
                WriteRepoHook = s.WriteRepoHook,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsGitHub? Revert(V2alpha3ConnectionOptionsGitHub? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsGitHub
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                AdminOrg = s.AdminOrg,
                AdminPublicKey = s.AdminPublicKey,
                AdminRepoHook = s.AdminRepoHook,
                DeleteRepo = s.DeleteRepo,
                Email = s.Email,
                Follow = s.Follow,
                Gist = s.Gist,
                Notifications = s.Notifications,
                Profile = s.Profile,
                PublicRepo = s.PublicRepo,
                ReadOrg = s.ReadOrg,
                ReadPublicKey = s.ReadPublicKey,
                ReadRepoHook = s.ReadRepoHook,
                ReadUser = s.ReadUser,
                Repo = s.Repo,
                RepoDeployment = s.RepoDeployment,
                RepoStatus = s.RepoStatus,
                WriteOrg = s.WriteOrg,
                WritePublicKey = s.WritePublicKey,
                WriteRepoHook = s.WriteRepoHook,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsGoogleApps? Convert(V2alpha1ConnectionOptionsGoogleApps? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsGoogleApps
            {
                AdminAccessToken = s.AdminAccessToken,
                AdminAccessTokenExpiresin = s.AdminAccessTokenExpiresin,
                AdminRefreshToken = s.AdminRefreshToken,
                AllowSettingLoginScopes = s.AllowSettingLoginScopes,
                ApiEnableGroups = s.ApiEnableGroups,
                ApiEnableUsers = s.ApiEnableUsers,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Domain = s.Domain,
                DomainAliases = s.DomainAliases,
                Email = s.Email,
                ExtAgreedTerms = s.ExtAgreedTerms,
                ExtGroups = s.ExtGroups,
                ExtGroupsExtended = s.ExtGroupsExtended,
                ExtIsAdmin = s.ExtIsAdmin,
                ExtIsSuspended = s.ExtIsSuspended,
                FederatedConnectionsAccessTokens = Convert(s.FederatedConnectionsAccessTokens),
                HandleLoginFromSocial = s.HandleLoginFromSocial,
                IconUrl = s.IconUrl,
                MapUserIdToId = s.MapUserIdToId,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsGoogleApps? Revert(V2alpha3ConnectionOptionsGoogleApps? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsGoogleApps
            {
                AdminAccessToken = s.AdminAccessToken,
                AdminAccessTokenExpiresin = s.AdminAccessTokenExpiresin,
                AdminRefreshToken = s.AdminRefreshToken,
                AllowSettingLoginScopes = s.AllowSettingLoginScopes,
                ApiEnableGroups = s.ApiEnableGroups,
                ApiEnableUsers = s.ApiEnableUsers,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Domain = s.Domain,
                DomainAliases = s.DomainAliases,
                Email = s.Email,
                ExtAgreedTerms = s.ExtAgreedTerms,
                ExtGroups = s.ExtGroups,
                ExtGroupsExtended = s.ExtGroupsExtended,
                ExtIsAdmin = s.ExtIsAdmin,
                ExtIsSuspended = s.ExtIsSuspended,
                FederatedConnectionsAccessTokens = Revert(s.FederatedConnectionsAccessTokens),
                HandleLoginFromSocial = s.HandleLoginFromSocial,
                IconUrl = s.IconUrl,
                MapUserIdToId = s.MapUserIdToId,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsGoogleOAuth2? Convert(V2alpha1ConnectionOptionsGoogleOAuth2? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsGoogleOAuth2
            {
                AllowedAudiences = s.AllowedAudiences,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                IconUrl = s.IconUrl,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                AdsenseManagement = s.AdsenseManagement,
                Analytics = s.Analytics,
                Blogger = s.Blogger,
                Calendar = s.Calendar,
                CalendarAddonsExecute = s.CalendarAddonsExecute,
                CalendarEvents = s.CalendarEvents,
                CalendarEventsReadonly = s.CalendarEventsReadonly,
                CalendarSettingsReadonly = s.CalendarSettingsReadonly,
                ChromeWebStore = s.ChromeWebStore,
                Contacts = s.Contacts,
                ContactsNew = s.ContactsNew,
                ContactsOtherReadonly = s.ContactsOtherReadonly,
                ContactsReadonly = s.ContactsReadonly,
                ContentApiForShopping = s.ContentApiForShopping,
                Coordinate = s.Coordinate,
                CoordinateReadonly = s.CoordinateReadonly,
                DirectoryReadonly = s.DirectoryReadonly,
                DocumentList = s.DocumentList,
                Drive = s.Drive,
                DriveActivity = s.DriveActivity,
                DriveActivityReadonly = s.DriveActivityReadonly,
                DriveAppdata = s.DriveAppdata,
                DriveAppsReadonly = s.DriveAppsReadonly,
                DriveFile = s.DriveFile,
                DriveMetadata = s.DriveMetadata,
                DriveMetadataReadonly = s.DriveMetadataReadonly,
                DrivePhotosReadonly = s.DrivePhotosReadonly,
                DriveReadonly = s.DriveReadonly,
                DriveScripts = s.DriveScripts,
                Email = s.Email,
                Gmail = s.Gmail,
                GmailCompose = s.GmailCompose,
                GmailInsert = s.GmailInsert,
                GmailLabels = s.GmailLabels,
                GmailMetadata = s.GmailMetadata,
                GmailModify = s.GmailModify,
                GmailNew = s.GmailNew,
                GmailReadonly = s.GmailReadonly,
                GmailSend = s.GmailSend,
                GmailSettingsBasic = s.GmailSettingsBasic,
                GmailSettingsSharing = s.GmailSettingsSharing,
                GoogleAffiliateNetwork = s.GoogleAffiliateNetwork,
                GoogleBooks = s.GoogleBooks,
                GoogleCloudStorage = s.GoogleCloudStorage,
                GoogleDrive = s.GoogleDrive,
                GoogleDriveFiles = s.GoogleDriveFiles,
                GooglePlus = s.GooglePlus,
                LatitudeBest = s.LatitudeBest,
                LatitudeCity = s.LatitudeCity,
                Moderator = s.Moderator,
                OfflineAccess = s.OfflineAccess,
                Orkut = s.Orkut,
                PicasaWeb = s.PicasaWeb,
                Profile = s.Profile,
                Sites = s.Sites,
                Tasks = s.Tasks,
                TasksReadonly = s.TasksReadonly,
                UrlShortener = s.UrlShortener,
                WebmasterTools = s.WebmasterTools,
                Youtube = s.Youtube,
                YoutubeChannelmembershipsCreator = s.YoutubeChannelmembershipsCreator,
                YoutubeNew = s.YoutubeNew,
                YoutubeReadonly = s.YoutubeReadonly,
                YoutubeUpload = s.YoutubeUpload,
                Youtubepartner = s.Youtubepartner,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsGoogleOAuth2? Revert(V2alpha3ConnectionOptionsGoogleOAuth2? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsGoogleOAuth2
            {
                AllowedAudiences = s.AllowedAudiences,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                IconUrl = s.IconUrl,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                AdsenseManagement = s.AdsenseManagement,
                Analytics = s.Analytics,
                Blogger = s.Blogger,
                Calendar = s.Calendar,
                CalendarAddonsExecute = s.CalendarAddonsExecute,
                CalendarEvents = s.CalendarEvents,
                CalendarEventsReadonly = s.CalendarEventsReadonly,
                CalendarSettingsReadonly = s.CalendarSettingsReadonly,
                ChromeWebStore = s.ChromeWebStore,
                Contacts = s.Contacts,
                ContactsNew = s.ContactsNew,
                ContactsOtherReadonly = s.ContactsOtherReadonly,
                ContactsReadonly = s.ContactsReadonly,
                ContentApiForShopping = s.ContentApiForShopping,
                Coordinate = s.Coordinate,
                CoordinateReadonly = s.CoordinateReadonly,
                DirectoryReadonly = s.DirectoryReadonly,
                DocumentList = s.DocumentList,
                Drive = s.Drive,
                DriveActivity = s.DriveActivity,
                DriveActivityReadonly = s.DriveActivityReadonly,
                DriveAppdata = s.DriveAppdata,
                DriveAppsReadonly = s.DriveAppsReadonly,
                DriveFile = s.DriveFile,
                DriveMetadata = s.DriveMetadata,
                DriveMetadataReadonly = s.DriveMetadataReadonly,
                DrivePhotosReadonly = s.DrivePhotosReadonly,
                DriveReadonly = s.DriveReadonly,
                DriveScripts = s.DriveScripts,
                Email = s.Email,
                Gmail = s.Gmail,
                GmailCompose = s.GmailCompose,
                GmailInsert = s.GmailInsert,
                GmailLabels = s.GmailLabels,
                GmailMetadata = s.GmailMetadata,
                GmailModify = s.GmailModify,
                GmailNew = s.GmailNew,
                GmailReadonly = s.GmailReadonly,
                GmailSend = s.GmailSend,
                GmailSettingsBasic = s.GmailSettingsBasic,
                GmailSettingsSharing = s.GmailSettingsSharing,
                GoogleAffiliateNetwork = s.GoogleAffiliateNetwork,
                GoogleBooks = s.GoogleBooks,
                GoogleCloudStorage = s.GoogleCloudStorage,
                GoogleDrive = s.GoogleDrive,
                GoogleDriveFiles = s.GoogleDriveFiles,
                GooglePlus = s.GooglePlus,
                LatitudeBest = s.LatitudeBest,
                LatitudeCity = s.LatitudeCity,
                Moderator = s.Moderator,
                OfflineAccess = s.OfflineAccess,
                Orkut = s.Orkut,
                PicasaWeb = s.PicasaWeb,
                Profile = s.Profile,
                Sites = s.Sites,
                Tasks = s.Tasks,
                TasksReadonly = s.TasksReadonly,
                UrlShortener = s.UrlShortener,
                WebmasterTools = s.WebmasterTools,
                Youtube = s.Youtube,
                YoutubeChannelmembershipsCreator = s.YoutubeChannelmembershipsCreator,
                YoutubeNew = s.YoutubeNew,
                YoutubeReadonly = s.YoutubeReadonly,
                YoutubeUpload = s.YoutubeUpload,
                Youtubepartner = s.Youtubepartner,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsIdpinitiated? Convert(V2alpha1ConnectionOptionsIdpinitiated? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsIdpinitiated
            {
                ClientId = s.ClientId,
                ClientProtocol = MapEnumN<V2alpha1ConnectionIdpInitiatedClientProtocol, V2alpha3ConnectionIdpInitiatedClientProtocol>(s.ClientProtocol),
                ClientAuthorizequery = s.ClientAuthorizequery,
            };
        }

        public static V2alpha1ConnectionOptionsIdpinitiated? Revert(V2alpha3ConnectionOptionsIdpinitiated? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsIdpinitiated
            {
                ClientId = s.ClientId,
                ClientProtocol = MapEnumN<V2alpha3ConnectionIdpInitiatedClientProtocol, V2alpha1ConnectionIdpInitiatedClientProtocol>(s.ClientProtocol),
                ClientAuthorizequery = s.ClientAuthorizequery,
            };
        }

        public static V2alpha3ConnectionOptionsIdpinitiatedSaml? Convert(V2alpha1ConnectionOptionsIdpinitiatedSaml? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsIdpinitiatedSaml
            {
                ClientAuthorizequery = s.ClientAuthorizequery,
                ClientId = s.ClientId,
                ClientProtocol = MapEnumN<V2alpha1ConnectionOptionsIdpInitiatedClientProtocolEnumSaml, V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml>(s.ClientProtocol),
                Enabled = s.Enabled,
            };
        }

        public static V2alpha1ConnectionOptionsIdpinitiatedSaml? Revert(V2alpha3ConnectionOptionsIdpinitiatedSaml? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsIdpinitiatedSaml
            {
                ClientAuthorizequery = s.ClientAuthorizequery,
                ClientId = s.ClientId,
                ClientProtocol = MapEnumN<V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml, V2alpha1ConnectionOptionsIdpInitiatedClientProtocolEnumSaml>(s.ClientProtocol),
                Enabled = s.Enabled,
            };
        }

        public static V2alpha3ConnectionOptionsInstagram? Convert(V2alpha1ConnectionOptionsInstagram? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsInstagram
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsInstagram? Revert(V2alpha3ConnectionOptionsInstagram? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsInstagram
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsKerberos? Convert(V2alpha1ConnectionOptionsKerberos? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsKerberos
            {
                Enabled = s.Enabled,
                Ips = s.Ips,
            };
        }

        public static V2alpha1ConnectionOptionsKerberos? Revert(V2alpha3ConnectionOptionsKerberos? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsKerberos
            {
                Enabled = s.Enabled,
                Ips = s.Ips,
            };
        }

        public static V2alpha3ConnectionOptionsKeyPair? Convert(V2alpha1ConnectionOptionsKeyPair? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsKeyPair
            {
                Key = s.Key,
                Cert = s.Cert,
            };
        }

        public static V2alpha1ConnectionOptionsKeyPair? Revert(V2alpha3ConnectionOptionsKeyPair? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsKeyPair
            {
                Key = s.Key,
                Cert = s.Cert,
            };
        }

        public static V2alpha3ConnectionOptionsLine? Convert(V2alpha1ConnectionOptionsLine? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsLine
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                Email = s.Email,
                Profile = s.Profile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsLine? Revert(V2alpha3ConnectionOptionsLine? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsLine
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                Email = s.Email,
                Profile = s.Profile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsLinkedin? Convert(V2alpha1ConnectionOptionsLinkedin? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsLinkedin
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                StrategyVersion = s.StrategyVersion,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                BasicProfile = s.BasicProfile,
                Email = s.Email,
                FullProfile = s.FullProfile,
                Network = s.Network,
                Openid = s.Openid,
                Profile = s.Profile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsLinkedin? Revert(V2alpha3ConnectionOptionsLinkedin? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsLinkedin
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                StrategyVersion = s.StrategyVersion,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                BasicProfile = s.BasicProfile,
                Email = s.Email,
                FullProfile = s.FullProfile,
                Network = s.Network,
                Openid = s.Openid,
                Profile = s.Profile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsMfa? Convert(V2alpha1ConnectionOptionsMfa? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsMfa
            {
                Active = s.Active,
                ReturnEnrollSettings = s.ReturnEnrollSettings,
            };
        }

        public static V2alpha1ConnectionOptionsMfa? Revert(V2alpha3ConnectionOptionsMfa? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsMfa
            {
                Active = s.Active,
                ReturnEnrollSettings = s.ReturnEnrollSettings,
            };
        }

        public static V2alpha3ConnectionOptionsOAuth1? Convert(V2alpha1ConnectionOptionsOAuth1? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOAuth1
            {
                AccessTokenUrl = s.AccessTokenUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                RequestTokenUrl = s.RequestTokenUrl,
                Scripts = Convert(s.Scripts),
                SignatureMethod = MapEnumN<V2alpha1ConnectionSignatureMethodOAuth1, V2alpha3ConnectionSignatureMethodOAuth1>(s.SignatureMethod),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UserAuthorizationUrl = s.UserAuthorizationUrl,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsOAuth1? Revert(V2alpha3ConnectionOptionsOAuth1? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOAuth1
            {
                AccessTokenUrl = s.AccessTokenUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                RequestTokenUrl = s.RequestTokenUrl,
                Scripts = Revert(s.Scripts),
                SignatureMethod = MapEnumN<V2alpha3ConnectionSignatureMethodOAuth1, V2alpha1ConnectionSignatureMethodOAuth1>(s.SignatureMethod),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UserAuthorizationUrl = s.UserAuthorizationUrl,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsOAuth1Common? Convert(V2alpha1ConnectionOptionsOAuth1Common? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOAuth1Common
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsOAuth1Common? Revert(V2alpha3ConnectionOptionsOAuth1Common? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOAuth1Common
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsOAuth2? Convert(V2alpha1ConnectionOptionsOAuth2? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOAuth2
            {
                AuthParams = s.AuthParams,
                AuthParamsMap = s.AuthParamsMap,
                AuthorizationUrl = s.AuthorizationUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                CustomHeaders = s.CustomHeaders,
                FieldsMap = s.FieldsMap,
                IconUrl = s.IconUrl,
                LogoutUrl = s.LogoutUrl,
                PkceEnabled = s.PkceEnabled,
                Scope = s.Scope,
                Scripts = Convert(s.Scripts),
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TokenUrl = s.TokenUrl,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UseOauthSpecScope = s.UseOauthSpecScope,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsOAuth2? Revert(V2alpha3ConnectionOptionsOAuth2? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOAuth2
            {
                AuthParams = s.AuthParams,
                AuthParamsMap = s.AuthParamsMap,
                AuthorizationUrl = s.AuthorizationUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                CustomHeaders = s.CustomHeaders,
                FieldsMap = s.FieldsMap,
                IconUrl = s.IconUrl,
                LogoutUrl = s.LogoutUrl,
                PkceEnabled = s.PkceEnabled,
                Scope = s.Scope,
                Scripts = Revert(s.Scripts),
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TokenUrl = s.TokenUrl,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UseOauthSpecScope = s.UseOauthSpecScope,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsOAuth2Common? Convert(V2alpha1ConnectionOptionsOAuth2Common? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOAuth2Common
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsOAuth2Common? Revert(V2alpha3ConnectionOptionsOAuth2Common? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOAuth2Common
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsOffice365? Convert(V2alpha1ConnectionOptionsOffice365? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOffice365
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
            };
        }

        public static V2alpha1ConnectionOptionsOffice365? Revert(V2alpha3ConnectionOptionsOffice365? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOffice365
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
            };
        }

        public static V2alpha3ConnectionOptionsOidc? Convert(V2alpha1ConnectionOptionsOidc? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOidc
            {
                AttributeMap = Convert(s.AttributeMap),
                DiscoveryUrl = s.DiscoveryUrl,
                Type = MapEnumN<V2alpha1ConnectionTypeEnumOidc, V2alpha3ConnectionTypeEnumOidc>(s.Type),
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                ConnectionSettings = Convert(s.ConnectionSettings),
                DomainAliases = s.DomainAliases,
                DpopSigningAlg = MapEnumN<V2alpha1ConnectionDpopSigningAlgEnum, V2alpha3ConnectionDpopSigningAlgEnum>(s.DpopSigningAlg),
                FederatedConnectionsAccessTokens = Convert(s.FederatedConnectionsAccessTokens),
                IconUrl = s.IconUrl,
                IdTokenSignedResponseAlgs = s.IdTokenSignedResponseAlgs?.Select(__x => MapEnum<V2alpha1ConnectionIdTokenSignedResponseAlgEnum, V2alpha3ConnectionIdTokenSignedResponseAlgEnum>(__x)).ToArray(),
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OidcMetadata = Convert(s.OidcMetadata),
                Scope = s.Scope,
                SendBackChannelNonce = s.SendBackChannelNonce,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethod = MapEnumN<V2alpha1ConnectionTokenEndpointAuthMethodEnum, V2alpha3ConnectionTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                TokenEndpointAuthSigningAlg = MapEnumN<V2alpha1ConnectionTokenEndpointAuthSigningAlgEnum, V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum>(s.TokenEndpointAuthSigningAlg),
                TokenEndpointJwtcaAudFormat = MapEnumN<V2alpha1ConnectionTokenEndpointJwtcaAudFormatEnumOidc, V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc>(s.TokenEndpointJwtcaAudFormat),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UserinfoEndpoint = s.UserinfoEndpoint,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsOidc? Revert(V2alpha3ConnectionOptionsOidc? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOidc
            {
                AttributeMap = Revert(s.AttributeMap),
                DiscoveryUrl = s.DiscoveryUrl,
                Type = MapEnumN<V2alpha3ConnectionTypeEnumOidc, V2alpha1ConnectionTypeEnumOidc>(s.Type),
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                ConnectionSettings = Revert(s.ConnectionSettings),
                DomainAliases = s.DomainAliases,
                DpopSigningAlg = MapEnumN<V2alpha3ConnectionDpopSigningAlgEnum, V2alpha1ConnectionDpopSigningAlgEnum>(s.DpopSigningAlg),
                FederatedConnectionsAccessTokens = Revert(s.FederatedConnectionsAccessTokens),
                IconUrl = s.IconUrl,
                IdTokenSignedResponseAlgs = s.IdTokenSignedResponseAlgs?.Select(__x => MapEnum<V2alpha3ConnectionIdTokenSignedResponseAlgEnum, V2alpha1ConnectionIdTokenSignedResponseAlgEnum>(__x)).ToArray(),
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OidcMetadata = Revert(s.OidcMetadata),
                Scope = s.Scope,
                SendBackChannelNonce = s.SendBackChannelNonce,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethod = MapEnumN<V2alpha3ConnectionTokenEndpointAuthMethodEnum, V2alpha1ConnectionTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                TokenEndpointAuthSigningAlg = MapEnumN<V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum, V2alpha1ConnectionTokenEndpointAuthSigningAlgEnum>(s.TokenEndpointAuthSigningAlg),
                TokenEndpointJwtcaAudFormat = MapEnumN<V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc, V2alpha1ConnectionTokenEndpointJwtcaAudFormatEnumOidc>(s.TokenEndpointJwtcaAudFormat),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UserinfoEndpoint = s.UserinfoEndpoint,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsOidcMetadata? Convert(V2alpha1ConnectionOptionsOidcMetadata? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOidcMetadata
            {
                AcrValuesSupported = s.AcrValuesSupported,
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClaimTypesSupported = s.ClaimTypesSupported,
                ClaimsLocalesSupported = s.ClaimsLocalesSupported,
                ClaimsParameterSupported = s.ClaimsParameterSupported,
                ClaimsSupported = s.ClaimsSupported,
                DisplayValuesSupported = s.DisplayValuesSupported,
                DpopSigningAlgValuesSupported = s.DpopSigningAlgValuesSupported,
                EndSessionEndpoint = s.EndSessionEndpoint,
                GrantTypesSupported = s.GrantTypesSupported,
                IdTokenEncryptionAlgValuesSupported = s.IdTokenEncryptionAlgValuesSupported,
                IdTokenEncryptionEncValuesSupported = s.IdTokenEncryptionEncValuesSupported,
                IdTokenSigningAlgValuesSupported = s.IdTokenSigningAlgValuesSupported,
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OpPolicyUri = s.OpPolicyUri,
                OpTosUri = s.OpTosUri,
                RegistrationEndpoint = s.RegistrationEndpoint,
                RequestObjectEncryptionAlgValuesSupported = s.RequestObjectEncryptionAlgValuesSupported,
                RequestObjectEncryptionEncValuesSupported = s.RequestObjectEncryptionEncValuesSupported,
                RequestObjectSigningAlgValuesSupported = s.RequestObjectSigningAlgValuesSupported,
                RequestParameterSupported = s.RequestParameterSupported,
                RequestUriParameterSupported = s.RequestUriParameterSupported,
                RequireRequestUriRegistration = s.RequireRequestUriRegistration,
                ResponseModesSupported = s.ResponseModesSupported,
                ResponseTypesSupported = s.ResponseTypesSupported,
                ScopesSupported = s.ScopesSupported,
                ServiceDocumentation = s.ServiceDocumentation,
                SubjectTypesSupported = s.SubjectTypesSupported,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethodsSupported = s.TokenEndpointAuthMethodsSupported,
                TokenEndpointAuthSigningAlgValuesSupported = s.TokenEndpointAuthSigningAlgValuesSupported,
                UiLocalesSupported = s.UiLocalesSupported,
                UserinfoEncryptionAlgValuesSupported = s.UserinfoEncryptionAlgValuesSupported,
                UserinfoEncryptionEncValuesSupported = s.UserinfoEncryptionEncValuesSupported,
                UserinfoEndpoint = s.UserinfoEndpoint,
                UserinfoSigningAlgValuesSupported = s.UserinfoSigningAlgValuesSupported,
            };
        }

        public static V2alpha1ConnectionOptionsOidcMetadata? Revert(V2alpha3ConnectionOptionsOidcMetadata? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOidcMetadata
            {
                AcrValuesSupported = s.AcrValuesSupported,
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClaimTypesSupported = s.ClaimTypesSupported,
                ClaimsLocalesSupported = s.ClaimsLocalesSupported,
                ClaimsParameterSupported = s.ClaimsParameterSupported,
                ClaimsSupported = s.ClaimsSupported,
                DisplayValuesSupported = s.DisplayValuesSupported,
                DpopSigningAlgValuesSupported = s.DpopSigningAlgValuesSupported,
                EndSessionEndpoint = s.EndSessionEndpoint,
                GrantTypesSupported = s.GrantTypesSupported,
                IdTokenEncryptionAlgValuesSupported = s.IdTokenEncryptionAlgValuesSupported,
                IdTokenEncryptionEncValuesSupported = s.IdTokenEncryptionEncValuesSupported,
                IdTokenSigningAlgValuesSupported = s.IdTokenSigningAlgValuesSupported,
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OpPolicyUri = s.OpPolicyUri,
                OpTosUri = s.OpTosUri,
                RegistrationEndpoint = s.RegistrationEndpoint,
                RequestObjectEncryptionAlgValuesSupported = s.RequestObjectEncryptionAlgValuesSupported,
                RequestObjectEncryptionEncValuesSupported = s.RequestObjectEncryptionEncValuesSupported,
                RequestObjectSigningAlgValuesSupported = s.RequestObjectSigningAlgValuesSupported,
                RequestParameterSupported = s.RequestParameterSupported,
                RequestUriParameterSupported = s.RequestUriParameterSupported,
                RequireRequestUriRegistration = s.RequireRequestUriRegistration,
                ResponseModesSupported = s.ResponseModesSupported,
                ResponseTypesSupported = s.ResponseTypesSupported,
                ScopesSupported = s.ScopesSupported,
                ServiceDocumentation = s.ServiceDocumentation,
                SubjectTypesSupported = s.SubjectTypesSupported,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethodsSupported = s.TokenEndpointAuthMethodsSupported,
                TokenEndpointAuthSigningAlgValuesSupported = s.TokenEndpointAuthSigningAlgValuesSupported,
                UiLocalesSupported = s.UiLocalesSupported,
                UserinfoEncryptionAlgValuesSupported = s.UserinfoEncryptionAlgValuesSupported,
                UserinfoEncryptionEncValuesSupported = s.UserinfoEncryptionEncValuesSupported,
                UserinfoEndpoint = s.UserinfoEndpoint,
                UserinfoSigningAlgValuesSupported = s.UserinfoSigningAlgValuesSupported,
            };
        }

        public static V2alpha3ConnectionOptionsOkta? Convert(V2alpha1ConnectionOptionsOkta? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsOkta
            {
                AttributeMap = Convert(s.AttributeMap),
                Domain = s.Domain,
                Type = MapEnumN<V2alpha1ConnectionTypeEnumOkta, V2alpha3ConnectionTypeEnumOkta>(s.Type),
                NonPersistentAttrs = s.NonPersistentAttrs,
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                ConnectionSettings = Convert(s.ConnectionSettings),
                DomainAliases = s.DomainAliases,
                DpopSigningAlg = MapEnumN<V2alpha1ConnectionDpopSigningAlgEnum, V2alpha3ConnectionDpopSigningAlgEnum>(s.DpopSigningAlg),
                FederatedConnectionsAccessTokens = Convert(s.FederatedConnectionsAccessTokens),
                IconUrl = s.IconUrl,
                IdTokenSignedResponseAlgs = s.IdTokenSignedResponseAlgs?.Select(__x => MapEnum<V2alpha1ConnectionIdTokenSignedResponseAlgEnum, V2alpha3ConnectionIdTokenSignedResponseAlgEnum>(__x)).ToArray(),
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OidcMetadata = Convert(s.OidcMetadata),
                Scope = s.Scope,
                SendBackChannelNonce = s.SendBackChannelNonce,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethod = MapEnumN<V2alpha1ConnectionTokenEndpointAuthMethodEnum, V2alpha3ConnectionTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                TokenEndpointAuthSigningAlg = MapEnumN<V2alpha1ConnectionTokenEndpointAuthSigningAlgEnum, V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum>(s.TokenEndpointAuthSigningAlg),
                TokenEndpointJwtcaAudFormat = MapEnumN<V2alpha1ConnectionTokenEndpointJwtcaAudFormatEnumOidc, V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc>(s.TokenEndpointJwtcaAudFormat),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                UserinfoEndpoint = s.UserinfoEndpoint,
            };
        }

        public static V2alpha1ConnectionOptionsOkta? Revert(V2alpha3ConnectionOptionsOkta? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsOkta
            {
                AttributeMap = Revert(s.AttributeMap),
                Domain = s.Domain,
                Type = MapEnumN<V2alpha3ConnectionTypeEnumOkta, V2alpha1ConnectionTypeEnumOkta>(s.Type),
                NonPersistentAttrs = s.NonPersistentAttrs,
                AuthorizationEndpoint = s.AuthorizationEndpoint,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                ConnectionSettings = Revert(s.ConnectionSettings),
                DomainAliases = s.DomainAliases,
                DpopSigningAlg = MapEnumN<V2alpha3ConnectionDpopSigningAlgEnum, V2alpha1ConnectionDpopSigningAlgEnum>(s.DpopSigningAlg),
                FederatedConnectionsAccessTokens = Revert(s.FederatedConnectionsAccessTokens),
                IconUrl = s.IconUrl,
                IdTokenSignedResponseAlgs = s.IdTokenSignedResponseAlgs?.Select(__x => MapEnum<V2alpha3ConnectionIdTokenSignedResponseAlgEnum, V2alpha1ConnectionIdTokenSignedResponseAlgEnum>(__x)).ToArray(),
                Issuer = s.Issuer,
                JwksUri = s.JwksUri,
                OidcMetadata = Revert(s.OidcMetadata),
                Scope = s.Scope,
                SendBackChannelNonce = s.SendBackChannelNonce,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                TenantDomain = s.TenantDomain,
                TokenEndpoint = s.TokenEndpoint,
                TokenEndpointAuthMethod = MapEnumN<V2alpha3ConnectionTokenEndpointAuthMethodEnum, V2alpha1ConnectionTokenEndpointAuthMethodEnum>(s.TokenEndpointAuthMethod),
                TokenEndpointAuthSigningAlg = MapEnumN<V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum, V2alpha1ConnectionTokenEndpointAuthSigningAlgEnum>(s.TokenEndpointAuthSigningAlg),
                TokenEndpointJwtcaAudFormat = MapEnumN<V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc, V2alpha1ConnectionTokenEndpointJwtcaAudFormatEnumOidc>(s.TokenEndpointJwtcaAudFormat),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                UserinfoEndpoint = s.UserinfoEndpoint,
            };
        }

        public static V2alpha3ConnectionOptionsPasskeyAuthenticationMethod? Convert(V2alpha1ConnectionOptionsPasskeyAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasskeyAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha1ConnectionOptionsPasskeyAuthenticationMethod? Revert(V2alpha3ConnectionOptionsPasskeyAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasskeyAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha3ConnectionOptionsPasskeyOptions? Convert(V2alpha1ConnectionOptionsPasskeyOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasskeyOptions
            {
                ChallengeUi = MapEnumN<V2alpha1ConnectionChallengeUi, V2alpha3ConnectionChallengeUi>(s.ChallengeUi),
                ProgressiveEnrollmentEnabled = s.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = s.LocalEnrollmentEnabled,
            };
        }

        public static V2alpha1ConnectionOptionsPasskeyOptions? Revert(V2alpha3ConnectionOptionsPasskeyOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasskeyOptions
            {
                ChallengeUi = MapEnumN<V2alpha3ConnectionChallengeUi, V2alpha1ConnectionChallengeUi>(s.ChallengeUi),
                ProgressiveEnrollmentEnabled = s.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = s.LocalEnrollmentEnabled,
            };
        }

        public static V2alpha3ConnectionOptionsPasswordAuthenticationMethod? Convert(V2alpha1ConnectionOptionsPasswordAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasswordAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha1ConnectionOptionsPasswordAuthenticationMethod? Revert(V2alpha3ConnectionOptionsPasswordAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasswordAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha3ConnectionOptionsPasswordComplexityOptions? Convert(V2alpha1ConnectionOptionsPasswordComplexityOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasswordComplexityOptions
            {
                MinLength = s.MinLength,
            };
        }

        public static V2alpha1ConnectionOptionsPasswordComplexityOptions? Revert(V2alpha3ConnectionOptionsPasswordComplexityOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasswordComplexityOptions
            {
                MinLength = s.MinLength,
            };
        }

        public static V2alpha3ConnectionOptionsPasswordDictionary? Convert(V2alpha1ConnectionOptionsPasswordDictionary? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasswordDictionary
            {
                Enable = s.Enable,
                Dictionary = s.Dictionary,
            };
        }

        public static V2alpha1ConnectionOptionsPasswordDictionary? Revert(V2alpha3ConnectionOptionsPasswordDictionary? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasswordDictionary
            {
                Enable = s.Enable,
                Dictionary = s.Dictionary,
            };
        }

        public static V2alpha3ConnectionOptionsPasswordHistory? Convert(V2alpha1ConnectionOptionsPasswordHistory? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasswordHistory
            {
                Enable = s.Enable,
                Size = s.Size,
            };
        }

        public static V2alpha1ConnectionOptionsPasswordHistory? Revert(V2alpha3ConnectionOptionsPasswordHistory? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasswordHistory
            {
                Enable = s.Enable,
                Size = s.Size,
            };
        }

        public static V2alpha3ConnectionOptionsPasswordNoPersonalInfo? Convert(V2alpha1ConnectionOptionsPasswordNoPersonalInfo? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPasswordNoPersonalInfo
            {
                Enable = s.Enable,
            };
        }

        public static V2alpha1ConnectionOptionsPasswordNoPersonalInfo? Revert(V2alpha3ConnectionOptionsPasswordNoPersonalInfo? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPasswordNoPersonalInfo
            {
                Enable = s.Enable,
            };
        }

        public static V2alpha3ConnectionOptionsPaypal? Convert(V2alpha1ConnectionOptionsPaypal? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPaypal
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                Address = s.Address,
                Email = s.Email,
                Phone = s.Phone,
                Profile = s.Profile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsPaypal? Revert(V2alpha3ConnectionOptionsPaypal? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPaypal
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                Address = s.Address,
                Email = s.Email,
                Phone = s.Phone,
                Profile = s.Profile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsPhoneNumberAttribute? Convert(V2alpha1ConnectionOptionsPhoneNumberAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPhoneNumberAttribute
            {
                Signup = Convert(s.Signup),
            };
        }

        public static V2alpha1ConnectionOptionsPhoneNumberAttribute? Revert(V2alpha3ConnectionOptionsPhoneNumberAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPhoneNumberAttribute
            {
                Signup = Revert(s.Signup),
            };
        }

        public static V2alpha3ConnectionOptionsPhoneNumberSignup? Convert(V2alpha1ConnectionOptionsPhoneNumberSignup? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPhoneNumberSignup
            {
                Status = MapEnumN<V2alpha1ConnectionOptionsAttributeStatus, V2alpha3ConnectionOptionsAttributeStatus>(s.Status),
                Verification = Convert(s.Verification),
            };
        }

        public static V2alpha1ConnectionOptionsPhoneNumberSignup? Revert(V2alpha3ConnectionOptionsPhoneNumberSignup? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPhoneNumberSignup
            {
                Status = MapEnumN<V2alpha3ConnectionOptionsAttributeStatus, V2alpha1ConnectionOptionsAttributeStatus>(s.Status),
                Verification = Revert(s.Verification),
            };
        }

        public static V2alpha3ConnectionOptionsPingFederate? Convert(V2alpha1ConnectionOptionsPingFederate? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPingFederate
            {
                PingFederateBaseUrl = s.PingFederateBaseUrl,
                SigningCert = s.SigningCert,
                AssertionDecryptionSettings = Convert(s.AssertionDecryptionSettings),
                Cert = s.Cert,
                DecryptionKey = Convert(s.DecryptionKey),
                DigestAlgorithm = MapEnumN<V2alpha1ConnectionDigestAlgorithmEnumSaml, V2alpha3ConnectionDigestAlgorithmEnumSaml>(s.DigestAlgorithm),
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                IconUrl = s.IconUrl,
                Idpinitiated = Convert(s.Idpinitiated),
                ProtocolBinding = MapEnumN<V2alpha1ConnectionProtocolBindingEnumSaml, V2alpha3ConnectionProtocolBindingEnumSaml>(s.ProtocolBinding),
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                SignSamlRequest = s.SignSamlRequest,
                SignatureAlgorithm = MapEnumN<V2alpha1ConnectionSignatureAlgorithmEnumSaml, V2alpha3ConnectionSignatureAlgorithmEnumSaml>(s.SignatureAlgorithm),
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsPingFederate? Revert(V2alpha3ConnectionOptionsPingFederate? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPingFederate
            {
                PingFederateBaseUrl = s.PingFederateBaseUrl,
                SigningCert = s.SigningCert,
                AssertionDecryptionSettings = Revert(s.AssertionDecryptionSettings),
                Cert = s.Cert,
                DecryptionKey = Revert(s.DecryptionKey),
                DigestAlgorithm = MapEnumN<V2alpha3ConnectionDigestAlgorithmEnumSaml, V2alpha1ConnectionDigestAlgorithmEnumSaml>(s.DigestAlgorithm),
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                IconUrl = s.IconUrl,
                Idpinitiated = Revert(s.Idpinitiated),
                ProtocolBinding = MapEnumN<V2alpha3ConnectionProtocolBindingEnumSaml, V2alpha1ConnectionProtocolBindingEnumSaml>(s.ProtocolBinding),
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                SignSamlRequest = s.SignSamlRequest,
                SignatureAlgorithm = MapEnumN<V2alpha3ConnectionSignatureAlgorithmEnumSaml, V2alpha1ConnectionSignatureAlgorithmEnumSaml>(s.SignatureAlgorithm),
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsPlanningCenter? Convert(V2alpha1ConnectionOptionsPlanningCenter? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsPlanningCenter
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsPlanningCenter? Revert(V2alpha3ConnectionOptionsPlanningCenter? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsPlanningCenter
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsSalesforce? Convert(V2alpha1ConnectionOptionsSalesforce? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsSalesforce
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsSalesforce? Revert(V2alpha3ConnectionOptionsSalesforce? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsSalesforce
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsSalesforceCommunity? Convert(V2alpha1ConnectionOptionsSalesforceCommunity? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsSalesforceCommunity
            {
                CommunityBaseUrl = s.CommunityBaseUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsSalesforceCommunity? Revert(V2alpha3ConnectionOptionsSalesforceCommunity? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsSalesforceCommunity
            {
                CommunityBaseUrl = s.CommunityBaseUrl,
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Profile = s.Profile,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsSaml? Convert(V2alpha1ConnectionOptionsSaml? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsSaml
            {
                Debug = s.Debug,
                Deflate = s.Deflate,
                DestinationUrl = s.DestinationUrl,
                DisableSignout = s.DisableSignout,
                FieldsMap = s.FieldsMap,
                GlobalTokenRevocationJwtIss = s.GlobalTokenRevocationJwtIss,
                GlobalTokenRevocationJwtSub = s.GlobalTokenRevocationJwtSub,
                MetadataUrl = s.MetadataUrl,
                MetadataXml = s.MetadataXml,
                RecipientUrl = s.RecipientUrl,
                RequestTemplate = s.RequestTemplate,
                SigningCert = s.SigningCert,
                SigningKey = Convert(s.SigningKey),
                SignOutEndpoint = s.SignOutEndpoint,
                UserIdAttribute = s.UserIdAttribute,
                AssertionDecryptionSettings = Convert(s.AssertionDecryptionSettings),
                Cert = s.Cert,
                DecryptionKey = Convert(s.DecryptionKey),
                DigestAlgorithm = MapEnumN<V2alpha1ConnectionDigestAlgorithmEnumSaml, V2alpha3ConnectionDigestAlgorithmEnumSaml>(s.DigestAlgorithm),
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                IconUrl = s.IconUrl,
                Idpinitiated = Convert(s.Idpinitiated),
                ProtocolBinding = MapEnumN<V2alpha1ConnectionProtocolBindingEnumSaml, V2alpha3ConnectionProtocolBindingEnumSaml>(s.ProtocolBinding),
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                SignSamlRequest = s.SignSamlRequest,
                SignatureAlgorithm = MapEnumN<V2alpha1ConnectionSignatureAlgorithmEnumSaml, V2alpha3ConnectionSignatureAlgorithmEnumSaml>(s.SignatureAlgorithm),
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsSaml? Revert(V2alpha3ConnectionOptionsSaml? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsSaml
            {
                Debug = s.Debug,
                Deflate = s.Deflate,
                DestinationUrl = s.DestinationUrl,
                DisableSignout = s.DisableSignout,
                FieldsMap = s.FieldsMap,
                GlobalTokenRevocationJwtIss = s.GlobalTokenRevocationJwtIss,
                GlobalTokenRevocationJwtSub = s.GlobalTokenRevocationJwtSub,
                MetadataUrl = s.MetadataUrl,
                MetadataXml = s.MetadataXml,
                RecipientUrl = s.RecipientUrl,
                RequestTemplate = s.RequestTemplate,
                SigningCert = s.SigningCert,
                SigningKey = Revert(s.SigningKey),
                SignOutEndpoint = s.SignOutEndpoint,
                UserIdAttribute = s.UserIdAttribute,
                AssertionDecryptionSettings = Revert(s.AssertionDecryptionSettings),
                Cert = s.Cert,
                DecryptionKey = Revert(s.DecryptionKey),
                DigestAlgorithm = MapEnumN<V2alpha3ConnectionDigestAlgorithmEnumSaml, V2alpha1ConnectionDigestAlgorithmEnumSaml>(s.DigestAlgorithm),
                DomainAliases = s.DomainAliases,
                EntityId = s.EntityId,
                IconUrl = s.IconUrl,
                Idpinitiated = Revert(s.Idpinitiated),
                ProtocolBinding = MapEnumN<V2alpha3ConnectionProtocolBindingEnumSaml, V2alpha1ConnectionProtocolBindingEnumSaml>(s.ProtocolBinding),
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                SignInEndpoint = s.SignInEndpoint,
                SignSamlRequest = s.SignSamlRequest,
                SignatureAlgorithm = MapEnumN<V2alpha3ConnectionSignatureAlgorithmEnumSaml, V2alpha1ConnectionSignatureAlgorithmEnumSaml>(s.SignatureAlgorithm),
                TenantDomain = s.TenantDomain,
                Thumbprints = s.Thumbprints,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsScripts? Convert(V2alpha1ConnectionOptionsScripts? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsScripts
            {
                FetchUserProfile = s.FetchUserProfile,
            };
        }

        public static V2alpha1ConnectionOptionsScripts? Revert(V2alpha3ConnectionOptionsScripts? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsScripts
            {
                FetchUserProfile = s.FetchUserProfile,
            };
        }

        public static V2alpha3ConnectionOptionsSharepoint? Convert(V2alpha1ConnectionOptionsSharepoint? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsSharepoint
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsSharepoint? Revert(V2alpha3ConnectionOptionsSharepoint? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsSharepoint
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsShop? Convert(V2alpha1ConnectionOptionsShop? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsShop
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsShop? Revert(V2alpha3ConnectionOptionsShop? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsShop
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsShopify? Convert(V2alpha1ConnectionOptionsShopify? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsShopify
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsShopify? Revert(V2alpha3ConnectionOptionsShopify? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsShopify
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsSms? Convert(V2alpha1ConnectionOptionsSms? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsSms
            {
                BruteForceProtection = s.BruteForceProtection,
                DisableSignup = s.DisableSignup,
                ForwardReqInfo = s.ForwardReqInfo,
                From = s.From,
                GatewayAuthentication = Convert(s.GatewayAuthentication),
                GatewayUrl = s.GatewayUrl,
                MessagingServiceSid = s.MessagingServiceSid,
                Name = s.Name,
                Provider = MapEnumN<V2alpha1ConnectionProviderEnumSms, V2alpha3ConnectionProviderEnumSms>(s.Provider),
                Syntax = MapEnumN<V2alpha1ConnectionTemplateSyntaxEnumSms, V2alpha3ConnectionTemplateSyntaxEnumSms>(s.Syntax),
                Template = s.Template,
                Totp = Convert(s.Totp),
                TwilioSid = s.TwilioSid,
                TwilioToken = s.TwilioToken,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsSms? Revert(V2alpha3ConnectionOptionsSms? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsSms
            {
                BruteForceProtection = s.BruteForceProtection,
                DisableSignup = s.DisableSignup,
                ForwardReqInfo = s.ForwardReqInfo,
                From = s.From,
                GatewayAuthentication = Revert(s.GatewayAuthentication),
                GatewayUrl = s.GatewayUrl,
                MessagingServiceSid = s.MessagingServiceSid,
                Name = s.Name,
                Provider = MapEnumN<V2alpha3ConnectionProviderEnumSms, V2alpha1ConnectionProviderEnumSms>(s.Provider),
                Syntax = MapEnumN<V2alpha3ConnectionTemplateSyntaxEnumSms, V2alpha1ConnectionTemplateSyntaxEnumSms>(s.Syntax),
                Template = s.Template,
                Totp = Revert(s.Totp),
                TwilioSid = s.TwilioSid,
                TwilioToken = s.TwilioToken,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsSoundcloud? Convert(V2alpha1ConnectionOptionsSoundcloud? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsSoundcloud
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsSoundcloud? Revert(V2alpha3ConnectionOptionsSoundcloud? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsSoundcloud
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsThirtySevenSignals? Convert(V2alpha1ConnectionOptionsThirtySevenSignals? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsThirtySevenSignals
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsThirtySevenSignals? Revert(V2alpha3ConnectionOptionsThirtySevenSignals? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsThirtySevenSignals
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsTwitter? Convert(V2alpha1ConnectionOptionsTwitter? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsTwitter
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Protocol = MapEnumN<V2alpha1ConnectionOptionsProtocolEnumTwitter, V2alpha3ConnectionOptionsProtocolEnumTwitter>(s.Protocol),
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                OfflineAccess = s.OfflineAccess,
                Profile = s.Profile,
                TweetRead = s.TweetRead,
                UsersRead = s.UsersRead,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsTwitter? Revert(V2alpha3ConnectionOptionsTwitter? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsTwitter
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Protocol = MapEnumN<V2alpha3ConnectionOptionsProtocolEnumTwitter, V2alpha1ConnectionOptionsProtocolEnumTwitter>(s.Protocol),
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                OfflineAccess = s.OfflineAccess,
                Profile = s.Profile,
                TweetRead = s.TweetRead,
                UsersRead = s.UsersRead,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsUntappd? Convert(V2alpha1ConnectionOptionsUntappd? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsUntappd
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsUntappd? Revert(V2alpha3ConnectionOptionsUntappd? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsUntappd
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsUserName? Convert(V2alpha1ConnectionOptionsUserName? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsUserName
            {
                Min = s.Min,
                Max = s.Max,
            };
        }

        public static V2alpha1ConnectionOptionsUserName? Revert(V2alpha3ConnectionOptionsUserName? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsUserName
            {
                Min = s.Min,
                Max = s.Max,
            };
        }

        public static V2alpha3ConnectionOptionsUsernameAttribute? Convert(V2alpha1ConnectionOptionsUsernameAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsUsernameAttribute
            {
                Identifier = Convert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Convert(s.Signup),
                Validation = Convert(s.Validation),
            };
        }

        public static V2alpha1ConnectionOptionsUsernameAttribute? Revert(V2alpha3ConnectionOptionsUsernameAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsUsernameAttribute
            {
                Identifier = Revert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Revert(s.Signup),
                Validation = Revert(s.Validation),
            };
        }

        public static V2alpha3ConnectionOptionsUsernameSignup? Convert(V2alpha1ConnectionOptionsUsernameSignup? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsUsernameSignup
            {
                Status = MapEnumN<V2alpha1ConnectionOptionsAttributeStatus, V2alpha3ConnectionOptionsAttributeStatus>(s.Status),
            };
        }

        public static V2alpha1ConnectionOptionsUsernameSignup? Revert(V2alpha3ConnectionOptionsUsernameSignup? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsUsernameSignup
            {
                Status = MapEnumN<V2alpha3ConnectionOptionsAttributeStatus, V2alpha1ConnectionOptionsAttributeStatus>(s.Status),
            };
        }

        public static V2alpha3ConnectionOptionsValidation? Convert(V2alpha1ConnectionOptionsValidation? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsValidation
            {
                UserName = Convert(s.UserName),
            };
        }

        public static V2alpha1ConnectionOptionsValidation? Revert(V2alpha3ConnectionOptionsValidation? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsValidation
            {
                UserName = Revert(s.UserName),
            };
        }

        public static V2alpha3ConnectionOptionsVerification? Convert(V2alpha1ConnectionOptionsVerification? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsVerification
            {
                Active = s.Active,
            };
        }

        public static V2alpha1ConnectionOptionsVerification? Revert(V2alpha3ConnectionOptionsVerification? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsVerification
            {
                Active = s.Active,
            };
        }

        public static V2alpha3ConnectionOptionsVkontakte? Convert(V2alpha1ConnectionOptionsVkontakte? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsVkontakte
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsVkontakte? Revert(V2alpha3ConnectionOptionsVkontakte? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsVkontakte
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsWeibo? Convert(V2alpha1ConnectionOptionsWeibo? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsWeibo
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsWeibo? Revert(V2alpha3ConnectionOptionsWeibo? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsWeibo
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsWindowsLive? Convert(V2alpha1ConnectionOptionsWindowsLive? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsWindowsLive
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                StrategyVersion = s.StrategyVersion,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                Applications = s.Applications,
                ApplicationsCreate = s.ApplicationsCreate,
                Basic = s.Basic,
                Birthday = s.Birthday,
                Calendars = s.Calendars,
                CalendarsUpdate = s.CalendarsUpdate,
                ContactsBirthday = s.ContactsBirthday,
                ContactsCalendars = s.ContactsCalendars,
                ContactsCreate = s.ContactsCreate,
                ContactsPhotos = s.ContactsPhotos,
                ContactsSkydrive = s.ContactsSkydrive,
                DirectoryAccessasuserAll = s.DirectoryAccessasuserAll,
                DirectoryReadAll = s.DirectoryReadAll,
                DirectoryReadwriteAll = s.DirectoryReadwriteAll,
                Emails = s.Emails,
                EventsCreate = s.EventsCreate,
                GraphCalendars = s.GraphCalendars,
                GraphCalendarsUpdate = s.GraphCalendarsUpdate,
                GraphContacts = s.GraphContacts,
                GraphContactsUpdate = s.GraphContactsUpdate,
                GraphDevice = s.GraphDevice,
                GraphDeviceCommand = s.GraphDeviceCommand,
                GraphEmails = s.GraphEmails,
                GraphEmailsUpdate = s.GraphEmailsUpdate,
                GraphFiles = s.GraphFiles,
                GraphFilesAll = s.GraphFilesAll,
                GraphFilesAllUpdate = s.GraphFilesAllUpdate,
                GraphFilesUpdate = s.GraphFilesUpdate,
                GraphNotes = s.GraphNotes,
                GraphNotesCreate = s.GraphNotesCreate,
                GraphNotesUpdate = s.GraphNotesUpdate,
                GraphTasks = s.GraphTasks,
                GraphTasksUpdate = s.GraphTasksUpdate,
                GraphUser = s.GraphUser,
                GraphUserActivity = s.GraphUserActivity,
                GraphUserUpdate = s.GraphUserUpdate,
                GroupReadAll = s.GroupReadAll,
                GroupReadwriteAll = s.GroupReadwriteAll,
                MailReadwriteAll = s.MailReadwriteAll,
                MailSend = s.MailSend,
                Messenger = s.Messenger,
                OfflineAccess = s.OfflineAccess,
                PhoneNumbers = s.PhoneNumbers,
                Photos = s.Photos,
                PostalAddresses = s.PostalAddresses,
                RolemanagementReadAll = s.RolemanagementReadAll,
                RolemanagementReadwriteDirectory = s.RolemanagementReadwriteDirectory,
                Share = s.Share,
                Signin = s.Signin,
                SitesReadAll = s.SitesReadAll,
                SitesReadwriteAll = s.SitesReadwriteAll,
                Skydrive = s.Skydrive,
                SkydriveUpdate = s.SkydriveUpdate,
                TeamReadbasicAll = s.TeamReadbasicAll,
                TeamReadwriteAll = s.TeamReadwriteAll,
                UserReadAll = s.UserReadAll,
                UserReadbasicAll = s.UserReadbasicAll,
                WorkProfile = s.WorkProfile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsWindowsLive? Revert(V2alpha3ConnectionOptionsWindowsLive? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsWindowsLive
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                FreeformScopes = s.FreeformScopes,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                StrategyVersion = s.StrategyVersion,
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                Applications = s.Applications,
                ApplicationsCreate = s.ApplicationsCreate,
                Basic = s.Basic,
                Birthday = s.Birthday,
                Calendars = s.Calendars,
                CalendarsUpdate = s.CalendarsUpdate,
                ContactsBirthday = s.ContactsBirthday,
                ContactsCalendars = s.ContactsCalendars,
                ContactsCreate = s.ContactsCreate,
                ContactsPhotos = s.ContactsPhotos,
                ContactsSkydrive = s.ContactsSkydrive,
                DirectoryAccessasuserAll = s.DirectoryAccessasuserAll,
                DirectoryReadAll = s.DirectoryReadAll,
                DirectoryReadwriteAll = s.DirectoryReadwriteAll,
                Emails = s.Emails,
                EventsCreate = s.EventsCreate,
                GraphCalendars = s.GraphCalendars,
                GraphCalendarsUpdate = s.GraphCalendarsUpdate,
                GraphContacts = s.GraphContacts,
                GraphContactsUpdate = s.GraphContactsUpdate,
                GraphDevice = s.GraphDevice,
                GraphDeviceCommand = s.GraphDeviceCommand,
                GraphEmails = s.GraphEmails,
                GraphEmailsUpdate = s.GraphEmailsUpdate,
                GraphFiles = s.GraphFiles,
                GraphFilesAll = s.GraphFilesAll,
                GraphFilesAllUpdate = s.GraphFilesAllUpdate,
                GraphFilesUpdate = s.GraphFilesUpdate,
                GraphNotes = s.GraphNotes,
                GraphNotesCreate = s.GraphNotesCreate,
                GraphNotesUpdate = s.GraphNotesUpdate,
                GraphTasks = s.GraphTasks,
                GraphTasksUpdate = s.GraphTasksUpdate,
                GraphUser = s.GraphUser,
                GraphUserActivity = s.GraphUserActivity,
                GraphUserUpdate = s.GraphUserUpdate,
                GroupReadAll = s.GroupReadAll,
                GroupReadwriteAll = s.GroupReadwriteAll,
                MailReadwriteAll = s.MailReadwriteAll,
                MailSend = s.MailSend,
                Messenger = s.Messenger,
                OfflineAccess = s.OfflineAccess,
                PhoneNumbers = s.PhoneNumbers,
                Photos = s.Photos,
                PostalAddresses = s.PostalAddresses,
                RolemanagementReadAll = s.RolemanagementReadAll,
                RolemanagementReadwriteDirectory = s.RolemanagementReadwriteDirectory,
                Share = s.Share,
                Signin = s.Signin,
                SitesReadAll = s.SitesReadAll,
                SitesReadwriteAll = s.SitesReadwriteAll,
                Skydrive = s.Skydrive,
                SkydriveUpdate = s.SkydriveUpdate,
                TeamReadbasicAll = s.TeamReadbasicAll,
                TeamReadwriteAll = s.TeamReadwriteAll,
                UserReadAll = s.UserReadAll,
                UserReadbasicAll = s.UserReadbasicAll,
                WorkProfile = s.WorkProfile,
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsWordpress? Convert(V2alpha1ConnectionOptionsWordpress? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsWordpress
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsWordpress? Revert(V2alpha3ConnectionOptionsWordpress? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsWordpress
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsYahoo? Convert(V2alpha1ConnectionOptionsYahoo? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsYahoo
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsYahoo? Revert(V2alpha3ConnectionOptionsYahoo? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsYahoo
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionOptionsYandex? Convert(V2alpha1ConnectionOptionsYandex? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionOptionsYandex
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha1ConnectionSetUserRootAttributesEnum, V2alpha3ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha1ConnectionOptionsYandex? Revert(V2alpha3ConnectionOptionsYandex? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionOptionsYandex
            {
                ClientId = s.ClientId,
                ClientSecret = s.ClientSecret,
                Scope = s.Scope,
                SetUserRootAttributes = MapEnumN<V2alpha3ConnectionSetUserRootAttributesEnum, V2alpha1ConnectionSetUserRootAttributesEnum>(s.SetUserRootAttributes),
                UpstreamParams = s.UpstreamParams?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
                NonPersistentAttrs = s.NonPersistentAttrs,
            };
        }

        public static V2alpha3ConnectionPasskeyAuthenticationMethod? Convert(V2alpha1ConnectionPasskeyAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasskeyAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha1ConnectionPasskeyAuthenticationMethod? Revert(V2alpha3ConnectionPasskeyAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasskeyAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha3ConnectionPasskeyOptions? Convert(V2alpha1ConnectionPasskeyOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasskeyOptions
            {
                ChallengeUi = MapEnumN<V2alpha1ConnectionPasskeyChallengeUiEnum, V2alpha3ConnectionPasskeyChallengeUiEnum>(s.ChallengeUi),
                ProgressiveEnrollmentEnabled = s.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = s.LocalEnrollmentEnabled,
            };
        }

        public static V2alpha1ConnectionPasskeyOptions? Revert(V2alpha3ConnectionPasskeyOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasskeyOptions
            {
                ChallengeUi = MapEnumN<V2alpha3ConnectionPasskeyChallengeUiEnum, V2alpha1ConnectionPasskeyChallengeUiEnum>(s.ChallengeUi),
                ProgressiveEnrollmentEnabled = s.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = s.LocalEnrollmentEnabled,
            };
        }

        public static V2alpha3ConnectionPasswordAuthenticationMethod? Convert(V2alpha1ConnectionPasswordAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordAuthenticationMethod
            {
                Enabled = s.Enabled,
                ApiBehavior = MapEnumN<V2alpha1ConnectionApiBehaviorEnum, V2alpha3ConnectionApiBehaviorEnum>(s.ApiBehavior),
                SignupBehavior = MapEnumN<V2alpha1ConnectionSignupBehaviorEnum, V2alpha3ConnectionSignupBehaviorEnum>(s.SignupBehavior),
            };
        }

        public static V2alpha1ConnectionPasswordAuthenticationMethod? Revert(V2alpha3ConnectionPasswordAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordAuthenticationMethod
            {
                Enabled = s.Enabled,
                ApiBehavior = MapEnumN<V2alpha3ConnectionApiBehaviorEnum, V2alpha1ConnectionApiBehaviorEnum>(s.ApiBehavior),
                SignupBehavior = MapEnumN<V2alpha3ConnectionSignupBehaviorEnum, V2alpha1ConnectionSignupBehaviorEnum>(s.SignupBehavior),
            };
        }

        public static V2alpha3ConnectionPasswordComplexityOptions? Convert(V2alpha1ConnectionPasswordComplexityOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordComplexityOptions
            {
                MinLength = s.MinLength,
            };
        }

        public static V2alpha1ConnectionPasswordComplexityOptions? Revert(V2alpha3ConnectionPasswordComplexityOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordComplexityOptions
            {
                MinLength = s.MinLength,
            };
        }

        public static V2alpha3ConnectionPasswordDictionaryOptions? Convert(V2alpha1ConnectionPasswordDictionaryOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordDictionaryOptions
            {
                Enable = s.Enable,
                Dictionary = s.Dictionary,
            };
        }

        public static V2alpha1ConnectionPasswordDictionaryOptions? Revert(V2alpha3ConnectionPasswordDictionaryOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordDictionaryOptions
            {
                Enable = s.Enable,
                Dictionary = s.Dictionary,
            };
        }

        public static V2alpha3ConnectionPasswordHistoryOptions? Convert(V2alpha1ConnectionPasswordHistoryOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordHistoryOptions
            {
                Enable = s.Enable,
                Size = s.Size,
            };
        }

        public static V2alpha1ConnectionPasswordHistoryOptions? Revert(V2alpha3ConnectionPasswordHistoryOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordHistoryOptions
            {
                Enable = s.Enable,
                Size = s.Size,
            };
        }

        public static V2alpha3ConnectionPasswordNoPersonalInfoOptions? Convert(V2alpha1ConnectionPasswordNoPersonalInfoOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordNoPersonalInfoOptions
            {
                Enable = s.Enable,
            };
        }

        public static V2alpha1ConnectionPasswordNoPersonalInfoOptions? Revert(V2alpha3ConnectionPasswordNoPersonalInfoOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordNoPersonalInfoOptions
            {
                Enable = s.Enable,
            };
        }

        public static V2alpha3ConnectionPasswordOptions? Convert(V2alpha1ConnectionPasswordOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordOptions
            {
                Complexity = Convert(s.Complexity),
                Dictionary = Convert(s.Dictionary),
                History = Convert(s.History),
                ProfileData = Convert(s.ProfileData),
            };
        }

        public static V2alpha1ConnectionPasswordOptions? Revert(V2alpha3ConnectionPasswordOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordOptions
            {
                Complexity = Revert(s.Complexity),
                Dictionary = Revert(s.Dictionary),
                History = Revert(s.History),
                ProfileData = Revert(s.ProfileData),
            };
        }

        public static V2alpha3ConnectionPasswordOptionsComplexity? Convert(V2alpha1ConnectionPasswordOptionsComplexity? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordOptionsComplexity
            {
                MinLength = s.MinLength,
                CharacterTypes = s.CharacterTypes?.Select(__x => MapEnum<V2alpha1ConnectionPasswordCharacterTypeEnum, V2alpha3ConnectionPasswordCharacterTypeEnum>(__x)).ToArray(),
                CharacterTypeRule = MapEnumN<V2alpha1ConnectionPasswordCharacterTypeRulePolicyEnum, V2alpha3ConnectionPasswordCharacterTypeRulePolicyEnum>(s.CharacterTypeRule),
                IdenticalCharacters = MapEnumN<V2alpha1ConnectionPasswordIdenticalCharactersPolicyEnum, V2alpha3ConnectionPasswordIdenticalCharactersPolicyEnum>(s.IdenticalCharacters),
                SequentialCharacters = MapEnumN<V2alpha1ConnectionPasswordSequentialCharactersPolicyEnum, V2alpha3ConnectionPasswordSequentialCharactersPolicyEnum>(s.SequentialCharacters),
                MaxLengthExceeded = MapEnumN<V2alpha1ConnectionPasswordMaxLengthExceededPolicyEnum, V2alpha3ConnectionPasswordMaxLengthExceededPolicyEnum>(s.MaxLengthExceeded),
            };
        }

        public static V2alpha1ConnectionPasswordOptionsComplexity? Revert(V2alpha3ConnectionPasswordOptionsComplexity? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordOptionsComplexity
            {
                MinLength = s.MinLength,
                CharacterTypes = s.CharacterTypes?.Select(__x => MapEnum<V2alpha3ConnectionPasswordCharacterTypeEnum, V2alpha1ConnectionPasswordCharacterTypeEnum>(__x)).ToArray(),
                CharacterTypeRule = MapEnumN<V2alpha3ConnectionPasswordCharacterTypeRulePolicyEnum, V2alpha1ConnectionPasswordCharacterTypeRulePolicyEnum>(s.CharacterTypeRule),
                IdenticalCharacters = MapEnumN<V2alpha3ConnectionPasswordIdenticalCharactersPolicyEnum, V2alpha1ConnectionPasswordIdenticalCharactersPolicyEnum>(s.IdenticalCharacters),
                SequentialCharacters = MapEnumN<V2alpha3ConnectionPasswordSequentialCharactersPolicyEnum, V2alpha1ConnectionPasswordSequentialCharactersPolicyEnum>(s.SequentialCharacters),
                MaxLengthExceeded = MapEnumN<V2alpha3ConnectionPasswordMaxLengthExceededPolicyEnum, V2alpha1ConnectionPasswordMaxLengthExceededPolicyEnum>(s.MaxLengthExceeded),
            };
        }

        public static V2alpha3ConnectionPasswordOptionsDictionary? Convert(V2alpha1ConnectionPasswordOptionsDictionary? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordOptionsDictionary
            {
                Active = s.Active,
                Custom = s.Custom,
                Default = MapEnumN<V2alpha1ConnectionPasswordDefaultDictionariesEnum, V2alpha3ConnectionPasswordDefaultDictionariesEnum>(s.Default),
            };
        }

        public static V2alpha1ConnectionPasswordOptionsDictionary? Revert(V2alpha3ConnectionPasswordOptionsDictionary? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordOptionsDictionary
            {
                Active = s.Active,
                Custom = s.Custom,
                Default = MapEnumN<V2alpha3ConnectionPasswordDefaultDictionariesEnum, V2alpha1ConnectionPasswordDefaultDictionariesEnum>(s.Default),
            };
        }

        public static V2alpha3ConnectionPasswordOptionsHistory? Convert(V2alpha1ConnectionPasswordOptionsHistory? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordOptionsHistory
            {
                Active = s.Active,
                Size = s.Size,
            };
        }

        public static V2alpha1ConnectionPasswordOptionsHistory? Revert(V2alpha3ConnectionPasswordOptionsHistory? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordOptionsHistory
            {
                Active = s.Active,
                Size = s.Size,
            };
        }

        public static V2alpha3ConnectionPasswordOptionsProfileData? Convert(V2alpha1ConnectionPasswordOptionsProfileData? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPasswordOptionsProfileData
            {
                Active = s.Active,
                BlockedFields = s.BlockedFields,
            };
        }

        public static V2alpha1ConnectionPasswordOptionsProfileData? Revert(V2alpha3ConnectionPasswordOptionsProfileData? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPasswordOptionsProfileData
            {
                Active = s.Active,
                BlockedFields = s.BlockedFields,
            };
        }

        public static V2alpha3ConnectionPhoneAttribute? Convert(V2alpha1ConnectionPhoneAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPhoneAttribute
            {
                Identifier = Convert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Convert(s.Signup),
            };
        }

        public static V2alpha1ConnectionPhoneAttribute? Revert(V2alpha3ConnectionPhoneAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPhoneAttribute
            {
                Identifier = Revert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Revert(s.Signup),
            };
        }

        public static V2alpha3ConnectionPhoneOtpAuthenticationMethod? Convert(V2alpha1ConnectionPhoneOtpAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionPhoneOtpAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha1ConnectionPhoneOtpAuthenticationMethod? Revert(V2alpha3ConnectionPhoneOtpAuthenticationMethod? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionPhoneOtpAuthenticationMethod
            {
                Enabled = s.Enabled,
            };
        }

        public static V2alpha3ConnectionReadOnlyAdditionalProperties? Convert(V2alpha1ConnectionReadOnlyAdditionalProperties? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionReadOnlyAdditionalProperties
            {
            };
        }

        public static V2alpha1ConnectionReadOnlyAdditionalProperties? Revert(V2alpha3ConnectionReadOnlyAdditionalProperties? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionReadOnlyAdditionalProperties
            {
            };
        }

        public static V2alpha3ConnectionScriptsOAuth1? Convert(V2alpha1ConnectionScriptsOAuth1? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionScriptsOAuth1
            {
                FetchUserProfile = s.FetchUserProfile,
            };
        }

        public static V2alpha1ConnectionScriptsOAuth1? Revert(V2alpha3ConnectionScriptsOAuth1? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionScriptsOAuth1
            {
                FetchUserProfile = s.FetchUserProfile,
            };
        }

        public static V2alpha3ConnectionScriptsOAuth2? Convert(V2alpha1ConnectionScriptsOAuth2? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionScriptsOAuth2
            {
                FetchUserProfile = s.FetchUserProfile,
                GetLogoutUrl = s.GetLogoutUrl,
            };
        }

        public static V2alpha1ConnectionScriptsOAuth2? Revert(V2alpha3ConnectionScriptsOAuth2? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionScriptsOAuth2
            {
                FetchUserProfile = s.FetchUserProfile,
                GetLogoutUrl = s.GetLogoutUrl,
            };
        }

        public static V2alpha3ConnectionSigningKeySaml? Convert(V2alpha1ConnectionSigningKeySaml? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionSigningKeySaml
            {
                Cert = s.Cert,
                Key = s.Key,
            };
        }

        public static V2alpha1ConnectionSigningKeySaml? Revert(V2alpha3ConnectionSigningKeySaml? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionSigningKeySaml
            {
                Cert = s.Cert,
                Key = s.Key,
            };
        }

        public static V2alpha3ConnectionSignupSchema? Convert(V2alpha1ConnectionSignupSchema? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionSignupSchema
            {
                Status = MapEnumN<V2alpha1ConnectionSignupStatusEnum, V2alpha3ConnectionSignupStatusEnum>(s.Status),
            };
        }

        public static V2alpha1ConnectionSignupSchema? Revert(V2alpha3ConnectionSignupSchema? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionSignupSchema
            {
                Status = MapEnumN<V2alpha3ConnectionSignupStatusEnum, V2alpha1ConnectionSignupStatusEnum>(s.Status),
            };
        }

        public static V2alpha3ConnectionSignupVerification? Convert(V2alpha1ConnectionSignupVerification? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionSignupVerification
            {
                Active = s.Active,
            };
        }

        public static V2alpha1ConnectionSignupVerification? Revert(V2alpha3ConnectionSignupVerification? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionSignupVerification
            {
                Active = s.Active,
            };
        }

        public static V2alpha3ConnectionSignupVerified? Convert(V2alpha1ConnectionSignupVerified? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionSignupVerified
            {
                Status = MapEnumN<V2alpha1ConnectionSignupStatusEnum, V2alpha3ConnectionSignupStatusEnum>(s.Status),
                Verification = Convert(s.Verification),
            };
        }

        public static V2alpha1ConnectionSignupVerified? Revert(V2alpha3ConnectionSignupVerified? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionSignupVerified
            {
                Status = MapEnumN<V2alpha3ConnectionSignupStatusEnum, V2alpha1ConnectionSignupStatusEnum>(s.Status),
                Verification = Revert(s.Verification),
            };
        }

        public static V2alpha3ConnectionTotpEmail? Convert(V2alpha1ConnectionTotpEmail? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionTotpEmail
            {
                Length = s.Length,
                TimeStep = s.TimeStep,
            };
        }

        public static V2alpha1ConnectionTotpEmail? Revert(V2alpha3ConnectionTotpEmail? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionTotpEmail
            {
                Length = s.Length,
                TimeStep = s.TimeStep,
            };
        }

        public static V2alpha3ConnectionTotpSms? Convert(V2alpha1ConnectionTotpSms? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionTotpSms
            {
                Length = s.Length,
                TimeStep = s.TimeStep,
            };
        }

        public static V2alpha1ConnectionTotpSms? Revert(V2alpha3ConnectionTotpSms? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionTotpSms
            {
                Length = s.Length,
                TimeStep = s.TimeStep,
            };
        }

        public static V2alpha3ConnectionUpstreamAdditionalProperties? Convert(V2alpha1ConnectionUpstreamAdditionalProperties? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionUpstreamAdditionalProperties
            {
                Alias = MapEnumN<V2alpha1ConnectionUpstreamAliasEnum, V2alpha3ConnectionUpstreamAliasEnum>(s.Alias),
                Value = s.Value,
            };
        }

        public static V2alpha1ConnectionUpstreamAdditionalProperties? Revert(V2alpha3ConnectionUpstreamAdditionalProperties? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionUpstreamAdditionalProperties
            {
                Alias = MapEnumN<V2alpha3ConnectionUpstreamAliasEnum, V2alpha1ConnectionUpstreamAliasEnum>(s.Alias),
                Value = s.Value,
            };
        }

        public static V2alpha3ConnectionUpstreamParam? Convert(V2alpha1ConnectionUpstreamParam? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionUpstreamParam
            {
                Alias = s.Alias,
                Value = s.Value,
            };
        }

        public static V2alpha1ConnectionUpstreamParam? Revert(V2alpha3ConnectionUpstreamParam? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionUpstreamParam
            {
                Alias = s.Alias,
                Value = s.Value,
            };
        }

        public static V2alpha3ConnectionUsernameAllowedTypes? Convert(V2alpha1ConnectionUsernameAllowedTypes? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionUsernameAllowedTypes
            {
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
            };
        }

        public static V2alpha1ConnectionUsernameAllowedTypes? Revert(V2alpha3ConnectionUsernameAllowedTypes? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionUsernameAllowedTypes
            {
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
            };
        }

        public static V2alpha3ConnectionUsernameAttribute? Convert(V2alpha1ConnectionUsernameAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionUsernameAttribute
            {
                Identifier = Convert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Convert(s.Signup),
                Validation = Convert(s.Validation),
            };
        }

        public static V2alpha1ConnectionUsernameAttribute? Revert(V2alpha3ConnectionUsernameAttribute? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionUsernameAttribute
            {
                Identifier = Revert(s.Identifier),
                ProfileRequired = s.ProfileRequired,
                Signup = Revert(s.Signup),
                Validation = Revert(s.Validation),
            };
        }

        public static V2alpha3ConnectionUsernameValidation? Convert(V2alpha1ConnectionUsernameValidation? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionUsernameValidation
            {
                MinLength = s.MinLength,
                MaxLength = s.MaxLength,
                AllowedTypes = Convert(s.AllowedTypes),
            };
        }

        public static V2alpha1ConnectionUsernameValidation? Revert(V2alpha3ConnectionUsernameValidation? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionUsernameValidation
            {
                MinLength = s.MinLength,
                MaxLength = s.MaxLength,
                AllowedTypes = Revert(s.AllowedTypes),
            };
        }

        public static V2alpha3ConnectionUsernameValidationOptions? Convert(V2alpha1ConnectionUsernameValidationOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionUsernameValidationOptions
            {
                Min = s.Min,
                Max = s.Max,
            };
        }

        public static V2alpha1ConnectionUsernameValidationOptions? Revert(V2alpha3ConnectionUsernameValidationOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionUsernameValidationOptions
            {
                Min = s.Min,
                Max = s.Max,
            };
        }

        public static V2alpha3ConnectionValidationOptions? Convert(V2alpha1ConnectionValidationOptions? s)
        {
            if (s is null) return null;
            return new V2alpha3ConnectionValidationOptions
            {
                Username = Convert(s.Username),
            };
        }

        public static V2alpha1ConnectionValidationOptions? Revert(V2alpha3ConnectionValidationOptions? s)
        {
            if (s is null) return null;
            return new V2alpha1ConnectionValidationOptions
            {
                Username = Revert(s.Username),
            };
        }

    }

}
