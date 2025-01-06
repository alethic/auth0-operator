﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Models.ResourceServer
{

    public class TokenEncryptionKey
    {

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("alg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Algorithm { get; set; }

        [JsonPropertyName("kid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Kid { get; set; }

        [JsonPropertyName("pem")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Pem { get; set; }

    }

}
