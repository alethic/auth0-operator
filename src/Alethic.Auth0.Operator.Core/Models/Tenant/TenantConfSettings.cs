using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{
    public class TenantConfSettings
    {

        [JsonPropertyName("friendly_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FriendlyName { get; set; }

        [JsonPropertyName("support_email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SupportEmail { get; set; }

        [JsonPropertyName("support_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SupportUrl { get; set; }

        [JsonPropertyName("enabled_locales")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? EnabledLocales { get; set; }

        [JsonPropertyName("change_password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantChangePassword? ChangePassword { get; set; }

        [JsonPropertyName("device_flow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantDeviceFlow? DeviceFlow { get; set; }

        [JsonPropertyName("flags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantFlags? Flags { get; set; }

        [JsonPropertyName("guardian_mfa_page")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantGuardianMfaPage? GuardianMfaPage { get; set; }

        [JsonPropertyName("default_audience")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DefaultAudience { get; set; }

        [JsonPropertyName("default_directory")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DefaultDirectory { get; set; }

        [JsonPropertyName("error_page")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantErrorPage? ErrorPage { get; set; }

        [JsonPropertyName("picture_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PictureUrl { get; set; }

        [JsonPropertyName("allowed_logout_urls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AllowedLogoutUrls { get; set; }

        [JsonPropertyName("session_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? SessionLifetime { get; set; }

        [JsonPropertyName("idle_session_lifetime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? IdleSessionLifetime { get; set; }

        [JsonPropertyName("sandbox_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SandboxVersion { get; set; }

        [JsonPropertyName("sandbox_versions_available")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? SandboxVersionsAvailable { get; set; }

        [JsonPropertyName("session_cookie")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SessionCookie? SessionCookie { get; set; }

        [JsonPropertyName("customize_mfa_in_postlogin_action")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CustomizeMfaInPostLoginAction { get; set; }

        [JsonPropertyName("acr_values_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? AcrValuesSupported { get; set; }

        [JsonPropertyName("pushed_authorization_requests_supported")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PushedAuthorizationRequestsSupported { get; set; }

        [JsonPropertyName("mtls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantMtls? Mtls { get; set; }

    }

}