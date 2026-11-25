using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Полиморфная сериализация IValueObject в легаси-формат <c>{"$type": "...", "value": {...}}</c>
    /// (AssemblyQualifiedName). Используется для обратной совместимости со старыми событиями
    /// (<c>ChangedValueObjectsJson</c>), записанными до появления IValueObjectSerializer.
    /// Concrete-типы должны быть десериализуемы System.Text.Json; VO с приватным конструктором
    /// + фабрикой требуют отдельного конвертера (см. DeserializeWithLegacy).
    /// </summary>
    public sealed class LegacyValueObjectJsonConverter : JsonConverter<IValueObject>
    {
        public override IValueObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("$type", out var typeElement))
            {
                throw new JsonException("Legacy VO envelope has no '$type' discriminator.");
            }

            var typeName = typeElement.GetString()
                ?? throw new JsonException("Legacy VO '$type' discriminator is empty.");

            var type = Type.GetType(typeName)
                ?? throw new InvalidOperationException($"VO type not found: {typeName}");

            if (!root.TryGetProperty("value", out var valueElement))
            {
                throw new JsonException("Legacy VO envelope has no 'value' property.");
            }

            return (IValueObject)JsonSerializer.Deserialize(valueElement.GetRawText(), type, options)!;
        }

        public override void Write(Utf8JsonWriter writer, IValueObject value, JsonSerializerOptions options)
        {
            var runtimeType = value.GetType();

            writer.WriteStartObject();
            writer.WriteString("$type", runtimeType.AssemblyQualifiedName);
            writer.WritePropertyName("value");
            JsonSerializer.Serialize(writer, value, runtimeType, options);
            writer.WriteEndObject();
        }
    }
}
