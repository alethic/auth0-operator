using System.Linq;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V2alpha3ResourceServerControllerMappingTests
    {

        // ──────────────────────── FromApi null-guard tests ────────────────────────

        [TestMethod]
        public void FromApi_ResourceServer_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ResourceServerController.FromApi((ResourceServer?)null));
        }

        [TestMethod]
        public void FromApi_ResourceServerScope_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ResourceServerController.FromApi((ResourceServerScope?)null));
        }

        [TestMethod]
        public void FromApi_SigningAlgorithm_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ResourceServerController.FromApi((SigningAlgorithmEnum?)null));
        }

        [TestMethod]
        public void FromApi_TokenEncryption_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ResourceServerController.FromApi((ResourceServerTokenEncryption?)null));
        }

        [TestMethod]
        public void FromApi_TokenEncryptionKey_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ResourceServerController.FromApi((ResourceServerTokenEncryptionKey?)null));
        }

        [TestMethod]
        public void FromApi_ProofOfPossession_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3ResourceServerController.FromApi((ResourceServerProofOfPossession?)null));
        }

        // ──────────────────────── FromApi property-mapping tests ────────────────────────

        [TestMethod]
        public void FromApi_ResourceServer_MapsScalarProperties()
        {
            var source = new ResourceServer
            {
                Id = "rs-1",
                Identifier = "https://api.example.com",
                Name = "My API",
                SigningSecret = "secret",
                TokenLifetime = 86400,
                TokenLifetimeForWeb = 7200,
                AllowOfflineAccess = true,
                SkipConsentForVerifiableFirstPartyClients = false,
                EnforcePolicies = true,
            };

            var result = V2alpha3ResourceServerController.FromApi(source)!;

            Assert.AreEqual("rs-1", result.Id);
            Assert.AreEqual("https://api.example.com", result.Identifier);
            Assert.AreEqual("My API", result.Name);
            Assert.AreEqual("secret", result.SigningSecret);
            Assert.AreEqual(86400, result.TokenLifetime);
            Assert.AreEqual(7200, result.TokenLifetimeForWeb);
            Assert.IsTrue(result.AllowOfflineAccess);
            Assert.IsFalse(result.SkipConsentForVerifiableFirstPartyClients);
            Assert.IsTrue(result.EnforcePolicies);
        }

        [TestMethod]
        public void FromApi_ResourceServerScope_MapsProperties()
        {
            var result = V2alpha3ResourceServerController.FromApi(new ResourceServerScope { Value = "read:users", Description = "Read users" })!;

            Assert.AreEqual("read:users", result.Value);
            Assert.AreEqual("Read users", result.Description);
        }

        [TestMethod]
        public void FromApi_ResourceServer_MapsScopes()
        {
            var source = new ResourceServer
            {
                Identifier = "https://api.example.com",
                Scopes = [new ResourceServerScope { Value = "read:data" }, new ResourceServerScope { Value = "write:data" }],
            };

            var result = V2alpha3ResourceServerController.FromApi(source)!;

            Assert.AreEqual(2, result.Scopes!.Length);
            Assert.AreEqual("read:data", result.Scopes[0].Value);
            Assert.AreEqual("write:data", result.Scopes[1].Value);
        }

        [TestMethod]
        public void FromApi_TokenEncryptionKey_MapsProperties()
        {
            var result = V2alpha3ResourceServerController.FromApi(new ResourceServerTokenEncryptionKey
            {
                Name = "mykey",
                Kid = "kid-1",
                Pem = "-----BEGIN PUBLIC KEY-----",
            })!;

            Assert.AreEqual("mykey", result.Name);
            Assert.AreEqual("kid-1", result.Kid);
            Assert.AreEqual("-----BEGIN PUBLIC KEY-----", result.Pem);
        }

        [TestMethod]
        public void FromApi_TokenEncryption_MapsFormatAndKey()
        {
            var result = V2alpha3ResourceServerController.FromApi(new ResourceServerTokenEncryption
            {
                Format = new ResourceServerTokenEncryptionFormatEnum(ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe),
                EncryptionKey = new ResourceServerTokenEncryptionKey { Name = "k1" },
            })!;

            Assert.AreEqual(V2alpha3ResourceServerTokenFormat.CompactNestedJwe, result.Format);
            Assert.AreEqual("k1", result.EncryptionKey!.Name);
        }

        [TestMethod]
        public void FromApi_ProofOfPossession_MapsProperties()
        {
            var result = V2alpha3ResourceServerController.FromApi(new ResourceServerProofOfPossession
            {
                Required = true,
                Mechanism = new ResourceServerProofOfPossessionMechanismEnum(ResourceServerProofOfPossessionMechanismEnum.Values.Mtls),
            })!;

            Assert.IsTrue(result.Required);
            Assert.AreEqual(V2alpha3ResourceServerMechanism.Mtls, result.Mechanism);
        }

        [TestMethod]
        public void FromApi_ResourceServer_MapsNewRequestBackedProperties()
        {
            var source = new ResourceServer
            {
                Identifier = "https://api.example.com",
                AllowOnlineAccess = true,
                AllowOnlineAccessWithEphemeralSessions = false,
                AuthorizationPolicy = new ResourceServerAuthorizationPolicy { PolicyId = "pol_123" },
                SubjectTypeAuthorization = new ResourceServerSubjectTypeAuthorization
                {
                    Client = new ResourceServerSubjectTypeAuthorizationClient
                    {
                        Policy = new ResourceServerSubjectTypeAuthorizationClientPolicyEnum(ResourceServerSubjectTypeAuthorizationClientPolicyEnum.Values.RequireClientGrant),
                    },
                    User = new ResourceServerSubjectTypeAuthorizationUser
                    {
                        Policy = new ResourceServerSubjectTypeAuthorizationUserPolicyEnum(ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.AllowAll),
                    },
                },
            };

            var result = V2alpha3ResourceServerController.FromApi(source)!;

            Assert.IsTrue(result.AllowOnlineAccess);
            Assert.IsFalse(result.AllowOnlineAccessWithEphemeralSessions);
            Assert.AreEqual("pol_123", result.AuthorizationPolicy!.PolicyId);
            Assert.AreEqual(V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy.RequireClientGrant, result.SubjectTypeAuthorization!.Client!.Policy);
            Assert.AreEqual(V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.AllowAll, result.SubjectTypeAuthorization.User!.Policy);
        }

        // ──────────────────────── FromApi enum tests ────────────────────────

        [TestMethod]
        public void FromApi_SigningAlgorithm_Hs256() => Assert.AreEqual(V2alpha3ResourceServerSigningAlgorithm.HS256, V2alpha3ResourceServerController.FromApi(new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Hs256)));
        [TestMethod]
        public void FromApi_SigningAlgorithm_Rs256() => Assert.AreEqual(V2alpha3ResourceServerSigningAlgorithm.RS256, V2alpha3ResourceServerController.FromApi(new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Rs256)));
        [TestMethod]
        public void FromApi_SigningAlgorithm_Ps256() => Assert.AreEqual(V2alpha3ResourceServerSigningAlgorithm.PS256, V2alpha3ResourceServerController.FromApi(new SigningAlgorithmEnum(SigningAlgorithmEnum.Values.Ps256)));

        [TestMethod]
        public void FromApi_TokenDialect_AccessToken() => Assert.AreEqual(V2alpha3ResourceServerTokenDialect.AccessToken, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(ResourceServerTokenDialectResponseEnum.Values.AccessToken)));
        [TestMethod]
        public void FromApi_TokenDialect_AccessTokenAuthz() => Assert.AreEqual(V2alpha3ResourceServerTokenDialect.AccessTokenAuthZ, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(ResourceServerTokenDialectResponseEnum.Values.AccessTokenAuthz)));
        [TestMethod]
        public void FromApi_TokenDialect_Rfc9068Profile() => Assert.AreEqual(V2alpha3ResourceServerTokenDialect.Rfc9068Profile, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(ResourceServerTokenDialectResponseEnum.Values.Rfc9068Profile)));
        [TestMethod]
        public void FromApi_TokenDialect_Rfc9068ProfileAuthz() => Assert.AreEqual(V2alpha3ResourceServerTokenDialect.Rfc9068ProfileAuthz, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(ResourceServerTokenDialectResponseEnum.Values.Rfc9068ProfileAuthz)));

        [TestMethod]
        public void FromApi_ConsentPolicy_TransactionalAuthorizationWithMfa_MapsCorrectly()
        {
            Assert.AreEqual(V2alpha3ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
                V2alpha3ResourceServerController.FromApi(new ResourceServerConsentPolicyEnum(ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa)));
        }

        [TestMethod]
        public void FromApi_TokenFormat_CompactNestedJwe_MapsCorrectly()
        {
            Assert.AreEqual(V2alpha3ResourceServerTokenFormat.CompactNestedJwe,
                V2alpha3ResourceServerController.FromApi(new ResourceServerTokenEncryptionFormatEnum(ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe)));
        }

        [TestMethod]
        public void FromApi_Mechanism_Mtls_MapsCorrectly()
        {
            Assert.AreEqual(V2alpha3ResourceServerMechanism.Mtls,
                V2alpha3ResourceServerController.FromApi(new ResourceServerProofOfPossessionMechanismEnum(ResourceServerProofOfPossessionMechanismEnum.Values.Mtls)));
        }

        [TestMethod]
        public void FromApi_Mechanism_Dpop_MapsCorrectly()
        {
            Assert.AreEqual(V2alpha3ResourceServerMechanism.Dpop,
                V2alpha3ResourceServerController.FromApi(new ResourceServerProofOfPossessionMechanismEnum(ResourceServerProofOfPossessionMechanismEnum.Values.Dpop)));
        }

        // ──────────────────────── ToApi enum tests ────────────────────────

        [TestMethod]
        public void ToApi_SigningAlgorithm_Hs256() => Assert.AreEqual(SigningAlgorithmEnum.Values.Hs256, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerSigningAlgorithm.HS256).Value);
        [TestMethod]
        public void ToApi_SigningAlgorithm_Rs256() => Assert.AreEqual(SigningAlgorithmEnum.Values.Rs256, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerSigningAlgorithm.RS256).Value);
        [TestMethod]
        public void ToApi_SigningAlgorithm_Ps256() => Assert.AreEqual(SigningAlgorithmEnum.Values.Ps256, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerSigningAlgorithm.PS256).Value);

        [TestMethod]
        public void ToApi_TokenDialect_AccessToken() => Assert.AreEqual(ResourceServerTokenDialectSchemaEnum.Values.AccessToken, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.AccessToken).Value);
        [TestMethod]
        public void ToApi_TokenDialect_AccessTokenAuthz() => Assert.AreEqual(ResourceServerTokenDialectSchemaEnum.Values.AccessTokenAuthz, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.AccessTokenAuthZ).Value);
        [TestMethod]
        public void ToApi_TokenDialect_Rfc9068Profile() => Assert.AreEqual(ResourceServerTokenDialectSchemaEnum.Values.Rfc9068Profile, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.Rfc9068Profile).Value);
        [TestMethod]
        public void ToApi_TokenDialect_Rfc9068ProfileAuthz() => Assert.AreEqual(ResourceServerTokenDialectSchemaEnum.Values.Rfc9068ProfileAuthz, V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.Rfc9068ProfileAuthz).Value);

        [TestMethod]
        public void ToApi_ConsentPolicy_TransactionalAuthorizationWithMfa_MapsCorrectly()
        {
            Assert.AreEqual(ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa,
                V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa).Value);
        }

        [TestMethod]
        public void ToApi_TokenFormat_CompactNestedJwe_MapsCorrectly()
        {
            Assert.AreEqual(ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe,
                V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenFormat.CompactNestedJwe).Value);
        }

        [TestMethod]
        public void ToApi_Mechanism_Mtls_MapsCorrectly()
        {
            Assert.AreEqual(ResourceServerProofOfPossessionMechanismEnum.Values.Mtls,
                V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerMechanism.Mtls).Value);
        }

        [TestMethod]
        public void ToApi_Mechanism_Dpop_MapsCorrectly()
        {
            Assert.AreEqual(ResourceServerProofOfPossessionMechanismEnum.Values.Dpop,
                V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerMechanism.Dpop).Value);
        }

        [TestMethod]
        public void ToApi_Scope_MapsProperties()
        {
            var result = V2alpha3ResourceServerController.ToApi(new V2alpha3ResourceServerScope { Value = "read:data", Description = "Read data" });

            Assert.AreEqual("read:data", result.Value);
            Assert.AreEqual("Read data", result.Description);
        }

        // ──────────────────────── Roundtrip tests ────────────────────────

        [TestMethod]
        public void SigningAlgorithm_Roundtrip_Hs256()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerSigningAlgorithm.HS256);
            Assert.AreEqual(V2alpha3ResourceServerSigningAlgorithm.HS256, V2alpha3ResourceServerController.FromApi((SigningAlgorithmEnum?)api));
        }
        [TestMethod]
        public void SigningAlgorithm_Roundtrip_Rs256()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerSigningAlgorithm.RS256); 
            Assert.AreEqual(V2alpha3ResourceServerSigningAlgorithm.RS256, V2alpha3ResourceServerController.FromApi((SigningAlgorithmEnum?)api));
        }
        [TestMethod]
        public void SigningAlgorithm_Roundtrip_Ps256()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerSigningAlgorithm.PS256); 
            Assert.AreEqual(V2alpha3ResourceServerSigningAlgorithm.PS256, V2alpha3ResourceServerController.FromApi((SigningAlgorithmEnum?)api));
        }

        [TestMethod]
        public void TokenDialect_Roundtrip_AccessToken()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.AccessToken); 
            Assert.AreEqual(V2alpha3ResourceServerTokenDialect.AccessToken, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(api.Value)));
        }
        [TestMethod]
        public void TokenDialect_Roundtrip_AccessTokenAuthZ()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.AccessTokenAuthZ); 
            Assert.AreEqual(V2alpha3ResourceServerTokenDialect.AccessTokenAuthZ, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(api.Value)));
        }
        [TestMethod]
        public void TokenDialect_Roundtrip_Rfc9068Profile()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.Rfc9068Profile); 
            Assert.AreEqual(V2alpha3ResourceServerTokenDialect.Rfc9068Profile, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(api.Value)));
        }
        [TestMethod]
        public void TokenDialect_Roundtrip_Rfc9068ProfileAuthz()
        {
            var api = V2alpha3ResourceServerController.ToApi(V2alpha3ResourceServerTokenDialect.Rfc9068ProfileAuthz);
            Assert.AreEqual(V2alpha3ResourceServerTokenDialect.Rfc9068ProfileAuthz, V2alpha3ResourceServerController.FromApi(new ResourceServerTokenDialectResponseEnum(api.Value)));
        }

        // ──────────────────────── ApplyToApi tests ────────────────────────

        [TestMethod]
        public void ApplyToApi_CreateRequest_MapsAllFields()
        {
            var conf = new V2alpha3ResourceServerConf
            {
                Identifier = "https://api.example.com",
                Name = "My API",
                SigningAlgorithm = V2alpha3ResourceServerSigningAlgorithm.RS256,
                SigningSecret = "secret",
                TokenLifetime = 86400,
                TokenLifetimeForWeb = 7200,
                AllowOfflineAccess = true,
                AllowOnlineAccess = true,
                AllowOnlineAccessWithEphemeralSessions = false,
                SkipConsentForVerifiableFirstPartyClients = false,
                VerificationLocation = "https://verify.example.com",
                TokenDialect = V2alpha3ResourceServerTokenDialect.AccessToken,
                EnforcePolicies = true,
                ConsentPolicy = V2alpha3ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
                Scopes = [new V2alpha3ResourceServerScope { Value = "read:data", Description = "Read" }],
                AuthorizationPolicy = new V2alpha3ResourceServerAuthorizationPolicy { PolicyId = "pol_123" },
                SubjectTypeAuthorization = new V2alpha3ResourceServerSubjectTypeAuthorization
                {
                    Client = new V2alpha3ResourceServerSubjectTypeAuthorizationClient
                    {
                        Policy = V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy.RequireClientGrant,
                    },
                    User = new V2alpha3ResourceServerSubjectTypeAuthorizationUser
                    {
                        Policy = V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy.AllowAll,
                    },
                },
            };

            var req = new CreateResourceServerRequestContent { Identifier = "https://api.example.com" };
            V2alpha3ResourceServerController.ApplyToApi(conf, req);

            Assert.AreEqual("https://api.example.com", req.Identifier);
            Assert.AreEqual("My API", req.Name);
            Assert.AreEqual(SigningAlgorithmEnum.Values.Rs256, req.SigningAlg?.Value);
            Assert.AreEqual("secret", req.SigningSecret);
            Assert.AreEqual(86400, req.TokenLifetime);
            Assert.IsTrue(req.AllowOfflineAccess);
            Assert.IsTrue(req.AllowOnlineAccess);
            Assert.IsFalse(req.AllowOnlineAccessWithEphemeralSessions);
            Assert.IsFalse(req.SkipConsentForVerifiableFirstPartyClients);
            Assert.AreEqual(ResourceServerTokenDialectSchemaEnum.Values.AccessToken, req.TokenDialect?.Value);
            Assert.IsTrue(req.EnforcePolicies);
            Assert.AreEqual(ResourceServerConsentPolicyEnum.Values.TransactionalAuthorizationWithMfa, req.ConsentPolicy.Value?.Value);
            Assert.AreEqual("pol_123", req.AuthorizationPolicy.Value!.PolicyId);
            Assert.IsNotNull(req.SubjectTypeAuthorization);
            Assert.AreEqual<string>(ResourceServerSubjectTypeAuthorizationClientPolicyEnum.Values.RequireClientGrant.ToString(), req.SubjectTypeAuthorization.Client!.Policy!.Value.ToString());
            Assert.AreEqual<string>(ResourceServerSubjectTypeAuthorizationUserPolicyEnum.Values.AllowAll.ToString(), req.SubjectTypeAuthorization.User!.Policy!.Value.ToString());
            Assert.IsNotNull(req.Scopes);
            Assert.AreEqual(1, req.Scopes.Count());
            Assert.AreEqual("read:data", req.Scopes.First().Value);
        }

        [TestMethod]
        public void ApplyToApi_UpdateRequest_DoesNotSetIdentifier()
        {
            var conf = new V2alpha3ResourceServerConf { Identifier = "https://api.example.com", Name = "My API" };

            var req = new UpdateResourceServerRequestContent();
            V2alpha3ResourceServerController.ApplyToApi(conf, req);

            Assert.AreEqual("My API", req.Name);
        }

        [TestMethod]
        public void ApplyToApi_TokenEncryption_MapsCorrectly()
        {
            var conf = new V2alpha3ResourceServerConf
            {
                TokenEncryption = new V2alpha3ResourceServerTokenEncryption
                {
                    Format = V2alpha3ResourceServerTokenFormat.CompactNestedJwe,
                    EncryptionKey = new V2alpha3ResourceServerTokenEncryptionKey
                    {
                        Name = "mykey",
                        Algorithm = "RSA-OAEP",
                        Kid = "kid-1",
                        Pem = "pem-data",
                    },
                },
            };

            var req = new CreateResourceServerRequestContent { Identifier = "https://api.example.com" };
            V2alpha3ResourceServerController.ApplyToApi(conf, req);

            Assert.AreEqual(ResourceServerTokenEncryptionFormatEnum.Values.CompactNestedJwe, req.TokenEncryption.Value!.Format.Value);
            Assert.AreEqual("mykey", req.TokenEncryption.Value!.EncryptionKey!.Name);
            Assert.AreEqual("kid-1", req.TokenEncryption.Value!.EncryptionKey!.Kid);
            Assert.AreEqual("pem-data", req.TokenEncryption.Value!.EncryptionKey!.Pem);
        }

        [TestMethod]
        public void ApplyToApi_ProofOfPossession_MapsCorrectly()
        {
            var conf = new V2alpha3ResourceServerConf
            {
                ProofOfPossession = new V2alpha3ResourceServerProofOfPossession
                {
                    Required = true,
                    Mechanism = V2alpha3ResourceServerMechanism.Mtls,
                },
            };

            var req = new CreateResourceServerRequestContent { Identifier = "https://api.example.com" };
            V2alpha3ResourceServerController.ApplyToApi(conf, req);
            Assert.AreEqual(ResourceServerProofOfPossessionMechanismEnum.Values.Mtls, req.ProofOfPossession.Value!.Mechanism.Value);
        }

        [TestMethod]
        public void ApplyToApi_NullOptionalFields_DoesNotOverwrite()
        {
            var conf = new V2alpha3ResourceServerConf { Name = "My API" };

            var req = new CreateResourceServerRequestContent { Identifier = "https://api.example.com" };
            V2alpha3ResourceServerController.ApplyToApi(conf, req);
            Assert.IsNull(req.TokenDialect);
            Assert.IsFalse(req.ConsentPolicy.IsDefined);
            Assert.IsFalse(req.TokenEncryption.IsDefined);
            Assert.IsFalse(req.ProofOfPossession.IsDefined);
            Assert.IsNull(req.Scopes);
            Assert.IsFalse(req.AuthorizationDetails.IsDefined);
        }

    }

}
