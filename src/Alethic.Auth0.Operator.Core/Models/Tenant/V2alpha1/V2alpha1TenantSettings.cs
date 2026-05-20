using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha1;

public record V2alpha1TenantSettings
{

    [JsonPropertyName("change_password")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantChangePassword? ChangePassword { get; set; }

    [JsonPropertyName("device_flow")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantDeviceFlow? DeviceFlow { get; set; }

    [JsonPropertyName("guardian_mfa_page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantGuardianMfaPage? GuardianMfaPage { get; set; }

    [JsonPropertyName("default_audience")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultAudience { get; set; }

    [JsonPropertyName("default_directory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultDirectory { get; set; }

    [JsonPropertyName("error_page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantErrorPage? ErrorPage { get; set; }

    [JsonPropertyName("default_token_quota")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantDefaultTokenQuota? DefaultTokenQuota { get; set; }

    [JsonPropertyName("flags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantFlags? Flags { get; set; }

    [JsonPropertyName("friendly_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FriendlyName { get; set; }

    [JsonPropertyName("picture_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PictureUrl { get; set; }

    [JsonPropertyName("support_email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SupportEmail { get; set; }

    [JsonPropertyName("support_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SupportUrl { get; set; }

    [JsonPropertyName("allowed_logout_urls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AllowedLogoutUrls { get; set; }

    [JsonPropertyName("session_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SessionLifetime { get; set; }

    [JsonPropertyName("idle_session_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IdleSessionLifetime { get; set; }

    [JsonPropertyName("ephemeral_session_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? EphemeralSessionLifetime { get; set; }

    [JsonPropertyName("idle_ephemeral_session_lifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IdleEphemeralSessionLifetime { get; set; }

    [JsonPropertyName("sandbox_version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SandboxVersion { get; set; }

    [JsonPropertyName("legacy_sandbox_version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LegacySandboxVersion { get; set; }

    [JsonPropertyName("default_redirection_uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultRedirectionUri { get; set; }

    [JsonPropertyName("enabled_locales")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? EnabledLocales { get; set; }

    [JsonPropertyName("session_cookie")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantSessionCookie? SessionCookie { get; set; }

    [JsonPropertyName("sessions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantSessions? Sessions { get; set; }

    [JsonPropertyName("oidc_logout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantOidcLogout? OidcLogout { get; set; }

    [JsonPropertyName("customize_mfa_in_postlogin_action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CustomizeMfaInPostloginAction { get; set; }

    [JsonPropertyName("allow_organization_name_in_authentication_api")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowOrganizationNameInAuthenticationApi { get; set; }

    [JsonPropertyName("acr_values_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AcrValuesSupported { get; set; }

    [JsonPropertyName("mtls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantMtls? Mtls { get; set; }

    [JsonPropertyName("pushed_authorization_requests_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PushedAuthorizationRequestsSupported { get; set; }

    [JsonPropertyName("authorization_response_iss_parameter_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AuthorizationResponseIssParameterSupported { get; set; }

    [JsonPropertyName("skip_non_verifiable_callback_uri_confirmation_prompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SkipNonVerifiableCallbackUriConfirmationPrompt { get; set; }

    [JsonPropertyName("resource_parameter_profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantResourceParameterProfile? ResourceParameterProfile { get; set; }

    [JsonPropertyName("client_id_metadata_document_supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClientIdMetadataDocumentSupported { get; set; }

    [JsonPropertyName("enable_ai_guide")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableAiGuide { get; set; }

    [JsonPropertyName("phone_consolidated_experience")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PhoneConsolidatedExperience { get; set; }

    [JsonPropertyName("dynamic_client_registration_security_mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1TenantDynamicClientRegistrationSecurityMode? DynamicClientRegistrationSecurityMode { get; set; }

}
