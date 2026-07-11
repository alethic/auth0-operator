using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsAuth0
{

    [JsonPropertyName("attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionAttributes? Attributes { get; set; }

    [JsonPropertyName("authenticationMethods")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionAuthenticationMethods? AuthenticationMethods { get; set; }

    [JsonPropertyName("bruteForceProtection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BruteForceProtection { get; set; }

    [JsonPropertyName("configuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Configuration { get; set; }

    [JsonPropertyName("customScripts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionCustomScripts? CustomScripts { get; set; }

    [JsonPropertyName("disableSelfServiceChangePassword")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableSelfServiceChangePassword { get; set; }

    [JsonPropertyName("disableSignup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableSignup { get; set; }

    [JsonPropertyName("enableScriptContext")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableScriptContext { get; set; }

    [JsonPropertyName("enabledDatabaseCustomization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnabledDatabaseCustomization { get; set; }

    [JsonPropertyName("importMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ImportMode { get; set; }

    [JsonPropertyName("mfa")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionMfa? Mfa { get; set; }

    [JsonPropertyName("passkeyOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasskeyOptions? PasskeyOptions { get; set; }

    [JsonPropertyName("passwordPolicy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordPolicyEnum? PasswordPolicy { get; set; }

    [JsonPropertyName("passwordComplexityOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordComplexityOptions? PasswordComplexityOptions { get; set; }

    [JsonPropertyName("passwordDictionary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordDictionaryOptions? PasswordDictionary { get; set; }

    [JsonPropertyName("passwordHistory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordHistoryOptions? PasswordHistory { get; set; }

    [JsonPropertyName("passwordNoPersonalInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordNoPersonalInfoOptions? PasswordNoPersonalInfo { get; set; }

    [JsonPropertyName("passwordOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionPasswordOptions? PasswordOptions { get; set; }

    [JsonPropertyName("precedence")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionIdentifierPrecedenceEnum[]? Precedence { get; set; }

    [JsonPropertyName("realmFallback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RealmFallback { get; set; }

    [JsonPropertyName("requiresUsername")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequiresUsername { get; set; }

    [JsonPropertyName("validation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionValidationOptions? Validation { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
