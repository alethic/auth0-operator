﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Models.Connection
{

    public class ConnectionOptionsPasskeyAuthenticationMethod
    {

        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

    }

}