using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V2alpha3;

public record V2alpha3TenantSettings
{

    [JsonPropertyName("changePassword")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantChangePassword? ChangePassword { get; set; }

    [JsonPropertyName("deviceFlow")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantDeviceFlow? DeviceFlow { get; set; }

    [JsonPropertyName("guardianMfaPage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantGuardianMfaPage? GuardianMfaPage { get; set; }

    [JsonPropertyName("defaultAudience")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultAudience { get; set; }

    [JsonPropertyName("defaultDirectory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultDirectory { get; set; }

    [JsonPropertyName("errorPage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantErrorPage? ErrorPage { get; set; }

    [JsonPropertyName("defaultTokenQuota")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantDefaultTokenQuota? DefaultTokenQuota { get; set; }

    [JsonPropertyName("flags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantFlags? Flags { get; set; }

    [JsonPropertyName("friendlyName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FriendlyName { get; set; }

    [JsonPropertyName("pictureUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PictureUrl { get; set; }

    [JsonPropertyName("supportEmail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SupportEmail { get; set; }

    [JsonPropertyName("supportUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SupportUrl { get; set; }

    [JsonPropertyName("allowedLogoutUrls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AllowedLogoutUrls { get; set; }

    [JsonPropertyName("sessionLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SessionLifetime { get; set; }

    [JsonPropertyName("idleSessionLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IdleSessionLifetime { get; set; }

    [JsonPropertyName("ephemeralSessionLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? EphemeralSessionLifetime { get; set; }

    [JsonPropertyName("idleEphemeralSessionLifetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IdleEphemeralSessionLifetime { get; set; }

    [JsonPropertyName("sandboxVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SandboxVersion { get; set; }

    [JsonPropertyName("legacySandboxVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LegacySandboxVersion { get; set; }

    [JsonPropertyName("defaultRedirectionUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultRedirectionUri { get; set; }

    [JsonPropertyName("enabledLocales")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? EnabledLocales { get; set; }

    [JsonPropertyName("sessionCookie")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantSessionCookie? SessionCookie { get; set; }

    [JsonPropertyName("sessions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantSessions? Sessions { get; set; }

    [JsonPropertyName("oidcLogout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantOidcLogout? OidcLogout { get; set; }

    [JsonPropertyName("customizeMfaInPostloginAction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CustomizeMfaInPostloginAction { get; set; }

    [JsonPropertyName("allowOrganizationNameInAuthenticationApi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowOrganizationNameInAuthenticationApi { get; set; }

    [JsonPropertyName("acrValuesSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AcrValuesSupported { get; set; }

    [JsonPropertyName("mtls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantMtls? Mtls { get; set; }

    [JsonPropertyName("pushedAuthorizationRequestsSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PushedAuthorizationRequestsSupported { get; set; }

    [JsonPropertyName("authorizationResponseIssParameterSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AuthorizationResponseIssParameterSupported { get; set; }

    [JsonPropertyName("skipNonVerifiableCallbackUriConfirmationPrompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SkipNonVerifiableCallbackUriConfirmationPrompt { get; set; }

    [JsonPropertyName("resourceParameterProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantResourceParameterProfile? ResourceParameterProfile { get; set; }

    [JsonPropertyName("clientIdMetadataDocumentSupported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClientIdMetadataDocumentSupported { get; set; }

    [JsonPropertyName("enableAiGuide")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableAiGuide { get; set; }

    [JsonPropertyName("phoneConsolidatedExperience")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PhoneConsolidatedExperience { get; set; }

    [JsonPropertyName("dynamicClientRegistrationSecurityMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantDynamicClientRegistrationSecurityMode? DynamicClientRegistrationSecurityMode { get; set; }

    [JsonPropertyName("countryCodes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantCountryCodes? CountryCodes { get; set; }

    [JsonPropertyName("includeSessionMetadataInTenantLogs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IncludeSessionMetadataInTenantLogs { get; set; }

    [JsonPropertyName("securityHeaders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3TenantSecurityHeaders? SecurityHeaders { get; set; }

}
