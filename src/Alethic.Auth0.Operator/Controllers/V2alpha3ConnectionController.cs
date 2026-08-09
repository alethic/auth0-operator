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
        /// <see cref="V2alpha3ConnectionConf.ProvisioningTicketUrl"/> is populated for the enterprise strategies whose
        /// response types expose it; it is read-only status enrichment and stays excluded from drift comparison.
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
                    var ad = JsonConvertTo<ConnectionResponseContentAd>(source);
                    conf.Options.Ad = FromApi(ad?.Options);
                    conf.ProvisioningTicketUrl = ad?.ProvisioningTicketUrl;
                    break;
                case ConnectionResponseContentAdfsStrategy.Values.Adfs:
                    var adfs = JsonConvertTo<ConnectionResponseContentAdfs>(source);
                    conf.Options.Adfs = FromApi(adfs?.Options);
                    conf.ProvisioningTicketUrl = adfs?.ProvisioningTicketUrl;
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
                    var googleApps = JsonConvertTo<ConnectionResponseContentGoogleApps>(source);
                    conf.Options.GoogleApps = FromApi(googleApps?.Options);
                    conf.ProvisioningTicketUrl = googleApps?.ProvisioningTicketUrl;
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
                    var office365 = JsonConvertTo<ConnectionResponseContentOffice365>(source);
                    conf.Options.Office365 = FromApi(office365?.Options);
                    conf.ProvisioningTicketUrl = office365?.ProvisioningTicketUrl;
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
                    var pingFederate = JsonConvertTo<ConnectionResponseContentPingFederate>(source);
                    conf.Options.PingFederate = FromApi(pingFederate?.Options);
                    conf.ProvisioningTicketUrl = pingFederate?.ProvisioningTicketUrl;
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
                    var saml = JsonConvertTo<ConnectionResponseContentSaml>(source);
                    conf.Options.Saml = FromApi(saml?.Options);
                    conf.ProvisioningTicketUrl = saml?.ProvisioningTicketUrl;
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
                    var azureAd = JsonConvertTo<ConnectionResponseContentAzureAd>(source);
                    conf.Options.AzureAd = FromApi(azureAd?.Options);
                    conf.ProvisioningTicketUrl = azureAd?.ProvisioningTicketUrl;
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

        /// <summary>
        /// Determines whether the connection differs from the desired configuration by explicitly comparing every
        /// property transmitted on update. Only spec-managed (non-null) properties count: values Auth0 fills in
        /// server-side that the spec does not set can never force an update. Name and strategy are excluded because
        /// they are create-only (never sent on update; this also prevents a known environment name skew from forcing
        /// eternal PATCHes), provisioningTicketUrl is excluded because it is read-only and never sent on update, and
        /// enabledClients is excluded because it is reconciled separately with its own delta logic in
        /// ApplyEnabledClientsAsync.
        /// </summary>
        internal static bool ConfRequiresUpdate(V2alpha3ConnectionConf conf, V2alpha3ConnectionConf last)
        {
            if (conf.DisplayName is not null && conf.DisplayName != last.DisplayName)
                return true;

            if (RequiresUpdate(conf.Metadata, last.Metadata))
                return true;

            if (conf.Realms is not null && (last.Realms is null || conf.Realms.ToHashSet().SetEquals(last.Realms) == false))
                return true;

            if (conf.IsDomainConnection is not null && conf.IsDomainConnection != last.IsDomainConnection)
                return true;

            if (conf.ShowAsButton is not null && conf.ShowAsButton != last.ShowAsButton)
                return true;

            if (RequiresUpdate(conf.Options, last.Options))
                return true;

            return false;
        }

        /// <inheritdoc />
        protected override bool NeedsUpdate(V2alpha3ConnectionConf conf, V2alpha3ConnectionConf last)
        {
            return ConfRequiresUpdate(conf, last);
        }

        /// <summary>
        /// Determines whether any metadata entry on the desired configuration is missing from, or differs on, the
        /// last known configuration. Entries present only on the last known configuration are unmanaged and ignored.
        /// </summary>
        static bool RequiresUpdate(System.Collections.Hashtable? conf, System.Collections.Hashtable? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            foreach (System.Collections.DictionaryEntry entry in conf)
                if (last.ContainsKey(entry.Key) == false || Equals(entry.Value?.ToString(), last[entry.Key]?.ToString()) == false)
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether any string dictionary entry on the desired configuration is missing from, or differs
        /// on, the last known configuration. Entries present only on the last known configuration are ignored.
        /// </summary>
        static bool RequiresUpdate(Dictionary<string, string>? conf, Dictionary<string, string>? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            foreach (var entry in conf)
                if (last.TryGetValue(entry.Key, out var value) == false || entry.Value != value)
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether any attribute map entry on the desired configuration is missing from, or differs on,
        /// the last known configuration. The values are opaque JSON fragments carried through the model untyped, so
        /// they are compared by their textual form. Entries present only on the last known configuration are ignored.
        /// </summary>
        static bool RequiresUpdate(Dictionary<string, object?>? conf, Dictionary<string, object?>? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            foreach (var entry in conf)
                if (last.TryGetValue(entry.Key, out var value) == false || Equals(entry.Value?.ToString(), value?.ToString()) == false)
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether any fields map entry on the desired configuration is missing from, or differs on, the
        /// last known configuration. Entries present only on the last known configuration are ignored.
        /// </summary>
        static bool RequiresUpdate(Dictionary<string, string[]?>? conf, Dictionary<string, string[]?>? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            foreach (var entry in conf)
            {
                if (last.TryGetValue(entry.Key, out var value) == false)
                    return true;

                // candidate attribute names are tried in order, so order is semantic here
                if (entry.Value is not null && (value is null || entry.Value.SequenceEqual(value) == false))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether any upstream parameter on the desired configuration is missing from, or differs on,
        /// the last known configuration. Entries present only on the last known configuration are ignored.
        /// </summary>
        static bool RequiresUpdate(Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? conf, Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            foreach (var entry in conf)
                if (last.TryGetValue(entry.Key, out var value) == false || RequiresUpdate(entry.Value, value))
                    return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionUpstreamAdditionalProperties? conf, V2alpha3ConnectionUpstreamAdditionalProperties? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Alias is not null && conf.Alias != last.Alias)
                return true;

            if (conf.Value is not null && conf.Value != last.Value)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionFederatedConnectionsAccessTokens? conf, V2alpha3ConnectionFederatedConnectionsAccessTokens? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether any strategy-specific options tree present on the desired configuration differs from
        /// the last known configuration. Each strategy member set on the spec is compared explicitly; members the
        /// spec leaves null are unmanaged and ignored.
        /// </summary>
        static bool RequiresUpdate(V2alpha3ConnectionOptions? conf, V2alpha3ConnectionOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Auth0, last.Auth0))
                return true;

            if (RequiresUpdate(conf.Ad, last.Ad))
                return true;

            if (RequiresUpdate(conf.Adfs, last.Adfs))
                return true;

            if (RequiresUpdate(conf.Amazon, last.Amazon))
                return true;

            if (RequiresUpdate(conf.Apple, last.Apple))
                return true;

            if (RequiresUpdate(conf.Auth0Oidc, last.Auth0Oidc))
                return true;

            if (RequiresUpdate(conf.AzureAd, last.AzureAd))
                return true;

            if (RequiresUpdate(conf.Baidu, last.Baidu))
                return true;

            if (RequiresUpdate(conf.Bitbucket, last.Bitbucket))
                return true;

            if (RequiresUpdate(conf.Bitly, last.Bitly))
                return true;

            if (RequiresUpdate(conf.Box, last.Box))
                return true;

            if (RequiresUpdate(conf.Daccount, last.Daccount))
                return true;

            if (RequiresUpdate(conf.Dropbox, last.Dropbox))
                return true;

            if (RequiresUpdate(conf.Dwolla, last.Dwolla))
                return true;

            if (RequiresUpdate(conf.Email, last.Email))
                return true;

            if (RequiresUpdate(conf.Evernote, last.Evernote))
                return true;

            if (RequiresUpdate(conf.EvernoteSandbox, last.EvernoteSandbox))
                return true;

            if (RequiresUpdate(conf.Exact, last.Exact))
                return true;

            if (RequiresUpdate(conf.Facebook, last.Facebook))
                return true;

            if (RequiresUpdate(conf.Fitbit, last.Fitbit))
                return true;

            if (RequiresUpdate(conf.GitHub, last.GitHub))
                return true;

            if (RequiresUpdate(conf.GoogleApps, last.GoogleApps))
                return true;

            if (RequiresUpdate(conf.GoogleOAuth2, last.GoogleOAuth2))
                return true;

            if (RequiresUpdate(conf.Instagram, last.Instagram))
                return true;

            if (RequiresUpdate(conf.Line, last.Line))
                return true;

            if (RequiresUpdate(conf.Linkedin, last.Linkedin))
                return true;

            if (RequiresUpdate(conf.OAuth1, last.OAuth1))
                return true;

            if (RequiresUpdate(conf.OAuth2, last.OAuth2))
                return true;

            if (RequiresUpdate(conf.Office365, last.Office365))
                return true;

            if (RequiresUpdate(conf.Oidc, last.Oidc))
                return true;

            if (RequiresUpdate(conf.Okta, last.Okta))
                return true;

            if (RequiresUpdate(conf.Paypal, last.Paypal))
                return true;

            if (RequiresUpdate(conf.PaypalSandbox, last.PaypalSandbox))
                return true;

            if (RequiresUpdate(conf.PingFederate, last.PingFederate))
                return true;

            if (RequiresUpdate(conf.PlanningCenter, last.PlanningCenter))
                return true;

            if (RequiresUpdate(conf.Salesforce, last.Salesforce))
                return true;

            if (RequiresUpdate(conf.SalesforceCommunity, last.SalesforceCommunity))
                return true;

            if (RequiresUpdate(conf.SalesforceSandbox, last.SalesforceSandbox))
                return true;

            if (RequiresUpdate(conf.Saml, last.Saml))
                return true;

            if (RequiresUpdate(conf.Sharepoint, last.Sharepoint))
                return true;

            if (RequiresUpdate(conf.Shop, last.Shop))
                return true;

            if (RequiresUpdate(conf.Shopify, last.Shopify))
                return true;

            if (RequiresUpdate(conf.Sms, last.Sms))
                return true;

            if (RequiresUpdate(conf.Soundcloud, last.Soundcloud))
                return true;

            if (RequiresUpdate(conf.ThirtySevenSignals, last.ThirtySevenSignals))
                return true;

            if (RequiresUpdate(conf.Twitter, last.Twitter))
                return true;

            if (RequiresUpdate(conf.Untappd, last.Untappd))
                return true;

            if (RequiresUpdate(conf.Vkontakte, last.Vkontakte))
                return true;

            if (RequiresUpdate(conf.Weibo, last.Weibo))
                return true;

            if (RequiresUpdate(conf.WindowsLive, last.WindowsLive))
                return true;

            if (RequiresUpdate(conf.Wordpress, last.Wordpress))
                return true;

            if (RequiresUpdate(conf.Yahoo, last.Yahoo))
                return true;

            if (RequiresUpdate(conf.Yandex, last.Yandex))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsAuth0? conf, V2alpha3ConnectionOptionsAuth0? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Attributes, last.Attributes))
                return true;

            if (RequiresUpdate(conf.AuthenticationMethods, last.AuthenticationMethods))
                return true;

            if (conf.BruteForceProtection is not null && conf.BruteForceProtection != last.BruteForceProtection)
                return true;

            if (RequiresUpdate(conf.Configuration, last.Configuration))
                return true;

            if (RequiresUpdate(conf.CustomScripts, last.CustomScripts))
                return true;

            if (conf.DisableSelfServiceChangePassword is not null && conf.DisableSelfServiceChangePassword != last.DisableSelfServiceChangePassword)
                return true;

            if (conf.DisableSignup is not null && conf.DisableSignup != last.DisableSignup)
                return true;

            if (conf.EnableScriptContext is not null && conf.EnableScriptContext != last.EnableScriptContext)
                return true;

            if (conf.EnabledDatabaseCustomization is not null && conf.EnabledDatabaseCustomization != last.EnabledDatabaseCustomization)
                return true;

            if (conf.ImportMode is not null && conf.ImportMode != last.ImportMode)
                return true;

            if (RequiresUpdate(conf.Mfa, last.Mfa))
                return true;

            if (RequiresUpdate(conf.PasskeyOptions, last.PasskeyOptions))
                return true;

            if (conf.PasswordPolicy is not null && conf.PasswordPolicy != last.PasswordPolicy)
                return true;

            if (RequiresUpdate(conf.PasswordComplexityOptions, last.PasswordComplexityOptions))
                return true;

            if (RequiresUpdate(conf.PasswordDictionary, last.PasswordDictionary))
                return true;

            if (RequiresUpdate(conf.PasswordHistory, last.PasswordHistory))
                return true;

            if (RequiresUpdate(conf.PasswordNoPersonalInfo, last.PasswordNoPersonalInfo))
                return true;

            if (RequiresUpdate(conf.PasswordOptions, last.PasswordOptions))
                return true;

            // the identifier precedence list is evaluated in order, so order is semantic here
            if (conf.Precedence is not null && (last.Precedence is null || conf.Precedence.SequenceEqual(last.Precedence) == false))
                return true;

            if (conf.RealmFallback is not null && conf.RealmFallback != last.RealmFallback)
                return true;

            if (conf.RequiresUsername is not null && conf.RequiresUsername != last.RequiresUsername)
                return true;

            if (RequiresUpdate(conf.Validation, last.Validation))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionAttributes? conf, V2alpha3ConnectionAttributes? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Email, last.Email))
                return true;

            if (RequiresUpdate(conf.PhoneNumber, last.PhoneNumber))
                return true;

            if (RequiresUpdate(conf.Username, last.Username))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionEmailAttribute? conf, V2alpha3ConnectionEmailAttribute? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Identifier, last.Identifier))
                return true;

            if (conf.Unique is not null && conf.Unique != last.Unique)
                return true;

            if (conf.ProfileRequired is not null && conf.ProfileRequired != last.ProfileRequired)
                return true;

            if (conf.VerificationMethod is not null && conf.VerificationMethod != last.VerificationMethod)
                return true;

            if (RequiresUpdate(conf.Signup, last.Signup))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPhoneAttribute? conf, V2alpha3ConnectionPhoneAttribute? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Identifier, last.Identifier))
                return true;

            if (conf.ProfileRequired is not null && conf.ProfileRequired != last.ProfileRequired)
                return true;

            if (RequiresUpdate(conf.Signup, last.Signup))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionUsernameAttribute? conf, V2alpha3ConnectionUsernameAttribute? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Identifier, last.Identifier))
                return true;

            if (conf.ProfileRequired is not null && conf.ProfileRequired != last.ProfileRequired)
                return true;

            if (RequiresUpdate(conf.Signup, last.Signup))
                return true;

            if (RequiresUpdate(conf.Validation, last.Validation))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionAttributeIdentifier? conf, V2alpha3ConnectionAttributeIdentifier? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            if (conf.DefaultMethod is not null && conf.DefaultMethod != last.DefaultMethod)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionSignupVerified? conf, V2alpha3ConnectionSignupVerified? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Status is not null && conf.Status != last.Status)
                return true;

            if (RequiresUpdate(conf.Verification, last.Verification))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionSignupVerification? conf, V2alpha3ConnectionSignupVerification? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionSignupSchema? conf, V2alpha3ConnectionSignupSchema? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Status is not null && conf.Status != last.Status)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionUsernameValidation? conf, V2alpha3ConnectionUsernameValidation? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.MinLength is not null && conf.MinLength != last.MinLength)
                return true;

            if (conf.MaxLength is not null && conf.MaxLength != last.MaxLength)
                return true;

            if (RequiresUpdate(conf.AllowedTypes, last.AllowedTypes))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionUsernameAllowedTypes? conf, V2alpha3ConnectionUsernameAllowedTypes? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.PhoneNumber is not null && conf.PhoneNumber != last.PhoneNumber)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionAuthenticationMethods? conf, V2alpha3ConnectionAuthenticationMethods? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Password, last.Password))
                return true;

            if (RequiresUpdate(conf.Passkey, last.Passkey))
                return true;

            if (RequiresUpdate(conf.EmailOtp, last.EmailOtp))
                return true;

            if (RequiresUpdate(conf.PhoneOtp, last.PhoneOtp))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordAuthenticationMethod? conf, V2alpha3ConnectionPasswordAuthenticationMethod? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            if (conf.ApiBehavior is not null && conf.ApiBehavior != last.ApiBehavior)
                return true;

            if (conf.SignupBehavior is not null && conf.SignupBehavior != last.SignupBehavior)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasskeyAuthenticationMethod? conf, V2alpha3ConnectionPasskeyAuthenticationMethod? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionEmailOtpAuthenticationMethod? conf, V2alpha3ConnectionEmailOtpAuthenticationMethod? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPhoneOtpAuthenticationMethod? conf, V2alpha3ConnectionPhoneOtpAuthenticationMethod? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionCustomScripts? conf, V2alpha3ConnectionCustomScripts? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Login is not null && conf.Login != last.Login)
                return true;

            if (conf.GetUser is not null && conf.GetUser != last.GetUser)
                return true;

            if (conf.Delete is not null && conf.Delete != last.Delete)
                return true;

            if (conf.ChangePassword is not null && conf.ChangePassword != last.ChangePassword)
                return true;

            if (conf.Verify is not null && conf.Verify != last.Verify)
                return true;

            if (conf.Create is not null && conf.Create != last.Create)
                return true;

            if (conf.ChangeUsername is not null && conf.ChangeUsername != last.ChangeUsername)
                return true;

            if (conf.ChangeEmail is not null && conf.ChangeEmail != last.ChangeEmail)
                return true;

            if (conf.ChangePhoneNumber is not null && conf.ChangePhoneNumber != last.ChangePhoneNumber)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionMfa? conf, V2alpha3ConnectionMfa? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            if (conf.ReturnEnrollSettings is not null && conf.ReturnEnrollSettings != last.ReturnEnrollSettings)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasskeyOptions? conf, V2alpha3ConnectionPasskeyOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ChallengeUi is not null && conf.ChallengeUi != last.ChallengeUi)
                return true;

            if (conf.ProgressiveEnrollmentEnabled is not null && conf.ProgressiveEnrollmentEnabled != last.ProgressiveEnrollmentEnabled)
                return true;

            if (conf.LocalEnrollmentEnabled is not null && conf.LocalEnrollmentEnabled != last.LocalEnrollmentEnabled)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordComplexityOptions? conf, V2alpha3ConnectionPasswordComplexityOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.MinLength is not null && conf.MinLength != last.MinLength)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordDictionaryOptions? conf, V2alpha3ConnectionPasswordDictionaryOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enable is not null && conf.Enable != last.Enable)
                return true;

            if (conf.Dictionary is not null && (last.Dictionary is null || conf.Dictionary.ToHashSet().SetEquals(last.Dictionary) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordHistoryOptions? conf, V2alpha3ConnectionPasswordHistoryOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enable is not null && conf.Enable != last.Enable)
                return true;

            if (conf.Size is not null && conf.Size != last.Size)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordNoPersonalInfoOptions? conf, V2alpha3ConnectionPasswordNoPersonalInfoOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Enable is not null && conf.Enable != last.Enable)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordOptions? conf, V2alpha3ConnectionPasswordOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Complexity, last.Complexity))
                return true;

            if (RequiresUpdate(conf.Dictionary, last.Dictionary))
                return true;

            if (RequiresUpdate(conf.History, last.History))
                return true;

            if (RequiresUpdate(conf.ProfileData, last.ProfileData))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordOptionsComplexity? conf, V2alpha3ConnectionPasswordOptionsComplexity? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.MinLength is not null && conf.MinLength != last.MinLength)
                return true;

            if (conf.CharacterTypes is not null && (last.CharacterTypes is null || conf.CharacterTypes.ToHashSet().SetEquals(last.CharacterTypes) == false))
                return true;

            if (conf.CharacterTypeRule is not null && conf.CharacterTypeRule != last.CharacterTypeRule)
                return true;

            if (conf.IdenticalCharacters is not null && conf.IdenticalCharacters != last.IdenticalCharacters)
                return true;

            if (conf.SequentialCharacters is not null && conf.SequentialCharacters != last.SequentialCharacters)
                return true;

            if (conf.MaxLengthExceeded is not null && conf.MaxLengthExceeded != last.MaxLengthExceeded)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordOptionsDictionary? conf, V2alpha3ConnectionPasswordOptionsDictionary? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            if (conf.Custom is not null && (last.Custom is null || conf.Custom.ToHashSet().SetEquals(last.Custom) == false))
                return true;

            if (conf.Default is not null && conf.Default != last.Default)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordOptionsHistory? conf, V2alpha3ConnectionPasswordOptionsHistory? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            if (conf.Size is not null && conf.Size != last.Size)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionPasswordOptionsProfileData? conf, V2alpha3ConnectionPasswordOptionsProfileData? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Active is not null && conf.Active != last.Active)
                return true;

            if (conf.BlockedFields is not null && (last.BlockedFields is null || conf.BlockedFields.ToHashSet().SetEquals(last.BlockedFields) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionValidationOptions? conf, V2alpha3ConnectionValidationOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Username, last.Username))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionUsernameValidationOptions? conf, V2alpha3ConnectionUsernameValidationOptions? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Min is not null && conf.Min != last.Min)
                return true;

            if (conf.Max is not null && conf.Max != last.Max)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsAd? conf, V2alpha3ConnectionOptionsAd? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AgentIp is not null && conf.AgentIp != last.AgentIp)
                return true;

            if (conf.AgentMode is not null && conf.AgentMode != last.AgentMode)
                return true;

            if (conf.AgentVersion is not null && conf.AgentVersion != last.AgentVersion)
                return true;

            if (conf.BruteForceProtection is not null && conf.BruteForceProtection != last.BruteForceProtection)
                return true;

            if (conf.CertAuth is not null && conf.CertAuth != last.CertAuth)
                return true;

            if (conf.Certs is not null && (last.Certs is null || conf.Certs.ToHashSet().SetEquals(last.Certs) == false))
                return true;

            if (conf.DisableCache is not null && conf.DisableCache != last.DisableCache)
                return true;

            if (conf.DisableSelfServiceChangePassword is not null && conf.DisableSelfServiceChangePassword != last.DisableSelfServiceChangePassword)
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.Ips is not null && (last.Ips is null || conf.Ips.ToHashSet().SetEquals(last.Ips) == false))
                return true;

            if (conf.Kerberos is not null && conf.Kerberos != last.Kerberos)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.SignInEndpoint is not null && conf.SignInEndpoint != last.SignInEndpoint)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.Thumbprints is not null && (last.Thumbprints is null || conf.Thumbprints.ToHashSet().SetEquals(last.Thumbprints) == false))
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsAdfs? conf, V2alpha3ConnectionOptionsAdfs? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AdfsServer is not null && conf.AdfsServer != last.AdfsServer)
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.EntityId is not null && conf.EntityId != last.EntityId)
                return true;

            if (conf.FedMetadataXml is not null && conf.FedMetadataXml != last.FedMetadataXml)
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.PrevThumbprints is not null && (last.PrevThumbprints is null || conf.PrevThumbprints.ToHashSet().SetEquals(last.PrevThumbprints) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.ShouldTrustEmailVerifiedConnection is not null && conf.ShouldTrustEmailVerifiedConnection != last.ShouldTrustEmailVerifiedConnection)
                return true;

            if (conf.SignInEndpoint is not null && conf.SignInEndpoint != last.SignInEndpoint)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.Thumbprints is not null && (last.Thumbprints is null || conf.Thumbprints.ToHashSet().SetEquals(last.Thumbprints) == false))
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.UserIdAttribute is not null && conf.UserIdAttribute != last.UserIdAttribute)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsAmazon? conf, V2alpha3ConnectionOptionsAmazon? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.PostalCode is not null && conf.PostalCode != last.PostalCode)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsApple? conf, V2alpha3ConnectionOptionsApple? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AppSecret is not null && conf.AppSecret != last.AppSecret)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Kid is not null && conf.Kid != last.Kid)
                return true;

            if (conf.Name is not null && conf.Name != last.Name)
                return true;

            if (conf.Scope is not null && conf.Scope != last.Scope)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.TeamId is not null && conf.TeamId != last.TeamId)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsAuth0Oidc? conf, V2alpha3ConnectionOptionsAuth0Oidc? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsAzureAd? conf, V2alpha3ConnectionOptionsAzureAd? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ApiEnableUsers is not null && conf.ApiEnableUsers != last.ApiEnableUsers)
                return true;

            if (conf.AppDomain is not null && conf.AppDomain != last.AppDomain)
                return true;

            if (conf.AppId is not null && conf.AppId != last.AppId)
                return true;

            if (conf.BasicProfile is not null && conf.BasicProfile != last.BasicProfile)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.ExtAccessToken is not null && conf.ExtAccessToken != last.ExtAccessToken)
                return true;

            if (conf.ExtAccountEnabled is not null && conf.ExtAccountEnabled != last.ExtAccountEnabled)
                return true;

            if (conf.ExtAdmin is not null && conf.ExtAdmin != last.ExtAdmin)
                return true;

            if (conf.ExtAgreedTerms is not null && conf.ExtAgreedTerms != last.ExtAgreedTerms)
                return true;

            if (conf.ExtAssignedLicenses is not null && conf.ExtAssignedLicenses != last.ExtAssignedLicenses)
                return true;

            if (conf.ExtAssignedPlans is not null && conf.ExtAssignedPlans != last.ExtAssignedPlans)
                return true;

            if (conf.ExtAzureId is not null && conf.ExtAzureId != last.ExtAzureId)
                return true;

            if (conf.ExtCity is not null && conf.ExtCity != last.ExtCity)
                return true;

            if (conf.ExtCountry is not null && conf.ExtCountry != last.ExtCountry)
                return true;

            if (conf.ExtDepartment is not null && conf.ExtDepartment != last.ExtDepartment)
                return true;

            if (conf.ExtDirSyncEnabled is not null && conf.ExtDirSyncEnabled != last.ExtDirSyncEnabled)
                return true;

            if (conf.ExtEmail is not null && conf.ExtEmail != last.ExtEmail)
                return true;

            if (conf.ExtExpiresIn is not null && conf.ExtExpiresIn != last.ExtExpiresIn)
                return true;

            if (conf.ExtFamilyName is not null && conf.ExtFamilyName != last.ExtFamilyName)
                return true;

            if (conf.ExtFax is not null && conf.ExtFax != last.ExtFax)
                return true;

            if (conf.ExtGivenName is not null && conf.ExtGivenName != last.ExtGivenName)
                return true;

            if (conf.ExtGroupIds is not null && conf.ExtGroupIds != last.ExtGroupIds)
                return true;

            if (conf.ExtGroups is not null && conf.ExtGroups != last.ExtGroups)
                return true;

            if (conf.ExtIsSuspended is not null && conf.ExtIsSuspended != last.ExtIsSuspended)
                return true;

            if (conf.ExtJobTitle is not null && conf.ExtJobTitle != last.ExtJobTitle)
                return true;

            if (conf.ExtLastSync is not null && conf.ExtLastSync != last.ExtLastSync)
                return true;

            if (conf.ExtMobile is not null && conf.ExtMobile != last.ExtMobile)
                return true;

            if (conf.ExtName is not null && conf.ExtName != last.ExtName)
                return true;

            if (conf.ExtNestedGroups is not null && conf.ExtNestedGroups != last.ExtNestedGroups)
                return true;

            if (conf.ExtNickname is not null && conf.ExtNickname != last.ExtNickname)
                return true;

            if (conf.ExtOid is not null && conf.ExtOid != last.ExtOid)
                return true;

            if (conf.ExtPhone is not null && conf.ExtPhone != last.ExtPhone)
                return true;

            if (conf.ExtPhysicalDeliveryOfficeName is not null && conf.ExtPhysicalDeliveryOfficeName != last.ExtPhysicalDeliveryOfficeName)
                return true;

            if (conf.ExtPostalCode is not null && conf.ExtPostalCode != last.ExtPostalCode)
                return true;

            if (conf.ExtPreferredLanguage is not null && conf.ExtPreferredLanguage != last.ExtPreferredLanguage)
                return true;

            if (conf.ExtProfile is not null && conf.ExtProfile != last.ExtProfile)
                return true;

            if (conf.ExtProvisionedPlans is not null && conf.ExtProvisionedPlans != last.ExtProvisionedPlans)
                return true;

            if (conf.ExtProvisioningErrors is not null && conf.ExtProvisioningErrors != last.ExtProvisioningErrors)
                return true;

            if (conf.ExtProxyAddresses is not null && conf.ExtProxyAddresses != last.ExtProxyAddresses)
                return true;

            if (conf.ExtPuid is not null && conf.ExtPuid != last.ExtPuid)
                return true;

            if (conf.ExtRefreshToken is not null && conf.ExtRefreshToken != last.ExtRefreshToken)
                return true;

            if (conf.ExtRoles is not null && conf.ExtRoles != last.ExtRoles)
                return true;

            if (conf.ExtState is not null && conf.ExtState != last.ExtState)
                return true;

            if (conf.ExtStreet is not null && conf.ExtStreet != last.ExtStreet)
                return true;

            if (conf.ExtTelephoneNumber is not null && conf.ExtTelephoneNumber != last.ExtTelephoneNumber)
                return true;

            if (conf.ExtTenantid is not null && conf.ExtTenantid != last.ExtTenantid)
                return true;

            if (conf.ExtUpn is not null && conf.ExtUpn != last.ExtUpn)
                return true;

            if (conf.ExtUsageLocation is not null && conf.ExtUsageLocation != last.ExtUsageLocation)
                return true;

            if (conf.ExtUserId is not null && conf.ExtUserId != last.ExtUserId)
                return true;

            if (RequiresUpdate(conf.FederatedConnectionsAccessTokens, last.FederatedConnectionsAccessTokens))
                return true;

            if (conf.Granted is not null && conf.Granted != last.Granted)
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.IdentityApi is not null && conf.IdentityApi != last.IdentityApi)
                return true;

            if (conf.MaxGroupsToRetrieve is not null && conf.MaxGroupsToRetrieve != last.MaxGroupsToRetrieve)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.ShouldTrustEmailVerifiedConnection is not null && conf.ShouldTrustEmailVerifiedConnection != last.ShouldTrustEmailVerifiedConnection)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.TenantId is not null && conf.TenantId != last.TenantId)
                return true;

            if (conf.Thumbprints is not null && (last.Thumbprints is null || conf.Thumbprints.ToHashSet().SetEquals(last.Thumbprints) == false))
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.UseWsfed is not null && conf.UseWsfed != last.UseWsfed)
                return true;

            if (conf.UseCommonEndpoint is not null && conf.UseCommonEndpoint != last.UseCommonEndpoint)
                return true;

            if (conf.UseridAttribute is not null && conf.UseridAttribute != last.UseridAttribute)
                return true;

            if (conf.WaadProtocol is not null && conf.WaadProtocol != last.WaadProtocol)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsBaidu? conf, V2alpha3ConnectionOptionsBaidu? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsBitbucket? conf, V2alpha3ConnectionOptionsBitbucket? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsBitly? conf, V2alpha3ConnectionOptionsBitly? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsBox? conf, V2alpha3ConnectionOptionsBox? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsDaccount? conf, V2alpha3ConnectionOptionsDaccount? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsDropbox? conf, V2alpha3ConnectionOptionsDropbox? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsDwolla? conf, V2alpha3ConnectionOptionsDwolla? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsEmail? conf, V2alpha3ConnectionOptionsEmail? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AuthParams is not null && conf.AuthParams != last.AuthParams)
                return true;

            if (conf.BruteForceProtection is not null && conf.BruteForceProtection != last.BruteForceProtection)
                return true;

            if (conf.DisableSignup is not null && conf.DisableSignup != last.DisableSignup)
                return true;

            if (RequiresUpdate(conf.Email, last.Email))
                return true;

            if (conf.Name is not null && conf.Name != last.Name)
                return true;

            if (RequiresUpdate(conf.Totp, last.Totp))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionEmailEmail? conf, V2alpha3ConnectionEmailEmail? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Body is not null && conf.Body != last.Body)
                return true;

            if (conf.From is not null && conf.From != last.From)
                return true;

            if (conf.Subject is not null && conf.Subject != last.Subject)
                return true;

            if (conf.Syntax is not null && conf.Syntax != last.Syntax)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionTotpEmail? conf, V2alpha3ConnectionTotpEmail? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Length is not null && conf.Length != last.Length)
                return true;

            if (conf.TimeStep is not null && conf.TimeStep != last.TimeStep)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsEvernote? conf, V2alpha3ConnectionOptionsEvernote? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsExact? conf, V2alpha3ConnectionOptionsExact? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.BaseUrl is not null && conf.BaseUrl != last.BaseUrl)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsFacebook? conf, V2alpha3ConnectionOptionsFacebook? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.Scope is not null && conf.Scope != last.Scope)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.AdsManagement is not null && conf.AdsManagement != last.AdsManagement)
                return true;

            if (conf.AdsRead is not null && conf.AdsRead != last.AdsRead)
                return true;

            if (conf.AllowContextProfileField is not null && conf.AllowContextProfileField != last.AllowContextProfileField)
                return true;

            if (conf.BusinessManagement is not null && conf.BusinessManagement != last.BusinessManagement)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.GroupsAccessMemberInfo is not null && conf.GroupsAccessMemberInfo != last.GroupsAccessMemberInfo)
                return true;

            if (conf.LeadsRetrieval is not null && conf.LeadsRetrieval != last.LeadsRetrieval)
                return true;

            if (conf.ManageNotifications is not null && conf.ManageNotifications != last.ManageNotifications)
                return true;

            if (conf.ManagePages is not null && conf.ManagePages != last.ManagePages)
                return true;

            if (conf.PagesManageCta is not null && conf.PagesManageCta != last.PagesManageCta)
                return true;

            if (conf.PagesManageInstantArticles is not null && conf.PagesManageInstantArticles != last.PagesManageInstantArticles)
                return true;

            if (conf.PagesMessaging is not null && conf.PagesMessaging != last.PagesMessaging)
                return true;

            if (conf.PagesMessagingPhoneNumber is not null && conf.PagesMessagingPhoneNumber != last.PagesMessagingPhoneNumber)
                return true;

            if (conf.PagesMessagingSubscriptions is not null && conf.PagesMessagingSubscriptions != last.PagesMessagingSubscriptions)
                return true;

            if (conf.PagesShowList is not null && conf.PagesShowList != last.PagesShowList)
                return true;

            if (conf.PublicProfile is not null && conf.PublicProfile != last.PublicProfile)
                return true;

            if (conf.PublishActions is not null && conf.PublishActions != last.PublishActions)
                return true;

            if (conf.PublishPages is not null && conf.PublishPages != last.PublishPages)
                return true;

            if (conf.PublishToGroups is not null && conf.PublishToGroups != last.PublishToGroups)
                return true;

            if (conf.PublishVideo is not null && conf.PublishVideo != last.PublishVideo)
                return true;

            if (conf.ReadAudienceNetworkInsights is not null && conf.ReadAudienceNetworkInsights != last.ReadAudienceNetworkInsights)
                return true;

            if (conf.ReadInsights is not null && conf.ReadInsights != last.ReadInsights)
                return true;

            if (conf.ReadMailbox is not null && conf.ReadMailbox != last.ReadMailbox)
                return true;

            if (conf.ReadPageMailboxes is not null && conf.ReadPageMailboxes != last.ReadPageMailboxes)
                return true;

            if (conf.ReadStream is not null && conf.ReadStream != last.ReadStream)
                return true;

            if (conf.UserAgeRange is not null && conf.UserAgeRange != last.UserAgeRange)
                return true;

            if (conf.UserBirthday is not null && conf.UserBirthday != last.UserBirthday)
                return true;

            if (conf.UserEvents is not null && conf.UserEvents != last.UserEvents)
                return true;

            if (conf.UserFriends is not null && conf.UserFriends != last.UserFriends)
                return true;

            if (conf.UserGender is not null && conf.UserGender != last.UserGender)
                return true;

            if (conf.UserGroups is not null && conf.UserGroups != last.UserGroups)
                return true;

            if (conf.UserHometown is not null && conf.UserHometown != last.UserHometown)
                return true;

            if (conf.UserLikes is not null && conf.UserLikes != last.UserLikes)
                return true;

            if (conf.UserLink is not null && conf.UserLink != last.UserLink)
                return true;

            if (conf.UserLocation is not null && conf.UserLocation != last.UserLocation)
                return true;

            if (conf.UserManagedGroups is not null && conf.UserManagedGroups != last.UserManagedGroups)
                return true;

            if (conf.UserPhotos is not null && conf.UserPhotos != last.UserPhotos)
                return true;

            if (conf.UserPosts is not null && conf.UserPosts != last.UserPosts)
                return true;

            if (conf.UserStatus is not null && conf.UserStatus != last.UserStatus)
                return true;

            if (conf.UserTaggedPlaces is not null && conf.UserTaggedPlaces != last.UserTaggedPlaces)
                return true;

            if (conf.UserVideos is not null && conf.UserVideos != last.UserVideos)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsFitbit? conf, V2alpha3ConnectionOptionsFitbit? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsGitHub? conf, V2alpha3ConnectionOptionsGitHub? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.AdminOrg is not null && conf.AdminOrg != last.AdminOrg)
                return true;

            if (conf.AdminPublicKey is not null && conf.AdminPublicKey != last.AdminPublicKey)
                return true;

            if (conf.AdminRepoHook is not null && conf.AdminRepoHook != last.AdminRepoHook)
                return true;

            if (conf.DeleteRepo is not null && conf.DeleteRepo != last.DeleteRepo)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.Follow is not null && conf.Follow != last.Follow)
                return true;

            if (conf.Gist is not null && conf.Gist != last.Gist)
                return true;

            if (conf.Notifications is not null && conf.Notifications != last.Notifications)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.PublicRepo is not null && conf.PublicRepo != last.PublicRepo)
                return true;

            if (conf.ReadOrg is not null && conf.ReadOrg != last.ReadOrg)
                return true;

            if (conf.ReadPublicKey is not null && conf.ReadPublicKey != last.ReadPublicKey)
                return true;

            if (conf.ReadRepoHook is not null && conf.ReadRepoHook != last.ReadRepoHook)
                return true;

            if (conf.ReadUser is not null && conf.ReadUser != last.ReadUser)
                return true;

            if (conf.Repo is not null && conf.Repo != last.Repo)
                return true;

            if (conf.RepoDeployment is not null && conf.RepoDeployment != last.RepoDeployment)
                return true;

            if (conf.RepoStatus is not null && conf.RepoStatus != last.RepoStatus)
                return true;

            if (conf.WriteOrg is not null && conf.WriteOrg != last.WriteOrg)
                return true;

            if (conf.WritePublicKey is not null && conf.WritePublicKey != last.WritePublicKey)
                return true;

            if (conf.WriteRepoHook is not null && conf.WriteRepoHook != last.WriteRepoHook)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsGoogleApps? conf, V2alpha3ConnectionOptionsGoogleApps? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AdminAccessToken is not null && conf.AdminAccessToken != last.AdminAccessToken)
                return true;

            if (conf.AdminAccessTokenExpiresin is not null && conf.AdminAccessTokenExpiresin != last.AdminAccessTokenExpiresin)
                return true;

            if (conf.AdminRefreshToken is not null && conf.AdminRefreshToken != last.AdminRefreshToken)
                return true;

            if (conf.AllowSettingLoginScopes is not null && conf.AllowSettingLoginScopes != last.AllowSettingLoginScopes)
                return true;

            if (conf.ApiEnableGroups is not null && conf.ApiEnableGroups != last.ApiEnableGroups)
                return true;

            if (conf.ApiEnableUsers is not null && conf.ApiEnableUsers != last.ApiEnableUsers)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Domain is not null && conf.Domain != last.Domain)
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.ExtAgreedTerms is not null && conf.ExtAgreedTerms != last.ExtAgreedTerms)
                return true;

            if (conf.ExtGroups is not null && conf.ExtGroups != last.ExtGroups)
                return true;

            if (conf.ExtGroupsExtended is not null && conf.ExtGroupsExtended != last.ExtGroupsExtended)
                return true;

            if (conf.ExtIsAdmin is not null && conf.ExtIsAdmin != last.ExtIsAdmin)
                return true;

            if (conf.ExtIsSuspended is not null && conf.ExtIsSuspended != last.ExtIsSuspended)
                return true;

            if (RequiresUpdate(conf.FederatedConnectionsAccessTokens, last.FederatedConnectionsAccessTokens))
                return true;

            if (conf.HandleLoginFromSocial is not null && conf.HandleLoginFromSocial != last.HandleLoginFromSocial)
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.MapUserIdToId is not null && conf.MapUserIdToId != last.MapUserIdToId)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsGoogleOAuth2? conf, V2alpha3ConnectionOptionsGoogleOAuth2? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AllowedAudiences is not null && (last.AllowedAudiences is null || conf.AllowedAudiences.ToHashSet().SetEquals(last.AllowedAudiences) == false))
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.AdsenseManagement is not null && conf.AdsenseManagement != last.AdsenseManagement)
                return true;

            if (conf.Analytics is not null && conf.Analytics != last.Analytics)
                return true;

            if (conf.Blogger is not null && conf.Blogger != last.Blogger)
                return true;

            if (conf.Calendar is not null && conf.Calendar != last.Calendar)
                return true;

            if (conf.CalendarAddonsExecute is not null && conf.CalendarAddonsExecute != last.CalendarAddonsExecute)
                return true;

            if (conf.CalendarEvents is not null && conf.CalendarEvents != last.CalendarEvents)
                return true;

            if (conf.CalendarEventsReadonly is not null && conf.CalendarEventsReadonly != last.CalendarEventsReadonly)
                return true;

            if (conf.CalendarSettingsReadonly is not null && conf.CalendarSettingsReadonly != last.CalendarSettingsReadonly)
                return true;

            if (conf.ChromeWebStore is not null && conf.ChromeWebStore != last.ChromeWebStore)
                return true;

            if (conf.Contacts is not null && conf.Contacts != last.Contacts)
                return true;

            if (conf.ContactsNew is not null && conf.ContactsNew != last.ContactsNew)
                return true;

            if (conf.ContactsOtherReadonly is not null && conf.ContactsOtherReadonly != last.ContactsOtherReadonly)
                return true;

            if (conf.ContactsReadonly is not null && conf.ContactsReadonly != last.ContactsReadonly)
                return true;

            if (conf.ContentApiForShopping is not null && conf.ContentApiForShopping != last.ContentApiForShopping)
                return true;

            if (conf.Coordinate is not null && conf.Coordinate != last.Coordinate)
                return true;

            if (conf.CoordinateReadonly is not null && conf.CoordinateReadonly != last.CoordinateReadonly)
                return true;

            if (conf.DirectoryReadonly is not null && conf.DirectoryReadonly != last.DirectoryReadonly)
                return true;

            if (conf.DocumentList is not null && conf.DocumentList != last.DocumentList)
                return true;

            if (conf.Drive is not null && conf.Drive != last.Drive)
                return true;

            if (conf.DriveActivity is not null && conf.DriveActivity != last.DriveActivity)
                return true;

            if (conf.DriveActivityReadonly is not null && conf.DriveActivityReadonly != last.DriveActivityReadonly)
                return true;

            if (conf.DriveAppdata is not null && conf.DriveAppdata != last.DriveAppdata)
                return true;

            if (conf.DriveAppsReadonly is not null && conf.DriveAppsReadonly != last.DriveAppsReadonly)
                return true;

            if (conf.DriveFile is not null && conf.DriveFile != last.DriveFile)
                return true;

            if (conf.DriveMetadata is not null && conf.DriveMetadata != last.DriveMetadata)
                return true;

            if (conf.DriveMetadataReadonly is not null && conf.DriveMetadataReadonly != last.DriveMetadataReadonly)
                return true;

            if (conf.DrivePhotosReadonly is not null && conf.DrivePhotosReadonly != last.DrivePhotosReadonly)
                return true;

            if (conf.DriveReadonly is not null && conf.DriveReadonly != last.DriveReadonly)
                return true;

            if (conf.DriveScripts is not null && conf.DriveScripts != last.DriveScripts)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.Gmail is not null && conf.Gmail != last.Gmail)
                return true;

            if (conf.GmailCompose is not null && conf.GmailCompose != last.GmailCompose)
                return true;

            if (conf.GmailInsert is not null && conf.GmailInsert != last.GmailInsert)
                return true;

            if (conf.GmailLabels is not null && conf.GmailLabels != last.GmailLabels)
                return true;

            if (conf.GmailMetadata is not null && conf.GmailMetadata != last.GmailMetadata)
                return true;

            if (conf.GmailModify is not null && conf.GmailModify != last.GmailModify)
                return true;

            if (conf.GmailNew is not null && conf.GmailNew != last.GmailNew)
                return true;

            if (conf.GmailReadonly is not null && conf.GmailReadonly != last.GmailReadonly)
                return true;

            if (conf.GmailSend is not null && conf.GmailSend != last.GmailSend)
                return true;

            if (conf.GmailSettingsBasic is not null && conf.GmailSettingsBasic != last.GmailSettingsBasic)
                return true;

            if (conf.GmailSettingsSharing is not null && conf.GmailSettingsSharing != last.GmailSettingsSharing)
                return true;

            if (conf.GoogleAffiliateNetwork is not null && conf.GoogleAffiliateNetwork != last.GoogleAffiliateNetwork)
                return true;

            if (conf.GoogleBooks is not null && conf.GoogleBooks != last.GoogleBooks)
                return true;

            if (conf.GoogleCloudStorage is not null && conf.GoogleCloudStorage != last.GoogleCloudStorage)
                return true;

            if (conf.GoogleDrive is not null && conf.GoogleDrive != last.GoogleDrive)
                return true;

            if (conf.GoogleDriveFiles is not null && conf.GoogleDriveFiles != last.GoogleDriveFiles)
                return true;

            if (conf.GooglePlus is not null && conf.GooglePlus != last.GooglePlus)
                return true;

            if (conf.LatitudeBest is not null && conf.LatitudeBest != last.LatitudeBest)
                return true;

            if (conf.LatitudeCity is not null && conf.LatitudeCity != last.LatitudeCity)
                return true;

            if (conf.Moderator is not null && conf.Moderator != last.Moderator)
                return true;

            if (conf.OfflineAccess is not null && conf.OfflineAccess != last.OfflineAccess)
                return true;

            if (conf.Orkut is not null && conf.Orkut != last.Orkut)
                return true;

            if (conf.PicasaWeb is not null && conf.PicasaWeb != last.PicasaWeb)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.Sites is not null && conf.Sites != last.Sites)
                return true;

            if (conf.Tasks is not null && conf.Tasks != last.Tasks)
                return true;

            if (conf.TasksReadonly is not null && conf.TasksReadonly != last.TasksReadonly)
                return true;

            if (conf.UrlShortener is not null && conf.UrlShortener != last.UrlShortener)
                return true;

            if (conf.WebmasterTools is not null && conf.WebmasterTools != last.WebmasterTools)
                return true;

            if (conf.Youtube is not null && conf.Youtube != last.Youtube)
                return true;

            if (conf.YoutubeChannelmembershipsCreator is not null && conf.YoutubeChannelmembershipsCreator != last.YoutubeChannelmembershipsCreator)
                return true;

            if (conf.YoutubeNew is not null && conf.YoutubeNew != last.YoutubeNew)
                return true;

            if (conf.YoutubeReadonly is not null && conf.YoutubeReadonly != last.YoutubeReadonly)
                return true;

            if (conf.YoutubeUpload is not null && conf.YoutubeUpload != last.YoutubeUpload)
                return true;

            if (conf.Youtubepartner is not null && conf.Youtubepartner != last.Youtubepartner)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsInstagram? conf, V2alpha3ConnectionOptionsInstagram? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsLine? conf, V2alpha3ConnectionOptionsLine? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsLinkedin? conf, V2alpha3ConnectionOptionsLinkedin? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.StrategyVersion is not null && conf.StrategyVersion != last.StrategyVersion)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.BasicProfile is not null && conf.BasicProfile != last.BasicProfile)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.FullProfile is not null && conf.FullProfile != last.FullProfile)
                return true;

            if (conf.Network is not null && conf.Network != last.Network)
                return true;

            if (conf.Openid is not null && conf.Openid != last.Openid)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsOAuth1? conf, V2alpha3ConnectionOptionsOAuth1? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AccessTokenUrl is not null && conf.AccessTokenUrl != last.AccessTokenUrl)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.RequestTokenUrl is not null && conf.RequestTokenUrl != last.RequestTokenUrl)
                return true;

            if (RequiresUpdate(conf.Scripts, last.Scripts))
                return true;

            if (conf.SignatureMethod is not null && conf.SignatureMethod != last.SignatureMethod)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.UserAuthorizationUrl is not null && conf.UserAuthorizationUrl != last.UserAuthorizationUrl)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionScriptsOAuth1? conf, V2alpha3ConnectionScriptsOAuth1? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.FetchUserProfile is not null && conf.FetchUserProfile != last.FetchUserProfile)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsOAuth2? conf, V2alpha3ConnectionOptionsOAuth2? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.AuthParams, last.AuthParams))
                return true;

            if (RequiresUpdate(conf.AuthParamsMap, last.AuthParamsMap))
                return true;

            if (conf.AuthorizationUrl is not null && conf.AuthorizationUrl != last.AuthorizationUrl)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (RequiresUpdate(conf.CustomHeaders, last.CustomHeaders))
                return true;

            if (RequiresUpdate(conf.FieldsMap, last.FieldsMap))
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.LogoutUrl is not null && conf.LogoutUrl != last.LogoutUrl)
                return true;

            if (conf.PkceEnabled is not null && conf.PkceEnabled != last.PkceEnabled)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (RequiresUpdate(conf.Scripts, last.Scripts))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.TokenUrl is not null && conf.TokenUrl != last.TokenUrl)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.UseOauthSpecScope is not null && conf.UseOauthSpecScope != last.UseOauthSpecScope)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionScriptsOAuth2? conf, V2alpha3ConnectionScriptsOAuth2? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.FetchUserProfile is not null && conf.FetchUserProfile != last.FetchUserProfile)
                return true;

            if (conf.GetLogoutUrl is not null && conf.GetLogoutUrl != last.GetLogoutUrl)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsOffice365? conf, V2alpha3ConnectionOptionsOffice365? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsOidc? conf, V2alpha3ConnectionOptionsOidc? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.AttributeMap, last.AttributeMap))
                return true;

            if (conf.DiscoveryUrl is not null && conf.DiscoveryUrl != last.DiscoveryUrl)
                return true;

            if (conf.Type is not null && conf.Type != last.Type)
                return true;

            if (conf.AuthorizationEndpoint is not null && conf.AuthorizationEndpoint != last.AuthorizationEndpoint)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (RequiresUpdate(conf.ConnectionSettings, last.ConnectionSettings))
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.DpopSigningAlg is not null && conf.DpopSigningAlg != last.DpopSigningAlg)
                return true;

            if (RequiresUpdate(conf.FederatedConnectionsAccessTokens, last.FederatedConnectionsAccessTokens))
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.IdTokenSignedResponseAlgs is not null && (last.IdTokenSignedResponseAlgs is null || conf.IdTokenSignedResponseAlgs.ToHashSet().SetEquals(last.IdTokenSignedResponseAlgs) == false))
                return true;

            if (conf.Issuer is not null && conf.Issuer != last.Issuer)
                return true;

            if (conf.JwksUri is not null && conf.JwksUri != last.JwksUri)
                return true;

            if (RequiresUpdate(conf.OidcMetadata, last.OidcMetadata))
                return true;

            if (conf.Scope is not null && conf.Scope != last.Scope)
                return true;

            if (conf.SendBackChannelNonce is not null && conf.SendBackChannelNonce != last.SendBackChannelNonce)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.TokenEndpoint is not null && conf.TokenEndpoint != last.TokenEndpoint)
                return true;

            if (conf.TokenEndpointAuthMethod is not null && conf.TokenEndpointAuthMethod != last.TokenEndpointAuthMethod)
                return true;

            if (conf.TokenEndpointAuthSigningAlg is not null && conf.TokenEndpointAuthSigningAlg != last.TokenEndpointAuthSigningAlg)
                return true;

            if (conf.TokenEndpointJwtcaAudFormat is not null && conf.TokenEndpointJwtcaAudFormat != last.TokenEndpointJwtcaAudFormat)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.UserinfoEndpoint is not null && conf.UserinfoEndpoint != last.UserinfoEndpoint)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionAttributeMapOidc? conf, V2alpha3ConnectionAttributeMapOidc? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Attributes, last.Attributes))
                return true;

            if (conf.MappingMode is not null && conf.MappingMode != last.MappingMode)
                return true;

            if (conf.UserinfoScope is not null && conf.UserinfoScope != last.UserinfoScope)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionConnectionSettings? conf, V2alpha3ConnectionConnectionSettings? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Pkce is not null && conf.Pkce != last.Pkce)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsOidcMetadata? conf, V2alpha3ConnectionOptionsOidcMetadata? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AcrValuesSupported is not null && (last.AcrValuesSupported is null || conf.AcrValuesSupported.ToHashSet().SetEquals(last.AcrValuesSupported) == false))
                return true;

            if (conf.AuthorizationEndpoint is not null && conf.AuthorizationEndpoint != last.AuthorizationEndpoint)
                return true;

            if (conf.ClaimTypesSupported is not null && (last.ClaimTypesSupported is null || conf.ClaimTypesSupported.ToHashSet().SetEquals(last.ClaimTypesSupported) == false))
                return true;

            if (conf.ClaimsLocalesSupported is not null && (last.ClaimsLocalesSupported is null || conf.ClaimsLocalesSupported.ToHashSet().SetEquals(last.ClaimsLocalesSupported) == false))
                return true;

            if (conf.ClaimsParameterSupported is not null && conf.ClaimsParameterSupported != last.ClaimsParameterSupported)
                return true;

            if (conf.ClaimsSupported is not null && (last.ClaimsSupported is null || conf.ClaimsSupported.ToHashSet().SetEquals(last.ClaimsSupported) == false))
                return true;

            if (conf.DisplayValuesSupported is not null && (last.DisplayValuesSupported is null || conf.DisplayValuesSupported.ToHashSet().SetEquals(last.DisplayValuesSupported) == false))
                return true;

            if (conf.DpopSigningAlgValuesSupported is not null && (last.DpopSigningAlgValuesSupported is null || conf.DpopSigningAlgValuesSupported.ToHashSet().SetEquals(last.DpopSigningAlgValuesSupported) == false))
                return true;

            if (conf.EndSessionEndpoint is not null && conf.EndSessionEndpoint != last.EndSessionEndpoint)
                return true;

            if (conf.GrantTypesSupported is not null && (last.GrantTypesSupported is null || conf.GrantTypesSupported.ToHashSet().SetEquals(last.GrantTypesSupported) == false))
                return true;

            if (conf.IdTokenEncryptionAlgValuesSupported is not null && (last.IdTokenEncryptionAlgValuesSupported is null || conf.IdTokenEncryptionAlgValuesSupported.ToHashSet().SetEquals(last.IdTokenEncryptionAlgValuesSupported) == false))
                return true;

            if (conf.IdTokenEncryptionEncValuesSupported is not null && (last.IdTokenEncryptionEncValuesSupported is null || conf.IdTokenEncryptionEncValuesSupported.ToHashSet().SetEquals(last.IdTokenEncryptionEncValuesSupported) == false))
                return true;

            if (conf.IdTokenSigningAlgValuesSupported is not null && (last.IdTokenSigningAlgValuesSupported is null || conf.IdTokenSigningAlgValuesSupported.ToHashSet().SetEquals(last.IdTokenSigningAlgValuesSupported) == false))
                return true;

            if (conf.Issuer is not null && conf.Issuer != last.Issuer)
                return true;

            if (conf.JwksUri is not null && conf.JwksUri != last.JwksUri)
                return true;

            if (conf.OpPolicyUri is not null && conf.OpPolicyUri != last.OpPolicyUri)
                return true;

            if (conf.OpTosUri is not null && conf.OpTosUri != last.OpTosUri)
                return true;

            if (conf.RegistrationEndpoint is not null && conf.RegistrationEndpoint != last.RegistrationEndpoint)
                return true;

            if (conf.RequestObjectEncryptionAlgValuesSupported is not null && (last.RequestObjectEncryptionAlgValuesSupported is null || conf.RequestObjectEncryptionAlgValuesSupported.ToHashSet().SetEquals(last.RequestObjectEncryptionAlgValuesSupported) == false))
                return true;

            if (conf.RequestObjectEncryptionEncValuesSupported is not null && (last.RequestObjectEncryptionEncValuesSupported is null || conf.RequestObjectEncryptionEncValuesSupported.ToHashSet().SetEquals(last.RequestObjectEncryptionEncValuesSupported) == false))
                return true;

            if (conf.RequestObjectSigningAlgValuesSupported is not null && (last.RequestObjectSigningAlgValuesSupported is null || conf.RequestObjectSigningAlgValuesSupported.ToHashSet().SetEquals(last.RequestObjectSigningAlgValuesSupported) == false))
                return true;

            if (conf.RequestParameterSupported is not null && conf.RequestParameterSupported != last.RequestParameterSupported)
                return true;

            if (conf.RequestUriParameterSupported is not null && conf.RequestUriParameterSupported != last.RequestUriParameterSupported)
                return true;

            if (conf.RequireRequestUriRegistration is not null && conf.RequireRequestUriRegistration != last.RequireRequestUriRegistration)
                return true;

            if (conf.ResponseModesSupported is not null && (last.ResponseModesSupported is null || conf.ResponseModesSupported.ToHashSet().SetEquals(last.ResponseModesSupported) == false))
                return true;

            if (conf.ResponseTypesSupported is not null && (last.ResponseTypesSupported is null || conf.ResponseTypesSupported.ToHashSet().SetEquals(last.ResponseTypesSupported) == false))
                return true;

            if (conf.ScopesSupported is not null && (last.ScopesSupported is null || conf.ScopesSupported.ToHashSet().SetEquals(last.ScopesSupported) == false))
                return true;

            if (conf.ServiceDocumentation is not null && conf.ServiceDocumentation != last.ServiceDocumentation)
                return true;

            if (conf.SubjectTypesSupported is not null && (last.SubjectTypesSupported is null || conf.SubjectTypesSupported.ToHashSet().SetEquals(last.SubjectTypesSupported) == false))
                return true;

            if (conf.TokenEndpoint is not null && conf.TokenEndpoint != last.TokenEndpoint)
                return true;

            if (conf.TokenEndpointAuthMethodsSupported is not null && (last.TokenEndpointAuthMethodsSupported is null || conf.TokenEndpointAuthMethodsSupported.ToHashSet().SetEquals(last.TokenEndpointAuthMethodsSupported) == false))
                return true;

            if (conf.TokenEndpointAuthSigningAlgValuesSupported is not null && (last.TokenEndpointAuthSigningAlgValuesSupported is null || conf.TokenEndpointAuthSigningAlgValuesSupported.ToHashSet().SetEquals(last.TokenEndpointAuthSigningAlgValuesSupported) == false))
                return true;

            if (conf.UiLocalesSupported is not null && (last.UiLocalesSupported is null || conf.UiLocalesSupported.ToHashSet().SetEquals(last.UiLocalesSupported) == false))
                return true;

            if (conf.UserinfoEncryptionAlgValuesSupported is not null && (last.UserinfoEncryptionAlgValuesSupported is null || conf.UserinfoEncryptionAlgValuesSupported.ToHashSet().SetEquals(last.UserinfoEncryptionAlgValuesSupported) == false))
                return true;

            if (conf.UserinfoEncryptionEncValuesSupported is not null && (last.UserinfoEncryptionEncValuesSupported is null || conf.UserinfoEncryptionEncValuesSupported.ToHashSet().SetEquals(last.UserinfoEncryptionEncValuesSupported) == false))
                return true;

            if (conf.UserinfoEndpoint is not null && conf.UserinfoEndpoint != last.UserinfoEndpoint)
                return true;

            if (conf.UserinfoSigningAlgValuesSupported is not null && (last.UserinfoSigningAlgValuesSupported is null || conf.UserinfoSigningAlgValuesSupported.ToHashSet().SetEquals(last.UserinfoSigningAlgValuesSupported) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsOkta? conf, V2alpha3ConnectionOptionsOkta? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.AttributeMap, last.AttributeMap))
                return true;

            if (conf.Domain is not null && conf.Domain != last.Domain)
                return true;

            if (conf.Type is not null && conf.Type != last.Type)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            if (conf.AuthorizationEndpoint is not null && conf.AuthorizationEndpoint != last.AuthorizationEndpoint)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (RequiresUpdate(conf.ConnectionSettings, last.ConnectionSettings))
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.DpopSigningAlg is not null && conf.DpopSigningAlg != last.DpopSigningAlg)
                return true;

            if (RequiresUpdate(conf.FederatedConnectionsAccessTokens, last.FederatedConnectionsAccessTokens))
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (conf.IdTokenSignedResponseAlgs is not null && (last.IdTokenSignedResponseAlgs is null || conf.IdTokenSignedResponseAlgs.ToHashSet().SetEquals(last.IdTokenSignedResponseAlgs) == false))
                return true;

            if (conf.Issuer is not null && conf.Issuer != last.Issuer)
                return true;

            if (conf.JwksUri is not null && conf.JwksUri != last.JwksUri)
                return true;

            if (RequiresUpdate(conf.OidcMetadata, last.OidcMetadata))
                return true;

            if (conf.Scope is not null && conf.Scope != last.Scope)
                return true;

            if (conf.SendBackChannelNonce is not null && conf.SendBackChannelNonce != last.SendBackChannelNonce)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.TokenEndpoint is not null && conf.TokenEndpoint != last.TokenEndpoint)
                return true;

            if (conf.TokenEndpointAuthMethod is not null && conf.TokenEndpointAuthMethod != last.TokenEndpointAuthMethod)
                return true;

            if (conf.TokenEndpointAuthSigningAlg is not null && conf.TokenEndpointAuthSigningAlg != last.TokenEndpointAuthSigningAlg)
                return true;

            if (conf.TokenEndpointJwtcaAudFormat is not null && conf.TokenEndpointJwtcaAudFormat != last.TokenEndpointJwtcaAudFormat)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.UserinfoEndpoint is not null && conf.UserinfoEndpoint != last.UserinfoEndpoint)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionAttributeMapOkta? conf, V2alpha3ConnectionAttributeMapOkta? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (RequiresUpdate(conf.Attributes, last.Attributes))
                return true;

            if (conf.MappingMode is not null && conf.MappingMode != last.MappingMode)
                return true;

            if (conf.UserinfoScope is not null && conf.UserinfoScope != last.UserinfoScope)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsPaypal? conf, V2alpha3ConnectionOptionsPaypal? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.Address is not null && conf.Address != last.Address)
                return true;

            if (conf.Email is not null && conf.Email != last.Email)
                return true;

            if (conf.Phone is not null && conf.Phone != last.Phone)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsPingFederate? conf, V2alpha3ConnectionOptionsPingFederate? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.PingFederateBaseUrl is not null && conf.PingFederateBaseUrl != last.PingFederateBaseUrl)
                return true;

            if (conf.SigningCert is not null && conf.SigningCert != last.SigningCert)
                return true;

            if (RequiresUpdate(conf.AssertionDecryptionSettings, last.AssertionDecryptionSettings))
                return true;

            if (conf.Cert is not null && conf.Cert != last.Cert)
                return true;

            if (RequiresUpdate(conf.DecryptionKey, last.DecryptionKey))
                return true;

            if (conf.DigestAlgorithm is not null && conf.DigestAlgorithm != last.DigestAlgorithm)
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.EntityId is not null && conf.EntityId != last.EntityId)
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (RequiresUpdate(conf.Idpinitiated, last.Idpinitiated))
                return true;

            if (conf.ProtocolBinding is not null && conf.ProtocolBinding != last.ProtocolBinding)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.SignInEndpoint is not null && conf.SignInEndpoint != last.SignInEndpoint)
                return true;

            if (conf.SignSamlRequest is not null && conf.SignSamlRequest != last.SignSamlRequest)
                return true;

            if (conf.SignatureAlgorithm is not null && conf.SignatureAlgorithm != last.SignatureAlgorithm)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.Thumbprints is not null && (last.Thumbprints is null || conf.Thumbprints.ToHashSet().SetEquals(last.Thumbprints) == false))
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionAssertionDecryptionSettings? conf, V2alpha3ConnectionAssertionDecryptionSettings? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.AlgorithmProfile is not null && conf.AlgorithmProfile != last.AlgorithmProfile)
                return true;

            if (conf.AlgorithmExceptions is not null && (last.AlgorithmExceptions is null || conf.AlgorithmExceptions.ToHashSet().SetEquals(last.AlgorithmExceptions) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionDecryptionKeySaml? conf, V2alpha3ConnectionDecryptionKeySaml? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            // the decryption key is a union of a raw private key or a cert/key pair; compare both shapes explicitly
            if (conf.PrivateKey is not null && conf.PrivateKey != last.PrivateKey)
                return true;

            if (RequiresUpdate(conf.KeyPair, last.KeyPair))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionDecryptionKeySamlCert? conf, V2alpha3ConnectionDecryptionKeySamlCert? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Cert is not null && conf.Cert != last.Cert)
                return true;

            if (conf.Key is not null && conf.Key != last.Key)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsIdpinitiatedSaml? conf, V2alpha3ConnectionOptionsIdpinitiatedSaml? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientAuthorizequery is not null && conf.ClientAuthorizequery != last.ClientAuthorizequery)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientProtocol is not null && conf.ClientProtocol != last.ClientProtocol)
                return true;

            if (conf.Enabled is not null && conf.Enabled != last.Enabled)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsPlanningCenter? conf, V2alpha3ConnectionOptionsPlanningCenter? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsSalesforce? conf, V2alpha3ConnectionOptionsSalesforce? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsSalesforceCommunity? conf, V2alpha3ConnectionOptionsSalesforceCommunity? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.CommunityBaseUrl is not null && conf.CommunityBaseUrl != last.CommunityBaseUrl)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsSaml? conf, V2alpha3ConnectionOptionsSaml? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Debug is not null && conf.Debug != last.Debug)
                return true;

            if (conf.Deflate is not null && conf.Deflate != last.Deflate)
                return true;

            if (conf.DestinationUrl is not null && conf.DestinationUrl != last.DestinationUrl)
                return true;

            if (conf.DisableSignout is not null && conf.DisableSignout != last.DisableSignout)
                return true;

            if (RequiresUpdate(conf.FieldsMap, last.FieldsMap))
                return true;

            if (conf.GlobalTokenRevocationJwtIss is not null && conf.GlobalTokenRevocationJwtIss != last.GlobalTokenRevocationJwtIss)
                return true;

            if (conf.GlobalTokenRevocationJwtSub is not null && conf.GlobalTokenRevocationJwtSub != last.GlobalTokenRevocationJwtSub)
                return true;

            if (conf.MetadataUrl is not null && conf.MetadataUrl != last.MetadataUrl)
                return true;

            if (conf.MetadataXml is not null && conf.MetadataXml != last.MetadataXml)
                return true;

            if (conf.RecipientUrl is not null && conf.RecipientUrl != last.RecipientUrl)
                return true;

            if (conf.RequestTemplate is not null && conf.RequestTemplate != last.RequestTemplate)
                return true;

            if (conf.SigningCert is not null && conf.SigningCert != last.SigningCert)
                return true;

            if (RequiresUpdate(conf.SigningKey, last.SigningKey))
                return true;

            if (conf.SignOutEndpoint is not null && conf.SignOutEndpoint != last.SignOutEndpoint)
                return true;

            if (conf.UserIdAttribute is not null && conf.UserIdAttribute != last.UserIdAttribute)
                return true;

            if (RequiresUpdate(conf.AssertionDecryptionSettings, last.AssertionDecryptionSettings))
                return true;

            if (conf.Cert is not null && conf.Cert != last.Cert)
                return true;

            if (RequiresUpdate(conf.DecryptionKey, last.DecryptionKey))
                return true;

            if (conf.DigestAlgorithm is not null && conf.DigestAlgorithm != last.DigestAlgorithm)
                return true;

            if (conf.DomainAliases is not null && (last.DomainAliases is null || conf.DomainAliases.ToHashSet().SetEquals(last.DomainAliases) == false))
                return true;

            if (conf.EntityId is not null && conf.EntityId != last.EntityId)
                return true;

            if (conf.IconUrl is not null && conf.IconUrl != last.IconUrl)
                return true;

            if (RequiresUpdate(conf.Idpinitiated, last.Idpinitiated))
                return true;

            if (conf.ProtocolBinding is not null && conf.ProtocolBinding != last.ProtocolBinding)
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.SignInEndpoint is not null && conf.SignInEndpoint != last.SignInEndpoint)
                return true;

            if (conf.SignSamlRequest is not null && conf.SignSamlRequest != last.SignSamlRequest)
                return true;

            if (conf.SignatureAlgorithm is not null && conf.SignatureAlgorithm != last.SignatureAlgorithm)
                return true;

            if (conf.TenantDomain is not null && conf.TenantDomain != last.TenantDomain)
                return true;

            if (conf.Thumbprints is not null && (last.Thumbprints is null || conf.Thumbprints.ToHashSet().SetEquals(last.Thumbprints) == false))
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionSigningKeySaml? conf, V2alpha3ConnectionSigningKeySaml? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Cert is not null && conf.Cert != last.Cert)
                return true;

            if (conf.Key is not null && conf.Key != last.Key)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsSharepoint? conf, V2alpha3ConnectionOptionsSharepoint? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsShop? conf, V2alpha3ConnectionOptionsShop? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsShopify? conf, V2alpha3ConnectionOptionsShopify? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsSms? conf, V2alpha3ConnectionOptionsSms? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.BruteForceProtection is not null && conf.BruteForceProtection != last.BruteForceProtection)
                return true;

            if (conf.DisableSignup is not null && conf.DisableSignup != last.DisableSignup)
                return true;

            if (conf.ForwardReqInfo is not null && conf.ForwardReqInfo != last.ForwardReqInfo)
                return true;

            if (conf.From is not null && conf.From != last.From)
                return true;

            if (RequiresUpdate(conf.GatewayAuthentication, last.GatewayAuthentication))
                return true;

            if (conf.GatewayUrl is not null && conf.GatewayUrl != last.GatewayUrl)
                return true;

            if (conf.MessagingServiceSid is not null && conf.MessagingServiceSid != last.MessagingServiceSid)
                return true;

            if (conf.Name is not null && conf.Name != last.Name)
                return true;

            if (conf.Provider is not null && conf.Provider != last.Provider)
                return true;

            if (conf.Syntax is not null && conf.Syntax != last.Syntax)
                return true;

            if (conf.Template is not null && conf.Template != last.Template)
                return true;

            if (RequiresUpdate(conf.Totp, last.Totp))
                return true;

            if (conf.TwilioSid is not null && conf.TwilioSid != last.TwilioSid)
                return true;

            if (conf.TwilioToken is not null && conf.TwilioToken != last.TwilioToken)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionGatewayAuthenticationSms? conf, V2alpha3ConnectionGatewayAuthenticationSms? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Audience is not null && conf.Audience != last.Audience)
                return true;

            if (conf.Method is not null && conf.Method != last.Method)
                return true;

            if (conf.Secret is not null && conf.Secret != last.Secret)
                return true;

            if (conf.SecretBase64Encoded is not null && conf.SecretBase64Encoded != last.SecretBase64Encoded)
                return true;

            if (conf.Subject is not null && conf.Subject != last.Subject)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionTotpSms? conf, V2alpha3ConnectionTotpSms? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.Length is not null && conf.Length != last.Length)
                return true;

            if (conf.TimeStep is not null && conf.TimeStep != last.TimeStep)
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsSoundcloud? conf, V2alpha3ConnectionOptionsSoundcloud? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsThirtySevenSignals? conf, V2alpha3ConnectionOptionsThirtySevenSignals? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsTwitter? conf, V2alpha3ConnectionOptionsTwitter? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Protocol is not null && conf.Protocol != last.Protocol)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.OfflineAccess is not null && conf.OfflineAccess != last.OfflineAccess)
                return true;

            if (conf.Profile is not null && conf.Profile != last.Profile)
                return true;

            if (conf.TweetRead is not null && conf.TweetRead != last.TweetRead)
                return true;

            if (conf.UsersRead is not null && conf.UsersRead != last.UsersRead)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsUntappd? conf, V2alpha3ConnectionOptionsUntappd? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsVkontakte? conf, V2alpha3ConnectionOptionsVkontakte? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsWeibo? conf, V2alpha3ConnectionOptionsWeibo? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsWindowsLive? conf, V2alpha3ConnectionOptionsWindowsLive? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.FreeformScopes is not null && (last.FreeformScopes is null || conf.FreeformScopes.ToHashSet().SetEquals(last.FreeformScopes) == false))
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (conf.StrategyVersion is not null && conf.StrategyVersion != last.StrategyVersion)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.Applications is not null && conf.Applications != last.Applications)
                return true;

            if (conf.ApplicationsCreate is not null && conf.ApplicationsCreate != last.ApplicationsCreate)
                return true;

            if (conf.Basic is not null && conf.Basic != last.Basic)
                return true;

            if (conf.Birthday is not null && conf.Birthday != last.Birthday)
                return true;

            if (conf.Calendars is not null && conf.Calendars != last.Calendars)
                return true;

            if (conf.CalendarsUpdate is not null && conf.CalendarsUpdate != last.CalendarsUpdate)
                return true;

            if (conf.ContactsBirthday is not null && conf.ContactsBirthday != last.ContactsBirthday)
                return true;

            if (conf.ContactsCalendars is not null && conf.ContactsCalendars != last.ContactsCalendars)
                return true;

            if (conf.ContactsCreate is not null && conf.ContactsCreate != last.ContactsCreate)
                return true;

            if (conf.ContactsPhotos is not null && conf.ContactsPhotos != last.ContactsPhotos)
                return true;

            if (conf.ContactsSkydrive is not null && conf.ContactsSkydrive != last.ContactsSkydrive)
                return true;

            if (conf.DirectoryAccessasuserAll is not null && conf.DirectoryAccessasuserAll != last.DirectoryAccessasuserAll)
                return true;

            if (conf.DirectoryReadAll is not null && conf.DirectoryReadAll != last.DirectoryReadAll)
                return true;

            if (conf.DirectoryReadwriteAll is not null && conf.DirectoryReadwriteAll != last.DirectoryReadwriteAll)
                return true;

            if (conf.Emails is not null && conf.Emails != last.Emails)
                return true;

            if (conf.EventsCreate is not null && conf.EventsCreate != last.EventsCreate)
                return true;

            if (conf.GraphCalendars is not null && conf.GraphCalendars != last.GraphCalendars)
                return true;

            if (conf.GraphCalendarsUpdate is not null && conf.GraphCalendarsUpdate != last.GraphCalendarsUpdate)
                return true;

            if (conf.GraphContacts is not null && conf.GraphContacts != last.GraphContacts)
                return true;

            if (conf.GraphContactsUpdate is not null && conf.GraphContactsUpdate != last.GraphContactsUpdate)
                return true;

            if (conf.GraphDevice is not null && conf.GraphDevice != last.GraphDevice)
                return true;

            if (conf.GraphDeviceCommand is not null && conf.GraphDeviceCommand != last.GraphDeviceCommand)
                return true;

            if (conf.GraphEmails is not null && conf.GraphEmails != last.GraphEmails)
                return true;

            if (conf.GraphEmailsUpdate is not null && conf.GraphEmailsUpdate != last.GraphEmailsUpdate)
                return true;

            if (conf.GraphFiles is not null && conf.GraphFiles != last.GraphFiles)
                return true;

            if (conf.GraphFilesAll is not null && conf.GraphFilesAll != last.GraphFilesAll)
                return true;

            if (conf.GraphFilesAllUpdate is not null && conf.GraphFilesAllUpdate != last.GraphFilesAllUpdate)
                return true;

            if (conf.GraphFilesUpdate is not null && conf.GraphFilesUpdate != last.GraphFilesUpdate)
                return true;

            if (conf.GraphNotes is not null && conf.GraphNotes != last.GraphNotes)
                return true;

            if (conf.GraphNotesCreate is not null && conf.GraphNotesCreate != last.GraphNotesCreate)
                return true;

            if (conf.GraphNotesUpdate is not null && conf.GraphNotesUpdate != last.GraphNotesUpdate)
                return true;

            if (conf.GraphTasks is not null && conf.GraphTasks != last.GraphTasks)
                return true;

            if (conf.GraphTasksUpdate is not null && conf.GraphTasksUpdate != last.GraphTasksUpdate)
                return true;

            if (conf.GraphUser is not null && conf.GraphUser != last.GraphUser)
                return true;

            if (conf.GraphUserActivity is not null && conf.GraphUserActivity != last.GraphUserActivity)
                return true;

            if (conf.GraphUserUpdate is not null && conf.GraphUserUpdate != last.GraphUserUpdate)
                return true;

            if (conf.GroupReadAll is not null && conf.GroupReadAll != last.GroupReadAll)
                return true;

            if (conf.GroupReadwriteAll is not null && conf.GroupReadwriteAll != last.GroupReadwriteAll)
                return true;

            if (conf.MailReadwriteAll is not null && conf.MailReadwriteAll != last.MailReadwriteAll)
                return true;

            if (conf.MailSend is not null && conf.MailSend != last.MailSend)
                return true;

            if (conf.Messenger is not null && conf.Messenger != last.Messenger)
                return true;

            if (conf.OfflineAccess is not null && conf.OfflineAccess != last.OfflineAccess)
                return true;

            if (conf.PhoneNumbers is not null && conf.PhoneNumbers != last.PhoneNumbers)
                return true;

            if (conf.Photos is not null && conf.Photos != last.Photos)
                return true;

            if (conf.PostalAddresses is not null && conf.PostalAddresses != last.PostalAddresses)
                return true;

            if (conf.RolemanagementReadAll is not null && conf.RolemanagementReadAll != last.RolemanagementReadAll)
                return true;

            if (conf.RolemanagementReadwriteDirectory is not null && conf.RolemanagementReadwriteDirectory != last.RolemanagementReadwriteDirectory)
                return true;

            if (conf.Share is not null && conf.Share != last.Share)
                return true;

            if (conf.Signin is not null && conf.Signin != last.Signin)
                return true;

            if (conf.SitesReadAll is not null && conf.SitesReadAll != last.SitesReadAll)
                return true;

            if (conf.SitesReadwriteAll is not null && conf.SitesReadwriteAll != last.SitesReadwriteAll)
                return true;

            if (conf.Skydrive is not null && conf.Skydrive != last.Skydrive)
                return true;

            if (conf.SkydriveUpdate is not null && conf.SkydriveUpdate != last.SkydriveUpdate)
                return true;

            if (conf.TeamReadbasicAll is not null && conf.TeamReadbasicAll != last.TeamReadbasicAll)
                return true;

            if (conf.TeamReadwriteAll is not null && conf.TeamReadwriteAll != last.TeamReadwriteAll)
                return true;

            if (conf.UserReadAll is not null && conf.UserReadAll != last.UserReadAll)
                return true;

            if (conf.UserReadbasicAll is not null && conf.UserReadbasicAll != last.UserReadbasicAll)
                return true;

            if (conf.WorkProfile is not null && conf.WorkProfile != last.WorkProfile)
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsWordpress? conf, V2alpha3ConnectionOptionsWordpress? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsYahoo? conf, V2alpha3ConnectionOptionsYahoo? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
        }

        static bool RequiresUpdate(V2alpha3ConnectionOptionsYandex? conf, V2alpha3ConnectionOptionsYandex? last)
        {
            if (conf is null)
                return false;

            if (last is null)
                return true;

            if (conf.ClientId is not null && conf.ClientId != last.ClientId)
                return true;

            if (conf.ClientSecret is not null && conf.ClientSecret != last.ClientSecret)
                return true;

            if (conf.Scope is not null && (last.Scope is null || conf.Scope.ToHashSet().SetEquals(last.Scope) == false))
                return true;

            if (conf.SetUserRootAttributes is not null && conf.SetUserRootAttributes != last.SetUserRootAttributes)
                return true;

            if (RequiresUpdate(conf.UpstreamParams, last.UpstreamParams))
                return true;

            if (conf.NonPersistentAttrs is not null && (last.NonPersistentAttrs is null || conf.NonPersistentAttrs.ToHashSet().SetEquals(last.NonPersistentAttrs) == false))
                return true;

            return false;
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
                await ApplyEnabledClientsAsync(api, entity, lastConf, defaultNamespace, cancellationToken);

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
        /// <param name="lastConf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task ApplyEnabledClientsAsync(IManagementApiClient api, V2alpha3Connection entity, V2alpha3ConnectionConf lastConf, string defaultNamespace, CancellationToken cancellationToken)
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

            // clients actually enabled right now, as read back by Get earlier this cycle
            var current = new HashSet<string>(lastConf.EnabledClients?.Select(i => i.Id).OfType<string>() ?? []);

            var req = ComputeEnabledClientsRequest(desired, previouslyManaged, current);
            if (req.Count > 0)
                await api.Connections.Clients.UpdateAsync(id, req, null, cancellationToken);

            // remember the set we now manage so future reconciles can scope disabling
            entity.Status.ManagedEnabledClientIds = desired.ToArray();
        }

        /// <summary>
        /// Computes the enabled-clients batch for a scoped, additive update: every <paramref name="desired"/> client
        /// not already in <paramref name="current"/> is enabled, and only <paramref name="previouslyManaged"/> clients
        /// that are no longer desired but still in <paramref name="current"/> are disabled. Clients already in the
        /// desired state are omitted so an in-sync connection produces an empty batch and no API call. Clients that
        /// were never managed by this Connection (e.g. enabled via a <c>ConnectionClient</c> resource) are never
        /// included, so they are left untouched.
        /// </summary>
        /// <param name="desired"></param>
        /// <param name="previouslyManaged"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        internal static List<UpdateEnabledClientConnectionsRequestContentItem> ComputeEnabledClientsRequest(ISet<string> desired, IEnumerable<string> previouslyManaged, ISet<string> current)
        {
            var req = new List<UpdateEnabledClientConnectionsRequestContentItem>();

            // enable every desired client not already enabled
            foreach (var clientId in desired)
                if (current.Contains(clientId) == false)
                    req.Add(new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = clientId, Status = true });

            // disable only previously-managed clients that are no longer desired and are still enabled
            foreach (var clientId in previouslyManaged)
                if (desired.Contains(clientId) == false && current.Contains(clientId))
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
