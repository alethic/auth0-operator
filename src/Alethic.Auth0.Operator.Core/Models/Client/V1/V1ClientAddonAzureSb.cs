using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Azure Storage Bus addon configuration.
/// </summary>
public record V1ClientAddonAzureSb
{

    /// <summary>
    /// Your Azure Service Bus namespace. Usually the first segment of your Service Bus URL (e.g. `https://acme-org.servicebus.windows.net` would be `acme-org`).
    /// </summary>
    [JsonPropertyName("namespace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Namespace { get; set; }

    /// <summary>
    /// Your shared access policy name defined in your Service Bus entity.
    /// </summary>
    [JsonPropertyName("sasKeyName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SasKeyName { get; set; }

    /// <summary>
    /// Primary Key associated with your shared access policy.
    /// </summary>
    [JsonPropertyName("sasKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SasKey { get; set; }

    /// <summary>
    /// Entity you want to request a token for. e.g. `my-queue`.'
    /// </summary>
    [JsonPropertyName("entityPath")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntityPath { get; set; }

}
