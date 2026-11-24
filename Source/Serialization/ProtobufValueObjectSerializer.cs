using System;
using System.Collections.Generic;
using System.IO;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using ProtoBuf;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Бинарный Protobuf-net-envelope сериализатор VO. Компактный формат для шины/Kafka.
    /// </summary>
    public sealed class ProtobufValueObjectSerializer : IValueObjectSerializer
    {
        private readonly IValueObjectTypeRegistry _registry;

        public ProtobufValueObjectSerializer(IValueObjectTypeRegistry registry)
        {
            _registry = registry;
        }

        /// <inheritdoc />
        public string Serialize(IReadOnlyDictionary<string, IValueObject> valueObjects)
        {
            var items = new List<PbEnvelope>(valueObjects.Count);
            foreach (var kvp in valueObjects)
            {
                var runtimeType = kvp.Value.GetType();
                items.Add(new PbEnvelope
                {
                    Key = kvp.Key,
                    SchemaVersion = 1,
                    Type = _registry.GetTypeName(runtimeType),
                    Payload = SerializePayload(kvp.Value)
                });
            }

            return Convert.ToBase64String(SerializePayload(items));
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IValueObject> Deserialize(string payload)
        {
            var items = DeserializePayload(typeof(List<PbEnvelope>), Convert.FromBase64String(payload)) as List<PbEnvelope>
                ?? new List<PbEnvelope>();

            var result = new Dictionary<string, IValueObject>(items.Count);
            foreach (var item in items)
            {
                var type = _registry.ResolveType(item.Type);
                result[item.Key] = (IValueObject)DeserializePayload(type, item.Payload);
            }

            return result;
        }

        private static byte[] SerializePayload(object value)
        {
            using var stream = new MemoryStream();
            Serializer.NonGeneric.Serialize(stream, value);
            return stream.ToArray();
        }

        private static object DeserializePayload(Type type, byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return Serializer.NonGeneric.Deserialize(type, stream);
        }

        [ProtoContract]
        private sealed class PbEnvelope
        {
            [ProtoMember(1)] public string Key { get; set; } = string.Empty;
            [ProtoMember(2)] public int SchemaVersion { get; set; }
            [ProtoMember(3)] public string Type { get; set; } = string.Empty;
            [ProtoMember(4)] public byte[] Payload { get; set; } = Array.Empty<byte>();
        }
    }
}
