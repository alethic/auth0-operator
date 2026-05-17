using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>twitter</c> social connection strategy.
    /// Each boolean property enables the corresponding Twitter permission.
    /// </summary>
    public record V1ConnectionTwitterOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("protocol")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Protocol { get; set; }

        [JsonPropertyName("offline_access")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? OfflineAccess { get; set; }

        [JsonPropertyName("profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Profile { get; set; }

        [JsonPropertyName("tweet_read")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? TweetRead { get; set; }

        [JsonPropertyName("users_read")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UsersRead { get; set; }

    }

}
