using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>salesforce</c> (and <c>salesforce-sandbox</c>) social connection strategy.
    /// </summary>
    public record V2alpha1ConnectionSalesforceOptions : V2alpha1ConnectionSocialOptions
    {

        [JsonPropertyName("profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Profile { get; set; }

    }

}
