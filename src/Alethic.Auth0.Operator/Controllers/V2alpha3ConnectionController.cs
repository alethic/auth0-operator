using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

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

    [EntityRbac(typeof(V2alpha3Connection), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha3Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V2alpha3ConnectionController :
        V1TenantEntityInstanceController<V2alpha3Connection, V2alpha3Connection.SpecDef, V2alpha3Connection.StatusDef, V2alpha3ConnectionConf, V2alpha3ConnectionConf>,
        IEntityController<V2alpha3Connection>
    {

        static JsonSerializerOptions GetAuth0JsonSerializerOptions()
        {
            var type = typeof(ConnectionOptionsAuth0).Assembly.GetType("Auth0.ManagementApi.Core.JsonOptions")
                ?? throw new InvalidOperationException("Unable to locate Auth0.ManagementApi.Core.JsonOptions.");

            return type.GetField("JsonSerializerOptions", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as JsonSerializerOptions
                ?? throw new InvalidOperationException("Unable to resolve Auth0 JSON serializer options.");
        }

        static readonly JsonSerializerOptions Auth0JsonSerializerOptions = GetAuth0JsonSerializerOptions();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V2alpha3ConnectionController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, Auth0HttpClientProvider httpClientProvider, ILogger<V2alpha3ConnectionController> logger) :
            base(kube, cache, options, httpClientProvider, logger)
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
            return JsonSerializer.Deserialize<TTo>(JsonSerializer.Serialize(source, Auth0JsonSerializerOptions), Auth0JsonSerializerOptions);
        }

        /// <summary>
        /// Converts an Auth0 strategy wire value to a <see cref="V2alpha3ConnectionStrategy"/>. Returns null for
        /// strategies not represented in the model so an unrecognized strategy cannot wedge reconciliation.
        /// </summary>
        internal static V2alpha3ConnectionStrategy? FromApiStrategy(string? strategy) => strategy switch
        {
            null => null,
            ConnectionResponseContentAuth0Strategy.Values.Auth0 => V2alpha3ConnectionStrategy.Auth0,
            ConnectionResponseContentAdStrategy.Values.Ad => V2alpha3ConnectionStrategy.Ad,
            ConnectionResponseContentAdfsStrategy.Values.Adfs => V2alpha3ConnectionStrategy.Adfs,
            ConnectionResponseContentAmazonStrategy.Values.Amazon => V2alpha3ConnectionStrategy.Amazon,
            ConnectionResponseContentAppleStrategy.Values.Apple => V2alpha3ConnectionStrategy.Apple,
            ConnectionResponseContentAuth0OidcStrategy.Values.Auth0Oidc => V2alpha3ConnectionStrategy.Auth0Oidc,
            ConnectionResponseContentBaiduStrategy.Values.Baidu => V2alpha3ConnectionStrategy.Baidu,
            ConnectionResponseContentAzureAdStrategy.Values.Waad => V2alpha3ConnectionStrategy.AzureAd,
            ConnectionResponseContentBitbucketStrategy.Values.Bitbucket => V2alpha3ConnectionStrategy.Bitbucket,
            ConnectionResponseContentBitlyStrategy.Values.Bitly => V2alpha3ConnectionStrategy.Bitly,
            ConnectionResponseContentBoxStrategy.Values.Box => V2alpha3ConnectionStrategy.Box,
            ConnectionResponseContentDaccountStrategy.Values.Daccount => V2alpha3ConnectionStrategy.Daccount,
            ConnectionResponseContentDropboxStrategy.Values.Dropbox => V2alpha3ConnectionStrategy.Dropbox,
            ConnectionResponseContentDwollaStrategy.Values.Dwolla => V2alpha3ConnectionStrategy.Dwolla,
            ConnectionResponseContentEmailStrategy.Values.Email => V2alpha3ConnectionStrategy.Email,
            ConnectionResponseContentEvernoteStrategy.Values.Evernote => V2alpha3ConnectionStrategy.Evernote,
            ConnectionResponseContentEvernoteSandboxStrategy.Values.EvernoteSandbox => V2alpha3ConnectionStrategy.EvernoteSandbox,
            ConnectionResponseContentExactStrategy.Values.Exact => V2alpha3ConnectionStrategy.Exact,
            ConnectionResponseContentFacebookStrategy.Values.Facebook => V2alpha3ConnectionStrategy.Facebook,
            ConnectionResponseContentFitbitStrategy.Values.Fitbit => V2alpha3ConnectionStrategy.Fitbit,
            ConnectionResponseContentGitHubStrategy.Values.Github => V2alpha3ConnectionStrategy.GitHub,
            ConnectionResponseContentGoogleAppsStrategy.Values.GoogleApps => V2alpha3ConnectionStrategy.GoogleApps,
            ConnectionResponseContentGoogleOAuth2Strategy.Values.GoogleOauth2 => V2alpha3ConnectionStrategy.GoogleOAuth2,
            ConnectionResponseContentInstagramStrategy.Values.Instagram => V2alpha3ConnectionStrategy.Instagram,
            ConnectionResponseContentLineStrategy.Values.Line => V2alpha3ConnectionStrategy.Line,
            ConnectionResponseContentLinkedinStrategy.Values.Linkedin => V2alpha3ConnectionStrategy.Linkedin,
            ConnectionResponseContentOAuth1Strategy.Values.Oauth1 => V2alpha3ConnectionStrategy.OAuth1,
            ConnectionResponseContentOAuth2Strategy.Values.Oauth2 => V2alpha3ConnectionStrategy.OAuth2,
            ConnectionResponseContentOffice365Strategy.Values.Office365 => V2alpha3ConnectionStrategy.Office365,
            ConnectionResponseContentOidcStrategy.Values.Oidc => V2alpha3ConnectionStrategy.Oidc,
            ConnectionResponseContentOktaStrategy.Values.Okta => V2alpha3ConnectionStrategy.Okta,
            ConnectionResponseContentPaypalStrategy.Values.Paypal => V2alpha3ConnectionStrategy.Paypal,
            ConnectionResponseContentPaypalSandboxStrategy.Values.PaypalSandbox => V2alpha3ConnectionStrategy.PaypalSandbox,
            ConnectionResponseContentPingFederateStrategy.Values.Pingfederate => V2alpha3ConnectionStrategy.PingFederate,
            ConnectionResponseContentPlanningCenterStrategy.Values.Planningcenter => V2alpha3ConnectionStrategy.PlanningCenter,
            ConnectionResponseContentSalesforceStrategy.Values.Salesforce => V2alpha3ConnectionStrategy.Salesforce,
            ConnectionResponseContentSalesforceCommunityStrategy.Values.SalesforceCommunity => V2alpha3ConnectionStrategy.SalesforceCommunity,
            ConnectionResponseContentSalesforceSandboxStrategy.Values.SalesforceSandbox => V2alpha3ConnectionStrategy.SalesforceSandbox,
            ConnectionResponseContentSamlStrategy.Values.Samlp => V2alpha3ConnectionStrategy.Saml,
            ConnectionResponseContentSharepointStrategy.Values.Sharepoint => V2alpha3ConnectionStrategy.Sharepoint,
            ConnectionResponseContentShopStrategy.Values.Shop => V2alpha3ConnectionStrategy.Shop,
            ConnectionResponseContentShopifyStrategy.Values.Shopify => V2alpha3ConnectionStrategy.Shopify,
            ConnectionResponseContentSmsStrategy.Values.Sms => V2alpha3ConnectionStrategy.Sms,
            ConnectionResponseContentSoundcloudStrategy.Values.Soundcloud => V2alpha3ConnectionStrategy.Soundcloud,
            ConnectionResponseContentThirtySevenSignalsStrategy.Values.Thirtysevensignals => V2alpha3ConnectionStrategy.ThirtySevenSignals,
            ConnectionResponseContentTwitterStrategy.Values.Twitter => V2alpha3ConnectionStrategy.Twitter,
            ConnectionResponseContentUntappdStrategy.Values.Untappd => V2alpha3ConnectionStrategy.Untappd,
            ConnectionResponseContentVkontakteStrategy.Values.Vkontakte => V2alpha3ConnectionStrategy.Vkontakte,
            ConnectionResponseContentWeiboStrategy.Values.Weibo => V2alpha3ConnectionStrategy.Weibo,
            ConnectionResponseContentWindowsLiveStrategy.Values.Windowslive => V2alpha3ConnectionStrategy.WindowsLive,
            ConnectionResponseContentWordpressStrategy.Values.Wordpress => V2alpha3ConnectionStrategy.Wordpress,
            ConnectionResponseContentYahooStrategy.Values.Yahoo => V2alpha3ConnectionStrategy.Yahoo,
            ConnectionResponseContentYandexStrategy.Values.Yandex => V2alpha3ConnectionStrategy.Yandex,
            _ => null,
        };

        /// <summary>
        /// Converts a <see cref="V2alpha3ConnectionStrategy"/> to the Auth0 strategy wire value.
        /// </summary>
        internal static string ToApiStrategy(V2alpha3ConnectionStrategy strategy) => strategy switch
        {
            V2alpha3ConnectionStrategy.Auth0 => ConnectionResponseContentAuth0Strategy.Values.Auth0,
            V2alpha3ConnectionStrategy.Ad => ConnectionResponseContentAdStrategy.Values.Ad,
            V2alpha3ConnectionStrategy.Adfs => ConnectionResponseContentAdfsStrategy.Values.Adfs,
            V2alpha3ConnectionStrategy.Amazon => ConnectionResponseContentAmazonStrategy.Values.Amazon,
            V2alpha3ConnectionStrategy.Apple => ConnectionResponseContentAppleStrategy.Values.Apple,
            V2alpha3ConnectionStrategy.Auth0Oidc => ConnectionResponseContentAuth0OidcStrategy.Values.Auth0Oidc,
            V2alpha3ConnectionStrategy.Baidu => ConnectionResponseContentBaiduStrategy.Values.Baidu,
            V2alpha3ConnectionStrategy.AzureAd => ConnectionResponseContentAzureAdStrategy.Values.Waad,
            V2alpha3ConnectionStrategy.Bitbucket => ConnectionResponseContentBitbucketStrategy.Values.Bitbucket,
            V2alpha3ConnectionStrategy.Bitly => ConnectionResponseContentBitlyStrategy.Values.Bitly,
            V2alpha3ConnectionStrategy.Box => ConnectionResponseContentBoxStrategy.Values.Box,
            V2alpha3ConnectionStrategy.Daccount => ConnectionResponseContentDaccountStrategy.Values.Daccount,
            V2alpha3ConnectionStrategy.Dropbox => ConnectionResponseContentDropboxStrategy.Values.Dropbox,
            V2alpha3ConnectionStrategy.Dwolla => ConnectionResponseContentDwollaStrategy.Values.Dwolla,
            V2alpha3ConnectionStrategy.Email => ConnectionResponseContentEmailStrategy.Values.Email,
            V2alpha3ConnectionStrategy.Evernote => ConnectionResponseContentEvernoteStrategy.Values.Evernote,
            V2alpha3ConnectionStrategy.EvernoteSandbox => ConnectionResponseContentEvernoteSandboxStrategy.Values.EvernoteSandbox,
            V2alpha3ConnectionStrategy.Exact => ConnectionResponseContentExactStrategy.Values.Exact,
            V2alpha3ConnectionStrategy.Facebook => ConnectionResponseContentFacebookStrategy.Values.Facebook,
            V2alpha3ConnectionStrategy.Fitbit => ConnectionResponseContentFitbitStrategy.Values.Fitbit,
            V2alpha3ConnectionStrategy.GitHub => ConnectionResponseContentGitHubStrategy.Values.Github,
            V2alpha3ConnectionStrategy.GoogleApps => ConnectionResponseContentGoogleAppsStrategy.Values.GoogleApps,
            V2alpha3ConnectionStrategy.GoogleOAuth2 => ConnectionResponseContentGoogleOAuth2Strategy.Values.GoogleOauth2,
            V2alpha3ConnectionStrategy.Instagram => ConnectionResponseContentInstagramStrategy.Values.Instagram,
            V2alpha3ConnectionStrategy.Line => ConnectionResponseContentLineStrategy.Values.Line,
            V2alpha3ConnectionStrategy.Linkedin => ConnectionResponseContentLinkedinStrategy.Values.Linkedin,
            V2alpha3ConnectionStrategy.OAuth1 => ConnectionResponseContentOAuth1Strategy.Values.Oauth1,
            V2alpha3ConnectionStrategy.OAuth2 => ConnectionResponseContentOAuth2Strategy.Values.Oauth2,
            V2alpha3ConnectionStrategy.Office365 => ConnectionResponseContentOffice365Strategy.Values.Office365,
            V2alpha3ConnectionStrategy.Oidc => ConnectionResponseContentOidcStrategy.Values.Oidc,
            V2alpha3ConnectionStrategy.Okta => ConnectionResponseContentOktaStrategy.Values.Okta,
            V2alpha3ConnectionStrategy.Paypal => ConnectionResponseContentPaypalStrategy.Values.Paypal,
            V2alpha3ConnectionStrategy.PaypalSandbox => ConnectionResponseContentPaypalSandboxStrategy.Values.PaypalSandbox,
            V2alpha3ConnectionStrategy.PingFederate => ConnectionResponseContentPingFederateStrategy.Values.Pingfederate,
            V2alpha3ConnectionStrategy.PlanningCenter => ConnectionResponseContentPlanningCenterStrategy.Values.Planningcenter,
            V2alpha3ConnectionStrategy.Salesforce => ConnectionResponseContentSalesforceStrategy.Values.Salesforce,
            V2alpha3ConnectionStrategy.SalesforceCommunity => ConnectionResponseContentSalesforceCommunityStrategy.Values.SalesforceCommunity,
            V2alpha3ConnectionStrategy.SalesforceSandbox => ConnectionResponseContentSalesforceSandboxStrategy.Values.SalesforceSandbox,
            V2alpha3ConnectionStrategy.Saml => ConnectionResponseContentSamlStrategy.Values.Samlp,
            V2alpha3ConnectionStrategy.Sharepoint => ConnectionResponseContentSharepointStrategy.Values.Sharepoint,
            V2alpha3ConnectionStrategy.Shop => ConnectionResponseContentShopStrategy.Values.Shop,
            V2alpha3ConnectionStrategy.Shopify => ConnectionResponseContentShopifyStrategy.Values.Shopify,
            V2alpha3ConnectionStrategy.Sms => ConnectionResponseContentSmsStrategy.Values.Sms,
            V2alpha3ConnectionStrategy.Soundcloud => ConnectionResponseContentSoundcloudStrategy.Values.Soundcloud,
            V2alpha3ConnectionStrategy.ThirtySevenSignals => ConnectionResponseContentThirtySevenSignalsStrategy.Values.Thirtysevensignals,
            V2alpha3ConnectionStrategy.Twitter => ConnectionResponseContentTwitterStrategy.Values.Twitter,
            V2alpha3ConnectionStrategy.Untappd => ConnectionResponseContentUntappdStrategy.Values.Untappd,
            V2alpha3ConnectionStrategy.Vkontakte => ConnectionResponseContentVkontakteStrategy.Values.Vkontakte,
            V2alpha3ConnectionStrategy.Weibo => ConnectionResponseContentWeiboStrategy.Values.Weibo,
            V2alpha3ConnectionStrategy.WindowsLive => ConnectionResponseContentWindowsLiveStrategy.Values.Windowslive,
            V2alpha3ConnectionStrategy.Wordpress => ConnectionResponseContentWordpressStrategy.Values.Wordpress,
            V2alpha3ConnectionStrategy.Yahoo => ConnectionResponseContentYahooStrategy.Values.Yahoo,
            V2alpha3ConnectionStrategy.Yandex => ConnectionResponseContentYandexStrategy.Values.Yandex,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Converts a <see cref="GetConnectionResponseContent"/> API response to a <see cref="V2alpha3ConnectionConf"/>.
        /// Note: <see cref="V2alpha3ConnectionConf.EnabledClients"/> is populated separately and left null here.
        /// </summary>
        [return: NotNullIfNotNull(nameof(source))]
        internal static V2alpha3ConnectionConf? FromApi(GetConnectionResponseContent? source)
        {
            if (source is null)
                return null;

            var conf = new V2alpha3ConnectionConf()
            {
                Name = source.Name,
                DisplayName = source.DisplayName,
                Strategy = FromApiStrategy(source.Strategy),
                Realms = source.Realms?.ToArray(),
                IsDomainConnection = source.IsDomainConnection,
                ShowAsButton = source.ShowAsButton,
                Metadata = source.Metadata is { } md ? new System.Collections.Hashtable(md) : null,
                Options = new V2alpha3ConnectionOptions()
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
                    conf.Options.Amazon = FromApi(JsonConvertTo<ConnectionResponseContentAmazon>(source)?.Options);
                    break;
                case ConnectionResponseContentAppleStrategy.Values.Apple:
                    conf.Options.Apple = FromApi(JsonConvertTo<ConnectionResponseContentApple>(source)?.Options);
                    break;
                case ConnectionResponseContentAuth0OidcStrategy.Values.Auth0Oidc:
                    conf.Options.Auth0Oidc = FromApi(JsonConvertTo<ConnectionResponseContentAuth0Oidc>(source)?.Options);
                    break;
                case ConnectionResponseContentBaiduStrategy.Values.Baidu:
                    conf.Options.Baidu = FromApi(JsonConvertTo<ConnectionResponseContentBaidu>(source)?.Options);
                    break;
                case ConnectionResponseContentBitbucketStrategy.Values.Bitbucket:
                    conf.Options.Bitbucket = FromApi(JsonConvertTo<ConnectionResponseContentBitbucket>(source)?.Options);
                    break;
                case ConnectionResponseContentBitlyStrategy.Values.Bitly:
                    conf.Options.Bitly = FromApi(JsonConvertTo<ConnectionResponseContentBitly>(source)?.Options);
                    break;
                case ConnectionResponseContentBoxStrategy.Values.Box:
                    conf.Options.Box = FromApi(JsonConvertTo<ConnectionResponseContentBox>(source)?.Options);
                    break;
                case ConnectionResponseContentDaccountStrategy.Values.Daccount:
                    conf.Options.Daccount = FromApi(JsonConvertTo<ConnectionResponseContentDaccount>(source)?.Options);
                    break;
                case ConnectionResponseContentDropboxStrategy.Values.Dropbox:
                    conf.Options.Dropbox = FromApi(JsonConvertTo<ConnectionResponseContentDropbox>(source)?.Options);
                    break;
                case ConnectionResponseContentDwollaStrategy.Values.Dwolla:
                    conf.Options.Dwolla = FromApi(JsonConvertTo<ConnectionResponseContentDwolla>(source)?.Options);
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
                    conf.Options.Fitbit = FromApi(JsonConvertTo<ConnectionResponseContentFitbit>(source)?.Options);
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
                    conf.Options.Instagram = FromApi(JsonConvertTo<ConnectionResponseContentInstagram>(source)?.Options);
                    break;
                case ConnectionResponseContentLineStrategy.Values.Line:
                    conf.Options.Line = FromApi(JsonConvertTo<ConnectionResponseContentLine>(source)?.Options);
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
                    conf.Options.PlanningCenter = FromApi(JsonConvertTo<ConnectionResponseContentPlanningCenter>(source)?.Options);
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
                    conf.Options.Sharepoint = FromApi(JsonConvertTo<ConnectionResponseContentSharepoint>(source)?.Options);
                    break;
                case ConnectionResponseContentShopifyStrategy.Values.Shopify:
                    conf.Options.Shopify = FromApi(JsonConvertTo<ConnectionResponseContentShopify>(source)?.Options);
                    break;
                case ConnectionResponseContentShopStrategy.Values.Shop:
                    conf.Options.Shop = FromApi(JsonConvertTo<ConnectionResponseContentShop>(source)?.Options);
                    break;
                case ConnectionResponseContentSmsStrategy.Values.Sms:
                    conf.Options.Sms = FromApi(JsonConvertTo<ConnectionResponseContentSms>(source)?.Options);
                    break;
                case ConnectionResponseContentSoundcloudStrategy.Values.Soundcloud:
                    conf.Options.Soundcloud = FromApi(JsonConvertTo<ConnectionResponseContentSoundcloud>(source)?.Options);
                    break;
                case ConnectionResponseContentThirtySevenSignalsStrategy.Values.Thirtysevensignals:
                    conf.Options.ThirtySevenSignals = FromApi(JsonConvertTo<ConnectionResponseContentThirtySevenSignals>(source)?.Options);
                    break;
                case ConnectionResponseContentTwitterStrategy.Values.Twitter:
                    conf.Options.Twitter = FromApi(JsonConvertTo<ConnectionResponseContentTwitter>(source)?.Options);
                    break;
                case ConnectionResponseContentUntappdStrategy.Values.Untappd:
                    conf.Options.Untappd = FromApi(JsonConvertTo<ConnectionResponseContentUntappd>(source)?.Options);
                    break;
                case ConnectionResponseContentVkontakteStrategy.Values.Vkontakte:
                    conf.Options.Vkontakte = FromApi(JsonConvertTo<ConnectionResponseContentVkontakte>(source)?.Options);
                    break;
                case ConnectionResponseContentAzureAdStrategy.Values.Waad:
                    conf.Options.AzureAd = FromApi(JsonConvertTo<ConnectionResponseContentAzureAd>(source)?.Options);
                    break;
                case ConnectionResponseContentWeiboStrategy.Values.Weibo:
                    conf.Options.Weibo = FromApi(JsonConvertTo<ConnectionResponseContentWeibo>(source)?.Options);
                    break;
                case ConnectionResponseContentWindowsLiveStrategy.Values.Windowslive:
                    conf.Options.WindowsLive = FromApi(JsonConvertTo<ConnectionResponseContentWindowsLive>(source)?.Options);
                    break;
                case ConnectionResponseContentWordpressStrategy.Values.Wordpress:
                    conf.Options.Wordpress = FromApi(JsonConvertTo<ConnectionResponseContentWordpress>(source)?.Options);
                    break;
                case ConnectionResponseContentYahooStrategy.Values.Yahoo:
                    conf.Options.Yahoo = FromApi(JsonConvertTo<ConnectionResponseContentYahoo>(source)?.Options);
                    break;
                case ConnectionResponseContentYandexStrategy.Values.Yandex:
                    conf.Options.Yandex = FromApi(JsonConvertTo<ConnectionResponseContentYandex>(source)?.Options);
                    break;
                default:
                    break;
            }

            return conf;
        }

        internal static V2alpha3ConnectionOptionsAuth0? FromApi(ConnectionOptionsAuth0? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsAuth0
            {
                Attributes = source.Attributes is { } attributes ? FromApi(attributes) : null,
                AuthenticationMethods = source.AuthenticationMethods.IsDefined && source.AuthenticationMethods.Value is { } authenticationMethods ? FromApi(authenticationMethods) : null,
                BruteForceProtection = source.BruteForceProtection,
                Configuration = source.Configuration?.ToDictionary(kv => kv.Key, kv => kv.Value),
                DisableSignup = source.DisableSignup,
                DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword,
                EnableScriptContext = source.EnableScriptContext,
                EnabledDatabaseCustomization = source.EnabledDatabaseCustomization,
                ImportMode = source.ImportMode,
                Mfa = source.Mfa is { } mfa ? FromApi(mfa) : null,
                PasskeyOptions = source.PasskeyOptions.IsDefined && source.PasskeyOptions.Value is { } passkeyOptions ? FromApi(passkeyOptions) : null,
                PasswordOptions = source.PasswordOptions is { } passwordOptions ? FromApi(passwordOptions) : null,
                Precedence = source.Precedence?.Select(static i => i.Value switch
                {
                    ConnectionIdentifierPrecedenceEnum.Values.Email => V2alpha3ConnectionIdentifierPrecedenceEnum.Email,
                    ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber => V2alpha3ConnectionIdentifierPrecedenceEnum.PhoneNumber,
                    ConnectionIdentifierPrecedenceEnum.Values.Username => V2alpha3ConnectionIdentifierPrecedenceEnum.Username,
                    _ => throw new ArgumentOutOfRangeException(nameof(source), i, null),
                }).ToArray(),
                RealmFallback = source.RealmFallback,
                RequiresUsername = source.RequiresUsername,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                PasswordPolicy = source.PasswordPolicy.IsDefined ? FromApi(source.PasswordPolicy.Value) : null,
                PasswordHistory = source.PasswordHistory.IsDefined && source.PasswordHistory.Value is { } ph ? FromApi(ph) : null,
                PasswordNoPersonalInfo = source.PasswordNoPersonalInfo.IsDefined && source.PasswordNoPersonalInfo.Value is { } pnpi ? FromApi(pnpi) : null,
                PasswordDictionary = source.PasswordDictionary.IsDefined && source.PasswordDictionary.Value is { } pd ? FromApi(pd) : null,
                PasswordComplexityOptions = source.PasswordComplexityOptions.IsDefined && source.PasswordComplexityOptions.Value is { } pco ? FromApi(pco) : null,
                Validation = source.Validation.IsDefined && source.Validation.Value is { } v ? FromApi(v) : null,
                CustomScripts = source.CustomScripts is { } cs ? FromApi(cs) : null,
            };
        }

        internal static V2alpha3ConnectionOptionsAd? FromApi(ConnectionOptionsAd? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsAd
            {
                AgentIp = source.AgentIp,
                AgentMode = source.AgentMode,
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
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Kerberos = source.Kerberos,
            };
        }

        internal static V2alpha3ConnectionOptionsAdfs? FromApi(ConnectionOptionsAdfs? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsAdfs
            {
                AdfsServer = source.AdfsServer,
                DomainAliases = source.DomainAliases?.ToArray(),
                EntityId = source.EntityId,
                FedMetadataXml = source.FedMetadataXml,
                IconUrl = source.IconUrl,
                PrevThumbprints = source.PrevThumbprints?.ToArray(),
                ShouldTrustEmailVerifiedConnection = source.ShouldTrustEmailVerifiedConnection switch
                {
                    { Value: ConnectionShouldTrustEmailVerifiedConnectionEnum.Values.NeverSetEmailsAsVerified } => V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum.NeverSetEmailsAsVerified,
                    { Value: ConnectionShouldTrustEmailVerifiedConnectionEnum.Values.AlwaysSetEmailsAsVerified } => V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum.AlwaysSetEmailsAsVerified,
                    null => null,
                    _ => throw new ArgumentOutOfRangeException(nameof(source), source.ShouldTrustEmailVerifiedConnection, null),
                },
                SignInEndpoint = source.SignInEndpoint,
                TenantDomain = source.TenantDomain,
                Thumbprints = source.Thumbprints?.ToArray(),
                UserIdAttribute = source.UserIdAttribute,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V2alpha3ConnectionOptionsAmazon? FromApi(ConnectionOptionsAmazon? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsAmazon
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                FreeformScopes = source.FreeformScopes?.ToArray(),
                PostalCode = source.PostalCode,
                Profile = source.Profile,
                Scope = source.Scope?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsApple? FromApi(ConnectionOptionsApple? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsApple
            {
                AppSecret = source.AppSecret.IsDefined ? source.AppSecret.Value : null,
                ClientId = source.ClientId.IsDefined ? source.ClientId.Value : null,
                Email = source.Email,
                FreeformScopes = source.FreeformScopes?.ToArray(),
                Kid = source.Kid.IsDefined ? source.Kid.Value : null,
                Name = source.Name,
                Scope = source.Scope,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                TeamId = source.TeamId.IsDefined ? source.TeamId.Value : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsAuth0Oidc? FromApi(ConnectionOptionsAuth0Oidc? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsAuth0Oidc
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
            };
        }

        internal static V2alpha3ConnectionOptionsBaidu? FromApi(ConnectionOptionsBaidu? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsBaidu
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsAzureAd? FromApi(ConnectionOptionsAzureAd? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsAzureAd
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
                IdentityApi = source.IdentityApi switch
                {
                    { Value: ConnectionIdentityApiEnumAzureAd.Values.MicrosoftIdentityPlatformV20 } => V2alpha3ConnectionIdentityApiEnumAzureAd.MicrosoftIdentityPlatformV20,
                    { Value: ConnectionIdentityApiEnumAzureAd.Values.AzureActiveDirectoryV10 } => V2alpha3ConnectionIdentityApiEnumAzureAd.AzureActiveDirectoryV10,
                    null => null,
                    _ => throw new ArgumentOutOfRangeException(nameof(source), source.IdentityApi, null),
                },
                MaxGroupsToRetrieve = source.MaxGroupsToRetrieve,
                Scope = source.Scope?.ToArray(),
                ShouldTrustEmailVerifiedConnection = source.ShouldTrustEmailVerifiedConnection switch
                {
                    { Value: ConnectionShouldTrustEmailVerifiedConnectionEnum.Values.NeverSetEmailsAsVerified } => V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum.NeverSetEmailsAsVerified,
                    { Value: ConnectionShouldTrustEmailVerifiedConnectionEnum.Values.AlwaysSetEmailsAsVerified } => V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum.AlwaysSetEmailsAsVerified,
                    null => null,
                    _ => throw new ArgumentOutOfRangeException(nameof(source), source.ShouldTrustEmailVerifiedConnection, null),
                },
                TenantDomain = source.TenantDomain,
                TenantId = source.TenantId,
                Thumbprints = source.Thumbprints?.ToArray(),
                UseCommonEndpoint = source.UseCommonEndpoint,
                UseWsfed = source.UseWsfed,
                FederatedConnectionsAccessTokens = source.FederatedConnectionsAccessTokens.IsDefined && source.FederatedConnectionsAccessTokens.Value is { } fcat ? FromApi(fcat) : null,
                UseridAttribute = source.UseridAttribute switch
                {
                    { Value: ConnectionUseridAttributeEnumAzureAd.Values.Oid } => V2alpha3ConnectionUseridAttributeEnumAzureAd.Oid,
                    { Value: ConnectionUseridAttributeEnumAzureAd.Values.Sub } => V2alpha3ConnectionUseridAttributeEnumAzureAd.Sub,
                    null => null,
                    _ => throw new ArgumentOutOfRangeException(nameof(source), source.UseridAttribute, null),
                },
                WaadProtocol = source.WaadProtocol switch
                {
                    { Value: ConnectionWaadProtocolEnumAzureAd.Values.WsFederation } => V2alpha3ConnectionWaadProtocolEnumAzureAd.WsFederation,
                    { Value: ConnectionWaadProtocolEnumAzureAd.Values.OpenidConnect } => V2alpha3ConnectionWaadProtocolEnumAzureAd.OpenidConnect,
                    null => null,
                    _ => throw new ArgumentOutOfRangeException(nameof(source), source.WaadProtocol, null),
                },
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V2alpha3ConnectionOptionsBitbucket? FromApi(ConnectionOptionsBitbucket? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsBitbucket
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToArray() : null,
                FreeformScopes = source.FreeformScopes?.ToArray(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Profile = source.Profile,
            };
        }

        internal static V2alpha3ConnectionOptionsBitly? FromApi(ConnectionOptionsBitly? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsBitly
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsBox? FromApi(ConnectionOptionsBox? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsBox
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsDaccount? FromApi(ConnectionOptionsDaccount? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsDaccount
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsDropbox? FromApi(ConnectionOptionsDropbox? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsDropbox
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsDwolla? FromApi(ConnectionOptionsDwolla? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsDwolla
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsEmail? FromApi(ConnectionOptionsEmail? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsEmail
            {
                Name = source.Name,
                Email = source.Email is { } e ? new V2alpha3ConnectionEmailEmail
                {
                    From = e.From,
                    Subject = e.Subject,
                    Body = e.Body,
                    Syntax = FromApi(e.Syntax),
                } : null,
                Totp = source.Totp is { } t ? new V2alpha3ConnectionTotpEmail
                {
                    Length = t.Length,
                    TimeStep = t.TimeStep,
                } : null,
                BruteForceProtection = source.BruteForceProtection,
                DisableSignup = source.DisableSignup,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsEvernote? FromApi(ConnectionOptionsEvernote? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsEvernote
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsFitbit? FromApi(ConnectionOptionsFitbit? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsFitbit
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsExact? FromApi(ConnectionOptionsExact? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsExact
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsFacebook? FromApi(ConnectionOptionsFacebook? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsFacebook
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope,
                FreeformScopes = source.FreeformScopes?.ToArray(),
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
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsGitHub? FromApi(ConnectionOptionsGitHub? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsGitHub
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToArray() : null,
                FreeformScopes = source.FreeformScopes?.ToArray(),
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
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsGoogleApps? FromApi(ConnectionOptionsGoogleApps? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsGoogleApps
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToArray() : null,
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
                    ? new V2alpha3ConnectionFederatedConnectionsAccessTokens { Active = fcat.Active }
                    : null,
                HandleLoginFromSocial = source.HandleLoginFromSocial,
            };
        }

        internal static V2alpha3ConnectionOptionsGoogleOAuth2? FromApi(ConnectionOptionsGoogleOAuth2? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsGoogleOAuth2
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
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
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsInstagram? FromApi(ConnectionOptionsInstagram? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsInstagram
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsLine? FromApi(ConnectionOptionsLine? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsLine
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                FreeformScopes = source.FreeformScopes?.ToArray(),
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                Email = source.Email,
                Profile = source.Profile,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsPlanningCenter? FromApi(ConnectionOptionsPlanningCenter? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsPlanningCenter
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsSharepoint? FromApi(ConnectionOptionsSharepoint? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsSharepoint
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsShop? FromApi(ConnectionOptionsShop? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsShop
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsShopify? FromApi(ConnectionOptionsShopify? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsShopify
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsSoundcloud? FromApi(ConnectionOptionsSoundcloud? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsSoundcloud
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsThirtySevenSignals? FromApi(ConnectionOptionsThirtySevenSignals? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsThirtySevenSignals
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsUntappd? FromApi(ConnectionOptionsUntappd? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsUntappd
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsVkontakte? FromApi(ConnectionOptionsVkontakte? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsVkontakte
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsWeibo? FromApi(ConnectionOptionsWeibo? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsWeibo
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsWordpress? FromApi(ConnectionOptionsWordpress? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsWordpress
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsYandex? FromApi(ConnectionOptionsYandex? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsYandex
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsLinkedin? FromApi(ConnectionOptionsLinkedin? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsLinkedin
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                BasicProfile = source.BasicProfile,
                Email = source.Email,
                Openid = source.Openid,
                FullProfile = source.FullProfile,
                StrategyVersion = source.StrategyVersion,
                Network = source.Network,
                Profile = source.Profile,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsOAuth1? FromApi(ConnectionOptionsOAuth1? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsOAuth1
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                AccessTokenUrl = source.AccessTokenUrl,
                RequestTokenUrl = source.RequestTokenUrl,
                SignatureMethod = FromApi(source.SignatureMethod),
                UserAuthorizationUrl = source.UserAuthorizationUrl,
                Scripts = source.Scripts is { } sc ? new V2alpha3ConnectionScriptsOAuth1 { FetchUserProfile = sc.FetchUserProfile } : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionOptionsOAuth2? FromApi(ConnectionOptionsOAuth2? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsOAuth2
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                AuthorizationUrl = source.AuthorizationUrl,
                TokenUrl = source.TokenUrl,
                LogoutUrl = source.LogoutUrl,
                Scope = source.Scope is not null ? source.Scope.ToString()!.Split(' ', StringSplitOptions.RemoveEmptyEntries) : null,
                IconUrl = source.IconUrl,
                PkceEnabled = source.PkceEnabled,
                UseOauthSpecScope = source.UseOauthSpecScope,
                Scripts = source.Scripts is { } sc ? new V2alpha3ConnectionScriptsOAuth2 { FetchUserProfile = sc.FetchUserProfile } : null,
                AuthParams = source.AuthParams?.ToDictionary(kv => kv.Key, kv => kv.Value),
                AuthParamsMap = source.AuthParamsMap?.ToDictionary(kv => kv.Key, kv => kv.Value),
                FieldsMap = source.FieldsMap?.ToDictionary(kv => kv.Key, kv => kv.Value),
                CustomHeaders = source.CustomHeaders?.ToDictionary(kv => kv.Key, kv => kv.Value),
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V2alpha3ConnectionOptionsOffice365? FromApi(ConnectionOptionsOffice365? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsOffice365
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
            };
        }

        internal static V2alpha3ConnectionOptionsOidc? FromApi(ConnectionOptionsOidc? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsOidc
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
                TokenEndpointAuthMethod = source.TokenEndpointAuthMethod.IsDefined ? FromApi(source.TokenEndpointAuthMethod.Value) : null,
                TokenEndpointAuthSigningAlg = source.TokenEndpointAuthSigningAlg.IsDefined ? FromApi(source.TokenEndpointAuthSigningAlg.Value) : null,
                TokenEndpointJwtcaAudFormat = FromApi(source.TokenEndpointJwtcaAudFormat),
                DpopSigningAlg = FromApi(source.DpopSigningAlg),
                IdTokenSignedResponseAlgs = source.IdTokenSignedResponseAlgs.IsDefined && source.IdTokenSignedResponseAlgs.Value is { } algs ? algs.Select(FromApi).ToArray() : null,
                SendBackChannelNonce = source.SendBackChannelNonce,
                Type = FromApi(source.Type),
                OidcMetadata = source.OidcMetadata is { } oidcMetadata ? FromApi(oidcMetadata) : null,
                AttributeMap = source.AttributeMap is { } am ? FromApi(am) : null,
                ConnectionSettings = source.ConnectionSettings is { } cs ? FromApi(cs) : null,
                FederatedConnectionsAccessTokens = source.FederatedConnectionsAccessTokens.IsDefined && source.FederatedConnectionsAccessTokens.Value is { } fcat ? FromApi(fcat) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V2alpha3ConnectionOptionsOkta? FromApi(ConnectionOptionsOkta? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsOkta
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
                TokenEndpointAuthMethod = source.TokenEndpointAuthMethod.IsDefined ? FromApi(source.TokenEndpointAuthMethod.Value) : null,
                TokenEndpointAuthSigningAlg = source.TokenEndpointAuthSigningAlg.IsDefined ? FromApi(source.TokenEndpointAuthSigningAlg.Value) : null,
                TokenEndpointJwtcaAudFormat = FromApi(source.TokenEndpointJwtcaAudFormat),
                DpopSigningAlg = FromApi(source.DpopSigningAlg),
                IdTokenSignedResponseAlgs = source.IdTokenSignedResponseAlgs.IsDefined && source.IdTokenSignedResponseAlgs.Value is { } algs ? algs.Select(FromApi).ToArray() : null,
                SendBackChannelNonce = source.SendBackChannelNonce,
                Type = FromApi(source.Type),
                OidcMetadata = source.OidcMetadata is { } oidcMetadata ? FromApi(oidcMetadata) : null,
                AttributeMap = source.AttributeMap is { } am ? FromApi(am) : null,
                ConnectionSettings = source.ConnectionSettings is { } cs ? FromApi(cs) : null,
                FederatedConnectionsAccessTokens = source.FederatedConnectionsAccessTokens.IsDefined && source.FederatedConnectionsAccessTokens.Value is { } fcat ? FromApi(fcat) : null,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Domain = source.Domain,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsPaypal? FromApi(ConnectionOptionsPaypal? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsPaypal
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Address = source.Address,
                Email = source.Email,
                Phone = source.Phone,
                Profile = source.Profile,
            };
        }

        internal static V2alpha3ConnectionOptionsPingFederate? FromApi(ConnectionOptionsPingFederate? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsPingFederate
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
                Idpinitiated = source.Idpinitiated is { } idp ? FromApi(idp) : null,
                DecryptionKey = FromApi(source.DecryptionKey),
                AssertionDecryptionSettings = source.AssertionDecryptionSettings is { } ads ? FromApi(ads) : null,
                IconUrl = source.IconUrl,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsSalesforce? FromApi(ConnectionOptionsSalesforce? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsSalesforce
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
                Profile = source.Profile,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsSalesforceCommunity? FromApi(ConnectionOptionsSalesforceCommunity? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsSalesforceCommunity
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                CommunityBaseUrl = source.CommunityBaseUrl,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
                Profile = source.Profile,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsSaml? FromApi(ConnectionOptionsSaml? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsSaml
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
                Idpinitiated = source.Idpinitiated is { } idp ? FromApi(idp) : null,
                SigningCert = source.SigningCert,
                SigningKey = source.SigningKey is { } signingKey ? new V2alpha3ConnectionSigningKeySaml { Key = signingKey.Key, Cert = signingKey.Cert } : null,
                DecryptionKey = FromApi(source.DecryptionKey),
                AssertionDecryptionSettings = source.AssertionDecryptionSettings is { } ads ? FromApi(ads) : null,
                FieldsMap = null,
                UserIdAttribute = source.UserIdAttribute,
                IconUrl = source.IconUrl,
                DomainAliases = source.DomainAliases?.ToArray(),
                TenantDomain = source.TenantDomain,
                GlobalTokenRevocationJwtIss = source.GlobalTokenRevocationJwtIss,
                GlobalTokenRevocationJwtSub = source.GlobalTokenRevocationJwtSub,
                UpstreamParams = FromApi(source.UpstreamParams),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
            };
        }

        internal static V2alpha3ConnectionDecryptionKeySaml? FromApi(ConnectionDecryptionKeySaml? source)
        {
            if (source is null)
                return null;

            if (source.IsString())
            {
                return new V2alpha3ConnectionDecryptionKeySaml
                {
                    PrivateKey = source.AsString(),
                };
            }

            if (source.IsConnectionDecryptionKeySamlCert())
            {
                var cert = source.AsConnectionDecryptionKeySamlCert();
                return new V2alpha3ConnectionDecryptionKeySaml
                {
                    KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert
                    {
                        Cert = cert.Cert,
                        Key = cert.Key,
                    },
                };
            }

            return new V2alpha3ConnectionDecryptionKeySaml();
        }

        internal static ConnectionDecryptionKeySaml? ToApi(V2alpha3ConnectionDecryptionKeySaml? source)
        {
            if (source is null)
                return null;

            if (source.PrivateKey is { } privateKey)
            {
                return ConnectionDecryptionKeySaml.FromString(privateKey);
            }

            if (source.KeyPair is { } keyPair)
            {
                return ConnectionDecryptionKeySaml.FromConnectionDecryptionKeySamlCert(
                    new ConnectionDecryptionKeySamlCert
                    {
                        Cert = keyPair.Cert,
                        Key = keyPair.Key,
                    });
            }

            return null;
        }

        internal static V2alpha3ConnectionOptionsSms? FromApi(ConnectionOptionsSms? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsSms
            {
                Name = source.Name,
                From = source.From,
                Template = source.Template,
                Syntax = FromApi(source.Syntax),
                Provider = FromApi(source.Provider),
                TwilioSid = source.TwilioSid,
                TwilioToken = source.TwilioToken,
                MessagingServiceSid = source.MessagingServiceSid,
                GatewayUrl = source.GatewayUrl,
                ForwardReqInfo = source.ForwardReqInfo,
                DisableSignup = source.DisableSignup,
                BruteForceProtection = source.BruteForceProtection,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                Totp = source.Totp is { } t ? new V2alpha3ConnectionTotpSms { Length = t.Length, TimeStep = t.TimeStep } : null,
                GatewayAuthentication = source.GatewayAuthentication.IsDefined && source.GatewayAuthentication.Value is { } ga ? FromApi(ga) : null,
            };
        }

        internal static V2alpha3ConnectionOptionsTwitter? FromApi(ConnectionOptionsTwitter? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsTwitter
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Protocol = FromApi(source.Protocol),
                OfflineAccess = source.OfflineAccess,
                Profile = source.Profile,
                TweetRead = source.TweetRead,
                UsersRead = source.UsersRead,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsWindowsLive? FromApi(ConnectionOptionsWindowsLive? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsWindowsLive
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope?.ToArray(),
                FreeformScopes = source.FreeformScopes?.ToArray(),
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                Basic = source.Basic,
                OfflineAccess = source.OfflineAccess,
                Signin = source.Signin,
                Birthday = source.Birthday,
                Calendars = source.Calendars,
                CalendarsUpdate = source.CalendarsUpdate,
                ContactsBirthday = source.ContactsBirthday,
                ContactsCreate = source.ContactsCreate,
                ContactsCalendars = source.ContactsCalendars,
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
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionOptionsYahoo? FromApi(ConnectionOptionsYahoo? source)
        {
            if (source is null)
                return null;

            return new V2alpha3ConnectionOptionsYahoo
            {
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                NonPersistentAttrs = source.NonPersistentAttrs?.ToArray(),
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                UpstreamParams = FromApi(source.UpstreamParams),
            };
        }

        internal static V2alpha3ConnectionSetUserRootAttributesEnum? FromApi(ConnectionSetUserRootAttributesEnum? source)
        {
            return source?.Value switch
            {
                ConnectionSetUserRootAttributesEnum.Values.OnEachLogin => V2alpha3ConnectionSetUserRootAttributesEnum.OnEachLogin,
                ConnectionSetUserRootAttributesEnum.Values.OnFirstLogin => V2alpha3ConnectionSetUserRootAttributesEnum.OnFirstLogin,
                ConnectionSetUserRootAttributesEnum.Values.NeverOnLogin => V2alpha3ConnectionSetUserRootAttributesEnum.NeverOnLogin,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static SignupStatusEnum? ToApi(V2alpha3ConnectionSignupStatusEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionSignupStatusEnum.Required => new SignupStatusEnum(SignupStatusEnum.Values.Required),
                V2alpha3ConnectionSignupStatusEnum.Optional => new SignupStatusEnum(SignupStatusEnum.Values.Optional),
                V2alpha3ConnectionSignupStatusEnum.Inactive => new SignupStatusEnum(SignupStatusEnum.Values.Inactive),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionShouldTrustEmailVerifiedConnectionEnum? ToApi(V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum.NeverSetEmailsAsVerified => new ConnectionShouldTrustEmailVerifiedConnectionEnum(ConnectionShouldTrustEmailVerifiedConnectionEnum.Values.NeverSetEmailsAsVerified),
                V2alpha3ConnectionShouldTrustEmailVerifiedConnectionEnum.AlwaysSetEmailsAsVerified => new ConnectionShouldTrustEmailVerifiedConnectionEnum(ConnectionShouldTrustEmailVerifiedConnectionEnum.Values.AlwaysSetEmailsAsVerified),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionPasskeyChallengeUiEnum? ToApi(V2alpha3ConnectionPasskeyChallengeUiEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionPasskeyChallengeUiEnum.Both => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Both),
                V2alpha3ConnectionPasskeyChallengeUiEnum.Autofill => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Autofill),
                V2alpha3ConnectionPasskeyChallengeUiEnum.Button => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Button),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionIdentityApiEnumAzureAd? ToApi(V2alpha3ConnectionIdentityApiEnumAzureAd? source)
        {
            return source switch
            {
                V2alpha3ConnectionIdentityApiEnumAzureAd.MicrosoftIdentityPlatformV20 => new ConnectionIdentityApiEnumAzureAd(ConnectionIdentityApiEnumAzureAd.Values.MicrosoftIdentityPlatformV20),
                V2alpha3ConnectionIdentityApiEnumAzureAd.AzureActiveDirectoryV10 => new ConnectionIdentityApiEnumAzureAd(ConnectionIdentityApiEnumAzureAd.Values.AzureActiveDirectoryV10),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionUseridAttributeEnumAzureAd? ToApi(V2alpha3ConnectionUseridAttributeEnumAzureAd? source)
        {
            return source switch
            {
                V2alpha3ConnectionUseridAttributeEnumAzureAd.Oid => new ConnectionUseridAttributeEnumAzureAd(ConnectionUseridAttributeEnumAzureAd.Values.Oid),
                V2alpha3ConnectionUseridAttributeEnumAzureAd.Sub => new ConnectionUseridAttributeEnumAzureAd(ConnectionUseridAttributeEnumAzureAd.Values.Sub),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionWaadProtocolEnumAzureAd? ToApi(V2alpha3ConnectionWaadProtocolEnumAzureAd? source)
        {
            return source switch
            {
                V2alpha3ConnectionWaadProtocolEnumAzureAd.WsFederation => new ConnectionWaadProtocolEnumAzureAd(ConnectionWaadProtocolEnumAzureAd.Values.WsFederation),
                V2alpha3ConnectionWaadProtocolEnumAzureAd.OpenidConnect => new ConnectionWaadProtocolEnumAzureAd(ConnectionWaadProtocolEnumAzureAd.Values.OpenidConnect),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionSignatureMethodOAuth1? ToApi(V2alpha3ConnectionSignatureMethodOAuth1? source)
        {
            return source switch
            {
                V2alpha3ConnectionSignatureMethodOAuth1.RsaSha1 => new ConnectionSignatureMethodOAuth1(ConnectionSignatureMethodOAuth1.Values.RsaSha1),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionTokenEndpointAuthMethodEnum? ToApi(V2alpha3ConnectionTokenEndpointAuthMethodEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionTokenEndpointAuthMethodEnum.ClientSecretPost => new ConnectionTokenEndpointAuthMethodEnum(ConnectionTokenEndpointAuthMethodEnum.Values.ClientSecretPost),
                V2alpha3ConnectionTokenEndpointAuthMethodEnum.PrivateKeyJwt => new ConnectionTokenEndpointAuthMethodEnum(ConnectionTokenEndpointAuthMethodEnum.Values.PrivateKeyJwt),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionTokenEndpointAuthSigningAlgEnum? ToApi(V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Es256 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Es256),
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Es384 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Es384),
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Ps256 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Ps256),
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Ps384 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Ps384),
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Rs256 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Rs256),
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Rs384 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Rs384),
                V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Rs512 => new ConnectionTokenEndpointAuthSigningAlgEnum(ConnectionTokenEndpointAuthSigningAlgEnum.Values.Rs512),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionTokenEndpointJwtcaAudFormatEnumOidc? ToApi(V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc? source)
        {
            return source switch
            {
                V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc.Issuer => new ConnectionTokenEndpointJwtcaAudFormatEnumOidc(ConnectionTokenEndpointJwtcaAudFormatEnumOidc.Values.Issuer),
                V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc.TokenEndpoint => new ConnectionTokenEndpointJwtcaAudFormatEnumOidc(ConnectionTokenEndpointJwtcaAudFormatEnumOidc.Values.TokenEndpoint),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionDpopSigningAlgEnum? ToApi(V2alpha3ConnectionDpopSigningAlgEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionDpopSigningAlgEnum.Es256 => new ConnectionDpopSigningAlgEnum(ConnectionDpopSigningAlgEnum.Values.Es256),
                V2alpha3ConnectionDpopSigningAlgEnum.Ed25519 => new ConnectionDpopSigningAlgEnum(ConnectionDpopSigningAlgEnum.Values.Ed25519),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionTypeEnumOidc? ToApi(V2alpha3ConnectionTypeEnumOidc? source)
        {
            return source switch
            {
                V2alpha3ConnectionTypeEnumOidc.BackChannel => new ConnectionTypeEnumOidc(ConnectionTypeEnumOidc.Values.BackChannel),
                V2alpha3ConnectionTypeEnumOidc.FrontChannel => new ConnectionTypeEnumOidc(ConnectionTypeEnumOidc.Values.FrontChannel),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionTypeEnumOkta? ToApi(V2alpha3ConnectionTypeEnumOkta? source)
        {
            return source switch
            {
                V2alpha3ConnectionTypeEnumOkta.BackChannel => new ConnectionTypeEnumOkta(ConnectionTypeEnumOkta.Values.BackChannel),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionOptionsProtocolEnumTwitter? ToApi(V2alpha3ConnectionOptionsProtocolEnumTwitter? source)
        {
            return source switch
            {
                V2alpha3ConnectionOptionsProtocolEnumTwitter.Oauth1 => new ConnectionOptionsProtocolEnumTwitter(ConnectionOptionsProtocolEnumTwitter.Values.Oauth1),
                V2alpha3ConnectionOptionsProtocolEnumTwitter.Oauth2 => new ConnectionOptionsProtocolEnumTwitter(ConnectionOptionsProtocolEnumTwitter.Values.Oauth2),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionSetUserRootAttributesEnum? ToApi(V2alpha3ConnectionSetUserRootAttributesEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionSetUserRootAttributesEnum.OnEachLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.OnEachLogin),
                V2alpha3ConnectionSetUserRootAttributesEnum.OnFirstLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.OnFirstLogin),
                V2alpha3ConnectionSetUserRootAttributesEnum.NeverOnLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.NeverOnLogin),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?> ToApiUpstreamAdditionalProperties(Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? source)
        {
            if (source is null)
                return default;

            var result = new Dictionary<string, ConnectionUpstreamAdditionalProperties?>(source.Count);
            foreach (var (key, value) in source)
                result[key] = value is null ? null : ToApi(value);

            return result;
        }

        internal static Dictionary<string, ConnectionUpstreamAdditionalProperties>? ToApiUpstreamAdditionalPropertiesNonOptional(Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? source)
        {
            if (source is null)
                return null;

            var result = new Dictionary<string, ConnectionUpstreamAdditionalProperties>(source.Count);
            foreach (var (key, value) in source)
                if (value is not null)
                    result[key] = ToApi(value);

            return result;
        }

        internal static ConnectionIdTokenSignedResponseAlgEnum ToApi(V2alpha3ConnectionIdTokenSignedResponseAlgEnum source)
        {
            return source switch
            {
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Es256 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Es256),
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Es384 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Es384),
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Ps256 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Ps256),
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Ps384 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Ps384),
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Rs256 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Rs256),
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Rs384 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Rs384),
                V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Rs512 => new ConnectionIdTokenSignedResponseAlgEnum(ConnectionIdTokenSignedResponseAlgEnum.Values.Rs512),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionMappingModeEnumOidc? ToApi(V2alpha3ConnectionMappingModeEnumOidc? source)
        {
            return source switch
            {
                V2alpha3ConnectionMappingModeEnumOidc.BindAll => new ConnectionMappingModeEnumOidc(ConnectionMappingModeEnumOidc.Values.BindAll),
                V2alpha3ConnectionMappingModeEnumOidc.UseMap => new ConnectionMappingModeEnumOidc(ConnectionMappingModeEnumOidc.Values.UseMap),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionMappingModeEnumOkta? ToApi(V2alpha3ConnectionMappingModeEnumOkta? source)
        {
            return source switch
            {
                V2alpha3ConnectionMappingModeEnumOkta.BasicProfile => new ConnectionMappingModeEnumOkta(ConnectionMappingModeEnumOkta.Values.BasicProfile),
                V2alpha3ConnectionMappingModeEnumOkta.UseMap => new ConnectionMappingModeEnumOkta(ConnectionMappingModeEnumOkta.Values.UseMap),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionConnectionSettingsPkceEnum? ToApi(V2alpha3ConnectionConnectionSettingsPkceEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionConnectionSettingsPkceEnum.Auto => new ConnectionConnectionSettingsPkceEnum(ConnectionConnectionSettingsPkceEnum.Values.Auto),
                V2alpha3ConnectionConnectionSettingsPkceEnum.S256 => new ConnectionConnectionSettingsPkceEnum(ConnectionConnectionSettingsPkceEnum.Values.S256),
                V2alpha3ConnectionConnectionSettingsPkceEnum.Plain => new ConnectionConnectionSettingsPkceEnum(ConnectionConnectionSettingsPkceEnum.Values.Plain),
                V2alpha3ConnectionConnectionSettingsPkceEnum.Disabled => new ConnectionConnectionSettingsPkceEnum(ConnectionConnectionSettingsPkceEnum.Values.Disabled),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionSignatureAlgorithmEnumSaml ToApi(V2alpha3ConnectionSignatureAlgorithmEnumSaml source)
        {
            return source switch
            {
                V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha1 => new ConnectionSignatureAlgorithmEnumSaml(ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha1),
                V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha256 => new ConnectionSignatureAlgorithmEnumSaml(ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha256),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionDigestAlgorithmEnumSaml ToApi(V2alpha3ConnectionDigestAlgorithmEnumSaml source)
        {
            return source switch
            {
                V2alpha3ConnectionDigestAlgorithmEnumSaml.Sha1 => new ConnectionDigestAlgorithmEnumSaml(ConnectionDigestAlgorithmEnumSaml.Values.Sha1),
                V2alpha3ConnectionDigestAlgorithmEnumSaml.Sha256 => new ConnectionDigestAlgorithmEnumSaml(ConnectionDigestAlgorithmEnumSaml.Values.Sha256),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionProtocolBindingEnumSaml ToApi(V2alpha3ConnectionProtocolBindingEnumSaml source)
        {
            return source switch
            {
                V2alpha3ConnectionProtocolBindingEnumSaml.UrnOasisNamesTcSaml20BindingsHttpPost => new ConnectionProtocolBindingEnumSaml(ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpPost),
                V2alpha3ConnectionProtocolBindingEnumSaml.UrnOasisNamesTcSaml20BindingsHttpRedirect => new ConnectionProtocolBindingEnumSaml(ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpRedirect),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionOptionsIdpInitiatedClientProtocolEnumSaml ToApi(V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml source)
        {
            return source switch
            {
                V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Oidc => new ConnectionOptionsIdpInitiatedClientProtocolEnumSaml(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Oidc),
                V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Samlp => new ConnectionOptionsIdpInitiatedClientProtocolEnumSaml(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Samlp),
                V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Wsfed => new ConnectionOptionsIdpInitiatedClientProtocolEnumSaml(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Wsfed),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionAssertionDecryptionAlgorithmProfileEnum ToApi(V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum source)
        {
            return source switch
            {
                V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum.V20261 => new ConnectionAssertionDecryptionAlgorithmProfileEnum(ConnectionAssertionDecryptionAlgorithmProfileEnum.Values.V20261),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static SignupStatusEnum ToApi(V2alpha3ConnectionOptionsAttributeStatus source)
        {
            return source switch
            {
                V2alpha3ConnectionOptionsAttributeStatus.Required => new SignupStatusEnum(SignupStatusEnum.Values.Required),
                V2alpha3ConnectionOptionsAttributeStatus.Optional => new SignupStatusEnum(SignupStatusEnum.Values.Optional),
                V2alpha3ConnectionOptionsAttributeStatus.Inactive => new SignupStatusEnum(SignupStatusEnum.Values.Inactive),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionPasswordPolicyEnum ToApi(V2alpha3ConnectionPasswordPolicyEnum source)
        {
            return source switch
            {
                V2alpha3ConnectionPasswordPolicyEnum.None => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.None),
                V2alpha3ConnectionPasswordPolicyEnum.Low => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Low),
                V2alpha3ConnectionPasswordPolicyEnum.Fair => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Fair),
                V2alpha3ConnectionPasswordPolicyEnum.Good => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Good),
                V2alpha3ConnectionPasswordPolicyEnum.Excellent => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Excellent),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionEmailEmailSyntax? ToApi(V2alpha3ConnectionEmailEmailSyntax? source)
        {
            return source switch
            {
                V2alpha3ConnectionEmailEmailSyntax.Liquid => new ConnectionEmailEmailSyntax(ConnectionEmailEmailSyntax.Values.Liquid),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? FromApi(Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?> source)
        {
            if (!source.IsDefined || source.Value is not { } dict)
                return null;

            var result = new Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>(dict.Count);
            foreach (var (key, value) in dict)
                if (value is not null)
                    result[key] = FromApi(value);

            return result.Count > 0 ? result : null;
        }

        internal static Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? FromApi(Dictionary<string, ConnectionUpstreamAdditionalProperties>? source)
        {
            if (source is null)
                return null;

            var result = new Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>(source.Count);
            foreach (var (key, value) in source)
                if (value is not null)
                    result[key] = FromApi(value);

            return result.Count > 0 ? result : null;
        }

        internal static V2alpha3ConnectionUpstreamAdditionalProperties FromApi(ConnectionUpstreamAdditionalProperties source)
        {
            if (source.TryGetConnectionUpstreamAlias(out var alias))
            {
                return new V2alpha3ConnectionUpstreamAdditionalProperties
                {
                    Alias = alias?.Alias is { } aliasValue ? FromApi(aliasValue) : null,
                };
            }

            if (source.TryGetConnectionUpstreamValue(out var value))
            {
                return new V2alpha3ConnectionUpstreamAdditionalProperties
                {
                    Value = value?.Value,
                };
            }

            throw new ArgumentOutOfRangeException(nameof(source), source, null);
        }

        internal static ConnectionUpstreamAdditionalProperties ToApi(V2alpha3ConnectionUpstreamAdditionalProperties source)
        {
            if (source.Alias is { } alias)
            {
                return ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamAlias(new ConnectionUpstreamAlias
                {
                    Alias = ToApi(alias),
                });
            }

            if (source.Value is { } value)
            {
                return ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamValue(new ConnectionUpstreamValue
                {
                    Value = value,
                });
            }

            throw new ArgumentException("Upstream additional properties must define either Alias or Value.", nameof(source));
        }

        internal static V2alpha3ConnectionUpstreamAliasEnum FromApi(ConnectionUpstreamAliasEnum source)
        {
            return source.Value switch
            {
                "acr_values" => V2alpha3ConnectionUpstreamAliasEnum.AcrValues,
                "audience" => V2alpha3ConnectionUpstreamAliasEnum.Audience,
                "client_id" => V2alpha3ConnectionUpstreamAliasEnum.ClientId,
                "display" => V2alpha3ConnectionUpstreamAliasEnum.Display,
                "id_token_hint" => V2alpha3ConnectionUpstreamAliasEnum.IdTokenHint,
                "login_hint" => V2alpha3ConnectionUpstreamAliasEnum.LoginHint,
                "max_age" => V2alpha3ConnectionUpstreamAliasEnum.MaxAge,
                "prompt" => V2alpha3ConnectionUpstreamAliasEnum.Prompt,
                "resource" => V2alpha3ConnectionUpstreamAliasEnum.Resource,
                "response_mode" => V2alpha3ConnectionUpstreamAliasEnum.ResponseMode,
                "response_type" => V2alpha3ConnectionUpstreamAliasEnum.ResponseType,
                "ui_locales" => V2alpha3ConnectionUpstreamAliasEnum.UiLocales,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionUpstreamAliasEnum ToApi(V2alpha3ConnectionUpstreamAliasEnum source)
        {
            return source switch
            {
                V2alpha3ConnectionUpstreamAliasEnum.AcrValues => ConnectionUpstreamAliasEnum.AcrValues,
                V2alpha3ConnectionUpstreamAliasEnum.Audience => ConnectionUpstreamAliasEnum.Audience,
                V2alpha3ConnectionUpstreamAliasEnum.ClientId => ConnectionUpstreamAliasEnum.ClientId,
                V2alpha3ConnectionUpstreamAliasEnum.Display => ConnectionUpstreamAliasEnum.Display,
                V2alpha3ConnectionUpstreamAliasEnum.IdTokenHint => ConnectionUpstreamAliasEnum.IdTokenHint,
                V2alpha3ConnectionUpstreamAliasEnum.LoginHint => ConnectionUpstreamAliasEnum.LoginHint,
                V2alpha3ConnectionUpstreamAliasEnum.MaxAge => ConnectionUpstreamAliasEnum.MaxAge,
                V2alpha3ConnectionUpstreamAliasEnum.Prompt => ConnectionUpstreamAliasEnum.Prompt,
                V2alpha3ConnectionUpstreamAliasEnum.Resource => ConnectionUpstreamAliasEnum.Resource,
                V2alpha3ConnectionUpstreamAliasEnum.ResponseMode => ConnectionUpstreamAliasEnum.ResponseMode,
                V2alpha3ConnectionUpstreamAliasEnum.ResponseType => ConnectionUpstreamAliasEnum.ResponseType,
                V2alpha3ConnectionUpstreamAliasEnum.UiLocales => ConnectionUpstreamAliasEnum.UiLocales,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionValidationOptions FromApi(ConnectionValidationOptions source)
        {
            return new V2alpha3ConnectionValidationOptions
            {
                Username = source.Username.IsDefined && source.Username.Value is { } u ? new V2alpha3ConnectionUsernameValidationOptions
                {
                    Min = u.Min,
                    Max = u.Max,
                } : null,
            };
        }

        internal static V2alpha3ConnectionAttributes FromApi(ConnectionAttributes source)
        {
            return new V2alpha3ConnectionAttributes
            {
                Email = source.Email is { } email ? FromApi(email) : null,
                PhoneNumber = source.PhoneNumber is { } phoneNumber ? FromApi(phoneNumber) : null,
                Username = source.Username is { } username ? FromApi(username) : null,
            };
        }

        internal static V2alpha3ConnectionEmailAttribute FromApi(EmailAttribute source)
        {
            return new V2alpha3ConnectionEmailAttribute
            {
                Identifier = source.Identifier is { } identifier ? FromApi(identifier) : null,
                Unique = source.Unique,
                ProfileRequired = source.ProfileRequired,
                VerificationMethod = FromApi(source.VerificationMethod),
                Signup = source.Signup is { } signup ? FromApi(signup) : null,
            };
        }

        internal static V2alpha3ConnectionPhoneAttribute FromApi(PhoneAttribute source)
        {
            return new V2alpha3ConnectionPhoneAttribute
            {
                Identifier = source.Identifier is { } identifier ? FromApi(identifier) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } signup ? FromApi(signup) : null,
            };
        }

        internal static V2alpha3ConnectionUsernameAttribute FromApi(UsernameAttribute source)
        {
            return new V2alpha3ConnectionUsernameAttribute
            {
                Identifier = source.Identifier is { } identifier ? FromApi(identifier) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } signup ? FromApi(signup) : null,
                Validation = source.Validation is { } validation ? FromApi(validation) : null,
            };
        }

        internal static V2alpha3ConnectionAttributeIdentifier FromApi(ConnectionAttributeIdentifier source)
        {
            return new V2alpha3ConnectionAttributeIdentifier
            {
                Active = source.Active,
            };
        }

        internal static V2alpha3ConnectionVerificationMethodEnum? FromApi(VerificationMethodEnum? source)
        {
            return source?.Value switch
            {
                VerificationMethodEnum.Values.Link => V2alpha3ConnectionVerificationMethodEnum.Link,
                VerificationMethodEnum.Values.Otp => V2alpha3ConnectionVerificationMethodEnum.Otp,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionSignupVerified FromApi(SignupVerified source)
        {
            return new V2alpha3ConnectionSignupVerified
            {
                Status = FromApi(source.Status),
                Verification = source.Verification is { } verification ? FromApi(verification) : null,
            };
        }

        internal static V2alpha3ConnectionSignupSchema FromApi(SignupSchema source)
        {
            return new V2alpha3ConnectionSignupSchema
            {
                Status = FromApi(source.Status),
            };
        }

        internal static V2alpha3ConnectionSignupStatusEnum? FromApi(SignupStatusEnum? source)
        {
            return source?.Value switch
            {
                SignupStatusEnum.Values.Required => V2alpha3ConnectionSignupStatusEnum.Required,
                SignupStatusEnum.Values.Optional => V2alpha3ConnectionSignupStatusEnum.Optional,
                SignupStatusEnum.Values.Inactive => V2alpha3ConnectionSignupStatusEnum.Inactive,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionSignupVerification FromApi(SignupVerification source)
        {
            return new V2alpha3ConnectionSignupVerification
            {
                Active = source.Active,
            };
        }

        internal static V2alpha3ConnectionUsernameValidation FromApi(UsernameValidation source)
        {
            return new V2alpha3ConnectionUsernameValidation
            {
                MinLength = source.MinLength,
                MaxLength = source.MaxLength,
                AllowedTypes = source.AllowedTypes is { } allowedTypes ? FromApi(allowedTypes) : null,
            };
        }

        internal static V2alpha3ConnectionUsernameAllowedTypes FromApi(UsernameAllowedTypes source)
        {
            return new V2alpha3ConnectionUsernameAllowedTypes
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
            };
        }

        internal static V2alpha3ConnectionAuthenticationMethods FromApi(ConnectionAuthenticationMethods source)
        {
            return new V2alpha3ConnectionAuthenticationMethods
            {
                Password = source.Password is { } password ? FromApi(password) : null,
                Passkey = source.Passkey is { } passkey ? FromApi(passkey) : null,
                EmailOtp = source.EmailOtp is { } emailOtp ? FromApi(emailOtp) : null,
                PhoneOtp = source.PhoneOtp is { } phoneOtp ? FromApi(phoneOtp) : null,
            };
        }

        internal static V2alpha3ConnectionPasswordAuthenticationMethod FromApi(ConnectionPasswordAuthenticationMethod source)
        {
            return new V2alpha3ConnectionPasswordAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V2alpha3ConnectionPasskeyAuthenticationMethod FromApi(ConnectionPasskeyAuthenticationMethod source)
        {
            return new V2alpha3ConnectionPasskeyAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V2alpha3ConnectionEmailOtpAuthenticationMethod FromApi(ConnectionEmailOtpAuthenticationMethod source)
        {
            return new V2alpha3ConnectionEmailOtpAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V2alpha3ConnectionPhoneOtpAuthenticationMethod FromApi(ConnectionPhoneOtpAuthenticationMethod source)
        {
            return new V2alpha3ConnectionPhoneOtpAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V2alpha3ConnectionMfa FromApi(ConnectionMfa source)
        {
            return new V2alpha3ConnectionMfa
            {
                Active = source.Active,
                ReturnEnrollSettings = source.ReturnEnrollSettings,
            };
        }

        internal static V2alpha3ConnectionPasskeyOptions FromApi(ConnectionPasskeyOptions source)
        {
            return new V2alpha3ConnectionPasskeyOptions
            {
                ChallengeUi = FromApi(source.ChallengeUi),
                ProgressiveEnrollmentEnabled = source.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = source.LocalEnrollmentEnabled,
            };
        }

        internal static V2alpha3ConnectionPasskeyChallengeUiEnum? FromApi(ConnectionPasskeyChallengeUiEnum? source)
        {
            return source?.Value switch
            {
                ConnectionPasskeyChallengeUiEnum.Values.Both => V2alpha3ConnectionPasskeyChallengeUiEnum.Both,
                ConnectionPasskeyChallengeUiEnum.Values.Autofill => V2alpha3ConnectionPasskeyChallengeUiEnum.Autofill,
                ConnectionPasskeyChallengeUiEnum.Values.Button => V2alpha3ConnectionPasskeyChallengeUiEnum.Button,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionPasswordOptions FromApi(ConnectionPasswordOptions source)
        {
            return new V2alpha3ConnectionPasswordOptions
            {
                Complexity = source.Complexity is { } complexity ? FromApi(complexity) : null,
                Dictionary = source.Dictionary is { } dictionary ? FromApi(dictionary) : null,
                History = source.History is { } history ? FromApi(history) : null,
                ProfileData = source.ProfileData is { } profileData ? FromApi(profileData) : null,
            };
        }

        internal static V2alpha3ConnectionPasswordOptionsComplexity FromApi(ConnectionPasswordOptionsComplexity source)
        {
            return new V2alpha3ConnectionPasswordOptionsComplexity
            {
                MinLength = source.MinLength,
            };
        }

        internal static V2alpha3ConnectionPasswordOptionsDictionary FromApi(ConnectionPasswordOptionsDictionary source)
        {
            return new V2alpha3ConnectionPasswordOptionsDictionary
            {
                Active = source.Active,
                Custom = source.Custom?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionPasswordOptionsHistory FromApi(ConnectionPasswordOptionsHistory source)
        {
            return new V2alpha3ConnectionPasswordOptionsHistory
            {
                Active = source.Active,
                Size = source.Size,
            };
        }

        internal static V2alpha3ConnectionPasswordOptionsProfileData FromApi(ConnectionPasswordOptionsProfileData source)
        {
            return new V2alpha3ConnectionPasswordOptionsProfileData
            {
                Active = source.Active,
                BlockedFields = source.BlockedFields?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionPasswordPolicyEnum? FromApi(ConnectionPasswordPolicyEnum? source)
        {
            return source?.Value switch
            {
                ConnectionPasswordPolicyEnum.Values.None => V2alpha3ConnectionPasswordPolicyEnum.None,
                ConnectionPasswordPolicyEnum.Values.Low => V2alpha3ConnectionPasswordPolicyEnum.Low,
                ConnectionPasswordPolicyEnum.Values.Fair => V2alpha3ConnectionPasswordPolicyEnum.Fair,
                ConnectionPasswordPolicyEnum.Values.Good => V2alpha3ConnectionPasswordPolicyEnum.Good,
                ConnectionPasswordPolicyEnum.Values.Excellent => V2alpha3ConnectionPasswordPolicyEnum.Excellent,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionPasswordHistoryOptions FromApi(ConnectionPasswordHistoryOptions source)
        {
            return new V2alpha3ConnectionPasswordHistoryOptions
            {
                Enable = source.Enable,
                Size = source.Size,
            };
        }

        internal static V2alpha3ConnectionPasswordNoPersonalInfoOptions FromApi(ConnectionPasswordNoPersonalInfoOptions source)
        {
            return new V2alpha3ConnectionPasswordNoPersonalInfoOptions
            {
                Enable = source.Enable,
            };
        }

        internal static V2alpha3ConnectionPasswordDictionaryOptions FromApi(ConnectionPasswordDictionaryOptions source)
        {
            return new V2alpha3ConnectionPasswordDictionaryOptions
            {
                Enable = source.Enable,
                Dictionary = source.Dictionary?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionPasswordComplexityOptions FromApi(ConnectionPasswordComplexityOptions source)
        {
            return new V2alpha3ConnectionPasswordComplexityOptions
            {
                MinLength = source.MinLength,
            };
        }

        internal static V2alpha3ConnectionCustomScripts FromApi(ConnectionCustomScripts source)
        {
            return new V2alpha3ConnectionCustomScripts
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

        internal static V2alpha3ConnectionEmailEmailSyntax? FromApi(ConnectionEmailEmailSyntax? source)
        {
            return source?.Value switch
            {
                ConnectionEmailEmailSyntax.Values.Liquid => V2alpha3ConnectionEmailEmailSyntax.Liquid,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionSignatureMethodOAuth1? FromApi(ConnectionSignatureMethodOAuth1? source)
        {
            return source?.Value switch
            {
                ConnectionSignatureMethodOAuth1.Values.RsaSha1 => V2alpha3ConnectionSignatureMethodOAuth1.RsaSha1,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionTokenEndpointAuthMethodEnum? FromApi(ConnectionTokenEndpointAuthMethodEnum? source)
        {
            return source?.Value switch
            {
                ConnectionTokenEndpointAuthMethodEnum.Values.ClientSecretPost => V2alpha3ConnectionTokenEndpointAuthMethodEnum.ClientSecretPost,
                ConnectionTokenEndpointAuthMethodEnum.Values.PrivateKeyJwt => V2alpha3ConnectionTokenEndpointAuthMethodEnum.PrivateKeyJwt,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum? FromApi(ConnectionTokenEndpointAuthSigningAlgEnum? source)
        {
            return source?.Value switch
            {
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Es256 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Es256,
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Es384 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Es384,
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Ps256 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Ps256,
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Ps384 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Ps384,
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Rs256 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Rs256,
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Rs384 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Rs384,
                ConnectionTokenEndpointAuthSigningAlgEnum.Values.Rs512 => V2alpha3ConnectionTokenEndpointAuthSigningAlgEnum.Rs512,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc? FromApi(ConnectionTokenEndpointJwtcaAudFormatEnumOidc? source)
        {
            return source?.Value switch
            {
                ConnectionTokenEndpointJwtcaAudFormatEnumOidc.Values.Issuer => V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc.Issuer,
                ConnectionTokenEndpointJwtcaAudFormatEnumOidc.Values.TokenEndpoint => V2alpha3ConnectionTokenEndpointJwtcaAudFormatEnumOidc.TokenEndpoint,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionDpopSigningAlgEnum? FromApi(ConnectionDpopSigningAlgEnum? source)
        {
            return source?.Value switch
            {
                ConnectionDpopSigningAlgEnum.Values.Es256 => V2alpha3ConnectionDpopSigningAlgEnum.Es256,
                ConnectionDpopSigningAlgEnum.Values.Ed25519 => V2alpha3ConnectionDpopSigningAlgEnum.Ed25519,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionIdTokenSignedResponseAlgEnum FromApi(ConnectionIdTokenSignedResponseAlgEnum source)
        {
            return source.Value switch
            {
                ConnectionIdTokenSignedResponseAlgEnum.Values.Es256 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Es256,
                ConnectionIdTokenSignedResponseAlgEnum.Values.Es384 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Es384,
                ConnectionIdTokenSignedResponseAlgEnum.Values.Ps256 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Ps256,
                ConnectionIdTokenSignedResponseAlgEnum.Values.Ps384 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Ps384,
                ConnectionIdTokenSignedResponseAlgEnum.Values.Rs256 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Rs256,
                ConnectionIdTokenSignedResponseAlgEnum.Values.Rs384 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Rs384,
                ConnectionIdTokenSignedResponseAlgEnum.Values.Rs512 => V2alpha3ConnectionIdTokenSignedResponseAlgEnum.Rs512,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionTypeEnumOidc? FromApi(ConnectionTypeEnumOidc? source)
        {
            return source?.Value switch
            {
                ConnectionTypeEnumOidc.Values.BackChannel => V2alpha3ConnectionTypeEnumOidc.BackChannel,
                ConnectionTypeEnumOidc.Values.FrontChannel => V2alpha3ConnectionTypeEnumOidc.FrontChannel,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionAttributeMapOidc FromApi(ConnectionAttributeMapOidc source)
        {
            return new V2alpha3ConnectionAttributeMapOidc
            {
                MappingMode = FromApi(source.MappingMode),
                UserinfoScope = source.UserinfoScope,
                Attributes = source.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value),
            };
        }

        internal static V2alpha3ConnectionMappingModeEnumOidc? FromApi(ConnectionMappingModeEnumOidc? source)
        {
            return source?.Value switch
            {
                ConnectionMappingModeEnumOidc.Values.BindAll => V2alpha3ConnectionMappingModeEnumOidc.BindAll,
                ConnectionMappingModeEnumOidc.Values.UseMap => V2alpha3ConnectionMappingModeEnumOidc.UseMap,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionConnectionSettings FromApi(ConnectionConnectionSettings source)
        {
            return new V2alpha3ConnectionConnectionSettings
            {
                Pkce = FromApi(source.Pkce),
            };
        }

        internal static V2alpha3ConnectionConnectionSettingsPkceEnum? FromApi(ConnectionConnectionSettingsPkceEnum? source)
        {
            return source?.Value switch
            {
                ConnectionConnectionSettingsPkceEnum.Values.Auto => V2alpha3ConnectionConnectionSettingsPkceEnum.Auto,
                ConnectionConnectionSettingsPkceEnum.Values.S256 => V2alpha3ConnectionConnectionSettingsPkceEnum.S256,
                ConnectionConnectionSettingsPkceEnum.Values.Plain => V2alpha3ConnectionConnectionSettingsPkceEnum.Plain,
                ConnectionConnectionSettingsPkceEnum.Values.Disabled => V2alpha3ConnectionConnectionSettingsPkceEnum.Disabled,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionFederatedConnectionsAccessTokens FromApi(ConnectionFederatedConnectionsAccessTokens source)
        {
            return new V2alpha3ConnectionFederatedConnectionsAccessTokens
            {
                Active = source.Active,
            };
        }

        internal static V2alpha3ConnectionOptionsOidcMetadata FromApi(ConnectionOptionsOidcMetadata source)
        {
            return new V2alpha3ConnectionOptionsOidcMetadata
            {
                AcrValuesSupported = source.AcrValuesSupported?.ToArray(),
                AuthorizationEndpoint = source.AuthorizationEndpoint,
                ClaimTypesSupported = source.ClaimTypesSupported?.ToArray(),
                ClaimsLocalesSupported = source.ClaimsLocalesSupported?.ToArray(),
                ClaimsParameterSupported = source.ClaimsParameterSupported,
                ClaimsSupported = source.ClaimsSupported?.ToArray(),
                DisplayValuesSupported = source.DisplayValuesSupported?.ToArray(),
                DpopSigningAlgValuesSupported = source.DpopSigningAlgValuesSupported?.ToArray(),
                EndSessionEndpoint = source.EndSessionEndpoint,
                GrantTypesSupported = source.GrantTypesSupported?.ToArray(),
                IdTokenEncryptionAlgValuesSupported = source.IdTokenEncryptionAlgValuesSupported?.ToArray(),
                IdTokenEncryptionEncValuesSupported = source.IdTokenEncryptionEncValuesSupported?.ToArray(),
                IdTokenSigningAlgValuesSupported = source.IdTokenSigningAlgValuesSupported?.ToArray(),
                Issuer = source.Issuer,
                JwksUri = source.JwksUri,
                OpPolicyUri = source.OpPolicyUri,
                OpTosUri = source.OpTosUri,
                RegistrationEndpoint = source.RegistrationEndpoint,
                RequestObjectEncryptionAlgValuesSupported = source.RequestObjectEncryptionAlgValuesSupported?.ToArray(),
                RequestObjectEncryptionEncValuesSupported = source.RequestObjectEncryptionEncValuesSupported?.ToArray(),
                RequestObjectSigningAlgValuesSupported = source.RequestObjectSigningAlgValuesSupported?.ToArray(),
                RequestParameterSupported = source.RequestParameterSupported,
                RequestUriParameterSupported = source.RequestUriParameterSupported,
                RequireRequestUriRegistration = source.RequireRequestUriRegistration,
                ResponseModesSupported = source.ResponseModesSupported?.ToArray(),
                ResponseTypesSupported = source.ResponseTypesSupported?.ToArray(),
                ScopesSupported = source.ScopesSupported.IsDefined && source.ScopesSupported.Value is { } scopesSupported ? scopesSupported.ToArray() : null,
                ServiceDocumentation = source.ServiceDocumentation,
                SubjectTypesSupported = source.SubjectTypesSupported?.ToArray(),
                TokenEndpoint = source.TokenEndpoint,
                TokenEndpointAuthMethodsSupported = source.TokenEndpointAuthMethodsSupported?.ToArray(),
                TokenEndpointAuthSigningAlgValuesSupported = source.TokenEndpointAuthSigningAlgValuesSupported?.ToArray(),
                UiLocalesSupported = source.UiLocalesSupported?.ToArray(),
                UserinfoEncryptionAlgValuesSupported = source.UserinfoEncryptionAlgValuesSupported?.ToArray(),
                UserinfoEncryptionEncValuesSupported = source.UserinfoEncryptionEncValuesSupported?.ToArray(),
                UserinfoEndpoint = source.UserinfoEndpoint,
                UserinfoSigningAlgValuesSupported = source.UserinfoSigningAlgValuesSupported?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionTypeEnumOkta? FromApi(ConnectionTypeEnumOkta? source)
        {
            return source?.Value switch
            {
                ConnectionTypeEnumOkta.Values.BackChannel => V2alpha3ConnectionTypeEnumOkta.BackChannel,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionAttributeMapOkta FromApi(ConnectionAttributeMapOkta source)
        {
            return new V2alpha3ConnectionAttributeMapOkta
            {
                MappingMode = FromApi(source.MappingMode),
                UserinfoScope = source.UserinfoScope,
                Attributes = source.Attributes?.ToDictionary(kv => kv.Key, kv => kv.Value),
            };
        }

        internal static V2alpha3ConnectionMappingModeEnumOkta? FromApi(ConnectionMappingModeEnumOkta? source)
        {
            return source?.Value switch
            {
                ConnectionMappingModeEnumOkta.Values.BasicProfile => V2alpha3ConnectionMappingModeEnumOkta.BasicProfile,
                ConnectionMappingModeEnumOkta.Values.UseMap => V2alpha3ConnectionMappingModeEnumOkta.UseMap,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionSignatureAlgorithmEnumSaml? FromApi(ConnectionSignatureAlgorithmEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha1 => V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha1,
                ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha256 => V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha256,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionDigestAlgorithmEnumSaml? FromApi(ConnectionDigestAlgorithmEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionDigestAlgorithmEnumSaml.Values.Sha1 => V2alpha3ConnectionDigestAlgorithmEnumSaml.Sha1,
                ConnectionDigestAlgorithmEnumSaml.Values.Sha256 => V2alpha3ConnectionDigestAlgorithmEnumSaml.Sha256,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionProtocolBindingEnumSaml? FromApi(ConnectionProtocolBindingEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpPost => V2alpha3ConnectionProtocolBindingEnumSaml.UrnOasisNamesTcSaml20BindingsHttpPost,
                ConnectionProtocolBindingEnumSaml.Values.UrnOasisNamesTcSaml20BindingsHttpRedirect => V2alpha3ConnectionProtocolBindingEnumSaml.UrnOasisNamesTcSaml20BindingsHttpRedirect,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionOptionsIdpinitiatedSaml FromApi(ConnectionOptionsIdpinitiatedSaml source)
        {
            return new V2alpha3ConnectionOptionsIdpinitiatedSaml
            {
                ClientId = source.ClientId,
                ClientProtocol = FromApi(source.ClientProtocol),
                ClientAuthorizequery = source.ClientAuthorizequery,
                Enabled = source.Enabled,
            };
        }

        internal static V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml? FromApi(ConnectionOptionsIdpInitiatedClientProtocolEnumSaml? source)
        {
            return source?.Value switch
            {
                ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Oidc => V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Oidc,
                ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Samlp => V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Samlp,
                ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Values.Wsfed => V2alpha3ConnectionOptionsIdpInitiatedClientProtocolEnumSaml.Wsfed,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionAssertionDecryptionSettings FromApi(ConnectionAssertionDecryptionSettings source)
        {
            return new V2alpha3ConnectionAssertionDecryptionSettings
            {
                AlgorithmProfile = FromApi(source.AlgorithmProfile),
                AlgorithmExceptions = source.AlgorithmExceptions?.ToArray(),
            };
        }

        internal static V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum FromApi(ConnectionAssertionDecryptionAlgorithmProfileEnum source)
        {
            return source.Value switch
            {
                ConnectionAssertionDecryptionAlgorithmProfileEnum.Values.V20261 => V2alpha3ConnectionAssertionDecryptionAlgorithmProfileEnum.V20261,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionTemplateSyntaxEnumSms? FromApi(ConnectionTemplateSyntaxEnumSms? source)
        {
            return source?.Value switch
            {
                ConnectionTemplateSyntaxEnumSms.Values.Liquid => V2alpha3ConnectionTemplateSyntaxEnumSms.Liquid,
                ConnectionTemplateSyntaxEnumSms.Values.MdWithMacros => V2alpha3ConnectionTemplateSyntaxEnumSms.MdWithMacros,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionProviderEnumSms? FromApi(ConnectionProviderEnumSms? source)
        {
            return source?.Value switch
            {
                ConnectionProviderEnumSms.Values.SmsGateway => V2alpha3ConnectionProviderEnumSms.SmsGateway,
                ConnectionProviderEnumSms.Values.Twilio => V2alpha3ConnectionProviderEnumSms.Twilio,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionGatewayAuthenticationSms FromApi(ConnectionGatewayAuthenticationSms source)
        {
            return new V2alpha3ConnectionGatewayAuthenticationSms
            {
                Method = source.Method,
                Subject = source.Subject,
                Audience = source.Audience,
                Secret = source.Secret,
                SecretBase64Encoded = source.SecretBase64Encoded,
            };
        }

        internal static V2alpha3ConnectionOptionsProtocolEnumTwitter? FromApi(ConnectionOptionsProtocolEnumTwitter? source)
        {
            return source?.Value switch
            {
                ConnectionOptionsProtocolEnumTwitter.Values.Oauth1 => V2alpha3ConnectionOptionsProtocolEnumTwitter.Oauth1,
                ConnectionOptionsProtocolEnumTwitter.Values.Oauth2 => V2alpha3ConnectionOptionsProtocolEnumTwitter.Oauth2,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionOptionsUserName FromApi(ConnectionUsernameValidationOptions source)
        {
            return new V2alpha3ConnectionOptionsUserName
            {
                Min = source.Min,
                Max = source.Max,
            };
        }

        internal static V2alpha3ConnectionGatewayAuthentication FromApi(ConnectionGatewayAuthentication source)
        {
            return new V2alpha3ConnectionGatewayAuthentication
            {
                Method = source.Method,
                Subject = source.Subject,
                Audience = source.Audience,
                Secret = source.Secret,
                SecretBase64Encoded = source.SecretBase64Encoded,
            };
        }

        internal static V2alpha3ConnectionOptionsPrecedence FromApi(ConnectionIdentifierPrecedenceEnum source)
        {
            return source.Value switch
            {
                ConnectionIdentifierPrecedenceEnum.Values.Email => V2alpha3ConnectionOptionsPrecedence.Email,
                ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber => V2alpha3ConnectionOptionsPrecedence.PhoneNumber,
                ConnectionIdentifierPrecedenceEnum.Values.Username => V2alpha3ConnectionOptionsPrecedence.UserName,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionIdentifierPrecedenceEnum ToApi(V2alpha3ConnectionOptionsPrecedence source)
        {
            return source switch
            {
                V2alpha3ConnectionOptionsPrecedence.Email => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Email),
                V2alpha3ConnectionOptionsPrecedence.PhoneNumber => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber),
                V2alpha3ConnectionOptionsPrecedence.UserName => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Username),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionIdentifierPrecedenceEnum ToApi(V2alpha3ConnectionIdentifierPrecedenceEnum source)
        {
            return source switch
            {
                V2alpha3ConnectionIdentifierPrecedenceEnum.Email => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Email),
                V2alpha3ConnectionIdentifierPrecedenceEnum.PhoneNumber => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber),
                V2alpha3ConnectionIdentifierPrecedenceEnum.Username => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Username),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static V2alpha3ConnectionChallengeUi FromApi(ConnectionPasskeyChallengeUiEnum source)
        {
            return source.Value switch
            {
                ConnectionPasskeyChallengeUiEnum.Values.Both => V2alpha3ConnectionChallengeUi.Both,
                ConnectionPasskeyChallengeUiEnum.Values.Autofill => V2alpha3ConnectionChallengeUi.AutoFill,
                ConnectionPasskeyChallengeUiEnum.Values.Button => V2alpha3ConnectionChallengeUi.Button,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionPasskeyChallengeUiEnum ToApi(V2alpha3ConnectionChallengeUi source)
        {
            return source switch
            {
                V2alpha3ConnectionChallengeUi.Both => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Both),
                V2alpha3ConnectionChallengeUi.AutoFill => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Autofill),
                V2alpha3ConnectionChallengeUi.Button => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Button),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionSetUserRootAttributesEnum? ToApi(V2alpha3ConnectionSetUserRootAttributes? source)
        {
            return source switch
            {
                V2alpha3ConnectionSetUserRootAttributes.OnEachLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.OnEachLogin),
                V2alpha3ConnectionSetUserRootAttributes.OnFirstLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.OnFirstLogin),
                V2alpha3ConnectionSetUserRootAttributes.NeverOnLogin => new ConnectionSetUserRootAttributesEnum(ConnectionSetUserRootAttributesEnum.Values.NeverOnLogin),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static ConnectionAttributes ToApi(V2alpha3ConnectionAttributes source)
        {
            return new ConnectionAttributes
            {
                Email = source.Email is { } email ? ToApi(email) : null,
                PhoneNumber = source.PhoneNumber is { } phoneNumber ? ToApi(phoneNumber) : null,
                Username = source.Username is { } username ? ToApi(username) : null,
            };
        }

        internal static EmailAttribute ToApi(V2alpha3ConnectionEmailAttribute source)
        {
            return new EmailAttribute
            {
                Identifier = source.Identifier is { } identifier ? ToApi(identifier) : null,
                Unique = source.Unique,
                ProfileRequired = source.ProfileRequired,
                VerificationMethod = ToApi(source.VerificationMethod),
                Signup = source.Signup is { } signup ? ToApi(signup) : null,
            };
        }

        internal static PhoneAttribute ToApi(V2alpha3ConnectionPhoneAttribute source)
        {
            return new PhoneAttribute
            {
                Identifier = source.Identifier is { } identifier ? ToApi(identifier) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } signup ? ToApi(signup) : null,
            };
        }

        internal static UsernameAttribute ToApi(V2alpha3ConnectionUsernameAttribute source)
        {
            return new UsernameAttribute
            {
                Identifier = source.Identifier is { } identifier ? ToApi(identifier) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } signup ? ToApi(signup) : null,
                Validation = source.Validation is { } validation ? ToApi(validation) : null,
            };
        }

        internal static ConnectionAttributeIdentifier ToApi(V2alpha3ConnectionAttributeIdentifier source)
        {
            return new ConnectionAttributeIdentifier
            {
                Active = source.Active,
            };
        }

        internal static VerificationMethodEnum? ToApi(V2alpha3ConnectionVerificationMethodEnum? source)
        {
            return source switch
            {
                V2alpha3ConnectionVerificationMethodEnum.Link => new VerificationMethodEnum(VerificationMethodEnum.Values.Link),
                V2alpha3ConnectionVerificationMethodEnum.Otp => new VerificationMethodEnum(VerificationMethodEnum.Values.Otp),
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
            };
        }

        internal static SignupVerified ToApi(V2alpha3ConnectionSignupVerified source)
        {
            return new SignupVerified
            {
                Status = source.Status is { } status ? ToApi(status) : null,
                Verification = source.Verification is { } verification ? ToApi(verification) : null,
            };
        }

        internal static SignupSchema ToApi(V2alpha3ConnectionSignupSchema source)
        {
            return new SignupSchema
            {
                Status = source.Status is { } status ? ToApi(status) : null,
            };
        }

        internal static SignupVerification ToApi(V2alpha3ConnectionSignupVerification source)
        {
            return new SignupVerification
            {
                Active = source.Active,
            };
        }

        internal static UsernameValidation ToApi(V2alpha3ConnectionUsernameValidation source)
        {
            return new UsernameValidation
            {
                MinLength = source.MinLength,
                MaxLength = source.MaxLength,
                AllowedTypes = source.AllowedTypes is { } allowedTypes ? ToApi(allowedTypes) : null,
            };
        }

        internal static UsernameAllowedTypes ToApi(V2alpha3ConnectionUsernameAllowedTypes source)
        {
            return new UsernameAllowedTypes
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
            };
        }

        internal static ConnectionAuthenticationMethods ToApi(V2alpha3ConnectionAuthenticationMethods source)
        {
            return new ConnectionAuthenticationMethods
            {
                Password = source.Password is { } password ? ToApi(password) : null,
                Passkey = source.Passkey is { } passkey ? ToApi(passkey) : null,
                EmailOtp = source.EmailOtp is { } emailOtp ? ToApi(emailOtp) : null,
                PhoneOtp = source.PhoneOtp is { } phoneOtp ? ToApi(phoneOtp) : null,
            };
        }

        internal static ConnectionPasswordAuthenticationMethod ToApi(V2alpha3ConnectionPasswordAuthenticationMethod source)
        {
            return new ConnectionPasswordAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static ConnectionPasskeyAuthenticationMethod ToApi(V2alpha3ConnectionPasskeyAuthenticationMethod source)
        {
            return new ConnectionPasskeyAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static ConnectionEmailOtpAuthenticationMethod ToApi(V2alpha3ConnectionEmailOtpAuthenticationMethod source)
        {
            return new ConnectionEmailOtpAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static ConnectionPhoneOtpAuthenticationMethod ToApi(V2alpha3ConnectionPhoneOtpAuthenticationMethod source)
        {
            return new ConnectionPhoneOtpAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static ConnectionPasskeyOptions ToApi(V2alpha3ConnectionPasskeyOptions source)
        {
            return new ConnectionPasskeyOptions
            {
                ChallengeUi = source.ChallengeUi is { } challengeUi ? ToApi(challengeUi) : null,
                ProgressiveEnrollmentEnabled = source.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = source.LocalEnrollmentEnabled,
            };
        }

        internal static ConnectionPasswordOptions ToApi(V2alpha3ConnectionPasswordOptions source)
        {
            return new ConnectionPasswordOptions
            {
                Complexity = source.Complexity is { } complexity ? ToApi(complexity) : null,
                Dictionary = source.Dictionary is { } dictionary ? ToApi(dictionary) : null,
                History = source.History is { } history ? ToApi(history) : null,
                ProfileData = source.ProfileData is { } profileData ? ToApi(profileData) : null,
            };
        }

        internal static ConnectionPasswordOptionsComplexity ToApi(V2alpha3ConnectionPasswordOptionsComplexity source)
        {
            return new ConnectionPasswordOptionsComplexity
            {
                MinLength = source.MinLength,
            };
        }

        internal static ConnectionPasswordOptionsDictionary ToApi(V2alpha3ConnectionPasswordOptionsDictionary source)
        {
            return new ConnectionPasswordOptionsDictionary
            {
                Active = source.Active,
                Custom = source.Custom,
            };
        }

        internal static ConnectionPasswordOptionsHistory ToApi(V2alpha3ConnectionPasswordOptionsHistory source)
        {
            return new ConnectionPasswordOptionsHistory
            {
                Active = source.Active,
                Size = source.Size,
            };
        }

        internal static ConnectionPasswordOptionsProfileData ToApi(V2alpha3ConnectionPasswordOptionsProfileData source)
        {
            return new ConnectionPasswordOptionsProfileData
            {
                Active = source.Active,
                BlockedFields = source.BlockedFields,
            };
        }

        internal static ConnectionPasswordHistoryOptions ToApi(V2alpha3ConnectionPasswordHistoryOptions source)
        {
            return new ConnectionPasswordHistoryOptions
            {
                Enable = source.Enable ?? false,
                Size = source.Size,
            };
        }

        internal static ConnectionPasswordNoPersonalInfoOptions ToApi(V2alpha3ConnectionPasswordNoPersonalInfoOptions source)
        {
            return new ConnectionPasswordNoPersonalInfoOptions
            {
                Enable = source.Enable ?? false,
            };
        }

        internal static ConnectionPasswordDictionaryOptions ToApi(V2alpha3ConnectionPasswordDictionaryOptions source)
        {
            return new ConnectionPasswordDictionaryOptions
            {
                Enable = source.Enable ?? false,
                Dictionary = source.Dictionary,
            };
        }

        internal static ConnectionPasswordComplexityOptions ToApi(V2alpha3ConnectionPasswordComplexityOptions source)
        {
            return new ConnectionPasswordComplexityOptions
            {
                MinLength = source.MinLength,
            };
        }

        internal static ConnectionValidationOptions ToApi(V2alpha3ConnectionValidationOptions source)
        {
            var target = new ConnectionValidationOptions();
            if (source.Username is { } username)
                target.Username = ToApi(username);

            return target;
        }

        internal static ConnectionUsernameValidationOptions ToApi(V2alpha3ConnectionUsernameValidationOptions source)
        {
            return new ConnectionUsernameValidationOptions
            {
                Min = source.Min ?? 0,
                Max = source.Max ?? 0,
            };
        }

        internal static ConnectionMfa ToApi(V2alpha3ConnectionMfa source)
        {
            return new ConnectionMfa
            {
                Active = source.Active,
                ReturnEnrollSettings = source.ReturnEnrollSettings,
            };
        }

        internal static ConnectionFederatedConnectionsAccessTokens ToApi(V2alpha3ConnectionFederatedConnectionsAccessTokens source)
        {
            return new ConnectionFederatedConnectionsAccessTokens
            {
                Active = source.Active,
            };
        }

        internal static ConnectionOptionsOidcMetadata ToApi(V2alpha3ConnectionOptionsOidcMetadata source)
        {
            return new ConnectionOptionsOidcMetadata
            {
                AcrValuesSupported = source.AcrValuesSupported,
                AuthorizationEndpoint = source.AuthorizationEndpoint,
                ClaimTypesSupported = source.ClaimTypesSupported,
                ClaimsLocalesSupported = source.ClaimsLocalesSupported,
                ClaimsParameterSupported = source.ClaimsParameterSupported,
                ClaimsSupported = source.ClaimsSupported,
                DisplayValuesSupported = source.DisplayValuesSupported,
                DpopSigningAlgValuesSupported = source.DpopSigningAlgValuesSupported,
                EndSessionEndpoint = source.EndSessionEndpoint,
                GrantTypesSupported = source.GrantTypesSupported,
                IdTokenEncryptionAlgValuesSupported = source.IdTokenEncryptionAlgValuesSupported,
                IdTokenEncryptionEncValuesSupported = source.IdTokenEncryptionEncValuesSupported,
                IdTokenSigningAlgValuesSupported = source.IdTokenSigningAlgValuesSupported,
                Issuer = source.Issuer,
                JwksUri = source.JwksUri,
                OpPolicyUri = source.OpPolicyUri,
                OpTosUri = source.OpTosUri,
                RegistrationEndpoint = source.RegistrationEndpoint,
                RequestObjectEncryptionAlgValuesSupported = source.RequestObjectEncryptionAlgValuesSupported,
                RequestObjectEncryptionEncValuesSupported = source.RequestObjectEncryptionEncValuesSupported,
                RequestObjectSigningAlgValuesSupported = source.RequestObjectSigningAlgValuesSupported,
                RequestParameterSupported = source.RequestParameterSupported,
                RequestUriParameterSupported = source.RequestUriParameterSupported,
                RequireRequestUriRegistration = source.RequireRequestUriRegistration,
                ResponseModesSupported = source.ResponseModesSupported,
                ResponseTypesSupported = source.ResponseTypesSupported,
                ServiceDocumentation = source.ServiceDocumentation,
                SubjectTypesSupported = source.SubjectTypesSupported,
                TokenEndpoint = source.TokenEndpoint,
                TokenEndpointAuthMethodsSupported = source.TokenEndpointAuthMethodsSupported,
                TokenEndpointAuthSigningAlgValuesSupported = source.TokenEndpointAuthSigningAlgValuesSupported,
                UiLocalesSupported = source.UiLocalesSupported,
                UserinfoEncryptionAlgValuesSupported = source.UserinfoEncryptionAlgValuesSupported,
                UserinfoEncryptionEncValuesSupported = source.UserinfoEncryptionEncValuesSupported,
                UserinfoEndpoint = source.UserinfoEndpoint,
                UserinfoSigningAlgValuesSupported = source.UserinfoSigningAlgValuesSupported,
                ScopesSupported = source.ScopesSupported is { } scopesSupported ? Optional<IEnumerable<string>?>.Of(scopesSupported) : default,
            };
        }

        internal static Optional<IEnumerable<ConnectionIdTokenSignedResponseAlgEnum>?> ToApi(IEnumerable<V2alpha3ConnectionIdTokenSignedResponseAlgEnum> source)
        {
            return Optional<IEnumerable<ConnectionIdTokenSignedResponseAlgEnum>?>.Of(source.Select(ToApi));
        }

        internal static ConnectionAttributeMapOidc ToApi(V2alpha3ConnectionAttributeMapOidc source)
        {
            return new ConnectionAttributeMapOidc
            {
                MappingMode = ToApi(source.MappingMode),
                UserinfoScope = source.UserinfoScope,
                Attributes = source.Attributes?.Where(kv => kv.Value is not null).ToDictionary(kv => kv.Key, kv => kv.Value),
            };
        }

        internal static ConnectionConnectionSettings ToApi(V2alpha3ConnectionConnectionSettings source)
        {
            return new ConnectionConnectionSettings
            {
                Pkce = ToApi(source.Pkce),
            };
        }

        internal static ConnectionAttributeMapOkta ToApi(V2alpha3ConnectionAttributeMapOkta source)
        {
            return new ConnectionAttributeMapOkta
            {
                MappingMode = ToApi(source.MappingMode),
                UserinfoScope = source.UserinfoScope,
                Attributes = source.Attributes?.Where(kv => kv.Value is not null).ToDictionary(kv => kv.Key, kv => kv.Value),
            };
        }

        internal static ConnectionOptionsIdpinitiatedSaml ToApi(V2alpha3ConnectionOptionsIdpinitiatedSaml source)
        {
            return new ConnectionOptionsIdpinitiatedSaml
            {
                ClientId = source.ClientId,
                ClientProtocol = source.ClientProtocol is { } clientProtocol ? ToApi(clientProtocol) : null,
                ClientAuthorizequery = source.ClientAuthorizequery,
            };
        }

        internal static ConnectionAssertionDecryptionSettings? ToApi(V2alpha3ConnectionAssertionDecryptionSettings source)
        {
            if (source.AlgorithmProfile is not { } algorithmProfile)
                return null;

            return new ConnectionAssertionDecryptionSettings
            {
                AlgorithmProfile = ToApi(algorithmProfile),
                AlgorithmExceptions = source.AlgorithmExceptions,
            };
        }

        internal static ConnectionSigningKeySaml ToApi(V2alpha3ConnectionSigningKeySaml source)
        {
            return new ConnectionSigningKeySaml
            {
                Key = source.Key,
                Cert = source.Cert,
            };
        }

        internal static ConnectionGatewayAuthenticationSms ToApi(V2alpha3ConnectionGatewayAuthenticationSms source)
        {
            var target = new ConnectionGatewayAuthenticationSms
            {
                Method = source.Method ?? string.Empty,
                Audience = source.Audience ?? string.Empty,
                Secret = source.Secret ?? string.Empty,
            };

            if (source.Subject is { } subject)
                target.Subject = subject;

            if (source.SecretBase64Encoded is { } secretBase64Encoded)
                target.SecretBase64Encoded = secretBase64Encoded;

            return target;
        }

        internal static ConnectionOptionsAuth0 ToApi(V2alpha3ConnectionOptionsAuth0 source)
        {
            var target = new ConnectionOptionsAuth0();
            if (source.Attributes is { } attributes)
                target.Attributes = ToApi(attributes);
            if (source.AuthenticationMethods is { } authenticationMethods)
                target.AuthenticationMethods = ToApi(authenticationMethods);
            target.BruteForceProtection = source.BruteForceProtection;
            if (source.Configuration is { } configuration)
                target.Configuration = configuration.ToDictionary(kv => kv.Key, kv => kv.Value);
            target.DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword;
            target.DisableSignup = source.DisableSignup;
            target.EnableScriptContext = source.EnableScriptContext;
            target.EnabledDatabaseCustomization = source.EnabledDatabaseCustomization;
            target.ImportMode = source.ImportMode;
            if (source.PasskeyOptions is { } passkeyOptions)
                target.PasskeyOptions = ToApi(passkeyOptions);
            if (source.PasswordOptions is { } passwordOptions)
                target.PasswordOptions = ToApi(passwordOptions);
            if (source.Precedence is { } precedence)
                target.Precedence = precedence.Select(i => ToApi(i)).ToArray();
            target.RealmFallback = source.RealmFallback;
            target.RequiresUsername = source.RequiresUsername;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.PasswordPolicy is { } pp)
                target.PasswordPolicy = ToApi(pp);
            if (source.PasswordHistory is { } ph)
                target.PasswordHistory = ToApi(ph);
            if (source.PasswordNoPersonalInfo is { } pnpi)
                target.PasswordNoPersonalInfo = ToApi(pnpi);
            if (source.PasswordDictionary is { } pd)
                target.PasswordDictionary = ToApi(pd);
            if (source.PasswordComplexityOptions is { } pco)
                target.PasswordComplexityOptions = ToApi(pco);
            if (source.Validation is { } val)
                target.Validation = ToApi(val);
            if (source.CustomScripts is { } cs) { target.CustomScripts ??= new ConnectionCustomScripts(); ApplyToApi(cs, target.CustomScripts); }
            if (source.Mfa is { } mfa)
                target.Mfa = ToApi(mfa);
            return target;
        }

        internal static ConnectionOptionsAd ToApi(V2alpha3ConnectionOptionsAd source)
        {
            var target = new ConnectionOptionsAd();
            target.AgentIp = source.AgentIp;
            target.AgentMode = source.AgentMode;
            target.AgentVersion = source.AgentVersion;
            target.BruteForceProtection = source.BruteForceProtection;
            target.CertAuth = source.CertAuth;
            if (source.Certs is { } certs)
                target.Certs = certs;
            target.DisableCache = source.DisableCache;
            target.DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword;
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.IconUrl = source.IconUrl;
            if (source.Ips is { } ips)
                target.Ips = ips;
            target.SignInEndpoint = source.SignInEndpoint;
            target.TenantDomain = source.TenantDomain;
            if (source.Thumbprints is { } tp)
                target.Thumbprints = tp;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.Kerberos is { } kb)
                target.Kerberos = kb;
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsAdfs ToApi(V2alpha3ConnectionOptionsAdfs source)
        {
            var target = new ConnectionOptionsAdfs();
            target.AdfsServer = source.AdfsServer;
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.EntityId = source.EntityId;
            target.FedMetadataXml = source.FedMetadataXml;
            target.IconUrl = source.IconUrl;
            if (source.PrevThumbprints is { } pt)
                target.PrevThumbprints = pt;
            if (source.ShouldTrustEmailVerifiedConnection is not null)
                target.ShouldTrustEmailVerifiedConnection = ToApi(source.ShouldTrustEmailVerifiedConnection);
            target.SignInEndpoint = source.SignInEndpoint;
            target.TenantDomain = source.TenantDomain;
            if (source.Thumbprints is { } tp)
                target.Thumbprints = tp;
            target.UserIdAttribute = source.UserIdAttribute;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsAmazon ToApi(V2alpha3ConnectionOptionsAmazon source)
        {
            var target = new ConnectionOptionsAmazon();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            target.PostalCode = source.PostalCode;
            target.Profile = source.Profile;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsApple ToApi(V2alpha3ConnectionOptionsApple source)
        {
            var target = new ConnectionOptionsApple();
            target.AppSecret = source.AppSecret;
            target.ClientId = source.ClientId;
            target.Email = source.Email;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            target.Kid = source.Kid;
            target.Name = source.Name;
            target.Scope = source.Scope;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            target.TeamId = source.TeamId;
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsAuth0Oidc ToApi(V2alpha3ConnectionOptionsAuth0Oidc source)
        {
            var target = new ConnectionOptionsAuth0Oidc();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            return target;
        }

        internal static ConnectionOptionsBaidu ToApi(V2alpha3ConnectionOptionsBaidu source)
        {
            var target = new ConnectionOptionsBaidu();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsAzureAd ToApi(V2alpha3ConnectionOptionsAzureAd source)
        {
            var target = new ConnectionOptionsAzureAd { ClientId = source.ClientId, ClientSecret = source.ClientSecret };
            target.ApiEnableUsers = source.ApiEnableUsers;
            target.AppDomain = source.AppDomain;
            target.AppId = source.AppId;
            target.BasicProfile = source.BasicProfile;
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
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
            if (source.FederatedConnectionsAccessTokens is { } federatedConnectionsAccessTokens)
                target.FederatedConnectionsAccessTokens = ToApi(federatedConnectionsAccessTokens);
            target.Granted = source.Granted;
            target.IconUrl = source.IconUrl;
            if (source.IdentityApi is not null)
                target.IdentityApi = ToApi(source.IdentityApi);
            target.MaxGroupsToRetrieve = source.MaxGroupsToRetrieve;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.ShouldTrustEmailVerifiedConnection is not null)
                target.ShouldTrustEmailVerifiedConnection = ToApi(source.ShouldTrustEmailVerifiedConnection);
            target.TenantDomain = source.TenantDomain;
            target.TenantId = source.TenantId;
            if (source.Thumbprints is { } tp)
                target.Thumbprints = tp;
            target.UseCommonEndpoint = source.UseCommonEndpoint;
            target.UseWsfed = source.UseWsfed;
            if (source.UseridAttribute is not null)
                target.UseridAttribute = ToApi(source.UseridAttribute);
            if (source.WaadProtocol is not null)
                target.WaadProtocol = ToApi(source.WaadProtocol);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsBitbucket ToApi(V2alpha3ConnectionOptionsBitbucket source)
        {
            var target = new ConnectionOptionsBitbucket();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            return target;
        }

        internal static ConnectionOptionsBitly ToApi(V2alpha3ConnectionOptionsBitly source)
        {
            var target = new ConnectionOptionsBitly();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsBox ToApi(V2alpha3ConnectionOptionsBox source)
        {
            var target = new ConnectionOptionsBox();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsDaccount ToApi(V2alpha3ConnectionOptionsDaccount source)
        {
            var target = new ConnectionOptionsDaccount();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsDropbox ToApi(V2alpha3ConnectionOptionsDropbox source)
        {
            var target = new ConnectionOptionsDropbox();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsDwolla ToApi(V2alpha3ConnectionOptionsDwolla source)
        {
            var target = new ConnectionOptionsDwolla();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsEmail ToApi(V2alpha3ConnectionOptionsEmail source)
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
                    Syntax = ToApi(e.Syntax),
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

        internal static ConnectionOptionsEvernote ToApi(V2alpha3ConnectionOptionsEvernote source)
        {
            var target = new ConnectionOptionsEvernote();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsExact ToApi(V2alpha3ConnectionOptionsExact source)
        {
            var target = new ConnectionOptionsExact();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsFacebook ToApi(V2alpha3ConnectionOptionsFacebook source)
        {
            var target = new ConnectionOptionsFacebook();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
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
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalPropertiesNonOptional(up);
            return target;
        }

        internal static ConnectionOptionsFitbit ToApi(V2alpha3ConnectionOptionsFitbit source)
        {
            var target = new ConnectionOptionsFitbit();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsGitHub ToApi(V2alpha3ConnectionOptionsGitHub source)
        {
            var target = new ConnectionOptionsGitHub();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
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
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsInstagram ToApi(V2alpha3ConnectionOptionsInstagram source)
        {
            var target = new ConnectionOptionsInstagram();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsLine ToApi(V2alpha3ConnectionOptionsLine source)
        {
            var target = new ConnectionOptionsLine();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            target.Email = source.Email;
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsGoogleApps ToApi(V2alpha3ConnectionOptionsGoogleApps source)
        {
            var target = new ConnectionOptionsGoogleApps { ClientId = source.ClientId, ClientSecret = source.ClientSecret };
            if (source.Scope is { } scope)
                target.Scope = scope;
            target.Domain = source.Domain;
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            target.IconUrl = source.IconUrl;
            target.Email = source.Email;
            target.Profile = source.Profile;
            target.ApiEnableUsers = source.ApiEnableUsers;
            target.MapUserIdToId = source.MapUserIdToId;
            target.AdminAccessToken = source.AdminAccessToken;
            target.AdminAccessTokenExpiresin = source.AdminAccessTokenExpiresin;
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
                target.FederatedConnectionsAccessTokens = new ConnectionFederatedConnectionsAccessTokens { Active = fcat.Active };
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsPlanningCenter ToApi(V2alpha3ConnectionOptionsPlanningCenter source)
        {
            var target = new ConnectionOptionsPlanningCenter();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsSharepoint ToApi(V2alpha3ConnectionOptionsSharepoint source)
        {
            var target = new ConnectionOptionsSharepoint();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsShop ToApi(V2alpha3ConnectionOptionsShop source)
        {
            var target = new ConnectionOptionsShop();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsShopify ToApi(V2alpha3ConnectionOptionsShopify source)
        {
            var target = new ConnectionOptionsShopify();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsSoundcloud ToApi(V2alpha3ConnectionOptionsSoundcloud source)
        {
            var target = new ConnectionOptionsSoundcloud();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsThirtySevenSignals ToApi(V2alpha3ConnectionOptionsThirtySevenSignals source)
        {
            var target = new ConnectionOptionsThirtySevenSignals();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsUntappd ToApi(V2alpha3ConnectionOptionsUntappd source)
        {
            var target = new ConnectionOptionsUntappd();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsVkontakte ToApi(V2alpha3ConnectionOptionsVkontakte source)
        {
            var target = new ConnectionOptionsVkontakte();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsWeibo ToApi(V2alpha3ConnectionOptionsWeibo source)
        {
            var target = new ConnectionOptionsWeibo();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsWordpress ToApi(V2alpha3ConnectionOptionsWordpress source)
        {
            var target = new ConnectionOptionsWordpress();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsYandex ToApi(V2alpha3ConnectionOptionsYandex source)
        {
            var target = new ConnectionOptionsYandex();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = ConnectionScopeOAuth2.FromListOfString(scope);
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            return target;
        }

        internal static ConnectionOptionsGoogleOAuth2 ToApi(V2alpha3ConnectionOptionsGoogleOAuth2 source)
        {
            var target = new ConnectionOptionsGoogleOAuth2();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            target.IconUrl = source.IconUrl;
            if (source.AllowedAudiences is { } aa)
                target.AllowedAudiences = aa;
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
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsLinkedin ToApi(V2alpha3ConnectionOptionsLinkedin source)
        {
            var target = new ConnectionOptionsLinkedin();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            target.BasicProfile = source.BasicProfile;
            target.Email = source.Email;
            target.FullProfile = source.FullProfile;
            target.Network = source.Network;
            target.Openid = source.Openid;
            target.Profile = source.Profile;
            target.StrategyVersion = source.StrategyVersion;
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsOAuth1 ToApi(V2alpha3ConnectionOptionsOAuth1 source)
        {
            var target = new ConnectionOptionsOAuth1();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            target.AccessTokenUrl = source.AccessTokenUrl;
            target.RequestTokenUrl = source.RequestTokenUrl;
            if (source.SignatureMethod is not null)
                target.SignatureMethod = ToApi(source.SignatureMethod);
            target.UserAuthorizationUrl = source.UserAuthorizationUrl;
            if (source.Scripts is { } sc)
                target.Scripts = new ConnectionScriptsOAuth1 { FetchUserProfile = sc.FetchUserProfile };
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsOAuth2 ToApi(V2alpha3ConnectionOptionsOAuth2 source)
        {
            var target = new ConnectionOptionsOAuth2();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            target.AuthorizationUrl = source.AuthorizationUrl;
            target.TokenUrl = source.TokenUrl;
            target.LogoutUrl = source.LogoutUrl;
            if (source.Scope is { } scope)
                target.Scope = string.Join(" ", scope);
            target.IconUrl = source.IconUrl;
            target.PkceEnabled = source.PkceEnabled;
            target.UseOauthSpecScope = source.UseOauthSpecScope;
            if (source.Scripts is { } sc)
                target.Scripts = new ConnectionScriptsOAuth2 { FetchUserProfile = sc.FetchUserProfile };
            if (source.AuthParams is { } ap)
                target.AuthParams = ap.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.AuthParamsMap is { } apm)
                target.AuthParamsMap = apm.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.FieldsMap is { } fm)
                target.FieldsMap = fm.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.CustomHeaders is { } ch)
                target.CustomHeaders = ch.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsOffice365 ToApi(V2alpha3ConnectionOptionsOffice365 source)
        {
            var target = new ConnectionOptionsOffice365();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            return target;
        }

        internal static ConnectionOptionsOidc ToApi(V2alpha3ConnectionOptionsOidc source)
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
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            if (source.TokenEndpointAuthMethod is not null)
                target.TokenEndpointAuthMethod = ToApi(source.TokenEndpointAuthMethod);
            if (source.TokenEndpointAuthSigningAlg is not null)
                target.TokenEndpointAuthSigningAlg = ToApi(source.TokenEndpointAuthSigningAlg);
            if (source.TokenEndpointJwtcaAudFormat is not null)
                target.TokenEndpointJwtcaAudFormat = ToApi(source.TokenEndpointJwtcaAudFormat);
            if (source.DpopSigningAlg is not null)
                target.DpopSigningAlg = ToApi(source.DpopSigningAlg);
            target.SendBackChannelNonce = source.SendBackChannelNonce;
            if (source.Type is not null)
                target.Type = ToApi(source.Type);
            if (source.OidcMetadata is { } oidcMetadata)
                target.OidcMetadata = ToApi(oidcMetadata);
            if (source.IdTokenSignedResponseAlgs is { } algs)
                target.IdTokenSignedResponseAlgs = ToApi(algs);
            if (source.AttributeMap is { } am)
                target.AttributeMap = ToApi(am);
            if (source.ConnectionSettings is { } cs)
                target.ConnectionSettings = ToApi(cs);
            if (source.FederatedConnectionsAccessTokens is { } fcat)
                target.FederatedConnectionsAccessTokens = ToApi(fcat);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsOkta ToApi(V2alpha3ConnectionOptionsOkta source)
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
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            if (source.TokenEndpointAuthMethod is not null)
                target.TokenEndpointAuthMethod = ToApi(source.TokenEndpointAuthMethod);
            if (source.TokenEndpointAuthSigningAlg is not null)
                target.TokenEndpointAuthSigningAlg = ToApi(source.TokenEndpointAuthSigningAlg);
            if (source.TokenEndpointJwtcaAudFormat is not null)
                target.TokenEndpointJwtcaAudFormat = ToApi(source.TokenEndpointJwtcaAudFormat);
            if (source.DpopSigningAlg is not null)
                target.DpopSigningAlg = ToApi(source.DpopSigningAlg);
            target.SendBackChannelNonce = source.SendBackChannelNonce;
            if (source.Type is not null)
                target.Type = ToApi(source.Type);
            if (source.OidcMetadata is { } oidcMetadata)
                target.OidcMetadata = ToApi(oidcMetadata);
            if (source.IdTokenSignedResponseAlgs is { } algs)
                target.IdTokenSignedResponseAlgs = ToApi(algs);
            if (source.AttributeMap is { } am)
                target.AttributeMap = ToApi(am);
            if (source.ConnectionSettings is { } cs)
                target.ConnectionSettings = ToApi(cs);
            if (source.FederatedConnectionsAccessTokens is { } fcat)
                target.FederatedConnectionsAccessTokens = ToApi(fcat);
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsPaypal ToApi(V2alpha3ConnectionOptionsPaypal source)
        {
            var target = new ConnectionOptionsPaypal();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            target.Address = source.Address;
            target.Email = source.Email;
            target.Phone = source.Phone;
            target.Profile = source.Profile;
            return target;
        }

        internal static ConnectionOptionsPingFederate ToApi(V2alpha3ConnectionOptionsPingFederate source)
        {
            var target = new ConnectionOptionsPingFederate { PingFederateBaseUrl = source.PingFederateBaseUrl };
            target.SignInEndpoint = source.SignInEndpoint;
            target.EntityId = source.EntityId;
            target.Cert = source.Cert;
            target.SigningCert = source.SigningCert;
            if (source.DecryptionKey is { } decryptionKey)
                target.DecryptionKey = ToApi(decryptionKey);
            if (source.Thumbprints is { } tp)
                target.Thumbprints = tp;
            if (source.SignatureAlgorithm is { } sigAlg)
                target.SignatureAlgorithm = ToApi(sigAlg);
            if (source.DigestAlgorithm is { } digAlg)
                target.DigestAlgorithm = ToApi(digAlg);
            if (source.ProtocolBinding is { } pb)
                target.ProtocolBinding = ToApi(pb);
            target.SignSamlRequest = source.SignSamlRequest;
            if (source.Idpinitiated is { } idp)
                target.Idpinitiated = ToApi(idp);
            if (source.AssertionDecryptionSettings is { } ads)
                target.AssertionDecryptionSettings = ToApi(ads);
            target.IconUrl = source.IconUrl;
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsSalesforce ToApi(V2alpha3ConnectionOptionsSalesforce source)
        {
            var target = new ConnectionOptionsSalesforce();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsSalesforceCommunity ToApi(V2alpha3ConnectionOptionsSalesforceCommunity source)
        {
            var target = new ConnectionOptionsSalesforceCommunity();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            target.CommunityBaseUrl = source.CommunityBaseUrl;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            target.Profile = source.Profile;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsSaml ToApi(V2alpha3ConnectionOptionsSaml source)
        {
            var target = new ConnectionOptionsSaml();
            target.SignInEndpoint = source.SignInEndpoint;
            target.SignOutEndpoint = source.SignOutEndpoint;
            target.DisableSignout = source.DisableSignout;
            target.DestinationUrl = source.DestinationUrl;
            target.RecipientUrl = source.RecipientUrl;
            target.Cert = source.Cert;
            if (source.Thumbprints is { } tp)
                target.Thumbprints = tp;
            target.MetadataUrl = source.MetadataUrl;
            target.MetadataXml = source.MetadataXml;
            target.EntityId = source.EntityId;
            target.SignSamlRequest = source.SignSamlRequest;
            if (source.SignatureAlgorithm is { } sigAlg)
                target.SignatureAlgorithm = ToApi(sigAlg);
            if (source.DigestAlgorithm is { } digAlg)
                target.DigestAlgorithm = ToApi(digAlg);
            if (source.ProtocolBinding is { } pb)
                target.ProtocolBinding = ToApi(pb);
            target.RequestTemplate = source.RequestTemplate;
            target.Debug = source.Debug;
            target.Deflate = source.Deflate;
            if (source.Idpinitiated is { } idp)
                target.Idpinitiated = ToApi(idp);
            target.SigningCert = source.SigningCert;
            if (source.SigningKey is { } sk)
                target.SigningKey = ToApi(sk);
            if (source.DecryptionKey is { } decryptionKey)
                target.DecryptionKey = ToApi(decryptionKey);
            if (source.AssertionDecryptionSettings is { } ads)
                target.AssertionDecryptionSettings = ToApi(ads);
            target.UserIdAttribute = source.UserIdAttribute;
            target.IconUrl = source.IconUrl;
            if (source.DomainAliases is { } da)
                target.DomainAliases = da;
            target.TenantDomain = source.TenantDomain;
            target.GlobalTokenRevocationJwtIss = source.GlobalTokenRevocationJwtIss;
            target.GlobalTokenRevocationJwtSub = source.GlobalTokenRevocationJwtSub;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsSms ToApi(V2alpha3ConnectionOptionsSms source)
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
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.Totp is { } t)
                target.Totp = new ConnectionTotpSms { Length = t.Length, TimeStep = t.TimeStep };
            if (source.GatewayAuthentication is { } ga)
                target.GatewayAuthentication = ToApi(ga);
            return target;
        }

        internal static ConnectionOptionsTwitter ToApi(V2alpha3ConnectionOptionsTwitter source)
        {
            var target = new ConnectionOptionsTwitter();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.Protocol is not null)
                target.Protocol = ToApi(source.Protocol);
            target.OfflineAccess = source.OfflineAccess;
            target.Profile = source.Profile;
            target.TweetRead = source.TweetRead;
            target.UsersRead = source.UsersRead;
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsWindowsLive ToApi(V2alpha3ConnectionOptionsWindowsLive source)
        {
            var target = new ConnectionOptionsWindowsLive();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.Scope is { } scope)
                target.Scope = scope;
            if (source.FreeformScopes is { } ffs)
                target.FreeformScopes = ffs;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            target.Basic = source.Basic;
            target.OfflineAccess = source.OfflineAccess;
            target.Signin = source.Signin;
            target.Birthday = source.Birthday;
            target.Calendars = source.Calendars;
            target.CalendarsUpdate = source.CalendarsUpdate;
            target.ContactsBirthday = source.ContactsBirthday;
            target.ContactsCreate = source.ContactsCreate;
            target.ContactsCalendars = source.ContactsCalendars;
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
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        internal static ConnectionOptionsYahoo ToApi(V2alpha3ConnectionOptionsYahoo source)
        {
            var target = new ConnectionOptionsYahoo();
            target.ClientId = source.ClientId;
            target.ClientSecret = source.ClientSecret;
            if (source.NonPersistentAttrs is { } npa)
                target.NonPersistentAttrs = npa;
            if (source.SetUserRootAttributes is not null)
                target.SetUserRootAttributes = ToApi(source.SetUserRootAttributes);
            if (source.UpstreamParams is { } up)
                target.UpstreamParams = ToApiUpstreamAdditionalProperties(up);
            return target;
        }

        /// <summary>
        /// Gets the list of enabled client IDs
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
        protected override async Task<V2alpha3ConnectionConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
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
            catch (NotFoundError)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V2alpha3Connection entity, V2alpha3Connection.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
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
                    catch (NotFoundError)
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
        protected override string? ValidateCreate(V2alpha3ConnectionConf conf)
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
        protected override async Task<string> Create(IManagementApiClient api, V2alpha3ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating connection in Auth0 with name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, conf.Name, conf.Strategy);

            if (conf.Strategy is null)
                throw new InvalidOperationException("Missing connection strategy.");

            var req = new CreateConnectionRequestContent()
            {
                Name = conf.Name ?? throw new InvalidOperationException("Missing connection name."),
                Strategy = ConnectionIdentityProviderEnum.FromCustom(ToApiStrategy(conf.Strategy.Value)),
            };

            ApplyToApi(conf, req);

            var self = await api.Connections.CreateAsync(req, null, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            Logger.LogInformation("{EntityTypeName} successfully created connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, self.Id, conf.Name, conf.Strategy);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V2alpha3ConnectionConf? last, V2alpha3ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);

            var req = new UpdateConnectionRequestContent();
            ApplyToApi(conf, req);

            await api.Connections.UpdateAsync(id, req, null, cancellationToken);

            Logger.LogInformation("{EntityTypeName} successfully updated connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
        }

        /// <summary>
        /// Resolves the strategy-specific options object for the given strategy name and options.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static object? ResolveStrategyOptions(V2alpha3ConnectionStrategy? strategy, V2alpha3ConnectionOptions? options) => strategy switch
        {
            V2alpha3ConnectionStrategy.Auth0 when options?.Auth0 is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Ad when options?.Ad is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Adfs when options?.Adfs is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Amazon when options?.Amazon is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Apple when options?.Apple is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Auth0Oidc when options?.Auth0Oidc is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.AzureAd when options?.AzureAd is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Baidu when options?.Baidu is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Bitbucket when options?.Bitbucket is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Bitly when options?.Bitly is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Box when options?.Box is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Daccount when options?.Daccount is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Dropbox when options?.Dropbox is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Dwolla when options?.Dwolla is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Email when options?.Email is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Evernote when options?.Evernote is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.EvernoteSandbox when options?.EvernoteSandbox is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Exact when options?.Exact is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Facebook when options?.Facebook is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Fitbit when options?.Fitbit is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.GitHub when options?.GitHub is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.GoogleApps when options?.GoogleApps is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.GoogleOAuth2 when options?.GoogleOAuth2 is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Instagram when options?.Instagram is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Line when options?.Line is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Linkedin when options?.Linkedin is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.OAuth1 when options?.OAuth1 is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.OAuth2 when options?.OAuth2 is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Office365 when options?.Office365 is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Oidc when options?.Oidc is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Okta when options?.Okta is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Paypal when options?.Paypal is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.PaypalSandbox when options?.PaypalSandbox is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.PingFederate when options?.PingFederate is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.PlanningCenter when options?.PlanningCenter is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Salesforce when options?.Salesforce is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.SalesforceCommunity when options?.SalesforceCommunity is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.SalesforceSandbox when options?.SalesforceSandbox is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Saml when options?.Saml is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Sharepoint when options?.Sharepoint is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Shop when options?.Shop is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Shopify when options?.Shopify is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Sms when options?.Sms is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Soundcloud when options?.Soundcloud is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.ThirtySevenSignals when options?.ThirtySevenSignals is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Twitter when options?.Twitter is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Untappd when options?.Untappd is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Vkontakte when options?.Vkontakte is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Weibo when options?.Weibo is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.WindowsLive when options?.WindowsLive is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Wordpress when options?.Wordpress is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Yahoo when options?.Yahoo is { } o => ToApi(o),
            V2alpha3ConnectionStrategy.Yandex when options?.Yandex is { } o => ToApi(o),
            _ => null,
        };

        /// <summary>
        /// Applies the specified configuration to the request object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        internal static void ApplyToApi(V2alpha3ConnectionConf source, CreateConnectionRequestContent target)
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
                target.Options = JsonSerializer.Deserialize<ConnectionPropertiesOptions>(JsonSerializer.Serialize(options, Auth0JsonSerializerOptions), Auth0JsonSerializerOptions);
        }

        internal static void ApplyToApi(V2alpha3ConnectionConf source, UpdateConnectionRequestContent target)
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
                target.Options = JsonSerializer.Deserialize<UpdateConnectionOptions>(JsonSerializer.Serialize(options, Auth0JsonSerializerOptions), Auth0JsonSerializerOptions);
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsEmailAttribute source, EmailAttribute target)
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

        static void ApplyToApi(V2alpha3ConnectionOptionsEmailSignup source, SignupVerified target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new SignupVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPhoneNumberAttribute source, PhoneAttribute target)
        {
            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupVerified();
                ApplyToApi(signup, target.Signup, true);
            }
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPhoneNumberSignup source, SignupVerified target, bool phone)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new SignupVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsUsernameAttribute source, UsernameAttribute target)
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

        static void ApplyToApi(V2alpha3ConnectionOptionsUsernameSignup source, SignupSchema target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsAttributeIdentifier source, ConnectionAttributeIdentifier target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsAttributeValidation source, UsernameValidation target)
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

        static void ApplyToApi(V2alpha3ConnectionOptionsAttributeAllowedTypes source, UsernameAllowedTypes target)
        {
            if (source.Email is { } email)
                target.Email = email;

            if (source.PhoneNumber is { } phoneNumber)
                target.PhoneNumber = phoneNumber;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsVerification source, SignupVerification target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V2alpha3ConnectionCustomScripts source, ConnectionCustomScripts target)
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

        static void ApplyToApi(V2alpha3ConnectionOptionsCustomScripts source, ConnectionCustomScripts target)
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

        static void ApplyToApi(V2alpha3ConnectionOptionsAuthenticationMethods source, ConnectionAuthenticationMethods target)
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

        static void ApplyToApi(V2alpha3ConnectionOptionsPasswordAuthenticationMethod source, ConnectionPasswordAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPasskeyAuthenticationMethod source, ConnectionPasskeyAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPasskeyOptions source, ConnectionPasskeyOptions target)
        {
            if (source.ChallengeUi is { } challengeUi)
                target.ChallengeUi = ToApi(challengeUi);

            if (source.ProgressiveEnrollmentEnabled is { } progressiveEnrollmentEnabled)
                target.ProgressiveEnrollmentEnabled = progressiveEnrollmentEnabled;

            if (source.LocalEnrollmentEnabled is { } localEnrollmentEnabled)
                target.LocalEnrollmentEnabled = localEnrollmentEnabled;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPasswordComplexityOptions source, ConnectionPasswordComplexityOptions target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPasswordHistory source, ConnectionPasswordHistoryOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Size is { } size)
                target.Size = size;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPasswordNoPersonalInfo source, ConnectionPasswordNoPersonalInfoOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;
        }

        static void ApplyToApi(V2alpha3ConnectionOptionsPasswordDictionary source, ConnectionPasswordDictionaryOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Dictionary is { } dictionary)
                target.Dictionary = dictionary;
        }

        static void ApplyToApi(V2alpha3ConnectionGatewayAuthentication source, ConnectionGatewayAuthentication target)
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

        static void ApplyToApi(V2alpha3ConnectionOptions source, ref dynamic target)
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

        /// <inheritdoc />
        protected override async Task ApplyStatus(IManagementApiClient api, V2alpha3Connection entity, V2alpha3ConnectionConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
        {
            // enabled-clients management is part of updating the connection; only apply it when Update is permitted
            if (((V1TenantEntityInstance<V2alpha3Connection.SpecDef, V2alpha3Connection.StatusDef, V2alpha3ConnectionConf, V2alpha3ConnectionConf>)entity).HasPolicy(V1EntityPolicyType.Update))
                await ApplyEnabledClientsAsync(api, entity, defaultNamespace, cancellationToken);

            await base.ApplyStatus(api, entity, lastConf, defaultNamespace, cancellationToken);
        }

        /// <summary>
        /// Reconciles the clients enabled on this connection via <c>conf.enabledClients</c>.
        /// This is scoped and additive: it enables every desired client and only disables clients this
        /// Connection previously enabled itself (tracked in <see cref="V2alpha3Connection.StatusDef.ManagedEnabledClientIds"/>).
        /// Clients enabled through separate <c>ConnectionClient</c> resources are never touched.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="entity"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task ApplyEnabledClientsAsync(IManagementApiClient api, V2alpha3Connection entity, string defaultNamespace, CancellationToken cancellationToken)
        {
            // a null enabledClients means this Connection does not manage enabled clients at all;
            // clear any previously-tracked managed set so the hand-off is clean and a later
            // re-adoption starts from an empty scope (never disabling clients out of a stale set)
            if (entity.Spec.Conf?.EnabledClients is not { } enabledClients)
            {
                entity.Status.ManagedEnabledClientIds = null;
                return;
            }

            var id = entity.Status.Id;
            if (string.IsNullOrWhiteSpace(id))
                return;

            // clients this Connection should have enabled
            var desired = new HashSet<string>(await ResolveClientRefsToIds(api, enabledClients, defaultNamespace, cancellationToken));

            // clients this Connection previously enabled itself; the only ones we are allowed to disable
            var previouslyManaged = entity.Status.ManagedEnabledClientIds ?? Array.Empty<string>();

            var req = ComputeEnabledClientsRequest(desired, previouslyManaged);
            if (req.Count > 0)
                await api.Connections.Clients.UpdateAsync(id, req, null, cancellationToken);

            // remember the set we now manage so future reconciles can scope disabling
            entity.Status.ManagedEnabledClientIds = desired.ToArray();
        }

        /// <summary>
        /// Computes the enabled-clients batch for a scoped, additive update: every <paramref name="desired"/> client is
        /// enabled, and only <paramref name="previouslyManaged"/> clients that are no longer desired are disabled.
        /// Clients that were never managed by this Connection (e.g. enabled via a <c>ConnectionClient</c> resource) are
        /// never included, so they are left untouched.
        /// </summary>
        /// <param name="desired"></param>
        /// <param name="previouslyManaged"></param>
        /// <returns></returns>
        internal static List<UpdateEnabledClientConnectionsRequestContentItem> ComputeEnabledClientsRequest(ISet<string> desired, IEnumerable<string> previouslyManaged)
        {
            var req = new List<UpdateEnabledClientConnectionsRequestContentItem>();

            // enable every desired client
            foreach (var clientId in desired)
                req.Add(new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = clientId, Status = true });

            // disable only previously-managed clients that are no longer desired
            foreach (var clientId in previouslyManaged)
                if (desired.Contains(clientId) == false)
                    req.Add(new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = clientId, Status = false });

            return req;
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
