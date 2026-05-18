using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>email</c> (passwordless email) connection strategy.
    /// </summary>
    public record V2alpha1ConnectionEmailOptions
    {

        /// <summary>
        /// Friendly name for the email connection.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// Email message configuration including subject, body, and syntax settings.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionEmailMessage? Email { get; set; }

        /// <summary>
        /// Authentication parameters appended to the magic link.
        /// </summary>
        [JsonPropertyName("authParams")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionEmailAuthParams? AuthParams { get; set; }

        /// <summary>
        /// TOTP (time-based one-time password) configuration for the connection.
        /// </summary>
        [JsonPropertyName("totp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionEmailTotp? Totp { get; set; }

        /// <summary>
        /// When <c>true</c>, new user sign-ups are disabled on this connection.
        /// </summary>
        [JsonPropertyName("disable_signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableSignup { get; set; }

        /// <summary>
        /// When <c>true</c>, Auth0 will lock user accounts temporarily after too many consecutive failed login attempts.
        /// </summary>
        [JsonPropertyName("brute_force_protection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BruteForceProtection { get; set; }

        /// <summary>
        /// List of user attributes that will not be persisted in the Auth0 user store after each login.
        /// </summary>
        [JsonPropertyName("non_persistent_attrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? NonPersistentAttrs { get; set; }

    }

}
