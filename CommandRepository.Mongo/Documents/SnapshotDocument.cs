using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Documents
{
    /// <summary>
    /// Документ snapshot агрегата в MongoDB.
    /// </summary>
    public class SnapshotDocument
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        /// <summary>Идентификатор агрегата.</summary>
        public Guid Id { get; set; }

        /// <summary>Версия агрегата на момент создания snapshot.</summary>
        public long Version { get; set; }

        /// <summary>Сериализованный агрегат.</summary>
        public string AggregateJson { get; set; } = string.Empty;

        /// <summary>Дата создания snapshot (UTC).</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
