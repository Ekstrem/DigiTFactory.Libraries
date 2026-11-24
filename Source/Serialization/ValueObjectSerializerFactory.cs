using System;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Фабрика сериализаторов VO. Точка расширения: подставляй свой registry или формат.
    /// Для PostgreSQL JSONB и InMemory-шины по умолчанию — JsonEnvelope.
    /// </summary>
    public static class ValueObjectSerializerFactory
    {
        /// <summary>Создаёт сериализатор нужного формата.</summary>
        /// <param name="format">Формат сериализации.</param>
        /// <param name="registry">Реестр типов VO; если null — пустой реестр (разрешение по AssemblyQualifiedName).</param>
        public static IValueObjectSerializer Create(
            ValueObjectSerializationFormat format,
            IValueObjectTypeRegistry registry = null)
        {
            var typeRegistry = registry ?? new ValueObjectTypeRegistry();

            return format switch
            {
                ValueObjectSerializationFormat.JsonEnvelope => new JsonEnvelopeValueObjectSerializer(typeRegistry),
                ValueObjectSerializationFormat.Protobuf => new ProtobufValueObjectSerializer(typeRegistry),
                ValueObjectSerializationFormat.MessagePack => new MessagePackValueObjectSerializer(typeRegistry),
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unknown ValueObjectSerializationFormat")
            };
        }

        /// <summary>Дефолт: JSON-envelope (PostgreSQL JSONB, InMemory-шина).</summary>
        public static IValueObjectSerializer CreateDefault(IValueObjectTypeRegistry registry = null)
            => Create(ValueObjectSerializationFormat.JsonEnvelope, registry);
    }
}
