﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection
{

    public class ConnectionOptionsPasswordNoPersonalInfo
    {

        [JsonPropertyName("enable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Enable { get; set; }

    }

}
