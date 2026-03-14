using System;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Entities
{
    /// <summary>
    /// Запись доменного события в Event Store.
    /// Таблица DomainEvents — основное хранилище событий.
    /// </summary>
    public class DomainEventEntry
    {
        /// <summary>Идентификатор агрегата.</summary>
        public Guid Id { get; set; }

        /// <summary>Версия события (UTC timestamp в миллисекундах).</summary>
        public long Version { get; set; }

        /// <summary>Маркер корреляции для отслеживания цепочки команд.</summary>
        public Guid CorrelationToken { get; set; }

        /// <summary>Имя ограниченного контекста.</summary>
        public string BoundedContext { get; set; } = string.Empty;

        /// <summary>Имя команды, породившей событие.</summary>
        public string CommandName { get; set; } = string.Empty;

        /// <summary>Имя субъекта, выполнившего команду.</summary>
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>JSON с изменёнными Value Objects (JSONB в PostgreSQL).</summary>
        public string ChangedValueObjectsJson { get; set; } = string.Empty;

        /// <summary>Результат выполнения команды.</summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>Дата создания события (UTC).</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
