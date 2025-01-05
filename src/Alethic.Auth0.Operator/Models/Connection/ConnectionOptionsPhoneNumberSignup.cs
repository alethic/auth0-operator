﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Models.Connection
{

    public class ConnectionOptionsPhoneNumberSignup
    {

        [JsonPropertyName("status")]
        public ConnectionOptionsAttributeStatus? Status { get; set; }

        [JsonPropertyName("verification")]
        public ConnectionOptionsVerification? Verification { get; set; }

    }

}