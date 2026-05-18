using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Set of typed configuration options.
    /// </summary>
    public record V1ConnectionOptions
    {

        /// <summary>
        /// Strategy-specific options for the <c>auth0</c> (database) connection strategy.
        /// </summary>
        [JsonPropertyName("auth0")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAuth0Options? Auth0 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>ad</c> (Active Directory / LDAP) connection strategy.
        /// </summary>
        [JsonPropertyName("ad")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAdOptions? Ad { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>adfs</c> (Active Directory Federation Services) connection strategy.
        /// </summary>
        [JsonPropertyName("adfs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAdfsOptions? Adfs { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>auth0-oidc</c> connection strategy (Auth0 tenant as OIDC provider).
        /// </summary>
        [JsonPropertyName("auth0Oidc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAuth0OidcOptions? Auth0Oidc { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>waad</c> (Azure Active Directory) connection strategy.
        /// </summary>
        [JsonPropertyName("azureAd")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAzureAdOptions? AzureAd { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>bitbucket</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("bitbucket")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionBitbucketOptions? Bitbucket { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>box</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("box")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionBoxOptions? Box { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>dropbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("dropbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionDropboxOptions? Dropbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>email</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEmailOptions? Email { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernote")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEvernoteOptions? Evernote { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernoteSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEvernoteOptions? EvernoteSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>exact</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("exact")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionExactOptions? Exact { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>facebook</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("facebook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionFacebookOptions? Facebook { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>github</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("gitHub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGitHubOptions? GitHub { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-apps</c> (Google Workspace) enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("googleApps")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleAppsOptions? GoogleApps { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-oauth2</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("googleOAuth2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleOAuth2Options? GoogleOAuth2 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>linkedin</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("linkedin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionLinkedinOptions? Linkedin { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth1</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oAuth1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOAuth1Options? OAuth1 { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth2</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oAuth2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOAuth2Options? OAuth2 { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>office365</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("office365")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOffice365Options? Office365 { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oidc</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oidc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOidcOptions? Oidc { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>okta</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("okta")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOktaOptions? Okta { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPaypalOptions? Paypal { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypalSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPaypalOptions? PaypalSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>pingfederate</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("pingFederate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPingFederateOptions? PingFederate { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceOptions? Salesforce { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-community</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceCommunity")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceCommunityOptions? SalesforceCommunity { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceSandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceOptions? SalesforceSandbox { get; set; }

        /// <summary>
        /// Strategy-specific options for a <c>samlp</c> (SAML Identity Provider) connection strategy.
        /// </summary>
        [JsonPropertyName("saml")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSamlOptions? Saml { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>sms</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("sms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSmsOptions? Sms { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>twitter</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("twitter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionTwitterOptions? Twitter { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>windowslive</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("windowsLive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionWindowsLiveOptions? WindowsLive { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>yahoo</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("yahoo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionYahooOptions? Yahoo { get; set; }

    }

}
