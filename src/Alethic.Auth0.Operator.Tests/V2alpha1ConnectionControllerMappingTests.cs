using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Converters;
using Alethic.Auth0.Operator.Core.Models.Connection.V1;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;
using Alethic.Auth0.Operator.Models;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    [System.Runtime.Versioning.RequiresPreviewFeatures]
    public class V2alpha1ConnectionControllerMappingTests
    {

        [TestMethod]
        public void FromApi_Null_ReturnsNull()
        {
            Assert.IsNull(V2alpha1ConnectionController.FromApi((GetConnectionResponseContent?)null));
        }

        [TestMethod]
        public void FromApi_Connection_MapsScalarProperties()
        {
            var source = new GetConnectionResponseContent
            {
                Name = "test-conn",
                DisplayName = "Test Connection",
                Strategy = "auth0",
                Realms = ["realm1", "realm2"],
                IsDomainConnection = true,
                ShowAsButton = false,
            };

            var result = V2alpha1ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.AreEqual("test-conn", result.Name);
            Assert.AreEqual("Test Connection", result.DisplayName);
            Assert.AreEqual(V2alpha1ConnectionStrategy.Auth0, result.Strategy);
            CollectionAssert.AreEqual(new[] { "realm1", "realm2" }, result.Realms);
            Assert.AreEqual(true, result.IsDomainConnection);
            Assert.AreEqual(false, result.ShowAsButton);
        }

        [TestMethod]
        public void FromApi_Connection_EnabledClientsIsNull()
        {
            var result = V2alpha1ConnectionController.FromApi(new GetConnectionResponseContent { Name = "x", Strategy = "auth0" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.EnabledClients);
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategyOptions_AllStrategySpecificPropertiesNull()
        {
            var result = V2alpha1ConnectionController.FromApi(new GetConnectionResponseContent { Name = "x", Strategy = "auth0" });
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
                Name = "x",
                Strategy = "auth0",
                Metadata = new System.Collections.Generic.Dictionary<string, string> { ["env"] = "prod" },
            };

            var result = V2alpha1ConnectionController.FromApi(source);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Metadata);
            Assert.AreEqual("prod", result.Metadata["env"]?.ToString());
        }

        [TestMethod]
        public void FromApi_Connection_NullStrategy_MapsNull()
        {
            var result = V2alpha1ConnectionController.FromApi(new GetConnectionResponseContent { Name = "no-strat" });
            Assert.IsNotNull(result);
            Assert.IsNull(result.Strategy);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsName()
        {
            var conf = new V2alpha1ConnectionConf { Name = "my-conn" };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "placeholder" };
            V2alpha1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("my-conn", req.Name);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsDisplayName()
        {
            var conf = new V2alpha1ConnectionConf { DisplayName = "My Conn" };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V2alpha1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("My Conn", req.DisplayName);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsRealms()
        {
            var conf = new V2alpha1ConnectionConf { Realms = ["r1"] };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V2alpha1ConnectionController.ApplyToApi(conf, req);
            CollectionAssert.AreEqual(new[] { "r1" }, req.Realms?.ToList());
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsIsDomainConnection()
        {
            var conf = new V2alpha1ConnectionConf { IsDomainConnection = true };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V2alpha1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.IsDomainConnection);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_MapsShowAsButton()
        {
            var conf = new V2alpha1ConnectionConf { ShowAsButton = true };
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "conn" };
            V2alpha1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual(true, req.ShowAsButton);
        }

        [TestMethod]
        public void ApplyToApi_ConnectionBase_NullFieldsLeaveTargetUnchanged()
        {
            var conf = new V2alpha1ConnectionConf();
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum("auth0"), Name = "original" };
            V2alpha1ConnectionController.ApplyToApi(conf, req);
            Assert.AreEqual("original", req.Name);
        }

        [TestMethod]
        public void FromApi_DecryptionKey_PrivateKey_MapsCustomProperty()
        {
            var source = ConnectionDecryptionKeySaml.FromString("pem-value");

            var result = V2alpha1ConnectionController.FromApi(source);

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

            var result = V2alpha1ConnectionController.FromApi(source);

            Assert.IsNotNull(result);
            Assert.IsNull(result.PrivateKey);
            Assert.IsNotNull(result.KeyPair);
            Assert.AreEqual("cert-value", result.KeyPair.Cert);
            Assert.AreEqual("key-value", result.KeyPair.Key);
        }

        [TestMethod]
        public void ApplyToApi_SamlOptions_MapsPrivateKeyDecryptionKey()
        {
            var options = V2alpha1ConnectionController.ToApi(
                new V2alpha1ConnectionOptionsSaml
                {
                    SignInEndpoint = "https://idp.example/signin",
                    DecryptionKey = new V2alpha1ConnectionDecryptionKeySaml
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
            var options = V2alpha1ConnectionController.ToApi(
                new V2alpha1ConnectionOptionsPingFederate
                {
                    PingFederateBaseUrl = "https://pingfed.example",
                    DecryptionKey = new V2alpha1ConnectionDecryptionKeySaml
                    {
                        KeyPair = new V2alpha1ConnectionDecryptionKeySamlCert
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
                UpstreamParams = global::Auth0.ManagementApi.Core.Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties>?>.Of(
                    new Dictionary<string, ConnectionUpstreamAdditionalProperties?>
                    {
                        ["login_hint"] = ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamValue(
                            new ConnectionUpstreamValue { Value = "user@example.com" }),
                    }),
            };

            var result = V2alpha1ConnectionController.FromApi(source);

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
                UpstreamParams = global::Auth0.ManagementApi.Core.Optional<Dictionary<string, ConnectionUpstreamAdditionalProperties>?>.Of(
                    new Dictionary<string, ConnectionUpstreamAdditionalProperties?>
                    {
                        ["resource"] = ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamAlias(
                            new ConnectionUpstreamAlias { Alias = ConnectionUpstreamAliasEnum.Resource }),
                    }),
            };

            var result = V2alpha1ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.UpstreamParams);
            Assert.IsTrue(result.UpstreamParams.TryGetValue("resource", out var value));
            Assert.AreEqual(V2alpha1ConnectionUpstreamAliasEnum.Resource, value?.Alias);
            Assert.IsNull(value?.Value);
        }

        [TestMethod]
        public void FromApi_Facebook_MapsUpstreamParams()
        {
            var source = new ConnectionOptionsFacebook
            {
                UpstreamParams = new Dictionary<string, ConnectionUpstreamAdditionalProperties?>
                {
                    ["prompt"] = ConnectionUpstreamAdditionalProperties.FromConnectionUpstreamAlias(
                        new ConnectionUpstreamAlias { Alias = ConnectionUpstreamAliasEnum.Prompt }),
                },
            };

            var result = V2alpha1ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.UpstreamParams);
            Assert.IsTrue(result.UpstreamParams.TryGetValue("prompt", out var value));
            Assert.AreEqual(V2alpha1ConnectionUpstreamAliasEnum.Prompt, value?.Alias);
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

            var result = V2alpha1ConnectionController.FromApi(source);

            Assert.IsNotNull(result?.OidcMetadata);
            Assert.AreEqual("https://issuer.example/authorize", result.OidcMetadata.AuthorizationEndpoint);
            CollectionAssert.AreEqual(new[] { "sub", "email" }, result.OidcMetadata.ClaimsSupported);
            CollectionAssert.AreEqual(new[] { "openid", "profile" }, result.OidcMetadata.ScopesSupported);
        }

        [TestMethod]
        public void ApplyToApi_Oidc_MapsOidcMetadata()
        {
            var options = V2alpha1ConnectionController.ToApi(
                new V2alpha1ConnectionOptionsOidc
                {
                    ClientId = "client-id",
                    ClientSecret = "client-secret",
                    OidcMetadata = new V2alpha1ConnectionOptionsOidcMetadata
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

            var result = V2alpha1ConnectionController.FromApi(source);

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
                        Strategy = "samlp",
                        Options = JsonSerializer.Deserialize<V1ConnectionOptions>("""
                        {
                          "decryptionKey": {
                            "privateKey": "pem-value"
                          }
                        }
                        """)
                    }
                }
            };

            var result = InvokeConvert(source);

            Assert.IsNotNull(result.Spec.Conf?.Options?.Saml?.DecryptionKey);
            Assert.AreEqual("pem-value", result.Spec.Conf.Options.Saml.DecryptionKey.PrivateKey);
            Assert.IsNull(result.Spec.Conf.Options.Saml.DecryptionKey.KeyPair);
        }

        [TestMethod]
        public void Converter_RevertConf_SamlDecryptionKey_PrefersPrivateKey()
        {
            var source = new V2alpha1Connection
            {
                Spec =
                {
                    Conf = new V2alpha1ConnectionConf
                    {
                        Name = "saml-conn",
                        Strategy = V2alpha1ConnectionStrategy.Saml,
                        Options = new V2alpha1ConnectionOptions
                        {
                            Saml = new V2alpha1ConnectionOptionsSaml
                            {
                                DecryptionKey = new V2alpha1ConnectionDecryptionKeySaml
                                {
                                    PrivateKey = "pem-value",
                                    KeyPair = new V2alpha1ConnectionDecryptionKeySamlCert
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

            Assert.AreEqual(JsonValueKind.Object, decryptionKey.ValueKind);
            Assert.AreEqual("pem-value", decryptionKey.GetProperty("privateKey").GetString());
            Assert.IsFalse(decryptionKey.TryGetProperty("keyPair", out JsonElement _));
        }

        [TestMethod]
        public void Converter_RevertConf_PingFederateDecryptionKey_UsesKeyPair()
        {
            var source = new V2alpha1Connection
            {
                Spec =
                {
                    Conf = new V2alpha1ConnectionConf
                    {
                        Name = "pingfed-conn",
                        Strategy = V2alpha1ConnectionStrategy.PingFederate,
                        Options = new V2alpha1ConnectionOptions
                        {
                            PingFederate = new V2alpha1ConnectionOptionsPingFederate
                            {
                                PingFederateBaseUrl = "https://pingfed.example",
                                DecryptionKey = new V2alpha1ConnectionDecryptionKeySaml
                                {
                                    KeyPair = new V2alpha1ConnectionDecryptionKeySamlCert
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
            var keyPair = decryptionKey.GetProperty("keyPair");

            Assert.IsFalse(decryptionKey.TryGetProperty("privateKey", out JsonElement _));
            Assert.AreEqual("cert-value", keyPair.GetProperty("cert").GetString());
            Assert.AreEqual("key-value", keyPair.GetProperty("key").GetString());
        }

        [TestMethod]
        public void Roundtrip_ScalarProperties()
        {
            var source = new GetConnectionResponseContent
            {
                Name = "roundtrip",
                DisplayName = "Roundtrip",
                Strategy = "auth0",
                IsDomainConnection = false,
                ShowAsButton = true,
            };

            var conf = V2alpha1ConnectionController.FromApi(source)!;
            var req = new CreateConnectionRequestContent { Strategy = new ConnectionIdentityProviderEnum(System.Text.Json.JsonSerializer.Serialize(conf.Strategy).Trim('"')), Name = conf.Name! };
            V2alpha1ConnectionController.ApplyToApi(conf, req);

            Assert.AreEqual(source.Name, req.Name);
            Assert.AreEqual(source.DisplayName, req.DisplayName);
            Assert.AreEqual(source.IsDomainConnection, req.IsDomainConnection);
            Assert.AreEqual(source.ShowAsButton, req.ShowAsButton);
        }

        static V2alpha1Connection InvokeConvert(V1Connection source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V2alpha1Connection)method!.Invoke(converter, [source])!;
        }

        static V1Connection InvokeRevert(V2alpha1Connection source)
        {
            var converter = CreateConverter();
            var method = converter.GetType().GetMethod("Revert", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method);
            return (V1Connection)method!.Invoke(converter, [source])!;
        }

        static object CreateConverter()
        {
            var converterType = Type.GetType("Alethic.Auth0.Operator.Converters.ConnectionConverter+V1ToV2alpha1, Alethic.Auth0.Operator");
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

    }

}
