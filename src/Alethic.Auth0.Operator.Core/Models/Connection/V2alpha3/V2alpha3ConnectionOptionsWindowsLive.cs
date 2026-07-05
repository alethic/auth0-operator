using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsWindowsLive
{

    [JsonPropertyName("clientId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("freeformScopes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? FreeformScopes { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("setUserRootAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("strategyVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? StrategyVersion { get; set; }

    [JsonPropertyName("upstreamParams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("applications")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Applications { get; set; }

    [JsonPropertyName("applicationsCreate")]
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

    [JsonPropertyName("calendarsUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CalendarsUpdate { get; set; }

    [JsonPropertyName("contactsBirthday")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsBirthday { get; set; }

    [JsonPropertyName("contactsCalendars")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsCalendars { get; set; }

    [JsonPropertyName("contactsCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsCreate { get; set; }

    [JsonPropertyName("contactsPhotos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsPhotos { get; set; }

    [JsonPropertyName("contactsSkydrive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContactsSkydrive { get; set; }

    [JsonPropertyName("directoryAccessasuserAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryAccessasuserAll { get; set; }

    [JsonPropertyName("directoryReadAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryReadAll { get; set; }

    [JsonPropertyName("directoryReadwriteAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DirectoryReadwriteAll { get; set; }

    [JsonPropertyName("emails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Emails { get; set; }

    [JsonPropertyName("eventsCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EventsCreate { get; set; }

    [JsonPropertyName("graphCalendars")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphCalendars { get; set; }

    [JsonPropertyName("graphCalendarsUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphCalendarsUpdate { get; set; }

    [JsonPropertyName("graphContacts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphContacts { get; set; }

    [JsonPropertyName("graphContactsUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphContactsUpdate { get; set; }

    [JsonPropertyName("graphDevice")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphDevice { get; set; }

    [JsonPropertyName("graphDeviceCommand")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphDeviceCommand { get; set; }

    [JsonPropertyName("graphEmails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphEmails { get; set; }

    [JsonPropertyName("graphEmailsUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphEmailsUpdate { get; set; }

    [JsonPropertyName("graphFiles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFiles { get; set; }

    [JsonPropertyName("graphFilesAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFilesAll { get; set; }

    [JsonPropertyName("graphFilesAllUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFilesAllUpdate { get; set; }

    [JsonPropertyName("graphFilesUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphFilesUpdate { get; set; }

    [JsonPropertyName("graphNotes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphNotes { get; set; }

    [JsonPropertyName("graphNotesCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphNotesCreate { get; set; }

    [JsonPropertyName("graphNotesUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphNotesUpdate { get; set; }

    [JsonPropertyName("graphTasks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphTasks { get; set; }

    [JsonPropertyName("graphTasksUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphTasksUpdate { get; set; }

    [JsonPropertyName("graphUser")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphUser { get; set; }

    [JsonPropertyName("graphUserActivity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphUserActivity { get; set; }

    [JsonPropertyName("graphUserUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GraphUserUpdate { get; set; }

    [JsonPropertyName("groupReadAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GroupReadAll { get; set; }

    [JsonPropertyName("groupReadwriteAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GroupReadwriteAll { get; set; }

    [JsonPropertyName("mailReadwriteAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MailReadwriteAll { get; set; }

    [JsonPropertyName("mailSend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MailSend { get; set; }

    [JsonPropertyName("messenger")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Messenger { get; set; }

    [JsonPropertyName("offlineAccess")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? OfflineAccess { get; set; }

    [JsonPropertyName("phoneNumbers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PhoneNumbers { get; set; }

    [JsonPropertyName("photos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Photos { get; set; }

    [JsonPropertyName("postalAddresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PostalAddresses { get; set; }

    [JsonPropertyName("rolemanagementReadAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RolemanagementReadAll { get; set; }

    [JsonPropertyName("rolemanagementReadwriteDirectory")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RolemanagementReadwriteDirectory { get; set; }

    [JsonPropertyName("share")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Share { get; set; }

    [JsonPropertyName("signin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Signin { get; set; }

    [JsonPropertyName("sitesReadAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SitesReadAll { get; set; }

    [JsonPropertyName("sitesReadwriteAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SitesReadwriteAll { get; set; }

    [JsonPropertyName("skydrive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Skydrive { get; set; }

    [JsonPropertyName("skydriveUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SkydriveUpdate { get; set; }

    [JsonPropertyName("teamReadbasicAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TeamReadbasicAll { get; set; }

    [JsonPropertyName("teamReadwriteAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TeamReadwriteAll { get; set; }

    [JsonPropertyName("userReadAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UserReadAll { get; set; }

    [JsonPropertyName("userReadbasicAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UserReadbasicAll { get; set; }

    [JsonPropertyName("workProfile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? WorkProfile { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
