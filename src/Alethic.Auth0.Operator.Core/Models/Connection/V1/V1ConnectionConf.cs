using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Desired configuration for an Auth0 connection resource.
    /// </summary>
    public record V1ConnectionConf
    {

        /// <summary>
        /// The name of the connection. Must be unique for the tenant. Max length 35 characters and must start and end with an alphanumeric character and can only contain alphanumeric characters and '-'.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// The display name of the connection, shown to end users in the Universal Login experience.
        /// </summary>
        [JsonPropertyName("display_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// The identity provider identifier. Determines which strategy-specific options object is used.
        /// </summary>
        [JsonPropertyName("strategy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Strategy { get; set; }

        /// <summary>
        /// Provisioning ticket URL used for enterprise connections during setup.
        /// </summary>
        [JsonPropertyName("provisioning_ticket_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProvisioningTicketUrl { get; set; }

        /// <summary>
        /// Metadata associated with the connection in the form of an object with string values (max 255 chars). A maximum of 10 metadata properties are allowed.
        /// </summary>
        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
        public Hashtable? Metadata { get; set; }

        /// <summary>
        /// Defines the realms for which the connection will be used (e.g. email domains). If not specified, the connection name will be added as realm.
        /// </summary>
        [JsonPropertyName("realms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Realms { get; set; }

        /// <summary>
        /// The list of clients for which the connection is enabled.
        /// </summary>
        [JsonPropertyName("enabled_clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? EnabledClients { get; set; }

        /// <summary>
        /// Whether the connection is shown as a button. Only for enterprise connections.
        /// </summary>
        [JsonPropertyName("show_as_button")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowAsButton { get; set; }

        /// <summary>
        /// True if the connection is a domain level connection, false otherwise.
        /// </summary>
        [JsonPropertyName("is_domain_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsDomainConnection { get; set; } = false;

        /// <summary>
        /// Strategy-specific options for the <c>auth0</c> (database) connection strategy.
        /// </summary>
        [JsonPropertyName("auth0Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAuth0Options? Auth0Options { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>ad</c> (Active Directory / LDAP) connection strategy.
        /// </summary>
        [JsonPropertyName("adOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAdOptions? AdOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>adfs</c> (Active Directory Federation Services) connection strategy.
        /// </summary>
        [JsonPropertyName("adfsOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAdfsOptions? AdfsOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>auth0-oidc</c> connection strategy (Auth0 tenant as OIDC provider).
        /// </summary>
        [JsonPropertyName("auth0OidcOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAuth0OidcOptions? Auth0OidcOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>waad</c> (Azure Active Directory) connection strategy.
        /// </summary>
        [JsonPropertyName("azureAdOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAzureAdOptions? AzureAdOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>bitbucket</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("bitbucketOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionBitbucketOptions? BitbucketOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>box</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("boxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionBoxOptions? BoxOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>dropbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("dropboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionDropboxOptions? DropboxOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>email</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("emailOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEmailOptions? EmailOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernoteOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEvernoteOptions? EvernoteOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>evernote-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("evernoteSandboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEvernoteOptions? EvernoteSandboxOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>exact</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("exactOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionExactOptions? ExactOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>facebook</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("facebookOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionFacebookOptions? FacebookOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>github</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("gitHubOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGitHubOptions? GitHubOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-apps</c> (Google Workspace) enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("googleAppsOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleAppsOptions? GoogleAppsOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>google-oauth2</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("googleOAuth2Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleOAuth2Options? GoogleOAuth2Options { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>linkedin</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("linkedinOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionLinkedinOptions? LinkedinOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth1</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oAuth1Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOAuth1Options? OAuth1Options { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oauth2</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oAuth2Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOAuth2Options? OAuth2Options { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>office365</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("office365Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOffice365Options? Office365Options { get; set; }

        /// <summary>
        /// Strategy-specific options for a generic <c>oidc</c> connection strategy.
        /// </summary>
        [JsonPropertyName("oidcOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOidcOptions? OidcOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>okta</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("oktaOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOktaOptions? OktaOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypalOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPaypalOptions? PaypalOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>paypal-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("paypalSandboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPaypalOptions? PaypalSandboxOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>pingfederate</c> enterprise connection strategy.
        /// </summary>
        [JsonPropertyName("pingFederateOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPingFederateOptions? PingFederateOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceOptions? SalesforceOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-community</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceCommunityOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceCommunityOptions? SalesforceCommunityOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>salesforce-sandbox</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("salesforceSandboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceOptions? SalesforceSandboxOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for a <c>samlp</c> (SAML Identity Provider) connection strategy.
        /// </summary>
        [JsonPropertyName("samlOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSamlOptions? SamlOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>sms</c> (passwordless) connection strategy.
        /// </summary>
        [JsonPropertyName("smsOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSmsOptions? SmsOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>twitter</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("twitterOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionTwitterOptions? TwitterOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>windowslive</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("windowsLiveOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionWindowsLiveOptions? WindowsLiveOptions { get; set; }

        /// <summary>
        /// Strategy-specific options for the <c>yahoo</c> social connection strategy.
        /// </summary>
        [JsonPropertyName("yahooOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionYahooOptions? YahooOptions { get; set; }

    }

}
