using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Identity provider strategy for an Auth0 connection.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha1ConnectionStrategy
    {

        /// <summary>Auth0 database connection.</summary>
        [JsonStringEnumMemberName("auth0")]
        Auth0,

        /// <summary>Active Directory / LDAP connection.</summary>
        [JsonStringEnumMemberName("ad")]
        Ad,

        /// <summary>Active Directory Federation Services connection.</summary>
        [JsonStringEnumMemberName("adfs")]
        Adfs,

        /// <summary>Amazon social connection.</summary>
        [JsonStringEnumMemberName("amazon")]
        Amazon,

        /// <summary>Apple social connection.</summary>
        [JsonStringEnumMemberName("apple")]
        Apple,

        /// <summary>Auth0 tenant as OIDC provider connection.</summary>
        [JsonStringEnumMemberName("auth0-oidc")]
        Auth0Oidc,

        /// <summary>Baidu social connection.</summary>
        [JsonStringEnumMemberName("baidu")]
        Baidu,

        /// <summary>Azure Active Directory (Microsoft Entra ID) connection.</summary>
        [JsonStringEnumMemberName("waad")]
        AzureAd,

        /// <summary>Bitbucket social connection.</summary>
        [JsonStringEnumMemberName("bitbucket")]
        Bitbucket,

        /// <summary>Bitly social connection.</summary>
        [JsonStringEnumMemberName("bitly")]
        Bitly,

        /// <summary>Box social connection.</summary>
        [JsonStringEnumMemberName("box")]
        Box,

        /// <summary>DAccount social connection.</summary>
        [JsonStringEnumMemberName("daccount")]
        Daccount,

        /// <summary>Dropbox social connection.</summary>
        [JsonStringEnumMemberName("dropbox")]
        Dropbox,

        /// <summary>Dwolla social connection.</summary>
        [JsonStringEnumMemberName("dwolla")]
        Dwolla,

        /// <summary>Email passwordless connection.</summary>
        [JsonStringEnumMemberName("email")]
        Email,

        /// <summary>Evernote social connection.</summary>
        [JsonStringEnumMemberName("evernote")]
        Evernote,

        /// <summary>Evernote sandbox social connection.</summary>
        [JsonStringEnumMemberName("evernote-sandbox")]
        EvernoteSandbox,

        /// <summary>Exact social connection.</summary>
        [JsonStringEnumMemberName("exact")]
        Exact,

        /// <summary>Facebook social connection.</summary>
        [JsonStringEnumMemberName("facebook")]
        Facebook,

        /// <summary>Fitbit social connection.</summary>
        [JsonStringEnumMemberName("fitbit")]
        Fitbit,

        /// <summary>GitHub social connection.</summary>
        [JsonStringEnumMemberName("github")]
        GitHub,

        /// <summary>Google Workspace (Google Apps) enterprise connection.</summary>
        [JsonStringEnumMemberName("google-apps")]
        GoogleApps,

        /// <summary>Google OAuth2 social connection.</summary>
        [JsonStringEnumMemberName("google-oauth2")]
        GoogleOAuth2,

        /// <summary>Instagram social connection.</summary>
        [JsonStringEnumMemberName("instagram")]
        Instagram,

        /// <summary>LINE social connection.</summary>
        [JsonStringEnumMemberName("line")]
        Line,

        /// <summary>LinkedIn social connection.</summary>
        [JsonStringEnumMemberName("linkedin")]
        Linkedin,

        /// <summary>Generic OAuth 1.0 connection.</summary>
        [JsonStringEnumMemberName("oauth1")]
        OAuth1,

        /// <summary>Generic OAuth 2.0 connection.</summary>
        [JsonStringEnumMemberName("oauth2")]
        OAuth2,

        /// <summary>Office 365 enterprise connection.</summary>
        [JsonStringEnumMemberName("office365")]
        Office365,

        /// <summary>Generic OpenID Connect connection.</summary>
        [JsonStringEnumMemberName("oidc")]
        Oidc,

        /// <summary>Okta enterprise connection.</summary>
        [JsonStringEnumMemberName("okta")]
        Okta,

        /// <summary>PayPal social connection.</summary>
        [JsonStringEnumMemberName("paypal")]
        Paypal,

        /// <summary>PayPal sandbox social connection.</summary>
        [JsonStringEnumMemberName("paypal-sandbox")]
        PaypalSandbox,

        /// <summary>PingFederate enterprise connection.</summary>
        [JsonStringEnumMemberName("pingfederate")]
        PingFederate,

        /// <summary>Planning Center social connection.</summary>
        [JsonStringEnumMemberName("planningcenter")]
        PlanningCenter,

        /// <summary>Salesforce social connection.</summary>
        [JsonStringEnumMemberName("salesforce")]
        Salesforce,

        /// <summary>Salesforce Community social connection.</summary>
        [JsonStringEnumMemberName("salesforce-community")]
        SalesforceCommunity,

        /// <summary>Salesforce sandbox social connection.</summary>
        [JsonStringEnumMemberName("salesforce-sandbox")]
        SalesforceSandbox,

        /// <summary>SAML Identity Provider connection.</summary>
        [JsonStringEnumMemberName("samlp")]
        Saml,

        /// <summary>SharePoint social connection.</summary>
        [JsonStringEnumMemberName("sharepoint")]
        Sharepoint,

        /// <summary>Shop social connection.</summary>
        [JsonStringEnumMemberName("shop")]
        Shop,

        /// <summary>Shopify social connection.</summary>
        [JsonStringEnumMemberName("shopify")]
        Shopify,

        /// <summary>SMS passwordless connection.</summary>
        [JsonStringEnumMemberName("sms")]
        Sms,

        /// <summary>SoundCloud social connection.</summary>
        [JsonStringEnumMemberName("soundcloud")]
        Soundcloud,

        /// <summary>37signals social connection.</summary>
        [JsonStringEnumMemberName("thirtysevensignals")]
        ThirtySevenSignals,

        /// <summary>Twitter social connection.</summary>
        [JsonStringEnumMemberName("twitter")]
        Twitter,

        /// <summary>Untappd social connection.</summary>
        [JsonStringEnumMemberName("untappd")]
        Untappd,

        /// <summary>VKontakte social connection.</summary>
        [JsonStringEnumMemberName("vkontakte")]
        Vkontakte,

        /// <summary>Weibo social connection.</summary>
        [JsonStringEnumMemberName("weibo")]
        Weibo,

        /// <summary>Windows Live social connection.</summary>
        [JsonStringEnumMemberName("windowslive")]
        WindowsLive,

        /// <summary>WordPress social connection.</summary>
        [JsonStringEnumMemberName("wordpress")]
        Wordpress,

        /// <summary>Yahoo social connection.</summary>
        [JsonStringEnumMemberName("yahoo")]
        Yahoo,

        /// <summary>Yandex social connection.</summary>
        [JsonStringEnumMemberName("yandex")]
        Yandex,

    }

}
