using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using KubeOps.Abstractions.Entities.Attributes;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for a database connection.
    /// </summary>
    [PreserveUnknownFields]
    public record V1ConnectionOptions
    {

        /// <summary>
        /// Username validation rules for the connection.
        /// </summary>
        [JsonPropertyName("validation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsValidation? Validation { get; set; }

        /// <summary>
        /// List of user attributes that will not be persisted in the Auth0 user store after each login.
        /// </summary>
        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttributes { get; set; }

        /// <summary>
        /// Ordered list of identifier attributes used during login precedence resolution.
        /// </summary>
        [JsonPropertyName("precedence")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPrecedence[]? Precedence { get; set; }

        /// <summary>
        /// Configuration for the connection's user attribute schema.
        /// </summary>
        [JsonPropertyName("attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributes? Attributes { get; set; }

        /// <summary>
        /// When <c>true</c>, enables script context so custom scripts have access to the connection configuration.
        /// </summary>
        [JsonPropertyName("enable_script_context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableScriptContext { get; set; }

        /// <summary>
        /// When <c>true</c>, enables the use of custom database scripts for this connection.
        /// </summary>
        [JsonPropertyName("enabledDatabaseCustomization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableDatabaseCustomization { get; set; }

        /// <summary>
        /// When <c>true</c>, the connection operates in import mode; users are migrated from a custom database on first login.
        /// </summary>
        [JsonPropertyName("import_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ImportMode { get; set; }

        /// <summary>
        /// Custom script implementations for CRUD operations on the backing user store.
        /// </summary>
        [JsonPropertyName("customScripts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsCustomScripts? CustomScripts { get; set; }

        /// <summary>
        /// Controls which authentication methods (password and/or passkey) are enabled for this connection.
        /// </summary>
        [JsonPropertyName("authentication_methods")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAuthenticationMethods? AuthenticationMethods { get; set; }

        /// <summary>
        /// Passkey-specific options such as challenge UI style and enrollment settings.
        /// </summary>
        [JsonPropertyName("passkey_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasskeyOptions? PasskeyOptions { get; set; }

        /// <summary>
        /// Password strength policy enforced for new and updated passwords.
        /// </summary>
        [JsonPropertyName("passwordPolicy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordPolicy? PasswordPolicy { get; set; }

        /// <summary>
        /// Minimum-length requirement and other complexity rules for passwords.
        /// </summary>
        [JsonPropertyName("password_complexity_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordComplexityOptions? PasswordComplexityOptions { get; set; }

        /// <summary>
        /// Controls whether password-history checking is enabled and how many previous passwords to retain.
        /// </summary>
        [JsonPropertyName("password_history")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordHistory? PasswordHistory { get; set; }

        /// <summary>
        /// When enabled, prevents users from using personal information (name, email, etc.) as part of their password.
        /// </summary>
        [JsonPropertyName("password_no_personal_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordNoPersonalInfo? PasswordNoPersonalInfo { get; set; }

        /// <summary>
        /// Controls whether a common-password dictionary check is enabled, optionally extended with custom words.
        /// </summary>
        [JsonPropertyName("password_dictionary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPasswordDictionary? PasswordDictionary { get; set; }

        /// <summary>
        /// When <c>true</c>, enables user management via the Auth0 Management API for this connection.
        /// </summary>
        [JsonPropertyName("api_enable_users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ApiEnableUsers { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the user's basic profile information from the identity provider.
        /// </summary>
        [JsonPropertyName("basic_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BasicProfile { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the <c>isAdmin</c> extended attribute from the identity provider.
        /// </summary>
        [JsonPropertyName("ext_admin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAdmin { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the <c>isSuspended</c> extended attribute from the identity provider.
        /// </summary>
        [JsonPropertyName("ext_is_suspended")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtIsSuspended { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the <c>agreedTerms</c> extended attribute from the identity provider.
        /// </summary>
        [JsonPropertyName("ext_agreed_terms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAgreedTerms { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the <c>groups</c> extended attribute from the identity provider.
        /// </summary>
        [JsonPropertyName("ext_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtGroups { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the <c>assignedPlans</c> extended attribute from the identity provider.
        /// </summary>
        [JsonPropertyName("ext_assigned_plans")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtAssignedPlans { get; set; }

        /// <summary>
        /// When <c>true</c>, requests the full profile extended attribute from the identity provider.
        /// </summary>
        [JsonPropertyName("ext_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ExtProfile { get; set; }

        /// <summary>
        /// When <c>true</c>, hides the self-service change-password option from users.
        /// </summary>
        [JsonPropertyName("disable_self_service_change_password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSelfServiceChangePassword { get; set; }

        /// <summary>
        /// Upstream parameters that will be forwarded to the identity provider on each authentication request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("upstream_params")]
        public Dictionary<string, V1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

        /// <summary>
        /// Controls when root profile attributes (<c>name</c>, <c>given_name</c>, etc.) are updated from the identity provider.
        /// </summary>
        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        /// <summary>
        /// Gateway authentication configuration used when the connection routes through a self-hosted gateway.
        /// </summary>
        [JsonPropertyName("gateway_authentication")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionGatewayAuthentication? GatewayAuthentication { get; set; }

        /// <summary>
        /// Additional properties not captured by the defined schema, preserved for round-trip fidelity.
        /// </summary>
        [JsonExtensionData]
        [Ignore]
        public Dictionary<string, object?>? AdditionalProperties { get; set; }

    }

}
