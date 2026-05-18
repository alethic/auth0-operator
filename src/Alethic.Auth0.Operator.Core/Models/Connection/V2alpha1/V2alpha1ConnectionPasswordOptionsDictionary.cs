using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    public record V2alpha1ConnectionPasswordOptionsDictionary
    {


        /// <summary>
        /// Enables dictionary checking to prevent use of common passwords and custom blocked words
        /// </summary>
        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

        /// <summary>
        /// Array of custom words to block in passwords. Maximum 200 items, each up to 50 characters
        /// </summary>
        [JsonPropertyName("custom")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Custom { get; set; }

        [JsonPropertyName("default")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha1ConnectionPasswordDefaultDictionaries? Default { get; set; }

    }

}