using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha1", Kind = "Connection")]
    [KubernetesEntityShortNames("a0con")]
    public partial class V2alpha1Connection :
        CustomKubernetesEntity<V2alpha1Connection.SpecDef, V2alpha1Connection.StatusDef>,
        V1TenantEntityInstance<V2alpha1Connection.SpecDef, V2alpha1Connection.StatusDef, V2alpha1ConnectionConf, V2alpha1ConnectionConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha1ConnectionConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("find")]
            public V2alpha1ConnectionFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V2alpha1ConnectionConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            [ValidationRule("!has(self.strategy) || self.strategy == 'auth0' ? true : !has(self.auth0Options)", "auth0Options must only be set when strategy is 'auth0'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'ad' ? true : !has(self.adOptions)", "adOptions must only be set when strategy is 'ad'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'adfs' ? true : !has(self.adfsOptions)", "adfsOptions must only be set when strategy is 'adfs'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'auth0-oidc' ? true : !has(self.auth0OidcOptions)", "auth0OidcOptions must only be set when strategy is 'auth0-oidc'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'waad' ? true : !has(self.azureAdOptions)", "azureAdOptions must only be set when strategy is 'waad'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'bitbucket' ? true : !has(self.bitbucketOptions)", "bitbucketOptions must only be set when strategy is 'bitbucket'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'box' ? true : !has(self.boxOptions)", "boxOptions must only be set when strategy is 'box'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'dropbox' ? true : !has(self.dropboxOptions)", "dropboxOptions must only be set when strategy is 'dropbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'email' ? true : !has(self.emailOptions)", "emailOptions must only be set when strategy is 'email'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'evernote' ? true : !has(self.evernoteOptions)", "evernoteOptions must only be set when strategy is 'evernote'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'evernote-sandbox' ? true : !has(self.evernoteSandboxOptions)", "evernoteSandboxOptions must only be set when strategy is 'evernote-sandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'exact' ? true : !has(self.exactOptions)", "exactOptions must only be set when strategy is 'exact'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'facebook' ? true : !has(self.facebookOptions)", "facebookOptions must only be set when strategy is 'facebook'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'github' ? true : !has(self.gitHubOptions)", "gitHubOptions must only be set when strategy is 'github'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'google-apps' ? true : !has(self.googleAppsOptions)", "googleAppsOptions must only be set when strategy is 'google-apps'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'google-oauth2'? true : !has(self.googleOAuth2Options)", "googleOAuth2Options must only be set when strategy is 'google-oauth2'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'linkedin' ? true : !has(self.linkedinOptions)", "linkedinOptions must only be set when strategy is 'linkedin'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oauth1' ? true : !has(self.oAuth1Options)", "oAuth1Options must only be set when strategy is 'oauth1'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oauth2' ? true : !has(self.oAuth2Options)", "oAuth2Options must only be set when strategy is 'oauth2'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'office365' ? true : !has(self.office365Options)", "office365Options must only be set when strategy is 'office365'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oidc' ? true : !has(self.oidcOptions)", "oidcOptions must only be set when strategy is 'oidc'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'okta' ? true : !has(self.oktaOptions)", "oktaOptions must only be set when strategy is 'okta'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'paypal' ? true : !has(self.paypalOptions)", "paypalOptions must only be set when strategy is 'paypal'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'paypal-sandbox' ? true : !has(self.paypalSandboxOptions)", "paypalSandboxOptions must only be set when strategy is 'paypal-sandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'pingfederate' ? true : !has(self.pingFederateOptions)", "pingFederateOptions must only be set when strategy is 'pingfederate'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce' ? true : !has(self.salesforceOptions)", "salesforceOptions must only be set when strategy is 'salesforce'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce-community' ? true : !has(self.salesforceCommunityOptions)", "salesforceCommunityOptions must only be set when strategy is 'salesforce-community'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce-sandbox' ? true : !has(self.salesforceSandboxOptions)", "salesforceSandboxOptions must only be set when strategy is 'salesforce-sandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'samlp' ? true : !has(self.samlOptions)", "samlOptions must only be set when strategy is 'samlp'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'sms' ? true : !has(self.smsOptions)", "smsOptions must only be set when strategy is 'sms'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'twitter' ? true : !has(self.twitterOptions)", "twitterOptions must only be set when strategy is 'twitter'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'windowslive' ? true : !has(self.windowsLiveOptions)", "windowsLiveOptions must only be set when strategy is 'windowslive'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'yahoo' ? true : !has(self.yahooOptions)", "yahooOptions must only be set when strategy is 'yahoo'")]
            public V2alpha1ConnectionConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha1ConnectionConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha1ConnectionConf? LastConf { get; set; }

        }

    }

}
