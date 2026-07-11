using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;
using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Connections;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    /// <summary>
    /// Provides conversions targeting <see cref="V2alpha3Connection"/>.
    /// </summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3Connection))]
    public class ConnectionConverter : ConversionWebhook<V2alpha3Connection>
    {

        static JsonSerializerOptions GetAuth0JsonSerializerOptions()
        {
            var type = typeof(ConnectionOptionsAuth0).Assembly.GetType("Auth0.ManagementApi.Core.JsonOptions")
                ?? throw new InvalidOperationException("Unable to locate Auth0.ManagementApi.Core.JsonOptions.");

            return type.GetField("JsonSerializerOptions", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as JsonSerializerOptions
                ?? throw new InvalidOperationException("Unable to resolve Auth0 JSON serializer options.");
        }

        protected override IEnumerable<IEntityConverter<V2alpha3Connection>> Converters => [
            new V1ToV2alpha3(),
            new V2alpha1ToV2alpha3()
        ];

        /// <summary>
        /// Converts between <see cref="V1Connection"/> and <see cref="V2alpha3Connection"/>.
        /// </summary>
        class V1ToV2alpha3 : IEntityConverter<V1Connection, V2alpha3Connection>
        {

            static readonly JsonSerializerOptions Auth0JsonSerializerOptions = GetAuth0JsonSerializerOptions();

            public V2alpha3Connection Convert(V1Connection from)
            {
                var result = new V2alpha3Connection { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Find = from.Spec.Find is { } find ? new V2alpha3ConnectionFind { ConnectionId = find.ConnectionId } : null;
                result.Spec.Init = ConvertConf(from.Spec.Init);
                result.Spec.Conf = ConvertConf(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = ConvertConf(from.Status.LastConf);
                return result;
            }

            public V1Connection Revert(V2alpha3Connection source)
            {
                var result = new V1Connection { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Find = source.Spec.Find is { } find ? new V1ConnectionFind { ConnectionId = find.ConnectionId } : null;
                result.Spec.Init = RevertConf(source.Spec.Init);
                result.Spec.Conf = RevertConf(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = RevertConf(source.Status.LastConf);
                return result;
            }

            static V2alpha3ConnectionConf? ConvertConf(V1ConnectionConf? source)
            {
                if (source is null)
                    return null;

                var strategy = source.Strategy switch
                {
                    ConnectionResponseContentAuth0Strategy.Values.Auth0 => V2alpha3ConnectionStrategy.Auth0,
                    ConnectionResponseContentAdStrategy.Values.Ad => V2alpha3ConnectionStrategy.Ad,
                    ConnectionResponseContentAdfsStrategy.Values.Adfs => V2alpha3ConnectionStrategy.Adfs,
                    ConnectionResponseContentAmazonStrategy.Values.Amazon => V2alpha3ConnectionStrategy.Amazon,
                    ConnectionResponseContentAppleStrategy.Values.Apple => V2alpha3ConnectionStrategy.Apple,
                    ConnectionResponseContentAuth0OidcStrategy.Values.Auth0Oidc => V2alpha3ConnectionStrategy.Auth0Oidc,
                    ConnectionResponseContentAzureAdStrategy.Values.Waad => V2alpha3ConnectionStrategy.AzureAd,
                    ConnectionResponseContentBaiduStrategy.Values.Baidu => V2alpha3ConnectionStrategy.Baidu,
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
                    _ => (V2alpha3ConnectionStrategy?)null,
                };

                return new V2alpha3ConnectionConf
                {
                    Name = source.Name,
                    DisplayName = source.DisplayName,
                    Strategy = strategy,
                    ProvisioningTicketUrl = source.ProvisioningTicketUrl,
                    Metadata = source.Metadata,
                    Realms = source.Realms,
                    EnabledClients = source.EnabledClients,
                    ShowAsButton = source.ShowAsButton,
                    IsDomainConnection = source.IsDomainConnection,
                    Options = strategy is { } s ? ConvertOptions(s, source.Options) : null,
                };
            }

            static V1ConnectionConf? RevertConf(V2alpha3ConnectionConf? source)
            {
                if (source is null)
                    return null;

                return new V1ConnectionConf
                {
                    Name = source.Name,
                    DisplayName = source.DisplayName,
                    Strategy = source.Strategy is { } sv ? JsonSerializer.SerializeToElement(sv).GetString() : null,
                    ProvisioningTicketUrl = source.ProvisioningTicketUrl,
                    Metadata = source.Metadata,
                    Realms = source.Realms,
                    EnabledClients = source.EnabledClients,
                    ShowAsButton = source.ShowAsButton,
                    IsDomainConnection = source.IsDomainConnection,
                    Options = source.Strategy is { } s ? RevertOptions(s, source.Options) : null,

                };
            }

            static V2alpha3ConnectionOptions? ConvertOptions(V2alpha3ConnectionStrategy strategy, V1ConnectionOptions? source)
            {
                if (source is null)
                    return null;

                var json = JsonSerializer.SerializeToElement(source);
                var options = new V2alpha3ConnectionOptions();

                switch (strategy)
                {
                    case V2alpha3ConnectionStrategy.Auth0:
                        options.Auth0 = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsAuth0>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Ad:
                        options.Ad = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsAd>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Adfs:
                        options.Adfs = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsAdfs>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Amazon:
                        options.Amazon = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsAmazon>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Apple:
                        options.Apple = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsApple>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Auth0Oidc:
                        options.Auth0Oidc = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsAuth0Oidc>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.AzureAd:
                        options.AzureAd = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsAzureAd>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Baidu:
                        options.Baidu = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsBaidu>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Bitbucket:
                        options.Bitbucket = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsBitbucket>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Bitly:
                        options.Bitly = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsBitly>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Box:
                        options.Box = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsBox>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Daccount:
                        options.Daccount = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsDaccount>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Dropbox:
                        options.Dropbox = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsDropbox>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Dwolla:
                        options.Dwolla = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsDwolla>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Email:
                        options.Email = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsEmail>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Evernote:
                        options.Evernote = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsEvernote>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.EvernoteSandbox:
                        options.EvernoteSandbox = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsEvernote>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Exact:
                        options.Exact = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsExact>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Facebook:
                        options.Facebook = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsFacebook>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Fitbit:
                        options.Fitbit = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsFitbit>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.GitHub:
                        options.GitHub = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsGitHub>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.GoogleApps:
                        options.GoogleApps = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsGoogleApps>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.GoogleOAuth2:
                        options.GoogleOAuth2 = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsGoogleOAuth2>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Instagram:
                        options.Instagram = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsInstagram>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Line:
                        options.Line = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsLine>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Linkedin:
                        options.Linkedin = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsLinkedin>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.OAuth1:
                        options.OAuth1 = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsOAuth1>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.OAuth2:
                        options.OAuth2 = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsOAuth2>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Office365:
                        options.Office365 = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsOffice365>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Oidc:
                        options.Oidc = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsOidc>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Okta:
                        options.Okta = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsOkta>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Paypal:
                        options.Paypal = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsPaypal>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.PaypalSandbox:
                        options.PaypalSandbox = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsPaypal>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.PingFederate:
                        options.PingFederate = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsPingFederate>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.PlanningCenter:
                        options.PlanningCenter = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsPlanningCenter>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Salesforce:
                        options.Salesforce = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSalesforce>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.SalesforceCommunity:
                        options.SalesforceCommunity = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSalesforceCommunity>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.SalesforceSandbox:
                        options.SalesforceSandbox = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSalesforce>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Saml:
                        options.Saml = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSaml>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Sharepoint:
                        options.Sharepoint = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSharepoint>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Shop:
                        options.Shop = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsShop>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Shopify:
                        options.Shopify = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsShopify>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Sms:
                        options.Sms = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSms>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Soundcloud:
                        options.Soundcloud = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsSoundcloud>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.ThirtySevenSignals:
                        options.ThirtySevenSignals = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsThirtySevenSignals>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Twitter:
                        options.Twitter = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsTwitter>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Untappd:
                        options.Untappd = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsUntappd>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Vkontakte:
                        options.Vkontakte = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsVkontakte>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Weibo:
                        options.Weibo = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsWeibo>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.WindowsLive:
                        options.WindowsLive = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsWindowsLive>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Wordpress:
                        options.Wordpress = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsWordpress>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Yahoo:
                        options.Yahoo = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsYahoo>(Auth0JsonSerializerOptions));
                        break;
                    case V2alpha3ConnectionStrategy.Yandex:
                        options.Yandex = V2alpha3ConnectionController.FromApi(json.Deserialize<ConnectionOptionsYandex>(Auth0JsonSerializerOptions));
                        break;
                }

                return options;
            }

            static V1ConnectionOptions? RevertOptions(V2alpha3ConnectionStrategy strategy, V2alpha3ConnectionOptions? source)
            {
                if (source is null)
                    return null;

                object? options = strategy switch
                {
                    V2alpha3ConnectionStrategy.Auth0 => source.Auth0 is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Ad => source.Ad is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Adfs => source.Adfs is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Amazon => source.Amazon is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Apple => source.Apple is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Auth0Oidc => source.Auth0Oidc is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.AzureAd => source.AzureAd is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Baidu => source.Baidu is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Bitbucket => source.Bitbucket is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Bitly => source.Bitly is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Box => source.Box is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Daccount => source.Daccount is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Dropbox => source.Dropbox is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Dwolla => source.Dwolla is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Email => source.Email is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Evernote => source.Evernote is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.EvernoteSandbox => source.EvernoteSandbox is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Exact => source.Exact is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Facebook => source.Facebook is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Fitbit => source.Fitbit is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.GitHub => source.GitHub is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.GoogleApps => source.GoogleApps is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.GoogleOAuth2 => source.GoogleOAuth2 is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Instagram => source.Instagram is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Line => source.Line is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Linkedin => source.Linkedin is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.OAuth1 => source.OAuth1 is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.OAuth2 => source.OAuth2 is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Office365 => source.Office365 is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Oidc => source.Oidc is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Okta => source.Okta is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Paypal => source.Paypal is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.PaypalSandbox => source.PaypalSandbox is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.PingFederate => source.PingFederate is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.PlanningCenter => source.PlanningCenter is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Salesforce => source.Salesforce is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.SalesforceCommunity => source.SalesforceCommunity is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.SalesforceSandbox => source.SalesforceSandbox is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Saml => source.Saml is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Sharepoint => source.Sharepoint is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Shop => source.Shop is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Shopify => source.Shopify is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Sms => source.Sms is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Soundcloud => source.Soundcloud is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.ThirtySevenSignals => source.ThirtySevenSignals is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Twitter => source.Twitter is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Untappd => source.Untappd is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Vkontakte => source.Vkontakte is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Weibo => source.Weibo is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.WindowsLive => source.WindowsLive is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Wordpress => source.Wordpress is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Yahoo => source.Yahoo is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    V2alpha3ConnectionStrategy.Yandex => source.Yandex is { } v ? V2alpha3ConnectionController.ToApi(v) : null,
                    _ => null,
                };

                return options is not null
                    ? JsonSerializer.Deserialize<V1ConnectionOptions>(JsonSerializer.Serialize(options, Auth0JsonSerializerOptions))
                    : null;
            }

        }


        /// <summary>Structural (property-copy) conversion between the
        /// structurally-identical V2alpha1 and V2alpha3 Connection models.</summary>
        class V2alpha1ToV2alpha3 : IEntityConverter<Models.V2alpha1Connection, Models.V2alpha3Connection>
        {

            public Models.V2alpha3Connection Convert(Models.V2alpha1Connection from)
            {
                var result = new Models.V2alpha3Connection { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Find = ConnectionCopy.Convert(from.Spec.Find);
                result.Spec.Init = ConnectionCopy.Convert(from.Spec.Init);
                result.Spec.Conf = ConnectionCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = ConnectionCopy.Convert(from.Status.LastConf);
                return result;
            }

            public Models.V2alpha1Connection Revert(Models.V2alpha3Connection source)
            {
                var result = new Models.V2alpha1Connection { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Find = ConnectionCopy.Revert(source.Spec.Find);
                result.Spec.Init = ConnectionCopy.Revert(source.Spec.Init);
                result.Spec.Conf = ConnectionCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = ConnectionCopy.Revert(source.Status.LastConf);
                return result;
            }

        }
    }

}
