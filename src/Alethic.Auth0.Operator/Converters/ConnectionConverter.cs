using System.Collections.Generic;
using System.Runtime.Versioning;

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

                return new V2alpha1ConnectionConf
                {
                    Name = source.Name,
                    DisplayName = source.DisplayName,
                    Strategy = source.Strategy,
                    ProvisioningTicketUrl = source.ProvisioningTicketUrl,
                    Metadata = source.Metadata,
                    Realms = source.Realms,
                    EnabledClients = source.EnabledClients,
                    ShowAsButton = source.ShowAsButton,
                    IsDomainConnection = source.IsDomainConnection,
                    Options = ConvertOptions(source.Strategy, source.Options),
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
                    Strategy = source.Strategy,
                    ProvisioningTicketUrl = source.ProvisioningTicketUrl,
                    Metadata = source.Metadata,
                    Realms = source.Realms,
                    EnabledClients = source.EnabledClients,
                    ShowAsButton = source.ShowAsButton,
                    IsDomainConnection = source.IsDomainConnection,
                    Options = RevertOptions(source.Strategy, source.Options),
                };
            }

            static V2alpha1ConnectionOptions? ConvertOptions(string? strategy, V1ConnectionOptions? source)
            {
                if (source is null)
                    return null;

                var options = new V2alpha1ConnectionOptions();

                switch (strategy)
                {
                    case "auth0":
                        options.Auth0 = new V2alpha1ConnectionAuth0Options
                        {
                            PasswordPolicy = source.PasswordPolicy?.ToString(),
                            PasswordHistory = source.PasswordHistory is { } ph ? new V2alpha1ConnectionOptionsPasswordHistory { Enable = ph.Enable, Size = ph.Size } : null,
                            PasswordNoPersonalInfo = source.PasswordNoPersonalInfo is { } pnpi ? new V2alpha1ConnectionOptionsPasswordNoPersonalInfo { Enable = pnpi.Enable } : null,
                            PasswordDictionary = source.PasswordDictionary is { } pd ? new V2alpha1ConnectionOptionsPasswordDictionary { Enable = pd.Enable, Dictionary = pd.Dictionary } : null,
                            PasswordComplexityOptions = source.PasswordComplexityOptions is { } pco ? new V2alpha1ConnectionOptionsPasswordComplexityOptions { MinLength = pco.MinLength } : null,
                            Validation = source.Validation is { } v ? new V2alpha1ConnectionOptionsValidation { UserName = v.UserName is { } un ? new V2alpha1ConnectionOptionsUserName { Min = un.Min, Max = un.Max } : null } : null,
                            EnableScriptContext = source.EnableScriptContext,
                            EnabledDatabaseCustomization = source.EnableDatabaseCustomization,
                            CustomScripts = source.CustomScripts is { } cs ? new V2alpha1ConnectionOptionsCustomScripts
                            {
                                Login = cs.Login,
                                GetUser = cs.GetUser,
                                Delete = cs.Delete,
                                ChangePassword = cs.ChangePassword,
                                Verify = cs.Verify,
                                Create = cs.Create,
                                ChangeUsername = cs.ChangeUsername,
                                ChangeEmail = cs.ChangeEmail,
                            } : null,
                            ImportMode = source.ImportMode,
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? ConvertSetUserRootAttributes(sura) : null,
                        };
                        break;
                    case "ad":
                        options.Ad = new V2alpha1ConnectionAdOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura2 ? ConvertSetUserRootAttributes(sura2) : null,
                        };
                        break;
                    case "adfs":
                        options.Adfs = new V2alpha1ConnectionAdfsOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura3 ? ConvertSetUserRootAttributes(sura3) : null,
                        };
                        break;
                    case "auth0-oidc":
                        options.Auth0Oidc = new V2alpha1ConnectionAuth0OidcOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura4 ? ConvertSetUserRootAttributes(sura4) : null,
                        };
                        break;
                    case "waad":
                        options.AzureAd = new V2alpha1ConnectionAzureAdOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura5 ? ConvertSetUserRootAttributes(sura5) : null,
                        };
                        break;
                    case "bitbucket":
                        options.Bitbucket = new V2alpha1ConnectionBitbucketOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura6 ? ConvertSetUserRootAttributes(sura6) : null,
                        };
                        break;
                    case "box":
                        options.Box = new V2alpha1ConnectionBoxOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura7 ? ConvertSetUserRootAttributes(sura7) : null,
                        };
                        break;
                    case "dropbox":
                        options.Dropbox = new V2alpha1ConnectionDropboxOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura8 ? ConvertSetUserRootAttributes(sura8) : null,
                        };
                        break;
                    case "email":
                        options.Email = new V2alpha1ConnectionEmailOptions();
                        break;
                    case "evernote":
                        options.Evernote = new V2alpha1ConnectionEvernoteOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura9 ? ConvertSetUserRootAttributes(sura9) : null,
                        };
                        break;
                    case "evernote-sandbox":
                        options.EvernoteSandbox = new V2alpha1ConnectionEvernoteOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura10 ? ConvertSetUserRootAttributes(sura10) : null,
                        };
                        break;
                    case "exact":
                        options.Exact = new V2alpha1ConnectionExactOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura11 ? ConvertSetUserRootAttributes(sura11) : null,
                        };
                        break;
                    case "facebook":
                        options.Facebook = new V2alpha1ConnectionFacebookOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura12 ? ConvertSetUserRootAttributes(sura12) : null,
                        };
                        break;
                    case "github":
                        options.GitHub = new V2alpha1ConnectionGitHubOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura13 ? ConvertSetUserRootAttributes(sura13) : null,
                        };
                        break;
                    case "google-apps":
                        options.GoogleApps = new V2alpha1ConnectionGoogleAppsOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura14 ? ConvertSetUserRootAttributes(sura14) : null,
                        };
                        break;
                    case "google-oauth2":
                        options.GoogleOAuth2 = new V2alpha1ConnectionGoogleOAuth2Options
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura15 ? ConvertSetUserRootAttributes(sura15) : null,
                        };
                        break;
                    case "linkedin":
                        options.Linkedin = new V2alpha1ConnectionLinkedinOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura16 ? ConvertSetUserRootAttributes(sura16) : null,
                        };
                        break;
                    case "oauth1":
                        options.OAuth1 = new V2alpha1ConnectionOAuth1Options
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                        };
                        break;
                    case "oauth2":
                        options.OAuth2 = new V2alpha1ConnectionOAuth2Options
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura17 ? ConvertSetUserRootAttributes(sura17) : null,
                        };
                        break;
                    case "office365":
                        options.Office365 = new V2alpha1ConnectionOffice365Options
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura18 ? ConvertSetUserRootAttributes(sura18) : null,
                        };
                        break;
                    case "oidc":
                        options.Oidc = new V2alpha1ConnectionOidcOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura19 ? ConvertSetUserRootAttributes(sura19) : null,
                        };
                        break;
                    case "okta":
                        options.Okta = new V2alpha1ConnectionOktaOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura20 ? ConvertSetUserRootAttributes(sura20) : null,
                        };
                        break;
                    case "paypal":
                        options.Paypal = new V2alpha1ConnectionPaypalOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura21 ? ConvertSetUserRootAttributes(sura21) : null,
                        };
                        break;
                    case "paypal-sandbox":
                        options.PaypalSandbox = new V2alpha1ConnectionPaypalOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura22 ? ConvertSetUserRootAttributes(sura22) : null,
                        };
                        break;
                    case "pingfederate":
                        options.PingFederate = new V2alpha1ConnectionPingFederateOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura23 ? ConvertSetUserRootAttributes(sura23) : null,
                        };
                        break;
                    case "salesforce":
                        options.Salesforce = new V2alpha1ConnectionSalesforceOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura24 ? ConvertSetUserRootAttributes(sura24) : null,
                        };
                        break;
                    case "salesforce-community":
                        options.SalesforceCommunity = new V2alpha1ConnectionSalesforceCommunityOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura25 ? ConvertSetUserRootAttributes(sura25) : null,
                        };
                        break;
                    case "salesforce-sandbox":
                        options.SalesforceSandbox = new V2alpha1ConnectionSalesforceOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura26 ? ConvertSetUserRootAttributes(sura26) : null,
                        };
                        break;
                    case "samlp":
                        options.Saml = new V2alpha1ConnectionSamlOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura27 ? ConvertSetUserRootAttributes(sura27) : null,
                        };
                        break;
                    case "sms":
                        options.Sms = new V2alpha1ConnectionSmsOptions();
                        break;
                    case "twitter":
                        options.Twitter = new V2alpha1ConnectionTwitterOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura28 ? ConvertSetUserRootAttributes(sura28) : null,
                        };
                        break;
                    case "windowslive":
                        options.WindowsLive = new V2alpha1ConnectionWindowsLiveOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura29 ? ConvertSetUserRootAttributes(sura29) : null,
                        };
                        break;
                    case "yahoo":
                        options.Yahoo = new V2alpha1ConnectionYahooOptions
                        {
                            NonPersistentAttrs = source.NonPersistentAttributes,
                            SetUserRootAttributes = source.SetUserRootAttributes is { } sura30 ? ConvertSetUserRootAttributes(sura30) : null,
                        };
                        break;
                }

                options.AdditionalProperties = source.AdditionalProperties;
                return options;
            }

            static V1ConnectionOptions? RevertOptions(string? strategy, V2alpha1ConnectionOptions? source)
            {
                if (source is null)
                    return null;

                var options = new V1ConnectionOptions();

                switch (strategy)
                {
                    case "auth0":
                        if (source.Auth0 is { } auth0)
                        {
                            options.PasswordPolicy = auth0.PasswordPolicy is { } pp && System.Enum.TryParse<V1ConnectionOptionsPasswordPolicy>(pp, ignoreCase: true, out var ppVal) ? ppVal : null;
                            options.PasswordHistory = auth0.PasswordHistory is { } ph ? new V1ConnectionOptionsPasswordHistory { Enable = ph.Enable, Size = ph.Size } : null;
                            options.PasswordNoPersonalInfo = auth0.PasswordNoPersonalInfo is { } pnpi ? new V1ConnectionOptionsPasswordNoPersonalInfo { Enable = pnpi.Enable } : null;
                            options.PasswordDictionary = auth0.PasswordDictionary is { } pd ? new V1ConnectionOptionsPasswordDictionary { Enable = pd.Enable, Dictionary = pd.Dictionary } : null;
                            options.PasswordComplexityOptions = auth0.PasswordComplexityOptions is { } pco ? new V1ConnectionOptionsPasswordComplexityOptions { MinLength = pco.MinLength } : null;
                            options.Validation = auth0.Validation is { } v ? new V1ConnectionOptionsValidation { UserName = v.UserName is { } un ? new V1ConnectionOptionsUserName { Min = un.Min, Max = un.Max } : null } : null;
                            options.EnableScriptContext = auth0.EnableScriptContext;
                            options.EnableDatabaseCustomization = auth0.EnabledDatabaseCustomization;
                            options.CustomScripts = auth0.CustomScripts is { } cs ? new V1ConnectionOptionsCustomScripts
                            {
                                Login = cs.Login,
                                GetUser = cs.GetUser,
                                Delete = cs.Delete,
                                ChangePassword = cs.ChangePassword,
                                Verify = cs.Verify,
                                Create = cs.Create,
                                ChangeUsername = cs.ChangeUsername,
                                ChangeEmail = cs.ChangeEmail,
                            } : null;
                            options.ImportMode = auth0.ImportMode;
                            options.NonPersistentAttributes = auth0.NonPersistentAttrs;
                            options.SetUserRootAttributes = auth0.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "ad":
                        if (source.Ad is { } ad)
                        {
                            options.NonPersistentAttributes = ad.NonPersistentAttrs;
                            options.SetUserRootAttributes = ad.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "adfs":
                        if (source.Adfs is { } adfs)
                        {
                            options.NonPersistentAttributes = adfs.NonPersistentAttrs;
                            options.SetUserRootAttributes = adfs.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "auth0-oidc":
                        if (source.Auth0Oidc is { } auth0Oidc)
                        {
                            options.NonPersistentAttributes = auth0Oidc.NonPersistentAttrs;
                            options.SetUserRootAttributes = auth0Oidc.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "waad":
                        if (source.AzureAd is { } azureAd)
                        {
                            options.NonPersistentAttributes = azureAd.NonPersistentAttrs;
                            options.SetUserRootAttributes = azureAd.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "bitbucket":
                        if (source.Bitbucket is { } bitbucket)
                        {
                            options.NonPersistentAttributes = bitbucket.NonPersistentAttrs;
                            options.SetUserRootAttributes = bitbucket.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "box":
                        if (source.Box is { } box)
                        {
                            options.NonPersistentAttributes = box.NonPersistentAttrs;
                            options.SetUserRootAttributes = box.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "dropbox":
                        if (source.Dropbox is { } dropbox)
                        {
                            options.NonPersistentAttributes = dropbox.NonPersistentAttrs;
                            options.SetUserRootAttributes = dropbox.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "evernote":
                        if (source.Evernote is { } evernote)
                        {
                            options.NonPersistentAttributes = evernote.NonPersistentAttrs;
                            options.SetUserRootAttributes = evernote.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "evernote-sandbox":
                        if (source.EvernoteSandbox is { } evernoteSandbox)
                        {
                            options.NonPersistentAttributes = evernoteSandbox.NonPersistentAttrs;
                            options.SetUserRootAttributes = evernoteSandbox.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "exact":
                        if (source.Exact is { } exact)
                        {
                            options.NonPersistentAttributes = exact.NonPersistentAttrs;
                            options.SetUserRootAttributes = exact.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "facebook":
                        if (source.Facebook is { } facebook)
                        {
                            options.NonPersistentAttributes = facebook.NonPersistentAttrs;
                            options.SetUserRootAttributes = facebook.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "github":
                        if (source.GitHub is { } gitHub)
                        {
                            options.NonPersistentAttributes = gitHub.NonPersistentAttrs;
                            options.SetUserRootAttributes = gitHub.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "google-apps":
                        if (source.GoogleApps is { } googleApps)
                        {
                            options.NonPersistentAttributes = googleApps.NonPersistentAttrs;
                            options.SetUserRootAttributes = googleApps.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "google-oauth2":
                        if (source.GoogleOAuth2 is { } googleOAuth2)
                        {
                            options.NonPersistentAttributes = googleOAuth2.NonPersistentAttrs;
                            options.SetUserRootAttributes = googleOAuth2.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "linkedin":
                        if (source.Linkedin is { } linkedin)
                        {
                            options.NonPersistentAttributes = linkedin.NonPersistentAttrs;
                            options.SetUserRootAttributes = linkedin.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "oauth1":
                        if (source.OAuth1 is { } oauth1)
                        {
                            options.NonPersistentAttributes = oauth1.NonPersistentAttrs;
                        }
                        break;
                    case "oauth2":
                        if (source.OAuth2 is { } oauth2)
                        {
                            options.NonPersistentAttributes = oauth2.NonPersistentAttrs;
                            options.SetUserRootAttributes = oauth2.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "office365":
                        if (source.Office365 is { } office365)
                        {
                            options.NonPersistentAttributes = office365.NonPersistentAttrs;
                            options.SetUserRootAttributes = office365.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "oidc":
                        if (source.Oidc is { } oidc)
                        {
                            options.NonPersistentAttributes = oidc.NonPersistentAttrs;
                            options.SetUserRootAttributes = oidc.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "okta":
                        if (source.Okta is { } okta)
                        {
                            options.NonPersistentAttributes = okta.NonPersistentAttrs;
                            options.SetUserRootAttributes = okta.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "paypal":
                        if (source.Paypal is { } paypal)
                        {
                            options.NonPersistentAttributes = paypal.NonPersistentAttrs;
                            options.SetUserRootAttributes = paypal.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "paypal-sandbox":
                        if (source.PaypalSandbox is { } paypalSandbox)
                        {
                            options.NonPersistentAttributes = paypalSandbox.NonPersistentAttrs;
                            options.SetUserRootAttributes = paypalSandbox.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "pingfederate":
                        if (source.PingFederate is { } pingFederate)
                        {
                            options.NonPersistentAttributes = pingFederate.NonPersistentAttrs;
                            options.SetUserRootAttributes = pingFederate.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "salesforce":
                        if (source.Salesforce is { } salesforce)
                        {
                            options.NonPersistentAttributes = salesforce.NonPersistentAttrs;
                            options.SetUserRootAttributes = salesforce.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "salesforce-community":
                        if (source.SalesforceCommunity is { } salesforceCommunity)
                        {
                            options.NonPersistentAttributes = salesforceCommunity.NonPersistentAttrs;
                            options.SetUserRootAttributes = salesforceCommunity.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "salesforce-sandbox":
                        if (source.SalesforceSandbox is { } salesforceSandbox)
                        {
                            options.NonPersistentAttributes = salesforceSandbox.NonPersistentAttrs;
                            options.SetUserRootAttributes = salesforceSandbox.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "samlp":
                        if (source.Saml is { } saml)
                        {
                            options.NonPersistentAttributes = saml.NonPersistentAttrs;
                            options.SetUserRootAttributes = saml.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "twitter":
                        if (source.Twitter is { } twitter)
                        {
                            options.NonPersistentAttributes = twitter.NonPersistentAttrs;
                            options.SetUserRootAttributes = twitter.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "windowslive":
                        if (source.WindowsLive is { } windowsLive)
                        {
                            options.NonPersistentAttributes = windowsLive.NonPersistentAttrs;
                            options.SetUserRootAttributes = windowsLive.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                    case "yahoo":
                        if (source.Yahoo is { } yahoo)
                        {
                            options.NonPersistentAttributes = yahoo.NonPersistentAttrs;
                            options.SetUserRootAttributes = yahoo.SetUserRootAttributes is { } sura ? RevertSetUserRootAttributes(sura) : null;
                        }
                        break;
                }

                options.AdditionalProperties = source.AdditionalProperties;
                return options;
            }

            static V2alpha1ConnectionSetUserRootAttributes ConvertSetUserRootAttributes(V1ConnectionSetUserRootAttributes source) => source switch
            {
                V1ConnectionSetUserRootAttributes.OnEachLogin => V2alpha1ConnectionSetUserRootAttributes.OnEachLogin,
                V1ConnectionSetUserRootAttributes.NeverOnLogin => V2alpha1ConnectionSetUserRootAttributes.NeverOnLogin,
                _ => V2alpha1ConnectionSetUserRootAttributes.OnFirstLogin,
            };

            static V1ConnectionSetUserRootAttributes RevertSetUserRootAttributes(V2alpha1ConnectionSetUserRootAttributes source) => source switch
            {
                V2alpha1ConnectionSetUserRootAttributes.OnEachLogin => V1ConnectionSetUserRootAttributes.OnEachLogin,
                V2alpha1ConnectionSetUserRootAttributes.NeverOnLogin => V1ConnectionSetUserRootAttributes.NeverOnLogin,
                _ => V1ConnectionSetUserRootAttributes.OnFirstLogin,
            };

        }

    }

}
