using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>twitter</c> social connection strategy.
    /// Each boolean property enables the corresponding Twitter permission.
    /// </summary>
    public record V1ConnectionTwitterOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("tweet_updates")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? TweetUpdates { get; set; }

        [JsonPropertyName("retweet_others_tweets")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RetweetOthersTweets { get; set; }

        [JsonPropertyName("like_tweets")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? LikeTweets { get; set; }

        [JsonPropertyName("create_or_delete_lists")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CreateOrDeleteLists { get; set; }

        [JsonPropertyName("write_dm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WriteDm { get; set; }

        [JsonPropertyName("follow_friends")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FollowFriends { get; set; }

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Email { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
