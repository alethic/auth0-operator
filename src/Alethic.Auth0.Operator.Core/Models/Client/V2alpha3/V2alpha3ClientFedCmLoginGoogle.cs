using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

/// <summary>
/// FedCM login settings for Google.
/// </summary>
public record V2alpha3ClientFedCmLoginGoogle
{

    [JsonPropertyName("isEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsEnabled { get; set; }

}
