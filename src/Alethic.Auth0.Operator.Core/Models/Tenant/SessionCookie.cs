﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Tenant
{

    public class SessionCookie
    {


        [JsonPropertyName("mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Mode { get; set; }

    }

}
