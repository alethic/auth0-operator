using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>yahoo</c> social connection strategy.
    /// Each boolean property enables the corresponding Yahoo API permission.
    /// </summary>
    public record V2alpha1ConnectionYahooOptions : V2alpha1ConnectionSocialOptions
    {

        [JsonPropertyName("mail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Mail { get; set; }

        [JsonPropertyName("calendar")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Calendar { get; set; }

        [JsonPropertyName("contacts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Contacts { get; set; }

        [JsonPropertyName("profiles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Profiles { get; set; }

        [JsonPropertyName("messenger")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Messenger { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
