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
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Models.Connections;
using Auth0.ManagementApi.Paging;

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
        /// Converts a <see cref="Connection"/> API response to a <see cref="V1ConnectionConf"/>.
        /// Note: <see cref="V1ConnectionConf.EnabledClients"/> is populated separately and left null here.
        /// </summary>
        internal static V1ConnectionConf? FromApi(Connection? source)
        {
            if (source is null)
                return null;

            return new V1ConnectionConf
            {
                Name = source.Name,
                DisplayName = source.DisplayName,
                Strategy = source.Strategy,
                Realms = source.Realms,
                IsDomainConnection = source.IsDomainConnection,
                ShowAsButton = source.ShowAsButton,
                ProvisioningTicketUrl = source.ProvisioningTicketUrl,
                Options = FromApiOptions(source.Options, source.Strategy),
                Metadata = TransformToSystemTextJson<System.Collections.Hashtable>(source.Metadata),
            };
        }

        internal static V1ConnectionOptions? FromApiOptions(object? source, string? strategy)
        {
            if (source is null)
                return null;

            if (strategy == "auth0")
            {
                var src = source as ConnectionOptions ?? TransformToNewtonsoftJson<object, ConnectionOptions>(source);
                if (src is null)
                    return null;

                return FromApi(src);
            }
            else
            {
                return TransformToSystemTextJson<V1ConnectionOptions>(source);
            }
        }

        internal static V1ConnectionOptions FromApi(ConnectionOptions source)
        {
            return new V1ConnectionOptions
            {
                Validation = source.Validation is { } v ? FromApi(v) : null,
                NonPersistentAttributes = source.NonPersistentAttributes,
                Precedence = source.Precedence is { } prec ? Array.ConvertAll(prec, FromApi) : null,
                Attributes = source.Attributes is { } attr ? FromApi(attr) : null,
                EnableScriptContext = source.EnableScriptContext,
                EnableDatabaseCustomization = source.EnableDatabaseCustomization,
                ImportMode = source.ImportMode,
                CustomScripts = source.CustomScripts is { } cs ? FromApi(cs) : null,
                AuthenticationMethods = source.AuthenticationMethods is { } am ? FromApi(am) : null,
                PasskeyOptions = source.PasskeyOptions is { } po ? FromApi(po) : null,
                PasswordPolicy = source.PasswordPolicy is { } pp ? FromApi(pp) : null,
                PasswordComplexityOptions = source.PasswordComplexityOptions is { } pco ? FromApi(pco) : null,
                PasswordHistory = source.PasswordHistory is { } ph ? FromApi(ph) : null,
                PasswordNoPersonalInfo = source.PasswordNoPersonalInfo is { } pnpi ? FromApi(pnpi) : null,
                PasswordDictionary = source.PasswordDictionary is { } pd ? FromApi(pd) : null,
                ApiEnableUsers = source.ApiEnableUsers,
                BasicProfile = source.BasicProfile,
                ExtAdmin = source.ExtAdmin,
                ExtIsSuspended = source.ExtIsSuspended,
                ExtAgreedTerms = source.ExtAgreedTerms,
                ExtGroups = source.ExtGroups,
                ExtAssignedPlans = source.ExtAssignedPlans,
                ExtProfile = source.ExtProfile,
                DisableSelfServiceChangePassword = source.DisableSelfServiceChangePassword,
                UpstreamParams = source.UpstreamParams,
                SetUserRootAttributes = source.SetUserRootAttributes is { } sura ? FromApi(sura) : null,
                GatewayAuthentication = source.GatewayAuthentication is { } ga ? FromApi(ga) : null,
            };
        }

        internal static V1ConnectionOptionsValidation FromApi(ConnectionOptionsValidation source)
        {
            return new V1ConnectionOptionsValidation
            {
                UserName = source.UserName is { } u ? FromApi(u) : null,
            };
        }

        internal static V1ConnectionOptionsUserName FromApi(ConnectionOptionsUserName source)
        {
            return new V1ConnectionOptionsUserName
            {
                Min = source.Min,
                Max = source.Max,
            };
        }

        internal static V1ConnectionOptionsAttributes FromApi(ConnectionOptionsAttributes source)
        {
            return new V1ConnectionOptionsAttributes
            {
                Email = source.Email is { } e ? FromApi(e) : null,
                PhoneNumber = source.PhoneNumber is { } p ? FromApi(p) : null,
                Username = source.Username is { } u ? FromApi(u) : null,
            };
        }

        internal static V1ConnectionOptionsEmailAttribute FromApi(ConnectionOptionsEmailAttribute source)
        {
            return new V1ConnectionOptionsEmailAttribute
            {
                Identifier = source.Identifier is { } i ? FromApi(i) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } s ? FromApi(s) : null,
            };
        }

        internal static V1ConnectionOptionsEmailSignup FromApi(ConnectionOptionsEmailSignup source)
        {
            return new V1ConnectionOptionsEmailSignup
            {
                Status = FromApi(source.Status),
                Verification = source.Verification is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsPhoneNumberAttribute FromApi(ConnectionOptionsPhoneNumberAttribute source)
        {
            return new V1ConnectionOptionsPhoneNumberAttribute
            {
                Signup = source.Signup is { } s ? FromApi(s) : null,
            };
        }

        internal static V1ConnectionOptionsPhoneNumberSignup FromApi(ConnectionOptionsPhoneNumberSignup source)
        {
            return new V1ConnectionOptionsPhoneNumberSignup
            {
                Status = FromApi(source.Status),
                Verification = source.Verification is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsUsernameAttribute FromApi(ConnectionOptionsUsernameAttribute source)
        {
            return new V1ConnectionOptionsUsernameAttribute
            {
                Identifier = source.Identifier is { } i ? FromApi(i) : null,
                ProfileRequired = source.ProfileRequired,
                Signup = source.Signup is { } s ? FromApi(s) : null,
                Validation = source.Validation is { } v ? FromApi(v) : null,
            };
        }

        internal static V1ConnectionOptionsUsernameSignup FromApi(ConnectionOptionsUsernameSignup source)
        {
            return new V1ConnectionOptionsUsernameSignup
            {
                Status = FromApi(source.Status),
            };
        }

        internal static V1ConnectionOptionsAttributeIdentifier FromApi(ConnectionOptionsAttributeIdentifier source)
        {
            return new V1ConnectionOptionsAttributeIdentifier
            {
                Active = source.Active,
            };
        }

        internal static V1ConnectionOptionsAttributeValidation FromApi(ConnectionOptionsAttributeValidation source)
        {
            return new V1ConnectionOptionsAttributeValidation
            {
                MinLength = source.MinLength,
                MaxLength = source.MaxLength,
                AllowedTypes = source.AllowedTypes is { } at ? FromApi(at) : null,
            };
        }

        internal static V1ConnectionOptionsAttributeAllowedTypes FromApi(ConnectionOptionsAttributeAllowedTypes source)
        {
            return new V1ConnectionOptionsAttributeAllowedTypes
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
            };
        }

        internal static V1ConnectionOptionsVerification FromApi(ConnectionOptionsVerification source)
        {
            return new V1ConnectionOptionsVerification
            {
                Active = source.Active,
            };
        }

        internal static V1ConnectionOptionsCustomScripts FromApi(ConnectionOptionsCustomScripts source)
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

        internal static V1ConnectionOptionsAuthenticationMethods FromApi(ConnectionOptionsAuthenticationMethods source)
        {
            return new V1ConnectionOptionsAuthenticationMethods
            {
                Password = source.Password is { } p ? FromApi(p) : null,
                Passkey = source.Passkey is { } pk ? FromApi(pk) : null,
            };
        }

        internal static V1ConnectionOptionsPasswordAuthenticationMethod FromApi(ConnectionOptionsPasswordAuthenticationMethod source)
        {
            return new V1ConnectionOptionsPasswordAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V1ConnectionOptionsPasskeyAuthenticationMethod FromApi(ConnectionOptionsPasskeyAuthenticationMethod source)
        {
            return new V1ConnectionOptionsPasskeyAuthenticationMethod
            {
                Enabled = source.Enabled,
            };
        }

        internal static V1ConnectionOptionsPasskeyOptions FromApi(ConnectionOptionsPasskeyOptions source)
        {
            return new V1ConnectionOptionsPasskeyOptions
            {
                ChallengeUi = FromApi(source.ChallengeUi),
                ProgressiveEnrollmentEnabled = source.ProgressiveEnrollmentEnabled,
                LocalEnrollmentEnabled = source.LocalEnrollmentEnabled,
            };
        }

        internal static V1ConnectionOptionsPasswordComplexityOptions FromApi(ConnectionOptionsPasswordComplexityOptions source)
        {
            return new V1ConnectionOptionsPasswordComplexityOptions
            {
                MinLength = source.MinLength,
            };
        }

        internal static V1ConnectionOptionsPasswordHistory FromApi(ConnectionOptionsPasswordHistory source)
        {
            return new V1ConnectionOptionsPasswordHistory
            {
                Enable = source.Enable,
                Size = source.Size,
            };
        }

        internal static V1ConnectionOptionsPasswordNoPersonalInfo FromApi(ConnectionOptionsPasswordNoPersonalInfo source)
        {
            return new V1ConnectionOptionsPasswordNoPersonalInfo
            {
                Enable = source.Enable,
            };
        }

        internal static V1ConnectionOptionsPasswordDictionary FromApi(ConnectionOptionsPasswordDictionary source)
        {
            return new V1ConnectionOptionsPasswordDictionary
            {
                Enable = source.Enable,
                Dictionary = source.Dictionary,
            };
        }

        internal static V1ConnectionGatewayAuthentication FromApi(GatewayAuthentication source)
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

        internal static V1ConnectionOptionsPrecedence FromApi(ConnectionOptionsPrecedence source) => source switch
        {
            ConnectionOptionsPrecedence.Email => V1ConnectionOptionsPrecedence.Email,
            ConnectionOptionsPrecedence.PhoneNumber => V1ConnectionOptionsPrecedence.PhoneNumber,
            ConnectionOptionsPrecedence.UserName => V1ConnectionOptionsPrecedence.UserName,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ConnectionOptionsPrecedence ToApi(V1ConnectionOptionsPrecedence source) => source switch
        {
            V1ConnectionOptionsPrecedence.Email => ConnectionOptionsPrecedence.Email,
            V1ConnectionOptionsPrecedence.PhoneNumber => ConnectionOptionsPrecedence.PhoneNumber,
            V1ConnectionOptionsPrecedence.UserName => ConnectionOptionsPrecedence.UserName,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionOptionsAttributeStatus FromApi(ConnectionOptionsAttributeStatus source) => source switch
        {
            ConnectionOptionsAttributeStatus.Required => V1ConnectionOptionsAttributeStatus.Required,
            ConnectionOptionsAttributeStatus.Optional => V1ConnectionOptionsAttributeStatus.Optional,
            ConnectionOptionsAttributeStatus.Inactive => V1ConnectionOptionsAttributeStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ConnectionOptionsAttributeStatus ToApi(V1ConnectionOptionsAttributeStatus source) => source switch
        {
            V1ConnectionOptionsAttributeStatus.Required => ConnectionOptionsAttributeStatus.Required,
            V1ConnectionOptionsAttributeStatus.Optional => ConnectionOptionsAttributeStatus.Optional,
            V1ConnectionOptionsAttributeStatus.Inactive => ConnectionOptionsAttributeStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionOptionsPasswordPolicy FromApi(ConnectionOptionsPasswordPolicy source) => source switch
        {
            ConnectionOptionsPasswordPolicy.None => V1ConnectionOptionsPasswordPolicy.None,
            ConnectionOptionsPasswordPolicy.Low => V1ConnectionOptionsPasswordPolicy.Low,
            ConnectionOptionsPasswordPolicy.Fair => V1ConnectionOptionsPasswordPolicy.Fair,
            ConnectionOptionsPasswordPolicy.Good => V1ConnectionOptionsPasswordPolicy.Good,
            ConnectionOptionsPasswordPolicy.Excellent => V1ConnectionOptionsPasswordPolicy.Excellent,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ConnectionOptionsPasswordPolicy ToApi(V1ConnectionOptionsPasswordPolicy source) => source switch
        {
            V1ConnectionOptionsPasswordPolicy.None => ConnectionOptionsPasswordPolicy.None,
            V1ConnectionOptionsPasswordPolicy.Low => ConnectionOptionsPasswordPolicy.Low,
            V1ConnectionOptionsPasswordPolicy.Fair => ConnectionOptionsPasswordPolicy.Fair,
            V1ConnectionOptionsPasswordPolicy.Good => ConnectionOptionsPasswordPolicy.Good,
            V1ConnectionOptionsPasswordPolicy.Excellent => ConnectionOptionsPasswordPolicy.Excellent,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionSetUserRootAttributes FromApi(SetUserRootAttributes source) => source switch
        {
            SetUserRootAttributes.OnEachLogin => V1ConnectionSetUserRootAttributes.OnEachLogin,
            SetUserRootAttributes.OnFirstLogin => V1ConnectionSetUserRootAttributes.OnFirstLogin,
            SetUserRootAttributes.NeverOnLogin => V1ConnectionSetUserRootAttributes.NeverOnLogin,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static SetUserRootAttributes ToApi(V1ConnectionSetUserRootAttributes source) => source switch
        {
            V1ConnectionSetUserRootAttributes.OnEachLogin => SetUserRootAttributes.OnEachLogin,
            V1ConnectionSetUserRootAttributes.OnFirstLogin => SetUserRootAttributes.OnFirstLogin,
            V1ConnectionSetUserRootAttributes.NeverOnLogin => SetUserRootAttributes.NeverOnLogin,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static V1ConnectionChallengeUi FromApi(ChallengeUi source) => source switch
        {
            ChallengeUi.Both => V1ConnectionChallengeUi.Both,
            ChallengeUi.AutoFill => V1ConnectionChallengeUi.AutoFill,
            ChallengeUi.Button => V1ConnectionChallengeUi.Button,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

        internal static ChallengeUi ToApi(V1ConnectionChallengeUi source) => source switch
        {
            V1ConnectionChallengeUi.Both => ChallengeUi.Both,
            V1ConnectionChallengeUi.AutoFill => ChallengeUi.AutoFill,
            V1ConnectionChallengeUi.Button => ChallengeUi.Button,
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
            var clients = await api.Connections.GetEnabledClientsAsync(new EnabledClientsGetRequest() { ConnectionId = connectionId }, cancellationToken: cancellationToken);

            var l = new List<string>(clients.Count);
            foreach (var client in clients)
                if (client.ClientId is not null)
                    l.Add(client.ClientId);

            return l.ToArray();
        }

        /// <inheritdoc />
        protected override async Task<V1ConnectionConf?> Get(IManagementApiClient api, string id, string defaultNamespace, CancellationToken cancellationToken)
        {
            try
            {
                var self = await api.Connections.GetAsync(id, cancellationToken: cancellationToken);
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
                        var connection = await api.Connections.GetAsync(connectionId, cancellationToken: cancellationToken);
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

                var list = await api.Connections.GetAllAsync(new GetConnectionsRequest(), (PaginationInfo?)null!, cancellationToken: cancellationToken);
                var self = list.FirstOrDefault(i => i.Name == conf.Name);
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

            var req = new ConnectionCreateRequest();
            ApplyToApi(conf, req);

            req.Strategy = conf.Strategy;

            // calculate options: depends on current strategy, but we need to potentially patch the existing resource
            if (conf.Options is not null)
            {
                if (conf.Strategy == "auth0")
                {
                    req.Options = new ConnectionOptions();
                    ApplyToApi(conf.Options, (ConnectionOptions)req.Options);
                }
                else
                {
                    var options = (dynamic)new JObject();
                    req.Options = options;
                    ApplyToApi(conf.Options, ref options);
                }
            }

            var self = await api.Connections.CreateAsync(req, cancellationToken);
            if (self is null)
                throw new InvalidOperationException();

            Logger.LogInformation("{EntityTypeName} successfully created connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, self.Id, conf.Name, conf.Strategy);
            return self.Id;
        }

        /// <inheritdoc />
        protected override async Task Update(IManagementApiClient api, string id, V1ConnectionConf? last, V1ConnectionConf conf, string defaultNamespace, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} updating connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);

            var req = new ConnectionUpdateRequest();
            ApplyToApi(conf, req);

            // name has to be cleared for an update
            req.Name = null!;

            // calculate options: depends on current strategy, but we need to potentially patch the existing resource
            if (conf.Options is not null)
            {
                var current = await api.Connections.GetAsync(id, cancellationToken: cancellationToken);
                if (current.Strategy == "auth0")
                {
                    var options = (ConnectionOptions)current.Options ?? new ConnectionOptions();
                    req.Options = options;
                    ApplyToApi(conf.Options, options);
                }
                else
                {
                    var options = (dynamic)((JObject?)current.Options ?? new JObject());
                    req.Options = options;
                    ApplyToApi(conf.Options, ref options);
                }
            }

            await api.Connections.UpdateAsync(id, req, cancellationToken);
            await UpdateEnabledClientsAsync(api, id, conf, defaultNamespace, cancellationToken);

            Logger.LogInformation("{EntityTypeName} successfully updated connection in Auth0 with ID: {ConnectionId}, name: {ConnectionName} and strategy: {Strategy}", EntityTypeName, id, conf.Name, conf.Strategy);
        }

        /// <summary>
        /// Applies the specified configuration to the request object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        internal static void ApplyToApi(V1ConnectionConf source, ConnectionBase target)
        {
            if (source.Name is { } name)
                target.Name = name;

            if (source.DisplayName is { } displayName)
                target.DisplayName = displayName;

            if (source.Metadata is { } metadata)
                target.Metadata = metadata;

            if (source.Realms is { } realms)
                target.Realms = realms;

            if (source.IsDomainConnection is not null)
                target.IsDomainConnection = source.IsDomainConnection ?? false;

            if (source.ShowAsButton is { } showAsButton)
                target.ShowAsButton = showAsButton;
        }

        void ApplyToApi(V1ConnectionOptions source, ConnectionOptions target)
        {
            if (source.Validation is { } validation)
            {
                target.Validation ??= new ConnectionOptionsValidation();
                ApplyToApi(validation, target.Validation);
            }

            if (source.NonPersistentAttributes is { } nonPersistentAttributes)
                target.NonPersistentAttributes = nonPersistentAttributes;

            if (source.Precedence is { } precedence)
                target.Precedence = Array.ConvertAll(precedence, ToApi);

            if (source.Attributes is { } attributes)
            {
                target.Attributes ??= new ConnectionOptionsAttributes();
                ApplyToApi(attributes, target.Attributes);
            }

            if (source.EnableScriptContext is { } enableScriptContext)
                target.EnableScriptContext = enableScriptContext;

            if (source.EnableDatabaseCustomization is { } enableDatabaseCustomization)
                target.EnableDatabaseCustomization = enableDatabaseCustomization;

            if (source.ImportMode is { } importMode)
                target.ImportMode = importMode;

            if (source.CustomScripts is { } customScripts)
            {
                target.CustomScripts ??= new ConnectionOptionsCustomScripts();
                ApplyToApi(customScripts, target.CustomScripts);
            }

            if (source.AuthenticationMethods is { } authenticationMethods)
            {
                target.AuthenticationMethods ??= new ConnectionOptionsAuthenticationMethods();
                ApplyToApi(authenticationMethods, target.AuthenticationMethods);
            }

            if (source.PasskeyOptions is { } passkeyOptions)
            {
                target.PasskeyOptions ??= new ConnectionOptionsPasskeyOptions();
                ApplyToApi(passkeyOptions, target.PasskeyOptions);
            }

            if (source.PasswordPolicy is { } passwordPolicy)
                target.PasswordPolicy = ToApi(passwordPolicy);

            if (source.PasswordComplexityOptions is { } passwordComplexityOptions)
            {
                target.PasswordComplexityOptions ??= new ConnectionOptionsPasswordComplexityOptions();
                ApplyToApi(passwordComplexityOptions, target.PasswordComplexityOptions);
            }

            if (source.PasswordHistory is { } passwordHistory)
            {
                target.PasswordHistory ??= new ConnectionOptionsPasswordHistory();
                ApplyToApi(passwordHistory, target.PasswordHistory);
            }

            if (source.PasswordNoPersonalInfo is { } passwordNoPersonalInfo)
            {
                target.PasswordNoPersonalInfo ??= new ConnectionOptionsPasswordNoPersonalInfo();
                ApplyToApi(passwordNoPersonalInfo, target.PasswordNoPersonalInfo);
            }

            if (source.PasswordDictionary is { } passwordDictionary)
            {
                target.PasswordDictionary ??= new ConnectionOptionsPasswordDictionary();
                ApplyToApi(passwordDictionary, target.PasswordDictionary);
            }

            if (source.ApiEnableUsers is { } apiEnableUsers)
                target.ApiEnableUsers = apiEnableUsers;

            if (source.BasicProfile is { } basicProfile)
                target.BasicProfile = basicProfile;

            if (source.ExtAdmin is { } extAdmin)
                target.ExtAdmin = extAdmin;

            if (source.ExtIsSuspended is { } extIsSuspended)
                target.ExtIsSuspended = extIsSuspended;

            if (source.ExtAgreedTerms is { } extAgreedTerms)
                target.ExtAgreedTerms = extAgreedTerms;

            if (source.ExtGroups is { } extGroups)
                target.ExtGroups = extGroups;

            if (source.ExtAssignedPlans is { } extAssignedPlans)
                target.ExtAssignedPlans = extAssignedPlans;

            if (source.ExtProfile is { } extProfile)
                target.ExtProfile = extProfile;

            if (source.DisableSelfServiceChangePassword is { } disableSelfServiceChangePassword)
                target.DisableSelfServiceChangePassword = disableSelfServiceChangePassword;

            if (source.UpstreamParams is { } upstreamParams)
                target.UpstreamParams = upstreamParams;

            if (source.SetUserRootAttributes is { } setUserRootAttributes)
                target.SetUserRootAttributes = ToApi(setUserRootAttributes);

            if (source.GatewayAuthentication is { } gatewayAuthentication)
            {
                target.GatewayAuthentication ??= new GatewayAuthentication();
                ApplyToApi(gatewayAuthentication, target.GatewayAuthentication);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsValidation source, ConnectionOptionsValidation target)
        {
            if (source.UserName is { } userName)
            {
                target.UserName ??= new ConnectionOptionsUserName();
                ApplyToApi(userName, target.UserName);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUserName source, ConnectionOptionsUserName target)
        {
            if (source.Min is { } min)
                target.Min = min;

            if (source.Max is { } max)
                target.Max = max;
        }

        static void ApplyToApi(V1ConnectionOptionsAttributes source, ConnectionOptionsAttributes target)
        {
            if (source.Email is { } email)
            {
                target.Email ??= new ConnectionOptionsEmailAttribute();
                ApplyToApi(email, target.Email);
            }

            if (source.PhoneNumber is { } phoneNumber)
            {
                target.PhoneNumber ??= new ConnectionOptionsPhoneNumberAttribute();
                ApplyToApi(phoneNumber, target.PhoneNumber);
            }

            if (source.Username is { } username)
            {
                target.Username ??= new ConnectionOptionsUsernameAttribute();
                ApplyToApi(username, target.Username);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsEmailAttribute source, ConnectionOptionsEmailAttribute target)
        {
            if (source.Identifier is { } identifier)
            {
                target.Identifier ??= new ConnectionOptionsAttributeIdentifier();
                ApplyToApi(identifier, target.Identifier);
            }

            if (source.ProfileRequired is { } profileRequired)
                target.ProfileRequired = profileRequired;

            if (source.Signup is { } signup)
            {
                target.Signup ??= new ConnectionOptionsEmailSignup();
                ApplyToApi(signup, target.Signup);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsEmailSignup source, ConnectionOptionsEmailSignup target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new ConnectionOptionsVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPhoneNumberAttribute source, ConnectionOptionsPhoneNumberAttribute target)
        {
            if (source.Signup is { } signup)
            {
                target.Signup ??= new ConnectionOptionsPhoneNumberSignup();
                ApplyToApi(signup, target.Signup);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPhoneNumberSignup source, ConnectionOptionsPhoneNumberSignup target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);

            if (source.Verification is { } verification)
            {
                target.Verification ??= new ConnectionOptionsVerification();
                ApplyToApi(verification, target.Verification);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUsernameAttribute source, ConnectionOptionsUsernameAttribute target)
        {
            if (source.Identifier is { } identifier)
            {
                target.Identifier ??= new ConnectionOptionsAttributeIdentifier();
                ApplyToApi(identifier, target.Identifier);
            }

            if (source.ProfileRequired is { } profileRequired)
                target.ProfileRequired = profileRequired;

            if (source.Signup is { } signup)
            {
                target.Signup ??= new ConnectionOptionsUsernameSignup();
                ApplyToApi(signup, target.Signup);
            }

            if (source.Validation is { } validation)
            {
                target.Validation ??= new ConnectionOptionsAttributeValidation();
                ApplyToApi(validation, target.Validation);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsUsernameSignup source, ConnectionOptionsUsernameSignup target)
        {
            if (source.Status is { } status)
                target.Status = ToApi(status);
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeIdentifier source, ConnectionOptionsAttributeIdentifier target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeValidation source, ConnectionOptionsAttributeValidation target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;

            if (source.MaxLength is { } maxLength)
                target.MaxLength = maxLength;

            if (source.AllowedTypes is { } allowedTypes)
            {
                target.AllowedTypes ??= new ConnectionOptionsAttributeAllowedTypes();
                ApplyToApi(allowedTypes, target.AllowedTypes);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsAttributeAllowedTypes source, ConnectionOptionsAttributeAllowedTypes target)
        {
            if (source.Email is { } email)
                target.Email = email;

            if (source.PhoneNumber is { } phoneNumber)
                target.PhoneNumber = phoneNumber;
        }

        static void ApplyToApi(V1ConnectionOptionsVerification source, ConnectionOptionsVerification target)
        {
            if (source.Active is { } active)
                target.Active = active;
        }

        static void ApplyToApi(V1ConnectionOptionsCustomScripts source, ConnectionOptionsCustomScripts target)
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

        static void ApplyToApi(V1ConnectionOptionsAuthenticationMethods source, ConnectionOptionsAuthenticationMethods target)
        {
            if (source.Password is { } password)
            {
                target.Password ??= new ConnectionOptionsPasswordAuthenticationMethod();
                ApplyToApi(password, target.Password);
            }

            if (source.Passkey is { } passkey)
            {
                target.Passkey ??= new ConnectionOptionsPasskeyAuthenticationMethod();
                ApplyToApi(passkey, target.Passkey);
            }
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordAuthenticationMethod source, ConnectionOptionsPasswordAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasskeyAuthenticationMethod source, ConnectionOptionsPasskeyAuthenticationMethod target)
        {
            if (source.Enabled is { } enabled)
                target.Enabled = enabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasskeyOptions source, ConnectionOptionsPasskeyOptions target)
        {
            if (source.ChallengeUi is { } challengeUi)
                target.ChallengeUi = ToApi(challengeUi);

            if (source.ProgressiveEnrollmentEnabled is { } progressiveEnrollmentEnabled)
                target.ProgressiveEnrollmentEnabled = progressiveEnrollmentEnabled;

            if (source.LocalEnrollmentEnabled is { } localEnrollmentEnabled)
                target.LocalEnrollmentEnabled = localEnrollmentEnabled;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordComplexityOptions source, ConnectionOptionsPasswordComplexityOptions target)
        {
            if (source.MinLength is { } minLength)
                target.MinLength = minLength;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordHistory source, ConnectionOptionsPasswordHistory target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Size is { } size)
                target.Size = size;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordNoPersonalInfo source, ConnectionOptionsPasswordNoPersonalInfo target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;
        }

        static void ApplyToApi(V1ConnectionOptionsPasswordDictionary source, ConnectionOptionsPasswordDictionary target)
        {
            if (source.Enable is { } enable)
                target.Enable = enable;

            if (source.Dictionary is { } dictionary)
                target.Dictionary = dictionary;
        }

        static void ApplyToApi(V1ConnectionGatewayAuthentication source, GatewayAuthentication target)
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
            var patch = TransformToNewtonsoftJson<V1ConnectionOptions, JObject>(source);
            if (patch is null)
                return;

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
                var req = new List<EnabledClientsToUpdate>();

                // apply existing clients, disabled by default
                foreach (var current in await api.Connections.GetEnabledClientsAsync(new() { ConnectionId = id }, cancellationToken: cancellationToken))
                    if (current.ClientId is not null)
                        req.Add(new() { ClientId = current.ClientId, Status = false });

                // add or enable clients specified in the configuration
                foreach (var clientId in await ResolveClientRefsToIds(api, conf.EnabledClients, defaultNamespace, cancellationToken))
                {
                    var existing = req.FirstOrDefault(i => i.ClientId == clientId);
                    if (existing is null)
                        req.Add(existing = new() { ClientId = clientId });

                    existing.Status = true;
                }

                // apply update
                if (req.Count > 0)
                    await api.Connections.UpdateEnabledClientsAsync(id, new() { EnabledClients = req }, cancellationToken: cancellationToken);
            }
        }

        /// <inheritdoc />
        protected override async Task DeletedAsync(IManagementApiClient api, string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("{EntityTypeName} deleting connection from Auth0 with ID: {ConnectionId} (reason: Kubernetes entity deleted)", EntityTypeName, id);
            await api.Connections.DeleteAsync(id, cancellationToken);
            Logger.LogInformation("{EntityTypeName} successfully deleted connection from Auth0 with ID: {ConnectionId}", EntityTypeName, id);
        }

    }

}
