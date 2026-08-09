using System.Collections;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    [System.Runtime.Versioning.RequiresPreviewFeatures]
    public class V2alpha3ConnectionNeedsUpdateTests
    {

        [TestMethod]
        public void ConfRequiresUpdate_Identical_ReturnsFalse()
        {
            var conf = new V2alpha3ConnectionConf
            {
                DisplayName = "Display",
                Realms = ["realm-a", "realm-b"],
                IsDomainConnection = true,
                ShowAsButton = false,
                Metadata = new Hashtable { ["team"] = "core" },
            };
            var last = new V2alpha3ConnectionConf
            {
                DisplayName = "Display",
                Realms = ["realm-a", "realm-b"],
                IsDomainConnection = true,
                ShowAsButton = false,
                Metadata = new Hashtable { ["team"] = "core" },
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_Auth0SideExtraValues_ReturnsFalse()
        {
            // Auth0 fills in server-side values the spec does not manage; they must never force a PATCH
            var conf = new V2alpha3ConnectionConf
            {
                DisplayName = "Display",
            };
            var last = new V2alpha3ConnectionConf
            {
                DisplayName = "Display",
                Realms = ["server-realm"],
                IsDomainConnection = false,
                ShowAsButton = true,
                Metadata = new Hashtable { ["server"] = "value" },
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true, Profile = true },
                },
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
        public void ConfRequiresUpdate_ChangedMetadataValue_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { Metadata = new Hashtable { ["team"] = "core" } };
            var last = new V2alpha3ConnectionConf { Metadata = new Hashtable { ["team"] = "other" } };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_MetadataKeyMissingFromLast_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { Metadata = new Hashtable { ["team"] = "core" } };
            var last = new V2alpha3ConnectionConf { Metadata = new Hashtable { ["other"] = "core" } };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_MetadataExtraKeyOnLastOnly_ReturnsFalse()
        {
            var conf = new V2alpha3ConnectionConf { Metadata = new Hashtable { ["team"] = "core" } };
            var last = new V2alpha3ConnectionConf { Metadata = new Hashtable { ["team"] = "core", ["server"] = "value" } };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ChangedRealms_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { Realms = ["realm-a", "realm-b"] };
            var last = new V2alpha3ConnectionConf { Realms = ["realm-a"] };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ReorderedRealms_ReturnsFalse()
        {
            // realms are a set; Auth0 returning them in a different order is not drift
            var conf = new V2alpha3ConnectionConf { Realms = ["realm-a", "realm-b"] };
            var last = new V2alpha3ConnectionConf { Realms = ["realm-b", "realm-a"] };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ReorderedScope_ReturnsFalse()
        {
            // scopes are a set; Auth0 returning them in a different order is not drift
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Scope = ["email", "profile", "openid"] },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Scope = ["openid", "email", "profile"] },
                },
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ChangedIsDomainConnection_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { IsDomainConnection = true };
            var last = new V2alpha3ConnectionConf { IsDomainConnection = false };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_ChangedShowAsButton_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf { ShowAsButton = true };
            var last = new V2alpha3ConnectionConf { ShowAsButton = false };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_GoogleOAuth2LeafChanged_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = false, Profile = true },
                },
            };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_GoogleOAuth2LeafSubset_ReturnsFalse()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true, Profile = true },
                },
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_Auth0PasswordPolicyLeafChanged_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Auth0 = new V2alpha3ConnectionOptionsAuth0
                    {
                        PasswordPolicy = V2alpha3ConnectionPasswordPolicyEnum.Good,
                        PasswordHistory = new V2alpha3ConnectionPasswordHistoryOptions { Enable = true, Size = 5 },
                    },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Auth0 = new V2alpha3ConnectionOptionsAuth0
                    {
                        PasswordPolicy = V2alpha3ConnectionPasswordPolicyEnum.Good,
                        PasswordHistory = new V2alpha3ConnectionPasswordHistoryOptions { Enable = true, Size = 3 },
                    },
                },
            };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_Auth0PasswordPolicyLeafSubset_ReturnsFalse()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Auth0 = new V2alpha3ConnectionOptionsAuth0
                    {
                        PasswordPolicy = V2alpha3ConnectionPasswordPolicyEnum.Good,
                    },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Auth0 = new V2alpha3ConnectionOptionsAuth0
                    {
                        PasswordPolicy = V2alpha3ConnectionPasswordPolicyEnum.Good,
                        PasswordHistory = new V2alpha3ConnectionPasswordHistoryOptions { Enable = true, Size = 3 },
                        BruteForceProtection = true,
                    },
                },
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_SamlSignatureLeafChanged_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Saml = new V2alpha3ConnectionOptionsSaml
                    {
                        SignatureAlgorithm = V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha256,
                    },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Saml = new V2alpha3ConnectionOptionsSaml
                    {
                        SignatureAlgorithm = V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha1,
                    },
                },
            };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_SamlDecryptionKeyLeafChanged_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Saml = new V2alpha3ConnectionOptionsSaml
                    {
                        DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                        {
                            KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert { Cert = "cert-a", Key = "key-a" },
                        },
                    },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Saml = new V2alpha3ConnectionOptionsSaml
                    {
                        DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                        {
                            KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert { Cert = "cert-a", Key = "key-b" },
                        },
                    },
                },
            };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_SamlLeafSubset_ReturnsFalse()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Saml = new V2alpha3ConnectionOptionsSaml
                    {
                        SignatureAlgorithm = V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha256,
                        DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                        {
                            KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert { Cert = "cert-a" },
                        },
                    },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    Saml = new V2alpha3ConnectionOptionsSaml
                    {
                        SignatureAlgorithm = V2alpha3ConnectionSignatureAlgorithmEnumSaml.RsaSha256,
                        DigestAlgorithm = V2alpha3ConnectionDigestAlgorithmEnumSaml.Sha256,
                        SignSamlRequest = true,
                        DecryptionKey = new V2alpha3ConnectionDecryptionKeySaml
                        {
                            KeyPair = new V2alpha3ConnectionDecryptionKeySamlCert { Cert = "cert-a", Key = "server-key" },
                        },
                    },
                },
            };

            Assert.IsFalse(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_OptionsSetButLastOptionsNull_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2 { Email = true },
                },
            };
            var last = new V2alpha3ConnectionConf();

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void ConfRequiresUpdate_UpstreamParamValueChanged_ReturnsTrue()
        {
            var conf = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2
                    {
                        UpstreamParams = new System.Collections.Generic.Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>
                        {
                            ["login_hint"] = new V2alpha3ConnectionUpstreamAdditionalProperties { Value = "a" },
                        },
                    },
                },
            };
            var last = new V2alpha3ConnectionConf
            {
                Options = new V2alpha3ConnectionOptions
                {
                    GoogleOAuth2 = new V2alpha3ConnectionOptionsGoogleOAuth2
                    {
                        UpstreamParams = new System.Collections.Generic.Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>
                        {
                            ["login_hint"] = new V2alpha3ConnectionUpstreamAdditionalProperties { Value = "b" },
                        },
                    },
                },
            };

            Assert.IsTrue(V2alpha3ConnectionController.ConfRequiresUpdate(conf, last));
        }

    }

}
