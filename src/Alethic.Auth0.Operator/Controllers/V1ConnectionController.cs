using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Connections;
using Auth0.ManagementApi.Core;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Connection), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ConnectionController :
        V1TenantEntityInstanceController<V1Connection, V1Connection.SpecDef, V1Connection.StatusDef, V1ConnectionConf, V1ConnectionConf>,
        IEntityController<V1Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ConnectionController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ConnectionController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Connection";

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static TTo? JsonConvertTo<TTo>(object? source)
        {
            return JsonSerializer.Deserialize<TTo>(JsonSerializer.Serialize(source));
        }

        /// <summary>
        /// Converts a <see cref="GetConnectionResponseContent"/> API response to a <see cref="V1ConnectionConf"/>.
        /// Note: <see cref="V1ConnectionConf.EnabledClients"/> is populated separately and left null here.
        /// </summary>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V1ConnectionConf? FromApi(GetConnectionResponseContent? source)
        {
            if (source is null)
                return null;

            var conf = new V1ConnectionConf()
            {
                Name = source.Name,
                DisplayName = source.DisplayName,
                Strategy = source.Strategy,
                Realms = source.Realms?.ToArray(),
                IsDomainConnection = source.IsDomainConnection,
                ShowAsButton = source.ShowAsButton,
                Metadata = source.Metadata is { } md ? new System.Collections.Hashtable(md) : null,
                Options = new V1ConnectionOptions()
            };

            switch (source.Strategy)
            {
                case ConnectionResponseContentAuth0Strategy.Values.Auth0:
                    conf.Options.Auth0 = FromApi(JsonConvertTo<ConnectionResponseContentAuth0>(source)?.Options);
                    break;
                case ConnectionResponseContentAdStrategy.Values.Ad:
                    conf.Options.Ad = FromApi(JsonConvertTo<ConnectionResponseContentAd>(source)?.Options);
                    break;
                case ConnectionResponseContentAdfsStrategy.Values.Adfs:
                    conf.Options.Adfs = FromApi(JsonConvertTo<ConnectionResponseContentAdfs>(source)?.Options);
                    break;
                case ConnectionResponseContentAmazonStrategy.Values.Amazon:
                    break;
                case ConnectionResponseContentAppleStrategy.Values.Apple:
                    break;
                case ConnectionResponseContentAuth0OidcStrategy.Values.Auth0Oidc:
                    conf.Options.Auth0Oidc = FromApi(JsonConvertTo<ConnectionResponseContentAuth0Oidc>(source)?.Options);
                    break;
                case ConnectionResponseContentBaiduStrategy.Values.Baidu:
                    break;
                case ConnectionResponseContentBitbucketStrategy.Values.Bitbucket:
                    conf.Options.Bitbucket = FromApi(JsonConvertTo<ConnectionResponseContentBitbucket>(source)?.Options);
                    break;
                case ConnectionResponseContentBitlyStrategy.Values.Bitly:
                    break;
                case ConnectionResponseContentBoxStrategy.Values.Box:
                    conf.Options.Box = FromApi(JsonConvertTo<ConnectionResponseContentBox>(source)?.Options);
                    break;
                case ConnectionResponseContentDaccountStrategy.Values.Daccount:
                    break;
                case ConnectionResponseContentDropboxStrategy.Values.Dropbox:
                    conf.Options.Dropbox = FromApi(JsonConvertTo<ConnectionResponseContentDropbox>(source)?.Options);
                    break;
                case ConnectionResponseContentDwollaStrategy.Values.Dwolla:
                    break;
                case ConnectionResponseContentEmailStrategy.Values.Email:
                    conf.Options.Email = FromApi(JsonConvertTo<ConnectionResponseContentEmail>(source)?.Options);
                    break;
                case ConnectionResponseContentEvernoteStrategy.Values.Evernote:
                    conf.Options.Evernote = FromApi(JsonConvertTo<ConnectionResponseContentEvernote>(source)?.Options);
                    break;
                case ConnectionResponseContentEvernoteSandboxStrategy.Values.EvernoteSandbox:
                    conf.Options.EvernoteSandbox = FromApi(JsonConvertTo<ConnectionResponseContentEvernoteSandbox>(source)?.Options);
                    break;
                case ConnectionResponseContentExactStrategy.Values.Exact:
                    conf.Options.Exact = FromApi(JsonConvertTo<ConnectionResponseContentExact>(source)?.Options);
                    break;
                case ConnectionResponseContentFacebookStrategy.Values.Facebook:
                    conf.Options.Facebook = FromApi(JsonConvertTo<ConnectionResponseContentFacebook>(source)?.Options);
                    break;
                case ConnectionResponseContentFitbitStrategy.Values.Fitbit:
                    break;
                case ConnectionResponseContentGitHubStrategy.Values.Github:
                    conf.Options.GitHub = FromApi(JsonConvertTo<ConnectionResponseContentGitHub>(source)?.Options);
                    break;
                case ConnectionResponseContentGoogleAppsStrategy.Values.GoogleApps:
                    conf.Options.GoogleApps = FromApi(JsonConvertTo<ConnectionResponseContentGoogleApps>(source)?.Options);
                    break;
                case ConnectionResponseContentGoogleOAuth2Strategy.Values.GoogleOauth2:
                    conf.Options.GoogleOAuth2 = FromApi(JsonConvertTo<ConnectionResponseContentGoogleOAuth2>(source)?.Options);
                    break;
                case ConnectionResponseContentInstagramStrategy.Values.Instagram:
                    break;
                case ConnectionResponseContentLineStrategy.Values.Line:
                    break;
                case ConnectionResponseContentLinkedinStrategy.Values.Linkedin:
                    conf.Options.Linkedin = FromApi(JsonConvertTo<ConnectionResponseContentLinkedin>(source)?.Options);
                    break;
                case ConnectionResponseContentOAuth1Strategy.Values.Oauth1:
                    conf.Options.OAuth1 = FromApi(JsonConvertTo<ConnectionResponseContentOAuth1>(source)?.Options);
                    break;
                case ConnectionResponseContentOAuth2Strategy.Values.Oauth2:
                    conf.Options.OAuth2 = FromApi(JsonConvertTo<ConnectionResponseContentOAuth2>(source)?.Options);
                    break;
                case ConnectionResponseContentOffice365Strategy.Values.Office365:
                    conf.Options.Office365 = FromApi(JsonConvertTo<ConnectionResponseContentOffice365>(source)?.Options);
                    break;
                case ConnectionResponseContentOidcStrategy.Values.Oidc:
                    conf.Options.Oidc = FromApi(JsonConvertTo<ConnectionResponseContentOidc>(source)?.Options);
                    break;
                case ConnectionResponseContentOktaStrategy.Values.Okta:
                    conf.Options.Okta = FromApi(JsonConvertTo<ConnectionResponseContentOkta>(source)?.Options);
                    break;
                case ConnectionResponseContentPaypalStrategy.Values.Paypal:
                    conf.Options.Paypal = FromApi(JsonConvertTo<ConnectionResponseContentPaypal>(source)?.Options);
                    break;
                case ConnectionResponseContentPaypalSandboxStrategy.Values.PaypalSandbox:
                    conf.Options.PaypalSandbox = FromApi(JsonConvertTo<ConnectionResponseContentPaypalSandbox>(source)?.Options);
                    break;
                case ConnectionResponseContentPingFederateStrategy.Values.Pingfederate:
                    conf.Options.PingFederate = FromApi(JsonConvertTo<ConnectionResponseContentPingFederate>(source)?.Options);
                    break;
                case ConnectionResponseContentPlanningCenterStrategy.Values.Planningcenter:
                    break;
                case ConnectionResponseContentSalesforceStrategy.Values.Salesforce:
                    conf.Options.Salesforce = FromApi(JsonConvertTo<ConnectionResponseContentSalesforce>(source)?.Options);
                    break;
                case ConnectionResponseContentSalesforceCommunityStrategy.Values.SalesforceCommunity:
                    conf.Options.SalesforceCommunity = FromApi(JsonConvertTo<ConnectionResponseContentSalesforceCommunity>(source)?.Options);
                    break;
                case ConnectionResponseContentSalesforceSandboxStrategy.Values.SalesforceSandbox:
                    conf.Options.SalesforceSandbox = FromApi(JsonConvertTo<ConnectionResponseContentSalesforceSandbox>(source)?.Options);
                    break;
                case ConnectionResponseContentSamlStrategy.Values.Samlp:
                    conf.Options.Saml = FromApi(JsonConvertTo<ConnectionResponseContentSaml>(source)?.Options);
                    break;
                case ConnectionResponseContentSharepointStrategy.Values.Sharepoint:
                    break;
                case ConnectionResponseContentShopifyStrategy.Values.Shopify:
                    break;
                case ConnectionResponseContentShopStrategy.Values.Shop:
                    break;
                case ConnectionResponseContentSmsStrategy.Values.Sms:
                    conf.Options.Sms = FromApi(JsonConvertTo<ConnectionResponseContentSms>(source)?.Options);
                    break;
                case ConnectionResponseContentSoundcloudStrategy.Values.Soundcloud:
                    break;
                case ConnectionResponseContentThirtySevenSignalsStrategy.Values.Thirtysevensignals:
                    break;
                case ConnectionResponseContentTwitterStrategy.Values.Twitter:
                    conf.Options.Twitter = FromApi(JsonConvertTo<ConnectionResponseContentTwitter>(source)?.Options);
                    break;
                case ConnectionResponseContentUntappdStrategy.Values.Untappd:
                    break;
                case ConnectionResponseContentVkontakteStrategy.Values.Vkontakte:
                    break;
                case ConnectionResponseContentAzureAdStrategy.Values.Waad:
                    conf.Options.AzureAd = FromApi(JsonConvertTo<ConnectionResponseContentAzureAd>(source)?.Options);
                    break;
                case ConnectionResponseContentWeiboStrategy.Values.Weibo:
                    break;
                case ConnectionResponseContentWindowsLiveStrategy.Values.Windowslive:
                    conf.Options.WindowsLive = FromApi(JsonConvertTo<ConnectionResponseContentWindowsLive>(source)?.Options);
                    break;
                case ConnectionResponseContentWordpressStrategy.Values.Wordpress:
                    break;
                case ConnectionResponseContentYahooStrategy.Values.Yahoo:
                    conf.Options.Yahoo = FromApi(JsonConvertTo<ConnectionResponseContentYahoo>(source)?.Options);
                    break;
                case ConnectionResponseContentYandexStrategy.Values.Yandex:
                    break;
                default:
                    break;
            }

            return conf;
        }

        internal static V1ConnectionAuth0Options? FromApi(ConnectionOptionsAuth0? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionAuth0Options
            {
                BruteForceProtection = source.BruteForceProtection,
                DisableSignup = source.DisableSignup,
                EnableScriptContext = source.EnableScriptContext,
                EnabledDatabaseCustomization = source.EnabledDatabaseCustomization,
                ImportMode = source.ImportMode,
                RequiresUsername = source.RequiresUsername,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                PasswordPolicy = source.PasswordPolicy.IsDefined ? FromApi(source.PasswordPolicy.Value)?.ToString()?.ToLowerInvariant() : null,
                PasswordHistory = source.PasswordHistory.IsDefined && source.PasswordHistory.Value is { } ph ? FromApi(ph) : null,
                PasswordNoPersonalInfo = source.PasswordNoPersonalInfo.IsDefined && source.PasswordNoPersonalInfo.Value is { } pnpi ? FromApi(pnpi) : null,
                PasswordDictionary = source.PasswordDictionary.IsDefined && source.PasswordDictionary.Value is { } pd ? FromApi(pd) : null,
                PasswordComplexityOptions = source.PasswordComplexityOptions.IsDefined && source.PasswordComplexityOptions.Value is { } pco ? FromApi(pco) : null,
                Validation = source.Validation.IsDefined && source.Validation.Value is { } v ? FromApi(v) : null,
                CustomScripts = source.CustomScripts is { } cs ? FromApi(cs) : null,
                Mfa = source.Mfa is { } mfa ? FromApi(mfa) : null,
            };
        }

        internal static V1ConnectionAdOptions? FromApi(ConnectionOptionsAd? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionAdOptions
            {
                AgentIp = source.AgentIp,
                AgentMode = source.AgentMode?.ToString(),
                AgentVersion = source.AgentVersion,
                BruteForceProtection = source.BruteForceProtection,
                CertAuth = source.CertAuth,
                Certs = source.Certs?.ToArray(),
                DisableCache = source.DisableCache,
                DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword,
                DomainAliases = source.DomainAliases?.ToArray(),
                IconUrl = source.IconUrl,
                Ips = source.Ips?.ToArray(),
                SignInEndpoint = source.SignInEndpoint,
                TenantDomain = source.TenantDomain,
                Thumbprints = source.Thumbprints?.ToArray(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Kerberos = source.Kerberos is bool kb ? new V1ConnectionOptionsKerberos { Enabled = kb } : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionAdfsOptions? FromApi(ConnectionOptionsAdfs? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionAdfsOptions
            {
                AdfsServer = source.AdfsServer,
                DomainAliases = source.DomainAliases?.ToArray(),
                EntityId = source.EntityId,
                FedMetadataXml = source.FedMetadataXml,
                IconUrl = source.IconUrl,
                PrevThumbprints = source.PrevThumbprints?.ToArray(),
                ShouldTrustEmailVerifiedConnection = source.ShouldTrustEmailVerifiedConnection?.ToString(),
                SignInEndpoint = source.SignInEndpoint,
                TenantDomain = source.TenantDomain,
                Thumbprints = source.Thumbprints?.ToArray(),
                UserIdAttribute = source.UserIdAttribute,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionAuth0OidcOptions? FromApi(ConnectionOptionsAuth0Oidc? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionAuth0OidcOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
            };
        }

        internal static V1ConnectionAzureAdOptions? FromApi(ConnectionOptionsAzureAd? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionAzureAdOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                ApiEnableUsers = source.ApiEnableUsers,
                AppDomain = source.AppDomain,
                AppId = source.AppId,
                BasicProfile = source.BasicProfile,
                DomainAliases = source.DomainAliases?.ToArray(),
                ExtAccessToken = source.ExtAccessToken,
                ExtAccountEnabled = source.ExtAccountEnabled,
                ExtAdmin = source.ExtAdmin,
                ExtAgreedTerms = source.ExtAgreedTerms,
                ExtAssignedLicenses = source.ExtAssignedLicenses,
                ExtAssignedPlans = source.ExtAssignedPlans,
                ExtAzureId = source.ExtAzureId,
                ExtCity = source.ExtCity,
                ExtCountry = source.ExtCountry,
                ExtDepartment = source.ExtDepartment,
                ExtDirSyncEnabled = source.ExtDirSyncEnabled,
                ExtEmail = source.ExtEmail,
                ExtExpiresIn = source.ExtExpiresIn,
                ExtFamilyName = source.ExtFamilyName,
                ExtFax = source.ExtFax,
                ExtGivenName = source.ExtGivenName,
                ExtGroupIds = source.ExtGroupIds,
                ExtGroups = source.ExtGroups,
                ExtIsSuspended = source.ExtIsSuspended,
                ExtJobTitle = source.ExtJobTitle,
                ExtLastSync = source.ExtLastSync,
                ExtMobile = source.ExtMobile,
                ExtName = source.ExtName,
                ExtNestedGroups = source.ExtNestedGroups,
                ExtNickname = source.ExtNickname,
                ExtOid = source.ExtOid,
                ExtPhone = source.ExtPhone,
                ExtPhysicalDeliveryOfficeName = source.ExtPhysicalDeliveryOfficeName,
                ExtPostalCode = source.ExtPostalCode,
                ExtPreferredLanguage = source.ExtPreferredLanguage,
                ExtProfile = source.ExtProfile,
                ExtProvisionedPlans = source.ExtProvisionedPlans,
                ExtProvisioningErrors = source.ExtProvisioningErrors,
                ExtProxyAddresses = source.ExtProxyAddresses,
                ExtPuid = source.ExtPuid,
                ExtRefreshToken = source.ExtRefreshToken,
                ExtRoles = source.ExtRoles,
                ExtState = source.ExtState,
                ExtStreet = source.ExtStreet,
                ExtTelephoneNumber = source.ExtTelephoneNumber,
                ExtTenantid = source.ExtTenantid,
                ExtUpn = source.ExtUpn,
                ExtUsageLocation = source.ExtUsageLocation,
                ExtUserId = source.ExtUserId,
                Granted = source.Granted,
                IconUrl = source.IconUrl,
                IdentityApi = source.IdentityApi?.ToString(),
                MaxGroupsToRetrieve = source.MaxGroupsToRetrieve,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                ShouldTrustEmailVerifiedConnection = source.ShouldTrustEmailVerifiedConnection?.ToString(),
                TenantDomain = source.TenantDomain,
                TenantId = source.TenantId,
                Thumbprints = source.Thumbprints?.ToArray(),
                UseCommonEndpoint = source.UseCommonEndpoint,
                UseWsfed = source.UseWsfed,
                UseridAttribute = source.UseridAttribute?.ToString(),
                WaadProtocol = source.WaadProtocol?.ToString(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionBitbucketOptions? FromApi(ConnectionOptionsBitbucket? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionBitbucketOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Profile = source.Profile,
            };
        }

        internal static V1ConnectionBoxOptions? FromApi(ConnectionOptionsBox? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionBoxOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionDropboxOptions? FromApi(ConnectionOptionsDropbox? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionDropboxOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionEmailOptions? FromApi(ConnectionOptionsEmail? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionEmailOptions
            {
                Name = source.Name,
                Email = source.Email is { } e ? new V1ConnectionEmailMessage
                {
                    From = e.From,
                    Subject = e.Subject,
                    Body = e.Body,
                    Syntax = e.Syntax?.Value,
                } : null,
                Totp = source.Totp is { } t ? new V1ConnectionEmailTotp
                {
                    Length = t.Length,
                    TimeStep = t.TimeStep,
                } : null,
                BruteForceProtection = source.BruteForceProtection,
                DisableSignup = source.DisableSignup,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V1ConnectionEvernoteOptions? FromApi(ConnectionOptionsEvernote? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionEvernoteOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionExactOptions? FromApi(ConnectionOptionsExact? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionExactOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionFacebookOptions? FromApi(ConnectionOptionsFacebook? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionFacebookOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Email = source.Email,
                AdsManagement = source.AdsManagement,
                AdsRead = source.AdsRead,
                BusinessManagement = source.BusinessManagement,
                GroupsAccessMemberInfo = source.GroupsAccessMemberInfo,
                LeadsRetrieval = source.LeadsRetrieval,
                ManagePages = source.ManagePages,
                PagesMessaging = source.PagesMessaging,
                PagesMessagingPhoneNumber = source.PagesMessagingPhoneNumber,
                PagesMessagingSubscriptions = source.PagesMessagingSubscriptions,
                PagesShowList = source.PagesShowList,
                PublishToGroups = source.PublishToGroups,
                ReadAudienceNetworkInsights = source.ReadAudienceNetworkInsights,
                ReadInsights = source.ReadInsights,
                ReadPageMailboxes = source.ReadPageMailboxes,
                PublicProfile = source.PublicProfile,
                UserBirthday = source.UserBirthday,
                UserLikes = source.UserLikes,
                UserGender = source.UserGender,
                UserAgeRange = source.UserAgeRange,
                UserLocation = source.UserLocation,
                UserHometown = source.UserHometown,
                UserFriends = source.UserFriends,
                UserLink = source.UserLink,
                UserPhotos = source.UserPhotos,
                UserVideos = source.UserVideos,
                UserPosts = source.UserPosts,
                UserStatus = source.UserStatus,
                UserTaggedPlaces = source.UserTaggedPlaces,
                UserEvents = source.UserEvents,
                UserGroups = source.UserGroups,
                UserManagedGroups = source.UserManagedGroups,
                ManageNotifications = source.ManageNotifications,
                PublishActions = source.PublishActions,
                PublishPages = source.PublishPages,
                PublishVideo = source.PublishVideo,
                ReadMailbox = source.ReadMailbox,
                ReadStream = source.ReadStream,
                AllowContextProfileField = source.AllowContextProfileField,
                PagesManageCta = source.PagesManageCta,
                PagesManageInstantArticles = source.PagesManageInstantArticles,
            };
        }

        internal static V1ConnectionGitHubOptions? FromApi(ConnectionOptionsGitHub? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionGitHubOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                AdminOrg = source.AdminOrg,
                AdminPublicKey = source.AdminPublicKey,
                AdminRepoHook = source.AdminRepoHook,
                DeleteRepo = source.DeleteRepo,
                Email = source.Email,
                Follow = source.Follow,
                Gist = source.Gist,
                Notifications = source.Notifications,
                PublicRepo = source.PublicRepo,
                ReadOrg = source.ReadOrg,
                ReadPublicKey = source.ReadPublicKey,
                ReadRepoHook = source.ReadRepoHook,
                ReadUser = source.ReadUser,
                Repo = source.Repo,
                RepoDeployment = source.RepoDeployment,
                RepoStatus = source.RepoStatus,
                WriteOrg = source.WriteOrg,
                WritePublicKey = source.WritePublicKey,
                WriteRepoHook = source.WriteRepoHook,
                Profile = source.Profile,
            };
        }

        internal static V1ConnectionGoogleAppsOptions? FromApi(ConnectionOptionsGoogleApps? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionGoogleAppsOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Domain = source.Domain,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                IconUrl = source.IconUrl,
                Email = source.Email,
                Profile = source.Profile,
                ApiEnableUsers = source.ApiEnableUsers,
                MapUserIdToId = source.MapUserIdToId,
                AdminAccessToken = source.AdminAccessToken,
                AdminAccessTokenExpiresin = source.AdminAccessTokenExpiresin,
                AdminRefreshToken = source.AdminRefreshToken,
                AllowSettingLoginScopes = source.AllowSettingLoginScopes,
                ApiEnableGroups = source.ApiEnableGroups,
                ExtAgreedTerms = source.ExtAgreedTerms,
                ExtGroups = source.ExtGroups,
                ExtGroupsExtended = source.ExtGroupsExtended,
                ExtIsAdmin = source.ExtIsAdmin,
                ExtIsSuspended = source.ExtIsSuspended,
                FederatedConnectionsAccessTokens = source.FederatedConnectionsAccessTokens.IsDefined && source.FederatedConnectionsAccessTokens.Value is { } fcat
                    ? new V1ConnectionGoogleAppsFederatedConnectionsAccessTokens { Active = fcat.Active }
                    : null,
                HandleLoginFromSocial = source.HandleLoginFromSocial,
            };
        }

        internal static V1ConnectionGoogleOAuth2Options? FromApi(ConnectionOptionsGoogleOAuth2? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionGoogleOAuth2Options
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                IconUrl = source.IconUrl,
                AllowedAudiences = source.AllowedAudiences?.ToArray(),
                Email = source.Email,
                Profile = source.Profile,
                OfflineAccess = source.OfflineAccess,
                AdsenseManagement = source.AdsenseManagement,
                Analytics = source.Analytics,
                Blogger = source.Blogger,
                Calendar = source.Calendar,
                CalendarAddonsExecute = source.CalendarAddonsExecute,
                CalendarEvents = source.CalendarEvents,
                CalendarEventsReadonly = source.CalendarEventsReadonly,
                CalendarSettingsReadonly = source.CalendarSettingsReadonly,
                ChromeWebStore = source.ChromeWebStore,
                Contacts = source.Contacts,
                ContactsNew = source.ContactsNew,
                ContactsOtherReadonly = source.ContactsOtherReadonly,
                ContactsReadonly = source.ContactsReadonly,
                ContentApiForShopping = source.ContentApiForShopping,
                Coordinate = source.Coordinate,
                CoordinateReadonly = source.CoordinateReadonly,
                DirectoryReadonly = source.DirectoryReadonly,
                DocumentList = source.DocumentList,
                Drive = source.Drive,
                DriveActivity = source.DriveActivity,
                DriveActivityReadonly = source.DriveActivityReadonly,
                DriveAppdata = source.DriveAppdata,
                DriveAppsReadonly = source.DriveAppsReadonly,
                DriveFile = source.DriveFile,
                DriveMetadata = source.DriveMetadata,
                DriveMetadataReadonly = source.DriveMetadataReadonly,
                DrivePhotosReadonly = source.DrivePhotosReadonly,
                DriveReadonly = source.DriveReadonly,
                DriveScripts = source.DriveScripts,
                Gmail = source.Gmail,
                GmailCompose = source.GmailCompose,
                GmailInsert = source.GmailInsert,
                GmailLabels = source.GmailLabels,
                GmailMetadata = source.GmailMetadata,
                GmailModify = source.GmailModify,
                GmailNew = source.GmailNew,
                GmailReadonly = source.GmailReadonly,
                GmailSend = source.GmailSend,
                GmailSettingsBasic = source.GmailSettingsBasic,
                GmailSettingsSharing = source.GmailSettingsSharing,
                GoogleAffiliateNetwork = source.GoogleAffiliateNetwork,
                GoogleBooks = source.GoogleBooks,
                GoogleCloudStorage = source.GoogleCloudStorage,
                GoogleDrive = source.GoogleDrive,
                GoogleDriveFiles = source.GoogleDriveFiles,
                GooglePlus = source.GooglePlus,
                LatitudeBest = source.LatitudeBest,
                LatitudeCity = source.LatitudeCity,
                Moderator = source.Moderator,
                Orkut = source.Orkut,
                PicasaWeb = source.PicasaWeb,
                Sites = source.Sites,
                Tasks = source.Tasks,
                TasksReadonly = source.TasksReadonly,
                UrlShortener = source.UrlShortener,
                WebmasterTools = source.WebmasterTools,
                Youtube = source.Youtube,
                YoutubeChannelmembershipsCreator = source.YoutubeChannelmembershipsCreator,
                YoutubeNew = source.YoutubeNew,
                YoutubeReadonly = source.YoutubeReadonly,
                YoutubeUpload = source.YoutubeUpload,
                Youtubepartner = source.Youtubepartner,
            };
        }

        internal static V1ConnectionLinkedinOptions? FromApi(ConnectionOptionsLinkedin? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionLinkedinOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                BasicProfile = source.BasicProfile,
                EmailAddress = source.Email,
                Openid = source.Openid,
                FullProfile = source.FullProfile,
                StrategyVersion = source.StrategyVersion,
                Network = source.Network,
                Profile = source.Profile,
            };
        }

        internal static V1ConnectionOAuth1Options? FromApi(ConnectionOptionsOAuth1? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionOAuth1Options
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                AccessTokenUrl = source.AccessTokenUrl,
                RequestTokenUrl = source.RequestTokenUrl,
                SignatureMethod = source.SignatureMethod?.ToString(),
                UserAuthorizationUrl = source.UserAuthorizationUrl,
                Scripts = source.Scripts is { } sc ? new V1ConnectionOptionsScripts { FetchUserProfile = sc.FetchUserProfile } : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionOAuth2Options? FromApi(ConnectionOptionsOAuth2? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionOAuth2Options
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                AuthorizationUrl = source.AuthorizationUrl,
                TokenUrl = source.TokenUrl,
                LogoutUrl = source.LogoutUrl,
                Scope = source.Scope?.ToString(),
                IconUrl = source.IconUrl,
                PkceEnabled = source.PkceEnabled,
                UseOauthSpecScope = source.UseOauthSpecScope,
                Scripts = source.Scripts is { } sc ? new V1ConnectionOptionsScripts { FetchUserProfile = sc.FetchUserProfile } : null,
                AuthParams = source.AuthParams?.ToDictionary(kv => kv.Key, kv => kv.Value),
                AuthParamsMap = source.AuthParamsMap?.ToDictionary(kv => kv.Key, kv => kv.Value),
                FieldsMap = source.FieldsMap?.ToDictionary(kv => kv.Key, kv => kv.Value),
                CustomHeaders = source.CustomHeaders?.ToDictionary(kv => kv.Key, kv => kv.Value),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionOffice365Options? FromApi(ConnectionOptionsOffice365? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionOffice365Options
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
            };
        }

        internal static V1ConnectionOidcOptions? FromApi(ConnectionOptionsOidc? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionOidcOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                DiscoveryUrl = source.DiscoveryUrl,
                AuthorizationEndpoint = source.AuthorizationEndpoint,
                TokenEndpoint = source.TokenEndpoint,
                UserinfoEndpoint = source.UserinfoEndpoint,
                JwksUri = source.JwksUri,
                Issuer = source.Issuer,
                Scope = source.Scope,
                IconUrl = source.IconUrl,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                TokenEndpointAuthMethod = source.TokenEndpointAuthMethod.IsDefined ? source.TokenEndpointAuthMethod.Value?.ToString() : null,
                TokenEndpointAuthSigningAlg = source.TokenEndpointAuthSigningAlg.IsDefined ? source.TokenEndpointAuthSigningAlg.Value?.ToString() : null,
                TokenEndpointJwtcaAudFormat = source.TokenEndpointJwtcaAudFormat?.ToString(),
                DpopSigningAlg = source.DpopSigningAlg?.ToString(),
                IdTokenSignedResponseAlgs = source.IdTokenSignedResponseAlgs.IsDefined && source.IdTokenSignedResponseAlgs.Value is { } algs ? algs.Select(a => a.Value).ToArray() : null,
                SendBackChannelNonce = source.SendBackChannelNonce,
                Type = source.Type?.ToString(),
                OidcMetadata = source.OidcMetadata?.AdditionalProperties?.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString()),
                AttributeMap = source.AttributeMap is { } am ? new V1ConnectionOptionsAttributeMap { MappingMode = am.MappingMode?.ToString(), UserinfoScope = am.UserinfoScope, Attributes = am.Attributes?.ToDictionary(kv => kv.Key, kv => (string?)kv.Value?.ToString()) } : null,
                ConnectionSettings = source.ConnectionSettings is { } cs ? new V1ConnectionOptionsConnectionSettings { Pkce = cs.Pkce?.ToString() } : null,
                FederatedConnectionsAccessTokens = source.FederatedConnectionsAccessTokens.IsDefined && source.FederatedConnectionsAccessTokens.Value is { } fcat ? new V1ConnectionOptionsFederatedConnectionsAccessTokens { Active = fcat.Active } : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionOktaOptions? FromApi(ConnectionOptionsOkta? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionOktaOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                AuthorizationEndpoint = source.AuthorizationEndpoint,
                TokenEndpoint = source.TokenEndpoint,
                UserinfoEndpoint = source.UserinfoEndpoint,
                JwksUri = source.JwksUri,
                Issuer = source.Issuer,
                Scope = source.Scope,
                IconUrl = source.IconUrl,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                TokenEndpointAuthMethod = source.TokenEndpointAuthMethod.IsDefined ? source.TokenEndpointAuthMethod.Value?.ToString() : null,
                TokenEndpointAuthSigningAlg = source.TokenEndpointAuthSigningAlg.IsDefined ? source.TokenEndpointAuthSigningAlg.Value?.ToString() : null,
                TokenEndpointJwtcaAudFormat = source.TokenEndpointJwtcaAudFormat?.ToString(),
                DpopSigningAlg = source.DpopSigningAlg?.ToString(),
                IdTokenSignedResponseAlgs = source.IdTokenSignedResponseAlgs.IsDefined && source.IdTokenSignedResponseAlgs.Value is { } algs ? algs.Select(a => a.Value).ToArray() : null,
                SendBackChannelNonce = source.SendBackChannelNonce,
                Type = source.Type?.ToString(),
                OidcMetadata = source.OidcMetadata?.AdditionalProperties?.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString()),
                AttributeMap = source.AttributeMap is { } am ? new V1ConnectionOptionsAttributeMap { MappingMode = am.MappingMode?.ToString(), UserinfoScope = am.UserinfoScope, Attributes = am.Attributes?.ToDictionary(kv => kv.Key, kv => (string?)kv.Value?.ToString()) } : null,
                ConnectionSettings = source.ConnectionSettings is { } cs ? new V1ConnectionOptionsConnectionSettings { Pkce = cs.Pkce?.ToString() } : null,
                FederatedConnectionsAccessTokens = source.FederatedConnectionsAccessTokens.IsDefined && source.FederatedConnectionsAccessTokens.Value is { } fcat ? new V1ConnectionOptionsFederatedConnectionsAccessTokens { Active = fcat.Active } : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Domain = source.Domain,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionPaypalOptions? FromApi(ConnectionOptionsPaypal? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionPaypalOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionPingFederateOptions? FromApi(ConnectionOptionsPingFederate? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionPingFederateOptions
            {
                PingFederateBaseUrl = source.PingFederateBaseUrl,
                SignInEndpoint = source.SignInEndpoint,
                EntityId = source.EntityId,
                Cert = source.Cert,
                SigningCert = source.SigningCert,
                Thumbprints = source.Thumbprints?.ToArray(),
                SignatureAlgorithm = FromApi(source.SignatureAlgorithm),
                DigestAlgorithm = FromApi(source.DigestAlgorithm),
                SignSamlRequest = source.SignSamlRequest,
                ProtocolBinding = FromApi(source.ProtocolBinding),
                Idpinitiated = source.Idpinitiated is { } idp ? new V1ConnectionOptionsIdpinitiated { ClientId = idp.ClientId, ClientProtocol = FromApi(idp.ClientProtocol), ClientAuthorizequery = idp.ClientAuthorizequery } : null,
                DecryptionKey = source.DecryptionKey is { } dk ? new V1ConnectionOptionsKeyPair { Key = dk.Value?.ToString() } : null,
                AssertionDecryptionSettings = source.AssertionDecryptionSettings is { } ads ? new V1ConnectionOptionsAssertionDecryptionSettings { DecryptionAlgorithm = FromApi(ads.AlgorithmProfile), KeyEncryptionAlgorithm = ads.AlgorithmExceptions is { } ae ? string.Join(",", ae) : null } : null,
                IconUrl = source.IconUrl,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionSalesforceOptions? FromApi(ConnectionOptionsSalesforce? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionSalesforceOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                Profile = source.Profile,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionSalesforceCommunityOptions? FromApi(ConnectionOptionsSalesforceCommunity? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionSalesforceCommunityOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                CommunityBaseUrl = source.CommunityBaseUrl,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                Profile = source.Profile,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionSamlOptions? FromApi(ConnectionOptionsSaml? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionSamlOptions
            {
                SignInEndpoint = source.SignInEndpoint,
                SignOutEndpoint = source.SignOutEndpoint,
                DisableSignout = source.DisableSignout,
                DestinationUrl = source.DestinationUrl,
                RecipientUrl = source.RecipientUrl,
                Cert = source.Cert,
                Thumbprints = source.Thumbprints?.ToArray(),
                MetadataUrl = source.MetadataUrl,
                MetadataXml = source.MetadataXml,
                EntityId = source.EntityId,
                SignatureAlgorithm = FromApi(source.SignatureAlgorithm),
                DigestAlgorithm = FromApi(source.DigestAlgorithm),
                SignSamlRequest = source.SignSamlRequest,
                ProtocolBinding = FromApi(source.ProtocolBinding),
                RequestTemplate = source.RequestTemplate,
                Debug = source.Debug,
                Deflate = source.Deflate,
                Idpinitiated = source.Idpinitiated is { } idp ? new V1ConnectionOptionsIdpinitiated { ClientId = idp.ClientId, ClientProtocol = FromApi(idp.ClientProtocol), ClientAuthorizequery = idp.ClientAuthorizequery } : null,
                SigningCert = source.SigningCert,
                SigningKey = source.SigningKey is { } sk ? new V1ConnectionOptionsKeyPair { Key = sk.Key, Cert = sk.Cert } : null,
                DecryptionKey = source.DecryptionKey is { } dk ? new V1ConnectionOptionsKeyPair { Key = dk.Value?.ToString() } : null,
                AssertionDecryptionSettings = source.AssertionDecryptionSettings is { } ads ? new V1ConnectionOptionsAssertionDecryptionSettings { DecryptionAlgorithm = FromApi(ads.AlgorithmProfile), KeyEncryptionAlgorithm = ads.AlgorithmExceptions is { } ae ? string.Join(",", ae) : null } : null,
                FieldsMap = source.FieldsMap?.ToDictionary(kv => kv.Key, kv => kv.Value?.Value?.ToString()),
                UserIdAttribute = source.UserIdAttribute,
                IconUrl = source.IconUrl,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                GlobalTokenRevocationJwtIss = source.GlobalTokenRevocationJwtIss,
                GlobalTokenRevocationJwtSub = source.GlobalTokenRevocationJwtSub,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V1ConnectionSmsOptions? FromApi(ConnectionOptionsSms? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionSmsOptions
            {
                Name = source.Name,
                From = source.From,
                Template = source.Template,
                Syntax = source.Syntax?.ToString(),
                Provider = source.Provider?.ToString(),
                TwilioSid = source.TwilioSid,
                TwilioToken = source.TwilioToken,
                MessagingServiceSid = source.MessagingServiceSid,
                GatewayUrl = source.GatewayUrl,
                ForwardReqInfo = source.ForwardReqInfo,
                DisableSignup = source.DisableSignup,
                BruteForceProtection = source.BruteForceProtection,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                Totp = source.Totp is { } t ? new V1ConnectionEmailTotp { Length = t.Length, TimeStep = t.TimeStep } : null,
                GatewayAuthentication = source.GatewayAuthentication.IsDefined && source.GatewayAuthentication.Value is { } ga ? new V1ConnectionGatewayAuthentication { Method = ga.Method, Subject = ga.Subject, Audience = ga.Audience, Secret = ga.Secret, SecretBase64Encoded = ga.SecretBase64Encoded } : null,
            };
        }

        internal static V1ConnectionTwitterOptions? FromApi(ConnectionOptionsTwitter? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionTwitterOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Protocol = source.Protocol?.ToString(),
                OfflineAccess = source.OfflineAccess,
                Profile = source.Profile,
                TweetRead = source.TweetRead,
                UsersRead = source.UsersRead,
            };
        }

        internal static V1ConnectionWindowsLiveOptions? FromApi(ConnectionOptionsWindowsLive? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionWindowsLiveOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? string.Join(" ", source.Scope) : null,
                FreeformScopes = source.FreeformScopes is not null ? source.FreeformScopes.Any() : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                BasicProfile = source.Basic,
                OfflineAccess = source.OfflineAccess,
                Signin = source.Signin,
                Birthday = source.Birthday,
                Calendars = source.Calendars,
                CalendarsUpdate = source.CalendarsUpdate,
                ContactsBirthday = source.ContactsBirthday,
                ContactsCreate = source.ContactsCreate,
                ContactsCalendar = source.ContactsCalendars,
                ContactsPhotos = source.ContactsPhotos,
                ContactsSkydrive = source.ContactsSkydrive,
                Emails = source.Emails,
                EventsCreate = source.EventsCreate,
                Messenger = source.Messenger,
                PhoneNumbers = source.PhoneNumbers,
                Photos = source.Photos,
                PostalAddresses = source.PostalAddresses,
                Share = source.Share,
                Skydrive = source.Skydrive,
                SkydriveUpdate = source.SkydriveUpdate,
                WorkProfile = source.WorkProfile,
                Applications = source.Applications,
                ApplicationsCreate = source.ApplicationsCreate,
                StrategyVersion = source.StrategyVersion,
                DirectoryAccessasuserAll = source.DirectoryAccessasuserAll,
                DirectoryReadAll = source.DirectoryReadAll,
                DirectoryReadwriteAll = source.DirectoryReadwriteAll,
                GraphCalendars = source.GraphCalendars,
                GraphCalendarsUpdate = source.GraphCalendarsUpdate,
                GraphContacts = source.GraphContacts,
                GraphContactsUpdate = source.GraphContactsUpdate,
                GraphDevice = source.GraphDevice,
                GraphDeviceCommand = source.GraphDeviceCommand,
                GraphEmails = source.GraphEmails,
                GraphEmailsUpdate = source.GraphEmailsUpdate,
                GraphFiles = source.GraphFiles,
                GraphFilesAll = source.GraphFilesAll,
                GraphFilesAllUpdate = source.GraphFilesAllUpdate,
                GraphFilesUpdate = source.GraphFilesUpdate,
                GraphNotes = source.GraphNotes,
                GraphNotesCreate = source.GraphNotesCreate,
                GraphNotesUpdate = source.GraphNotesUpdate,
                GraphTasks = source.GraphTasks,
                GraphTasksUpdate = source.GraphTasksUpdate,
                GraphUser = source.GraphUser,
                GraphUserActivity = source.GraphUserActivity,
                GraphUserUpdate = source.GraphUserUpdate,
                GroupReadAll = source.GroupReadAll,
                GroupReadwriteAll = source.GroupReadwriteAll,
                MailReadwriteAll = source.MailReadwriteAll,
                MailSend = source.MailSend,
                RolemanagementReadAll = source.RolemanagementReadAll,
                RolemanagementReadwriteDirectory = source.RolemanagementReadwriteDirectory,
                SitesReadAll = source.SitesReadAll,
                SitesReadwriteAll = source.SitesReadwriteAll,
                TeamReadbasicAll = source.TeamReadbasicAll,
                TeamReadwriteAll = source.TeamReadwriteAll,
                UserReadAll = source.UserReadAll,
                UserReadbasicAll = source.UserReadbasicAll,
            };
        }

        internal static V1ConnectionYahooOptions? FromApi(ConnectionOptionsYahoo? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionYahooOptions
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V1ConnectionSetUserRootAttributes FromApi(ConnectionSetUserRootAttributesEnum source)
        {
            return source.Value switch
            {
                ConnectionSetUserRootAttributesEnum.Values.OnEachLogin => V1ConnectionSetUserRootAttributes.OnEachLogin,
                ConnectionSetUserRootAttributesEnum.Values.OnFirstLogin => V1ConnectionSetUserRootAttributes.OnFirstLogin,
                ConnectionSetUserRootAttributesEnum.Values.NeverOnLogin => V1ConnectionSetUserRootAttributes.NeverOnLogin,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static Dictionary<string, V1ConnectionUpstreamParam?>? FromApi(Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?> source)
        {
            if (!source.IsDefined || source.Value is not { } dict)
                return null;

            var result = new Dictionary<string, V1ConnectionUpstreamParam?>(dict.Count);
            foreach (var (key, value) in dict)
            {
                string? alias = null;
                if (value is { } v && v.IsConnectionUpstreamAlias())
                    alias = v.AsConnectionUpstreamAlias().Alias?.Value;
                result[key] = alias is not null ? new V1ConnectionUpstreamParam { Alias = alias } : null;
            }
            return result;
        }

        internal static Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?> ToApiUpstreamParams(Dictionary<string, V1ConnectionUpstreamParam?>? source)
        {
            if (source is null)
                return default;

            var result = new Dictionary<string, ConnectionUpstreamAdditionalProperties?>(source.Count);
            foreach (var (key, value) in source)
                result[key] = value?.Alias is { } alias
                    ? ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamAlias(new ConnectionUpstreamAlias { Alias = new ConnectionUpstreamAliasEnum(alias) })
                    : null;
            return Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?>.Of(result);
        }

        internal static V1ConnectionOptionsValidation FromApi(ConnectionValidationOptions source)
        {
            return new V1ConnectionOptionsValidation
            {
                UserName = source.Username.IsDefined && source.Username.Value is { } u ? FromApi(u) : null,
            };
        }

        internal static V1ConnectionOptionsUserName FromApi(ConnectionUsernameValidationOptions source)
        {
            return new V1ConnectionOptionsUserName
            {
                Min = source.Min,
                Max = source.Max,
            };
        }

        internal static V1ConnectionOptionsAttributes FromApi(ConnectionAttributes source)
        {
            return new V1ConnectionOptionsAttributes
            {
                Email = source.Email is { } e ? FromApi(e) : null,
                PhoneNumber = source.PhoneNumber is { } p ? FromApi(p) : null,
                Username = source.Username is { } u ? FromApi(u) : null,
            };
        }

        internal static V1ConnectionOptionsEmailAttribute FromApi(EmailAttribute source)
        {
            return new V1ConnectionOptionsEmailAttribute
            {
                Identifier = source.Identifier is { } i ? FromApi(i) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } s ? FromApi(s) : null,
            };
        }

        internal static V1ConnectionOptionsEmailSignup FromApi(SignupVerified source)
        {
            return new V1ConnectionOptionsEmailSignup
            {
                Status = source.Status is { } st ? FromApi(st) : null,
                Verification = source.Verification is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsPhoneNumberAttribute FromApi(PhoneAttribute source)
        {
            return new V1ConnectionOptionsPhoneNumberAttribute
            {
                Signup = source.Signup is { } s ? FromApi(s, true) : null,
            };
        }

        internal static V1ConnectionOptionsPhoneNumberSignup FromApi(SignupVerified source, bool phone)
        {
            return new V1ConnectionOptionsPhoneNumberSignup
            {
                Status = source.Status is { } st ? FromApi(st) : null,
                Verification = source.Verification is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsUsernameAttribute FromApi(UsernameAttribute source)
        {
            return new V1ConnectionOptionsUsernameAttribute
            {
                Identifier = source.Identifier is { } i ? FromApi(i) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } s ? FromApi(s) : null,
                Validation = source.Validation is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsUsernameSignup FromApi(SignupSchema source)
        {
            return new V1ConnectionOptionsUsernameSignup
            {
                Status = source.Status is { } st ? FromApi(st) : null,
            };
        }

        internal static V1ConnectionOptionsAttributeIdentifier FromApi(ConnectionAttributeIdentifier source)
        {
            return new V1ConnectionOptionsAttributeIdentifier
            {
                Active = source.Active,
            };
        }

        internal static V1ConnectionOptionsAttributeValidation FromApi(UsernameValidation source)
        {
            return new V1ConnectionOptionsAttributeValidation
            {
                MinLength = (int?)source.MinLength,
                MaxLength = (int?)source.MaxLength,
                AllowedTypes = source.AllowedTypes is { } at ? FromApi(at) : null,
            };
        }

        internal static V1ConnectionOptionsAttributeAllowedTypes FromApi(UsernameAllowedTypes source)
        {
            return new V1ConnectionOptionsAttributeAllowedTypes
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
            };
        }

        internal static V1ConnectionOptionsVerification FromApi(SignupVerification source)
        {
            return new V1ConnectionOptionsVerification
            {
                Active = source.Active,
            };
        }

        internal static V1ConnectionOptionsMfa FromApi(ConnectionMfa source)
        {
            return new V1ConnectionOptionsMfa
            {
                Active = source.Active,
                ReturnEnrollSettings = source.ReturnEnrollSettings,
            };
        }

        internal static V1ConnectionOptionsCustomScripts FromApi(ConnectionCustomScripts source)
        {
            return new V1ConnectionOptionsCustomScripts
            {
                Login = source.Login,
                GetUser = source.GetUser,
                Delete = source.Delete,
                ChangePassword = source.ChangePassword,
                Verify = source.Verify,
                Create = source.Create,
                ChangeUsername = source.ChangeUsername,
                ChangeEmail = source.ChangeEmail,
                ChangePhoneNumber = source.ChangePhoneNumber,
            };
        }

        internal static V1ConnectionOptionsAuthenticationMethods FromApi(ConnectionAuthenticationMethods source)
        {
            return new V1ConnectionOptionsAuthenticationMethods
            {
                Password = source.Password is { } p ? FromApi(p) : null,
                Passkey = source.Passkey is { } pk ? FromApi(pk) : null,
            };
        }

        internal static V1ConnectionOptionsPasswordAuthenticationMethod FromApi(ConnectionPasswordAuthenticationMethod source)
        {
            return new V1ConnectionOptionsPasswordAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V1ConnectionOptionsPasskeyAuthenticationMethod FromApi(ConnectionPasskeyAuthenticationMethod source)
        {
            return new V1ConnectionOptionsPasskeyAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V1ConnectionOptionsPasskeyOptions FromApi(ConnectionPasskeyOptions source)
        {
            return new V1ConnectionOptionsPasskeyOptions
            {
                ChallengeUi = source.ChallengeUi is { } cui ? FromApi(cui) : null,
                ProgressiveEnrollmentEnabled = source.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = source.LocalEnrollmentEnabled,
            };
        }

        internal static V1ConnectionOptionsPasswordComplexityOptions FromApi(ConnectionPasswordComplexityOptions source)
        {
            return new V1ConnectionOptionsPasswordComplexityOptions
            {
                MinLength = source.MinLength,
            };
        }

        internal static V1ConnectionOptionsPasswordHistory FromApi(ConnectionPasswordHistoryOptions source)
        {
            return new V1ConnectionOptionsPasswordHistory
            {
                Enable = source.Enable,
                Size = source.Size,
            };
        }

        internal static V1ConnectionOptionsPasswordNoPersonalInfo FromApi(ConnectionPasswordNoPersonalInfoOptions source)
        {
            return new V1ConnectionOptionsPasswordNoPersonalInfo
            {
                Enable = source.Enable,
            };
        }

        internal static V1ConnectionOptionsPasswordDictionary FromApi(ConnectionPasswordDictionaryOptions source)
        {
            return new V1ConnectionOptionsPasswordDictionary
            {
                Enable = source.Enable,
                Dictionary = source.Dictionary?.ToArray(),
            };
        }

        internal static V1ConnectionGatewayAuthentication FromApi(ConnectionGatewayAuthentication source)
        {
            return new V1ConnectionGatewayAuthentication
            {
                Method = source.Method,
                Subject = source.Subject,
                Audience = source.Audience,
                Secret = source.Secret,
                SecretBase64Encoded = source.SecretBase64Encoded,
            };
        }

        internal static V1ConnectionOptionsPrecedence FromApi(ConnectionIdentifierPrecedenceEnum source)
        {
            return source.Value switch
            {
                ConnectionIdentifierPrecedenceEnum.Values.Email => V1ConnectionOptionsPrecedence.Email,
                ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber => V1ConnectionOptionsPrecedence.PhoneNumber,
                ConnectionIdentifierPrecedenceEnum.Values.Username => V1ConnectionOptionsPrecedence.UserName,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionIdentifierPrecedenceEnum ToApi(V1ConnectionOptionsPrecedence source)
        {
            return source switch
            {
                V1ConnectionOptionsPrecedence.Email => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Email),
                V1ConnectionOptionsPrecedence.PhoneNumber => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber),
                V1ConnectionOptionsPrecedence.UserName => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Username),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionOptionsAttributeStatus? FromApi(SignupStatusEnum? source)
        {
            return source?.Value switch
            {
                SignupStatusEnum.Values.Required => V1ConnectionOptionsAttributeStatus.Required,
                SignupStatusEnum.Values.Optional => V1ConnectionOptionsAttributeStatus.Optional,
                SignupStatusEnum.Values.Inactive => V1ConnectionOptionsAttributeStatus.Inactive,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static SignupStatusEnum ToApi(V1ConnectionOptionsAttributeStatus source)
        {
            return source switch
            {
                V1ConnectionOptionsAttributeStatus.Required => new SignupStatusEnum(SignupStatusEnum.Values.Required),
                V1ConnectionOptionsAttributeStatus.Optional => new SignupStatusEnum(SignupStatusEnum.Values.Optional),
                V1ConnectionOptionsAttributeStatus.Inactive => new SignupStatusEnum(SignupStatusEnum.Values.Inactive),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionOptionsPasswordPolicy? FromApi(ConnectionPasswordPolicyEnum? source)
        {
            return source?.Value switch
            {
                ConnectionPasswordPolicyEnum.Values.None => V1ConnectionOptionsPasswordPolicy.None,
                ConnectionPasswordPolicyEnum.Values.Low => V1ConnectionOptionsPasswordPolicy.Low,
                ConnectionPasswordPolicyEnum.Values.Fair => V1ConnectionOptionsPasswordPolicy.Fair,
                ConnectionPasswordPolicyEnum.Values.Good => V1ConnectionOptionsPasswordPolicy.Good,
                ConnectionPasswordPolicyEnum.Values.Excellent => V1ConnectionOptionsPasswordPolicy.Excellent,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionPasswordPolicyEnum ToApi(V1ConnectionOptionsPasswordPolicy source)
        {
            return source switch
            {
                V1ConnectionOptionsPasswordPolicy.None => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.None),
                V1ConnectionOptionsPasswordPolicy.Low => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Low),
                V1ConnectionOptionsPasswordPolicy.Fair => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Fair),
                V1ConnectionOptionsPasswordPolicy.Good => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Good),
                V1ConnectionOptionsPasswordPolicy.Excellent => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Excellent),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionSamlSignatureAlgorithm? FromApi(ConnectionSignatureAlgorithmEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha1 => V1ConnectionSamlSignatureAlgorithm.RsaSha1,
                ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha256 => V1ConnectionSamlSignatureAlgorithm.RsaSha256,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionSignatureAlgorithmEnumSaml ToApiSamlSignatureAlgorithm(V1ConnectionSamlSignatureAlgorithm source)
        {
            return source switch
            {
                V1ConnectionSamlSignatureAlgorithm.RsaSha1 => new ConnectionSignatureAlgorithmEnumSaml(ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha1),
                V1ConnectionSamlSignatureAlgorithm.RsaSha256 => new ConnectionSignatureAlgorithmEnumSaml(ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha256),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionSamlDigestAlgorithm? FromApi(ConnectionDigestAlgorithmEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionDigestAlgorithmEnumSaml.Values.Sha1 => V1ConnectionSamlDigestAlgorithm.Sha1,
                ConnectionDigestAlgorithmEnumSaml.Values.Sha256 => V1ConnectionSamlDigestAlgorithm.Sha256,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionDigestAlgorithmEnumSaml ToApiSamlDigestAlgorithm(V1ConnectionSamlDigestAlgorithm source)
        {
            return source switch
            {
                V1ConnectionSamlDigestAlgorithm.Sha1 => new ConnectionDigestAlgorithmEnumSaml(ConnectionDigestAlgorithmEnumSaml.Values.Sha1),
                V1ConnectionSamlDigestAlgorithm.Sha256 => new ConnectionDigestAlgorithmEnumSaml(ConnectionDigestAlgorithmEnumSaml.Values.Sha256),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionSamlProtocolBinding? FromApi(ConnectionProtocolBindingEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpPost => V1ConnectionSamlProtocolBinding.HttpPost,
                ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpRedirect => V1ConnectionSamlProtocolBinding.HttpRedirect,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionProtocolBindingEnumSaml ToApiSamlProtocolBinding(V1ConnectionSamlProtocolBinding source)
        {
            return source switch
            {
                V1ConnectionSamlProtocolBinding.HttpPost => new ConnectionProtocolBindingEnumSaml(ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpPost),
                V1ConnectionSamlProtocolBinding.HttpRedirect => new ConnectionProtocolBindingEnumSaml(ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpRedirect),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionIdpInitiatedClientProtocol? FromApi(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Oidc => V1ConnectionIdpInitiatedClientProtocol.Oidc,
                ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Samlp => V1ConnectionIdpInitiatedClientProtocol.Samlp,
                ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Wsfed => V1ConnectionIdpInitiatedClientProtocol.WsFed,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionOptionsIdpInitiatedClientProtocolEnumSaml ToApiIdpInitiatedClientProtocol(V1ConnectionIdpInitiatedClientProtocol source)
        {
            return source switch
            {
                V1ConnectionIdpInitiatedClientProtocol.Oidc => new ConnectionOptionsIdpInitiatedClientProtocolEnumSaml(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Oidc),
                V1ConnectionIdpInitiatedClientProtocol.Samlp => new ConnectionOptionsIdpInitiatedClientProtocolEnumSaml(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Samlp),
                V1ConnectionIdpInitiatedClientProtocol.WsFed => new ConnectionOptionsIdpInitiatedClientProtocolEnumSaml(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Wsfed),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionAssertionDecryptionAlgorithmProfile? FromApi(ConnectionAssertionDecryptionAlgorithmProfileEnum source)
        {
            return source.Value switch
            {
                ConnectionAssertionDecryptionAlgorithmProfileEnum.Values.V20261 => V1ConnectionAssertionDecryptionAlgorithmProfile.V20261,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionAssertionDecryptionAlgorithmProfileEnum ToApiAssertionDecryptionAlgorithmProfile(V1ConnectionAssertionDecryptionAlgorithmProfile source)
        {
            return source switch
            {
                V1ConnectionAssertionDecryptionAlgorithmProfile.V20261 => new ConnectionAssertionDecryptionAlgorithmProfileEnum(ConnectionAssertionDecryptionAlgorithmProfileEnum.Values.V20261),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V1ConnectionChallengeUi FromApi(ConnectionPasskeyChallengeUiEnum source)
        {
            return source.Value switch
            {
                ConnectionPasskeyChallengeUiEnum.Values.Both => V1ConnectionChallengeUi.Both,
                ConnectionPasskeyChallengeUiEnum.Values.Autofill => V1ConnectionChallengeUi.AutoFill,
                ConnectionPasskeyChallengeUiEnum.Values.Button => V1ConnectionChallengeUi.Button,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionPasskeyChallengeUiEnum ToApi(V1ConnectionChallengeUi source)
        {
            return source switch
            {
                V1ConnectionChallengeUi.Both => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Both),
                V1ConnectionChallengeUi.AutoFill => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Autofill),
                V1ConnectionChallengeUi.Button => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Button),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionSetUserRootAttributesEnum? ToApi(V1ConnectionSetUserRootAttributes? source)
        {
            return source switch
            {
                V1ConnectionSetUserRootAttributes.OnEachLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.OnEachLogin),
                V1ConnectionSetUserRootAttributes.OnFirstLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.OnFirstLogin),
                V1ConnectionSetUserRootAttributes.NeverOnLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.NeverOnLogin),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionOptionsAuth0 ToApi(V1ConnectionAuth0Options source)
        {
            var target = new ConnectionOptionsAuth0();
            target.BruteForceProtection = source.BruteForceProtection;
            target.DisableSignup = source.DisableSignup;
            target.EnableScriptContext = source.EnableScriptContext;
            target.EnabledDatabaseCustomization = source.EnabledDatabaseCustomization;
            target.ImportMode = source.ImportMode;
            target.RequiresUsername = source.RequiresUsername;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (source.PasswordPolicy is { } pp) target.PasswordPolicy = Optional<ConnectionPasswordPolicyEnum?>.Of(new ConnectionPasswordPolicyEnum(pp));
            if (source.PasswordHistory is { } ph) target.PasswordHistory = Optional<ConnectionPasswordHistoryOptions?>.Of(new ConnectionPasswordHistoryOptions { Enable = ph.Enable ?? false, Size = ph.Size });
            if (source.PasswordNoPersonalInfo is { } pnpi) target.PasswordNoPersonalInfo = Optional<ConnectionPasswordNoPersonalInfoOptions?>.Of(new ConnectionPasswordNoPersonalInfoOptions { Enable = pnpi.Enable ?? false });
            if (source.PasswordDictionary is { } pd) target.PasswordDictionary = Optional<ConnectionPasswordDictionaryOptions?>.Of(new ConnectionPasswordDictionaryOptions { Enable = pd.Enable ?? false, Dictionary = pd.Dictionary });
            if (source.PasswordComplexityOptions is { } pco) target.PasswordComplexityOptions = Optional<ConnectionPasswordComplexityOptions?>.Of(new ConnectionPasswordComplexityOptions { MinLength = pco.MinLength });
            if (source.Validation is { } val) { var v = new ConnectionValidationOptions(); if (val.UserName is { } un) v.Username = Optional<ConnectionUsernameValidationOptions?>.Of(new ConnectionUsernameValidationOptions { Min = un.Min ?? 0, Max = un.Max ?? 0 }); target.Validation = Optional<ConnectionValidationOptions?>.Of(v); }
            if (source.CustomScripts is { } cs) { target.CustomScripts ??= new ConnectionCustomScripts(); ApplyToApi(cs, target.CustomScripts); }
            if (source.Mfa is { } mfa) target.Mfa = new ConnectionMfa { Active = mfa.Active, ReturnEnrollSettings = mfa.ReturnEnrollSettings };
            return target;
        }

        internal static ConnectionOptionsAd ToApi(V1ConnectionAdOptions source)
        {
            var target = new ConnectionOptionsAd();
            target.AgentIp = source.AgentIp;
            target.AgentVersion = source.AgentVersion;
            target.BruteForceProtection = source.BruteForceProtection;
            target.CertAuth = source.CertAuth;
            if (source.Certs is { } certs) target.Certs = certs;
            target.DisableCache = source.DisableCache;
            target.DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.IconUrl = source.IconUrl;
            if (source.Ips is { } ips) target.Ips = ips;
            target.SignInEndpoint = source.SignInEndpoint;
            target.TenantDomain = source.TenantDomain;
            if (source.Thumbprints is { } tp) target.Thumbprints = tp;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.Kerberos is { } kb) target.Kerberos = kb.Enabled;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsAdfs ToApi(V1ConnectionAdfsOptions source)
        {
            var target = new ConnectionOptionsAdfs();
            target.AdfsServer = source.AdfsServer;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.EntityId = source.EntityId;
            target.FedMetadataXml = source.FedMetadataXml;
            target.IconUrl = source.IconUrl;
            if (source.PrevThumbprints is { } pt) target.PrevThumbprints = pt;
            target.SignInEndpoint = source.SignInEndpoint;
            target.TenantDomain = source.TenantDomain;
            if (source.Thumbprints is { } tp) target.Thumbprints = tp;
            target.UserIdAttribute = source.UserIdAttribute;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsAuth0Oidc ToApi(V1ConnectionAuth0OidcOptions source)
        {
            var target = new ConnectionOptionsAuth0Oidc();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            return target;
        }

        internal static ConnectionOptionsAzureAd ToApi(V1ConnectionAzureAdOptions source)
        {
            var target = new ConnectionOptionsAzureAd { ClientId = source.ClientId, ClientSecret = source.ClientSecret };
            target.ApiEnableUsers = source.ApiEnableUsers;
            target.AppDomain = source.AppDomain;
            target.AppId = source.AppId;
            target.BasicProfile = source.BasicProfile;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.ExtAccessToken = source.ExtAccessToken;
            target.ExtAccountEnabled = source.ExtAccountEnabled;
            target.ExtAdmin = source.ExtAdmin;
            target.ExtAgreedTerms = source.ExtAgreedTerms;
            target.ExtAssignedLicenses = source.ExtAssignedLicenses;
            target.ExtAssignedPlans = source.ExtAssignedPlans;
            target.ExtAzureId = source.ExtAzureId;
            target.ExtCity = source.ExtCity;
            target.ExtCountry = source.ExtCountry;
            target.ExtDepartment = source.ExtDepartment;
            target.ExtDirSyncEnabled = source.ExtDirSyncEnabled;
            target.ExtEmail = source.ExtEmail;
            target.ExtExpiresIn = source.ExtExpiresIn;
            target.ExtFamilyName = source.ExtFamilyName;
            target.ExtFax = source.ExtFax;
            target.ExtGivenName = source.ExtGivenName;
            target.ExtGroupIds = source.ExtGroupIds;
            target.ExtGroups = source.ExtGroups;
            target.ExtIsSuspended = source.ExtIsSuspended;
            target.ExtJobTitle = source.ExtJobTitle;
            target.ExtLastSync = source.ExtLastSync;
            target.ExtMobile = source.ExtMobile;
            target.ExtName = source.ExtName;
            target.ExtNestedGroups = source.ExtNestedGroups;
            target.ExtNickname = source.ExtNickname;
            target.ExtOid = source.ExtOid;
            target.ExtPhone = source.ExtPhone;
            target.ExtPhysicalDeliveryOfficeName = source.ExtPhysicalDeliveryOfficeName;
            target.ExtPostalCode = source.ExtPostalCode;
            target.ExtPreferredLanguage = source.ExtPreferredLanguage;
            target.ExtProfile = source.ExtProfile;
            target.ExtProvisionedPlans = source.ExtProvisionedPlans;
            target.ExtProvisioningErrors = source.ExtProvisioningErrors;
            target.ExtProxyAddresses = source.ExtProxyAddresses;
            target.ExtPuid = source.ExtPuid;
            target.ExtRefreshToken = source.ExtRefreshToken;
            target.ExtRoles = source.ExtRoles;
            target.ExtState = source.ExtState;
            target.ExtStreet = source.ExtStreet;
            target.ExtTelephoneNumber = source.ExtTelephoneNumber;
            target.ExtTenantid = source.ExtTenantid;
            target.ExtUpn = source.ExtUpn;
            target.ExtUsageLocation = source.ExtUsageLocation;
            target.ExtUserId = source.ExtUserId;
            target.Granted = source.Granted;
            target.IconUrl = source.IconUrl;
            target.MaxGroupsToRetrieve = source.MaxGroupsToRetrieve;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            target.TenantDomain = source.TenantDomain;
            target.TenantId = source.TenantId;
            if (source.Thumbprints is { } tp) target.Thumbprints = tp;
            target.UseCommonEndpoint = source.UseCommonEndpoint;
            target.UseWsfed = source.UseWsfed;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsBitbucket ToApi(V1ConnectionBitbucketOptions source)
        {
            var target = new ConnectionOptionsBitbucket();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsBox ToApi(V1ConnectionBoxOptions source)
        {
            var target = new ConnectionOptionsBox();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsDropbox ToApi(V1ConnectionDropboxOptions source)
        {
            var target = new ConnectionOptionsDropbox();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsEmail ToApi(V1ConnectionEmailOptions source)
        {
            return new ConnectionOptionsEmail
            {
                Name = source.Name ?? string.Empty,
                BruteForceProtection = source.BruteForceProtection ?? false,
                Email = source.Email is { } e ? new ConnectionEmailEmail
                {
                    From = e.From,
                    Subject = e.Subject,
                    Body = e.Body,
                    Syntax = e.Syntax is { } s ? new ConnectionEmailEmailSyntax(s) : null,
                } : new ConnectionEmailEmail(),
                Totp = source.Totp is { } t ? new ConnectionTotpEmail
                {
                    Length = t.Length,
                    TimeStep = t.TimeStep,
                } : null,
                DisableSignup = source.DisableSignup,
                NonPersistentAttrs = source.NonPersistentAttrs,
            };
        }

        internal static ConnectionOptionsEvernote ToApi(V1ConnectionEvernoteOptions source)
        {
            var target = new ConnectionOptionsEvernote();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsExact ToApi(V1ConnectionExactOptions source)
        {
            var target = new ConnectionOptionsExact();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsFacebook ToApi(V1ConnectionFacebookOptions source)
        {
            var target = new ConnectionOptionsFacebook();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope;
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.AdsManagement = source.AdsManagement;
            target.AdsRead = source.AdsRead;
            target.AllowContextProfileField = source.AllowContextProfileField;
            target.BusinessManagement = source.BusinessManagement;
            target.Email = source.Email;
            target.GroupsAccessMemberInfo = source.GroupsAccessMemberInfo;
            target.LeadsRetrieval = source.LeadsRetrieval;
            target.ManageNotifications = source.ManageNotifications;
            target.ManagePages = source.ManagePages;
            target.PagesManageCta = source.PagesManageCta;
            target.PagesManageInstantArticles = source.PagesManageInstantArticles;
            target.PagesMessaging = source.PagesMessaging;
            target.PagesMessagingPhoneNumber = source.PagesMessagingPhoneNumber;
            target.PagesMessagingSubscriptions = source.PagesMessagingSubscriptions;
            target.PagesShowList = source.PagesShowList;
            target.PublicProfile = source.PublicProfile;
            target.PublishActions = source.PublishActions;
            target.PublishPages = source.PublishPages;
            target.PublishToGroups = source.PublishToGroups;
            target.PublishVideo = source.PublishVideo;
            target.ReadAudienceNetworkInsights = source.ReadAudienceNetworkInsights;
            target.ReadInsights = source.ReadInsights;
            target.ReadMailbox = source.ReadMailbox;
            target.ReadPageMailboxes = source.ReadPageMailboxes;
            target.ReadStream = source.ReadStream;
            target.UserAgeRange = source.UserAgeRange;
            target.UserBirthday = source.UserBirthday;
            target.UserEvents = source.UserEvents;
            target.UserFriends = source.UserFriends;
            target.UserGender = source.UserGender;
            target.UserGroups = source.UserGroups;
            target.UserHometown = source.UserHometown;
            target.UserLikes = source.UserLikes;
            target.UserLink = source.UserLink;
            target.UserLocation = source.UserLocation;
            target.UserManagedGroups = source.UserManagedGroups;
            target.UserPhotos = source.UserPhotos;
            target.UserPosts = source.UserPosts;
            target.UserStatus = source.UserStatus;
            target.UserTaggedPlaces = source.UserTaggedPlaces;
            target.UserVideos = source.UserVideos;
            return target;
        }

        internal static ConnectionOptionsGitHub ToApi(V1ConnectionGitHubOptions source)
        {
            var target = new ConnectionOptionsGitHub();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.AdminOrg = source.AdminOrg;
            target.AdminPublicKey = source.AdminPublicKey;
            target.AdminRepoHook = source.AdminRepoHook;
            target.DeleteRepo = source.DeleteRepo;
            target.Email = source.Email;
            target.Follow = source.Follow;
            target.Gist = source.Gist;
            target.Notifications = source.Notifications;
            target.Profile = source.Profile;
            target.PublicRepo = source.PublicRepo;
            target.ReadOrg = source.ReadOrg;
            target.ReadPublicKey = source.ReadPublicKey;
            target.ReadRepoHook = source.ReadRepoHook;
            target.ReadUser = source.ReadUser;
            target.Repo = source.Repo;
            target.RepoDeployment = source.RepoDeployment;
            target.RepoStatus = source.RepoStatus;
            target.WriteOrg = source.WriteOrg;
            target.WritePublicKey = source.WritePublicKey;
            target.WriteRepoHook = source.WriteRepoHook;
            return target;
        }

        internal static ConnectionOptionsGoogleApps ToApi(V1ConnectionGoogleAppsOptions source)
        {
            var target = new ConnectionOptionsGoogleApps { ClientId = source.ClientId, ClientSecret = source.ClientSecret };
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.Domain = source.Domain;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            target.IconUrl = source.IconUrl;
            target.Email = source.Email;
            target.Profile = source.Profile;
            target.ApiEnableUsers = source.ApiEnableUsers;
            target.MapUserIdToId = source.MapUserIdToId;
            target.AdminAccessToken = source.AdminAccessToken;
            target.AdminRefreshToken = source.AdminRefreshToken;
            target.AllowSettingLoginScopes = source.AllowSettingLoginScopes;
            target.ApiEnableGroups = source.ApiEnableGroups;
            target.ExtAgreedTerms = source.ExtAgreedTerms;
            target.ExtGroups = source.ExtGroups;
            target.ExtGroupsExtended = source.ExtGroupsExtended;
            target.ExtIsAdmin = source.ExtIsAdmin;
            target.ExtIsSuspended = source.ExtIsSuspended;
            target.HandleLoginFromSocial = source.HandleLoginFromSocial;
            if (source.FederatedConnectionsAccessTokens is { } fcat)
                target.FederatedConnectionsAccessTokens = Optional<ConnectionFederatedConnectionsAccessTokens?>.Of(
                    new ConnectionFederatedConnectionsAccessTokens { Active = fcat.Active });
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsGoogleOAuth2 ToApi(V1ConnectionGoogleOAuth2Options source)
        {
            var target = new ConnectionOptionsGoogleOAuth2();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.IconUrl = source.IconUrl;
            if (source.AllowedAudiences is { } aa) target.AllowedAudiences = aa;
            target.AdsenseManagement = source.AdsenseManagement;
            target.Analytics = source.Analytics;
            target.Blogger = source.Blogger;
            target.Calendar = source.Calendar;
            target.CalendarAddonsExecute = source.CalendarAddonsExecute;
            target.CalendarEvents = source.CalendarEvents;
            target.CalendarEventsReadonly = source.CalendarEventsReadonly;
            target.CalendarSettingsReadonly = source.CalendarSettingsReadonly;
            target.ChromeWebStore = source.ChromeWebStore;
            target.Contacts = source.Contacts;
            target.ContactsNew = source.ContactsNew;
            target.ContactsOtherReadonly = source.ContactsOtherReadonly;
            target.ContactsReadonly = source.ContactsReadonly;
            target.ContentApiForShopping = source.ContentApiForShopping;
            target.Coordinate = source.Coordinate;
            target.CoordinateReadonly = source.CoordinateReadonly;
            target.DirectoryReadonly = source.DirectoryReadonly;
            target.DocumentList = source.DocumentList;
            target.Drive = source.Drive;
            target.DriveActivity = source.DriveActivity;
            target.DriveActivityReadonly = source.DriveActivityReadonly;
            target.DriveAppdata = source.DriveAppdata;
            target.DriveAppsReadonly = source.DriveAppsReadonly;
            target.DriveFile = source.DriveFile;
            target.DriveMetadata = source.DriveMetadata;
            target.DriveMetadataReadonly = source.DriveMetadataReadonly;
            target.DrivePhotosReadonly = source.DrivePhotosReadonly;
            target.DriveReadonly = source.DriveReadonly;
            target.DriveScripts = source.DriveScripts;
            target.Email = source.Email;
            target.Gmail = source.Gmail;
            target.GmailCompose = source.GmailCompose;
            target.GmailInsert = source.GmailInsert;
            target.GmailLabels = source.GmailLabels;
            target.GmailMetadata = source.GmailMetadata;
            target.GmailModify = source.GmailModify;
            target.GmailNew = source.GmailNew;
            target.GmailReadonly = source.GmailReadonly;
            target.GmailSend = source.GmailSend;
            target.GmailSettingsBasic = source.GmailSettingsBasic;
            target.GmailSettingsSharing = source.GmailSettingsSharing;
            target.GoogleAffiliateNetwork = source.GoogleAffiliateNetwork;
            target.GoogleBooks = source.GoogleBooks;
            target.GoogleCloudStorage = source.GoogleCloudStorage;
            target.GoogleDrive = source.GoogleDrive;
            target.GoogleDriveFiles = source.GoogleDriveFiles;
            target.GooglePlus = source.GooglePlus;
            target.LatitudeBest = source.LatitudeBest;
            target.LatitudeCity = source.LatitudeCity;
            target.Moderator = source.Moderator;
            target.OfflineAccess = source.OfflineAccess;
            target.Orkut = source.Orkut;
            target.PicasaWeb = source.PicasaWeb;
            target.Profile = source.Profile;
            target.Sites = source.Sites;
            target.Tasks = source.Tasks;
            target.TasksReadonly = source.TasksReadonly;
            target.UrlShortener = source.UrlShortener;
            target.WebmasterTools = source.WebmasterTools;
            target.Youtube = source.Youtube;
            target.YoutubeChannelmembershipsCreator = source.YoutubeChannelmembershipsCreator;
            target.YoutubeNew = source.YoutubeNew;
            target.YoutubeReadonly = source.YoutubeReadonly;
            target.YoutubeUpload = source.YoutubeUpload;
            target.Youtubepartner = source.Youtubepartner;
            return target;
        }

        internal static ConnectionOptionsLinkedin ToApi(V1ConnectionLinkedinOptions source)
        {
            var target = new ConnectionOptionsLinkedin();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.BasicProfile = source.BasicProfile;
            target.Email = source.EmailAddress;
            target.FullProfile = source.FullProfile;
            target.Network = source.Network;
            target.Openid = source.Openid;
            target.Profile = source.Profile;
            target.StrategyVersion = source.StrategyVersion;
            return target;
        }

        internal static ConnectionOptionsOAuth1 ToApi(V1ConnectionOAuth1Options source)
        {
            var target = new ConnectionOptionsOAuth1();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            target.AccessTokenUrl = source.AccessTokenUrl;
            target.RequestTokenUrl = source.RequestTokenUrl;
            target.UserAuthorizationUrl = source.UserAuthorizationUrl;
            if (source.Scripts is { } sc) target.Scripts = new ConnectionScriptsOAuth1 { FetchUserProfile = sc.FetchUserProfile };
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsOAuth2 ToApi(V1ConnectionOAuth2Options source)
        {
            var target = new ConnectionOptionsOAuth2();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            target.AuthorizationUrl = source.AuthorizationUrl;
            target.TokenUrl = source.TokenUrl;
            target.LogoutUrl = source.LogoutUrl;
            target.IconUrl = source.IconUrl;
            target.PkceEnabled = source.PkceEnabled;
            target.UseOauthSpecScope = source.UseOauthSpecScope;
            if (source.Scripts is { } sc) target.Scripts = new ConnectionScriptsOAuth2 { FetchUserProfile = sc.FetchUserProfile };
            if (source.AuthParams is { } ap) target.AuthParams = ap.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.AuthParamsMap is { } apm) target.AuthParamsMap = apm.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.FieldsMap is { } fm) target.FieldsMap = fm.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.CustomHeaders is { } ch) target.CustomHeaders = ch.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsOffice365 ToApi(V1ConnectionOffice365Options source)
        {
            var target = new ConnectionOptionsOffice365();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            return target;
        }

        internal static ConnectionOptionsOidc ToApi(V1ConnectionOidcOptions source)
        {
            var target = new ConnectionOptionsOidc { ClientId = source.ClientId, ClientSecret = source.ClientSecret };
            target.DiscoveryUrl = source.DiscoveryUrl;
            target.AuthorizationEndpoint = source.AuthorizationEndpoint;
            target.TokenEndpoint = source.TokenEndpoint;
            target.UserinfoEndpoint = source.UserinfoEndpoint;
            target.JwksUri = source.JwksUri;
            target.Issuer = source.Issuer;
            target.Scope = source.Scope;
            target.IconUrl = source.IconUrl;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            target.SendBackChannelNonce = source.SendBackChannelNonce;
            if (source.IdTokenSignedResponseAlgs is { } algs) target.IdTokenSignedResponseAlgs = Optional<IEnumerable<ConnectionIdTokenSignedResponseAlgEnum>?>.Of(algs.Select(a => new ConnectionIdTokenSignedResponseAlgEnum(a)));
            if (source.AttributeMap is { } am) target.AttributeMap = new ConnectionAttributeMapOidc { MappingMode = am.MappingMode is { } mm ? new ConnectionMappingModeEnumOidc(mm) : null, UserinfoScope = am.UserinfoScope, Attributes = am.Attributes?.ToDictionary(kv => kv.Key, kv => (object?)kv.Value) };
            if (source.ConnectionSettings is { } cs) target.ConnectionSettings = new ConnectionConnectionSettings { Pkce = cs.Pkce is { } p ? new ConnectionConnectionSettingsPkceEnum(p) : null };
            if (source.FederatedConnectionsAccessTokens is { } fcat) target.FederatedConnectionsAccessTokens = Optional<ConnectionFederatedConnectionsAccessTokens?>.Of(new ConnectionFederatedConnectionsAccessTokens { Active = fcat.Active });
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsOkta ToApi(V1ConnectionOktaOptions source)
        {
            var target = new ConnectionOptionsOkta { ClientId = source.ClientId, ClientSecret = source.ClientSecret };
            target.Domain = source.Domain;
            target.AuthorizationEndpoint = source.AuthorizationEndpoint;
            target.TokenEndpoint = source.TokenEndpoint;
            target.UserinfoEndpoint = source.UserinfoEndpoint;
            target.JwksUri = source.JwksUri;
            target.Issuer = source.Issuer;
            target.Scope = source.Scope;
            target.IconUrl = source.IconUrl;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            target.SendBackChannelNonce = source.SendBackChannelNonce;
            if (source.IdTokenSignedResponseAlgs is { } algs) target.IdTokenSignedResponseAlgs = Optional<IEnumerable<ConnectionIdTokenSignedResponseAlgEnum>?>.Of(algs.Select(a => new ConnectionIdTokenSignedResponseAlgEnum(a)));
            if (source.AttributeMap is { } am) target.AttributeMap = new ConnectionAttributeMapOkta { MappingMode = am.MappingMode is { } mm ? new ConnectionMappingModeEnumOkta(mm) : null, UserinfoScope = am.UserinfoScope, Attributes = am.Attributes?.ToDictionary(kv => kv.Key, kv => (object?)kv.Value) };
            if (source.ConnectionSettings is { } cs) target.ConnectionSettings = new ConnectionConnectionSettings { Pkce = cs.Pkce is { } p ? new ConnectionConnectionSettingsPkceEnum(p) : null };
            if (source.FederatedConnectionsAccessTokens is { } fcat) target.FederatedConnectionsAccessTokens = Optional<ConnectionFederatedConnectionsAccessTokens?>.Of(new ConnectionFederatedConnectionsAccessTokens { Active = fcat.Active });
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsPaypal ToApi(V1ConnectionPaypalOptions source)
        {
            var target = new ConnectionOptionsPaypal();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsPingFederate ToApi(V1ConnectionPingFederateOptions source)
        {
            var target = new ConnectionOptionsPingFederate { PingFederateBaseUrl = source.PingFederateBaseUrl };
            target.SignInEndpoint = source.SignInEndpoint;
            target.EntityId = source.EntityId;
            target.Cert = source.Cert;
            target.SigningCert = source.SigningCert;
            if (source.Thumbprints is { } tp) target.Thumbprints = tp;
            if (source.SignatureAlgorithm is { } sigAlg) target.SignatureAlgorithm = ToApiSamlSignatureAlgorithm(sigAlg);
            if (source.DigestAlgorithm is { } digAlg) target.DigestAlgorithm = ToApiSamlDigestAlgorithm(digAlg);
            if (source.ProtocolBinding is { } pb) target.ProtocolBinding = ToApiSamlProtocolBinding(pb);
            target.SignSamlRequest = source.SignSamlRequest;
            if (source.Idpinitiated is { } idp) target.Idpinitiated = new ConnectionOptionsIdpinitiatedSaml { ClientId = idp.ClientId, ClientProtocol = idp.ClientProtocol is { } cp ? ToApiIdpInitiatedClientProtocol(cp) : null, ClientAuthorizequery = idp.ClientAuthorizequery };
            if (source.DecryptionKey is { Key: { } dkKey }) target.DecryptionKey = ConnectionDecryptionKeySaml.FromString(dkKey);
            if (source.AssertionDecryptionSettings is { DecryptionAlgorithm: { } adsAlg }) target.AssertionDecryptionSettings = new ConnectionAssertionDecryptionSettings { AlgorithmProfile = ToApiAssertionDecryptionAlgorithmProfile(adsAlg) };
            target.IconUrl = source.IconUrl;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsSalesforce ToApi(V1ConnectionSalesforceOptions source)
        {
            var target = new ConnectionOptionsSalesforce();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsSalesforceCommunity ToApi(V1ConnectionSalesforceCommunityOptions source)
        {
            var target = new ConnectionOptionsSalesforceCommunity();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            target.CommunityBaseUrl = source.CommunityBaseUrl;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        internal static ConnectionOptionsSaml ToApi(V1ConnectionSamlOptions source)
        {
            var target = new ConnectionOptionsSaml();
            target.SignInEndpoint = source.SignInEndpoint;
            target.SignOutEndpoint = source.SignOutEndpoint;
            target.DisableSignout = source.DisableSignout;
            target.DestinationUrl = source.DestinationUrl;
            target.RecipientUrl = source.RecipientUrl;
            target.Cert = source.Cert;
            if (source.Thumbprints is { } tp) target.Thumbprints = tp;
            target.MetadataUrl = source.MetadataUrl;
            target.MetadataXml = source.MetadataXml;
            target.EntityId = source.EntityId;
            target.SignSamlRequest = source.SignSamlRequest;
            if (source.SignatureAlgorithm is { } sigAlg) target.SignatureAlgorithm = ToApiSamlSignatureAlgorithm(sigAlg);
            if (source.DigestAlgorithm is { } digAlg) target.DigestAlgorithm = ToApiSamlDigestAlgorithm(digAlg);
            if (source.ProtocolBinding is { } pb) target.ProtocolBinding = ToApiSamlProtocolBinding(pb);
            target.RequestTemplate = source.RequestTemplate;
            target.Debug = source.Debug;
            target.Deflate = source.Deflate;
            if (source.Idpinitiated is { } idp) target.Idpinitiated = new ConnectionOptionsIdpinitiatedSaml { ClientId = idp.ClientId, ClientProtocol = idp.ClientProtocol is { } cp ? ToApiIdpInitiatedClientProtocol(cp) : null, ClientAuthorizequery = idp.ClientAuthorizequery };
            target.SigningCert = source.SigningCert;
            if (source.SigningKey is { } sk) target.SigningKey = new ConnectionSigningKeySaml { Key = sk.Key, Cert = sk.Cert };
            if (source.DecryptionKey is { Key: { } dkKey }) target.DecryptionKey = ConnectionDecryptionKeySaml.FromString(dkKey);
            if (source.AssertionDecryptionSettings is { DecryptionAlgorithm: { } adsAlg }) target.AssertionDecryptionSettings = new ConnectionAssertionDecryptionSettings { AlgorithmProfile = ToApiAssertionDecryptionAlgorithmProfile(adsAlg) };
            if (source.FieldsMap is { } fm) target.FieldsMap = fm.Where(kv => kv.Value is not null).ToDictionary(kv => kv.Key, kv => ConnectionFieldsMapSamlValue.FromString(kv.Value!));
            target.UserIdAttribute = source.UserIdAttribute;
            target.IconUrl = source.IconUrl;
            if (source.DomainAliases is { } da) target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            target.GlobalTokenRevocationJwtIss = source.GlobalTokenRevocationJwtIss;
            target.GlobalTokenRevocationJwtSub = source.GlobalTokenRevocationJwtSub;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            if (source.UpstreamParams is { } up) target.UpstreamParams = ToApiUpstreamParams(up);
            return target;
        }

        internal static ConnectionOptionsSms ToApi(V1ConnectionSmsOptions source)
        {
            var target = new ConnectionOptionsSms();
            target.Name = source.Name;
            target.From = source.From;
            target.Template = source.Template;
            target.TwilioSid = source.TwilioSid;
            target.TwilioToken = source.TwilioToken;
            target.MessagingServiceSid = source.MessagingServiceSid;
            target.GatewayUrl = source.GatewayUrl;
            target.ForwardReqInfo = source.ForwardReqInfo;
            target.DisableSignup = source.DisableSignup;
            target.BruteForceProtection = source.BruteForceProtection;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (source.Totp is { } t) target.Totp = new ConnectionTotpSms { Length = t.Length, TimeStep = t.TimeStep };
            if (source.GatewayAuthentication is { } ga)
            {
                var gatewayAuth = new ConnectionGatewayAuthenticationSms
                {
                    Method = ga.Method ?? string.Empty,
                    Audience = ga.Audience ?? string.Empty,
                    Secret = ga.Secret ?? string.Empty,
                };
                if (ga.Subject is { } subject) gatewayAuth.Subject = subject;
                if (ga.SecretBase64Encoded is { } sbe) gatewayAuth.SecretBase64Encoded = sbe;
                target.GatewayAuthentication = Optional<ConnectionGatewayAuthenticationSms?>.Of(gatewayAuth);
            }
            return target;
        }

        internal static ConnectionOptionsTwitter ToApi(V1ConnectionTwitterOptions source)
        {
            var target = new ConnectionOptionsTwitter();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.OfflineAccess = source.OfflineAccess;
            target.Profile = source.Profile;
            target.TweetRead = source.TweetRead;
            target.UsersRead = source.UsersRead;
            return target;
        }

        internal static ConnectionOptionsWindowsLive ToApi(V1ConnectionWindowsLiveOptions source)
        {
            var target = new ConnectionOptionsWindowsLive();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope) target.Scope = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (source.FreeformScopes is { } ffs) target.FreeformScopes = ffs ? (IEnumerable<string>)new[] { "true" } : Array.Empty<string>();
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            target.Basic = source.BasicProfile;
            target.OfflineAccess = source.OfflineAccess;
            target.Signin = source.Signin;
            target.Birthday = source.Birthday;
            target.Calendars = source.Calendars;
            target.CalendarsUpdate = source.CalendarsUpdate;
            target.ContactsBirthday = source.ContactsBirthday;
            target.ContactsCreate = source.ContactsCreate;
            target.ContactsCalendars = source.ContactsCalendar;
            target.ContactsPhotos = source.ContactsPhotos;
            target.ContactsSkydrive = source.ContactsSkydrive;
            target.Emails = source.Emails;
            target.EventsCreate = source.EventsCreate;
            target.Messenger = source.Messenger;
            target.PhoneNumbers = source.PhoneNumbers;
            target.Photos = source.Photos;
            target.PostalAddresses = source.PostalAddresses;
            target.Share = source.Share;
            target.Skydrive = source.Skydrive;
            target.SkydriveUpdate = source.SkydriveUpdate;
            target.WorkProfile = source.WorkProfile;
            target.Applications = source.Applications;
            target.ApplicationsCreate = source.ApplicationsCreate;
            target.StrategyVersion = source.StrategyVersion;
            target.DirectoryAccessasuserAll = source.DirectoryAccessasuserAll;
            target.DirectoryReadAll = source.DirectoryReadAll;
            target.DirectoryReadwriteAll = source.DirectoryReadwriteAll;
            target.GraphCalendars = source.GraphCalendars;
            target.GraphCalendarsUpdate = source.GraphCalendarsUpdate;
            target.GraphContacts = source.GraphContacts;
            target.GraphContactsUpdate = source.GraphContactsUpdate;
            target.GraphDevice = source.GraphDevice;
            target.GraphDeviceCommand = source.GraphDeviceCommand;
            target.GraphEmails = source.GraphEmails;
            target.GraphEmailsUpdate = source.GraphEmailsUpdate;
            target.GraphFiles = source.GraphFiles;
            target.GraphFilesAll = source.GraphFilesAll;
            target.GraphFilesAllUpdate = source.GraphFilesAllUpdate;
            target.GraphFilesUpdate = source.GraphFilesUpdate;
            target.GraphNotes = source.GraphNotes;
            target.GraphNotesCreate = source.GraphNotesCreate;
            target.GraphNotesUpdate = source.GraphNotesUpdate;
            target.GraphTasks = source.GraphTasks;
            target.GraphTasksUpdate = source.GraphTasksUpdate;
            target.GraphUser = source.GraphUser;
            target.GraphUserActivity = source.GraphUserActivity;
            target.GraphUserUpdate = source.GraphUserUpdate;
            target.GroupReadAll = source.GroupReadAll;
            target.GroupReadwriteAll = source.GroupReadwriteAll;
            target.MailReadwriteAll = source.MailReadwriteAll;
            target.MailSend = source.MailSend;
            target.RolemanagementReadAll = source.RolemanagementReadAll;
            target.RolemanagementReadwriteDirectory = source.RolemanagementReadwriteDirectory;
            target.SitesReadAll = source.SitesReadAll;
            target.SitesReadwriteAll = source.SitesReadwriteAll;
            target.TeamReadbasicAll = source.TeamReadbasicAll;
            target.TeamReadwriteAll = source.TeamReadwriteAll;
            target.UserReadAll = source.UserReadAll;
            target.UserReadbasicAll = source.UserReadbasicAll;
            return target;
        }

        internal static ConnectionOptionsYahoo ToApi(V1ConnectionYahooOptions source)
        {
            var target = new ConnectionOptionsYahoo();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa) target.NonPersistentAttrs = npa;
            if (ToApi(source.SetUserRootAttributes) is { } sura) target.SetUserRootAttributes = sura;
            return target;
        }

        /// <summary>
        /// Gets the list of enabled client IDs for the specified connection.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="connectionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<string[]> GetEnabledClientsAsync(IManagementApiClient api, string connectionId, CancellationToken cancellationToken)
        {
            var pager = await api.Connections.Clients.GetAsync(connectionId, new GetConnectionEnabledClientsRequestParameters(), null, cancellationToken);

            var l = new List<string>();
            foreach (var client in pager.CurrentPage.Items)
                if (client.ClientId is not null)
                    l.Add(client.ClientId);

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<V1ConnectionConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                var self = await api.Connections.GetAsync(id, new GetConnectionRequestParameters(), null, cancellationToken);
                if (self == null)
                    return null;

                var conf = FromApi(self);
                conf.EnabledClients = (await GetEnabledClientsAsync(api, id, cancellationToken)).Select(i => new V1ClientReference() { Id = i }).ToArray();
                return conf;
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Connection entity, V1Connection.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.ConnectionId is string connectionId)
                {
                    try
                    {
                        var connection = await api.Connections.GetAsync(connectionId, new GetConnectionRequestParameters(), null, cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing connection: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), connection.Name);
                        return connection.Id;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find connection with id {ConnectionId}.", EntityTypeName, entity.Namespace(), entity.Name(), connectionId);
                        return null;
                    }
                }

                return null;
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is null || string.IsNullOrEmpty(conf.Name))
                    return null;

                var pager = await api.Connections.ListAsync(new ListConnectionsQueryParameters { Name = conf.Name }, null, cancellationToken);
                var self = pager.CurrentPage.Items?.FirstOrDefault(i => i.Name == conf.Name);
                if (self is not null)
                    Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing connection by name: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);

                return self?.Id;
            }
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1ConnectionConf conf)
        {
            return null;
        }

        /// <summary>
        /// Attempts to resolve the list of client references to client IDs.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="refs"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        async Task<string[]> ResolveClientRefsToIds(IManagementApiClient api, V1ClientReference[]? refs, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (refs is null)
                return Array.Empty<string>();

            var l = new List<string>(refs.Length);

            foreach (var i in refs)
                l.Add(await ResolveClientRefToId(api, i, defaultNamespace, cancellationToken) ?? throw new InvalidOperationException());

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating connection in Auth0 with name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, conf.Name, conf.Strategy);

            if (conf.Strategy is null)
                throw new InvalidOperationException("Missing connection strategy.");

            var req = new CreateConnectionRequestContent()
            {
                Name = conf.Name ?? throw new InvalidOperationException("Missing connection name."),
                Strategy = ConnectionIdentityProviderEnum.FromCustom(conf.Strategy),
            };

            ApplyToApi(conf, req);

            var self = await api.Connections.CreateAsync(req, null, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            Logger.LogInformation("{EntityTypeName} successfully created connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, self.Id, conf.Name, conf.Strategy);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ConnectionConf? last, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);

            var req = new UpdateConnectionRequestContent();
            ApplyToApi(conf, req);

            await api.Connections.UpdateAsync(id, req, null, cancellationToken);
            await UpdateEnabledClientsAsync(api, id, conf, defaultNamespace, cancellationToken);

            Logger.LogInformation("{EntityTypeName} successfully updated connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
        }

        /// <summary>
        /// Resolves the strategy-specific options object for the given strategy name and options.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static object? ResolveStrategyOptions(string? strategy, V1ConnectionOptions? options) => strategy switch
        {
            "auth0" when options?.Auth0 is { } o => ToApi(o),
            "ad" when options?.Ad is { } o => ToApi(o),
            "adfs" when options?.Adfs is { } o => ToApi(o),
            "auth0-oidc" when options?.Auth0Oidc is { } o => ToApi(o),
            "waad" when options?.AzureAd is { } o => ToApi(o),
            "bitbucket" when options?.Bitbucket is { } o => ToApi(o),
            "box" when options?.Box is { } o => ToApi(o),
            "dropbox" when options?.Dropbox is { } o => ToApi(o),
            "email" when options?.Email is { } o => ToApi(o),
            "evernote" when options?.Evernote is { } o => ToApi(o),
            "evernote-sandbox" when options?.EvernoteSandbox is { } o => ToApi(o),
            "exact" when options?.Exact is { } o => ToApi(o),
            "facebook" when options?.Facebook is { } o => ToApi(o),
            "github" when options?.GitHub is { } o => ToApi(o),
            "google-apps" when options?.GoogleApps is { } o => ToApi(o),
            "google-oauth2" when options?.GoogleOAuth2 is { } o => ToApi(o),
            "linkedin" when options?.Linkedin is { } o => ToApi(o),
            "oauth1" when options?.OAuth1 is { } o => ToApi(o),
            "oauth2" when options?.OAuth2 is { } o => ToApi(o),
            "office365" when options?.Office365 is { } o => ToApi(o),
            "oidc" when options?.Oidc is { } o => ToApi(o),
            "okta" when options?.Okta is { } o => ToApi(o),
            "paypal" when options?.Paypal is { } o => ToApi(o),
            "paypal-sandbox" when options?.PaypalSandbox is { } o => ToApi(o),
            "pingfederate" when options?.PingFederate is { } o => ToApi(o),
            "salesforce" when options?.Salesforce is { } o => ToApi(o),
            "salesforce-community" when options?.SalesforceCommunity is { } o => ToApi(o),
            "salesforce-sandbox" when options?.SalesforceSandbox is { } o => ToApi(o),
            "samlp" when options?.Saml is { } o => ToApi(o),
            "sms" when options?.Sms is { } o => ToApi(o),
            "twitter" when options?.Twitter is { } o => ToApi(o),
            "windowslive" when options?.WindowsLive is { } o => ToApi(o),
            "yahoo" when options?.Yahoo is { } o => ToApi(o),
            _ => null,
        };

        /// <summary>
        /// Applies the specified configuration to the request object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        internal static void ApplyToApi(V1ConnectionConf source, CreateConnectionRequestContent target)
        {
            if (source.Name is { } name)
                target.Name = name;

            if (source.DisplayName is { } displayName)
                target.DisplayName = displayName;

            if (source.Metadata is { } metadata)
                target.Metadata = metadata.Cast<System.Collections.DictionaryEntry>().ToDictionary(e => (string)e.Key, e => e.Value?.ToString());

            if (source.Realms is { } realms)
                target.Realms = realms;

            if (source.IsDomainConnection is not null)
                target.IsDomainConnection = source.IsDomainConnection ?? false;

            if (source.ShowAsButton is { } showAsButton)
                target.ShowAsButton = showAsButton;

            var options = ResolveStrategyOptions(source.Strategy, source.Options);
            if (options is not null)
                target.Options = JsonSerializer.Deserialize<ConnectionPropertiesOptions>(JsonSerializer.Serialize(options));
        }

        internal static void ApplyToApi(V1ConnectionConf source, UpdateConnectionRequestContent target)
        {
            if (source.DisplayName is { } displayName)
                target.DisplayName = displayName;

            if (source.Metadata is { } metadata)
                target.Metadata = metadata.Cast<System.Collections.DictionaryEntry>().ToDictionary(e => (string)e.Key, e => e.Value?.ToString());

            if (source.Realms is { } realms)
                target.Realms = realms;

            if (source.IsDomainConnection is not null)
                target.IsDomainConnection = source.IsDomainConnection ?? false;

            if (source.ShowAsButton is { } showAsButton)
                target.ShowAsButton = showAsButton;

            var options = ResolveStrategyOptions(source.Strategy, source.Options);
            if (options is not null)
                target.Options = JsonSerializer.Deserialize<UpdateConnectionOptions>(JsonSerializer.Serialize(options));
        }

        static void ApplyToApi(V1ConnectionOptionsEmailAttribute source, EmailAttribute target)
        {
            if (source.Identifier is { } identifier)
            {
                target.Identifier ??= new ConnectionAttributeIdentifier();
                ApplyToApi(identifier, target.Identifier);
            }

            if (source.ProfileRequired is { } profileRequired)
                target.ProfileRequired = profileRequired;

            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupVerified();
                ApplyToApi(signup, target.Signup);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsEmailSignup source, SignupVerified target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new SignupVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPhoneNumberAttribute source, PhoneAttribute target)
        {
            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupVerified();
                ApplyToApi(signup, target.Signup, true);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPhoneNumberSignup source, SignupVerified target, bool phone)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new SignupVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUsernameAttribute source, UsernameAttribute target)
        {
            if (source.Identifier is { } identifier)
            {
                target.Identifier ??= new ConnectionAttributeIdentifier();
                ApplyToApi(identifier, target.Identifier);
            }

            if (source.ProfileRequired is { } profileRequired)
                target.ProfileRequired = profileRequired;

            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupSchema();
                ApplyToApi(signup, target.Signup);
            }

            if (source.Validation is { } validation)
            {
                target.Validation ??= new UsernameValidation();
                ApplyToApi(validation, target.Validation);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUsernameSignup source, SignupSchema target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeIdentifier source, ConnectionAttributeIdentifier target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeValidation source, UsernameValidation target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;

            if (source.MaxLength is { } maxLength)
                target.MaxLength = maxLength;

            if (source.AllowedTypes is { } allowedTypes)
            {
                target.AllowedTypes ??= new UsernameAllowedTypes();
                ApplyToApi(allowedTypes, target.AllowedTypes);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeAllowedTypes source, UsernameAllowedTypes target)
        {
            if (source.Email is { } email)
                target.Email = email;

            if (source.PhoneNumber is { } phoneNumber)
                target.PhoneNumber = phoneNumber;
        }

        static void ApplyToApi(V1ConnectionOptionsVerification source, SignupVerification target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V1ConnectionOptionsCustomScripts source, ConnectionCustomScripts target)
        {
            if (source.Login is { } login)
                target.Login = login;

            if (source.GetUser is { } getUser)
                target.GetUser = getUser;

            if (source.Delete is { } delete)
                target.Delete = delete;

            if (source.ChangePassword is { } changePassword)
                target.ChangePassword = changePassword;

            if (source.Verify is { } verify)
                target.Verify = verify;

            if (source.Create is { } create)
                target.Create = create;

            if (source.ChangeUsername is { } changeUsername)
                target.ChangeUsername = changeUsername;

            if (source.ChangeEmail is { } changeEmail)
                target.ChangeEmail = changeEmail;

            if (source.ChangePhoneNumber is { } changePhoneNumber)
                target.ChangePhoneNumber = changePhoneNumber;
        }

        static void ApplyToApi(V1ConnectionOptionsAuthenticationMethods source, ConnectionAuthenticationMethods target)
        {
            if (source.Password is { } password)
            {
                target.Password ??= new ConnectionPasswordAuthenticationMethod();
                ApplyToApi(password, target.Password);
            }

            if (source.Passkey is { } passkey)
            {
                target.Passkey ??= new ConnectionPasskeyAuthenticationMethod();
                ApplyToApi(passkey, target.Passkey);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordAuthenticationMethod source, ConnectionPasswordAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasskeyAuthenticationMethod source, ConnectionPasskeyAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasskeyOptions source, ConnectionPasskeyOptions target)
        {
            if (source.ChallengeUi is { } challengeUi)
                target.ChallengeUi = ToApi(challengeUi);

            if (source.ProgressiveEnrollmentEnabled is { } progressiveEnrollmentEnabled)
                target.ProgressiveEnrollmentEnabled = progressiveEnrollmentEnabled;

            if (source.LocalEnrollmentEnabled is { } localEnrollmentEnabled)
                target.LocalEnrollmentEnabled = localEnrollmentEnabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordComplexityOptions source, ConnectionPasswordComplexityOptions target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordHistory source, ConnectionPasswordHistoryOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Size is { } size)
                target.Size = size;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordNoPersonalInfo source, ConnectionPasswordNoPersonalInfoOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordDictionary source, ConnectionPasswordDictionaryOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Dictionary is { } dictionary)
                target.Dictionary = dictionary;
        }

        static void ApplyToApi(V1ConnectionGatewayAuthentication source, ConnectionGatewayAuthentication target)
        {
            if (source.Method is { } method)
                target.Method = method;

            if (source.Subject is { } subject)
                target.Subject = subject;

            if (source.Audience is { } audience)
                target.Audience = audience;

            if (source.Secret is { } secret)
                target.Secret = secret;

            if (source.SecretBase64Encoded is { } secretBase64Encoded)
                target.SecretBase64Encoded = secretBase64Encoded;
        }

        static void ApplyToApi(V1ConnectionOptions source, ref dynamic target)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(source);
            var patch = JObject.Parse(json);

            if (target is not JObject targetObj)
                targetObj = new JObject();

            MergeJObject(patch, targetObj);
            target = targetObj;
        }

        static void MergeJObject(JObject source, JObject target)
        {
            foreach (var property in source.Properties())
            {
                if (target[property.Name] is JObject existingObj && property.Value is JObject sourceObj)
                    MergeJObject(sourceObj, existingObj);
                else
                    target[property.Name] = property.Value;
            }
        }

        /// <summary>
        /// Applies the update of enabled clients.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task UpdateEnabledClientsAsync(IManagementApiClient api, string id, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (conf.EnabledClients is not null)
            {
                var req = new List<UpdateEnabledClientConnectionsRequestContentItem>();

                // apply existing clients, disabled by default
                var existingResponse = await api.Connections.Clients.GetAsync(id, new GetConnectionEnabledClientsRequestParameters(), null, cancellationToken);
                if (existingResponse?.CurrentPage?.Items is { } existingItems)
                    foreach (var current in existingItems)
                        if (current.ClientId is not null)
                            req.Add(new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = current.ClientId, Status = false });

                // add or enable clients specified in the configuration
                foreach (var clientId in await ResolveClientRefsToIds(api, conf.EnabledClients, defaultNamespace, cancellationToken))
                {
                    var existing = req.FirstOrDefault(i => i.ClientId == clientId);
                    if (existing is null)
                        req.Add(existing = new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = clientId, Status = false });

                    existing.Status = true;
                }

                // apply update
                if (req.Count > 0)
                    await api.Connections.Clients.UpdateAsync(id, req, null, cancellationToken);
            }
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting connection from Auth0 with ID: {ConnectionId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Connections.DeleteAsync(id, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted connection from Auth0 with ID: {ConnectionId}", EntityTypeName, id);
        }

    }

}
