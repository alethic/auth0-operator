using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Converters;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;
using Auth0.ManagementApi.Connections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    [System.Runtime.Versioning.RequiresPreviewFeatures]
    public class V2alpha3ConnectionControllerMappingTests
    {

        [TestMethod]
        public void FromApi_Null_ReturnsNull()
        {
            Assert.IsNull(V2alpha3ConnectionController.FromApi((GetConnectionResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_Connection_MapsScalarProperties()
        {
            var source = new GetConnectionResponseContent
            {
                Id = "con_test_conn",
                Name = "test-conn",
                DisplayName = "Test Connection",
                Strategy = ConnectionResponseContentAuth0Strategy.Values.Auth0,
                Realms = ["realm1", "realm2"],
                IsDomainConnection = true,
                ShowAsButton = false,
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("test-conn", result.Name);
            Assert.AreEqual("Test Connection", result.DisplayName);
            Assert.AreEqual(V2alpha3ConnectionStrategy.Auth0, result.Strategy);
            CollectionAssert.AreEqual(new[] { "realm1", "realm2" }, result.Realms);
            Assert.AreEqual(true, result.IsDomainConnection);
            Assert.AreEqual(false, result.ShowAsButton);
        }

        [TestMethod]
        public void FromApi_Connection_EnabledClientsIsNull()
        {
            var result = V2alpha3ConnectionController.FromApi(new GetConnectionResponseContent { Id = "con_x", Name = "x", Strategy = ConnectionResponseContentAuth0Strategy.Values.Auth0 });
            Assert.IsNotNull(result);
            Assert.IsNull(result.EnabledClients);
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategyOptions_AllStrategySpecificPropertiesNull()
        {
            var result = V2alpha3ConnectionController.FromApi(new GetConnectionResponseContent { Id = "con_x", Name = "x", Strategy = ConnectionResponseContentAuth0Strategy.Values.Auth0 });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Options?.Auth0);
            Assert.IsNull(result.Options?.Oidc);
            Assert.IsNull(result.Metadata);
        }

        [TestMethod]
        public void FromApi_Connection_MapsMetadata()
        {
            var source = new GetConnectionResponseContent
            {
                Id = "con_metadata",
                Name = "x",
                Strategy = ConnectionResponseContentAuth0Strategy.Values.Auth0,
                Metadata = new System.Collections.Generic.Dictionary<string, string?> { ["env"] = "prod" },
            };

            var result = V2alpha3ConnectionController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Metadata);
            Assert.AreEqual("prod", result.Metadata["env"]?.ToString());
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategy_MapsNull()
        {
            var result = V2alpha3ConnectionController.FromApi(new GetConnectionResponseContent { Name = "no-strat" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Strategy);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsName()
        {
            var conf = new V2alpha3ConnectionConf { Name = "my-conn" };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(ConnectionResponseContentAuth0Strategy.Values.Auth0), Name = "placeholder" };
            V2alpha3ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("my-conn", req.Name);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsDisplayName()
        {
            var conf = new V2alpha3ConnectionConf { DisplayName = "My Conn" };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(ConnectionResponseContentAuth0Strategy.Values.Auth0), Name = "conn" };
            V2alpha3ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("My Conn", req.DisplayName);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsRealms()
        {
            var conf = new V2alpha3ConnectionConf { Realms = ["r1"] };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(ConnectionResponseContentAuth0Strategy.Values.Auth0), Name = "conn" };
            V2alpha3ConnectionController.ApplyToApi(conf, req);
            CollectionAssert.AreEqual(new[] { "r1" }, req.Realms?.ToList());
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsIsDomainConnection()
        {
            var conf = new V2alpha3ConnectionConf { IsDomainConnection = true };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(ConnectionResponseContentAuth0Strategy.Values.Auth0), Name = "conn" };
            V2alpha3ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.IsDomainConnection);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsShowAsButton()
        {
            var conf = new V2alpha3ConnectionConf { ShowAsButton = true };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(ConnectionResponseContentAuth0Strategy.Values.Auth0), Name = "conn" };
            V2alpha3ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.ShowAsButton);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_NullFieldsLeaveTargetUnchanged()
        {
            var conf = new V2alpha3ConnectionConf();
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(ConnectionResponseContentAuth0Strategy.Values.Auth0), Name = "original" };
            V2alpha3ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("original", req.Name);
        }

        [TestMethod]
        public void FromApi_DecryptionKey_PrivateKey_MapsCustomProperty()
        {
            var source = ConnectionDecryptionKeySaml.FromString("pem-value");

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("pem-value", result.PrivateKey);
            Assert.IsNull(result.KeyPair);
        }

        [TestMethod]
        public void FromApi_DecryptionKey_KeyPair_MapsCustomProperty()
        {
            var source = ConnectionDecryptionKeySaml.FromConnectionDecryptionKeySamlCert(
                new ConnectionDecryptionKeySamlCert
                {
                    Cert = "cert-value",
                    Key = "key-value",
                });

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNull(result.PrivateKey);
            Assert.IsNotNull(result.KeyPair);
            Assert.AreEqual("cert-value", result.KeyPair.Cert);
            Assert.AreEqual("key-value", result.KeyPair.Key);
        }

        [TestMethod]
        public void ApplyToApi_SamlOptions_MapsPrivateKeyDecryptionKey()
        {
            var options = V2alpha3ConnectionController.ToApi(
                new V2alpha3ConnectionOptionsSaml
                {
                    SignInEndpoint = "https://idp.example/signin",
                    DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                    {
                        PrivateKey = "pem-value",
                    },
                });

            Assert.IsNotNull(options);
            Assert.IsNotNull(options.DecryptionKey);
            Assert.IsTrue(options.DecryptionKey.IsString());
            Assert.AreEqual("pem-value", options.DecryptionKey.AsString());
        }

        [TestMethod]
        public void ApplyToApi_PingFederateOptions_MapsKeyPairDecryptionKey()
        {
            var options = V2alpha3ConnectionController.ToApi(
                new V2alpha3ConnectionOptionsPingFederate
                {
                    PingFederateBaseUrl = "https://pingfed.example",
                    DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                    {
                        KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert
                        {
                            Cert = "cert-value",
                            Key = "key-value",
                        },
                    },
                });

            Assert.IsNotNull(options);
            Assert.IsNotNull(options.DecryptionKey);
            Assert.IsTrue(options.DecryptionKey.IsConnectionDecryptionKeySamlCert());
            var cert = options.DecryptionKey.AsConnectionDecryptionKeySamlCert();
            Assert.AreEqual("cert-value", cert.Cert);
            Assert.AreEqual("key-value", cert.Key);
        }

        [TestMethod]
        public void FromApi_GitHub_MapsUpstreamParams()
        {
            var source = new ConnectionOptionsGitHub
            {
                UpstreamParams = global::Auth0.ManagementApi.Core.Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?>.Of(
                    new Dictionary<string, ConnectionUpstreamAdditionalProperties?>
                    {
                        ["login_hint"] = ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamValue(
                            new ConnectionUpstreamValue { Value = "user@example.com" }),
                    }),
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.UpstreamParams);
            Assert.IsTrue(result.UpstreamParams.TryGetValue("login_hint", out var value));
            Assert.AreEqual("user@example.com", value?.Value);
            Assert.IsNull(value?.Alias);
        }

        [TestMethod]
        public void FromApi_PingFederate_MapsUpstreamParams()
        {
            var source = new ConnectionOptionsPingFederate
            {
                PingFederateBaseUrl = "https://pingfed.example",
                UpstreamParams = global::Auth0.ManagementApi.Core.Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties?>?>.Of(
                    new Dictionary<string, ConnectionUpstreamAdditionalProperties?>
                    {
                        ["resource"] = ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamAlias(
                            new ConnectionUpstreamAlias { Alias = ConnectionUpstreamAliasEnum.Resource }),
                    }),
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.UpstreamParams);
            Assert.IsTrue(result.UpstreamParams.TryGetValue("resource", out var value));
            Assert.AreEqual(V2alpha3ConnectionUpstreamAliasEnum.Resource, value?.Alias);
            Assert.IsNull(value?.Value);
        }

        [TestMethod]
        public void FromApi_Facebook_MapsUpstreamParams()
        {
            var source = new ConnectionOptionsFacebook
            {
                UpstreamParams = new Dictionary<string, ConnectionUpstreamAdditionalProperties>
                {
                    ["prompt"] = ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamAlias(
                        new ConnectionUpstreamAlias { Alias = ConnectionUpstreamAliasEnum.Prompt }),
                },
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.UpstreamParams);
            Assert.IsTrue(result.UpstreamParams.TryGetValue("prompt", out var value));
            Assert.AreEqual(V2alpha3ConnectionUpstreamAliasEnum.Prompt, value?.Alias);
            Assert.IsNull(value?.Value);
        }

        [TestMethod]
        public void FromApi_Oidc_MapsOidcMetadata()
        {
            var source = new ConnectionOptionsOidc
            {
                ClientId = "client-id",
                OidcMetadata = new ConnectionOptionsOidcMetadata
                {
                    AuthorizationEndpoint = "https://issuer.example/authorize",
                    Issuer = "https://issuer.example",
                    JwksUri = "https://issuer.example/jwks",
                    ClaimsSupported = ["sub", "email"],
                    ScopesSupported = global::Auth0.ManagementApi.Core.Optional<IEnumerable<string>?>.Of(["openid", "profile"]),
                },
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.OidcMetadata);
            Assert.AreEqual("https://issuer.example/authorize", result.OidcMetadata.AuthorizationEndpoint);
            CollectionAssert.AreEqual(new[] { "sub", "email" }, result.OidcMetadata.ClaimsSupported);
            CollectionAssert.AreEqual(new[] { "openid", "profile" }, result.OidcMetadata.ScopesSupported);
        }

        [TestMethod]
        public void ApplyToApi_Oidc_MapsOidcMetadata()
        {
            var options = V2alpha3ConnectionController.ToApi(
                new V2alpha3ConnectionOptionsOidc
                {
                    ClientId = "client-id",
                    ClientSecret = "client-secret",
                    OidcMetadata = new V2alpha3ConnectionOptionsOidcMetadata
                    {
                        AuthorizationEndpoint = "https://issuer.example/authorize",
                        ClaimsSupported = ["sub", "email"],
                        ScopesSupported = ["openid", "profile"],
                    },
                });

            Assert.IsNotNull(options.OidcMetadata);
            Assert.AreEqual("https://issuer.example/authorize", options.OidcMetadata.AuthorizationEndpoint);
            CollectionAssert.AreEqual(new[] { "sub", "email" }, options.OidcMetadata.ClaimsSupported?.ToArray());
            Assert.IsTrue(options.OidcMetadata.ScopesSupported.IsDefined);
            CollectionAssert.AreEqual(new[] { "openid", "profile" }, options.OidcMetadata.ScopesSupported.Value?.ToArray());
        }

        [TestMethod]
        public void FromApi_Okta_MapsOidcMetadata()
        {
            var source = new ConnectionOptionsOkta
            {
                ClientId = "client-id",
                OidcMetadata = new ConnectionOptionsOidcMetadata
                {
                    AuthorizationEndpoint = "https://issuer.example/authorize",
                    Issuer = "https://issuer.example",
                    JwksUri = "https://issuer.example/jwks",
                    ResponseTypesSupported = ["code", "id_token"],
                },
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.OidcMetadata);
            Assert.AreEqual("https://issuer.example", result.OidcMetadata.Issuer);
            CollectionAssert.AreEqual(new[] { "code", "id_token" }, result.OidcMetadata.ResponseTypesSupported);
        }

        [TestMethod]
        public void Converter_ConvertConf_SamlDecryptionKey_MapsPrivateKey()
        {
            var source = new V1Connection
            {
                Spec =
                {
                    Conf = new V1ConnectionConf
                    {
                        Name = "saml-conn",
                        Strategy = ConnectionResponseContentSamlStrategy.Values.Samlp,
                        Options = JsonSerializer.Deserialize<V1ConnectionOptions>("""
                        {
                          "decryptionKey": "pem-value"
                        }
                        """)
                    }
                }
            };

            var result = InvokeConvert(source);

            var decryptionKey = result.Spec.Conf?.Options?.Saml?.DecryptionKey;
            Assert.IsNotNull(decryptionKey);
            Assert.AreEqual("pem-value", decryptionKey.PrivateKey);
            Assert.IsNull(decryptionKey.KeyPair);
        }

        [TestMethod]
        public void Converter_ConvertConf_OidcMetadata_MapsThroughAuth0ApiModel()
        {
            var source = new V1Connection
            {
                Spec =
                {
                    Conf = new V1ConnectionConf
                    {
                        Name = "oidc-conn",
                        Strategy = "oidc",
                        Options = JsonSerializer.Deserialize<V1ConnectionOptions>("""
                        {
                          "client_id": "client-id",
                          "issuer": "https://issuer.example",
                          "jwks_uri": "https://issuer.example/jwks.json",
                          "oidc_metadata": {
                            "authorization_endpoint": "https://issuer.example/authorize",
                            "issuer": "https://issuer.example",
                            "jwks_uri": "https://issuer.example/jwks.json",
                            "claims_supported": ["sub", "email"],
                            "scopes_supported": ["openid", "profile"]
                          }
                        }
                        """)
                    }
                }
            };

            var result = InvokeConvert(source);
            var metadata = result.Spec.Conf?.Options?.Oidc?.OidcMetadata;

            Assert.IsNotNull(metadata);
            Assert.AreEqual("https://issuer.example/authorize", metadata.AuthorizationEndpoint);
            CollectionAssert.AreEqual(new[] { "sub", "email" }, metadata.ClaimsSupported);
            CollectionAssert.AreEqual(new[] { "openid", "profile" }, metadata.ScopesSupported);
        }

        [TestMethod]
        public void Converter_RevertConf_SamlDecryptionKey_PrefersPrivateKey()
        {
            var source = new V2alpha3Connection
            {
                Spec =
                {
                    Conf = new V2alpha3ConnectionConf
                    {
                        Name = "saml-conn",
                        Strategy = V2alpha3ConnectionStrategy.Saml,
                        Options = new V2alpha3ConnectionOptions
                        {
                            Saml = new V2alpha3ConnectionOptionsSaml
                            {
                                DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                                {
                                    PrivateKey = "pem-value",
                                    KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert
                                    {
                                        Cert = "cert-value",
                                        Key = "key-value",
                                    },
                                },
                            },
                        },
                    }
                }
            };

            var result = InvokeRevert(source);
            var decryptionKey = GetAdditionalProperty(result.Spec.Conf?.Options, "decryptionKey");

            Assert.AreEqual(JsonValueKind.String, decryptionKey.ValueKind);
            Assert.AreEqual("pem-value", decryptionKey.GetString());
        }

        [TestMethod]
        public void Converter_RevertConf_OidcMetadata_MapsThroughAuth0ApiModel()
        {
            var source = new V2alpha3Connection
            {
                Spec =
                {
                    Conf = new V2alpha3ConnectionConf
                    {
                        Name = "oidc-conn",
                        Strategy = V2alpha3ConnectionStrategy.Oidc,
                        Options = new V2alpha3ConnectionOptions
                        {
                            Oidc = new V2alpha3ConnectionOptionsOidc
                            {
                                ClientId = "client-id",
                                OidcMetadata = new V2alpha3ConnectionOptionsOidcMetadata
                                {
                                    AuthorizationEndpoint = "https://issuer.example/authorize",
                                    ClaimsSupported = ["sub", "email"],
                                    Issuer = "https://issuer.example",
                                    JwksUri = "https://issuer.example/jwks.json",
                                    ScopesSupported = ["openid", "profile"],
                                },
                            },
                        },
                    }
                }
            };

            var result = InvokeRevert(source);
            var oidcMetadata = GetAdditionalProperty(result.Spec.Conf?.Options, "oidc_metadata");

            Assert.AreEqual("https://issuer.example/authorize", oidcMetadata.GetProperty("authorization_endpoint").GetString());
            CollectionAssert.AreEqual(new[] { "sub", "email" }, oidcMetadata.GetProperty("claims_supported").EnumerateArray().Select(static i => i.GetString()).ToArray());
            CollectionAssert.AreEqual(new[] { "openid", "profile" }, oidcMetadata.GetProperty("scopes_supported").EnumerateArray().Select(static i => i.GetString()).ToArray());
        }

        [TestMethod]
        public void Converter_RevertConf_PingFederateDecryptionKey_UsesKeyPair()
        {
            var source = new V2alpha3Connection
            {
                Spec =
                {
                    Conf = new V2alpha3ConnectionConf
                    {
                        Name = "pingfed-conn",
                        Strategy = V2alpha3ConnectionStrategy.PingFederate,
                        Options = new V2alpha3ConnectionOptions
                        {
                            PingFederate = new V2alpha3ConnectionOptionsPingFederate
                            {
                                PingFederateBaseUrl = "https://pingfed.example",
                                DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                                {
                                    KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert
                                    {
                                        Cert = "cert-value",
                                        Key = "key-value",
                                    },
                                },
                            },
                        },
                    }
                }
            };

            var result = InvokeRevert(source);
            var decryptionKey = GetAdditionalProperty(result.Spec.Conf?.Options, "decryptionKey");

            Assert.IsFalse(decryptionKey.TryGetProperty("privateKey", out JsonElement _));
            Assert.AreEqual("cert-value", decryptionKey.GetProperty("cert").GetString());
            Assert.AreEqual("key-value", decryptionKey.GetProperty("key").GetString());
        }

        [TestMethod]
        public void Roundtrip_ScalarProperties()
        {
            var source = new GetConnectionResponseContent
            {
                Id = "con_roundtrip",
                Name = "roundtrip",
                DisplayName = "Roundtrip",
                Strategy = ConnectionResponseContentAuth0Strategy.Values.Auth0,
                IsDomainConnection = false,
                ShowAsButton = true,
            };

            var conf = V2alpha3ConnectionController.FromApi(source)!;
            var req = new CreateConnectionRequestContent { Strategy = ConnectionIdentityProviderEnum.FromCustom(V2alpha3ConnectionController.ToApiStrategy(conf.Strategy!.Value)), Name = conf.Name! };
            V2alpha3ConnectionController.ApplyToApi(conf, req);

            Assert.AreEqual(source.Strategy, req.Strategy.Value);
            Assert.AreEqual(source.Name, req.Name);
            Assert.AreEqual(source.DisplayName, req.DisplayName);
            Assert.AreEqual(source.IsDomainConnection, req.IsDomainConnection);
            Assert.AreEqual(source.ShowAsButton, req.ShowAsButton);
        }

        // ──────────────────────── enabledClients scoped/additive update ──────────

        [TestMethod]
        public void ComputeEnabledClientsRequest_EnablesAllDesiredNotYetEnabled()
        {
            var req = V2alpha3ConnectionController.ComputeEnabledClientsRequest(
                new HashSet<string> { "a", "b" },
                System.Array.Empty<string>(),
                new HashSet<string>());

            CollectionAssert.AreEquivalent(new[] { "a", "b" }, req.Select(static i => i.ClientId).ToArray());
            Assert.IsTrue(req.All(static i => i.Status == true));
        }

        [TestMethod]
        public void ComputeEnabledClientsRequest_DisablesOnlyPreviouslyManagedNoLongerDesired()
        {
            // previously managed a and b, both currently enabled; now only a is desired
            // => b gets disabled, a is already enabled and stays untouched
            var req = V2alpha3ConnectionController.ComputeEnabledClientsRequest(
                new HashSet<string> { "a" },
                new[] { "a", "b" },
                new HashSet<string> { "a", "b" });

            var byId = req.ToDictionary(static i => i.ClientId!, static i => i.Status);
            Assert.IsFalse(byId.ContainsKey("a"));
            Assert.AreEqual(false, byId["b"]);
        }

        [TestMethod]
        public void ComputeEnabledClientsRequest_LeavesUnmanagedClientsUntouched()
        {
            // "c" was never managed by this Connection (e.g. enabled via a ConnectionClient resource)
            // and is not desired => it must not appear in the request at all, even though it is enabled.
            var req = V2alpha3ConnectionController.ComputeEnabledClientsRequest(
                new HashSet<string> { "a" },
                new[] { "a" },
                new HashSet<string> { "c" });

            Assert.IsFalse(req.Any(static i => i.ClientId == "c"));
            CollectionAssert.AreEqual(new[] { "a" }, req.Select(static i => i.ClientId).ToArray());
        }

        [TestMethod]
        public void ComputeEnabledClientsRequest_NoDesiredNoManaged_ProducesEmpty()
        {
            var req = V2alpha3ConnectionController.ComputeEnabledClientsRequest(
                new HashSet<string>(),
                System.Array.Empty<string>(),
                new HashSet<string>());

            Assert.AreEqual(0, req.Count);
        }

        [TestMethod]
        public void ComputeEnabledClientsRequest_InSync_ProducesEmpty()
        {
            // everything desired is already enabled and nothing needs disabling => no API call at all
            var req = V2alpha3ConnectionController.ComputeEnabledClientsRequest(
                new HashSet<string> { "a", "b" },
                new[] { "a", "b" },
                new HashSet<string> { "a", "b" });

            Assert.AreEqual(0, req.Count);
        }

        [TestMethod]
        public void ComputeEnabledClientsRequest_DisableSkippedWhenAlreadyDisabled()
        {
            // b is no longer desired but is also no longer enabled => nothing to send
            var req = V2alpha3ConnectionController.ComputeEnabledClientsRequest(
                new HashSet<string> { "a" },
                new[] { "a", "b" },
                new HashSet<string> { "a" });

            Assert.AreEqual(0, req.Count);
        }

        // ──────────────────────── drift detection ────────────────────────────────

        [TestMethod]
        public void ConfRequiresUpdate_IdenticalConf_ReturnsFalse()
        {
            var conf = new V2alpha3ConnectionConf { DisplayName = "Display", ShowAsButton = true };
            var last = new V2alpha3ConnectionConf { DisplayName = "Display", ShowAsButton = true, Realms = ["realm"] };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_IgnoresCreateOnlyAndSeparatelyReconciledFields()
        {
            // name/strategy are never sent on update and enabledClients has its own delta logic;
            // mismatches there must not force a PATCH (the prod google connection has exactly this name skew)
            var conf = new V2alpha3ConnectionConf
            {
                Name = "googleOauth2",
                Strategy = V2alpha3ConnectionStrategy.GoogleOAuth2,
                EnabledClients = [new Core.Models.V1ClientReference { Name = "some-client" }],
                DisplayName = "Google",
            };
            var last = new V2alpha3ConnectionConf
            {
                Name = "google-oauth2",
                Strategy = V2alpha3ConnectionStrategy.GoogleOAuth2,
                EnabledClients = [new Core.Models.V1ClientReference { Id = "abc123" }],
                DisplayName = "Google",
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ChangedDisplayName_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { DisplayName = "New" };
            var last = new V2alpha3ConnectionConf { DisplayName = "Old" };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ChangedOptionLeaf_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { Options = new V2alpha3ConnectionOptions { GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true } } };
            var last = new V2alpha3ConnectionConf { Options = new V2alpha3ConnectionOptions { GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = false, Profile = true } } };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_OptionLeafSubset_ReturnsFalse()
        {
            // Auth0 fills server-side defaults the spec does not manage; only spec-set leaves count
            var conf = new V2alpha3ConnectionConf { Options = new V2alpha3ConnectionOptions { GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true } } };
            var last = new V2alpha3ConnectionConf { Options = new V2alpha3ConnectionOptions { GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true, Profile = true } } };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        static V2alpha3Connection InvokeConvert(V1Connection source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V2alpha3Connection)method!.Invoke(converter, [source])!;
        }

        static V1Connection InvokeRevert(V2alpha3Connection source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Revert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V1Connection)method!.Invoke(converter, [source])!;
        }

        static object CreateConverter()
        {
            var converterType = Type.GetType("Alethic.Auth0.Operator.Converters.ConnectionConverter+V1ToV2alpha3, Alethic.Auth0.Operator");
            Assert.IsNotNull(converterType);
            return Activator.CreateInstance(converterType!, nonPublic: true)!;
        }

        static JsonElement GetAdditionalProperty(V1ConnectionOptions? options, string propertyName)
        {
            Assert.IsNotNull(options?.AdditionalProperties);
            Assert.IsTrue(options.AdditionalProperties.TryGetValue(propertyName, out var value));
            Assert.IsInstanceOfType<JsonElement>(value);
            return (JsonElement)value!;
        }

        [TestMethod]
        public void FromApiStrategy_Null_ReturnsNull()
        {
            Assert.IsNull(V2alpha3ConnectionController.FromApiStrategy(null));
        }

        [TestMethod]
        public void FromApiStrategy_UnknownStrategy_ReturnsNull()
        {
            Assert.IsNull(V2alpha3ConnectionController.FromApiStrategy("custom"));
        }

        [TestMethod]
        public void FromApiStrategy_GoogleOAuth2WireValue_MapsToEnum()
        {
            Assert.AreEqual(V2alpha3ConnectionStrategy.GoogleOAuth2, V2alpha3ConnectionController.FromApiStrategy("google-oauth2"));
        }

        [TestMethod]
        public void ToApiStrategy_ProducesAuth0WireValues()
        {
            Assert.AreEqual("google-oauth2", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.GoogleOAuth2));
            Assert.AreEqual("waad", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.AzureAd));
            Assert.AreEqual("samlp", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.Saml));
            Assert.AreEqual("google-apps", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.GoogleApps));
            Assert.AreEqual("evernote-sandbox", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.EvernoteSandbox));
            Assert.AreEqual("paypal-sandbox", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.PaypalSandbox));
            Assert.AreEqual("salesforce-community", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.SalesforceCommunity));
            Assert.AreEqual("salesforce-sandbox", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.SalesforceSandbox));
            Assert.AreEqual("auth0", V2alpha3ConnectionController.ToApiStrategy(V2alpha3ConnectionStrategy.Auth0));
        }

        [TestMethod]
        public void StrategyMapping_RoundTrips_AllEnumMembers()
        {
            foreach (var strategy in Enum.GetValues<V2alpha3ConnectionStrategy>())
                Assert.AreEqual(strategy, V2alpha3ConnectionController.FromApiStrategy(V2alpha3ConnectionController.ToApiStrategy(strategy)), $"Strategy {strategy} did not round-trip through the Auth0 wire value.");
        }

        [TestMethod]
        public void FromApi_Connection_GoogleOAuth2Strategy_MapsStrategy()
        {
            var source = new GetConnectionResponseContent
            {
                Id = "con_google",
                Name = "google-oauth2",
                Strategy = ConnectionResponseContentGoogleOAuth2Strategy.Values.GoogleOauth2,
            };

            var result = V2alpha3ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha3ConnectionStrategy.GoogleOAuth2, result.Strategy);
        }

        // ──────────────────────── Cross-App Access / id token session expiry ─────

        [TestMethod]
        public void FromApi_Connection_MapsCrossAppAccess()
        {
            var result = V2alpha3ConnectionController.FromApi(new GetConnectionResponseContent
            {
                Name = "test",
                CrossAppAccessRequestingApp = new CrossAppAccessRequestingApp { Active = true },
                CrossAppAccessResourceApp = new CrossAppAccessResourceApp { Status = new CrossAppAccessResourceAppStatusEnum(CrossAppAccessResourceAppStatusEnum.Values.Enabled) },
            });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.CrossAppAccessRequestingApp?.Active);
            Assert.AreEqual(V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum.Enabled, result.CrossAppAccessResourceApp?.Status);
        }

        [TestMethod]
        public void ApplyToApi_Create_SetsCrossAppAccess()
        {
            var target = new CreateConnectionRequestContent { Name = "test", Strategy = new ConnectionIdentityProviderEnum(ConnectionIdentityProviderEnum.Values.Auth0) };
            V2alpha3ConnectionController.ApplyToApi(new V2alpha3ConnectionConf
            {
                Name = "test",
                CrossAppAccessRequestingApp = new V2alpha3ConnectionCrossAppAccessRequestingApp { Active = true },
                CrossAppAccessResourceApp = new V2alpha3ConnectionCrossAppAccessResourceApp { Status = V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum.Disabled },
            }, target);

            Assert.IsTrue(target.CrossAppAccessRequestingApp?.Active);
            Assert.AreEqual(CrossAppAccessResourceAppStatusEnum.Values.Disabled, target.CrossAppAccessResourceApp?.Status.Value);
        }

        [TestMethod]
        public void ApplyToApi_Update_SetsCrossAppAccess()
        {
            var target = new UpdateConnectionRequestContent();
            V2alpha3ConnectionController.ApplyToApi(new V2alpha3ConnectionConf
            {
                CrossAppAccessRequestingApp = new V2alpha3ConnectionCrossAppAccessRequestingApp { Active = false },
                CrossAppAccessResourceApp = new V2alpha3ConnectionCrossAppAccessResourceApp { Status = V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum.Enabled },
            }, target);

            Assert.IsFalse(target.CrossAppAccessRequestingApp?.Active);
            Assert.IsTrue(target.CrossAppAccessResourceApp.IsDefined);
            Assert.AreEqual(CrossAppAccessResourceAppStatusEnum.Values.Enabled, target.CrossAppAccessResourceApp.Value?.Status.Value);
        }

        [TestMethod]
        public void FromApi_OptionsOidc_MapsIdTokenSessionExpirySupported()
        {
            var result = V2alpha3ConnectionController.FromApi(new ConnectionOptionsOidc { ClientId = "client_1", IdTokenSessionExpirySupported = true });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IdTokenSessionExpirySupported);
        }

        [TestMethod]
        public void ToApi_OptionsOidc_MapsIdTokenSessionExpirySupported()
        {
            var result = V2alpha3ConnectionController.ToApi(new V2alpha3ConnectionOptionsOidc { IdTokenSessionExpirySupported = true });

            Assert.IsTrue(result.IdTokenSessionExpirySupported);
        }

        [TestMethod]
        public void ToApi_OptionsOkta_MapsIdTokenSessionExpirySupported()
        {
            var result = V2alpha3ConnectionController.ToApi(new V2alpha3ConnectionOptionsOkta { IdTokenSessionExpirySupported = false });

            Assert.IsFalse(result.IdTokenSessionExpirySupported);
        }

        [TestMethod]
        public void ConfRequiresUpdate_DetectsCrossAppAccessDrift()
        {
            var conf = new V2alpha3ConnectionConf
            {
                CrossAppAccessRequestingApp = new V2alpha3ConnectionCrossAppAccessRequestingApp { Active = true },
                CrossAppAccessResourceApp = new V2alpha3ConnectionCrossAppAccessResourceApp { Status = V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum.Enabled },
            };

            var same = new V2alpha3ConnectionConf
            {
                CrossAppAccessRequestingApp = new V2alpha3ConnectionCrossAppAccessRequestingApp { Active = true },
                CrossAppAccessResourceApp = new V2alpha3ConnectionCrossAppAccessResourceApp { Status = V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum.Enabled },
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, same));
            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, new V2alpha3ConnectionConf()));

            same.CrossAppAccessResourceApp!.Status = V2alpha3ConnectionCrossAppAccessResourceAppStatusEnum.Disabled;
            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, same));
        }

    }

}
