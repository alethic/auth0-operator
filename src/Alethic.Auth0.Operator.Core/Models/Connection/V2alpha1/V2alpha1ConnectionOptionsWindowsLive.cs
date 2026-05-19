using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsWindowsLive
{

    [JsonPropertyName("client_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("freeform_scopes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? FreeformScopes { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("set_user_root_attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("strategy_version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? StrategyVersion { get; set; }

    [JsonPropertyName("upstream_params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha1ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("applications")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Applications { get; set; }

    [JsonPropertyName("applications_create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ApplicationsCreate { get; set; }

    [JsonPropertyName("basic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Basic { get; set; }

    [JsonPropertyName("birthday")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Birthday { get; set; }

    [JsonPropertyName("calendars")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Calendars { get; set; }

    [JsonPropertyName("calendars_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CalendarsUpdate { get; set; }

    [JsonPropertyName("contacts_birthday")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsBirthday { get; set; }

    [JsonPropertyName("contacts_calendars")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsCalendars { get; set; }

    [JsonPropertyName("contacts_create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsCreate { get; set; }

    [JsonPropertyName("contacts_photos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsPhotos { get; set; }

    [JsonPropertyName("contacts_skydrive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsSkydrive { get; set; }

    [JsonPropertyName("directory_accessasuser_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryAccessasuserAll { get; set; }

    [JsonPropertyName("directory_read_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryReadAll { get; set; }

    [JsonPropertyName("directory_readwrite_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryReadwriteAll { get; set; }

    [JsonPropertyName("emails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Emails { get; set; }

    [JsonPropertyName("events_create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EventsCreate { get; set; }

    [JsonPropertyName("graph_calendars")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphCalendars { get; set; }

    [JsonPropertyName("graph_calendars_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphCalendarsUpdate { get; set; }

    [JsonPropertyName("graph_contacts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphContacts { get; set; }

    [JsonPropertyName("graph_contacts_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphContactsUpdate { get; set; }

    [JsonPropertyName("graph_device")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphDevice { get; set; }

    [JsonPropertyName("graph_device_command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphDeviceCommand { get; set; }

    [JsonPropertyName("graph_emails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphEmails { get; set; }

    [JsonPropertyName("graph_emails_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphEmailsUpdate { get; set; }

    [JsonPropertyName("graph_files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFiles { get; set; }

    [JsonPropertyName("graph_files_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFilesAll { get; set; }

    [JsonPropertyName("graph_files_all_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFilesAllUpdate { get; set; }

    [JsonPropertyName("graph_files_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFilesUpdate { get; set; }

    [JsonPropertyName("graph_notes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphNotes { get; set; }

    [JsonPropertyName("graph_notes_create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphNotesCreate { get; set; }

    [JsonPropertyName("graph_notes_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphNotesUpdate { get; set; }

    [JsonPropertyName("graph_tasks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphTasks { get; set; }

    [JsonPropertyName("graph_tasks_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphTasksUpdate { get; set; }

    [JsonPropertyName("graph_user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphUser { get; set; }

    [JsonPropertyName("graph_user_activity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphUserActivity { get; set; }

    [JsonPropertyName("graph_user_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphUserUpdate { get; set; }

    [JsonPropertyName("group_read_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GroupReadAll { get; set; }

    [JsonPropertyName("group_readwrite_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GroupReadwriteAll { get; set; }

    [JsonPropertyName("mail_readwrite_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MailReadwriteAll { get; set; }

    [JsonPropertyName("mail_send")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MailSend { get; set; }

    [JsonPropertyName("messenger")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Messenger { get; set; }

    [JsonPropertyName("offline_access")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? OfflineAccess { get; set; }

    [JsonPropertyName("phone_numbers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PhoneNumbers { get; set; }

    [JsonPropertyName("photos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Photos { get; set; }

    [JsonPropertyName("postal_addresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PostalAddresses { get; set; }

    [JsonPropertyName("rolemanagement_read_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RolemanagementReadAll { get; set; }

    [JsonPropertyName("rolemanagement_readwrite_directory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RolemanagementReadwriteDirectory { get; set; }

    [JsonPropertyName("share")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Share { get; set; }

    [JsonPropertyName("signin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Signin { get; set; }

    [JsonPropertyName("sites_read_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SitesReadAll { get; set; }

    [JsonPropertyName("sites_readwrite_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SitesReadwriteAll { get; set; }

    [JsonPropertyName("skydrive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Skydrive { get; set; }

    [JsonPropertyName("skydrive_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SkydriveUpdate { get; set; }

    [JsonPropertyName("team_readbasic_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TeamReadbasicAll { get; set; }

    [JsonPropertyName("team_readwrite_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TeamReadwriteAll { get; set; }

    [JsonPropertyName("user_read_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UserReadAll { get; set; }

    [JsonPropertyName("user_readbasic_all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UserReadbasicAll { get; set; }

    [JsonPropertyName("work_profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? WorkProfile { get; set; }

    [JsonPropertyName("non_persistent_attrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
