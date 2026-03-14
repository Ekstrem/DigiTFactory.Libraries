using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Documents
{
    /// <summary>
    /// Документ доменного события в MongoDB.
    /// </summary>
    public class DomainEventDocument
    {
        /// <summary>Внутренний идентификатор MongoDB.</summary>
        [BsonId]
        public ObjectId MongoId { get; set; }

        /// <summary>Идентификатор агрегата.</summary>
        public Guid Id { get; set; }

        /// <summary>Версия события (UTC timestamp в миллисекундах).</summary>
        public long Version { get; set; }

        /// <summary>Маркер корреляции.</summary>
        public Guid CorrelationToken { get; set; }

        /// <summary>Имя ограниченного контекста.</summary>
        public string BoundedContext { get; set; } = string.Empty;

        /// <summary>Имя команды, породившей событие.</summary>
        public string CommandName { get; set; } = string.Empty;

        /// <summary>Имя субъекта, выполнившего команду.</summary>
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>Изменённые Value Objects (BSON).</summary>
        public string ChangedValueObjectsJson { get; set; } = string.Empty;

        /// <summary>Результат выполнения команды.</summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>Дата создания события (UTC).</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
