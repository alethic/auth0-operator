using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>windowslive</c> social connection strategy.
    /// Each boolean property enables the corresponding Microsoft Live permission scope.
    /// </summary>
    public record V1ConnectionWindowsLiveOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("basic_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? BasicProfile { get; set; }

        [JsonPropertyName("offline_access")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? OfflineAccess { get; set; }

        [JsonPropertyName("signin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Signin { get; set; }

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

        [JsonPropertyName("contacts_create")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ContactsCreate { get; set; }

        [JsonPropertyName("contacts_calendar")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ContactsCalendar { get; set; }

        [JsonPropertyName("contacts_photos")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ContactsPhotos { get; set; }

        [JsonPropertyName("contacts_skydrive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ContactsSkydrive { get; set; }

        [JsonPropertyName("emails")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Emails { get; set; }

        [JsonPropertyName("events_create")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EventsCreate { get; set; }

        [JsonPropertyName("messenger")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Messenger { get; set; }

        [JsonPropertyName("phone_numbers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PhoneNumbers { get; set; }

        [JsonPropertyName("photos")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Photos { get; set; }

        [JsonPropertyName("postal_addresses")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PostalAddresses { get; set; }

        [JsonPropertyName("share")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Share { get; set; }

        [JsonPropertyName("skydrive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Skydrive { get; set; }

        [JsonPropertyName("skydrive_update")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SkydriveUpdate { get; set; }

        [JsonPropertyName("work_profile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WorkProfile { get; set; }

        [JsonPropertyName("applications")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Applications { get; set; }

        [JsonPropertyName("applications_create")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ApplicationsCreate { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
