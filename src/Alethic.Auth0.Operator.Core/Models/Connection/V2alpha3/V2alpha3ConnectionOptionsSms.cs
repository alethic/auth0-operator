using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsSms
{

    [JsonPropertyName("bruteForceProtection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BruteForceProtection { get; set; }

    [JsonPropertyName("disableSignup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableSignup { get; set; }

    [JsonPropertyName("forwardReqInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ForwardReqInfo { get; set; }

    [JsonPropertyName("from")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? From { get; set; }

    [JsonPropertyName("gatewayAuthentication")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionGatewayAuthenticationSms? GatewayAuthentication { get; set; }

    [JsonPropertyName("gatewayUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GatewayUrl { get; set; }

    [JsonPropertyName("messagingServiceSid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MessagingServiceSid { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("provider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionProviderEnumSms? Provider { get; set; }

    [JsonPropertyName("syntax")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionTemplateSyntaxEnumSms? Syntax { get; set; }

    [JsonPropertyName("template")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Template { get; set; }

    [JsonPropertyName("totp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionTotpSms? Totp { get; set; }

    [JsonPropertyName("twilioSid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TwilioSid { get; set; }

    [JsonPropertyName("twilioToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TwilioToken { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
