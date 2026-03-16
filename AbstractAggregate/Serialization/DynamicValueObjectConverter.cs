using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;

namespace DigiTFactory.Libraries.AbstractAggregate.Serialization
{
    /// <summary>
    /// JSON-конвертер для DynamicValueObject.
    /// Сериализует/десериализует свойства VO в/из JSON.
    /// </summary>
    public sealed class DynamicValueObjectConverter : JsonConverter<DynamicValueObject>
    {
        public override DynamicValueObject Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            var typeName = root.TryGetProperty("TypeName", out var tn)
                ? tn.GetString() ?? "Unknown"
                : "Unknown";

            var properties = new Dictionary<string, object?>();

            if (root.TryGetProperty("Properties", out var propsElement) &&
                propsElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in propsElement.EnumerateObject())
                {
                    properties[prop.Name] = ReadJsonValue(prop.Value);
                }
            }

            return new DynamicValueObject(typeName, properties);
        }

        public override void Write(
            Utf8JsonWriter writer, DynamicValueObject value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("TypeName", value.TypeName);
            writer.WritePropertyName("Properties");
            writer.WriteStartObject();

            foreach (var (key, propValue) in value.Properties)
            {
                writer.WritePropertyName(key);
                JsonSerializer.Serialize(writer, propValue, options);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        private static object? ReadJsonValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => ReadArray(element),
                JsonValueKind.Object => ReadObject(element),
                _ => element.GetRawText()
            };
        }

        private static List<object?> ReadArray(JsonElement element)
        {
            var list = new List<object?>();
            foreach (var item in element.EnumerateArray())
                list.Add(ReadJsonValue(item));
            return list;
        }

        private static Dictionary<string, object?> ReadObject(JsonElement element)
        {
            var dict = new Dictionary<string, object?>();
            foreach (var prop in element.EnumerateObject())
                dict[prop.Name] = ReadJsonValue(prop.Value);
            return dict;
        }
    }
}
