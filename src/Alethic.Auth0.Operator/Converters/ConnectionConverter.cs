using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text.Json;

using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    /// <summary>
    /// Provides conversions targeting <see cref="V2alpha1Connection"/>.
    /// </summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha1Connection))]
    public class ConnectionConverter : ConversionWebhook<V2alpha1Connection>
    {

        protected override IEnumerable<IEntityConverter<V2alpha1Connection>> Converters => [
            new V1ToV2alpha1()
        ];

        /// <summary>
        /// Converts between <see cref="V1Connection"/> and <see cref="V2alpha1Connection"/>.
        /// </summary>
        class V1ToV2alpha1 : IEntityConverter<V1Connection, V2alpha1Connection>
        {

            public V2alpha1Connection Convert(V1Connection from)
            {
                var result = new V2alpha1Connection { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Find = from.Spec.Find is { } find ? new V2alpha1ConnectionFind { ConnectionId = find.ConnectionId } : null;
                result.Spec.Init = ConvertConf(from.Spec.Init);
                result.Spec.Conf = ConvertConf(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = ConvertConf(from.Status.LastConf);
                return result;
            }

            public V1Connection Revert(V2alpha1Connection source)
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

            static V2alpha1ConnectionConf? ConvertConf(V1ConnectionConf? source)
            {
                if (source is null)
                    return null;

                var strategy = source.Strategy is { } s ? JsonSerializer.Deserialize<V2alpha1ConnectionStrategy?>(JsonSerializer.Serialize(s)) : null;

                return new V2alpha1ConnectionConf
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
                    Options = ConvertOptions(strategy, source.Options),
                };
            }

            static V1ConnectionConf? RevertConf(V2alpha1ConnectionConf? source)
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
                    Options = RevertOptions(source.Strategy, source.Options),

                };
            }

            static V2alpha1ConnectionOptions? ConvertOptions(V2alpha1ConnectionStrategy? strategy, V1ConnectionOptions? source)
            {
                if (source is null)
                    return null;

                var json = JsonSerializer.SerializeToElement(source);
                var options = new V2alpha1ConnectionOptions();

                switch (strategy)
                {
                    case V2alpha1ConnectionStrategy.Auth0:
                        options.Auth0 = json.Deserialize<V2alpha1ConnectionOptionsAuth0>();
                        break;
                    case V2alpha1ConnectionStrategy.Ad:
                        options.Ad = json.Deserialize<V2alpha1ConnectionOptionsAd>();
                        break;
                    case V2alpha1ConnectionStrategy.Adfs:
                        options.Adfs = json.Deserialize<V2alpha1ConnectionOptionsAdfs>();
                        break;
                    case V2alpha1ConnectionStrategy.Auth0Oidc:
                        options.Auth0Oidc = json.Deserialize<V2alpha1ConnectionOptionsAuth0Oidc>();
                        break;
                    case V2alpha1ConnectionStrategy.AzureAd:
                        options.AzureAd = json.Deserialize<V2alpha1ConnectionOptionsAzureAd>();
                        break;
                    case V2alpha1ConnectionStrategy.Bitbucket:
                        options.Bitbucket = json.Deserialize<V2alpha1ConnectionOptionsBitbucket>();
                        break;
                    case V2alpha1ConnectionStrategy.Box:
                        options.Box = json.Deserialize<V2alpha1ConnectionOptionsBox>();
                        break;
                    case V2alpha1ConnectionStrategy.Dropbox:
                        options.Dropbox = json.Deserialize<V2alpha1ConnectionOptionsDropbox>();
                        break;
                    case V2alpha1ConnectionStrategy.Email:
                        options.Email = json.Deserialize<V2alpha1ConnectionOptionsEmail>();
                        break;
                    case V2alpha1ConnectionStrategy.Evernote:
                        options.Evernote = json.Deserialize<V2alpha1ConnectionOptionsEvernote>();
                        break;
                    case V2alpha1ConnectionStrategy.EvernoteSandbox:
                        options.EvernoteSandbox = json.Deserialize<V2alpha1ConnectionOptionsEvernote>();
                        break;
                    case V2alpha1ConnectionStrategy.Exact:
                        options.Exact = json.Deserialize<V2alpha1ConnectionOptionsExact>();
                        break;
                    case V2alpha1ConnectionStrategy.Facebook:
                        options.Facebook = json.Deserialize<V2alpha1ConnectionOptionsFacebook>();
                        break;
                    case V2alpha1ConnectionStrategy.GitHub:
                        options.GitHub = json.Deserialize<V2alpha1ConnectionOptionsGitHub>();
                        break;
                    case V2alpha1ConnectionStrategy.GoogleApps:
                        options.GoogleApps = json.Deserialize<V2alpha1ConnectionOptionsGoogleApps>();
                        break;
                    case V2alpha1ConnectionStrategy.GoogleOAuth2:
                        options.GoogleOAuth2 = json.Deserialize<V2alpha1ConnectionOptionsGoogleOAuth2>();
                        break;
                    case V2alpha1ConnectionStrategy.Linkedin:
                        options.Linkedin = json.Deserialize<V2alpha1ConnectionOptionsLinkedin>();
                        break;
                    case V2alpha1ConnectionStrategy.OAuth1:
                        options.OAuth1 = json.Deserialize<V2alpha1ConnectionOptionsOAuth1>();
                        break;
                    case V2alpha1ConnectionStrategy.OAuth2:
                        options.OAuth2 = json.Deserialize<V2alpha1ConnectionOptionsOAuth2>();
                        break;
                    case V2alpha1ConnectionStrategy.Office365:
                        options.Office365 = json.Deserialize<V2alpha1ConnectionOptionsOffice365>();
                        break;
                    case V2alpha1ConnectionStrategy.Oidc:
                        options.Oidc = json.Deserialize<V2alpha1ConnectionOptionsOidc>();
                        break;
                    case V2alpha1ConnectionStrategy.Okta:
                        options.Okta = json.Deserialize<V2alpha1ConnectionOptionsOkta>();
                        break;
                    case V2alpha1ConnectionStrategy.Paypal:
                        options.Paypal = json.Deserialize<V2alpha1ConnectionOptionsPaypal>();
                        break;
                    case V2alpha1ConnectionStrategy.PaypalSandbox:
                        options.PaypalSandbox = json.Deserialize<V2alpha1ConnectionOptionsPaypal>();
                        break;
                    case V2alpha1ConnectionStrategy.PingFederate:
                        options.PingFederate = json.Deserialize<V2alpha1ConnectionOptionsPingFederate>();
                        break;
                    case V2alpha1ConnectionStrategy.Salesforce:
                        options.Salesforce = json.Deserialize<V2alpha1ConnectionOptionsSalesforce>();
                        break;
                    case V2alpha1ConnectionStrategy.SalesforceCommunity:
                        options.SalesforceCommunity = json.Deserialize<V2alpha1ConnectionOptionsSalesforceCommunity>();
                        break;
                    case V2alpha1ConnectionStrategy.SalesforceSandbox:
                        options.SalesforceSandbox = json.Deserialize<V2alpha1ConnectionOptionsSalesforce>();
                        break;
                    case V2alpha1ConnectionStrategy.Saml:
                        options.Saml = json.Deserialize<V2alpha1ConnectionOptionsSaml>();
                        break;
                    case V2alpha1ConnectionStrategy.Sms:
                        options.Sms = json.Deserialize<V2alpha1ConnectionOptionsSms>();
                        break;
                    case V2alpha1ConnectionStrategy.Twitter:
                        options.Twitter = json.Deserialize<V2alpha1ConnectionOptionsTwitter>();
                        break;
                    case V2alpha1ConnectionStrategy.WindowsLive:
                        options.WindowsLive = json.Deserialize<V2alpha1ConnectionOptionsWindowsLive>();
                        break;
                    case V2alpha1ConnectionStrategy.Yahoo:
                        options.Yahoo = json.Deserialize<V2alpha1ConnectionOptionsYahoo>();
                        break;
                }

                return options;
            }

            static V1ConnectionOptions? RevertOptions(V2alpha1ConnectionStrategy? strategy, V2alpha1ConnectionOptions? source)
            {
                if (source is null)
                    return null;

                JsonElement? json = strategy switch
                {
                    V2alpha1ConnectionStrategy.Auth0 => source.Auth0 is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Ad => source.Ad is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Adfs => source.Adfs is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Auth0Oidc => source.Auth0Oidc is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.AzureAd => source.AzureAd is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Bitbucket => source.Bitbucket is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Box => source.Box is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Dropbox => source.Dropbox is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Email => source.Email is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Evernote => source.Evernote is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.EvernoteSandbox => source.EvernoteSandbox is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Exact => source.Exact is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Facebook => source.Facebook is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.GitHub => source.GitHub is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.GoogleApps => source.GoogleApps is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.GoogleOAuth2 => source.GoogleOAuth2 is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Linkedin => source.Linkedin is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.OAuth1 => source.OAuth1 is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.OAuth2 => source.OAuth2 is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Office365 => source.Office365 is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Oidc => source.Oidc is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Okta => source.Okta is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Paypal => source.Paypal is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.PaypalSandbox => source.PaypalSandbox is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.PingFederate => source.PingFederate is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Salesforce => source.Salesforce is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.SalesforceCommunity => source.SalesforceCommunity is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.SalesforceSandbox => source.SalesforceSandbox is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Saml => source.Saml is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Sms => source.Sms is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Twitter => source.Twitter is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.WindowsLive => source.WindowsLive is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    V2alpha1ConnectionStrategy.Yahoo => source.Yahoo is { } v ? JsonSerializer.SerializeToElement(v) : null,
                    _ => null,
                };

                return json is { } j ? j.Deserialize<V1ConnectionOptions>() : null;
            }

        }

    }

}
