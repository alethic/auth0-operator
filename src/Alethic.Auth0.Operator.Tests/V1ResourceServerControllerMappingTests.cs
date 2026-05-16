using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V1;

using Auth0.ManagementApi.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V1ResourceServerControllerMappingTests
    {

        // ──────────────────────── FromApi null-guard tests ────────────────────────

        [TestMethod]
        public void FromApi_ResourceServer_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((ResourceServer?)null));
        }

        [TestMethod]
        public void FromApi_ResourceServerScope_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((ResourceServerScope?)null));
        }

        [TestMethod]
        public void FromApi_SigningAlgorithm_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((SigningAlgorithm?)null));
        }

        [TestMethod]
        public void FromApi_TokenDialect_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((TokenDialect?)null));
        }

        [TestMethod]
        public void FromApi_ConsentPolicy_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((ConsentPolicy?)null));
        }

        [TestMethod]
        public void FromApi_AuthorizationDetail_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((ResourceServerAuthorizationDetail?)null));
        }

        [TestMethod]
        public void FromApi_TokenEncryption_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((TokenEncryption?)null));
        }

        [TestMethod]
        public void FromApi_TokenEncryptionKey_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((TokenEncryptionKey?)null));
        }

        [TestMethod]
        public void FromApi_ProofOfPossession_Null_Returns_Null()
        {
            Assert.IsNull(V1ResourceServerController.FromApi((ProofOfPossession?)null));
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
                VerificationLocation = "https://verify.example.com",
                EnforcePolicies = true,
            };

            var result = V1ResourceServerController.FromApi(source)!;

            Assert.AreEqual("rs-1", result.Id);
            Assert.AreEqual("https://api.example.com", result.Identifier);
            Assert.AreEqual("My API", result.Name);
            Assert.AreEqual("secret", result.SigningSecret);
            Assert.AreEqual(86400, result.TokenLifetime);
            Assert.AreEqual(7200, result.TokenLifetimeForWeb);
            Assert.IsTrue(result.AllowOfflineAccess);
            Assert.IsFalse(result.SkipConsentForVerifiableFirstPartyClients);
            Assert.AreEqual("https://verify.example.com", result.VerificationLocation);
            Assert.IsTrue(result.EnforcePolicies);
        }

        [TestMethod]
        public void FromApi_ResourceServerScope_MapsProperties()
        {
            var result = V1ResourceServerController.FromApi(new ResourceServerScope { Value = "read:users", Description = "Read users" })!;

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

            var result = V1ResourceServerController.FromApi(source)!;

            Assert.AreEqual(2, result.Scopes!.Count);
            Assert.AreEqual("read:data", result.Scopes[0].Value);
            Assert.AreEqual("write:data", result.Scopes[1].Value);
        }

        [TestMethod]
        public void FromApi_AuthorizationDetail_MapsType()
        {
            var result = V1ResourceServerController.FromApi(new ResourceServerAuthorizationDetail { Type = "payment_initiation" })!;

            Assert.AreEqual("payment_initiation", result.Type);
        }

        [TestMethod]
        public void FromApi_TokenEncryptionKey_MapsProperties()
        {
            var result = V1ResourceServerController.FromApi(new TokenEncryptionKey
            {
                Name = "mykey",
                Algorithm = "RSA-OAEP",
                Kid = "kid-1",
                Pem = "-----BEGIN PUBLIC KEY-----",
            })!;

            Assert.AreEqual("mykey", result.Name);
            Assert.AreEqual("RSA-OAEP", result.Algorithm);
            Assert.AreEqual("kid-1", result.Kid);
            Assert.AreEqual("-----BEGIN PUBLIC KEY-----", result.Pem);
        }

        [TestMethod]
        public void FromApi_TokenEncryption_MapsFormatAndKey()
        {
            var result = V1ResourceServerController.FromApi(new TokenEncryption
            {
                Format = TokenFormat.CompactNestedJwe,
                EncryptionKey = new TokenEncryptionKey { Name = "k1" },
            })!;

            Assert.AreEqual(V1ResourceServerTokenFormat.CompactNestedJwe, result.Format);
            Assert.AreEqual("k1", result.EncryptionKey!.Name);
        }

        [TestMethod]
        public void FromApi_ProofOfPossession_MapsProperties()
        {
            var result = V1ResourceServerController.FromApi(new ProofOfPossession
            {
                Required = true,
                Mechanism = Mechanism.Mtls,
            })!;

            Assert.IsTrue(result.Required);
            Assert.AreEqual(V1ResourceServerMechanism.Mtls, result.Mechanism);
        }

        // ──────────────────────── FromApi enum tests ────────────────────────

        [TestMethod]
        [DataRow(SigningAlgorithm.HS256, V1ResourceServerSigningAlgorithm.HS256)]
        [DataRow(SigningAlgorithm.RS256, V1ResourceServerSigningAlgorithm.RS256)]
        [DataRow(SigningAlgorithm.PS256, V1ResourceServerSigningAlgorithm.PS256)]
        public void FromApi_SigningAlgorithm_MapsCorrectly(SigningAlgorithm input, V1ResourceServerSigningAlgorithm expected)
        {
            Assert.AreEqual(expected, V1ResourceServerController.FromApi((SigningAlgorithm?)input));
        }

        [TestMethod]
        [DataRow(TokenDialect.AccessToken, V1ResourceServerTokenDialect.AccessToken)]
        [DataRow(TokenDialect.AccessTokenAuthZ, V1ResourceServerTokenDialect.AccessTokenAuthZ)]
        [DataRow(TokenDialect.Rfc9068Profile, V1ResourceServerTokenDialect.Rfc9068Profile)]
        [DataRow(TokenDialect.Rfc9068ProfileAuthz, V1ResourceServerTokenDialect.Rfc9068ProfileAuthz)]
        public void FromApi_TokenDialect_MapsCorrectly(TokenDialect input, V1ResourceServerTokenDialect expected)
        {
            Assert.AreEqual(expected, V1ResourceServerController.FromApi((TokenDialect?)input));
        }

        [TestMethod]
        public void FromApi_ConsentPolicy_TransactionalAuthorizationWithMfa_MapsCorrectly()
        {
            Assert.AreEqual(V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
                V1ResourceServerController.FromApi((ConsentPolicy?)ConsentPolicy.TransactionalAuthorizationWithMfa));
        }

        [TestMethod]
        public void FromApi_TokenFormat_CompactNestedJwe_MapsCorrectly()
        {
            Assert.AreEqual(V1ResourceServerTokenFormat.CompactNestedJwe,
                V1ResourceServerController.FromApi(TokenFormat.CompactNestedJwe));
        }

        [TestMethod]
        public void FromApi_Mechanism_Mtls_MapsCorrectly()
        {
            Assert.AreEqual(V1ResourceServerMechanism.Mtls,
                V1ResourceServerController.FromApi(Mechanism.Mtls));
        }

        // ──────────────────────── ToApi enum tests ────────────────────────

        [TestMethod]
        [DataRow(V1ResourceServerSigningAlgorithm.HS256, SigningAlgorithm.HS256)]
        [DataRow(V1ResourceServerSigningAlgorithm.RS256, SigningAlgorithm.RS256)]
        [DataRow(V1ResourceServerSigningAlgorithm.PS256, SigningAlgorithm.PS256)]
        public void ToApi_SigningAlgorithm_MapsCorrectly(V1ResourceServerSigningAlgorithm input, SigningAlgorithm expected)
        {
            Assert.AreEqual(expected, V1ResourceServerController.ToApi(input));
        }

        [TestMethod]
        [DataRow(V1ResourceServerTokenDialect.AccessToken, TokenDialect.AccessToken)]
        [DataRow(V1ResourceServerTokenDialect.AccessTokenAuthZ, TokenDialect.AccessTokenAuthZ)]
        [DataRow(V1ResourceServerTokenDialect.Rfc9068Profile, TokenDialect.Rfc9068Profile)]
        [DataRow(V1ResourceServerTokenDialect.Rfc9068ProfileAuthz, TokenDialect.Rfc9068ProfileAuthz)]
        public void ToApi_TokenDialect_MapsCorrectly(V1ResourceServerTokenDialect input, TokenDialect expected)
        {
            Assert.AreEqual(expected, V1ResourceServerController.ToApi(input));
        }

        [TestMethod]
        public void ToApi_ConsentPolicy_TransactionalAuthorizationWithMfa_MapsCorrectly()
        {
            Assert.AreEqual(ConsentPolicy.TransactionalAuthorizationWithMfa,
                V1ResourceServerController.ToApi(V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa));
        }

        [TestMethod]
        public void ToApi_TokenFormat_CompactNestedJwe_MapsCorrectly()
        {
            Assert.AreEqual(TokenFormat.CompactNestedJwe,
                V1ResourceServerController.ToApi(V1ResourceServerTokenFormat.CompactNestedJwe));
        }

        [TestMethod]
        public void ToApi_Mechanism_Mtls_MapsCorrectly()
        {
            Assert.AreEqual(Mechanism.Mtls,
                V1ResourceServerController.ToApi(V1ResourceServerMechanism.Mtls));
        }

        [TestMethod]
        public void ToApi_Scope_MapsProperties()
        {
            var result = V1ResourceServerController.ToApi(new V1ResourceServerScope { Value = "read:data", Description = "Read data" });

            Assert.AreEqual("read:data", result.Value);
            Assert.AreEqual("Read data", result.Description);
        }

        [TestMethod]
        public void ToApi_AuthorizationDetail_MapsType()
        {
            var result = V1ResourceServerController.ToApi(new V1ResourceServerAuthorizationDetail { Type = "payment_initiation" });

            Assert.AreEqual("payment_initiation", result.Type);
        }

        // ──────────────────────── Roundtrip tests ────────────────────────

        [TestMethod]
        [DataRow(V1ResourceServerSigningAlgorithm.HS256)]
        [DataRow(V1ResourceServerSigningAlgorithm.RS256)]
        [DataRow(V1ResourceServerSigningAlgorithm.PS256)]
        public void SigningAlgorithm_Roundtrip(V1ResourceServerSigningAlgorithm input)
        {
            var api = V1ResourceServerController.ToApi(input);
            Assert.AreEqual(input, V1ResourceServerController.FromApi((SigningAlgorithm?)api));
        }

        [TestMethod]
        [DataRow(V1ResourceServerTokenDialect.AccessToken)]
        [DataRow(V1ResourceServerTokenDialect.AccessTokenAuthZ)]
        [DataRow(V1ResourceServerTokenDialect.Rfc9068Profile)]
        [DataRow(V1ResourceServerTokenDialect.Rfc9068ProfileAuthz)]
        public void TokenDialect_Roundtrip(V1ResourceServerTokenDialect input)
        {
            var api = V1ResourceServerController.ToApi(input);
            Assert.AreEqual(input, V1ResourceServerController.FromApi((TokenDialect?)api));
        }

        // ──────────────────────── ApplyToApi tests ────────────────────────

        [TestMethod]
        public void ApplyToApi_CreateRequest_MapsAllFields()
        {
            var conf = new V1ResourceServerConf
            {
                Identifier = "https://api.example.com",
                Name = "My API",
                SigningAlgorithm = V1ResourceServerSigningAlgorithm.RS256,
                SigningSecret = "secret",
                TokenLifetime = 86400,
                TokenLifetimeForWeb = 7200,
                AllowOfflineAccess = true,
                SkipConsentForVerifiableFirstPartyClients = false,
                VerificationLocation = "https://verify.example.com",
                TokenDialect = V1ResourceServerTokenDialect.AccessToken,
                EnforcePolicies = true,
                ConsentPolicy = V1ResourceServerConsentPolicy.TransactionalAuthorizationWithMfa,
                Scopes = [new V1ResourceServerScope { Value = "read:data", Description = "Read" }],
                AuthorizationDetails = [new V1ResourceServerAuthorizationDetail { Type = "payment_initiation" }],
            };

            var req = new ResourceServerCreateRequest();
            V1ResourceServerController.ApplyToApi(conf, req);

            Assert.AreEqual("https://api.example.com", req.Identifier);
            Assert.AreEqual("My API", req.Name);
            Assert.AreEqual(SigningAlgorithm.RS256, req.SigningAlgorithm);
            Assert.AreEqual("secret", req.SigningSecret);
            Assert.AreEqual(86400, req.TokenLifetime);
            Assert.AreEqual(7200, req.TokenLifetimeForWeb);
            Assert.IsTrue(req.AllowOfflineAccess);
            Assert.IsFalse(req.SkipConsentForVerifiableFirstPartyClients);
            Assert.AreEqual("https://verify.example.com", req.VerificationLocation);
            Assert.AreEqual(TokenDialect.AccessToken, req.TokenDialect);
            Assert.IsTrue(req.EnforcePolicies);
            Assert.AreEqual(ConsentPolicy.TransactionalAuthorizationWithMfa, req.ConsentPolicy);
            Assert.AreEqual(1, req.Scopes!.Count);
            Assert.AreEqual("read:data", req.Scopes[0].Value);
            Assert.AreEqual(1, req.AuthorizationDetails!.Count);
            Assert.AreEqual("payment_initiation", req.AuthorizationDetails[0].Type);
        }

        [TestMethod]
        public void ApplyToApi_UpdateRequest_DoesNotSetIdentifier()
        {
            var conf = new V1ResourceServerConf { Identifier = "https://api.example.com", Name = "My API" };

            var req = new ResourceServerUpdateRequest();
            V1ResourceServerController.ApplyToApi(conf, req);

            Assert.AreEqual("My API", req.Name);
        }

        [TestMethod]
        public void ApplyToApi_TokenEncryption_MapsCorrectly()
        {
            var conf = new V1ResourceServerConf
            {
                TokenEncryption = new V1ResourceServerTokenEncryption
                {
                    Format = V1ResourceServerTokenFormat.CompactNestedJwe,
                    EncryptionKey = new V1ResourceServerTokenEncryptionKey
                    {
                        Name = "mykey",
                        Algorithm = "RSA-OAEP",
                        Kid = "kid-1",
                        Pem = "pem-data",
                    },
                },
            };

            var req = new ResourceServerCreateRequest();
            V1ResourceServerController.ApplyToApi(conf, req);

            Assert.AreEqual(TokenFormat.CompactNestedJwe, req.TokenEncryption!.Format);
            Assert.AreEqual("mykey", req.TokenEncryption.EncryptionKey!.Name);
            Assert.AreEqual("RSA-OAEP", req.TokenEncryption.EncryptionKey.Algorithm);
            Assert.AreEqual("kid-1", req.TokenEncryption.EncryptionKey.Kid);
            Assert.AreEqual("pem-data", req.TokenEncryption.EncryptionKey.Pem);
        }

        [TestMethod]
        public void ApplyToApi_ProofOfPossession_MapsCorrectly()
        {
            var conf = new V1ResourceServerConf
            {
                ProofOfPossession = new V1ResourceServerProofOfPossession
                {
                    Required = true,
                    Mechanism = V1ResourceServerMechanism.Mtls,
                },
            };

            var req = new ResourceServerCreateRequest();
            V1ResourceServerController.ApplyToApi(conf, req);

            Assert.IsTrue(req.ProofOfPossession!.Required);
            Assert.AreEqual(Mechanism.Mtls, req.ProofOfPossession.Mechanism);
        }

        [TestMethod]
        public void ApplyToApi_NullOptionalFields_DoesNotOverwrite()
        {
            var conf = new V1ResourceServerConf { Name = "My API" };

            var req = new ResourceServerCreateRequest();
            V1ResourceServerController.ApplyToApi(conf, req);

            Assert.IsNull(req.SigningAlgorithm);
            Assert.IsNull(req.TokenDialect);
            Assert.IsNull(req.ConsentPolicy);
            Assert.IsNull(req.TokenEncryption);
            Assert.IsNull(req.ProofOfPossession);
            Assert.IsNull(req.Scopes);
            Assert.IsNull(req.AuthorizationDetails);
        }

    }

}
