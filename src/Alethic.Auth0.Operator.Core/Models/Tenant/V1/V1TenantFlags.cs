using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant.V1
{

    public record V1TenantFlags
    {

        [JsonPropertyName("allow_legacy_ro_grant_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowLegacyRoGrantTypes { get; set; }

        [JsonPropertyName("allow_legacy_delegation_grant_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowLegacyDelegationGrantTypes { get; set; }

        [JsonPropertyName("allow_legacy_tokeninfo_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowLegacyTokeninfoEndpoint { get; set; }

        [JsonPropertyName("change_pwd_flow_v1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ChangePwdFlowV1 { get; set; }

        [JsonPropertyName("disable_clickjack_protection_headers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableClickjackProtectionHeaders { get; set; }

        [JsonPropertyName("disable_management_api_sms_obfuscation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableManagementApiSmsObfuscation { get; set; }

        [JsonPropertyName("enable_adfs_waad_email_verification")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableAdfsWaadEmailVerification { get; set; }

        [JsonPropertyName("enable_apis_section")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableAPIsSection { get; set; }

        [JsonPropertyName("enable_client_connections")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableClientConnections { get; set; }

        [JsonPropertyName("enable_custom_domain_in_emails")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableCustomDomainInEmails { get; set; }

        [JsonPropertyName("enable_dynamic_client_registration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableDynamicClientRegistration { get; set; }

        [JsonPropertyName("enable_idtoken_api2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableIdTokenApi2 { get; set; }

        [JsonPropertyName("enable_legacy_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableLegacyProfile { get; set; }

        [JsonPropertyName("enable_pipeline2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnablePipeline2 { get; set; }

        [JsonPropertyName("enable_public_signup_user_exists_error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnablePublicSignupUserExistsError { get; set; }

        [JsonPropertyName("enable_sso")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableSSO { get; set; }

        [JsonPropertyName("enforce_client_authentication_on_passwordless_start")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnforceClientAuthenticationOnPasswordlessStart { get; set; }

        [JsonPropertyName("no_disclose_enterprise_connections")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NoDiscloseEnterpriseConnections { get; set; }

        [JsonPropertyName("remove_alg_from_jwks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RemoveAlgFromJwks { get; set; }

        [JsonPropertyName("require_pushed_authorization_requests")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequirePushedAuthorizationRequests { get; set; }

        [JsonPropertyName("revoke_refresh_token_grant")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RevokeRefreshTokenGrant { get; set; }

        [JsonPropertyName("trust_azure_adfs_email_verified_connection_property")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? TrustAzureAdfsEmailVerifiedConnectionProperty { get; set; }

        [JsonPropertyName("dashboard_log_streams_next")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DashboardLogStreamsNext { get; set; }

        [JsonPropertyName("dashboard_insights_view")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DashboardInsightsView { get; set; }

        [JsonPropertyName("disable_fields_map_fix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableFieldsMapFix { get; set; }

        [JsonPropertyName("mfa_show_factor_list_on_enrollment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? MfaShowFactorListOnEnrollment { get; set; }

        [JsonPropertyName("improved_signup_bot_detection_in_classic")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ImprovedSignupBotDetectionInClassic { get; set; }

        [JsonPropertyName("genai_trial")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? GenaiTrial { get; set; }

        [JsonPropertyName("custom_domains_provisioning")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CustomDomainsProvisioning { get; set; }

    }

}
