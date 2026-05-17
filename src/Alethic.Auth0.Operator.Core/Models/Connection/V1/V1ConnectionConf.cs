using System.Collections;
using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Extensions;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionConf
    {

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("display_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Discriminator field that identifies which strategy-specific options object is populated.
        /// </summary>
        [JsonPropertyName("strategy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Strategy { get; set; }

        [JsonPropertyName("provisioning_ticket_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProvisioningTicketUrl { get; set; }

        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(SimplePrimitiveHashtableConverter))]
        public Hashtable? Metadata { get; set; }

        [JsonPropertyName("realms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Realms { get; set; }

        [JsonPropertyName("enabled_clients")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ClientReference[]? EnabledClients { get; set; }

        [JsonPropertyName("show_as_button")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowAsButton { get; set; }

        [JsonPropertyName("is_domain_connection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsDomainConnection { get; set; } = false;

        // ── Strategy-specific options (discriminated union) ────────────────────

        /// <summary>
        /// Options for the <c>auth0</c> database connection strategy. Also used as the fallback
        /// for unknown strategies via <see cref="V1ConnectionOptions.AdditionalProperties"/>.
        /// </summary>
        [JsonPropertyName("auth0Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAuth0Options? Auth0Options { get; set; }

        [JsonPropertyName("adOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAdOptions? AdOptions { get; set; }

        [JsonPropertyName("adfsOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAdfsOptions? AdfsOptions { get; set; }

        [JsonPropertyName("auth0OidcOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAuth0OidcOptions? Auth0OidcOptions { get; set; }

        [JsonPropertyName("azureAdOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionAzureAdOptions? AzureAdOptions { get; set; }

        [JsonPropertyName("bitbucketOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionBitbucketOptions? BitbucketOptions { get; set; }

        [JsonPropertyName("boxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionBoxOptions? BoxOptions { get; set; }

        [JsonPropertyName("dropboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionDropboxOptions? DropboxOptions { get; set; }

        [JsonPropertyName("emailOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEmailOptions? EmailOptions { get; set; }

        [JsonPropertyName("evernoteOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEvernoteOptions? EvernoteOptions { get; set; }

        [JsonPropertyName("evernoteSandboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEvernoteSandboxOptions? EvernoteSandboxOptions { get; set; }

        [JsonPropertyName("exactOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionExactOptions? ExactOptions { get; set; }

        [JsonPropertyName("facebookOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionFacebookOptions? FacebookOptions { get; set; }

        [JsonPropertyName("gitHubOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGitHubOptions? GitHubOptions { get; set; }

        [JsonPropertyName("googleAppsOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleAppsOptions? GoogleAppsOptions { get; set; }

        [JsonPropertyName("googleOAuth2Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGoogleOAuth2Options? GoogleOAuth2Options { get; set; }

        [JsonPropertyName("linkedinOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionLinkedinOptions? LinkedinOptions { get; set; }

        [JsonPropertyName("oAuth1Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOAuth1Options? OAuth1Options { get; set; }

        [JsonPropertyName("oAuth2Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOAuth2Options? OAuth2Options { get; set; }

        [JsonPropertyName("office365Options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOffice365Options? Office365Options { get; set; }

        [JsonPropertyName("oidcOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOidcOptions? OidcOptions { get; set; }

        [JsonPropertyName("oktaOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOktaOptions? OktaOptions { get; set; }

        [JsonPropertyName("paypalOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPaypalOptions? PaypalOptions { get; set; }

        [JsonPropertyName("paypalSandboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPaypalSandboxOptions? PaypalSandboxOptions { get; set; }

        [JsonPropertyName("pingFederateOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionPingFederateOptions? PingFederateOptions { get; set; }

        [JsonPropertyName("salesforceOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceOptions? SalesforceOptions { get; set; }

        [JsonPropertyName("salesforceCommunityOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceCommunityOptions? SalesforceCommunityOptions { get; set; }

        [JsonPropertyName("salesforceSandboxOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSalesforceSandboxOptions? SalesforceSandboxOptions { get; set; }

        [JsonPropertyName("samlOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSamlOptions? SamlOptions { get; set; }

        [JsonPropertyName("smsOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSmsOptions? SmsOptions { get; set; }

        [JsonPropertyName("twitterOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionTwitterOptions? TwitterOptions { get; set; }

        [JsonPropertyName("windowsLiveOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionWindowsLiveOptions? WindowsLiveOptions { get; set; }

        [JsonPropertyName("yahooOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionYahooOptions? YahooOptions { get; set; }

    }

}
