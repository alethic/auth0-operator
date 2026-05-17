using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionFacebookOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Email { get; set; }

        [JsonPropertyName("birthday")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Birthday { get; set; }

        [JsonPropertyName("likes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Likes { get; set; }

        [JsonPropertyName("locale")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Locale { get; set; }

        [JsonPropertyName("timezone")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Timezone { get; set; }

        [JsonPropertyName("location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Location { get; set; }

        [JsonPropertyName("gender")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Gender { get; set; }

        [JsonPropertyName("age_range")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AgeRange { get; set; }

        [JsonPropertyName("updated_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UpdatedTime { get; set; }

        [JsonPropertyName("picture")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Picture { get; set; }

        [JsonPropertyName("publish_stream")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PublishStream { get; set; }

        [JsonPropertyName("manage_pages")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ManagePages { get; set; }

        [JsonPropertyName("manage_ads")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ManageAds { get; set; }

        [JsonPropertyName("ads_management")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdsManagement { get; set; }

        [JsonPropertyName("ads_read")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdsRead { get; set; }

        [JsonPropertyName("read_audience_network_insights")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadAudienceNetworkInsights { get; set; }

        [JsonPropertyName("read_insights")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadInsights { get; set; }

        [JsonPropertyName("read_page_mailboxes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadPageMailboxes { get; set; }

        [JsonPropertyName("pages_manage_ads")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PagesManageAds { get; set; }

        [JsonPropertyName("pages_manage_metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PagesManageMetadata { get; set; }

        [JsonPropertyName("pages_show_list")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PagesShowList { get; set; }

        [JsonPropertyName("pages_messaging")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PagesMessaging { get; set; }

        [JsonPropertyName("pages_messaging_phone_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PagesMessagingPhoneNumber { get; set; }

        [JsonPropertyName("pages_messaging_subscriptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PagesMessagingSubscriptions { get; set; }

        [JsonPropertyName("business_management")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BusinessManagement { get; set; }

        [JsonPropertyName("leads_retrieval")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? LeadsRetrieval { get; set; }

        [JsonPropertyName("instagram_basic")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? InstagramBasic { get; set; }

        [JsonPropertyName("instagram_manage_comments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? InstagramManageComments { get; set; }

        [JsonPropertyName("instagram_manage_insights")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? InstagramManageInsights { get; set; }

        [JsonPropertyName("instagram_content_publish")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? InstagramContentPublish { get; set; }

        [JsonPropertyName("groups_access_member_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? GroupsAccessMemberInfo { get; set; }

        [JsonPropertyName("publish_to_groups")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PublishToGroups { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
