using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>evernote</c> (and <c>evernote-sandbox</c>) social connection strategy.
    /// </summary>
    public record V1ConnectionEvernoteOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("evernote_sandbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EvernoteSandbox { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
