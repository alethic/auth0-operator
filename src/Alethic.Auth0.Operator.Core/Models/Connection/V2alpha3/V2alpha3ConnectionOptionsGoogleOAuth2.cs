using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsGoogleOAuth2
{

    [JsonPropertyName("allowedAudiences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AllowedAudiences { get; set; }

    [JsonPropertyName("clientId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("freeformScopes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? FreeformScopes { get; set; }

    [JsonPropertyName("iconUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("setUserRootAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("upstreamParams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("adsenseManagement")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AdsenseManagement { get; set; }

    [JsonPropertyName("analytics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Analytics { get; set; }

    [JsonPropertyName("blogger")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Blogger { get; set; }

    [JsonPropertyName("calendar")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Calendar { get; set; }

    [JsonPropertyName("calendarAddonsExecute")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CalendarAddonsExecute { get; set; }

    [JsonPropertyName("calendarEvents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CalendarEvents { get; set; }

    [JsonPropertyName("calendarEventsReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CalendarEventsReadonly { get; set; }

    [JsonPropertyName("calendarSettingsReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CalendarSettingsReadonly { get; set; }

    [JsonPropertyName("chromeWebStore")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ChromeWebStore { get; set; }

    [JsonPropertyName("contacts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Contacts { get; set; }

    [JsonPropertyName("contactsNew")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsNew { get; set; }

    [JsonPropertyName("contactsOtherReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsOtherReadonly { get; set; }

    [JsonPropertyName("contactsReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsReadonly { get; set; }

    [JsonPropertyName("contentApiForShopping")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContentApiForShopping { get; set; }

    [JsonPropertyName("coordinate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Coordinate { get; set; }

    [JsonPropertyName("coordinateReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CoordinateReadonly { get; set; }

    [JsonPropertyName("directoryReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryReadonly { get; set; }

    [JsonPropertyName("documentList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DocumentList { get; set; }

    [JsonPropertyName("drive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Drive { get; set; }

    [JsonPropertyName("driveActivity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveActivity { get; set; }

    [JsonPropertyName("driveActivityReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveActivityReadonly { get; set; }

    [JsonPropertyName("driveAppdata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveAppdata { get; set; }

    [JsonPropertyName("driveAppsReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveAppsReadonly { get; set; }

    [JsonPropertyName("driveFile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveFile { get; set; }

    [JsonPropertyName("driveMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveMetadata { get; set; }

    [JsonPropertyName("driveMetadataReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveMetadataReadonly { get; set; }

    [JsonPropertyName("drivePhotosReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DrivePhotosReadonly { get; set; }

    [JsonPropertyName("driveReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveReadonly { get; set; }

    [JsonPropertyName("driveScripts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DriveScripts { get; set; }

    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Email { get; set; }

    [JsonPropertyName("gmail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Gmail { get; set; }

    [JsonPropertyName("gmailCompose")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailCompose { get; set; }

    [JsonPropertyName("gmailInsert")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailInsert { get; set; }

    [JsonPropertyName("gmailLabels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailLabels { get; set; }

    [JsonPropertyName("gmailMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailMetadata { get; set; }

    [JsonPropertyName("gmailModify")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailModify { get; set; }

    [JsonPropertyName("gmailNew")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailNew { get; set; }

    [JsonPropertyName("gmailReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailReadonly { get; set; }

    [JsonPropertyName("gmailSend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailSend { get; set; }

    [JsonPropertyName("gmailSettingsBasic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailSettingsBasic { get; set; }

    [JsonPropertyName("gmailSettingsSharing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GmailSettingsSharing { get; set; }

    [JsonPropertyName("googleAffiliateNetwork")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GoogleAffiliateNetwork { get; set; }

    [JsonPropertyName("googleBooks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GoogleBooks { get; set; }

    [JsonPropertyName("googleCloudStorage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GoogleCloudStorage { get; set; }

    [JsonPropertyName("googleDrive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GoogleDrive { get; set; }

    [JsonPropertyName("googleDriveFiles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GoogleDriveFiles { get; set; }

    [JsonPropertyName("googlePlus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GooglePlus { get; set; }

    [JsonPropertyName("latitudeBest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LatitudeBest { get; set; }

    [JsonPropertyName("latitudeCity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LatitudeCity { get; set; }

    [JsonPropertyName("moderator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Moderator { get; set; }

    [JsonPropertyName("offlineAccess")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? OfflineAccess { get; set; }

    [JsonPropertyName("orkut")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Orkut { get; set; }

    [JsonPropertyName("picasaWeb")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PicasaWeb { get; set; }

    [JsonPropertyName("profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Profile { get; set; }

    [JsonPropertyName("sites")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Sites { get; set; }

    [JsonPropertyName("tasks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Tasks { get; set; }

    [JsonPropertyName("tasksReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TasksReadonly { get; set; }

    [JsonPropertyName("urlShortener")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UrlShortener { get; set; }

    [JsonPropertyName("webmasterTools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? WebmasterTools { get; set; }

    [JsonPropertyName("youtube")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Youtube { get; set; }

    [JsonPropertyName("youtubeChannelmembershipsCreator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? YoutubeChannelmembershipsCreator { get; set; }

    [JsonPropertyName("youtubeNew")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? YoutubeNew { get; set; }

    [JsonPropertyName("youtubeReadonly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? YoutubeReadonly { get; set; }

    [JsonPropertyName("youtubeUpload")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? YoutubeUpload { get; set; }

    [JsonPropertyName("youtubepartner")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Youtubepartner { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
