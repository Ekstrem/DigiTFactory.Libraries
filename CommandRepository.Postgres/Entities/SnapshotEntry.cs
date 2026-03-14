using System;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Entities
{
    /// <summary>
    /// Snapshot агрегата. Используется стратегией <see cref="Configuration.EventStoreStrategy.SnapshotAfterN"/>.
    /// Хранится в отдельной таблице Snapshots.
    /// </summary>
    public class SnapshotEntry
    {
        /// <summary>Идентификатор агрегата.</summary>
        public Guid Id { get; set; }

        /// <summary>Версия агрегата на момент создания snapshot.</summary>
        public long Version { get; set; }

        /// <summary>Сериализованный агрегат (JSONB в PostgreSQL).</summary>
        public string AggregateJson { get; set; } = string.Empty;

        /// <summary>Дата создания snapshot (UTC).</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
