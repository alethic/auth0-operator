using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

/// <summary>
/// Identity Assertion Authorization Grant (ID-JAG) configuration for the client.
/// </summary>
public record V2alpha3ClientIdentityAssertionAuthorizationGrant
{

    [JsonPropertyName("active")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

}
