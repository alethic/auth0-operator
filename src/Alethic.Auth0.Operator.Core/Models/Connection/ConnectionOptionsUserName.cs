﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection
{

    public class ConnectionOptionsUserName
    {

        [JsonPropertyName("min")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Min { get; set; }

        [JsonPropertyName("max")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Max { get; set; }

    }

}