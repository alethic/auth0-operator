using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>auth0</c> database connection strategy.
    /// </summary>
    public record V2alpha1ConnectionAuth0Options
    {

        /// <summary>
        /// Password strength level required for new user passwords. Can be <c>none</c>, <c>low</c>, <c>fair</c>, <c>good</c>, or <c>excellent</c>.
        /// </summary>
        [JsonPropertyName("password_policy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PasswordPolicy { get; set; }

        /// <summary>
        /// Configuration for password history enforcement. Prevents reuse of recent passwords.
        /// </summary>
        [JsonPropertyName("password_history")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPasswordHistory? PasswordHistory { get; set; }

        /// <summary>
        /// Configuration for blocking passwords that contain personal information.
        /// </summary>
        [JsonPropertyName("password_no_personal_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPasswordNoPersonalInfo? PasswordNoPersonalInfo { get; set; }

        /// <summary>
        /// Configuration for blocking passwords from a dictionary of common passwords.
        /// </summary>
        [JsonPropertyName("password_dictionary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPasswordDictionary? PasswordDictionary { get; set; }

        /// <summary>
        /// Configuration for additional password complexity requirements (e.g. minimum length).
        /// </summary>
        [JsonPropertyName("password_complexity_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsPasswordComplexityOptions? PasswordComplexityOptions { get; set; }

        /// <summary>
        /// Validation rules applied to the username field (e.g. min/max length).
        /// </summary>
        [JsonPropertyName("validation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsValidation? Validation { get; set; }

        /// <summary>
        /// When <c>true</c>, the context of the current authentication transaction is passed to custom database action scripts.
        /// </summary>
        [JsonPropertyName("enable_script_context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableScriptContext { get; set; }

        /// <summary>
        /// When <c>true</c>, database action scripts are enabled for the connection.
        /// </summary>
        [JsonPropertyName("enabledDatabaseCustomization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnabledDatabaseCustomization { get; set; }

        /// <summary>
        /// Custom database action scripts (login, get_user, create, etc.) keyed by script name.
        /// </summary>
        [JsonPropertyName("customScripts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsCustomScripts? CustomScripts { get; set; }

        /// <summary>
        /// When <c>true</c>, the connection uses a lazy migration mode: users are imported from an external database on first login.
        /// </summary>
        [JsonPropertyName("import_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ImportMode { get; set; }

        /// <summary>
        /// When <c>true</c>, new user sign-ups are disabled on this connection.
        /// </summary>
        [JsonPropertyName("disable_signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSignup { get; set; }

        /// <summary>
        /// When <c>true</c>, users are required to provide a username in addition to their email address during registration.
        /// </summary>
        [JsonPropertyName("requires_username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequiresUsername { get; set; }

        /// <summary>
        /// When <c>true</c>, Auth0 will lock user accounts temporarily after too many consecutive failed login attempts.
        /// </summary>
        [JsonPropertyName("brute_force_protection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BruteForceProtection { get; set; }

        /// <summary>
        /// MFA configuration settings for the connection.
        /// </summary>
        [JsonPropertyName("mfa")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionOptionsMfa? Mfa { get; set; }

        /// <summary>
        /// List of user attributes that will not be persisted in the Auth0 user store after each login.
        /// </summary>
        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

        /// <summary>
        /// Controls when root profile attributes (<c>name</c>, <c>given_name</c>, etc.) are updated from the identity provider.
        /// </summary>
        [JsonPropertyName("set_user_root_attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionSetUserRootAttributes? SetUserRootAttributes { get; set; }

        /// <summary>
        /// Upstream parameters that will be sent to the identity provider on each authentication request.
        /// </summary>
        [JsonPropertyName("upstream_params")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, V2alpha1ConnectionUpstreamParam?>? UpstreamParams { get; set; }

    }

}
