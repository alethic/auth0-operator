﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Entities
{

    public partial class TenantDeviceFlow
    {

        [JsonPropertyName("charset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TenantCharset? Charset { get; set; }

        [JsonPropertyName("mask")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Mask { get; set; }

    }

}
