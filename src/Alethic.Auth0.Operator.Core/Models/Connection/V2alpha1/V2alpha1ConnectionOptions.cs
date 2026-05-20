using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Set of typed configuration options.
    /// </summary>
    public record V2alpha1ConnectionOptions
    {

        /// <summary>
        /// Strategy-specific options for the <c>auth0</c> (database) connection strategy.
        /// </summary>
        [JsonPropertyName("auth0")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsAuth0? Auth0 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>ad</c> (Active Directory / LDAP) connection strategy.
        /// </summary>
        [JsonPropertyName("ad")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsAd? Ad { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>adfs</c> (Active Directory Federation Services) connection strategy.
        /// </summary>
        [JsonPropertyName("adfs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsAdfs? Adfs { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>auth0-oidc</c> connection strategy (Auth0 tenant as OIDC provider).
        /// </summary>
        [JsonPropertyName("auth0Oidc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsAuth0Oidc? Auth0Oidc { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>waad</c> (Azure Active Directory) connection strategy.
        /// </summary>
        [JsonPropertyName("azureAd")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsAzureAd? AzureAd { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>bitbucket</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("bitbucket")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsBitbucket? Bitbucket { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>box</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("box")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsBox? Box { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>dropbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("dropbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsDropbox? Dropbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>email</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsEmail? Email { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernote")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsEvernote? Evernote { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernoteSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsEvernote? EvernoteSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>exact</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("exact")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsExact? Exact { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>facebook</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("facebook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsFacebook? Facebook { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>github</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("gitHub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsGitHub? GitHub { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-apps</c> (Google Workspace) enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("googleApps")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsGoogleApps? GoogleApps { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-oauth2</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("googleOAuth2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsGoogleOAuth2? GoogleOAuth2 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>linkedin</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("linkedin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsLinkedin? Linkedin { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth1</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oAuth1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsOAuth1? OAuth1 { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth2</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oAuth2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsOAuth2? OAuth2 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>office365</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("office365")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsOffice365? Office365 { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oidc</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oidc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsOidc? Oidc { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>okta</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("okta")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsOkta? Okta { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPaypal? Paypal { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypalSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPaypal? PaypalSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>pingfederate</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("pingFederate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPingFederate? PingFederate { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsSalesforce? Salesforce { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-community</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceCommunity")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsSalesforceCommunity? SalesforceCommunity { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsSalesforce? SalesforceSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for a <c>samlp</c> (SAML Identity Provider) connection strategy.
        /// </summary>
        [JsonPropertyName("saml")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsSaml? Saml { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>sms</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("sms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsSms? Sms { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>twitter</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("twitter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsTwitter? Twitter { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>windowslive</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("windowsLive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsWindowsLive? WindowsLive { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>yahoo</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("yahoo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsYahoo? Yahoo { get; set; }

    }

}
