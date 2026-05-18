using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>auth0</c> database connection strategy.
    /// </summary>
    public record V2alpha1ConnectionAuth0Options
    {

        [JsonPropertyName("attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConnectionAttributes? Attributes { get; set; }

        [JsonPropertyName("authentication_methods")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionAuthenticationMethods?> AuthenticationMethods { get; set; }

        [JsonPropertyName("brute_force_protection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BruteForceProtection { get; set; }

        [JsonPropertyName("configuration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string>? Configuration { get; set; }

        [JsonPropertyName("customScripts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConnectionCustomScripts? CustomScripts { get; set; }

        [JsonPropertyName("disable_self_service_change_password")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSelfServiceChangePassword { get; set; }

        [JsonPropertyName("disable_signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSignup { get; set; }

        [JsonPropertyName("enable_script_context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableScriptContext { get; set; }

        [JsonPropertyName("enabledDatabaseCustomization")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnabledDatabaseCustomization { get; set; }

        [JsonPropertyName("import_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ImportMode { get; set; }

        [JsonPropertyName("mfa")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConnectionMfa? Mfa { get; set; }

        [JsonPropertyName("passkey_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionPasskeyOptions?> PasskeyOptions { get; set; }

        [JsonPropertyName("passwordPolicy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionPasswordPolicyEnum?> PasswordPolicy { get; set; }

        [JsonPropertyName("password_complexity_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionPasswordComplexityOptions?> PasswordComplexityOptions { get; set; }

        [JsonPropertyName("password_dictionary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionPasswordDictionaryOptions?> PasswordDictionary { get; set; }

        [JsonPropertyName("password_history")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionPasswordHistoryOptions?> PasswordHistory { get; set; }

        [JsonPropertyName("password_no_personal_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Optional<ConnectionPasswordNoPersonalInfoOptions?> PasswordNoPersonalInfo { get; set; }

        [JsonPropertyName("password_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConnectionPasswordOptions? PasswordOptions { get; set; }

        [JsonPropertyName("precedence")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionIdentifierPrecedence[]? Precedence { get; set; }

        [JsonPropertyName("realm_fallback")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RealmFallback { get; set; }

        [JsonPropertyName("requires_username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequiresUsername { get; set; }

        [JsonPropertyName("validation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionValidationOptions? Validation { get; set; }

        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? NonPersistentAttrs { get; set; }

    }

}
