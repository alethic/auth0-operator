﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    public class TenantErrorPage
    {

        [JsonPropertyName("html")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Html { get; set; }

        [JsonPropertyName("show_log_link")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowLogLink { get; set; }

        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }

    }

}
