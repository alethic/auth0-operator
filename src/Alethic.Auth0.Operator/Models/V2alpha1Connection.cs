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
            [ValidationRule("!has(self.strategy) || self.strategy == 'auth0' ? true : !has(self.options) || !has(self.options.auth0)", "options.auth0 must only be set when strategy is 'auth0'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'ad' ? true : !has(self.options) || !has(self.options.ad)", "options.ad must only be set when strategy is 'ad'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'adfs' ? true : !has(self.options) || !has(self.options.adfs)", "options.adfs must only be set when strategy is 'adfs'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'auth0-oidc' ? true : !has(self.options) || !has(self.options.auth0Oidc)", "options.auth0Oidc must only be set when strategy is 'auth0-oidc'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'waad' ? true : !has(self.options) || !has(self.options.azureAd)", "options.azureAd must only be set when strategy is 'waad'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'bitbucket' ? true : !has(self.options) || !has(self.options.bitbucket)", "options.bitbucket must only be set when strategy is 'bitbucket'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'box' ? true : !has(self.options) || !has(self.options.box)", "options.box must only be set when strategy is 'box'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'dropbox' ? true : !has(self.options) || !has(self.options.dropbox)", "options.dropbox must only be set when strategy is 'dropbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'email' ? true : !has(self.options) || !has(self.options.email)", "options.email must only be set when strategy is 'email'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'evernote' ? true : !has(self.options) || !has(self.options.evernote)", "options.evernote must only be set when strategy is 'evernote'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'evernote-sandbox' ? true : !has(self.options) || !has(self.options.evernoteSandbox)", "options.evernoteSandbox must only be set when strategy is 'evernote-sandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'exact' ? true : !has(self.options) || !has(self.options.exact)", "options.exact must only be set when strategy is 'exact'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'facebook' ? true : !has(self.options) || !has(self.options.facebook)", "options.facebook must only be set when strategy is 'facebook'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'github' ? true : !has(self.options) || !has(self.options.gitHub)", "options.gitHub must only be set when strategy is 'github'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'google-apps' ? true : !has(self.options) || !has(self.options.googleApps)", "options.googleApps must only be set when strategy is 'google-apps'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'google-oauth2' ? true : !has(self.options) || !has(self.options.googleOAuth2)", "options.googleOAuth2 must only be set when strategy is 'google-oauth2'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'linkedin' ? true : !has(self.options) || !has(self.options.linkedin)", "options.linkedin must only be set when strategy is 'linkedin'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oauth1' ? true : !has(self.options) || !has(self.options.oAuth1)", "options.oAuth1 must only be set when strategy is 'oauth1'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oauth2' ? true : !has(self.options) || !has(self.options.oAuth2)", "options.oAuth2 must only be set when strategy is 'oauth2'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'office365' ? true : !has(self.options) || !has(self.options.office365)", "options.office365 must only be set when strategy is 'office365'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oidc' ? true : !has(self.options) || !has(self.options.oidc)", "options.oidc must only be set when strategy is 'oidc'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'okta' ? true : !has(self.options) || !has(self.options.okta)", "options.okta must only be set when strategy is 'okta'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'paypal' ? true : !has(self.options) || !has(self.options.paypal)", "options.paypal must only be set when strategy is 'paypal'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'paypal-sandbox' ? true : !has(self.options) || !has(self.options.paypalSandbox)", "options.paypalSandbox must only be set when strategy is 'paypal-sandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'pingfederate' ? true : !has(self.options) || !has(self.options.pingFederate)", "options.pingFederate must only be set when strategy is 'pingfederate'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce' ? true : !has(self.options) || !has(self.options.salesforce)", "options.salesforce must only be set when strategy is 'salesforce'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce-community' ? true : !has(self.options) || !has(self.options.salesforceCommunity)", "options.salesforceCommunity must only be set when strategy is 'salesforce-community'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce-sandbox' ? true : !has(self.options) || !has(self.options.salesforceSandbox)", "options.salesforceSandbox must only be set when strategy is 'salesforce-sandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'samlp' ? true : !has(self.options) || !has(self.options.saml)", "options.saml must only be set when strategy is 'samlp'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'sms' ? true : !has(self.options) || !has(self.options.sms)", "options.sms must only be set when strategy is 'sms'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'twitter' ? true : !has(self.options) || !has(self.options.twitter)", "options.twitter must only be set when strategy is 'twitter'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'windowslive' ? true : !has(self.options) || !has(self.options.windowsLive)", "options.windowsLive must only be set when strategy is 'windowslive'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'yahoo' ? true : !has(self.options) || !has(self.options.yahoo)", "options.yahoo must only be set when strategy is 'yahoo'")]
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
