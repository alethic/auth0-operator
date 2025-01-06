﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Entities
{

    public class V1ClientRef
    {

        [JsonPropertyName("namespace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Namespace { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Id is not null)
                return Id;
            else
                return $"{Namespace}/{Name}";
        }

    }

}
