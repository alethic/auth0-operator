using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models
{

    /// <summary>
    /// Represents a reference to a client, which can be either a simple string (client ID) or an object containing namespace, name, and ID.
    /// </summary>
    class V1ClientReferenceConverter : JsonConverter<V1ClientReference>
    {

        public override V1ClientReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return new V1ClientReference { Id = reader.GetString() };

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var j = JsonElement.ParseValue(ref reader);
                return new V1ClientReference
                {
                    Namespace = j.TryGetProperty("namespace", out var ns) ? ns.GetString() : null,
                    Name = j.TryGetProperty("name", out var name) ? name.GetString() : null,
                    Id = j.TryGetProperty("id", out var id) ? id.GetString() : null
                };
            }

            throw new JsonException($"Unexpected token parsing client reference. Expected String or StartObject, got {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, V1ClientReference value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value.Namespace is not null)
                writer.WriteString("namespace", value.Namespace);

            if (value.Name is not null)
                writer.WriteString("name", value.Name);

            if (value.Id is not null)
                writer.WriteString("id", value.Id);

            writer.WriteEndObject();
        }

    }

}
