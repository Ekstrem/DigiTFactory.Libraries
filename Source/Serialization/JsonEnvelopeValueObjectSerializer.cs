using System.Collections.Generic;
using System.Text.Json;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Версионный JSON-envelope сериализатор VO. Формат: [{key, type, payload}],
    /// payload сериализуется по runtime-типу. Предпочтителен для PostgreSQL JSONB и InMemory-шины.
    /// </summary>
    public sealed class JsonEnvelopeValueObjectSerializer : IValueObjectSerializer
    {
        private readonly IValueObjectTypeRegistry _registry;
        private readonly JsonSerializerOptions _options;

        public JsonEnvelopeValueObjectSerializer(IValueObjectTypeRegistry registry)
            : this(registry, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
        {
        }

        public JsonEnvelopeValueObjectSerializer(IValueObjectTypeRegistry registry, JsonSerializerOptions options)
        {
            _registry = registry;
            _options = options;
        }

        /// <inheritdoc />
        public string Serialize(IReadOnlyDictionary<string, IValueObject> valueObjects)
        {
            var items = new List<VoEnvelope>(valueObjects.Count);
            foreach (var kvp in valueObjects)
            {
                var runtimeType = kvp.Value.GetType();
                items.Add(new VoEnvelope
                {
                    Key = kvp.Key,
                    Type = _registry.GetTypeName(runtimeType),
                    Payload = JsonSerializer.SerializeToElement(kvp.Value, runtimeType, _options)
                });
            }

            return JsonSerializer.Serialize(items, _options);
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IValueObject> Deserialize(string payload)
        {
            var items = JsonSerializer.Deserialize<List<VoEnvelope>>(payload, _options) ?? new List<VoEnvelope>();
            var result = new Dictionary<string, IValueObject>(items.Count);

            foreach (var item in items)
            {
                var type = _registry.ResolveType(item.Type);
                var value = item.Payload.Deserialize(type, _options) as IValueObject;
                if (value == null)
                {
                    throw new JsonException($"Value object '{item.Key}' did not deserialize as IValueObject.");
                }

                result[item.Key] = value;
            }

            return result;
        }

        private sealed class VoEnvelope
        {
            public string Key { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public JsonElement Payload { get; set; }
        }
    }
}
