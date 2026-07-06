using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

using k8s.Models;

using KubeOps.Abstractions.Entities;
using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Models
{

    [EntityScope(EntityScope.Namespaced)]
    [KubernetesEntity(Group = "kubernetes.auth0.com", ApiVersion = "v2alpha3", Kind = "Connection")]
    [KubernetesEntityShortNames("a0con")]
    public partial class V2alpha3Connection :
        CustomKubernetesEntity<V2alpha3Connection.SpecDef, V2alpha3Connection.StatusDef>,
        V1TenantEntityInstance<V2alpha3Connection.SpecDef, V2alpha3Connection.StatusDef, V2alpha3ConnectionConf, V2alpha3ConnectionConf>
    {

        public class SpecDef : V1TenantEntityInstanceSpec<V2alpha3ConnectionConf>
        {

            [JsonPropertyName("policy")]
            public V1EntityPolicyType[]? Policy { get; set; }

            [JsonPropertyName("tenantRef")]
            [Required]
            public V1TenantReference? TenantRef { get; set; }

            [JsonPropertyName("find")]
            public V2alpha3ConnectionFind? Find { get; set; }

            [JsonPropertyName("init")]
            public V2alpha3ConnectionConf? Init { get; set; }

            [JsonPropertyName("conf")]
            [Required]
            [ValidationRule("!has(self.strategy) || self.strategy == 'auth0' ? true : !has(self.options) || !has(self.options.auth0)", message:"options.auth0 must only be set when strategy is 'auth0'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'ad' ? true : !has(self.options) || !has(self.options.ad)", message: "options.ad must only be set when strategy is 'ad'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'adfs' ? true : !has(self.options) || !has(self.options.adfs)", message: "options.adfs must only be set when strategy is 'adfs'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'auth0Oidc' ? true : !has(self.options) || !has(self.options.auth0Oidc)", message: "options.auth0Oidc must only be set when strategy is 'auth0Oidc'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'waad' ? true : !has(self.options) || !has(self.options.waad)", message: "options.waad must only be set when strategy is 'waad'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'bitbucket' ? true : !has(self.options) || !has(self.options.bitbucket)", message: "options.bitbucket must only be set when strategy is 'bitbucket'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'box' ? true : !has(self.options) || !has(self.options.box)", message: "options.box must only be set when strategy is 'box'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'dropbox' ? true : !has(self.options) || !has(self.options.dropbox)", message: "options.dropbox must only be set when strategy is 'dropbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'email' ? true : !has(self.options) || !has(self.options.email)", message: "options.email must only be set when strategy is 'email'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'evernote' ? true : !has(self.options) || !has(self.options.evernote)", message: "options.evernote must only be set when strategy is 'evernote'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'evernoteSandbox' ? true : !has(self.options) || !has(self.options.evernoteSandbox)", message: "options.evernoteSandbox must only be set when strategy is 'evernoteSandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'exact' ? true : !has(self.options) || !has(self.options.exact)", message: "options.exact must only be set when strategy is 'exact'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'facebook' ? true : !has(self.options) || !has(self.options.facebook)", message: "options.facebook must only be set when strategy is 'facebook'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'github' ? true : !has(self.options) || !has(self.options.github)", message: "options.github must only be set when strategy is 'github'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'googleApps' ? true : !has(self.options) || !has(self.options.googleApps)", message: "options.googleApps must only be set when strategy is 'googleApps'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'googleOauth2' ? true : !has(self.options) || !has(self.options.googleOauth2)", message: "options.googleOauth2 must only be set when strategy is 'googleOauth2'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'linkedin' ? true : !has(self.options) || !has(self.options.linkedin)", message: "options.linkedin must only be set when strategy is 'linkedin'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oauth1' ? true : !has(self.options) || !has(self.options.oauth1)", message: "options.oauth1 must only be set when strategy is 'oauth1'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oauth2' ? true : !has(self.options) || !has(self.options.oauth2)", message: "options.oauth2 must only be set when strategy is 'oauth2'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'office365' ? true : !has(self.options) || !has(self.options.office365)", message: "options.office365 must only be set when strategy is 'office365'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'oidc' ? true : !has(self.options) || !has(self.options.oidc)", message: "options.oidc must only be set when strategy is 'oidc'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'okta' ? true : !has(self.options) || !has(self.options.okta)", message: "options.okta must only be set when strategy is 'okta'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'paypal' ? true : !has(self.options) || !has(self.options.paypal)", message: "options.paypal must only be set when strategy is 'paypal'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'paypalSandbox' ? true : !has(self.options) || !has(self.options.paypalSandbox)", message: "options.paypalSandbox must only be set when strategy is 'paypalSandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'pingfederate' ? true : !has(self.options) || !has(self.options.pingfederate)", message: "options.pingfederate must only be set when strategy is 'pingfederate'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforce' ? true : !has(self.options) || !has(self.options.salesforce)", message: "options.salesforce must only be set when strategy is 'salesforce'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforceCommunity' ? true : !has(self.options) || !has(self.options.salesforceCommunity)", message: "options.salesforceCommunity must only be set when strategy is 'salesforceCommunity'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'salesforceSandbox' ? true : !has(self.options) || !has(self.options.salesforceSandbox)", message: "options.salesforceSandbox must only be set when strategy is 'salesforceSandbox'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'samlp' ? true : !has(self.options) || !has(self.options.samlp)", message: "options.samlp must only be set when strategy is 'samlp'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'sms' ? true : !has(self.options) || !has(self.options.sms)", message: "options.sms must only be set when strategy is 'sms'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'twitter' ? true : !has(self.options) || !has(self.options.twitter)", message: "options.twitter must only be set when strategy is 'twitter'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'windowslive' ? true : !has(self.options) || !has(self.options.windowslive)", message: "options.windowslive must only be set when strategy is 'windowslive'")]
            [ValidationRule("!has(self.strategy) || self.strategy == 'yahoo' ? true : !has(self.options) || !has(self.options.yahoo)", message: "options.yahoo must only be set when strategy is 'yahoo'")]
            public V2alpha3ConnectionConf? Conf { get; set; }

        }

        public class StatusDef : V1TenantEntityInstanceStatus<V2alpha3ConnectionConf>
        {

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lastConf")]
            public V2alpha3ConnectionConf? LastConf { get; set; }

            /// <summary>
            /// The Auth0 client IDs this Connection last enabled via <c>conf.enabledClients</c>. Used to scope disabling so
            /// the Connection only disables clients it previously enabled itself, leaving clients enabled through
            /// <c>ConnectionClient</c> resources untouched.
            /// </summary>
            [JsonPropertyName("managedEnabledClientIds")]
            public string[]? ManagedEnabledClientIds { get; set; }

        }

    }

}
