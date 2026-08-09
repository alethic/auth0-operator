using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Tests for the explicit drift comparers on <see cref="V2alpha3TenantController"/> that decide whether a
    /// reconcile cycle pushes settings, branding, or prompts updates to Auth0.
    /// </summary>
    [TestClass]
    public class V2alpha3TenantNeedsUpdateTests
    {

        static V2alpha3TenantSettings MakeSettings() => new()
        {
            FriendlyName = "My Tenant",
            DefaultAudience = "https://api.example.com",
            AllowedLogoutUrls = ["https://example.com/logout"],
            EnabledLocales = ["en", "de"],
            SessionLifetime = 720,
            IdleSessionLifetime = 72,
            Flags = new V2alpha3TenantFlags
            {
                EnableClientConnections = true,
                RevokeRefreshTokenGrant = false,
                EnableSso = true,
            },
            SessionCookie = new V2alpha3TenantSessionCookie
            {
                Mode = V2alpha3TenantSessionCookieModeEnum.Persistent,
            },
            Sessions = new V2alpha3TenantSessions
            {
                OidcLogoutPromptEnabled = false,
            },
            ChangePassword = new V2alpha3TenantChangePassword
            {
                Enabled = true,
                Html = "<html></html>",
            },
            DefaultTokenQuota = new V2alpha3TenantDefaultTokenQuota
            {
                Clients = new V2alpha3TenantTokenQuotaConfiguration
                {
                    ClientCredentials = new V2alpha3TenantTokenQuotaClientCredentials
                    {
                        Enforce = true,
                        PerDay = 100,
                        PerHour = 10,
                    },
                },
            },
        };

        [TestMethod]
        public void Settings_Identical_DoesNotRequireUpdate()
        {
            Assert.IsFalse(V2alpha3TenantController.SettingsRequireUpdate(MakeSettings(), MakeSettings()));
        }

        [TestMethod]
        public void Settings_Auth0SideExtras_DoNotRequireUpdate()
        {
            var conf = new V2alpha3TenantSettings
            {
                FriendlyName = "My Tenant",
                Flags = new V2alpha3TenantFlags { EnableClientConnections = true },
            };

            var last = MakeSettings();
            last.PictureUrl = "https://example.com/logo.png";
            last.SupportEmail = "support@example.com";
            last.SandboxVersion = "18";
            last.Flags!.EnablePipeline2 = true;
            last.Mtls = new V2alpha3TenantMtls { EnableEndpointAliases = false };

            Assert.IsFalse(V2alpha3TenantController.SettingsRequireUpdate(conf, last));
        }

        [TestMethod]
        public void Settings_FriendlyNameChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.FriendlyName = "Renamed Tenant";

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_FlagsLeafChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.Flags!.RevokeRefreshTokenGrant = true;

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_EnableSsoChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.Flags!.EnableSso = false;

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_SessionCookieModeChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.SessionCookie!.Mode = V2alpha3TenantSessionCookieModeEnum.NonPersistent;

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_SessionsLeafChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.Sessions!.OidcLogoutPromptEnabled = true;

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_TokenQuotaLeafChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.DefaultTokenQuota!.Clients!.ClientCredentials!.PerDay = 200;

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_AllowedLogoutUrlsChange_RequiresUpdate()
        {
            var conf = MakeSettings();
            conf.AllowedLogoutUrls = ["https://example.com/logout", "https://other.example.com/logout"];

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, MakeSettings()));
        }

        [TestMethod]
        public void Settings_NestedConfWithNullLast_RequiresUpdate()
        {
            var conf = MakeSettings();
            var last = MakeSettings();
            last.SessionCookie = null;

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, last));
        }

        static V2alpha3TenantBranding MakeBranding() => new()
        {
            LogoUrl = "https://example.com/logo.png",
            FaviconUrl = "https://example.com/favicon.ico",
            Colors = new V2alpha3TenantBrandingColors
            {
                Primary = "#123456",
                PageBackground = "#ffffff",
            },
        };

        [TestMethod]
        public void Branding_Identical_DoesNotRequireUpdate()
        {
            Assert.IsFalse(V2alpha3TenantController.BrandingRequiresUpdate(MakeBranding(), MakeBranding()));
        }

        [TestMethod]
        public void Branding_ColorChange_RequiresUpdate()
        {
            var conf = MakeBranding();
            conf.Colors!.Primary = "#654321";

            Assert.IsTrue(V2alpha3TenantController.BrandingRequiresUpdate(conf, MakeBranding()));
        }

        [TestMethod]
        public void Branding_SparseConf_IgnoresUnmanagedProperties()
        {
            var conf = new V2alpha3TenantBranding { LogoUrl = "https://example.com/logo.png" };

            Assert.IsFalse(V2alpha3TenantController.BrandingRequiresUpdate(conf, MakeBranding()));
        }

        static V2alpha3TenantPrompts MakePrompts() => new()
        {
            IdentifierFirst = true,
            UniversalLoginExperience = V2alpha3TenantUniversalLoginExperience.New,
            WebauthnPlatformFirstFactor = false,
        };

        [TestMethod]
        public void Prompts_Identical_DoesNotRequireUpdate()
        {
            Assert.IsFalse(V2alpha3TenantController.PromptsRequireUpdate(MakePrompts(), MakePrompts()));
        }

        [TestMethod]
        public void Prompts_ExperienceChange_RequiresUpdate()
        {
            var conf = MakePrompts();
            conf.UniversalLoginExperience = V2alpha3TenantUniversalLoginExperience.Classic;

            Assert.IsTrue(V2alpha3TenantController.PromptsRequireUpdate(conf, MakePrompts()));
        }

        [TestMethod]
        public void Prompts_IdentifierFirstChange_RequiresUpdate()
        {
            var conf = MakePrompts();
            conf.IdentifierFirst = false;

            Assert.IsTrue(V2alpha3TenantController.PromptsRequireUpdate(conf, MakePrompts()));
        }

        [TestMethod]
        public void Flags_DisableImpersonationChange_RequiresUpdate()
        {
            var conf = new V2alpha3TenantSettings { Flags = new V2alpha3TenantFlags { DisableImpersonation = true } };
            var last = new V2alpha3TenantSettings { Flags = new V2alpha3TenantFlags { DisableImpersonation = false } };

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, last));
        }

        [TestMethod]
        public void Flags_AllowChangingEnableSsoSkew_NoUpdate()
        {
            // allowChangingEnableSso is a read-only indicator; a skew there can never be corrected by updating
            var conf = new V2alpha3TenantSettings { Flags = new V2alpha3TenantFlags { AllowChangingEnableSso = true } };
            var last = new V2alpha3TenantSettings { Flags = new V2alpha3TenantFlags { AllowChangingEnableSso = false } };

            Assert.IsFalse(V2alpha3TenantController.SettingsRequireUpdate(conf, last));
        }

        [TestMethod]
        public void Settings_AllowedLogoutUrlsReordered_NoUpdate()
        {
            // logout URLs are an allowlist; ordering is not drift
            var conf = new V2alpha3TenantSettings { AllowedLogoutUrls = ["https://a", "https://b"] };
            var last = new V2alpha3TenantSettings { AllowedLogoutUrls = ["https://b", "https://a"] };

            Assert.IsFalse(V2alpha3TenantController.SettingsRequireUpdate(conf, last));
        }

        [TestMethod]
        public void Settings_EnabledLocalesReordered_RequiresUpdate()
        {
            // the first enabled locale acts as the default, so ordering is semantic
            var conf = new V2alpha3TenantSettings { EnabledLocales = ["en", "fr"] };
            var last = new V2alpha3TenantSettings { EnabledLocales = ["fr", "en"] };

            Assert.IsTrue(V2alpha3TenantController.SettingsRequireUpdate(conf, last));
        }

        [TestMethod]
        public void Branding_FontChange_RequiresUpdate()
        {
            var conf = new V2alpha3TenantBranding { Font = new V2alpha3TenantBrandingFont { Url = "https://fonts/new.woff2" } };
            var last = new V2alpha3TenantBranding { Font = new V2alpha3TenantBrandingFont { Url = "https://fonts/old.woff2" } };

            Assert.IsTrue(V2alpha3TenantController.BrandingRequiresUpdate(conf, last));
        }

        [TestMethod]
        public void Branding_FontIdentical_NoUpdate()
        {
            var conf = new V2alpha3TenantBranding { Font = new V2alpha3TenantBrandingFont { Url = "https://fonts/a.woff2" } };
            var last = new V2alpha3TenantBranding { Font = new V2alpha3TenantBrandingFont { Url = "https://fonts/a.woff2" } };

            Assert.IsFalse(V2alpha3TenantController.BrandingRequiresUpdate(conf, last));
        }

    }

}
