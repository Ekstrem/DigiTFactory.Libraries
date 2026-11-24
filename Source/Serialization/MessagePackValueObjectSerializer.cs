using System;
using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using MessagePack;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Бинарный MessagePack-envelope сериализатор VO. Полезен для шины/Kafka, где важен размер.
    /// </summary>
    public sealed class MessagePackValueObjectSerializer : IValueObjectSerializer
    {
        private readonly IValueObjectTypeRegistry _registry;
        private readonly MessagePackSerializerOptions _options;

        public MessagePackValueObjectSerializer(IValueObjectTypeRegistry registry)
            : this(registry, MessagePackSerializerOptions.Standard)
        {
        }

        public MessagePackValueObjectSerializer(IValueObjectTypeRegistry registry, MessagePackSerializerOptions options)
        {
            _registry = registry;
            _options = options;
        }

        /// <inheritdoc />
        public string Serialize(IReadOnlyDictionary<string, IValueObject> valueObjects)
        {
            var items = new List<MpEnvelope>(valueObjects.Count);
            foreach (var kvp in valueObjects)
            {
                var runtimeType = kvp.Value.GetType();
                items.Add(new MpEnvelope
                {
                    SchemaVersion = 1,
                    Type = _registry.GetTypeName(runtimeType),
                    Payload = MessagePackSerializer.Serialize(runtimeType, kvp.Value, _options)
                });
            }

            return Convert.ToBase64String(MessagePackSerializer.Serialize(items, _options));
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IValueObject> Deserialize(string payload)
        {
            var items = MessagePackSerializer.Deserialize<List<MpEnvelope>>(Convert.FromBase64String(payload), _options);
            var result = new Dictionary<string, IValueObject>(items.Count);

            foreach (var item in items)
            {
                var type = _registry.ResolveType(item.Type);
                result[item.Key] = (IValueObject)MessagePackSerializer.Deserialize(type, item.Payload, _options);
            }

            return result;
        }

        [MessagePackObject]
        private sealed class MpEnvelope
        {
            [Key(0)] public string Key { get; set; } = string.Empty;
            [Key(1)] public int SchemaVersion { get; set; }
            [Key(2)] public string Type { get; set; } = string.Empty;
            [Key(3)] public byte[] Payload { get; set; } = Array.Empty<byte>();
        }
    }
}
