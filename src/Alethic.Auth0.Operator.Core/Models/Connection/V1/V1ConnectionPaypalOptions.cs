using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>paypal</c> (and <c>paypal-sandbox</c>) social connection strategy.
    /// </summary>
    public record V1ConnectionPaypalOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("paypal_scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PaypalScope { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
