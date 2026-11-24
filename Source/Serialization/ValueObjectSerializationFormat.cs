namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>Поддерживаемые форматы сериализации VO.</summary>
    public enum ValueObjectSerializationFormat
    {
        /// <summary>Версионный JSON-envelope: предпочтителен для PostgreSQL JSONB и InMemory-шины.</summary>
        JsonEnvelope,

        /// <summary>Бинарный Protobuf-net: компактнее для шины/Kafka.</summary>
        Protobuf,

        /// <summary>Бинарный MessagePack: компромисс между JSON и Protobuf.</summary>
        MessagePack
    }
}
