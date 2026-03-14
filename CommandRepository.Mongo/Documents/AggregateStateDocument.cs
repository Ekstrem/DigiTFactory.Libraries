using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Documents
{
    /// <summary>
    /// Документ текущего состояния агрегата в MongoDB (стратегия StateOnly).
    /// </summary>
    public class AggregateStateDocument
    {
        /// <summary>Идентификатор агрегата (используется как _id).</summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        /// <summary>Текущая версия агрегата.</summary>
        public long Version { get; set; }

        /// <summary>Сериализованный агрегат.</summary>
        public string AggregateJson { get; set; } = string.Empty;

        /// <summary>Дата последнего обновления (UTC).</summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
