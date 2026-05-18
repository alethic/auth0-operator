using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>google-oauth2</c> social connection strategy.
    /// Each boolean property enables the corresponding Google API OAuth scope.
    /// </summary>
    public record V2alpha1ConnectionGoogleOAuth2Options : V2alpha1ConnectionSocialOptions
    {

        [JsonPropertyName("email")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Email { get; set; }
        [JsonPropertyName("profile")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Profile { get; set; }
        [JsonPropertyName("offline_access")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? OfflineAccess { get; set; }
        [JsonPropertyName("allowed_audiences")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string[]? AllowedAudiences { get; set; }
        [JsonPropertyName("adsense_management")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? AdsenseManagement { get; set; }
        [JsonPropertyName("analytics")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Analytics { get; set; }
        [JsonPropertyName("blogger")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Blogger { get; set; }
        [JsonPropertyName("calendar")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Calendar { get; set; }
        [JsonPropertyName("calendar_addons_execute")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? CalendarAddonsExecute { get; set; }
        [JsonPropertyName("calendar_events")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? CalendarEvents { get; set; }
        [JsonPropertyName("calendar_events_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? CalendarEventsReadonly { get; set; }
        [JsonPropertyName("calendar_settings_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? CalendarSettingsReadonly { get; set; }
        [JsonPropertyName("chrome_web_store")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? ChromeWebStore { get; set; }
        [JsonPropertyName("contacts")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Contacts { get; set; }
        [JsonPropertyName("contacts_new")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? ContactsNew { get; set; }
        [JsonPropertyName("contacts_other_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? ContactsOtherReadonly { get; set; }
        [JsonPropertyName("contacts_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? ContactsReadonly { get; set; }
        [JsonPropertyName("content_api_for_shopping")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? ContentApiForShopping { get; set; }
        [JsonPropertyName("coordinate")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Coordinate { get; set; }
        [JsonPropertyName("coordinate_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? CoordinateReadonly { get; set; }
        [JsonPropertyName("directory_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DirectoryReadonly { get; set; }
        [JsonPropertyName("document_list")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DocumentList { get; set; }
        [JsonPropertyName("drive")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Drive { get; set; }
        [JsonPropertyName("drive_activity")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveActivity { get; set; }
        [JsonPropertyName("drive_activity_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveActivityReadonly { get; set; }
        [JsonPropertyName("drive_appdata")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveAppdata { get; set; }
        [JsonPropertyName("drive_apps_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveAppsReadonly { get; set; }
        [JsonPropertyName("drive_file")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveFile { get; set; }
        [JsonPropertyName("drive_metadata")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveMetadata { get; set; }
        [JsonPropertyName("drive_metadata_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveMetadataReadonly { get; set; }
        [JsonPropertyName("drive_photos_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DrivePhotosReadonly { get; set; }
        [JsonPropertyName("drive_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveReadonly { get; set; }
        [JsonPropertyName("drive_scripts")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? DriveScripts { get; set; }
        [JsonPropertyName("gmail")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Gmail { get; set; }
        [JsonPropertyName("gmail_compose")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailCompose { get; set; }
        [JsonPropertyName("gmail_insert")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailInsert { get; set; }
        [JsonPropertyName("gmail_labels")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailLabels { get; set; }
        [JsonPropertyName("gmail_metadata")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailMetadata { get; set; }
        [JsonPropertyName("gmail_modify")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailModify { get; set; }
        [JsonPropertyName("gmail_new")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailNew { get; set; }
        [JsonPropertyName("gmail_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailReadonly { get; set; }
        [JsonPropertyName("gmail_send")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailSend { get; set; }
        [JsonPropertyName("gmail_settings_basic")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailSettingsBasic { get; set; }
        [JsonPropertyName("gmail_settings_sharing")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GmailSettingsSharing { get; set; }
        [JsonPropertyName("google_affiliate_network")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GoogleAffiliateNetwork { get; set; }
        [JsonPropertyName("google_books")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GoogleBooks { get; set; }
        [JsonPropertyName("google_cloud_storage")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GoogleCloudStorage { get; set; }
        [JsonPropertyName("google_drive")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GoogleDrive { get; set; }
        [JsonPropertyName("google_drive_files")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GoogleDriveFiles { get; set; }
        [JsonPropertyName("google_plus")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? GooglePlus { get; set; }
        [JsonPropertyName("latitude_best")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? LatitudeBest { get; set; }
        [JsonPropertyName("latitude_city")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? LatitudeCity { get; set; }
        [JsonPropertyName("moderator")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Moderator { get; set; }
        [JsonPropertyName("orkut")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Orkut { get; set; }
        [JsonPropertyName("picasa_web")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? PicasaWeb { get; set; }
        [JsonPropertyName("sites")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Sites { get; set; }
        [JsonPropertyName("tasks")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Tasks { get; set; }
        [JsonPropertyName("tasks_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? TasksReadonly { get; set; }
        [JsonPropertyName("url_shortener")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? UrlShortener { get; set; }
        [JsonPropertyName("webmaster_tools")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? WebmasterTools { get; set; }
        [JsonPropertyName("youtube")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Youtube { get; set; }
        [JsonPropertyName("youtube_channelmemberships_creator")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? YoutubeChannelmembershipsCreator { get; set; }
        [JsonPropertyName("youtube_new")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? YoutubeNew { get; set; }
        [JsonPropertyName("youtube_readonly")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? YoutubeReadonly { get; set; }
        [JsonPropertyName("youtube_upload")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? YoutubeUpload { get; set; }
        [JsonPropertyName("youtubepartner")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? Youtubepartner { get; set; }
        [JsonPropertyName("icon_url")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? IconUrl { get; set; }

    }

}
