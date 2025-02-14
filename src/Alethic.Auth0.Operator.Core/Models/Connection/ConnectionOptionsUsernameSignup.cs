﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection
{

    public class ConnectionOptionsUsernameSignup
    {

        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConnectionOptionsAttributeStatus? Status { get; set; }

    }

}