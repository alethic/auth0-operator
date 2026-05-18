using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>sms</c> (passwordless SMS) connection strategy.
    /// </summary>
    public record V2alpha1ConnectionSmsOptions
    {

        /// <summary>
        /// Friendly name for the SMS connection.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// Phone number to use as the sender of SMS messages.
        /// </summary>
        [JsonPropertyName("from")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? From { get; set; }

        /// <summary>
        /// Body template for SMS messages. Supports Liquid syntax.
        /// </summary>
        [JsonPropertyName("template")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Template { get; set; }

        /// <summary>
        /// Template syntax engine. Can be <c>liquid</c> or <c>markdown</c>.
        /// </summary>
        [JsonPropertyName("syntax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Syntax { get; set; }

        /// <summary>
        /// SMS provider to use. Can be <c>twilio</c>, <c>phone-message-hook</c>, or a custom gateway.
        /// </summary>
        [JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Provider { get; set; }

        /// <summary>
        /// Twilio account SID when using the <c>twilio</c> provider.
        /// </summary>
        [JsonPropertyName("twilio_sid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TwilioSid { get; set; }

        /// <summary>
        /// Twilio authentication token when using the <c>twilio</c> provider.
        /// </summary>
        [JsonPropertyName("twilio_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TwilioToken { get; set; }

        /// <summary>
        /// Twilio Messaging Service SID when using the <c>twilio</c> provider.
        /// </summary>
        [JsonPropertyName("messaging_service_sid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MessagingServiceSid { get; set; }

        /// <summary>
        /// URL of the custom SMS gateway when not using Twilio.
        /// </summary>
        [JsonPropertyName("gateway_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? GatewayUrl { get; set; }

        /// <summary>
        /// Authentication settings for the custom SMS gateway.
        /// </summary>
        [JsonPropertyName("gateway_authentication")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionGatewayAuthentication? GatewayAuthentication { get; set; }

        /// <summary>
        /// When <c>true</c>, request info (IP, user-agent) is forwarded to the custom SMS gateway.
        /// </summary>
        [JsonPropertyName("forward_req_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ForwardReqInfo { get; set; }

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
