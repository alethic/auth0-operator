using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Set of typed configuration options.
    /// </summary>
    public record V2alpha3ConnectionOptions
    {

        /// <summary>
        /// Strategy-specific options for the <c>auth0</c> (database) connection strategy.
        /// </summary>
        [JsonPropertyName("auth0")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAuth0? Auth0 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>ad</c> (Active Directory / LDAP) connection strategy.
        /// </summary>
        [JsonPropertyName("ad")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAd? Ad { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>adfs</c> (Active Directory Federation Services) connection strategy.
        /// </summary>
        [JsonPropertyName("adfs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAdfs? Adfs { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>amazon</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("amazon")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAmazon? Amazon { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>apple</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("apple")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsApple? Apple { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>auth0-oidc</c> connection strategy (Auth0 tenant as OIDC provider).
        /// </summary>
        [JsonPropertyName("auth0Oidc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAuth0Oidc? Auth0Oidc { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>waad</c> (Azure Active Directory) connection strategy.
        /// </summary>
        [JsonPropertyName("waad")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsAzureAd? AzureAd { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>baidu</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("baidu")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsBaidu? Baidu { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>bitbucket</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("bitbucket")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsBitbucket? Bitbucket { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>bitly</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("bitly")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsBitly? Bitly { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>box</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("box")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsBox? Box { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>daccount</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("daccount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsDaccount? Daccount { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>dropbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("dropbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsDropbox? Dropbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>dwolla</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("dwolla")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsDwolla? Dwolla { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>email</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsEmail? Email { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernote")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsEvernote? Evernote { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernoteSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsEvernote? EvernoteSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>exact</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("exact")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsExact? Exact { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>facebook</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("facebook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsFacebook? Facebook { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>fitbit</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("fitbit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsFitbit? Fitbit { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>github</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("github")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsGitHub? GitHub { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-apps</c> (Google Workspace) enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("googleApps")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsGoogleApps? GoogleApps { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-oauth2</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("googleOauth2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsGoogleOAuth2? GoogleOAuth2 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>instagram</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("instagram")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsInstagram? Instagram { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>line</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("line")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsLine? Line { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>linkedin</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("linkedin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsLinkedin? Linkedin { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth1</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oauth1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsOAuth1? OAuth1 { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth2</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oauth2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsOAuth2? OAuth2 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>office365</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("office365")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsOffice365? Office365 { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oidc</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oidc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsOidc? Oidc { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>okta</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("okta")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsOkta? Okta { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPaypal? Paypal { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypalSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPaypal? PaypalSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>pingfederate</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("pingfederate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPingFederate? PingFederate { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>planningcenter</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("planningcenter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsPlanningCenter? PlanningCenter { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSalesforce? Salesforce { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-community</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceCommunity")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSalesforceCommunity? SalesforceCommunity { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSalesforce? SalesforceSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for a <c>samlp</c> (SAML Identity Provider) connection strategy.
        /// </summary>
        [JsonPropertyName("samlp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSaml? Saml { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>sharepoint</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("sharepoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSharepoint? Sharepoint { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>shop</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("shop")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsShop? Shop { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>shopify</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("shopify")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsShopify? Shopify { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>sms</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("sms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSms? Sms { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>soundcloud</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("soundcloud")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsSoundcloud? Soundcloud { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>thirtysevensignals</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("thirtysevensignals")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsThirtySevenSignals? ThirtySevenSignals { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>twitter</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("twitter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsTwitter? Twitter { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>untappd</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("untappd")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsUntappd? Untappd { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>vkontakte</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("vkontakte")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsVkontakte? Vkontakte { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>weibo</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("weibo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsWeibo? Weibo { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>windowslive</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("windowslive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsWindowsLive? WindowsLive { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>wordpress</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("wordpress")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsWordpress? Wordpress { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>yahoo</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("yahoo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsYahoo? Yahoo { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>yandex</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("yandex")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3ConnectionOptionsYandex? Yandex { get; set; }

    }

}
