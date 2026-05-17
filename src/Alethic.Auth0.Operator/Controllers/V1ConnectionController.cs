using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Models;
using Alethic.Auth0.Operator.Options;

using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Connections;
using Auth0.ManagementApi.Core;

using k8s.Models;

using KubeOps.Abstractions.Rbac;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.KubernetesClient;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

namespace Alethic.Auth0.Operator.Controllers
{

    [EntityRbac(typeof(V1Connection), Verbs = RbacVerb.All)]
    [EntityRbac(typeof(V2alpha1Tenant), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(V1Secret), Verbs = RbacVerb.List | RbacVerb.Get)]
    [EntityRbac(typeof(Eventsv1Event), Verbs = RbacVerb.All)]
    public class V1ConnectionController :
        V1TenantEntityInstanceController<V1Connection, V1Connection.SpecDef, V1Connection.StatusDef, V1ConnectionConf, V1ConnectionConf>,
        IEntityController<V1Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kube"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public V1ConnectionController(IKubernetesClient kube, IMemoryCache cache, IOptions<OperatorOptions> options, ILogger<V1ConnectionController> logger) :
            base(kube, cache, options, logger)
        {

        }

        /// <inheritdoc />
        protected override string EntityTypeName => "Connection";

        /// <summary>
        /// Converts a <see cref="GetConnectionResponseContent"/> API response to a <see cref="V1ConnectionConf"/>.
        /// Note: <see cref="V1ConnectionConf.EnabledClients"/> is populated separately and left null here.
        /// </summary>
        internal static V1ConnectionConf? FromApi(GetConnectionResponseContent? source)
        {
            if (source is null)
                return null;

            return source.Strategy switch
            {
                "auth0" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentAuth0>(System.Text.Json.JsonSerializer.Serialize(source))),
                "ad" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentAd>(System.Text.Json.JsonSerializer.Serialize(source))),
                "adfs" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentAdfs>(System.Text.Json.JsonSerializer.Serialize(source))),
                "amazon" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentAmazon>(System.Text.Json.JsonSerializer.Serialize(source))),
                "apple" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentApple>(System.Text.Json.JsonSerializer.Serialize(source))),
                "auth0-oidc" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentAuth0Oidc>(System.Text.Json.JsonSerializer.Serialize(source))),
                "baidu" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentBaidu>(System.Text.Json.JsonSerializer.Serialize(source))),
                "bitbucket" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentBitbucket>(System.Text.Json.JsonSerializer.Serialize(source))),
                "bitly" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentBitly>(System.Text.Json.JsonSerializer.Serialize(source))),
                "box" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentBox>(System.Text.Json.JsonSerializer.Serialize(source))),
                "daccount" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentDaccount>(System.Text.Json.JsonSerializer.Serialize(source))),
                "dropbox" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentDropbox>(System.Text.Json.JsonSerializer.Serialize(source))),
                "dwolla" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentDwolla>(System.Text.Json.JsonSerializer.Serialize(source))),
                "email" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentEmail>(System.Text.Json.JsonSerializer.Serialize(source))),
                "evernote" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentEvernote>(System.Text.Json.JsonSerializer.Serialize(source))),
                "evernote-sandbox" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentEvernoteSandbox>(System.Text.Json.JsonSerializer.Serialize(source))),
                "exact" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentExact>(System.Text.Json.JsonSerializer.Serialize(source))),
                "facebook" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentFacebook>(System.Text.Json.JsonSerializer.Serialize(source))),
                "fitbit" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentFitbit>(System.Text.Json.JsonSerializer.Serialize(source))),
                "github" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentGitHub>(System.Text.Json.JsonSerializer.Serialize(source))),
                "google-apps" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentGoogleApps>(System.Text.Json.JsonSerializer.Serialize(source))),
                "google-oauth2" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentGoogleOAuth2>(System.Text.Json.JsonSerializer.Serialize(source))),
                "instagram" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentInstagram>(System.Text.Json.JsonSerializer.Serialize(source))),
                "line" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentLine>(System.Text.Json.JsonSerializer.Serialize(source))),
                "linkedin" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentLinkedin>(System.Text.Json.JsonSerializer.Serialize(source))),
                "oauth1" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentOAuth1>(System.Text.Json.JsonSerializer.Serialize(source))),
                "oauth2" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentOAuth2>(System.Text.Json.JsonSerializer.Serialize(source))),
                "office365" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentOffice365>(System.Text.Json.JsonSerializer.Serialize(source))),
                "oidc" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentOidc>(System.Text.Json.JsonSerializer.Serialize(source))),
                "okta" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentOkta>(System.Text.Json.JsonSerializer.Serialize(source))),
                "paypal" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentPaypal>(System.Text.Json.JsonSerializer.Serialize(source))),
                "paypal-sandbox" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentPaypalSandbox>(System.Text.Json.JsonSerializer.Serialize(source))),
                "pingfederate" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentPingFederate>(System.Text.Json.JsonSerializer.Serialize(source))),
                "planningcenter" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentPlanningCenter>(System.Text.Json.JsonSerializer.Serialize(source))),
                "salesforce" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSalesforce>(System.Text.Json.JsonSerializer.Serialize(source))),
                "salesforce-community" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSalesforceCommunity>(System.Text.Json.JsonSerializer.Serialize(source))),
                "salesforce-sandbox" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSalesforceSandbox>(System.Text.Json.JsonSerializer.Serialize(source))),
                "samlp" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSaml>(System.Text.Json.JsonSerializer.Serialize(source))),
                "sharepoint" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSharepoint>(System.Text.Json.JsonSerializer.Serialize(source))),
                "shopify" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentShopify>(System.Text.Json.JsonSerializer.Serialize(source))),
                "shop" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentShop>(System.Text.Json.JsonSerializer.Serialize(source))),
                "sms" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSms>(System.Text.Json.JsonSerializer.Serialize(source))),
                "soundcloud" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentSoundcloud>(System.Text.Json.JsonSerializer.Serialize(source))),
                "thirtysevensignals" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentThirtySevenSignals>(System.Text.Json.JsonSerializer.Serialize(source))),
                "twitter" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentTwitter>(System.Text.Json.JsonSerializer.Serialize(source))),
                "untappd" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentUntappd>(System.Text.Json.JsonSerializer.Serialize(source))),
                "vkontakte" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentVkontakte>(System.Text.Json.JsonSerializer.Serialize(source))),
                "waad" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentAzureAd>(System.Text.Json.JsonSerializer.Serialize(source))),
                "weibo" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentWeibo>(System.Text.Json.JsonSerializer.Serialize(source))),
                "windowslive" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentWindowsLive>(System.Text.Json.JsonSerializer.Serialize(source))),
                "wordpress" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentWordpress>(System.Text.Json.JsonSerializer.Serialize(source))),
                "yahoo" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentYahoo>(System.Text.Json.JsonSerializer.Serialize(source))),
                "yandex" => FromApi(System.Text.Json.JsonSerializer.Deserialize<ConnectionResponseContentYandex>(System.Text.Json.JsonSerializer.Serialize(source))),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Builds a <see cref="V1ConnectionConf"/> from a strategy-specific response-content type that carries
        /// a typed <see cref="ConnectionOptionsAuth0"/> on its <c>Options</c> property.
        /// </summary>
        internal static V1ConnectionConf? FromApi(ConnectionResponseContentAuth0 source)
        {
            if (source is null)
                return null;

            return new V1ConnectionConf()
            {
                Name = source.Name,
                DisplayName = source.DisplayName,
                Strategy = "auth0",
                Realms = source.Realms?.ToArray(),
                IsDomainConnection = source.IsDomainConnection,
                Options = source.Options is { } opts ? FromApi(opts) : null,
                Metadata = TransformToSystemTextJson<System.Collections.Hashtable>(source.Metadata),
            };
        }

        internal static V1ConnectionOptions FromApi(ConnectionOptionsAuth0 source)
        {
            return new V1ConnectionOptions()
            {
                Validation = source.Validation.IsDefined && source.Validation.Value is { } v ? FromApi(v) : null,
                NonPersistentAttributes = source.NonPersistentAttrs?.ToArray(),
                Precedence = source.Precedence is { } prec ? prec.Select(FromApi).ToArray() : null,
                Attributes = source.Attributes is { } attr ? FromApi(attr) : null,
                EnableScriptContext = source.EnableScriptContext,
                EnableDatabaseCustomization = source.EnabledDatabaseCustomization,
                ImportMode = source.ImportMode,
                CustomScripts = source.CustomScripts is { } cs ? FromApi(cs) : null,
                AuthenticationMethods = source.AuthenticationMethods.IsDefined && source.AuthenticationMethods.Value is { } am ? FromApi(am) : null,
                PasskeyOptions = source.PasskeyOptions.IsDefined && source.PasskeyOptions.Value is { } po ? FromApi(po) : null,
                PasswordPolicy = source.PasswordPolicy.IsDefined && source.PasswordPolicy.Value is { } pp ? FromApi(pp) : null,
                PasswordComplexityOptions = source.PasswordComplexityOptions.IsDefined && source.PasswordComplexityOptions.Value is { } pco ? FromApi(pco) : null,
                PasswordHistory = source.PasswordHistory.IsDefined && source.PasswordHistory.Value is { } ph ? FromApi(ph) : null,
                PasswordNoPersonalInfo = source.PasswordNoPersonalInfo.IsDefined && source.PasswordNoPersonalInfo.Value is { } pnpi ? FromApi(pnpi) : null,
                PasswordDictionary = source.PasswordDictionary.IsDefined && source.PasswordDictionary.Value is { } pd ? FromApi(pd) : null,
                DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword,
            };
        }

        internal static V1ConnectionOptionsValidation FromApi(ConnectionValidationOptions source)
        {
            return new V1ConnectionOptionsValidation
            {
                UserName = source.Username.IsDefined && source.Username.Value is { } u ? FromApi(u) : null,
            };
        }

        internal static V1ConnectionOptionsUserName FromApi(ConnectionUsernameValidationOptions source)
        {
            return new V1ConnectionOptionsUserName
            {
                Min = source.Min,
                Max = source.Max,
            };
        }

        internal static V1ConnectionOptionsAttributes FromApi(ConnectionAttributes source)
        {
            return new V1ConnectionOptionsAttributes
            {
                Email = source.Email is { } e ? FromApi(e) : null,
                PhoneNumber = source.PhoneNumber is { } p ? FromApi(p) : null,
                Username = source.Username is { } u ? FromApi(u) : null,
            };
        }

        internal static V1ConnectionOptionsEmailAttribute FromApi(EmailAttribute source)
        {
            return new V1ConnectionOptionsEmailAttribute
            {
                Identifier = source.Identifier is { } i ? FromApi(i) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } s ? FromApi(s) : null,
            };
        }

        internal static V1ConnectionOptionsEmailSignup FromApi(SignupVerified source)
        {
            return new V1ConnectionOptionsEmailSignup
            {
                Status = source.Status is { } st ? FromApi(st) : null,
                Verification = source.Verification is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsPhoneNumberAttribute FromApi(PhoneAttribute source)
        {
            return new V1ConnectionOptionsPhoneNumberAttribute
            {
                Signup = source.Signup is { } s ? FromApi(s, true) : null,
            };
        }

        internal static V1ConnectionOptionsPhoneNumberSignup FromApi(SignupVerified source, bool phone)
        {
            return new V1ConnectionOptionsPhoneNumberSignup
            {
                Status = source.Status is { } st ? FromApi(st) : null,
                Verification = source.Verification is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsUsernameAttribute FromApi(UsernameAttribute source)
        {
            return new V1ConnectionOptionsUsernameAttribute
            {
                Identifier = source.Identifier is { } i ? FromApi(i) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } s ? FromApi(s) : null,
                Validation = source.Validation is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsUsernameSignup FromApi(SignupSchema source)
        {
            return new V1ConnectionOptionsUsernameSignup
            {
                Status = source.Status is { } st ? FromApi(st) : null,
            };
        }

        internal static V1ConnectionOptionsAttributeIdentifier FromApi(ConnectionAttributeIdentifier source)
        {
            return new V1ConnectionOptionsAttributeIdentifier
            {
                Active = source.Active,
            };
        }

        internal static V1ConnectionOptionsAttributeValidation FromApi(UsernameValidation source)
        {
            return new V1ConnectionOptionsAttributeValidation
            {
                MinLength = (int?)source.MinLength,
                MaxLength = (int?)source.MaxLength,
                AllowedTypes = source.AllowedTypes is { } at ? FromApi(at) : null,
            };
        }

        internal static V1ConnectionOptionsAttributeAllowedTypes FromApi(UsernameAllowedTypes source)
        {
            return new V1ConnectionOptionsAttributeAllowedTypes
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
            };
        }

        internal static V1ConnectionOptionsVerification FromApi(SignupVerification source)
        {
            return new V1ConnectionOptionsVerification
            {
                Active = source.Active,
            };
        }

        internal static V1ConnectionOptionsCustomScripts FromApi(ConnectionCustomScripts source)
        {
            return new V1ConnectionOptionsCustomScripts
            {
                Login = source.Login,
                GetUser = source.GetUser,
                Delete = source.Delete,
                ChangePassword = source.ChangePassword,
                Verify = source.Verify,
                Create = source.Create,
                ChangeUsername = source.ChangeUsername,
                ChangeEmail = source.ChangeEmail,
                ChangePhoneNumber = source.ChangePhoneNumber,
            };
        }

        internal static V1ConnectionOptionsAuthenticationMethods FromApi(ConnectionAuthenticationMethods source)
        {
            return new V1ConnectionOptionsAuthenticationMethods
            {
                Password = source.Password is { } p ? FromApi(p) : null,
                Passkey = source.Passkey is { } pk ? FromApi(pk) : null,
            };
        }

        internal static V1ConnectionOptionsPasswordAuthenticationMethod FromApi(ConnectionPasswordAuthenticationMethod source)
        {
            return new V1ConnectionOptionsPasswordAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V1ConnectionOptionsPasskeyAuthenticationMethod FromApi(ConnectionPasskeyAuthenticationMethod source)
        {
            return new V1ConnectionOptionsPasskeyAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V1ConnectionOptionsPasskeyOptions FromApi(ConnectionPasskeyOptions source)
        {
            return new V1ConnectionOptionsPasskeyOptions
            {
                ChallengeUi = source.ChallengeUi is { } cui ? FromApi(cui) : null,
                ProgressiveEnrollmentEnabled = source.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = source.LocalEnrollmentEnabled,
            };
        }

        internal static V1ConnectionOptionsPasswordComplexityOptions FromApi(ConnectionPasswordComplexityOptions source)
        {
            return new V1ConnectionOptionsPasswordComplexityOptions
            {
                MinLength = source.MinLength,
            };
        }

        internal static V1ConnectionOptionsPasswordHistory FromApi(ConnectionPasswordHistoryOptions source)
        {
            return new V1ConnectionOptionsPasswordHistory
            {
                Enable = source.Enable,
                Size = source.Size,
            };
        }

        internal static V1ConnectionOptionsPasswordNoPersonalInfo FromApi(ConnectionPasswordNoPersonalInfoOptions source)
        {
            return new V1ConnectionOptionsPasswordNoPersonalInfo
            {
                Enable = source.Enable,
            };
        }

        internal static V1ConnectionOptionsPasswordDictionary FromApi(ConnectionPasswordDictionaryOptions source)
        {
            return new V1ConnectionOptionsPasswordDictionary
            {
                Enable = source.Enable,
                Dictionary = source.Dictionary?.ToArray(),
            };
        }

        internal static V1ConnectionGatewayAuthentication FromApi(ConnectionGatewayAuthentication source)
        {
            return new V1ConnectionGatewayAuthentication
            {
                Method = source.Method,
                Subject = source.Subject,
                Audience = source.Audience,
                Secret = source.Secret,
                SecretBase64Encoded = source.SecretBase64Encoded,
            };
        }

        internal static V1ConnectionOptionsPrecedence FromApi(ConnectionIdentifierPrecedenceEnum source) => source.Value switch
        {
            ConnectionIdentifierPrecedenceEnum.Values.Email => V1ConnectionOptionsPrecedence.Email,
            ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber => V1ConnectionOptionsPrecedence.PhoneNumber,
            ConnectionIdentifierPrecedenceEnum.Values.Username => V1ConnectionOptionsPrecedence.UserName,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ConnectionIdentifierPrecedenceEnum ToApi(V1ConnectionOptionsPrecedence source) => source switch
        {
            V1ConnectionOptionsPrecedence.Email => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Email),
            V1ConnectionOptionsPrecedence.PhoneNumber => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.PhoneNumber),
            V1ConnectionOptionsPrecedence.UserName => new ConnectionIdentifierPrecedenceEnum(ConnectionIdentifierPrecedenceEnum.Values.Username),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionOptionsAttributeStatus? FromApi(SignupStatusEnum? source) => source?.Value switch
        {
            SignupStatusEnum.Values.Required => V1ConnectionOptionsAttributeStatus.Required,
            SignupStatusEnum.Values.Optional => V1ConnectionOptionsAttributeStatus.Optional,
            SignupStatusEnum.Values.Inactive => V1ConnectionOptionsAttributeStatus.Inactive,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static SignupStatusEnum ToApi(V1ConnectionOptionsAttributeStatus source) => source switch
        {
            V1ConnectionOptionsAttributeStatus.Required => new SignupStatusEnum(SignupStatusEnum.Values.Required),
            V1ConnectionOptionsAttributeStatus.Optional => new SignupStatusEnum(SignupStatusEnum.Values.Optional),
            V1ConnectionOptionsAttributeStatus.Inactive => new SignupStatusEnum(SignupStatusEnum.Values.Inactive),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionOptionsPasswordPolicy? FromApi(ConnectionPasswordPolicyEnum? source) => source?.Value switch
        {
            ConnectionPasswordPolicyEnum.Values.None => V1ConnectionOptionsPasswordPolicy.None,
            ConnectionPasswordPolicyEnum.Values.Low => V1ConnectionOptionsPasswordPolicy.Low,
            ConnectionPasswordPolicyEnum.Values.Fair => V1ConnectionOptionsPasswordPolicy.Fair,
            ConnectionPasswordPolicyEnum.Values.Good => V1ConnectionOptionsPasswordPolicy.Good,
            ConnectionPasswordPolicyEnum.Values.Excellent => V1ConnectionOptionsPasswordPolicy.Excellent,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ConnectionPasswordPolicyEnum ToApi(V1ConnectionOptionsPasswordPolicy source) => source switch
        {
            V1ConnectionOptionsPasswordPolicy.None => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.None),
            V1ConnectionOptionsPasswordPolicy.Low => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Low),
            V1ConnectionOptionsPasswordPolicy.Fair => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Fair),
            V1ConnectionOptionsPasswordPolicy.Good => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Good),
            V1ConnectionOptionsPasswordPolicy.Excellent => new ConnectionPasswordPolicyEnum(ConnectionPasswordPolicyEnum.Values.Excellent),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionChallengeUi FromApi(ConnectionPasskeyChallengeUiEnum source) => source.Value switch
        {
            ConnectionPasskeyChallengeUiEnum.Values.Both => V1ConnectionChallengeUi.Both,
            ConnectionPasskeyChallengeUiEnum.Values.Autofill => V1ConnectionChallengeUi.AutoFill,
            ConnectionPasskeyChallengeUiEnum.Values.Button => V1ConnectionChallengeUi.Button,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ConnectionPasskeyChallengeUiEnum ToApi(V1ConnectionChallengeUi source) => source switch
        {
            V1ConnectionChallengeUi.Both => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Both),
            V1ConnectionChallengeUi.AutoFill => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Autofill),
            V1ConnectionChallengeUi.Button => new ConnectionPasskeyChallengeUiEnum(ConnectionPasskeyChallengeUiEnum.Values.Button),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        /// <summary>
        /// Gets the list of enabled client IDs for the specified connection.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="connectionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<string[]> GetEnabledClientsAsync(IManagementApiClient api, string connectionId, CancellationToken cancellationToken)
        {
            var pager = await api.Connections.Clients.GetAsync(connectionId, new GetConnectionEnabledClientsRequestParameters(), null, cancellationToken);

            var l = new List<string>();
            foreach (var client in pager.CurrentPage.Items)
                if (client.ClientId is not null)
                    l.Add(client.ClientId);

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<V1ConnectionConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                var self = await api.Connections.GetAsync(id, new GetConnectionRequestParameters(), null, cancellationToken);
                if (self == null)
                    return null;

                var conf = FromApi(self)!;
                conf.EnabledClients = (await GetEnabledClientsAsync(api, self.Id, cancellationToken))
                    .Select(i => new V1ClientReference() { Id = i })
                    .ToArray();
                return conf;
            }
            catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<string?> Find(IManagementApiClient api, V1Connection entity, V1Connection.SpecDef spec, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (spec.Find is not null)
            {
                if (spec.Find.ConnectionId is string connectionId)
                {
                    try
                    {
                        var connection = await api.Connections.GetAsync(connectionId, new GetConnectionRequestParameters(), null, cancellationToken);
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing connection: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), connection.Name);
                        return connection.Id;
                    }
                    catch (ErrorApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} could not find connection with id {ConnectionId}.", EntityTypeName, entity.Namespace(), entity.Name(), connectionId);
                        return null;
                    }
                }

                return null;
            }
            else
            {
                var conf = spec.Init ?? spec.Conf;
                if (conf is null || string.IsNullOrEmpty(conf.Name))
                    return null;

                var pager = await api.Connections.ListAsync(new ListConnectionsQueryParameters { Name = Optional<string?>.Of(conf.Name) }, null, cancellationToken);
                var self = pager.CurrentPage.Items?.FirstOrDefault(i => i.Name == conf.Name);
                if (self is not null)
                    Logger.LogInformation("{EntityTypeName} {EntityNamespace}/{EntityName} found existing connection by name: {Name}", EntityTypeName, entity.Namespace(), entity.Name(), conf.Name);

                return self?.Id;
            }
        }

        /// <inheritdoc />
        protected override string? ValidateCreate(V1ConnectionConf conf)
        {
            return null;
        }

        /// <summary>
        /// Attempts to resolve the list of client references to client IDs.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="refs"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        async Task<string[]> ResolveClientRefsToIds(IManagementApiClient api, V1ClientReference[]? refs, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (refs is null)
                return Array.Empty<string>();

            var l = new List<string>(refs.Length);

            foreach (var i in refs)
                l.Add(await ResolveClientRefToId(api, i, defaultNamespace, cancellationToken) ?? throw new InvalidOperationException());

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<string> Create(IManagementApiClient api, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} creating connection in Auth0 with name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, conf.Name, conf.Strategy);

            if (conf.Strategy is null)
                throw new InvalidOperationException("Missing connection strategy.");

            if (conf.Options is null)
                throw new InvalidOperationException("Missing connection options.");

            var req = new CreateConnectionRequestContent()
            {
                Name = conf.Name ?? throw new InvalidOperationException("Missing connection name."),
                Strategy = ConnectionIdentityProviderEnum.FromCustom(conf.Strategy),
            };
            ApplyToApi(conf, req);

            // calculate options: depends on current strategy, but we need to potentially patch the existing resource
            if (conf.Options is not null)
            {
                if (conf.Strategy == "auth0")
                {
                    var options = new ConnectionPropertiesOptions();
                    req.Options = options;
                    ApplyToApi(conf.Options, options);
                }
                else
                {
                    var options = (dynamic)new JObject();
                    req.Options = options;
                    ApplyToApi(conf.Options, ref options);
                }
            }

            var self = await api.Connections.CreateAsync(req, null, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            Logger.LogInformation("{EntityTypeName} successfully created connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, self.Id, conf.Name, conf.Strategy);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ConnectionConf? last, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);

            var req = new UpdateConnectionRequestContent();
            ApplyToApi(conf, req);

            // calculate options: depends on current strategy, but we need to potentially patch the existing resource
            if (conf.Options is not null)
            {
                var current = await api.Connections.GetAsync(id, new GetConnectionRequestParameters(), null, cancellationToken);
                if (current.Strategy == "auth0")
                {
                    var options = new UpdateConnectionOptions();
                    req.Options = Optional<UpdateConnectionOptions?>.Of(options);
                    ApplyToApi(conf.Options, options);
                }
                else
                {
                    var options = (dynamic)(current.Options is not null ? JObject.FromObject(current.Options) : new JObject());
                    req.Options = Optional<UpdateConnectionOptions?>.Of(options);
                    ApplyToApi(conf.Options, ref options);
                }
            }

            await api.Connections.UpdateAsync(id, req, null, cancellationToken);
            await UpdateEnabledClientsAsync(api, id, conf, defaultNamespace, cancellationToken);

            Logger.LogInformation("{EntityTypeName} successfully updated connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
        }

        /// <summary>
        /// Applies the specified configuration to the request object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        internal static void ApplyToApi(V1ConnectionConf source, CreateConnectionRequestContent target)
        {
            if (source.Name is { } name)
                target.Name = name;

            if (source.DisplayName is { } displayName)
                target.DisplayName = displayName;

            if (source.Metadata is { } metadata)
                target.Metadata = metadata.Cast<System.Collections.DictionaryEntry>().ToDictionary(e => (string)e.Key, e => e.Value?.ToString());

            if (source.Realms is { } realms)
                target.Realms = realms;

            if (source.IsDomainConnection is not null)
                target.IsDomainConnection = source.IsDomainConnection ?? false;

            if (source.ShowAsButton is { } showAsButton)
                target.ShowAsButton = showAsButton;
        }

        internal static void ApplyToApi(V1ConnectionConf source, UpdateConnectionRequestContent target)
        {
            if (source.DisplayName is { } displayName)
                target.DisplayName = displayName;

            if (source.Metadata is { } metadata)
                target.Metadata = metadata.Cast<System.Collections.DictionaryEntry>().ToDictionary(e => (string)e.Key, e => e.Value?.ToString());

            if (source.Realms is { } realms)
                target.Realms = realms;

            if (source.IsDomainConnection is not null)
                target.IsDomainConnection = source.IsDomainConnection ?? false;

            if (source.ShowAsButton is { } showAsButton)
                target.ShowAsButton = showAsButton;
        }

        void ApplyToApi(V1ConnectionOptions source, ConnectionOptionsAuth0 target)
        {
            if (source.Validation is { } validation)
            {
                var v = new ConnectionValidationOptions();
                ApplyToApi(validation, v);
                target.Validation = Optional<ConnectionValidationOptions?>.Of(v);
            }

            if (source.NonPersistentAttributes is { } nonPersistentAttributes)
                target.NonPersistentAttrs = nonPersistentAttributes;

            if (source.Precedence is { } precedence)
                target.Precedence = precedence.Select(ToApi).ToArray();

            if (source.Attributes is { } attributes)
            {
                target.Attributes ??= new ConnectionAttributes();
                ApplyToApi(attributes, target.Attributes);
            }

            if (source.EnableScriptContext is { } enableScriptContext)
                target.EnableScriptContext = enableScriptContext;

            if (source.EnableDatabaseCustomization is { } enableDatabaseCustomization)
                target.EnabledDatabaseCustomization = enableDatabaseCustomization;

            if (source.ImportMode is { } importMode)
                target.ImportMode = importMode;

            if (source.CustomScripts is { } customScripts)
            {
                target.CustomScripts ??= new ConnectionCustomScripts();
                ApplyToApi(customScripts, target.CustomScripts);
            }

            if (source.AuthenticationMethods is { } authenticationMethods)
            {
                var am = new ConnectionAuthenticationMethods();
                ApplyToApi(authenticationMethods, am);
                target.AuthenticationMethods = Optional<ConnectionAuthenticationMethods?>.Of(am);
            }

            if (source.PasskeyOptions is { } passkeyOptions)
            {
                var po = new ConnectionPasskeyOptions();
                ApplyToApi(passkeyOptions, po);
                target.PasskeyOptions = Optional<ConnectionPasskeyOptions?>.Of(po);
            }

            if (source.PasswordPolicy is { } passwordPolicy)
                target.PasswordPolicy = Optional<ConnectionPasswordPolicyEnum?>.Of(ToApi(passwordPolicy));

            if (source.PasswordComplexityOptions is { } passwordComplexityOptions)
            {
                var pco = new ConnectionPasswordComplexityOptions();
                ApplyToApi(passwordComplexityOptions, pco);
                target.PasswordComplexityOptions = Optional<ConnectionPasswordComplexityOptions?>.Of(pco);
            }

            if (source.PasswordHistory is { } passwordHistory)
            {
                var ph = new ConnectionPasswordHistoryOptions() { Enable = false };
                ApplyToApi(passwordHistory, ph);
                target.PasswordHistory = Optional<ConnectionPasswordHistoryOptions?>.Of(ph);
            }

            if (source.PasswordNoPersonalInfo is { } passwordNoPersonalInfo)
            {
                var pnpi = new ConnectionPasswordNoPersonalInfoOptions() { Enable = false };
                ApplyToApi(passwordNoPersonalInfo, pnpi);
                target.PasswordNoPersonalInfo = Optional<ConnectionPasswordNoPersonalInfoOptions?>.Of(pnpi);
            }

            if (source.PasswordDictionary is { } passwordDictionary)
            {
                var pd = new ConnectionPasswordDictionaryOptions() { Enable = false };
                ApplyToApi(passwordDictionary, pd);
                target.PasswordDictionary = Optional<ConnectionPasswordDictionaryOptions?>.Of(pd);
            }

            if (source.DisableSelfServiceChangePassword is { } disableSelfServiceChangePassword)
                target.DisableSelfServiceChangePassword = disableSelfServiceChangePassword;
        }

        void ApplyToApi(V1ConnectionOptions source, ConnectionPropertiesOptions target)
        {
            if (source.Validation is { } validation)
            {
                var v = new ConnectionValidationOptions();
                ApplyToApi(validation, v);
                target.Validation = Optional<ConnectionValidationOptions?>.Of(v);
            }

            if (source.NonPersistentAttributes is { } nonPersistentAttributes)
                target.NonPersistentAttrs = nonPersistentAttributes;

            if (source.Precedence is { } precedence)
                target.Precedence = precedence.Select(ToApi).ToArray();

            if (source.Attributes is { } attributes)
            {
                target.Attributes ??= new ConnectionAttributes();
                ApplyToApi(attributes, target.Attributes);
            }

            if (source.EnableScriptContext is { } enableScriptContext)
                target.EnableScriptContext = enableScriptContext;

            if (source.EnableDatabaseCustomization is { } enableDatabaseCustomization)
                target.EnabledDatabaseCustomization = enableDatabaseCustomization;

            if (source.ImportMode is { } importMode)
                target.ImportMode = importMode;

            if (source.CustomScripts is { } customScripts)
            {
                target.CustomScripts ??= new ConnectionCustomScripts();
                ApplyToApi(customScripts, target.CustomScripts);
            }

            if (source.AuthenticationMethods is { } authenticationMethods)
            {
                var am = new ConnectionAuthenticationMethods();
                ApplyToApi(authenticationMethods, am);
                target.AuthenticationMethods = Optional<ConnectionAuthenticationMethods?>.Of(am);
            }

            if (source.PasskeyOptions is { } passkeyOptions)
            {
                var po = new ConnectionPasskeyOptions();
                ApplyToApi(passkeyOptions, po);
                target.PasskeyOptions = Optional<ConnectionPasskeyOptions?>.Of(po);
            }

            if (source.PasswordPolicy is { } passwordPolicy)
                target.PasswordPolicy = Optional<ConnectionPasswordPolicyEnum?>.Of(ToApi(passwordPolicy));

            if (source.PasswordComplexityOptions is { } passwordComplexityOptions)
            {
                var pco = new ConnectionPasswordComplexityOptions();
                ApplyToApi(passwordComplexityOptions, pco);
                target.PasswordComplexityOptions = Optional<ConnectionPasswordComplexityOptions?>.Of(pco);
            }

            if (source.PasswordHistory is { } passwordHistory)
            {
                var ph = new ConnectionPasswordHistoryOptions() { Enable = false };
                ApplyToApi(passwordHistory, ph);
                target.PasswordHistory = Optional<ConnectionPasswordHistoryOptions?>.Of(ph);
            }

            if (source.PasswordNoPersonalInfo is { } passwordNoPersonalInfo)
            {
                var pnpi = new ConnectionPasswordNoPersonalInfoOptions() { Enable = false };
                ApplyToApi(passwordNoPersonalInfo, pnpi);
                target.PasswordNoPersonalInfo = Optional<ConnectionPasswordNoPersonalInfoOptions?>.Of(pnpi);
            }

            if (source.PasswordDictionary is { } passwordDictionary)
            {
                var pd = new ConnectionPasswordDictionaryOptions() { Enable = false };
                ApplyToApi(passwordDictionary, pd);
                target.PasswordDictionary = Optional<ConnectionPasswordDictionaryOptions?>.Of(pd);
            }

            if (source.DisableSelfServiceChangePassword is { } disableSelfServiceChangePassword)
                target.DisableSelfServiceChangePassword = disableSelfServiceChangePassword;
        }

        void ApplyToApi(V1ConnectionOptions source, UpdateConnectionOptions target)
        {
            if (source.Validation is { } validation)
            {
                var v = new ConnectionValidationOptions();
                ApplyToApi(validation, v);
                target.Validation = Optional<ConnectionValidationOptions?>.Of(v);
            }

            if (source.NonPersistentAttributes is { } nonPersistentAttributes)
                target.NonPersistentAttrs = nonPersistentAttributes;

            if (source.Precedence is { } precedence)
                target.Precedence = precedence.Select(ToApi).ToArray();

            if (source.Attributes is { } attributes)
            {
                target.Attributes ??= new ConnectionAttributes();
                ApplyToApi(attributes, target.Attributes);
            }

            if (source.EnableScriptContext is { } enableScriptContext)
                target.EnableScriptContext = enableScriptContext;

            if (source.EnableDatabaseCustomization is { } enableDatabaseCustomization)
                target.EnabledDatabaseCustomization = enableDatabaseCustomization;

            if (source.ImportMode is { } importMode)
                target.ImportMode = importMode;

            if (source.CustomScripts is { } customScripts)
            {
                target.CustomScripts ??= new ConnectionCustomScripts();
                ApplyToApi(customScripts, target.CustomScripts);
            }

            if (source.AuthenticationMethods is { } authenticationMethods)
            {
                var am = new ConnectionAuthenticationMethods();
                ApplyToApi(authenticationMethods, am);
                target.AuthenticationMethods = Optional<ConnectionAuthenticationMethods?>.Of(am);
            }

            if (source.PasskeyOptions is { } passkeyOptions)
            {
                var po = new ConnectionPasskeyOptions();
                ApplyToApi(passkeyOptions, po);
                target.PasskeyOptions = Optional<ConnectionPasskeyOptions?>.Of(po);
            }

            if (source.PasswordPolicy is { } passwordPolicy)
                target.PasswordPolicy = Optional<ConnectionPasswordPolicyEnum?>.Of(ToApi(passwordPolicy));

            if (source.PasswordComplexityOptions is { } passwordComplexityOptions)
            {
                var pco = new ConnectionPasswordComplexityOptions();
                ApplyToApi(passwordComplexityOptions, pco);
                target.PasswordComplexityOptions = Optional<ConnectionPasswordComplexityOptions?>.Of(pco);
            }

            if (source.PasswordHistory is { } passwordHistory)
            {
                var ph = new ConnectionPasswordHistoryOptions() { Enable = false };
                ApplyToApi(passwordHistory, ph);
                target.PasswordHistory = Optional<ConnectionPasswordHistoryOptions?>.Of(ph);
            }

            if (source.PasswordNoPersonalInfo is { } passwordNoPersonalInfo)
            {
                var pnpi = new ConnectionPasswordNoPersonalInfoOptions() { Enable = false };
                ApplyToApi(passwordNoPersonalInfo, pnpi);
                target.PasswordNoPersonalInfo = Optional<ConnectionPasswordNoPersonalInfoOptions?>.Of(pnpi);
            }

            if (source.PasswordDictionary is { } passwordDictionary)
            {
                var pd = new ConnectionPasswordDictionaryOptions() { Enable = false };
                ApplyToApi(passwordDictionary, pd);
                target.PasswordDictionary = Optional<ConnectionPasswordDictionaryOptions?>.Of(pd);
            }

            if (source.DisableSelfServiceChangePassword is { } disableSelfServiceChangePassword)
                target.DisableSelfServiceChangePassword = disableSelfServiceChangePassword;
        }

        static void ApplyToApi(V1ConnectionOptionsValidation source, ConnectionValidationOptions target)
        {
            if (source.UserName is { } userName)
            {
                var u = new ConnectionUsernameValidationOptions() { Min = 0, Max = 0 };
                ApplyToApi(userName, u);
                target.Username = Optional<ConnectionUsernameValidationOptions?>.Of(u);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUserName source, ConnectionUsernameValidationOptions target)
        {
            if (source.Min is { } min)
                target.Min = min;

            if (source.Max is { } max)
                target.Max = max;
        }

        static void ApplyToApi(V1ConnectionOptionsAttributes source, ConnectionAttributes target)
        {
            if (source.Email is { } email)
            {
                target.Email ??= new EmailAttribute();
                ApplyToApi(email, target.Email);
            }

            if (source.PhoneNumber is { } phoneNumber)
            {
                target.PhoneNumber ??= new PhoneAttribute();
                ApplyToApi(phoneNumber, target.PhoneNumber);
            }

            if (source.Username is { } username)
            {
                target.Username ??= new UsernameAttribute();
                ApplyToApi(username, target.Username);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsEmailAttribute source, EmailAttribute target)
        {
            if (source.Identifier is { } identifier)
            {
                target.Identifier ??= new ConnectionAttributeIdentifier();
                ApplyToApi(identifier, target.Identifier);
            }

            if (source.ProfileRequired is { } profileRequired)
                target.ProfileRequired = profileRequired;

            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupVerified();
                ApplyToApi(signup, target.Signup);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsEmailSignup source, SignupVerified target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new SignupVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPhoneNumberAttribute source, PhoneAttribute target)
        {
            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupVerified();
                ApplyToApi(signup, target.Signup, true);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPhoneNumberSignup source, SignupVerified target, bool phone)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new SignupVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUsernameAttribute source, UsernameAttribute target)
        {
            if (source.Identifier is { } identifier)
            {
                target.Identifier ??= new ConnectionAttributeIdentifier();
                ApplyToApi(identifier, target.Identifier);
            }

            if (source.ProfileRequired is { } profileRequired)
                target.ProfileRequired = profileRequired;

            if (source.Signup is { } signup)
            {
                target.Signup ??= new SignupSchema();
                ApplyToApi(signup, target.Signup);
            }

            if (source.Validation is { } validation)
            {
                target.Validation ??= new UsernameValidation();
                ApplyToApi(validation, target.Validation);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUsernameSignup source, SignupSchema target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeIdentifier source, ConnectionAttributeIdentifier target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeValidation source, UsernameValidation target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;

            if (source.MaxLength is { } maxLength)
                target.MaxLength = maxLength;

            if (source.AllowedTypes is { } allowedTypes)
            {
                target.AllowedTypes ??= new UsernameAllowedTypes();
                ApplyToApi(allowedTypes, target.AllowedTypes);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeAllowedTypes source, UsernameAllowedTypes target)
        {
            if (source.Email is { } email)
                target.Email = email;

            if (source.PhoneNumber is { } phoneNumber)
                target.PhoneNumber = phoneNumber;
        }

        static void ApplyToApi(V1ConnectionOptionsVerification source, SignupVerification target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V1ConnectionOptionsCustomScripts source, ConnectionCustomScripts target)
        {
            if (source.Login is { } login)
                target.Login = login;

            if (source.GetUser is { } getUser)
                target.GetUser = getUser;

            if (source.Delete is { } delete)
                target.Delete = delete;

            if (source.ChangePassword is { } changePassword)
                target.ChangePassword = changePassword;

            if (source.Verify is { } verify)
                target.Verify = verify;

            if (source.Create is { } create)
                target.Create = create;

            if (source.ChangeUsername is { } changeUsername)
                target.ChangeUsername = changeUsername;

            if (source.ChangeEmail is { } changeEmail)
                target.ChangeEmail = changeEmail;

            if (source.ChangePhoneNumber is { } changePhoneNumber)
                target.ChangePhoneNumber = changePhoneNumber;
        }

        static void ApplyToApi(V1ConnectionOptionsAuthenticationMethods source, ConnectionAuthenticationMethods target)
        {
            if (source.Password is { } password)
            {
                target.Password ??= new ConnectionPasswordAuthenticationMethod();
                ApplyToApi(password, target.Password);
            }

            if (source.Passkey is { } passkey)
            {
                target.Passkey ??= new ConnectionPasskeyAuthenticationMethod();
                ApplyToApi(passkey, target.Passkey);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordAuthenticationMethod source, ConnectionPasswordAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasskeyAuthenticationMethod source, ConnectionPasskeyAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasskeyOptions source, ConnectionPasskeyOptions target)
        {
            if (source.ChallengeUi is { } challengeUi)
                target.ChallengeUi = ToApi(challengeUi);

            if (source.ProgressiveEnrollmentEnabled is { } progressiveEnrollmentEnabled)
                target.ProgressiveEnrollmentEnabled = progressiveEnrollmentEnabled;

            if (source.LocalEnrollmentEnabled is { } localEnrollmentEnabled)
                target.LocalEnrollmentEnabled = localEnrollmentEnabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordComplexityOptions source, ConnectionPasswordComplexityOptions target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordHistory source, ConnectionPasswordHistoryOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Size is { } size)
                target.Size = size;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordNoPersonalInfo source, ConnectionPasswordNoPersonalInfoOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordDictionary source, ConnectionPasswordDictionaryOptions target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Dictionary is { } dictionary)
                target.Dictionary = dictionary;
        }

        static void ApplyToApi(V1ConnectionGatewayAuthentication source, ConnectionGatewayAuthentication target)
        {
            if (source.Method is { } method)
                target.Method = method;

            if (source.Subject is { } subject)
                target.Subject = subject;

            if (source.Audience is { } audience)
                target.Audience = audience;

            if (source.Secret is { } secret)
                target.Secret = secret;

            if (source.SecretBase64Encoded is { } secretBase64Encoded)
                target.SecretBase64Encoded = secretBase64Encoded;
        }

        static void ApplyToApi(V1ConnectionOptions source, ref dynamic target)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(source);
            var patch = JObject.Parse(json);

            if (target is not JObject targetObj)
                targetObj = new JObject();

            MergeJObject(patch, targetObj);
            target = targetObj;
        }

        static void MergeJObject(JObject source, JObject target)
        {
            foreach (var property in source.Properties())
            {
                if (target[property.Name] is JObject existingObj && property.Value is JObject sourceObj)
                    MergeJObject(sourceObj, existingObj);
                else
                    target[property.Name] = property.Value;
            }
        }

        /// <summary>
        /// Applies the update of enabled clients.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="id"></param>
        /// <param name="conf"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task UpdateEnabledClientsAsync(IManagementApiClient api, string id, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            if (conf.EnabledClients is not null)
            {
                var req = new List<UpdateEnabledClientConnectionsRequestContentItem>();

                // apply existing clients, disabled by default
                var existingResponse = await api.Connections.Clients.GetAsync(id, new GetConnectionEnabledClientsRequestParameters(), null, cancellationToken);
                if (existingResponse?.CurrentPage?.Items is { } existingItems)
                    foreach (var current in existingItems)
                        if (current.ClientId is not null)
                            req.Add(new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = current.ClientId, Status = false });

                // add or enable clients specified in the configuration
                foreach (var clientId in await ResolveClientRefsToIds(api, conf.EnabledClients, defaultNamespace, cancellationToken))
                {
                    var existing = req.FirstOrDefault(i => i.ClientId == clientId);
                    if (existing is null)
                        req.Add(existing = new UpdateEnabledClientConnectionsRequestContentItem() { ClientId = clientId, Status = false });

                    existing.Status = true;
                }

                // apply update
                if (req.Count > 0)
                    await api.Connections.Clients.UpdateAsync(id, req, null, cancellationToken);
            }
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting connection from Auth0 with ID: {ConnectionId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Connections.DeleteAsync(id, null, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted connection from Auth0 with ID: {ConnectionId}", EntityTypeName, id);
        }

    }

}
