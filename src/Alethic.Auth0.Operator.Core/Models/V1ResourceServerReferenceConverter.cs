using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models
{

    class V1ResourceServerReferenceConverter : JsonConverter<V1ResourceServerReference>
    {

        public override V1ResourceServerReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return new V1ResourceServerReference { Id = reader.GetString() };

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var j = JsonElement.ParseValue(ref reader);
                return new V1ResourceServerReference
                {
                    Namespace = j.TryGetProperty("namespace", out var ns) ? ns.GetString() : null,
                    Name = j.TryGetProperty("name", out var name) ? name.GetString() : null,
                    Id = j.TryGetProperty("id", out var id) ? id.GetString() : null,
                    Identifier = j.TryGetProperty("identifier", out var identifier) ? identifier.GetString() : null
                };
            }

            throw new JsonException($"Unexpected token parsing client reference. Expected String or StartObject, got {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, V1ResourceServerReference value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value.Namespace is not null)
                writer.WriteString("namespace", value.Namespace);

            if (value.Name is not null)
                writer.WriteString("name", value.Name);

            if (value.Id is not null)
                writer.WriteString("id", value.Id);

            if (value.Identifier is not null)
                writer.WriteString("identifier", value.Identifier);

            writer.WriteEndObject();
        }

    }

}
