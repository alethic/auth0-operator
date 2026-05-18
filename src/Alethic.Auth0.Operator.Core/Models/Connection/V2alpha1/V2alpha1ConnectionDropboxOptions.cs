using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>dropbox</c> social connection strategy.
    /// </summary>
    public record V2alpha1ConnectionDropboxOptions : V2alpha1ConnectionSocialOptions
    {

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
