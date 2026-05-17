using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Email message template configuration (subject, body, from address, and syntax).
    /// </summary>
    public record V1ConnectionEmailMessage
    {

        /// <summary>
        /// The sender address for the magic-link email.
        /// </summary>
        [JsonPropertyName("from")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? From { get; set; }

        /// <summary>
        /// Subject line of the magic-link email.
        /// </summary>
        [JsonPropertyName("subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Subject { get; set; }

        /// <summary>
        /// Body of the magic-link email (may contain Liquid template syntax).
        /// </summary>
        [JsonPropertyName("body")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Body { get; set; }

        /// <summary>
        /// Template syntax used in the body. Use <c>"liquid"</c> for Liquid templates.
        /// </summary>
        [JsonPropertyName("syntax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Syntax { get; set; }

    }

    /// <summary>
    /// TOTP (time-based one-time password) configuration for the email connection.
    /// </summary>
    public record V1ConnectionEmailTotp
    {

        /// <summary>
        /// Length of the one-time password.
        /// </summary>
        [JsonPropertyName("length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Length { get; set; }

        /// <summary>
        /// Time step in seconds for TOTP code generation.
        /// </summary>
        [JsonPropertyName("time_step")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TimeStep { get; set; }

    }

    /// <summary>
    /// Configuration options for the <c>email</c> (passwordless email) connection strategy.
    /// </summary>
    public record V1ConnectionEmailOptions
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
        public V1ConnectionEmailMessage? Email { get; set; }

        /// <summary>
        /// TOTP (time-based one-time password) configuration for the connection.
        /// </summary>
        [JsonPropertyName("totp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionEmailTotp? Totp { get; set; }

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
