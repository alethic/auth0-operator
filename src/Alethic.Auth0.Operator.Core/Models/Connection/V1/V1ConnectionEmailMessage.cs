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

}
