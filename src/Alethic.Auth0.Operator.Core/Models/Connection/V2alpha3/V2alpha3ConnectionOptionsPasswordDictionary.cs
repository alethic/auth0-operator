using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Controls whether Auth0 rejects passwords found in a common-password dictionary, optionally extended with custom entries.
    /// </summary>
    public record V2alpha3ConnectionOptionsPasswordDictionary
    {

        /// <summary>
        /// When <c>true</c>, passwords that appear in the common-password dictionary are rejected.
        /// </summary>
        [JsonPropertyName("enable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enable { get; set; }

        /// <summary>
        /// Additional custom words to reject as passwords, supplementing the built-in dictionary.
        /// </summary>
        [JsonPropertyName("dictionary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Dictionary { get; set; }

    }

}